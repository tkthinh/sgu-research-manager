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
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import {
  deleteScimagoField,
  getScimagoFields,
  getScimagoFieldsByWorkTypeId,
} from "../../../lib/api/scimagoFieldsApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import ScimagoFieldForm from "./ScimagoFieldForm";

export default function ScimagoFieldPage() {
  const queryClient = useQueryClient();
  const [selectedWorkTypeId, setSelectedWorkTypeId] = useState<string>("");

  // Fetch work types for filter
  const { data: workTypesData, isLoading: isLoadingWorkTypes } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  // Fetch SCImago fields with optional workTypeId filter
  const {
    data,
    error,
    isPending,
    isSuccess,
    dataUpdatedAt
  } = useQuery({
    queryKey: ["scimagoFields", selectedWorkTypeId],
    queryFn: () => selectedWorkTypeId 
      ? getScimagoFieldsByWorkTypeId(selectedWorkTypeId)
      : getScimagoFields(),
  });

  // Toast notifications
  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message, { toastId: "fetch-scimago-fields-success" });
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
    mutationFn: (id: string) => deleteScimagoField(id),
    onSuccess: () => {
      toast.success("Xóa ngành SCImago thành công!");
      queryClient.invalidateQueries({ queryKey: ["scimagoFields"] });
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
        const rowIndex = Array.from(params.api.getAllRowIds()).findIndex(id => id === params.row.id);
        return <div>{rowIndex + 1}</div>;
      },
    },
    { field: "name", headerName: "Tên ngành SCImago", type: "string", width: 400 },
    { field: "workTypeName", headerName: "Loại công trình", type: "string", width: 400 },
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
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        sx={{ marginBottom: 2 }}
      >
        <Typography variant="h4">Quản lý ngành SCImago</Typography>
        
        <Box display="flex" alignItems="center" gap={2}>
          <FormControl sx={{ minWidth: 220 }}>
            <InputLabel id="work-type-filter-label">Lọc theo loại công trình</InputLabel>
            <Select
              labelId="work-type-filter-label"
              id="work-type-filter"
              value={selectedWorkTypeId}
              onChange={handleWorkTypeChange}
              label="Lọc theo loại công trình"
            >
              <MenuItem value="">Tất cả</MenuItem>
              {workTypesData?.data?.map((workType) => (
                <MenuItem key={workType.id} value={workType.id}>
                  {workType.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          
          <Button variant="contained" onClick={() => handleOpen(null)}>
            Thêm ngành SCImago
          </Button>
        </Box>
      </Box>
      
      <Paper sx={{ width: "100%", overflow: "hidden" }}>
        <GenericTable columns={columns} data={data?.data || []} />
      </Paper>
      
      <ScimagoFieldForm
        open={open}
        handleClose={handleClose}
        data={selectedData}
      />

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa ngành SCImago này?</Typography>
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