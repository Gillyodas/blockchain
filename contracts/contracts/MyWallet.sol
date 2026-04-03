// SPDX-License-Identifier: GPL-3.0

pragma solidity >=0.8.2 <0.9.0;

contract MyWallet{
    address public owner;
    constructor(){
        owner = msg.sender;
    }

    function deposite() payable external {
    }

    function withdraw(uint256 amount) external {
        require(msg.sender == owner, "You are not the owner");
        payable(msg.sender).transfer(amount);
    }

    function withdrawAll() external {
        require(msg.sender == owner, "You are not the owner");
        uint256 balance = address(this).balance;
        require(balance > 0, "No balance to withdraw");
        payable(msg.sender).transfer(balance);
    }

    function getBalance() external view returns (uint256) {
        return address(this).balance;
    }
}