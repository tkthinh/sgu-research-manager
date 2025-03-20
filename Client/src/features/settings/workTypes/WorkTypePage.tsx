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
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import { deleteWorkType, getWorkTypesWithDetails } from "../../../lib/api/workTypesApi";
import WorkTypeForm from "./WorkTypeForm";

export default function WorkTypePage() {
  const queryClient = useQueryClient();

  // Fetch work types with details
  const { data, error, isPending, isSuccess, dataUpdatedAt } = useQuery({
    queryKey: ["workTypes", "details"],
    queryFn: async () => {
      const result = await getWorkTypesWithDetails();
      if (result?.data) {
        console.log("WorkTypes with details:", result.data);
        // Log chi tiết về Scimago fields
        result.data.forEach(wt => {
          console.log(`WorkType ${wt.name} has ${wt.scImagoFieldCount} Scimago fields:`, wt.scImagoFields);
        });
      }
      return result;
    }
  });

  // Toast notifications
  useEffect(() => {
    if (isSuccess && data?.message) {
      toast.success(data.message);
    }
  }, [dataUpdatedAt, isSuccess, data]);

  useEffect(() => {
    if (error) {
      toast.error("Có lỗi đã xảy ra: " + (error as Error).message);
    }
  }, [error]);

  // Handle form dialog
  const [open, setOpen] = useState(false);
  const [selectedData, setSelectedData] = useState(null);

  const handleOpen = (data) => {
    setSelectedData(data);
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  const handleEdit = (row) => {
    handleOpen(row);
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
    mutationFn: (id: string) => deleteWorkType(id),
    onSuccess: () => {
      toast.success("Xóa loại công trình thành công!");
      queryClient.invalidateQueries({ queryKey: ["workTypes"] }); // Refresh the data
      setDeleteDialogOpen(false);
    },
    onError: (error) => {
      toast.error("Lỗi khi xóa: " + (error as Error).message);
    },
  });

  const handleDeleteConfirm = () => {
    if (deleteId) {
      deleteMutation.mutate(deleteId);
    }
  };

  // Định dạng số với đuôi
  const formatWithSuffix = (count: number, suffix: string) => {
    return `${count} ${suffix}`;
  };

  // Tooltip nội dung
  const getWorkLevelTooltip = (workLevels) => {
    if (!workLevels || workLevels.length === 0) {
      return "Không có cấp công trình nào";
    }
    return workLevels.map(level => level.name).join("; ");
  };

  const getPurposeTooltip = (purposes) => {
    if (!purposes || purposes.length === 0) {
      return "Không có mục đích quy đổi nào";
    }
    return purposes.map(purpose => purpose.name).join("; ");
  };

  const getAuthorRoleTooltip = (authorRoles) => {
    if (!authorRoles || authorRoles.length === 0) {
      return "Không có vai trò tác giả nào";
    }
    return authorRoles.map(role => role.name).join("; ");
  };

  const columns: GridColDef[] = [
    {
      field: "stt",
      headerName: "STT",
      width: 70,
      renderCell: (params) => {
        const rowIndex = Array.from(params.api.getAllRowIds()).findIndex(id => id === params.row.id);
        return <div>{rowIndex + 1}</div>;
      },
    },
    {
      field: "name",
      headerName: "Tên loại công trình",
      type: "string",
      width: 200,
    },
    {
      field: "workLevelCount",
      headerName: "Cấp công trình",
      width: 130,
      renderCell: (params) => {
        const count = params.row.workLevelCount || 0;
        return (
          <Tooltip title={getWorkLevelTooltip(params.row.workLevels)} arrow>
            <span>{formatWithSuffix(count, "cấp")}</span>
          </Tooltip>
        );
      },
    },
    {
      field: "purposeCount",
      headerName: "Mục đích quy đổi",
      width: 150,
      renderCell: (params) => {
        const count = params.row.purposeCount || 0;
        return (
          <Tooltip title={getPurposeTooltip(params.row.purposes)} arrow>
            <span>{formatWithSuffix(count, "mục đích")}</span>
          </Tooltip>
        );
      },
    },
    {
      field: "authorRoleCount",
      headerName: "Vai trò tác giả",
      width: 140,
      renderCell: (params) => {
        const count = params.row.authorRoleCount || 0;
        return (
          <Tooltip title={getAuthorRoleTooltip(params.row.authorRoles)} arrow>
            <span>{formatWithSuffix(count, "vai trò")}</span>
          </Tooltip>
        );
      },
    },
    {
      field: "factorCount",
      headerName: "Hệ số quy đổi",
      width: 140,
      renderCell: (params) => {
        const count = params.row.factorCount || 0;
        return (
          <span>{formatWithSuffix(count, "hệ số")}</span>
        );
      },
    },
    {
      field: "scImagoFieldCount",
      headerName: "Ngành SCImago",
      width: 140,
      renderCell: (params) => {
        const count = params.row.scImagoFieldCount || 0;
        return (
            <span>{formatWithSuffix(count, "ngành")}</span>
        );
      },
    },
    {
      field: "actions",
      headerName: "Thao tác",
      width: 200,
      renderCell: (params) => (
        <Box>
          <Button
            variant="contained"
            color="primary"
            size="small"
            onClick={() => handleEdit(params.row)}
            sx={{ marginRight: 1 }}
            disabled={deleteMutation.isPending}
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

  if (isPending) return <CircularProgress />;
  if (error) return <p>Error: {(error as Error).message}</p>;

  return (
    <>
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        sx={{ marginBottom: 2 }}
      >
        <Typography variant="h4">Quản lý loại công trình</Typography>
        
        <Button variant="contained" onClick={() => handleOpen(null)}>
          Thêm loại công trình
        </Button>
      </Box>
      
      <Paper sx={{ width: "100%", overflow: "hidden" }}>
        <GenericTable columns={columns} data={data?.data || []} />
      </Paper>
      
      <WorkTypeForm open={open} handleClose={handleClose} data={selectedData} />

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa loại công trình này?</Typography>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={handleDeleteCancel}
            disabled={deleteMutation.isPending}
          >
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
    </>
  );
}
