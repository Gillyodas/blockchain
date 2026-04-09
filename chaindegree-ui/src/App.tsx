import { useState, useEffect, useCallback } from 'react';
import { ethers } from 'ethers';
import { MY_ABI } from './abi'; 

const CONTRACT_ADDRESS = "0x6Aa377433d906F7E65c6c8086c573C6183C6d249";

// Định nghĩa kiểu cho MetaMask
interface EthereumProvider {
  request: (args: { method: string; params?: unknown[] }) => Promise<unknown>;
}

declare global {
  interface Window {
    ethereum?: EthereumProvider;
  }
}

function App() {
  const [status, setStatus] = useState<string>("Chưa kết nối");
  const [balance, setBalance] = useState<string>("0");
  const [owner, setOwner] = useState<string>("");
  const [amount, setAmount] = useState<string>("0.1");

  // Dùng useCallback để tránh lỗi render vòng lặp và thỏa mãn ESLint
  const fetchContractInfo = useCallback(async () => {
    const eth = window.ethereum;
    if (!eth) return;

    try {
      const provider = new ethers.BrowserProvider(eth as any); // Ép kiểu tạm thời cho provider
      const contract = new ethers.Contract(CONTRACT_ADDRESS, MY_ABI, provider);
      
      const b = await contract.getBalance() as bigint;
      const o = await contract.owner() as string;

      setBalance(ethers.formatEther(b));
      setOwner(o);
    } catch (err: unknown) {
      console.error("Lỗi lấy thông tin:", err);
    }
  }, []);

  useEffect(() => {
    // Gọi hàm fetch dữ liệu
    fetchContractInfo();
  }, [fetchContractInfo]); // Dependency array chuẩn TS

  const handleTransaction = async (action: 'deposit' | 'withdraw' | 'withdrawAll') => {
    const eth = window.ethereum;
    if (!eth) return alert("Cài MetaMask!");

    try {
      setStatus("Đang yêu cầu chữ ký...");
      const provider = new ethers.BrowserProvider(eth as any);
      const signer = await provider.getSigner();
      const contract = new ethers.Contract(CONTRACT_ADDRESS, MY_ABI, signer);

      let tx;
      if (action === 'deposit') {
        tx = await contract.deposite({ value: ethers.parseEther(amount) });
      } else if (action === 'withdraw') {
        tx = await contract.withdraw(ethers.parseEther(amount));
      } else {
        tx = await contract.withdrawAll();
      }

      setStatus("Đang đợi xác nhận...");
      await tx.wait();
      
      setStatus("Giao dịch thành công!");
      await fetchContractInfo(); // Cập nhật lại số liệu
    } catch (error: unknown) {
      // Fix lỗi Unexpected any ở đây
      if (error instanceof Error) {
        const ethersError = error as { reason?: string; message: string };
        setStatus("Lỗi: " + (ethersError.reason || ethersError.message));
      } else {
        setStatus("Lỗi không xác định");
      }
    }
  };

  return (
    <div style={{ padding: '40px', maxWidth: '600px', margin: '0 auto' }}>
      <h1>Dashboard</h1>
      <div style={{ border: '1px solid #ccc', padding: '15px', borderRadius: '8px' }}>
        <p>Số dư: {balance} ETH</p>
        <p>Trạng thái: {status}</p>
        <input 
          type="number" 
          value={amount} 
          onChange={(e) => setAmount(e.target.value)} 
          style={{ width: '100%', marginBottom: '10px' }}
        />
        <button onClick={() => handleTransaction('deposit')}>Nạp tiền</button>
        <button onClick={() => handleTransaction('withdraw')}>Rút tiền</button>
      </div>
    </div>
  );
}

export default App;