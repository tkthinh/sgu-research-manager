// AssignmentPage.tsx
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

import GenericTable from "../../app/shared/components/tables/DataTable";
import { deleteAssignment, getAssignments } from "../../lib/api/assignmentApi";
import AssignmentForm from "./AssignmentForm";

export default function AssignmentPage() {
  const queryClient = useQueryClient();

  const { data, error, isPending, isSuccess, dataUpdatedAt } = useQuery({
    queryKey: ["assignments"],
    queryFn: getAssignments,
  });

  const processedData = data?.data.map((item) => ({
    ...item,
    id:
      item.id === "00000000-0000-0000-0000-000000000000"
        ? `temp-${item.managerId}`
        : item.id,
  }));
  

  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message, { toastId: "fetch-assignments-success" });
    }
  }, [dataUpdatedAt]);

  useEffect(() => {
    if (error) {
      toast.error("Có lỗi khi tải phân công: " + (error as Error).message);
    }
  }, [error]);

  const [open, setOpen] = useState(false);
  const [selectedData, setSelectedData] = useState(null);

  const handleOpen = (data: any) => {
    setSelectedData(data);
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

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

  const deleteMutation = useMutation({
    mutationFn: (id: string) => deleteAssignment(id),
    onSuccess: () => {
      toast.success("Xóa phân công thành công!");
      queryClient.invalidateQueries({ queryKey: ["assignments"] });
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
    { field: "managerFullName", headerName: "Họ và tên quản lý", width: 200 },
    {
      field: "managerDepartmentName",
      headerName: "Đơn vị công tác",
      width: 220,
    },
    {
      field: "assignedDepartmentName",
      headerName: "Đơn vị được phân công",
      width: 220,
    },
    {
      field: "actions",
      headerName: "Thao tác",
      width: 360,
      renderCell: (params) => (
        <Box>
          <Button
            onClick={() => handleOpen(params.row)}
            variant="contained"
            sx={{ mr: 1 }}
          >
            Phân công
          </Button>
          <Button
            onClick={() => handleDeleteClick(params.row.id)}
            variant="contained"
            color="error"
            disabled={params.row.departmentId === "00000000-0000-0000-0000-000000000000"}
          >
            Bỏ phân công
          </Button>
        </Box>
      ),
    },
  ];

  if (isPending) return <CircularProgress />;
  if (error) return <p>Lỗi: {(error as Error).message}</p>;

  return (
    <>
      <Paper sx={{ width: "100%", overflow: "auto" }}>
        <GenericTable columns={columns} data={processedData || []} />
      </Paper>

      <AssignmentForm
        open={open}
        handleClose={handleClose}
        data={selectedData}
      />

      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa phân công này?</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteCancel}>Hủy</Button>
          <Button
            onClick={handleDeleteConfirm}
            color="error"
            variant="contained"
          >
            {deleteMutation.isPending ? "Đang xóa..." : "Xóa"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
