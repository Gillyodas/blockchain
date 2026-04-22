using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.Application.External.ChainDegreeBlockchainService;

/// <summary>
/// Cấu hình cho Besu blockchain
/// Được read từ appsettings.json hoặc environment variables
/// </summary>
public class BesuOptions
{
    public const string SectionName = "Blockchain:Besu";

    /// <summary>
    /// Đường dẫn thư mục config của Besu
    /// Nơi chứa genesis.json, toEncode.json
    /// </summary>
    public string ConfigPath { get; set; } = "./besu/config";

    /// <summary>
    /// RPC endpoint của Besu node
    /// Dùng để gọi JSON-RPC methods
    /// </summary>
    public string RpcUrl { get; set; } = "http://localhost:8545";

    /// <summary>
    /// Container name của Besu trong Docker
    /// Dùng để restart container
    /// </summary>
    public string ContainerName { get; set; } = "chaindegree-besu";

    /// <summary>
    /// Docker socket URL (Unix socket hoặc Windows named pipe)
    /// </summary>
    public string DockerSocketUrl { get; set; } = "unix:///var/run/docker.sock";

    /// <summary>
    /// Timeout cho RPC calls (milliseconds)
    /// </summary>
    public int RpcTimeoutMs { get; set; } = 30000;

    /// <summary>
    /// Timeout cho Docker calls (milliseconds)
    /// </summary>
    public int DockerTimeoutMs { get; set; } = 60000;
}
