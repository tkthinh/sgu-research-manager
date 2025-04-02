import { useQuery } from '@tanstack/react-query';
import { getSystemConfig } from '../lib/api/systemConfigApi';
import { SystemConfig } from '../lib/types/models/SystemConfig';
import { ApiResponse } from '../lib/types/common/ApiResponse';

export function useSystemStatus() {
  const { data, isLoading } = useQuery<ApiResponse<SystemConfig>>({
    queryKey: ['systemConfig'],
    queryFn: getSystemConfig,
  });

  const isSystemOpen = () => {
    if (!data?.data) return false;
    
    const now = new Date();
    const start = new Date(data.data.startDate);
    const end = new Date(data.data.endDate);
    
    return now >= start && now <= end && !data.data.isClosed;
  };

  const canEditWork = (proofStatus?: number) => {
    if (!data?.data) return false;
    
    const now = new Date();
    const start = new Date(data.data.startDate);
    const end = new Date(data.data.endDate);
    
    // Nếu hệ thống đóng, chỉ cho phép sửa các công trình có trạng thái KhongHopLe
    if (data.data.isClosed || now < start || now > end) {
      return proofStatus === 1; // 1 là KhongHopLe
    }
    
    return true;
  };

  return {
    systemConfig: data?.data,
    isLoading,
    isSystemOpen: isSystemOpen(),
    canEditWork,
  };
} 