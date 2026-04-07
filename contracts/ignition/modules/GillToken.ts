import { buildModule } from "@nomicfoundation/hardhat-ignition/modules";

export default buildModule("GillTokenModule", (m) => {
  const counter = m.contract("GillToken");

  return { counter };
});
