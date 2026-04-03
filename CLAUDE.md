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

# Compile & deploy smart contracts (from contracts/)
cd contracts
npx hardhat compile
npx hardhat ignition deploy ignition/modules/MyWallet.ts --network <network>
```

No test projects exist yet.

## Architecture

Clean Architecture with 5 layers under `ChainDegree/`:

- **API** → ASP.NET Core entry point, controllers, middleware. Uses Scalar UI (not Swagger) for OpenAPI at `/scalar/v1`. Uses ControlHub framework which internally registers SwaggerGen, auth, and other infrastructure.
- **Application** → Application services, use cases. References SharedKernel. **Currently empty — pending implementation.**
- **Domain** → Entities, value objects. Uses `ControlHub.Domain`. References SharedKernel.
- **Infrastructure** → Blockchain integration (Nethereum), database (SQL Server/EF Core). References Application, Domain, SharedKernel.
- **SharedKernel** → Centralized error definitions and base types. No dependencies.

Dependency flow: API → Application → Domain, API → Infrastructure → Application/Domain, all → SharedKernel.

## Domain Model

The active bounded context is `QuanLyBangCap` (Credential Management). All code uses Vietnamese ubiquitous language.

**Key aggregates:**
- **CoSoDaoTao** (Educational Institution) — root aggregate; issues credentials, manages licenses (`GiayPhepCSDT`), creates students
- **BangCap** (Degree/Credential) — has a dual state machine:
  - SQL status: `ChuaXacNhan → DaXacNhan → DaThuHoi` (revocable) or `DaHuy` (final cancel)
  - Blockchain status: `ChoDuyet → XacNhan | ThatBai`
- **SinhVien** (Student) — entity under CoSoDaoTao; validated CCCD (Vietnamese ID) + email

**Stub aggregates** (domain folders exist, no implementation yet): `NhaTuyenDung`, `YeuCauDangKy`, `NhatKyXacMinh`, `BaoCaoGianLan`

**Blockchain storage pattern:** SQL Server holds all detailed credential data. Only `credentialHash = keccak256(normalized data + salt)` is written to the blockchain. Verification = recompute hash, compare on-chain. Credentials are immutable: update = revoke old + issue new.

## Key Dependencies

- **ControlHub.Core** — Custom framework that bundles SwaggerGen, authentication, authorization, and more. Do NOT add `AddSwaggerGen()` manually — ControlHub already includes it.
- **ControlHub.SharedKernel** — Provides `ValueObject` base class, `Error`, and `Result<T>` types.
- **Nethereum.Web3 / Nethereum.Besu** — .NET library for Ethereum/Besu blockchain interaction.
- **Serilog** — Structured logging. Included via ControlHub. Logs to console + daily rolling files (`Logs/log-.json` for machine parsing, `Logs/log-.txt` for human reading).

## Error Handling Pattern

All domain operations return `Result` or `Result<T>` — no exceptions for validation. Error definitions live in `SharedKernel` as static readonly instances:

```csharp
public static readonly Error DiemKhongHopLe =
    Error.Validation("BangCap.DiemKhongHopLe", "Điểm không hợp lệ...");
```

Factory methods (`Create()`) and aggregate operations (`DanhDauHuy()`, etc.) return `Result<T>` or `Result`. New error codes go in the appropriate `*Error.cs` file under `SharedKernel`.

## Smart Contracts

The `contracts/` folder uses **Hardhat 3** with **Hardhat Ignition** for deployment. Current contracts (`Counter.sol`, `MyWallet.sol`) are proof-of-concept only — the production credential contract has not been written yet.

- Solidity version: 0.8.28
- Deployment artifacts: `contracts/ignition/deployments/chain-338/`
- Chain 338 = Cronos testnet (used for PoC deployment)

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
- Business process specification: `docs/DacTaQuyTrinhNghiepVu.md`
