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
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import { deleteWork, getMyWorks, createWork, updateWorkByAuthor } from "../../../lib/api/worksApi";
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

  // Fetch works
  const { data: worksData, error: worksError, isPending: isLoadingWorks } = useQuery({
    queryKey: ["works", "my-works"],
    queryFn: getMyWorks,
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
    mutationFn: createWork,
    onSuccess: () => {
      toast.success("Công trình đã được thêm thành công");
      queryClient.invalidateQueries({ queryKey: ["works"] });
      setOpenFormDialog(false);
    },
    onError: (error) => {
      toast.error("Lỗi khi thêm công trình: " + (error as Error).message);
    },
  });

  const updateMutation = useMutation({
    mutationFn: (params: { workId: string; data: any }) => {
      return updateWorkByAuthor(params.workId, params.data);
    },
    onSuccess: () => {
      toast.success("Công trình đã được cập nhật thành công");
      queryClient.invalidateQueries({ queryKey: ["works"] });
      setOpenFormDialog(false);
    },
    onError: (error) => {
      toast.error("Lỗi khi cập nhật công trình: " + (error as Error).message);
    },
  });

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
    setSelectedWork(work || null);
    setActiveTab(0);
    setOpenFormDialog(true);
  };

  const handleCloseFormDialog = () => {
    setSelectedWork(null);
    setOpenFormDialog(false);
  };

  const handleSubmit = async (data: any) => {
    if (selectedWork?.id) {
      await updateMutation.mutateAsync({ workId: selectedWork.id, data });
    } else {
      await createMutation.mutateAsync(data);
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
      queryClient.invalidateQueries({ queryKey: ["works"] });
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
      field: "details",
      headerName: "Thông tin chi tiết của công trình",
      type: "string",
      width: 350,
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
    // Các cột liên quan đến tác giả:
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
      field: "scImagoFieldName",
      headerName: "Ngành SCImago",
      type: "string",
      width: 200,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.scImagoFieldName : "Không xác định"}</div>;
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
      field: "position",
      headerName: "Vị trí tác giả",
      type: "string",
      width: 130,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.position !== undefined && author?.position !== null ? author.position : "Không xác định"}</div>;
      },
    },
    {
      field: "scoreLevel",
      headerName: "Mức điểm",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        if (!author || author.scoreLevel === undefined || author.scoreLevel === null) {
          return <div>Không xác định</div>;
        }
        return <div>{getScoreLevelText(author.scoreLevel)}</div>;
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