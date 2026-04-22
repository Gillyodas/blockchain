using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Azure;
using ChainDegree.Application.External.ChainDegreeBlockchainService;
using ChainDegree.Application.External.ChainDegreeBlockchainService.Services;
using ChainDegree.SharedKernel.External.ChainDegreeBlockchainService;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChainDegree.Infrastructure.External.ChainDegreeBlockchainService.Services;

/// <summary>
/// Tương tác với Besu blockchain node
/// 
/// Tại sao separate service:
/// - Docker operations (restart container)
/// - HTTP RPC calls (verify validators)
/// - RLP encoding (shell command)
/// 
/// Tất cả blockchain-specific logic đều ở đây
/// </summary>
public class BesuService : IBesuService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly DockerClient _dockerClient;
    private readonly ILogger<BesuService> _logger;
    private readonly string _configPath;
    private readonly string _rpcUrl;
    private readonly string _containerName;
    private readonly int _rpcTimeoutMs;

    public BesuService(
            IConfiguration configuration,
            HttpClient httpClient,
            ILogger<BesuService> logger)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;

        _configPath = configuration["Blockchain:Besu:ConfigPath"] ?? "./besu/config";
        _rpcUrl = configuration["Blockchain:Besu:RpcUrl"] ?? "http://localhost:8545";
        _containerName = configuration["Blockchain:Besu:ContainerName"] ?? "chaindegree-besu";
        _rpcTimeoutMs = int.Parse(configuration["Blockchain:Besu:RpcTimeoutMs"] ?? "30000");

        var dockerSocketUrl = GetDockerSocketUrl();
        _dockerClient = new DockerClientConfiguration(
            new Uri(dockerSocketUrl)
        ).CreateClient();

        var uri = new Uri(dockerSocketUrl);
        _dockerClient = new DockerClientConfiguration(uri).CreateClient();
    }

    public async Task<string> EncodeValidatorsToExtraDataAsync(
        string toEncodePath,
        CancellationToken ct = default)
    {
        CreateContainerResponse? response = null;

        try
        {
            _logger.LogInformation("Starting RLP encoding validators...");
            _logger.LogInformation("toEncode.json path: {Path}", toEncodePath);

            if (!File.Exists(toEncodePath))
            {
                throw new FileNotFoundException(
                    $"toEncode.json not found at {toEncodePath}");
            }

            var configDirectory = Path.GetDirectoryName(toEncodePath);
            if (string.IsNullOrEmpty(configDirectory))
            {
                throw new InvalidOperationException(
                    "Cannot determine config directory from toEncode.json path");
            }

            _logger.LogInformation("Config directory: {Directory}", configDirectory);

            var containerConfig = new CreateContainerParameters
            {
                Image = "hyperledger/besu:latest",

                Cmd = new List<string>
                {
                    "rlp", "encode",
                    "--from=/config/toEncode.json",
                    "--type=IBFT_EXTRA_DATA"
                },

                HostConfig = new HostConfig
                {
                    Binds = new List<string>
                    {
                        $"{Path.GetFullPath(configDirectory)}:/config:ro"
                    },
                }
            };

            _logger.LogInformation("Creating Docker container for RLP encoding");

            var info = await _dockerClient.System.GetSystemInfoAsync(ct);
            _logger.LogDebug("Docker connected. Containers: {Count}", info.Containers);

            response = await _dockerClient.Containers
                .CreateContainerAsync(containerConfig, ct);

            _logger.LogInformation("Container created: {Id}", response.ID[..12]);

            var started = await _dockerClient.Containers
                .StartContainerAsync(response.ID, new ContainerStartParameters(), ct);

            if (!started)
            {
                throw new InvalidOperationException("Failed to start Docker container");
            }

            _logger.LogInformation("Container started");

            var waitResponse = await _dockerClient.Containers
                .WaitContainerAsync(response.ID, ct);

            _logger.LogInformation("Container exited with code: {Code}", waitResponse.StatusCode);

            var logsStream = await _dockerClient.Containers
                .GetContainerLogsAsync(
                    response.ID,
                    new ContainerLogsParameters
                    {
                        ShowStdout = true,
                        ShowStderr = true
                    },
                    ct);

            string encodedResult = await ParseEncodedOutputAsync(logsStream);

            _logger.LogInformation(
                "RLP encoding completed. ExtraData: {ExtraData}",
                encodedResult[..50] + "...");

            return encodedResult;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("RLP encoding cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error encoding validators to RLP");
            throw new BesuServiceException(
                $"Failed to encode validators: {ex.Message}",
                ex);
        }
        finally
        {
            if (response != null)
            {
                try
                {
                    await _dockerClient.Containers.RemoveContainerAsync(
                        response.ID,
                        new ContainerRemoveParameters { Force = true },
                        ct);

                    var idShort = response.ID != null
                        ? response.ID.Substring(0, Math.Min(12, response.ID.Length))
                        : "<unknown>";

                    _logger.LogDebug("Container cleaned up: {Id}", idShort);
                }
                catch (Exception ex)
                {
                    var idShort = response?.ID != null
                        ? response.ID.Substring(0, Math.Min(12, response.ID.Length))
                        : "<unknown>";

                    _logger.LogWarning(ex, "Failed to cleanup container: {Id}", idShort);
                    // Don't throw - cleanup failure shouldn't fail the whole operation
                }
            }
        }
    }

    public async Task<string> EncodeValidatorsToExtraDataAsync(
        List<string> validators,
        CancellationToken ct = default)
    {
        CreateContainerResponse? response = null;
        string toEncodePath = null;

        try
        {
            if (validators == null || validators.Count == 0)
            {
                throw new ArgumentException("Validators list cannot be empty", nameof(validators));
            }

            _logger.LogInformation("Starting RLP encoding for {Count} validators (direct)...", validators.Count);

            // Create temp file directly in config directory (so Docker can see it via mount)
            var tempFilename = $"toEncode_{Guid.NewGuid()}.json";
            toEncodePath = Path.Combine(_configPath, tempFilename);

            // Create the validators object
            // var toEncodeObject = new { validators = validators };

            // Write to file in config directory
            var json = JsonSerializer.Serialize(
                validators,
                new JsonSerializerOptions { WriteIndented = true }
);

            await File.WriteAllTextAsync(toEncodePath, json, ct);

            _logger.LogInformation("Temp toEncode.json created at: {Path}", toEncodePath);
            _logger.LogDebug("Validators JSON: {Json}", json);

            var configDirectory = Path.GetDirectoryName(toEncodePath);
            if (string.IsNullOrEmpty(configDirectory))
            {
                throw new InvalidOperationException("Cannot determine config directory");
            }

            var containerConfig = new CreateContainerParameters
            {
                Image = "hyperledger/besu:latest",

                Cmd = new List<string>
                {
                    "rlp", "encode",
                    $"--from=/config/{tempFilename}",
                    "--type=IBFT_EXTRA_DATA"
                },

                HostConfig = new HostConfig
                {
                    Binds = new List<string>
                    {
                        $"{Path.GetFullPath(configDirectory)}:/config:ro"
                    },
                }
            };

            _logger.LogInformation("Creating Docker container for RLP encoding");

            var info = await _dockerClient.System.GetSystemInfoAsync(ct);
            _logger.LogDebug("Docker connected. Containers: {Count}", info.Containers);

            response = await _dockerClient.Containers
                .CreateContainerAsync(containerConfig, ct);

            _logger.LogInformation("Container created: {Id}", response.ID[..12]);

            var started = await _dockerClient.Containers
                .StartContainerAsync(response.ID, new ContainerStartParameters(), ct);

            if (!started)
            {
                throw new InvalidOperationException("Failed to start Docker container");
            }

            _logger.LogInformation("Container started");

            var waitResponse = await _dockerClient.Containers
                .WaitContainerAsync(response.ID, ct);

            _logger.LogInformation("Container exited with code: {Code}", waitResponse.StatusCode);

            var logsStream = await _dockerClient.Containers
                .GetContainerLogsAsync(
                    response.ID,
                    new ContainerLogsParameters
                    {
                        ShowStdout = true,
                        ShowStderr = true
                    },
                    ct);

            string encodedResult = await ParseEncodedOutputAsync(logsStream);

            _logger.LogInformation(
                "RLP encoding completed. ExtraData: {ExtraData}",
                encodedResult[..Math.Min(50, encodedResult.Length)] + "...");

            return encodedResult;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("RLP encoding cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error encoding validators to RLP");
            throw new BesuServiceException(
                $"Failed to encode validators: {ex.Message}",
                ex);
        }
        finally
        {
            if (response != null)
            {
                try
                {
                    await _dockerClient.Containers.RemoveContainerAsync(
                        response.ID,
                        new ContainerRemoveParameters { Force = true },
                        ct);

                    var idShort = response.ID != null
                        ? response.ID.Substring(0, Math.Min(12, response.ID.Length))
                        : "<unknown>";

                    _logger.LogDebug("Container cleaned up: {Id}", idShort);
                }
                catch (Exception ex)
                {
                    var idShort = response?.ID != null
                        ? response.ID.Substring(0, Math.Min(12, response.ID.Length))
                        : "<unknown>";

                    _logger.LogWarning(ex, "Failed to cleanup container: {Id}", idShort);
                }
            }

            // Clean up temp file
            if (!string.IsNullOrEmpty(toEncodePath))
            {
                try
                {
                    File.Delete(toEncodePath);
                    _logger.LogDebug("Temp toEncode.json deleted: {Path}", toEncodePath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete temp file: {Path}", toEncodePath);
                }
            }
        }
    }

    public async Task RestartBesuContainerAsync(CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation(
                "Starting Besu restart process (container: {Name})",
                _containerName);

            // Find container by name
            var containers = await _dockerClient.Containers
                .ListContainersAsync(
                    new ContainersListParameters { All = true },
                    ct);

            var besuContainer = containers.FirstOrDefault(c =>
                c.Names.Any(n => n.Contains(_containerName, StringComparison.OrdinalIgnoreCase)))
                ?? throw new InvalidOperationException(
                    $"Besu container '{_containerName}' not found");

            var containerId = besuContainer.ID;
            _logger.LogInformation("Found container: {Id}", containerId[..12]);

            // Stop container
            _logger.LogInformation("Stopping Besu container...");

            await _dockerClient.Containers
                .StopContainerAsync(
                    containerId,
                    new ContainerStopParameters
                    {
                        // Give container 10 seconds to stop gracefully
                        WaitBeforeKillSeconds = 10
                    },
                    ct);

            _logger.LogInformation("Besu stopped");

            // Wait a bit to ensure full stop
            // Docker sometimes needs time between stop and start
            await Task.Delay(2000, ct);

            // Clear Besu database to allow fresh initialization with new genesis
            // Tại sao cần clear: Khi genesis.json có extraData khác, 
            // database cũ sẽ conflict → Besu weigert start
            _logger.LogInformation("Clearing Besu database for fresh initialization...");
            await ClearBesuDataDirectoryAsync(containerId, ct);

            // Step 4: Start container
            _logger.LogInformation("Starting Besu container...");
            await _dockerClient.Containers
                .StartContainerAsync(
                    containerId,
                    new ContainerStartParameters(),
                    ct);

            _logger.LogInformation("Besu started");

            // Wait for Besu to fully startup
            // Tại sao delay: Besu needs time để initialize blockchain, read genesis, etc.
            // Khi genesis.json có extraData mới, Besu cần thời gian để validate & load
            _logger.LogInformation("Waiting for Besu startup (10000ms)...", 10000);
            await Task.Delay(10000, ct);

            // Verify Besu is healthy
            _logger.LogInformation("Checking Besu health...");

            var isHealthy = false;
            for (int i = 0; i < 20; i++)
            {
                isHealthy = await IsHealthyAsync(ct);
                if (isHealthy)
                {
                    _logger.LogInformation("Besu health check passed");
                    break;
                }

                _logger.LogInformation(
                    "Health check attempt {Attempt}/20 failed, retrying...",
                    i + 1);

                await Task.Delay(2000, ct);
            }

            if (!isHealthy)
            {
                throw new InvalidOperationException(
                    "Besu failed to start after restart - health check failed after 20 attempts");
            }

            _logger.LogInformation("Besu restart completed successfully");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Besu restart cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restarting Besu");
            throw new BesuServiceException(
                $"Failed to restart Besu: {ex.Message}",
                ex);
        }
    }

    public async Task<bool> VerifyValidatorsAsync(
        List<string> expectedAddresses,
        CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation(
                "Verifying {Count} validators on blockchain",
                expectedAddresses.Count);

            // Query blockchain for current validators. IBFT 2.0 consensus provides this RPC method
            var payload = new
            {
                jsonrpc = "2.0",
                method = "ibft_getValidatorsByBlockNumber",
                @params = new object[] { "0x0" },  // Block 0 = Genesis block
                id = 1
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json");

            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromMilliseconds(_rpcTimeoutMs));

            _logger.LogInformation("Calling Besu RPC: {Url}", _rpcUrl);

            var response = await _httpClient.PostAsync(
                _rpcUrl,
                content,
                cts.Token);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Besu RPC returned status {Status}: {Content}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(ct));

                return false;
            }

            var responseBody = await response.Content.ReadAsStringAsync(ct);

            // Parse response
            using var doc = JsonDocument.Parse(responseBody);
            var resultArray = doc.RootElement
                .GetProperty("result")
                .EnumerateArray();

            // Extract validator addresses từ response
            var onChainValidators = new List<string>();
            foreach (var validator in resultArray)
            {
                var address = validator.GetString();
                if (!string.IsNullOrEmpty(address))
                {
                    onChainValidators.Add(address.ToLower());
                }
            }

            _logger.LogInformation(
                "Found {Count} validators on-chain",
                onChainValidators.Count);

            // Compare expected vs on-chain. Normalize (lowercase + sort) để compare
            var expected = expectedAddresses
                .Select(a => a.ToLower())
                .OrderBy(a => a)
                .ToList();

            var onChain = onChainValidators
                .OrderBy(a => a)
                .ToList();

            // Log differences
            if (expected.Count != onChain.Count)
            {
                _logger.LogWarning(
                    "Validator count mismatch: expected {Expected}, found {OnChain}",
                    expected.Count,
                    onChain.Count);

                return false;
            }

            var matches = expected.SequenceEqual(onChain);

            if (!matches)
            {
                _logger.LogWarning("Validator addresses don't match");

                // Log missing & extra validators for debugging
                var missing = expected.Except(onChain);
                var extra = onChain.Except(expected);

                if (missing.Any())
                {
                    _logger.LogWarning(
                        "   Missing validators: {Validators}",
                        string.Join(", ", missing));
                }

                if (extra.Any())
                {
                    _logger.LogWarning(
                        "   Extra validators: {Validators}",
                        string.Join(", ", extra));
                }
            }
            else
            {
                _logger.LogInformation("All validators verified on-chain");
            }

            return matches;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Validator verification cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying validators");
            throw new BesuServiceException(
                $"Failed to verify validators: {ex.Message}",
                ex);
        }
    }

    public async Task<bool> IsHealthyAsync(CancellationToken ct = default)
    {
        try
        {
            var blockNumber = await GetBlockNumberAsync(ct);
            return blockNumber != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> GetBlockNumberAsync(CancellationToken ct = default)
    {
        try
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "eth_blockNumber",
                @params = Array.Empty<object>(),
                id = 1
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json");

            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromMilliseconds(_rpcTimeoutMs));

            var response = await _httpClient.PostAsync(
                _rpcUrl,
                content,
                cts.Token);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync(ct);

            using var doc = JsonDocument.Parse(responseBody);
            if (doc.RootElement.TryGetProperty("result", out var result))
            {
                return result.GetString();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    private async Task<string> ParseEncodedOutputAsync(Stream logsStream)
    {
        try
        {
            // Docker logs stream contains both stdout and stderr
            // Format: [1 byte stream type][4 bytes size][payload]

            using var reader = new StreamReader(logsStream);
            var logs = await reader.ReadToEndAsync();

            _logger.LogInformation("Docker logs: {Logs}", logs[..Math.Min(200, logs.Length)] + "...");

            // RLP encoder outputs hex string: 0x...
            // Regex: Match hex string pattern
            var match = System.Text.RegularExpressions.Regex.Match(
                logs,
                @"0x[0-9a-fA-F]+",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                throw new InvalidOperationException(
                    "Failed to parse RLP encoded output from Docker logs");
            }

            return match.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing RLP output");
            throw;
        }
    }

    private async Task ClearBesuDataDirectoryAsync(string containerId, CancellationToken ct = default)
    {
        try
        {
            // Remove all files inside the container's /opt/besu/data directory
            // This allows Besu to reinitialize with new genesis.json

            _logger.LogInformation("Clearing Besu data directory via Docker");

            // Use busybox container to clear the volume mounted to besu container
            var clearResult = await _dockerClient.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Image = "busybox:latest",
                    Cmd = new[] { "sh", "-c", "rm -rf /opt/besu/data/* 2>/dev/null || true" },
                    HostConfig = new HostConfig
                    {
                        // Mount volumes from the besu container
                        VolumesFrom = new List<string> { containerId }
                    }
                },
                ct);

            // Start temp container to execute cleanup
            await _dockerClient.Containers.StartContainerAsync(
                clearResult.ID,
                new ContainerStartParameters(),
                ct);

            // Wait for it to finish
            var clearWait = await _dockerClient.Containers
                .WaitContainerAsync(clearResult.ID, ct);

            _logger.LogInformation(
                "Besu data cleared (exit code: {ExitCode})",
                clearWait.StatusCode);

            // Cleanup temp container
            await _dockerClient.Containers.RemoveContainerAsync(
                clearResult.ID,
                new ContainerRemoveParameters { Force = true },
                ct);

            _logger.LogInformation("Besu data directory cleaned successfully");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Warning: Could not clear Besu data directory automatically. " +
                "Manual cleanup may be needed. Error: {Message}", ex.Message);
            // Don't throw - let startup continue, might work anyway
        }
    }

    private string GetDockerSocketUrl()
    {
        // ✅ Windows
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "npipe://./pipe/docker_engine";  // ← Windows named pipe
        }

        // ✅ Linux/Mac
        return "unix:///var/run/docker.sock";  // ← Unix socket
    }
}
