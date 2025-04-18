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
import { useEffect, useRef, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import {
  deleteUser,
  getUsers,
  importUserFromExcelFile,
} from "../../../lib/api/usersApi";
import { User } from "../../../lib/types/models/User";
import { getAcademicTitle } from "../../../lib/utils/academicTitleMap";
import { getOfficerRank } from "../../../lib/utils/officerRankMap";
import { getRole } from "../../../lib/utils/roleMap";
import UserForm from "./UserForm";

export default function UserPage() {
  const queryClient = useQueryClient();
  const fileInputRef = useRef<HTMLInputElement>(null);

  // Fetch users
  const { data, error, isPending, isSuccess, dataUpdatedAt, refetch } =
    useQuery({
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
    setSelectedUser(user);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    refetch();
  };

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
      refetch();
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

  // Import from Excel
  const handleImportClick = () => {
    fileInputRef.current?.click();
  };

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      const file = e.target.files[0];
      try {
        const response = await importUserFromExcelFile(file);
        toast.success(response.message);
        queryClient.invalidateQueries({ queryKey: ["users"] });
        refetch();
      } catch (error) {
        toast.error("Lỗi khi nhập file: " + (error as Error).message);
      }
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
      {/* Admin Import Button */}
      <Box
        sx={{ marginBottom: 2, display: "flex", justifyContent: "flex-end" }}
      >
        <Button variant="contained" color="primary" onClick={handleImportClick}>
          Nhập người dùng từ Excel
        </Button>
        <input
          type="file"
          accept=".xls,.xlsx"
          style={{ display: "none" }}
          ref={fileInputRef}
          onChange={handleFileChange}
        />
      </Box>

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
