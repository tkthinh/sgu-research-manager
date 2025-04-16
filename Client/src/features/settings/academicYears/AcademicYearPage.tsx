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
import {
  deleteAcademicYear,
  getAcademicYears,
} from "../../../lib/api/academicYearApi";
import AcademicYearForm from "./AcademicYearForm";

export default function AcademicYearPage() {
  const queryClient = useQueryClient();

  const { data, error, isPending, isSuccess, dataUpdatedAt, refetch } =
    useQuery({
      queryKey: ["academic-years"],
      queryFn: getAcademicYears,
    });

  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message, { toastId: "fetch-academic-years-success" });
    }
  }, [dataUpdatedAt]);

  useEffect(() => {
    if (error) {
      toast.error("Có lỗi xảy ra: " + (error as Error).message);
    }
  }, [error]);

  const [open, setOpen] = useState(false);
  const [selectedData, setSelectedData] = useState(null);

  const handleOpen = (data) => {
    setSelectedData(data);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    refetch();
  }

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
    mutationFn: (id: string) => deleteAcademicYear(id),
    onSuccess: () => {
      toast.success("Xóa năm học thành công!");
      queryClient.invalidateQueries({ queryKey: ["academic-years"] });
      setDeleteDialogOpen(false);
      refetch();
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
    { field: "name", headerName: "Tên năm học", width: 200 },
    {
      field: "startDate",
      headerName: "Bắt đầu",
      valueFormatter: (params) => new Date(params).toLocaleDateString(),
      width: 150,
    },
    {
      field: "endDate",
      headerName: "Kết thúc",
      valueFormatter: (params) => new Date(params).toLocaleDateString(),
      width: 150,
    },
    {
      field: "actions",
      headerName: "Hành động",
      width: 300,
      renderCell: (params) => (
        <Box>
          <Button
            variant="contained"
            color="primary"
            size="small"
            onClick={() => handleOpen(params.row)}
            sx={{ marginRight: 1 }}
          >
            Sửa
          </Button>
          <Button
            variant="contained"
            color="error"
            size="small"
            onClick={() => handleDeleteClick(params.row.id)}
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
      <Box display="flex" justifyContent="flex-end" mb={2}>
        <Button variant="contained" onClick={() => handleOpen(null)}>
          Thêm năm học
        </Button>
      </Box>
      <Paper sx={{ width: 1010, marginX: "auto" }}>
        <GenericTable
          columns={columns}
          data={(data?.data || [])
            .slice()
            .sort((a, b) => b.name.localeCompare(a.name))}
        />
      </Paper>
      <AcademicYearForm
        open={open}
        handleClose={handleClose}
        data={selectedData}
      />

      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa năm học này?</Typography>
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
