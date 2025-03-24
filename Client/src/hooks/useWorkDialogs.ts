import { useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-toastify";
import { Work } from "../lib/types/models/Work";
import { ProofStatus } from "../lib/types/enums/ProofStatus";
import { updateWorkByAdmin, updateWorkByAuthor, createWork } from "../lib/api/worksApi";

interface UseWorkDialogsProps {
  userId?: string;
  worksData?: any;
  refetchWorks?: () => void;
  isAuthorPage?: boolean;
}

/**
 * Hook tùy chỉnh để quản lý các dialog và logic cập nhật công trình
 */
export function useWorkDialogs({ userId, worksData, refetchWorks, isAuthorPage = false }: UseWorkDialogsProps) {
  const queryClient = useQueryClient();
  const [selectedWork, setSelectedWork] = useState<Work | null>(null);
  const [openUpdateDialog, setOpenUpdateDialog] = useState(false);
  const [openStatusDialog, setOpenStatusDialog] = useState(false);
  const [openNoteDialog, setOpenNoteDialog] = useState(false);
  const [newStatus, setNewStatus] = useState<ProofStatus>(ProofStatus.ChuaXuLy);
  const [newNote, setNewNote] = useState("");
  const [activeTab, setActiveTab] = useState(0);

  // Mutation cho Admin - cập nhật công trình
  const updateWorkMutation = useMutation({
    mutationFn: (params: { workId: string; userId: string; data: any }) => {
      return isAuthorPage 
        ? updateWorkByAuthor(params.workId, params.data) 
        : updateWorkByAdmin(params.workId, params.userId, params.data);
    },
    onSuccess: (response) => {
      console.log("Dữ liệu trả về từ API:", response);
      console.log("Cập nhật thành công, đang invalidate queries");
      
      if (isAuthorPage) {
        queryClient.invalidateQueries({ queryKey: ["works", "my-works"] });
      } else if (userId) {
        queryClient.invalidateQueries({ queryKey: ["works", "user", userId] });
      }
    },
    onError: (error) => {
      toast.error("Lỗi khi cập nhật công trình: " + (error as Error).message);
    },
  });

  // Mutation cho Author - tạo công trình mới
  const createWorkMutation = useMutation({
    mutationFn: (data: any) => {
      return createWork(data);
    },
    onSuccess: () => {
      toast.success("Đã thêm công trình mới thành công");
      
      // Xóa cache và refetch dữ liệu mới
      queryClient.invalidateQueries({ queryKey: ["works", "my-works"] });
      
      // Đóng dialog và refetch dữ liệu
      setOpenUpdateDialog(false);
      if (refetchWorks) {
        setTimeout(() => {
          refetchWorks();
        }, 100);
      }
    },
    onError: (error) => {
      toast.error("Lỗi khi thêm công trình mới: " + (error as Error).message);
    },
  });

  // Xử lý mở dialog cập nhật
  const handleOpenUpdateDialog = (work?: Work) => {
    if (work) {
      const currentUserId = localStorage.getItem("userId") || "";
      
      const authorInfo = work.authors && work.authors.length > 0 
        ? {
            authorRoleId: work.authors[0].authorRoleId,
            purposeId: work.authors[0].purposeId,
            position: work.authors[0].position || 1,
            scoreLevel: work.authors[0].scoreLevel,
            scImagoFieldId: work.authors[0].scImagoFieldId || "",
            fieldId: work.authors[0].fieldId || "",
            proofStatus: work.authors[0].proofStatus,
            note: work.authors[0].note || "",
          }
        : work.author || {
            authorRoleId: "",
            purposeId: "",
            position: 1,
            scoreLevel: undefined,
            scImagoFieldId: "",
            fieldId: "",
            proofStatus: ProofStatus.ChuaXuLy,
            note: "",
          };

      const workWithDefaults = {
        ...work,
        details: work.details || {},
        totalAuthors: work.totalAuthors || 1,
        totalMainAuthors: work.totalMainAuthors || 1,
        author: authorInfo,
        coAuthorUserIds: Array.isArray(work.coAuthorUserIds) 
          ? work.coAuthorUserIds
              .filter(id => id.toString() !== currentUserId)
              .map(id => id.toString())
          : []
      };
      
      setSelectedWork(workWithDefaults);
    } else {
      setSelectedWork(null);
    }
    setActiveTab(0);
    setOpenUpdateDialog(true);
  };

  const handleCloseUpdateDialog = () => {
    setSelectedWork(null);
    setOpenUpdateDialog(false);
  };

  // Xử lý mở dialog cập nhật trạng thái
  const handleOpenStatusDialog = (work: Work) => {
    setSelectedWork(work);
    if (work.authors && work.authors.length > 0) {
      setNewStatus(work.authors[0].proofStatus || ProofStatus.ChuaXuLy);
    }
    setOpenStatusDialog(true);
  };

  const handleCloseStatusDialog = () => {
    setSelectedWork(null);
    setOpenStatusDialog(false);
  };

  // Xử lý mở dialog ghi chú
  const handleOpenNoteDialog = (work: Work) => {
    setSelectedWork(work);
    if (work.authors && work.authors.length > 0) {
      setNewNote(work.authors[0].note || "");
    }
    setOpenNoteDialog(true);
  };

  const handleCloseNoteDialog = () => {
    setSelectedWork(null);
    setNewNote("");
    setOpenNoteDialog(false);
  };

  // Xử lý thay đổi trạng thái
  const handleStatusChange = (newValue: number) => {
    setNewStatus(newValue as ProofStatus);
  };

  // Xử lý thay đổi ghi chú
  const handleNoteChange = (newValue: string) => {
    setNewNote(newValue);
  };

  // Xử lý submit trạng thái
  const handleStatusSubmit = () => {
    if (selectedWork && selectedWork.authors && selectedWork.authors.length > 0 && userId) {
      console.log("Cập nhật trạng thái:", newStatus);
      
      const requestData = {
        workRequest: null,
        authorRequest: {
          proofStatus: newStatus,
        },
      };
      
      updateWorkMutation.mutate({
        workId: selectedWork.id,
        userId: userId,
        data: requestData,
      }, {
        onSuccess: (data) => {
          if (data.data && worksData) {
            const updatedWorks = worksData?.data?.map((work: Work) => 
              work.id === data.data.id ? data.data : work
            ) || [];
            
            if (worksData) {
              worksData.data = updatedWorks;
            }
            
            toast.success("Cập nhật trạng thái thành công");
            setOpenStatusDialog(false);
            
            if (refetchWorks) {
              refetchWorks();
            }
          }
        }
      });
    }
  };
  
  // Xử lý submit ghi chú
  const handleNoteSubmit = () => {
    if (selectedWork && selectedWork.authors && selectedWork.authors.length > 0 && userId) {
      console.log("Cập nhật ghi chú:", newNote);
      
      const currentProofStatus = selectedWork.authors[0].proofStatus;
      
      const requestData = {
        workRequest: null,
        authorRequest: {
          note: newNote,
          proofStatus: currentProofStatus 
        },
      };
      
      updateWorkMutation.mutate({
        workId: selectedWork.id,
        userId: userId,
        data: requestData,
      }, {
        onSuccess: (data) => {
          if (data.data && worksData) {
            const updatedWorks = worksData?.data?.map((work: Work) => 
              work.id === data.data.id ? data.data : work
            ) || [];
            
            if (worksData) {
              worksData.data = updatedWorks;
            }
            
            toast.success("Cập nhật ghi chú thành công");
            setOpenNoteDialog(false);
            
            if (refetchWorks) {
              refetchWorks();
            }
          }
        }
      });
    }
  };

  // Xử lý submit form công trình
  const handleUpdateSubmit = (data: any) => {
    // Xử lý ngày tháng
    let formattedDate: string | undefined = undefined;
    if (data.timePublished) {
      try {
        const date = new Date(data.timePublished);
        formattedDate = date.toISOString().split('T')[0]; 
      } catch (err) {
        console.error("Lỗi khi chuyển đổi ngày tháng:", err);
      }
    }
    
    // Xử lý tham số scImagoFieldId
    const scImagoFieldId = data.author.sCImagoFieldId || data.author.scImagoFieldId;
    
    // Đảm bảo coAuthorUserIds là một mảng hợp lệ
    const coAuthorUserIds = Array.isArray(data.coAuthorUserIds) 
      ? data.coAuthorUserIds.filter((id: any) => id) 
      : [];
    
    // AuthorRoleId có thể null
    const authorRoleId = data.author.authorRoleId === "" ? null : data.author.authorRoleId;
    
    const requestData = {
      workRequest: {
        title: data.title?.trim(),
        timePublished: formattedDate,
        totalAuthors: data.totalAuthors ? Number(data.totalAuthors) : undefined,
        totalMainAuthors: data.totalMainAuthors ? Number(data.totalMainAuthors) : undefined,
        source: Number(data.source),
        workTypeId: data.workTypeId,
        workLevelId: data.workLevelId || null,
        details: data.details || {},
        coAuthorUserIds: coAuthorUserIds
      },
      authorRequest: {
        authorRoleId: authorRoleId,
        purposeId: data.author.purposeId,
        position: data.author.position ? parseInt(String(data.author.position)) : undefined,
        scoreLevel: data.author.scoreLevel ? Number(data.author.scoreLevel) : null,
        scImagoFieldId: scImagoFieldId || null,
        fieldId: data.author.fieldId || null,
        proofStatus: data.author.proofStatus,
        note: data.author.note,
      },
    };

    // Nếu đang cập nhật công trình
    if (selectedWork && selectedWork.id) {
      updateWorkMutation.mutate({
        workId: selectedWork.id,
        userId: userId || "",
        data: requestData,
      }, {
        onSuccess: (data) => {
          if (data.data && worksData) {
            const updatedWorks = worksData?.data?.map((work: Work) => 
              work.id === data.data.id ? data.data : work
            ) || [];
            
            if (worksData) {
              worksData.data = updatedWorks;
            }
            
            toast.success("Cập nhật công trình thành công");
            setOpenUpdateDialog(false);
            
            if (refetchWorks) {
              refetchWorks();
            }
          }
        }
      });
    } 
    // Nếu đang tạo công trình mới
    else if (isAuthorPage) {
      // Tạo đối tượng request cho API tạo mới
      const createRequest = {
        title: requestData.workRequest.title,
        timePublished: requestData.workRequest.timePublished,
        totalAuthors: requestData.workRequest.totalAuthors,
        totalMainAuthors: requestData.workRequest.totalMainAuthors,
        source: requestData.workRequest.source,
        workTypeId: requestData.workRequest.workTypeId,
        workLevelId: requestData.workRequest.workLevelId,
        details: requestData.workRequest.details,
        coAuthorUserIds: requestData.workRequest.coAuthorUserIds,
        author: {
          authorRoleId: requestData.authorRequest.authorRoleId,
          purposeId: requestData.authorRequest.purposeId,
          position: requestData.authorRequest.position,
          scoreLevel: requestData.authorRequest.scoreLevel,
          scImagoFieldId: requestData.authorRequest.scImagoFieldId,
          fieldId: requestData.authorRequest.fieldId,
        },
      };
      
      createWorkMutation.mutate(createRequest);
    }
  };

  return {
    // State
    selectedWork,
    openUpdateDialog,
    openStatusDialog,
    openNoteDialog,
    newStatus,
    newNote,
    activeTab,
    updateWorkMutation,
    createWorkMutation,
    
    // Dialog handlers
    handleOpenUpdateDialog,
    handleCloseUpdateDialog,
    handleOpenStatusDialog,
    handleCloseStatusDialog,
    handleOpenNoteDialog,
    handleCloseNoteDialog,
    
    // Actions handlers
    handleStatusChange,
    handleNoteChange,
    handleStatusSubmit,
    handleNoteSubmit,
    handleUpdateSubmit,
    
    // Tab handlers
    setActiveTab,
  };
} 