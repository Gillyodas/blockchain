using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.SharedKernel.External.ChainDegreeBlockchainService;

/// <summary>
/// Exception cho Besu service errors
/// </summary>
public class BesuServiceException : Exception
{
    public BesuServiceException(string message) : base(message) { }
    public BesuServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}
