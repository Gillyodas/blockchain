using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.Application.External.ChainDegreeFileService;

/// <summary>
/// Interface cho file service - xử lý I/O operations
/// Abstraction giúp dễ mock trong tests
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Ghi JSON data vào file
    /// </summary>
    /// <param name="path">Đường dẫn file</param>
    /// <param name="data">Data cần ghi</param>
    /// <param name="ct">Cancellation token</param>
    Task WriteJsonAsync<T>(string path, T data, CancellationToken ct = default);

    /// <summary>
    /// Đọc JSON từ file
    /// </summary>
    Task<T?> ReadJsonAsync<T>(string path, CancellationToken ct = default);

    /// <summary>
    /// Update extraData field trong genesis.json
    /// Chỉ update 1 field, không thay thế toàn bộ file
    /// </summary>
    /// <param name="newExtraData">Giá trị extraData mới (hex string)</param>
    Task UpdateGenesisExtraDataAsync(string newExtraData, CancellationToken ct = default);

    /// <summary>
    /// Backup genesis.json trước khi update
    /// Để có thể revert nếu cần
    /// </summary>
    Task BackupGenesisAsync(CancellationToken ct = default);
}
