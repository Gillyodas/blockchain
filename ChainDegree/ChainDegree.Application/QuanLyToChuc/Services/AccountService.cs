using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.Application.AccessControl.Settings;
using ControlHub.Application.Identity.Interfaces.Repositories;
using ControlHub.Domain.Identity.Aggregates;
using ControlHub.Domain.Identity.Security;
using ControlHub.Domain.Identity.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChainDegree.Application.QuanLyToChuc.Services;

public class AccountService
{
    public static void CreateCSDTAccount(string plainPassword, CancellationToken cancellationToken, IPasswordHasher passwordHasher, IAccountRepository accountRepository, ILogger<AccountService> logger, IConfiguration config)
    {
        var passResult = Password.Create(plainPassword, passwordHasher);
        if (passResult.IsFailure)
        {
            logger.LogError("Failed to create password: {Error}", passResult.Error);
            return;
        }

        var pass = passResult.Value;
        var roleId = Guid.Parse(config["RoleSettings:CSDTRoleId"]);
        var acc = Account.Create(Guid.NewGuid(), pass, roleId);

        accountRepository.AddAsync(acc, cancellationToken);
    }
}
