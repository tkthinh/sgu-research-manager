import { useQuery } from '@tanstack/react-query';
import { getSystemConfig } from '../lib/api/systemConfigApi';
import { SystemConfig } from '../lib/types/models/SystemConfig';
import { ApiResponse } from '../lib/types/common/ApiResponse';
import { DateTime } from "luxon";
import { ProofStatus } from '../lib/types/enums/ProofStatus';
import { useMemo } from 'react';

export function useSystemStatus() {
  const { data, isLoading } = useQuery<ApiResponse<SystemConfig>>({
    queryKey: ['systemConfig'],
    queryFn: getSystemConfig,
  });

  const systemConfig = data?.data;

  const isSystemOpen = useMemo(() => {
    if (!systemConfig) return false;
    
    // Use Luxon to create DateTime objects in the Asia/Saigon timezone
    const now = DateTime.now().setZone("Asia/Saigon");
    const openDateTime = DateTime.fromISO(systemConfig.openTime, { zone: "utc" }).setZone("Asia/Saigon");
    const closeDateTime = DateTime.fromISO(systemConfig.closeTime, { zone: "utc" }).setZone("Asia/Saigon");
    
    return now >= openDateTime && now <= closeDateTime;
  }, [systemConfig]);

  const canEditWork = (proofStatus: number | undefined, isLocked: boolean, hasOtherValidAuthors: boolean = false) => {
    // Nếu công trình đã bị khóa (có tác giả đã được chấm hợp lệ)
    if (isLocked) {
      // Nếu tác giả hiện tại đã được chấm hợp lệ thì không được sửa/xóa
      if (proofStatus === 0) {
        return false;
      }
      // Nếu tác giả hiện tại chưa được chấm hoặc không hợp lệ thì được sửa thông tin tác giả
      return true;
    }

    // Nếu công trình chưa bị khóa
    // Khi hệ thống đóng, chỉ được sửa nếu proofStatus = 1 (không hợp lệ)
    if (!isSystemOpen) {
      return proofStatus === 1;
    }

    // Khi hệ thống mở, được sửa nếu chưa được chấm hoặc không hợp lệ
    return proofStatus !== 0;
  };

  const canDeleteWork = (proofStatus: number | undefined, isLocked: boolean, hasOtherValidAuthors: boolean = false) => {
    // Nếu có tác giả khác đã được chấm hợp lệ thì không được xóa
    if (hasOtherValidAuthors) {
      return false;
    }

    // Nếu công trình đã bị khóa (có tác giả đã được chấm hợp lệ)
    if (isLocked) {
      // Nếu tác giả hiện tại đã được chấm hợp lệ thì không được xóa
      if (proofStatus === 0) {
        return false;
      }
      // Nếu tác giả hiện tại chưa được chấm hoặc không hợp lệ thì được xóa
      return true;
    }

    // Nếu công trình chưa bị khóa
    // Khi hệ thống đóng, chỉ được xóa nếu proofStatus = 1 (không hợp lệ)
    if (!isSystemOpen) {
      return proofStatus === 1;
    }

    // Khi hệ thống mở, được xóa nếu chưa được chấm hoặc không hợp lệ
    return proofStatus !== 0;
  };

  return {
    systemConfig,
    isLoading,
    isSystemOpen,
    canEditWork,
    canDeleteWork,
  };
}
