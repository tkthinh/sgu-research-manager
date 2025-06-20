import {
  Box,
  Button,
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
  deleteFactor,
  getFactors,
  getFactorsByWorkTypeId,
} from "../../../lib/api/factorsApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import { getScoreLevelText } from "../../../lib/utils/scoreLevelUtils";
import FactorForm from "./FactorForm";

export default function FactorPage() {
  const queryClient = useQueryClient();
  const [selectedWorkTypeId, setSelectedWorkTypeId] = useState<string>("");

  // Fetch work types for filter
  const { data: workTypesData, isLoading: isLoadingWorkTypes } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  // Fetch factors with optional workTypeId filter
  const { data, error, isPending, isSuccess, dataUpdatedAt } = useQuery({
    queryKey: ["factors", selectedWorkTypeId],
    queryFn: () =>
      selectedWorkTypeId
        ? getFactorsByWorkTypeId(selectedWorkTypeId)
        : getFactors(),
  });

  // Toast notifications
  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message, { toastId: "fetch-factors-success" });
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
    mutationFn: (id: string) => deleteFactor(id),
    onSuccess: () => {
      toast.success("Xóa hệ số quy đổi thành công!");
      queryClient.invalidateQueries({ queryKey: ["factors"] });
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
      field: "workTypeName",
      headerName: "Loại công trình",
      type: "string",
      width: 180,
    },
    {
      field: "workLevelName",
      headerName: "Cấp công trình",
      type: "string",
      width: 150,
    },
    {
      field: "purposeName",
      headerName: "Mục đích quy đổi",
      type: "string",
      width: 150,
    },
    { field: "name", headerName: "Tên hệ số", type: "string", width: 200 },
    {
      field: "scoreLevel",
      headerName: "Mức điểm",
      width: 160,
      renderCell: (params) => (
        <div>{getScoreLevelText(params.row.scoreLevel)}</div>
      ),
    },
    {
      field: "convertHour",
      headerName: "Giờ quy đổi",
      type: "number",
      width: 120,
    },
    {
      field: "maxAllowed",
      headerName: "Giới hạn đánh dấu",
      type: "number",
      width: 150,
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
        justifyContent="space-between"
        alignItems={{ xs: "flex-start", sm: "center" }}
        spacing={2}
        sx={{ mb: 2 }}
      >
        <Stack direction={{ xs: "column", sm: "row" }} spacing={2} width={{ xs: "100%", sm: "auto" }}>
          <FormControl fullWidth={true} sx={{ minWidth: { sm: 220 } }}>
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
  
          <Button
            variant="contained"
            onClick={() => handleOpen(null)}
          >
            Thêm hệ số quy đổi
          </Button>
        </Stack>
      </Stack>

      <Paper sx={{ width: "100%", overflow: "hidden" }}>
        <GenericTable columns={columns} data={data?.data || []} />
      </Paper>

      <FactorForm open={open} handleClose={handleClose} data={selectedData} />

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa hệ số quy đổi này?</Typography>
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
