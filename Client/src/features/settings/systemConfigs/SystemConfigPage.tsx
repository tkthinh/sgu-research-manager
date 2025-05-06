import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Paper,
  Stack,
  Typography,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useMemo, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import { getCurrentAcademicYear } from "../../../lib/api/academicYearApi";
import {
  deleteSystemConfig,
  getSystemConfigByAcademicYearId,
  notifySystemConfig,
} from "../../../lib/api/systemConfigApi";
import { AcademicYear } from "../../../lib/types/models/AcademicYear";
import { SystemConfig } from "../../../lib/types/models/SystemConfig";
import { formatDateTime } from "../../../lib/utils/dateTimeFormatter";
import { isSystemOpen } from "../../../lib/utils/systemCheck";
import SystemConfigForm from "./SystemConfigForm";

export default function SystemConfigPage() {
  const queryClient = useQueryClient();

  const [currentAcademicYear, setCurrentAcademicYear] =
    useState<AcademicYear | null>(null);

  useEffect(() => {
    const fetchCurrentAcademicYear = async () => {
      const response = await getCurrentAcademicYear();
      if (response && response.data) {
        setCurrentAcademicYear(response.data);
      }
    };
    fetchCurrentAcademicYear();
  }, []);

  const { data, error, isPending, isSuccess, dataUpdatedAt, refetch } =
    useQuery({
      queryKey: ["system-configs"],
      queryFn: () =>
        getSystemConfigByAcademicYearId(currentAcademicYear?.id || ""),
      enabled: !!currentAcademicYear?.id,
    });

  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message, { toastId: "fetch-system-configs-success" });
    }
  }, [dataUpdatedAt]);

  useEffect(() => {
    if (error) {
      toast.error("Có lỗi xảy ra: " + (error as Error).message);
    }
  }, [error]);

  const isAnySystemOpen = useMemo(() => {
    if (!data || !data.data) return false;
    return data.data.some((config) => isSystemOpen(config));
  }, [data]);

  const [open, setOpen] = useState(false);
  const [selectedData, setSelectedData] = useState(null);

  const handleOpen = (data) => {
    setSelectedData(data);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    refetch();
  };

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
    mutationFn: (id: string) => deleteSystemConfig(id),
    onSuccess: () => {
      toast.success("Xóa cấu hình thành công!");
      queryClient.invalidateQueries({ queryKey: ["system-configs"] });
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

  //Notify system config
  const [notifyDialogOpen, setNotifyDialogOpen] = useState(false);
  const [notifyTarget, setNotifyTarget] = useState<SystemConfig | null>(null);

  const handleNotifyClick = (data: SystemConfig) => {
    setNotifyTarget(data);
    setNotifyDialogOpen(true);
  };

  const handleNotifyCancel = () => {
    setNotifyDialogOpen(false);
    setNotifyTarget(null);
  };

  const notifyMutation = useMutation({
    mutationFn: () => notifySystemConfig(notifyTarget!, notifyTarget?.id!),
    onSuccess: () => {
      toast.success("Đã tạo thông báo thành công.");
      setNotifyDialogOpen(false);
      setNotifyTarget(null);
      queryClient.invalidateQueries({ queryKey: ["system-configs"] });
      refetch();
    },
    onError: (error) => {
      toast.error("Lỗi khi tạo thông báo: " + (error as Error).message);
    },
  });

  const columns: GridColDef[] = [
    { field: "name", headerName: "Tên cấu hình", width: 200 },
    { field: "academicYearName", headerName: "Năm học", width: 200 },
    {
      field: "openTime",
      headerName: "Thời gian mở",
      width: 200,
      valueFormatter: (params) => formatDateTime(params),
    },
    {
      field: "closeTime",
      headerName: "Thời gian đóng",
      width: 200,
      valueFormatter: (params) => formatDateTime(params),
    },
    {
      field: "isNotified",
      headerName: "Đã thông báo",
      type: "boolean",
      width: 120,
    },
    {
      field: "actions",
      headerName: "Hành động",
      width: 400,
      renderCell: (params) => {
        const isNotified = params.row.isNotified;

        return (
          <Box>
            <Button
              variant="contained"
              color="primary"
              size="small"
              onClick={() => handleOpen(params.row)}
              sx={{ marginRight: 1 }}
              disabled={isNotified}
            >
              Sửa
            </Button>
            <Button
              variant="contained"
              color="error"
              size="small"
              onClick={() => handleDeleteClick(params.row.id)}
              sx={{ marginRight: 1 }}
              disabled={isNotified}
            >
              Xóa
            </Button>
            <Button
              variant="contained"
              color="secondary"
              size="small"
              onClick={() => handleNotifyClick(params.row)}
              disabled={isNotified}
            >
              Thông báo
            </Button>
          </Box>
        );
      },
    },
  ];

  if (isPending) return <CircularProgress />;
  if (error) return <p>Error: {(error as Error).message}</p>;

  return (
    <>
      <Stack
        direction={{ xs: "column", sm: "row" }}
        justifyContent="space-between"
        alignItems={{ xs: "flex-start", sm: "center" }}
        spacing={2}
        sx={{ mb: 2 }}
      >
        <Typography variant="body1">
          Năm học hiện tại:{" "}
          <Typography
            component="span"
            variant="body1"
            sx={{ fontWeight: "bold" }}
            color="primary"
          >
            {currentAcademicYear?.name}
          </Typography>
        </Typography>

        <Button variant="contained" onClick={() => handleOpen(null)}>
          Thêm cấu hình
        </Button>
      </Stack>

      {/* Bảng dữ liệu */}
      <Paper
        sx={{ width: { xs: "100%", sm: 1010 }, mx: "auto", overflow: "hidden" }}
      >
        <GenericTable columns={columns} data={data?.data || []} />
      </Paper>

      {/* Trạng thái hệ thống */}
      {isAnySystemOpen !== undefined && (
        <Stack my={4}>
          <Typography fontSize={24} variant="body1">
            Trạng thái hệ thống:{" "}
            <Typography
              component="span"
              fontSize={24}
              fontWeight={500}
              color={isAnySystemOpen ? "primary" : "error"}
            >
              {isAnySystemOpen ? "Đang mở" : "Đang đóng"}
            </Typography>
          </Typography>
        </Stack>
      )}

      {/* Form thêm / sửa cấu hình */}
      <SystemConfigForm
        open={open}
        handleClose={handleClose}
        data={selectedData}
      />

      {/* Delete dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa cấu hình này?</Typography>
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

      {/* Notify dialog */}
      <Dialog open={notifyDialogOpen} onClose={handleNotifyCancel}>
        <DialogTitle>Xác nhận gửi thông báo</DialogTitle>
        <DialogContent>
          <Typography>
            Sau khi thông báo, cấu hình này sẽ không thể chỉnh sửa hoặc xóa. Bạn
            có chắc chắn muốn tiếp tục?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleNotifyCancel}>Hủy</Button>
          <Button
            variant="contained"
            color="secondary"
            onClick={() => {
              if (notifyTarget) {
                notifyMutation.mutate();
              }
            }}
          >
            {notifyMutation.isPending ? "Đang gửi..." : "Thông báo"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
