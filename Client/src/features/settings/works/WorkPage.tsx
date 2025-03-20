import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Paper,
  Typography,
  Tooltip,
  Container,
  Tabs,
  Tab,
  Switch,
  FormControlLabel,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import { deleteWork, getMyWorks, createWork, updateWorkByAuthor, setMarkedForScoring } from "../../../lib/api/worksApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import { getWorkLevels } from "../../../lib/api/workLevelsApi";
import { getAuthorRoles } from "../../../lib/api/authorRolesApi";
import { getPurposes } from "../../../lib/api/purposesApi";
import { getScimagoFields } from "../../../lib/api/scimagoFieldsApi";
import { getFields } from "../../../lib/api/fieldsApi";
import WorkForm from "./WorkForm";
import { format } from "date-fns";
import { vi } from "date-fns/locale";
import { Work } from "../../../lib/types/models/Work";
import { ProofStatus } from "../../../lib/types/enums/ProofStatus";
import { ScoreLevel } from "../../../lib/types/enums/ScoreLevel";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import CancelIcon from "@mui/icons-material/Cancel";
import HistoryIcon from "@mui/icons-material/History";
import AddIcon from "@mui/icons-material/Add";
import { searchUsers, getUserById } from "../../../lib/api/usersApi";
import { User } from "../../../lib/types/models/User";

// Hàm chuyển đổi ScoreLevel thành chuỗi hiển thị
const getScoreLevelText = (scoreLevel: number): string => {
  switch (scoreLevel) {
    case ScoreLevel.One:
      return "1 điểm";
    case ScoreLevel.ZeroPointSevenFive:
      return "0.75 điểm";
    case ScoreLevel.ZeroPointFive:
      return "0.5 điểm";
    case ScoreLevel.TenPercent:
      return "Top 10%";
    case ScoreLevel.ThirtyPercent:
      return "Top 30%";
    case ScoreLevel.FiftyPercent:
      return "Top 50%";
    case ScoreLevel.HundredPercent:
      return "Top 100%";
    default:
      return "Không xác định";
  }
};

export default function WorksPage() {
  const queryClient = useQueryClient();
  const [openFormDialog, setOpenFormDialog] = useState(false);
  const [activeTab, setActiveTab] = useState(0);
  const [markingAuthorId, setMarkingAuthorId] = useState<string | null>(null);
  const [coAuthorsMap, setCoAuthorsMap] = useState<Record<string, User[]>>({});

  // Fetch works
  const { data: worksData, error: worksError, isPending: isLoadingWorks, refetch } = useQuery({
    queryKey: ["works", "my-works"],
    queryFn: getMyWorks,
    staleTime: 0, // Luôn refetch khi cần
  });

  // Fetch data for dropdowns
  const { data: workTypesData } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  const { data: workLevelsData } = useQuery({
    queryKey: ["workLevels"],
    queryFn: getWorkLevels,
  });

  const { data: authorRolesData } = useQuery({
    queryKey: ["authorRoles"],
    queryFn: getAuthorRoles,
  });

  const { data: purposesData } = useQuery({
    queryKey: ["purposes"],
    queryFn: getPurposes,
  });

  const { data: scimagoFieldsData } = useQuery({
    queryKey: ["scimagoFields"],
    queryFn: getScimagoFields,
  });

  const { data: fieldsData } = useQuery({
    queryKey: ["fields"],
    queryFn: getFields,
  });

  // Create/Update mutation
  const createMutation = useMutation({
    mutationFn: async (data: any) => {
      try {
        console.log("Dữ liệu thực tế gửi lên server:", JSON.stringify(data, null, 2));
        return await createWork(data);
      } catch (error: any) {
        console.error("Chi tiết lỗi từ server:", error.response?.data);
        throw error;
      }
    },
    onSuccess: (data) => {
      toast.success("Công trình đã được thêm thành công");
      
      console.log("Bắt đầu cập nhật lại dữ liệu sau khi thêm mới");
      
      // Xóa cache và refetch dữ liệu mới
      queryClient.removeQueries({ queryKey: ["works", "my-works"] });
      
      // Bắt buộc refetch dữ liệu ngay lập tức
      setTimeout(() => {
        console.log("Bắt đầu refetch sau khi xóa cache");
        refetch();
      }, 100);
      
      setOpenFormDialog(false);
    },
    onError: (error) => {
      console.error("Lỗi đầy đủ:", error);
      toast.error("Lỗi khi thêm công trình: " + (error as Error).message);
    },
  });

  const updateMutation = useMutation({
    mutationFn: (params: { workId: string; data: any }) => {
      console.log("Dữ liệu gửi đi khi cập nhật:", JSON.stringify(params.data, null, 2));
      return updateWorkByAuthor(params.workId, params.data);
    },
    onSuccess: (data) => {
      toast.success("Công trình đã được cập nhật thành công");
      console.log("Phản hồi từ API sau khi cập nhật:", JSON.stringify(data, null, 2));
      console.log("Details sau khi cập nhật:", data.data?.details);
      
      console.log("Bắt đầu cập nhật lại dữ liệu sau khi update");
      
      // Xóa cache và refetch dữ liệu mới
      queryClient.removeQueries({ queryKey: ["works", "my-works"] });
      
      // Bắt buộc refetch dữ liệu ngay lập tức
      setTimeout(() => {
        console.log("Bắt đầu refetch sau khi xóa cache");
        refetch();
      }, 100);
      
      setOpenFormDialog(false);
    },
    onError: (error) => {
      console.error("Lỗi khi cập nhật công trình:", error);
      toast.error("Lỗi khi cập nhật công trình: " + (error as Error).message);
    },
  });

  // Mutation để cập nhật trạng thái đánh dấu
  const markForScoringMutation = useMutation({
    mutationFn: (params: { authorId: string; marked: boolean }) => {
      setMarkingAuthorId(params.authorId);
      console.log(`Đánh dấu authorId ${params.authorId} với trạng thái ${params.marked}`);
      return setMarkedForScoring(params.authorId, params.marked);
    },
    onSuccess: () => {
      toast.success("Cập nhật trạng thái đánh dấu thành công");
      
      // Xóa cache và refetch dữ liệu mới
      queryClient.removeQueries({ queryKey: ["works", "my-works"] });
      
      // Bắt buộc refetch dữ liệu ngay lập tức
      setTimeout(() => {
        refetch();
        setMarkingAuthorId(null);
      }, 100);
    },
    onError: (error: any) => {
      console.error("Lỗi khi cập nhật trạng thái đánh dấu:", error);
      // Kiểm tra nếu lỗi chứa thông báo về vượt quá giới hạn
      const errorMessage = error.response?.data?.message || error.message || 'Đã có lỗi xảy ra';
      if (errorMessage.includes("vượt quá giới hạn") || errorMessage.includes("quá giới hạn")) {
        toast.error(`Không thể đánh dấu: ${errorMessage}`, { autoClose: 7000 });
      } else {
        toast.error(`Lỗi khi cập nhật trạng thái đánh dấu: ${errorMessage}`);
      }
      setMarkingAuthorId(null);
    },
  });

  // Hàm xử lý thay đổi trạng thái đánh dấu
  const handleMarkForScoring = async (authorId: string, currentMarked: boolean) => {
    try {
      await markForScoringMutation.mutateAsync({
        authorId,
        marked: !currentMarked
      });
    } catch (error) {
      // Lỗi đã được xử lý trong onError của mutation
    }
  };

  // Lấy thông tin đồng tác giả khi có dữ liệu công trình
  useEffect(() => {
    if (worksData?.data && worksData.data.length > 0) {
      const fetchCoAuthors = async () => {
        const newCoAuthorsMap: Record<string, User[]> = {};
        
        for (const work of worksData.data) {
          if (work.coAuthorUserIds && work.coAuthorUserIds.length > 0) {
            const coAuthors: User[] = [];
            
            // Lấy thông tin từng đồng tác giả
            for (const userId of work.coAuthorUserIds) {
              try {
                const response = await getUserById(userId);
                if (response.success && response.data) {
                  coAuthors.push(response.data);
                }
              } catch (error) {
                console.error("Lỗi khi lấy thông tin đồng tác giả:", error);
              }
            }
            
            newCoAuthorsMap[work.id] = coAuthors;
          }
        }
        
        setCoAuthorsMap(newCoAuthorsMap);
      };

      fetchCoAuthors();
    }
  }, [worksData]);

  // Toast notifications
  useEffect(() => {
    if (worksData && worksData.message) {
      toast.success(worksData.message, { toastId: "fetch-works-success" });
    }
  }, [worksData]);

  useEffect(() => {
    if (worksError) {
      toast.error("Có lỗi đã xảy ra: " + (worksError as Error).message, { toastId: "fetch-works-error" });
    }
  }, [worksError]);

  // Handle form dialog
  const [selectedWork, setSelectedWork] = useState<Work | null>(null);

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
    setActiveTab(newValue);
  };

  const handleOpenFormDialog = (work?: Work) => {
    if (work) {
      console.log("Mở form với công trình:", work);
      console.log("coAuthorUserIds:", work.coAuthorUserIds);
      console.log("Giờ tác giả:", work.authors?.[0]?.authorHour);
      console.log("Giờ công trình:", work.authors?.[0]?.workHour);
      console.log("Chi tiết công trình:", work.details);
      
      // Lấy userId từ localStorage hoặc context auth
      const currentUserId = localStorage.getItem("userId") || "";
      
      // Lấy thông tin tác giả từ authors[0] hoặc author
      const authorInfo = work.authors && work.authors.length > 0 
        ? {
            authorRoleId: work.authors[0].authorRoleId,
            purposeId: work.authors[0].purposeId,
            position: work.authors[0].position || 1,
            scoreLevel: work.authors[0].scoreLevel,
            scImagoFieldId: work.authors[0].scImagoFieldId || "",
            fieldId: work.authors[0].fieldId || "",
          }
        : work.author || {
            authorRoleId: "",
            purposeId: "",
            position: 1,
            scoreLevel: undefined,
            scImagoFieldId: "",
            fieldId: "",
          };

      // Đảm bảo tất cả các trường cần thiết có giá trị
      const workWithDefaults = {
        ...work,
        details: work.details || {},
        totalAuthors: work.totalAuthors || 1,
        totalMainAuthors: work.totalMainAuthors || 1,
        author: authorInfo,
        // Lọc người dùng hiện tại khỏi đồng tác giả - đảm bảo so sánh đúng kiểu dữ liệu
        coAuthorUserIds: Array.isArray(work.coAuthorUserIds) 
          ? work.coAuthorUserIds
              .filter(id => {
                console.log(`So sánh ${id.toString()} với ${currentUserId}`);
                return id.toString() !== currentUserId;
              })
              .map(id => id.toString())
          : []
      };
      
      console.log("Mở form với dữ liệu đã xử lý:", workWithDefaults);
      setSelectedWork(workWithDefaults);
    } else {
      setSelectedWork(null);
    }
    setActiveTab(0);
    setOpenFormDialog(true);
  };

  const handleCloseFormDialog = () => {
    setSelectedWork(null);
    setOpenFormDialog(false);
  };

  // Hàm kiểm tra tính hợp lệ của dữ liệu trước khi gửi
  const validateWorkData = (data: any): string[] => {
    // Danh sách lỗi
    const errors: string[] = [];
    
    // Kiểm tra các trường bắt buộc
    if (!data.title || data.title.trim() === "") {
      errors.push("Tiêu đề không được để trống");
    }
    
    if (!data.workTypeId) {
      errors.push("Vui lòng chọn loại công trình");
    }

    // Kiểm tra thông tin tác giả
    if (!data.author) {
      errors.push("Thông tin tác giả là bắt buộc");
    } else {
      if (!data.author.authorRoleId) {
        errors.push("Vai trò tác giả là bắt buộc");
      }
      
      if (!data.author.purposeId) {
        errors.push("Mục đích là bắt buộc");
      }
    }
    
    return errors;
  };

  const handleSubmit = async (data: any) => {
    try {
      console.log("Dữ liệu gốc từ form:", data);
      
      // Kiểm tra dữ liệu trước khi gửi
      const validationErrors = validateWorkData(data);
      if (validationErrors.length > 0) {
        // Hiển thị lỗi và dừng việc gửi dữ liệu
        toast.error("Dữ liệu không hợp lệ: " + validationErrors.join(", "));
        return;
      }
      
      // Xử lý ngày tháng - đảm bảo định dạng đúng cho server
      let formattedDate: string | undefined = undefined;
      if (data.timePublished) {
        try {
          // Chuyển đổi sang định dạng ISO cho timePublished
          const date = new Date(data.timePublished);
          // Sử dụng định dạng yyyy-MM-dd theo quy định của server
          formattedDate = date.toISOString().split('T')[0]; 
        } catch (err) {
          console.error("Lỗi khi chuyển đổi ngày tháng:", err);
        }
      }
      
      // Xử lý dữ liệu trước khi gửi
      const formattedData = {
        workRequest: {
          title: data.title?.trim(),
          timePublished: formattedDate,
          totalAuthors: data.totalAuthors ? Number(data.totalAuthors) : undefined,
          totalMainAuthors: data.totalMainAuthors ? Number(data.totalMainAuthors) : undefined,
          source: Number(data.source),
          workTypeId: data.workTypeId,
          workLevelId: data.workLevelId || undefined,
          details: data.details || {}  // Thêm thông tin chi tiết vào yêu cầu
        },
        authorRequest: {
          authorRoleId: data.author.authorRoleId,
          purposeId: data.author.purposeId,
          position: data.author.position ? parseInt(String(data.author.position)) : undefined,
          scoreLevel: data.author.scoreLevel ? Number(data.author.scoreLevel) : undefined,
          scImagoFieldId: data.author.sCImagoFieldId ? String(data.author.sCImagoFieldId) : undefined,
          fieldId: data.author.fieldId ? String(data.author.fieldId) : undefined
        },
        coAuthorUserIds: Array.isArray(data.coAuthorUserIds) ? data.coAuthorUserIds : []
      };
      
      // Log dữ liệu sau khi xử lý để debug
      console.log("Dữ liệu sau khi xử lý:", JSON.stringify(formattedData, null, 2));
      
      if (selectedWork?.id) {
        await updateMutation.mutateAsync({ workId: selectedWork.id, data: formattedData });
      } else {
        await createMutation.mutateAsync(formattedData);
      }
    } catch (error) {
      console.error("Lỗi khi gửi dữ liệu:", error);
      toast.error("Có lỗi xảy ra khi gửi dữ liệu. Vui lòng kiểm tra lại thông tin.");
    }
  };

  // Handle delete confirmation dialog
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);

  const handleDeleteClick = (id: string) => {
    setDeleteId(id);
    setDeleteDialogOpen(true);
  };

  const handleDeleteCancel = () => {
    setDeleteId(null);
    setDeleteDialogOpen(false);
  };

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: (id: string) => deleteWork(id),
    onSuccess: () => {
      toast.success("Xóa công trình thành công!");
      
      // Xóa cache và refetch dữ liệu mới
      queryClient.removeQueries({ queryKey: ["works", "my-works"] });
      
      // Bắt buộc refetch dữ liệu ngay lập tức
      setTimeout(() => {
        refetch();
      }, 100);
      
      setDeleteDialogOpen(false);
    },
    onError: (error: Error) => {
      toast.error("Lỗi khi xóa công trình: " + error.message);
    },
  });

  const handleDeleteConfirm = async () => {
    if (deleteId) {
      try {
        await deleteMutation.mutateAsync(deleteId);
      } catch (error) {
        // Lỗi đã được xử lý trong onError của mutation
      }
    }
  };

  const getProofStatusText = (status: number) => {
    switch (status) {
      case ProofStatus.ChuaXuLy:
        return "Chưa xử lý";
      case ProofStatus.HopLe:
        return "Hợp lệ";
      case ProofStatus.KhongHopLe:
        return "Không hợp lệ";
      default:
        return "Không xác định";
    }
  };

  const columns: GridColDef[] = [
    {
      field: "stt",
      headerName: "STT",
      width: 70,
      renderCell: (params) => {
        const rowIds = params.api.getAllRowIds();
        const index = rowIds.indexOf(params.id);
        return <div>{index + 1}</div>;
      },
    },
    {
      field: "title",
      headerName: "Tên công trình",
      type: "string",
      width: 250,
    },
    {
      field: "timePublished",
      headerName: "Thời gian xuất bản",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        if (!params.value) return <div>-</div>;
        try {
          // Hiển thị giá trị gốc nếu không thể chuyển đổi
          const formattedDate = format(new Date(params.value), "dd/MM/yyyy", { locale: vi });
          return <div>{formattedDate}</div>;
        } catch (error) {
          console.log("Lỗi định dạng ngày:", params.value, error);
          return <div>{params.value}</div>;
        }
      },
    },
    {
      field: "workTypeName",
      headerName: "Loại công trình",
      type: "string",
      width: 150,
    },
    {
      field: "workLevelName",
      headerName: "Cấp công trình",
      type: "string",
      width: 150,
    },
    {
      field: "details",
      headerName: "Thông tin chi tiết của công trình",
      type: "string",
      width: 300,
      renderCell: (params: any) => {
        if (!params.row.details) return <div>-</div>;
        
        const details = params.row.details;
        const detailsText = Object.entries(details)
          .map(([key, value]) => `${key}: ${String(value)}`)
          .join('\n');
        
        return (
          <Tooltip 
            title={
              <div style={{ whiteSpace: 'pre-line' }}>
                {detailsText}
              </div>
            } 
            arrow
          >
            <div style={{ 
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              whiteSpace: 'nowrap',
              width: '100%'
            }}>
              {Object.entries(details).map(([key, value], index) => (
                <span key={index}>
                  {index > 0 && '; '}
                  <b>{key}</b>: {String(value)}
                </span>
              ))}
            </div>
          </Tooltip>
        );
      },
    },
    {
      field: "totalAuthors",
      headerName: "Tổng số tác giả",
      type: "number",
      width: 130,
    },
    {
      field: "totalMainAuthors",
      headerName: "Tổng số tác giả chính",
      type: "number",
      width: 160,
    },
    {
      field: "authorRoleName",
      headerName: "Vai trò tác giả",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.authorRoleName : "Không xác định"}</div>;
      },
    },
    {
      field: "position",
      headerName: "Vị trí tác giả",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.position !== undefined && author?.position !== null ? author.position : "Không xác định"}</div>;
      },
    },
    {
      field: "coAuthors",
      headerName: "Đồng tác giả",
      type: "string",
      width: 250,
      renderCell: (params: any) => {
        const workId = params.row.id;
        const coAuthors = coAuthorsMap[workId] || [];
        
        if (coAuthors.length === 0) return <div>-</div>;
        
        const coAuthorsText = `${coAuthors.length} đồng tác giả`;
        
        return (
          <Tooltip 
            title={
              <div>
                <Typography variant="subtitle2">Danh sách đồng tác giả:</Typography>
                {coAuthors.map((user, index) => (
                  <Typography key={index} variant="body2">
                    • {user.fullName} - {user.userName} - {user.departmentName || "Chưa có phòng ban"}
                  </Typography>
                ))}
              </div>
            }
          >
            <div>{coAuthorsText}</div>
          </Tooltip>
        );
      },
    },
    {
      field: "purposeName",
      headerName: "Mục đích quy đổi",
      type: "string",
      width: 180,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.purposeName : "Không xác định"}</div>;
      },
    },
    {
      field: "fieldName",
      headerName: "Ngành tính điểm",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.fieldName : "Không xác định"}</div>;
      },
    },
    {
      field: "scImagoFieldName",
      headerName: "Ngành SCImago",
      type: "string",
      width: 180,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.scImagoFieldName : "Không xác định"}</div>;
      },
    },
    {
      field: "workHour",
      headerName: "Giờ công trình",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.workHour !== undefined && author?.workHour !== null ? author.workHour : "Không xác định"}</div>;
      },
    },
    {
      field: "authorHour",
      headerName: "Giờ tác giả",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.authorHour !== undefined && author?.authorHour !== null ? author.authorHour : "Không xác định"}</div>;
      },
    },
    {
      field: "markedForScoring",
      headerName: "Đánh dấu",
      width: 120,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        if (!author) return <div>-</div>;
        
        const isMarked = author.markedForScoring === true;
        const authorId = author.id;
        const isLoading = markingAuthorId === authorId;
        
        return (
          <Tooltip 
            title={isMarked 
                ? "Bỏ đánh dấu công trình này" 
                : "Đánh dấu công trình này để tính điểm"
            }
          >
            <span>
              {isLoading ? (
                <CircularProgress size={24} />
              ) : (
                <FormControlLabel
                  control={
                    <Switch
                      checked={isMarked}
                      onChange={() => handleMarkForScoring(authorId, isMarked)}
                      disabled={markForScoringMutation.isPending}
                      color="primary"
                    />
                  }
                  label=""
                />
              )}
            </span>
          </Tooltip>
        );
      },
    },
    {
      field: "note",
      headerName: "Ghi chú",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.note || ""}</div>;
      },
    },
    {
      field: "proofStatus",
      headerName: "Trạng thái",
      type: "string",
      width: 140,
      renderCell: (params: any) => {
        // Lấy proofStatus từ author đầu tiên
        const author = params.row.authors && params.row.authors[0];
        const proofStatus = author ? author.proofStatus : undefined;
                
        if (proofStatus === undefined || proofStatus === null) {
          return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>Không xác định</div>;
        }
        
        // Kiểm tra giá trị và áp dụng trạng thái tương ứng
        if (proofStatus === ProofStatus.HopLe) {
          return (
            <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
              <CheckCircleIcon color="success" />
              Hợp lệ
            </div>
          );
        } else if (proofStatus === ProofStatus.KhongHopLe) {
          return (
            <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
              <CancelIcon color="error" />
              Không hợp lệ
            </div>
          );
        } else if (proofStatus === ProofStatus.ChuaXuLy) {
          return (
            <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
              <HistoryIcon color="action" />
              Chưa xử lý
            </div>
          );
        } else {
          return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>Không xác định</div>;
        }
      },
    },
    {
      field: "actions",
      headerName: "Thao tác",
      width: 200,
      renderCell: (params: any) => (
        <Box>
          <Button
            variant="contained"
            color="primary"
            size="small"
            onClick={() => handleOpenFormDialog(params.row)}
            sx={{ marginRight: 1 }}
            disabled={updateMutation.isPending}
          >
            Sửa
          </Button>
          <Button
            variant="contained"
            color="error"
            size="small"
            onClick={() => handleDeleteClick(params.row.id)}
            disabled={deleteMutation.isPending}
          >
            Xóa
          </Button>
        </Box>
      ),
    },
  ];

  if (isLoadingWorks) return <CircularProgress />;
  if (worksError) return <p>Lỗi: {(worksError as Error).message}</p>;

  return (
    <Container maxWidth="xl">
      <Box sx={{ display: "flex", justifyContent: "space-between", mb: 2 }}>
        <Typography variant="h4">Danh sách công trình</Typography>
        <Button 
          variant="contained" 
          color="primary" 
          onClick={() => handleOpenFormDialog()} 
          startIcon={<AddIcon />}
        >
          Thêm công trình
        </Button>
      </Box>

      {isLoadingWorks ? (
        <Box sx={{ display: "flex", justifyContent: "center", mt: 4 }}>
          <CircularProgress />
        </Box>
      ) : (
        <GenericTable columns={columns} data={worksData.data || []} />
      )}

      {/* Form Dialog */}
      <Dialog 
        open={openFormDialog} 
        onClose={handleCloseFormDialog}
        fullWidth
        maxWidth="md"
      >
        <DialogTitle>
          {selectedWork ? "Cập nhật công trình" : "Thêm công trình mới"}
        </DialogTitle>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', px: 3 }}>
          <Tabs value={activeTab} onChange={handleTabChange} aria-label="form tabs">
            <Tab label="Thông tin công trình" id="tab-0" />
            <Tab label="Thông tin tác giả" id="tab-1" />
          </Tabs>
        </Box>
        <DialogContent>
          {(workTypesData?.data && workLevelsData?.data && authorRolesData?.data && 
            purposesData?.data && scimagoFieldsData?.data && fieldsData?.data) ? (
            <WorkForm
              initialData={selectedWork}
              onSubmit={handleSubmit}
              isLoading={createMutation.isPending || updateMutation.isPending}
              workTypes={workTypesData.data}
              workLevels={workLevelsData.data}
              authorRoles={authorRolesData.data}
              purposes={purposesData.data}
              scimagoFields={scimagoFieldsData.data}
              fields={fieldsData.data}
              activeTab={activeTab}
            />
          ) : (
            <Box sx={{ display: "flex", justifyContent: "center", p: 3 }}>
              <CircularProgress />
            </Box>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseFormDialog} color="inherit">
            Đóng
          </Button>
        </DialogActions>
      </Dialog>

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa công trình này không?</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteCancel} color="inherit">
            Hủy
          </Button>
          <Button
            onClick={handleDeleteConfirm}
            color="error"
            variant="contained"
            disabled={deleteMutation.isPending}
          >
            {deleteMutation.isPending ? <CircularProgress size={24} /> : "Xóa"}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}