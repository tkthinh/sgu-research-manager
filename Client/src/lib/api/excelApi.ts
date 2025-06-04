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

export const exportWorksByAdmin = async (userId: string, academicYearId?: string, proofStatus?: number): Promise<Blob> => {
    const params = new URLSearchParams();
    params.append('userId', userId);
    if (academicYearId) params.append('academicYearId', academicYearId);
    if (proofStatus !== undefined) params.append('proofStatus', proofStatus.toString());

    const response = await apiClient.get(`/excel/export-by-admin?${params.toString()}`, {
        responseType: 'blob'
    });
    return response.data;
};

export const exportAllWorks = async (academicYearId?: string, proofStatus?: number, source?: number): Promise<Blob> => {
    const params = new URLSearchParams();
    if (academicYearId) params.append('academicYearId', academicYearId);
    if (proofStatus !== undefined) params.append('proofStatus', proofStatus.toString());
    if (source !== undefined) params.append('source', source.toString());

    const response = await apiClient.get(`/excel/export-all-works?${params.toString()}`, {
        responseType: 'blob'
    });
    return response.data;
};

export const importExcel = async (file: File): Promise<void> => {
    const formData = new FormData();
    formData.append('file', file);
    
    await apiClient.post('/excel/import', formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });
};