# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**ChainDegree** — Blockchain-based academic records system for storing and verifying student degrees. Uses Hyperledger Besu private blockchain with a .NET 10 backend.

## Build & Run Commands

```bash
# Build solution
dotnet build ChainDegree/ChainDegree.slnx

# Run API (from repo root)
dotnet run --project ChainDegree/ChainDegree.API

# Start Besu blockchain node
docker-compose up -d

# Stop Besu node
docker-compose down

# Verify Besu is running
curl -X POST --data '{"jsonrpc":"2.0","method":"eth_blockNumber","params":[],"id":1}' http://localhost:8545
```

No test projects exist yet.

## Architecture

Clean Architecture with 5 layers under `ChainDegree/`:

- **API** → ASP.NET Core entry point, controllers, middleware. Uses Scalar UI (not Swagger) for OpenAPI at `/scalar/v1`. Uses ControlHub framework which internally registers SwaggerGen, auth, and other infrastructure.
- **Application** → Application services, use cases. References SharedKernel.
- **Domain** → Entities, value objects. Uses `ControlHub.Domain`. References SharedKernel.
- **Infrastructure** → Blockchain integration (Nethereum), database (SQL Server/EF Core). References Application, Domain, SharedKernel.
- **SharedKernel** → Base classes, shared types. No dependencies.

Dependency flow: API → Application → Domain, API → Infrastructure → Application/Domain, all → SharedKernel.

## Key Dependencies

- **ControlHub.Core** — Custom framework that bundles SwaggerGen, authentication, authorization, and more. Do NOT add `AddSwaggerGen()` manually — ControlHub already includes it.
- **Nethereum.Web3 / Nethereum.Besu** — .NET library for Ethereum/Besu blockchain interaction.
- **Serilog** — Structured logging. Included via ControlHub. Logs to console + daily rolling files (`Logs/log-.json` for machine parsing, `Logs/log-.txt` for human reading).

## Blockchain Setup

- **Network**: Hyperledger Besu, IBFT 2.0 consensus, Chain ID 1337
- **Genesis config**: `besu/config/genesis.json`
- **Validator list**: `besu/config/toEncode.json` (used with Besu RLP encoder)
- **Ports**: 8545 (HTTP-RPC), 8546 (WebSocket)
- **Pre-funded account**: `0xfe3b557e8fb62b89f4916b721be55ceb828dbd73` (Besu dev test key — replace for production)
- **ContractAddress** in `appsettings.json` is placeholder `0x000...` until smart contract is deployed
- To re-encode validator extraData: `MSYS_NO_PATHCONV=1 docker run --rm -v "E:\blockchain\besu\config:/config" hyperledger/besu:latest rlp encode --from=/config/toEncode.json --type=IBFT_EXTRA_DATA`

## Development Notes

- **Target framework**: .NET 10.0
- **API ports**: 5160 (HTTP), 7178 (HTTPS)
- **Database**: SQL Server local instance, database name `ChainDegree`
- **Solution format**: `.slnx` (modern XML-based solution file)
- **Git repo** is at the repository root (`E:\blockchain\`), tracking both .NET code and blockchain config
- `appsettings.json` is gitignored; use `appsettings.Example.json` as template
- On Windows Git Bash, prefix Docker commands with `MSYS_NO_PATHCONV=1` to avoid path mangling
