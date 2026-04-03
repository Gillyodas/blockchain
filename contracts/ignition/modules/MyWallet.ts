import { buildModule } from "@nomicfoundation/hardhat-ignition/modules";

export default buildModule("MyWalletModule", (m) => {
  const counter = m.contract("MyWallet");

  return { counter };
});
