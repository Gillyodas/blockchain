using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.Application.Common.Services;

public interface IBesuService
{
    Task<string> EncodeValidatorsToExtraData(string toEncodePath, CancellationToken cancellationToken);
    Task RestartBesuContainer(CancellationToken cancellationToken);
    Task<bool> VerifyValidators(List<string> addresses, CancellationToken cancellationToken);
}
