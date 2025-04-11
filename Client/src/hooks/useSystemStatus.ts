import { useQuery } from '@tanstack/react-query';
import { getSystemConfig } from '../lib/api/systemConfigApi';
import { SystemConfig } from '../lib/types/models/SystemConfig';
import { ApiResponse } from '../lib/types/common/ApiResponse';
import { DateTime } from "luxon";

export function useSystemStatus() {
  const { data, isLoading } = useQuery<ApiResponse<SystemConfig>>({
    queryKey: ['systemConfig'],
    queryFn: getSystemConfig,
  });

  const systemConfig = data?.data;

  const isSystemOpen = () => {
    if (!systemConfig) return false;

    // Use Luxon to create DateTime objects in the Asia/Saigon timezone
    const now = DateTime.now().setZone("Asia/Saigon");
    const openDateTime = DateTime.fromISO(systemConfig.openTime, { zone: "utc" }).setZone("Asia/Saigon");
    const closeDateTime = DateTime.fromISO(systemConfig.closeTime, { zone: "utc" }).setZone("Asia/Saigon");
    
    return now >= openDateTime && now <= closeDateTime;
  };

  const canEditWork = (proofStatus?: number) => {
    if (!systemConfig) return false;
    
    const now = DateTime.now().setZone("Asia/Saigon");
    const startDateTime = DateTime.fromISO(systemConfig.openTime, { zone: "utc" }).setZone("Asia/Saigon");
    const endDateTime = DateTime.fromISO(systemConfig.closeTime, { zone: "utc" }).setZone("Asia/Saigon");

    // If proofStatus is 0, editing is a no-go
    if (proofStatus === 0) {
      return false;
    }
    
    // If the system's closed or we're outside the allowed time, only allow editing if proofStatus is 1 (KhongHopLe)
    if (now < startDateTime || now > endDateTime) {
      return proofStatus === 1;
    }
    
    return true;
  };

  return {
    systemConfig,
    isLoading,
    isSystemOpen: isSystemOpen(),
    canEditWork,
  };
}
