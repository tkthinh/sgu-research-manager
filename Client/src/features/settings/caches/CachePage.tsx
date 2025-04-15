import {
  Box,
  Button,
  CircularProgress,
  Paper,
  Typography,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import {
  clearCaches,
  deleteCache,
  getCaches,
} from "../../../lib/api/cachesApi";

export default function CachePage() {
  const queryClient = useQueryClient();

  const { data, error, isPending, isSuccess, dataUpdatedAt, refetch } = useQuery({
    queryKey: ["cache-keys"],
    queryFn: getCaches,
  });

  useEffect(() => {
    if (isSuccess && data) {
      toast.success("Tải danh sách cache thành công", {
        toastId: "fetch-caches",
      });
    }
  }, [dataUpdatedAt]);

  useEffect(() => {
    if (error) {
      toast.error("Lỗi khi fetch cache: " + (error as Error).message);
    }
  }, [error]);

  const deleteMutation = useMutation({
    mutationFn: (key: string) => deleteCache(key),
    onSuccess: () => {
      toast.success("Đã xóa cache");
      queryClient.invalidateQueries({ queryKey: ["cache-keys"] });
      refetch();
    },
    onError: (error) => {
      toast.error("Xóa thất bại: " + (error as Error).message);
    },
  });

  const clearAllMutation = useMutation({
    mutationFn: clearCaches,
    onSuccess: () => {
      toast.success("Xóa tất cả cache thành công");
      queryClient.invalidateQueries({ queryKey: ["cache-keys"] });
      refetch();
    },
    onError: (error) => {
      toast.error("Xóa toàn bộ cache thất bại: " + (error as Error).message);
    },
  });

  const columns: GridColDef[] = [
    { field: "key", headerName: "Cache Key", width: 600 },
    {
      field: "actions",
      headerName: "Hành động",
      width: 200,
      renderCell: (params) => (
        <Button
          variant="outlined"
          color="error"
          size="small"
          onClick={() => deleteMutation.mutate(params.row.key)}
          disabled={deleteMutation.isPending}
        >
          Xóa
        </Button>
      ),
    },
  ];

  if (isPending) return <CircularProgress />;
  if (error) return <p>Error: {(error as Error).message}</p>;

  return (
    <>
      <Box display="flex" justifyContent="space-between" mb={2}>
        <Typography variant="h6">Quản lý Cache</Typography>
        <Button
          variant="contained"
          color="error"
          onClick={() => clearAllMutation.mutate()}
          disabled={clearAllMutation.isPending}
        >
          {clearAllMutation.isPending ? "Đang xóa..." : "Xóa tất cả"}
        </Button>
      </Box>
      <Paper sx={{ width: 900, margin: "auto" }}>
        <GenericTable
          columns={columns}
          data={(data?.data || []).map((key) => ({ id: key, key }))}
        />
      </Paper>
    </>
  );
}
