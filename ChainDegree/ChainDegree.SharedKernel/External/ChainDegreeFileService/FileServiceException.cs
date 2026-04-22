using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.SharedKernel.External.ChainDegreeFileService;

/// <summary>
/// Exception cho file service errors
/// Tại sao custom exception: Dễ catch specific errors, better error handling
/// </summary>
public class FileServiceException : Exception
{
    public FileServiceException(string message) : base(message) { }
    public FileServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}
