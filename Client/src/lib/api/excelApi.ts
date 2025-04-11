import apiClient from "./api";

export const exportWorks = async (): Promise<Blob> => {
    console.log("Đang gọi API xuất Excel");
    const response = await apiClient.get(`/excel/export-by-user`, {
        responseType: 'blob'
    });
    return response.data;
};