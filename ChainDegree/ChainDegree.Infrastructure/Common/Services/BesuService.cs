using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.Common.Services;

namespace ChainDegree.Infrastructure.Common.Services;

internal class BesuService : IBesuService
{
    public Task<string> EncodeValidatorsToExtraData(string toEncodePath, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RestartBesuContainer(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyValidators(List<string> addresses, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
