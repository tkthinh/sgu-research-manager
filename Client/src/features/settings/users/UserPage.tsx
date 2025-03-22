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
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import { deleteUser, getUsers } from "../../../lib/api/usersApi";
import { User } from "../../../lib/types/models/User";
import { getAcademicTitle } from "../../../lib/utils/academicTitleMap";
import { getOfficerRank } from "../../../lib/utils/officerRankMap";
import { getRole } from "../../../lib/utils/roleMap";
import UserForm from "./UserForm";

export default function UserPage() {
  const queryClient = useQueryClient();

  // Fetch users
  const { data, error, isPending, isSuccess, dataUpdatedAt } = useQuery({
    queryKey: ["users"],
    queryFn: getUsers,
  });

  // Toast notifications on success and error
  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message, { toastId: "fetch-users-success" });
    }
  }, [dataUpdatedAt]);

  useEffect(() => {
    if (error) {
      toast.error("Có lỗi đã xảy ra: " + (error as Error).message);
    }
  }, [error]);

  // Handle form dialog for editing (creation is not allowed)
  const [open, setOpen] = useState(false);
  const [selectedUser, setSelectedUser] = useState<User | null>(null);

  const handleOpen = (user: User) => {
    console.log(user);
    setSelectedUser(user);
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  // Delete confirmation dialog
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
    mutationFn: (id: string) => deleteUser(id),
    onSuccess: () => {
      toast.success("Xóa người dùng thành công!");
      queryClient.invalidateQueries({ queryKey: ["users"] });
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
      field: "userName",
      headerName: "Mã số viên chức",
      type: "string",
      width: 150,
    },
    { field: "fullName", headerName: "Họ và tên", type: "string", width: 200 },
    { field: "email", headerName: "Email", type: "string", width: 200 },
    {
      field: "departmentName",
      headerName: "Đơn vị công tác",
      type: "string",
      width: 180,
    },
    { field: "fieldName", headerName: "Ngành", type: "string", width: 180 },
    {
      field: "phoneNumber",
      headerName: "Số điện thoại",
      type: "string",
      width: 150,
    },
    {
      field: "academicTitle",
      headerName: "Học hàm",
      type: "string",
      width: 180,
      valueFormatter: (value) => getAcademicTitle(value),
    },
    {
      field: "officerRank",
      headerName: "Ngạch công chức",
      type: "string",
      width: 180,
      valueFormatter: (value) => getOfficerRank(value),
    },
    {
      field: "role",
      headerName: "Quyền hạn",
      type: "string",
      width: 150,
      valueFormatter: (value) => getRole(value),
    },
    {
      field: "isApproved",
      headerName: "Đã duyệt",
      type: "boolean",
      width: 120,
    },
    {
      field: "actions",
      headerName: "",
      width: 300,
      renderCell: (params) => (
        <Box>
          <Button
            variant="contained"
            color="primary"
            size="small"
            onClick={() => handleOpen(params.row)}
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
      {/* User creation is not allowed so there is no add button */}
      <Paper sx={{ width: "100%", marginX: "auto", padding: 2 }}>
        <GenericTable columns={columns} data={data?.data || []} />
      </Paper>

      <UserForm open={open} handleClose={handleClose} data={selectedUser} />

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa người dùng này?</Typography>
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
            {deleteMutation.isPending ? "Đang xóa..." : "Xóa"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
