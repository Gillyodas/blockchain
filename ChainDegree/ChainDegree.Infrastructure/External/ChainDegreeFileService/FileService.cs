using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ChainDegree.Application.External.ChainDegreeBlockchainService;
using ChainDegree.Application.External.ChainDegreeFileService;
using ChainDegree.SharedKernel.External.ChainDegreeFileService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChainDegree.Infrastructure.External.ChainDegreeFileService;

/// <summary>
/// Xử lý file operations: read/write JSON, update genesis.json
/// 
/// Tại sao abstraction này cần:
/// - Testing: Mock IFileService thay vì thực I/O
/// - Flexibility: Có thể thay đổi storage (file → S3 → etc.)
/// - Error handling: Centralized logging & error handling
/// </summary>
public class FileService : IFileService
{
    private readonly IConfiguration _configuration;
    private readonly string _configPath;
    private readonly ILogger<FileService> _logger;

    public FileService(
        IConfiguration configuration,
        ILogger<FileService> logger)
    {
        _configuration = configuration;
        _configPath = configuration["Blockchain:Besu:ConfigPath"]
            ?? "./besu/config";
        _logger = logger;
    }

    public async Task BackupGenesisAsync(CancellationToken ct = default)
    {
        try
        {
            var genesisPath = Path.Combine(_configPath, "genesis.json");
            var backupPath = Path.Combine(
                _configPath,
                $"genesis.backup.{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");

            _logger.LogInformation("Backing up genesis.json to: {BackupPath}", backupPath);

            if (File.Exists(genesisPath))
            {
                await Task.Run(() =>
                {
                    File.Copy(genesisPath, backupPath, overwrite: true);
                }, ct);

                _logger.LogInformation("genesis.json backed up to: {BackupPath}", backupPath);
            }
            else
            {
                _logger.LogWarning("genesis.json not found for backup");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error backing up genesis.json");
            // Don't throw - backup failure shouldn't block operation
            // But log it so admin knows
        }
    }

    public async Task<T?> ReadJsonAsync<T>(string path, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Reading JSON from: {Path}", path);

            if (!File.Exists(path))
            {
                _logger.LogWarning("File not found: {Path}", path);
                return default;
            }

            var json = await File.ReadAllTextAsync(path, ct);

            var result = JsonSerializer.Deserialize<T>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            _logger.LogInformation("JSON read successfully from: {Path}", path);
            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Read JSON cancelled: {Path}", path);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Invalid JSON format in: {Path}", path);
            throw new FileServiceException(
                $"Invalid JSON format in {path}: {ex.Message}",
                ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading JSON from: {Path}", path);
            throw new FileServiceException(
                $"Failed to read JSON from {path}: {ex.Message}",
                ex);
        }
    }

    public async Task UpdateGenesisExtraDataAsync(string newExtraData, CancellationToken ct = default)
    {
        try
        {
            var genesisPath = Path.Combine(_configPath, "genesis.json");

            _logger.LogInformation("Updating genesis.json extraData");
            _logger.LogInformation("Genesis path: {Path}", genesisPath);
            _logger.LogInformation("New extraData: {ExtraData}", newExtraData[..Math.Min(50, newExtraData.Length)] + "...");

            if (!File.Exists(genesisPath))
            {
                throw new FileNotFoundException($"genesis.json not found at {genesisPath}");
            }

            var genesisText = await File.ReadAllTextAsync(genesisPath, ct);

            using var doc = JsonDocument.Parse(genesisText);
            _logger.LogInformation("genesis.json is valid JSON");

            // Use JSON manipulation instead of regex - more reliable
            // Parse the JSON, modify it, and re-serialize it
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Deserialize to dynamic object
            var genesis = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(genesisText);

            if (genesis == null)
            {
                throw new InvalidOperationException("Failed to deserialize genesis.json");
            }

            // Update extraData
            genesis["extraData"] = JsonSerializer.SerializeToElement(newExtraData);

            // Re-serialize with indentation
            var updatedJson = JsonSerializer.Serialize(genesis, options);

            // Validate the updated JSON
            using var newDoc = JsonDocument.Parse(updatedJson);
            _logger.LogInformation("Updated genesis.json is still valid JSON");

            await File.WriteAllTextAsync(genesisPath, updatedJson, ct);
            _logger.LogInformation("genesis.json extraData updated successfully");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Update genesis cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating genesis.json extraData");
            throw new FileServiceException(
                $"Failed to update genesis.json: {ex.Message}",
                ex);
        }
    }

    public async Task WriteJsonAsync<T>(string path, T data, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Writing JSON to {Path}", path);

            var directory = Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
                _logger.LogInformation("Directory ensured: {Directory}", directory);
            }

            var json = JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    // Nếu có null value, vẫn ghi (explicit set null)
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
                });

            await File.WriteAllTextAsync(path, json, ct);

            _logger.LogInformation("JSON written successfully to: {Path}", path);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Write JSON cancelled: {Path}", path);
            throw;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Permission denied when writing: {Path}", path);
            throw new FileServiceException(
                $"Permission denied when writing to {path}",
                ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.LogError(ex, "Directory not found: {Path}", path);
            throw new FileServiceException(
                $"Directory not found for {path}",
                ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error writing JSON to: {Path}", path);
            throw new FileServiceException(
                $"Failed to write JSON to {path}: {ex.Message}",
                ex);
        }
    }
}
