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
import { deleteWorkType, getWorkTypes } from "../../../lib/api/workTypesApi";
import WorkTypeForm from "./WorkTypeForm";

export default function WorkTypePage() {
  const queryClient = useQueryClient();

  // Fetch work types
  const { data, error, isPending, isSuccess, dataUpdatedAt } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  // Toast notifications
  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message);
    }
  }, [dataUpdatedAt]);

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

  const columns: GridColDef[] = [
    {
      field: "name",
      headerName: "Tên loại công trình",
      type: "string",
      width: 330,
    },
    {
      field: "workLevelCount",
      headerName: "Số cấp liên quan",
      type: "number",
      valueFormatter: (value) => value + ' cấp',
      width: 330,
    },
    {
      field: "actions",
      headerName: "",
      width: 330,
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
        flexDirection="column-reverse"
        alignItems="flex-end"
        sx={{ marginBottom: 2 }}
      >
        <Button variant="contained" onClick={() => handleOpen(null)}>
          Thêm loại công trình
        </Button>
      </Box>
      <Paper sx={{ width: 1010, marginX: "auto" }}>
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
            {deleteMutation.isPending ? "Đang xóa..." : "Xóa"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
