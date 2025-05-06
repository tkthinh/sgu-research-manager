import CancelIcon from "@mui/icons-material/Cancel";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import {
  Box,
  Button,
  Chip,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Stack,
  Typography,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import {
  deleteAuthorRole,
  getAuthorRoles,
  getAuthorRolesByWorkTypeId,
} from "../../../lib/api/authorRolesApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import AuthorRoleForm from "./AuthorRoleForm";

export default function AuthorRolePage() {
  const queryClient = useQueryClient();
  const [selectedWorkTypeId, setSelectedWorkTypeId] = useState<string>("");

  // Fetch work types for filter
  const { data: workTypesData, isLoading: isLoadingWorkTypes } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  // Fetch author roles with optional workTypeId filter
  const { data, error, isPending, isSuccess, dataUpdatedAt } = useQuery({
    queryKey: ["authorRoles", selectedWorkTypeId],
    queryFn: () =>
      selectedWorkTypeId
        ? getAuthorRolesByWorkTypeId(selectedWorkTypeId)
        : getAuthorRoles(),
  });

  // Toast notifications
  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message, { toastId: "fetch-author-roles-success" });
    }
  }, [dataUpdatedAt, isSuccess, data]);

  useEffect(() => {
    if (error) {
      toast.error("Có lỗi đã xảy ra: " + (error as Error).message);
    }
  }, [error]);

  // Handle workType filter change
  const handleWorkTypeChange = (event: any) => {
    setSelectedWorkTypeId(event.target.value as string);
  };

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
    mutationFn: (id: string) => deleteAuthorRole(id),
    onSuccess: () => {
      toast.success("Xóa vai trò tác giả thành công!");
      queryClient.invalidateQueries({ queryKey: ["authorRoles"] });
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

  const columns: GridColDef[] = [
    {
      field: "stt",
      headerName: "STT",
      width: 70,
      renderCell: (params) => {
        const rowIndex = Array.from(params.api.getAllRowIds()).findIndex(
          (id) => id === params.row.id,
        );
        return <div>{rowIndex + 1}</div>;
      },
    },
    {
      field: "name",
      headerName: "Tên vai trò tác giả",
      type: "string",
      width: 300,
    },
    {
      field: "workTypeName",
      headerName: "Loại công trình",
      type: "string",
      width: 300,
    },
    {
      field: "isMainAuthor",
      headerName: "Tác giả chính",
      width: 150,
      renderCell: (params) => {
        const isMainAuthor = params.row.isMainAuthor;

        return isMainAuthor ? (
          <Chip
            icon={<CheckCircleIcon />}
            label="Tác giả chính"
            color="success"
            variant="outlined"
          />
        ) : (
          <Chip
            icon={<CancelIcon />}
            label="Thành viên"
            color="default"
            variant="outlined"
          />
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

  if (isPending && isLoadingWorkTypes) return <CircularProgress />;
  if (error) return <p>Lỗi: {(error as Error).message}</p>;

  return (
    <>
      <Stack
        direction={{ xs: "column", sm: "row" }}
        justifyContent="flex-end"
        alignItems="center"
        spacing={2}
        sx={{ mb: 2 }}
      >
        <Stack
          direction={{ xs: "column", sm: "row" }}
          spacing={2}
          width={{ xs: "100%", sm: "auto" }}
        >
          <FormControl fullWidth sx={{ minWidth: { sm: 220 } }}>
            <InputLabel id="work-type-filter-label">
              Lọc theo loại công trình
            </InputLabel>
            <Select
              labelId="work-type-filter-label"
              id="work-type-filter"
              value={selectedWorkTypeId}
              onChange={handleWorkTypeChange}
              label="Lọc theo loại công trình"
            >
              <MenuItem value="">Tất cả</MenuItem>
              {workTypesData?.data?.map((wt) => (
                <MenuItem key={wt.id} value={wt.id}>
                  {wt.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          <Button variant="contained" onClick={() => handleOpen(null)}>
            Thêm vai trò tác giả
          </Button>
        </Stack>
      </Stack>

      {/* Bảng dữ liệu */}
      <Paper sx={{ width: "100%", overflow: "hidden" }}>
        <GenericTable columns={columns} data={data?.data || []} />
      </Paper>

      {/* Form thêm / sửa vai trò tác giả */}
      <AuthorRoleForm
        open={open}
        handleClose={handleClose}
        data={selectedData}
      />

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>
            Bạn có chắc chắn muốn xóa vai trò tác giả này?
          </Typography>
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
