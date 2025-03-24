import { useQuery } from "@tanstack/react-query";
import { getWorkTypes } from "../lib/api/workTypesApi";
import { getWorkLevels } from "../lib/api/workLevelsApi";
import { getAuthorRoles } from "../lib/api/authorRolesApi";
import { getPurposes } from "../lib/api/purposesApi";
import { getScimagoFields } from "../lib/api/scimagoFieldsApi";
import { getFields } from "../lib/api/fieldsApi";

/**
 * Hook tùy chỉnh để tải tất cả dữ liệu cần thiết cho form công trình
 * Sử dụng ở cả WorkPage và UserWorkDetailPage
 */
export function useWorkFormData() {
  const workTypesQuery = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  const workLevelsQuery = useQuery({
    queryKey: ["workLevels"],
    queryFn: getWorkLevels,
  });

  const authorRolesQuery = useQuery({
    queryKey: ["authorRoles"],
    queryFn: getAuthorRoles,
  });

  const purposesQuery = useQuery({
    queryKey: ["purposes"],
    queryFn: getPurposes,
  });

  const scimagoFieldsQuery = useQuery({
    queryKey: ["scimagoFields"],
    queryFn: getScimagoFields,
  });

  const fieldsQuery = useQuery({
    queryKey: ["fields"],
    queryFn: getFields,
  });

  // Kiểm tra xem tất cả các truy vấn đã hoàn thành chưa
  const isLoading = 
    workTypesQuery.isLoading || 
    workLevelsQuery.isLoading || 
    authorRolesQuery.isLoading || 
    purposesQuery.isLoading || 
    scimagoFieldsQuery.isLoading || 
    fieldsQuery.isLoading;

  // Kiểm tra xem tất cả dữ liệu đã có sẵn chưa
  const isDataReady = 
    workTypesQuery.data?.data && 
    workLevelsQuery.data?.data && 
    authorRolesQuery.data?.data && 
    purposesQuery.data?.data && 
    scimagoFieldsQuery.data?.data && 
    fieldsQuery.data?.data;

  return {
    workTypes: workTypesQuery.data?.data || [],
    workLevels: workLevelsQuery.data?.data || [],
    authorRoles: authorRolesQuery.data?.data || [],
    purposes: purposesQuery.data?.data || [],
    scimagoFields: scimagoFieldsQuery.data?.data || [],
    fields: fieldsQuery.data?.data || [],
    isLoading,
    isDataReady
  };
} 