using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.Application.External.ChainDegreeBlockchainService.Services;

/// <summary>
/// Interface cho Besu service - tương tác với Besu blockchain
/// </summary>
public interface IBesuService
{
    /// <summary>
    /// Encode validator addresses thành RLP extraData format
    /// Dùng Besu CLI tool qua Docker
    /// </summary>
    /// <param name="toEncodePath">Đường dẫn đến toEncode.json file</param>
    /// <returns>Hex string của extraData (0x...)</returns>
    Task<string> EncodeValidatorsToExtraDataAsync(
        string toEncodePath,
        CancellationToken ct = default);

    /// <summary>
    /// Encode validator addresses thành RLP extraData format (trực tiếp)
    /// Không cần file temp - pass validators trực tiếp
    /// </summary>
    /// <param name="validators">Danh sách validator addresses</param>
    /// <returns>Hex string của extraData (0x...)</returns>
    Task<string> EncodeValidatorsToExtraDataAsync(
        List<string> validators,
        CancellationToken ct = default);

    /// <summary>
    /// Restart Besu container
    /// Để apply genesis.json changes
    /// </summary>
    Task RestartBesuContainerAsync(CancellationToken ct = default);

    /// <summary>
    /// Verify rằng validators được recognize bởi Besu
    /// Query blockchain: ibft_getValidatorsByBlockNumber
    /// </summary>
    /// <param name="expectedAddresses">Danh sách addresses expected</param>
    /// <returns>true nếu tất cả addresses đều được tìm thấy</returns>
    Task<bool> VerifyValidatorsAsync(
        List<string> expectedAddresses,
        CancellationToken ct = default);

    /// <summary>
    /// Check Besu health - có đang chạy không?
    /// </summary>
    Task<bool> IsHealthyAsync(CancellationToken ct = default);

    /// <summary>
    /// Get current block number
    /// Dùng để verify Besu đang mine blocks
    /// </summary>
    Task<string?> GetBlockNumberAsync(CancellationToken ct = default);
}
