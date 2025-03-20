import {
  Box,
  Button,
  Card,
  CardContent,
  CircularProgress,
  Container,
  FormControlLabel,
  Grid,
  Switch,
  TextField,
  Typography,
} from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { vi } from "date-fns/locale";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { format, parseISO } from "date-fns";
import { getSystemConfig, createSystemConfig, updateSystemConfig } from "../../../lib/api/systemConfigApi";
import { SystemConfig, CreateSystemConfigRequest, UpdateSystemConfigRequest } from "../../../lib/types/models/SystemConfig";
import { ApiResponse } from "../../../lib/types/common/ApiResponse";

export default function SystemConfigPage() {
  const queryClient = useQueryClient();
  const [startDate, setStartDate] = useState<Date | null>(null);
  const [endDate, setEndDate] = useState<Date | null>(null);
  const [isClosed, setIsClosed] = useState(false);
  const [hasExistingConfig, setHasExistingConfig] = useState(false);

  // Fetch dữ liệu cấu hình hiện tại
  const { data, isLoading, error, refetch } = useQuery<ApiResponse<SystemConfig>>({
    queryKey: ["systemConfig"],
    queryFn: getSystemConfig
  });

  // Cập nhật state khi có dữ liệu
  useEffect(() => {
    if (data?.success && data.data) {
      setStartDate(parseISO(data.data.startDate));
      setEndDate(parseISO(data.data.endDate));
      setIsClosed(data.data.isClosed);
      setHasExistingConfig(true);
    } else {
      setHasExistingConfig(false);
    }
  }, [data]);

  // Mutation cho việc tạo cấu hình mới
  const createMutation = useMutation({
    mutationFn: (data: CreateSystemConfigRequest) => createSystemConfig(data),
    onSuccess: (data) => {
      if (data.success) {
        toast.success("Tạo cấu hình hệ thống thành công");
        queryClient.invalidateQueries({ queryKey: ["systemConfig"] });
        refetch();
      } else {
        toast.error(data.message || "Có lỗi xảy ra khi tạo cấu hình hệ thống");
      }
    },
    onError: (error: any) => {
      toast.error(`Lỗi: ${error.message || "Không thể tạo cấu hình hệ thống"}`);
    },
  });

  // Mutation cho việc cập nhật cấu hình
  const updateMutation = useMutation({
    mutationFn: (data: UpdateSystemConfigRequest) => updateSystemConfig(data),
    onSuccess: (data) => {
      if (data.success) {
        toast.success("Cập nhật cấu hình hệ thống thành công");
        queryClient.invalidateQueries({ queryKey: ["systemConfig"] });
        refetch();
      } else {
        toast.error(data.message || "Có lỗi xảy ra khi cập nhật cấu hình hệ thống");
      }
    },
    onError: (error: any) => {
      toast.error(`Lỗi: ${error.message || "Không thể cập nhật cấu hình hệ thống"}`);
    },
  });

  // Xử lý submit form
  const handleSubmit = () => {
    if (!startDate || !endDate) {
      toast.error("Vui lòng chọn ngày bắt đầu và ngày kết thúc");
      return;
    }

    if (startDate > endDate) {
      toast.error("Ngày bắt đầu phải trước ngày kết thúc");
      return;
    }

    const formattedStartDate = format(startDate, "yyyy-MM-dd'T'HH:mm:ss");
    const formattedEndDate = format(endDate, "yyyy-MM-dd'T'HH:mm:ss");

    const configData = {
      startDate: formattedStartDate,
      endDate: formattedEndDate,
      isClosed: isClosed,
    };

    if (hasExistingConfig) {
      updateMutation.mutate(configData);
    } else {
      createMutation.mutate(configData);
    }
  };

  // Kiểm tra và hiển thị trạng thái hệ thống
  const getSystemStatus = () => {
    if (!data?.data) return null;

    const config = data.data;
    const now = new Date();
    const start = new Date(config.startDate);
    const end = new Date(config.endDate);

    if (config.isClosed) {
      return { text: "Đóng", color: "error.main" };
    } else if (now < start) {
      return { text: "Chưa mở", color: "warning.main" };
    } else if (now > end) {
      return { text: "Đã kết thúc", color: "error.main" };
    } else {
      return { text: "Đang mở", color: "success.main" };
    }
  };

  const systemStatus = getSystemStatus();

  if (isLoading) return <CircularProgress />;

  return (
    <Container maxWidth="lg">
      {systemStatus && data?.data && (
        <Box mb={4}>
          <Card>
            <CardContent>
              <Typography variant="h5" gutterBottom>
                Trạng thái hệ thống
              </Typography>
              <Typography variant="h6" color={systemStatus.color} fontWeight="bold">
                {systemStatus.text}
              </Typography>
              <Grid container spacing={2} mt={1}>
                <Grid item xs={12} md={6}>
                  <Typography variant="body1">
                    <strong>Ngày bắt đầu:</strong>{" "}
                    {format(new Date(data.data.startDate), "dd/MM/yyyy HH:mm", {
                      locale: vi,
                    })}
                  </Typography>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Typography variant="body1">
                    <strong>Ngày kết thúc:</strong>{" "}
                    {format(new Date(data.data.endDate), "dd/MM/yyyy HH:mm", {
                      locale: vi,
                    })}
                  </Typography>
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </Box>
      )}

      <Card>
        <CardContent>
          <Typography variant="h5" mb={2}>
            {hasExistingConfig ? "Cập nhật cấu hình" : "Tạo cấu hình mới"}
          </Typography>

          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={vi}>
                <DatePicker
                  label="Ngày bắt đầu"
                  value={startDate}
                  onChange={(newValue) => setStartDate(newValue)}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      variant: "outlined",
                    },
                  }}
                />
              </LocalizationProvider>
            </Grid>

            <Grid item xs={12} md={6}>
              <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={vi}>
                <DatePicker
                  label="Ngày kết thúc"
                  value={endDate}
                  onChange={(newValue) => setEndDate(newValue)}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      variant: "outlined",
                    },
                  }}
                />
              </LocalizationProvider>
            </Grid>

            <Grid item xs={12}>
              <FormControlLabel
                control={
                  <Switch
                    checked={isClosed}
                    onChange={(e) => setIsClosed(e.target.checked)}
                    color="primary"
                  />
                }
                label="Đóng hệ thống (không cho phép kê khai)"
              />
            </Grid>

            <Grid item xs={12}>
              <Button
                variant="contained"
                color="primary"
                onClick={handleSubmit}
                disabled={createMutation.isPending || updateMutation.isPending}
              >
                {createMutation.isPending || updateMutation.isPending ? (
                  <CircularProgress size={24} />
                ) : hasExistingConfig ? (
                  "Cập nhật"
                ) : (
                  "Tạo mới"
                )}
              </Button>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
    </Container>
  );
} 