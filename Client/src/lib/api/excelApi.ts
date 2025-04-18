import apiClient from "./api";
import { WorkFilter } from "../types/models/Work";

export const exportWorks = async (filter: WorkFilter): Promise<Blob> => {
    console.log("Đang gọi API xuất Excel với filter:", filter);
    const response = await apiClient.get(`/excel/export-by-user`, {
        params: {
            academicYearId: filter.academicYearId,
            proofStatus: filter.proofStatus,
            source: filter.source,
            onlyRegisteredWorks: filter.onlyRegisteredWorks,
            onlyRegisterableWorks: filter.onlyRegisterableWorks
        },
        responseType: 'blob'
    });
    return response.data;
};