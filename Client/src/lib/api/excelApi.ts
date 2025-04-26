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
    try {
        console.log('Gọi API exportWorksByAdmin với params:', { userId, academicYearId, proofStatus });
        
        const response = await apiClient.get(`/excel/export-by-admin`, {
            params: {
                userId,
                academicYearId,
                proofStatus
            },
            responseType: 'blob',
            timeout: 30000 // Tăng timeout lên 30 giây
        });

        if (!response.data) {
            throw new Error('Không nhận được dữ liệu từ server');
        }

        return response.data;
    } catch (error: any) {
        console.error('Lỗi khi gọi API exportWorksByAdmin:', error);
        if (error.code === 'ECONNABORTED') {
            throw new Error('Yêu cầu xuất Excel bị timeout. Vui lòng thử lại sau.');
        }
        throw error;
    }
};