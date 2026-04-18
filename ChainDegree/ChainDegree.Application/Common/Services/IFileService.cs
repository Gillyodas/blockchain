using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.Application.Common.Services;

public interface IFileService
{
    Task WriteJsonAsync<T>(string filePath, List<T> data, CancellationToken cancellationToken);
    Task UpdateGenesisExtraData(string extraData, CancellationToken cancellationToken);
}
