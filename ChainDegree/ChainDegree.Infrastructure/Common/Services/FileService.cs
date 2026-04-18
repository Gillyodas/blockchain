using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.Common.Services;

namespace ChainDegree.Infrastructure.Common.Services;

internal class FileService : IFileService
{
    public Task UpdateGenesisExtraData(string extraData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task WriteJsonAsync<T>(string filePath, List<T> data, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
