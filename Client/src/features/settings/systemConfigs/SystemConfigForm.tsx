import { zodResolver } from "@hookform/resolvers/zod";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Divider,
  MenuItem,
  TextField,
  Typography,
} from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import { z } from "zod";
import { getAcademicYears, getCurrentAcademicYear } from "../../../lib/api/academicYearApi";
import {
  createSystemConfig,
  updateSystemConfig,
} from "../../../lib/api/systemConfigApi";
import { SystemConfig } from "../../../lib/types/models/SystemConfig";
import { DateTime } from "luxon";
import { AcademicYear } from "../../../lib/types/models/AcademicYear";

interface SystemConfigFormProps {
  open: boolean;
  handleClose: () => void;
  data?: SystemConfig | null;
}

const schema = z
  .object({
    name: z.string().min(2, "Tên cấu hình là bắt buộc"),
    openTime: z.string().min(1, "Thời gian mở là bắt buộc"),
    openDate: z.string().min(1, "Ngày mở là bắt buộc"),
    closeTime: z.string().min(1, "Thời gian đóng là bắt buộc"),
    closeDate: z.string().min(1, "Ngày đóng là bắt buộc"),
    // We still define academicYearId for validation but we're going to force it later.
    academicYearId: z.string(),
  })
  .refine(
    (data) => {
      const mergedOpenDateTime = new Date(`${data.openDate}T${data.openTime}`);
      const mergedCloseDateTime = new Date(`${data.closeDate}T${data.closeTime}`);
      return mergedCloseDateTime > mergedOpenDateTime;
    },
    {
      message: "Thời gian kết thúc phải sau Thời gian bắt đầu",
      path: ["closeDate"],
    },
  );

export default function SystemConfigForm({
  open,
  handleClose,
  data,
}: SystemConfigFormProps) {
  const queryClient = useQueryClient();

  const { data: academicYears, isLoading: isLoadingAcademicYears } = useQuery({
    queryKey: ["academic-years"],
    queryFn: getAcademicYears,
  });

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    control,
    formState: { errors, isSubmitting },
  } = useForm({
    resolver: zodResolver(schema),
    defaultValues: {
      name: "",
      openTime: "",
      closeTime: "",
      academicYearId: "",
    },
  });

  const [currentAcademicYear, setCurrentAcademicYear] =
    useState<AcademicYear | null>(null);

  // Fetch the current academic year
  useEffect(() => {
    const fetchCurrentAcademicYear = async () => {
      const response = await getCurrentAcademicYear();
      if (response && response.data) {
        setCurrentAcademicYear(response.data);
        // Force the form's academicYearId to currentAcademicYear.id when fetched.
        setValue("academicYearId", response.data.id);
      }
    };
    fetchCurrentAcademicYear();
  }, [setValue]);

  // Setup form values on load or when editing
  useEffect(() => {
    if (data) {
      // Parse the ISO strings directly without timezone conversion
      const dtOpen = DateTime.fromISO(data.openTime);
      const dtClose = DateTime.fromISO(data.closeTime);
  
      // Set the form fields using formatted values.
      setValue("name", data.name);
      setValue("openDate", dtOpen.toFormat("yyyy-LL-dd")); // e.g., "2025-04-01"
      setValue("openTime", dtOpen.toFormat("HH:mm")); // e.g., "17:00"
      setValue("closeDate", dtClose.toFormat("yyyy-LL-dd"));
      setValue("closeTime", dtClose.toFormat("HH:mm"));
      // Even in edit mode, force academicYearId to currentAcademicYear if available.
      setValue("academicYearId", currentAcademicYear?.id ?? data.academicYearId);
    } else {
      reset();
      // Also force academicYearId on reset if currentAcademicYear is available.
      if (currentAcademicYear) {
        setValue("academicYearId", currentAcademicYear.id);
      }
    }
  }, [data, open, setValue, reset, currentAcademicYear]);

  const mutation = useMutation({
    mutationFn: async (formData: Partial<SystemConfig>) => {
      if (data?.id) {
        return updateSystemConfig(data.id, formData);
      } else {
        return createSystemConfig(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["system-configs"] });
      toast.success(data?.id ? "Cập nhật thành công" : "Thêm mới thành công");
      handleClose();
    },
    onError: (error) => {
      toast.error("Lỗi khi lưu: " + (error as Error).message);
    },
  });

  const onSubmit = async (formData: {
    name: string;
    openTime: string;
    openDate: string;
    closeTime: string;
    closeDate: string;
    academicYearId: string;
  }) => {
    // Override academicYearId to ensure we always use currentAcademicYear.id.
    const forcedAcademicYearId = currentAcademicYear?.id;
    if (!forcedAcademicYearId) {
      toast.error("Không tìm thấy năm học hiện tại, vui lòng thử lại sau.");
      return;
    }
    await mutation.mutateAsync({
      name: formData.name,
      openTime: toISOStringWithOffset(formData.openDate, formData.openTime),
      closeTime: toISOStringWithOffset(formData.closeDate, formData.closeTime),
      academicYearId: forcedAcademicYearId,
    });
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>{data ? "Cập nhật Cấu hình" : "Thêm Cấu hình"}</DialogTitle>
      <DialogContent>
        <TextField
          label="Tên Cấu hình"
          {...register("name")}
          error={!!errors.name}
          helperText={errors.name?.message}
          fullWidth
          margin="dense"
        />
        <Divider sx={{ marginY: 2 }} />
        <Box display="flex" gap={2} alignItems={"center"} marginBottom={2}>
          <Typography minWidth={120}>Thời gian mở</Typography>
          <Controller
            control={control}
            name="openTime"
            render={({ field }) => (
              <TextField
                {...field}
                type="time"
                error={!!errors.openTime}
                helperText={errors.openTime?.message}
                fullWidth
                margin="dense"
              />
            )}
          />
          <Controller
            control={control}
            name="openDate"
            render={({ field }) => (
              <TextField
                {...field}
                type="date"
                error={!!errors.openDate}
                helperText={errors.openDate?.message}
                fullWidth
                margin="dense"
              />
            )}
          />
        </Box>
        <Box display="flex" gap={2} alignItems={"center"} marginBottom={2}>
          <Typography minWidth={120}>Thời gian đóng</Typography>
          <Controller
            control={control}
            name="closeTime"
            render={({ field }) => (
              <TextField
                {...field}
                type="time"
                error={!!errors.closeTime}
                helperText={errors.closeTime?.message}
                fullWidth
                margin="dense"
              />
            )}
          />
          <Controller
            control={control}
            name="closeDate"
            render={({ field }) => (
              <TextField
                {...field}
                type="date"
                error={!!errors.closeDate}
                helperText={errors.closeDate?.message}
                fullWidth
                margin="dense"
              />
            )}
          />
        </Box>
        <Divider sx={{ marginY: 2 }} />
        <Controller
          control={control}
          name="academicYearId"
          render={({ field }) => (
            <TextField
              {...field}
              select
              label="Năm học"
              error={!!errors.academicYearId}
              helperText={errors.academicYearId?.message}
              fullWidth
              disabled
            >
              {isLoadingAcademicYears ? (
                <MenuItem disabled>Đang tải...</MenuItem>
              ) : (
                academicYears?.data?.map((year) => (
                  <MenuItem key={year.id} value={year.id}>
                    {year.name}
                  </MenuItem>
                ))
              )}
            </TextField>
          )}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isSubmitting}>
          Hủy
        </Button>
        <Button
          onClick={handleSubmit(onSubmit)}
          variant="contained"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Đang lưu..." : "Lưu"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}

const toISOStringWithOffset = (dateString: string, timeString: string): string => {
  // Combine the date and time and return as ISO string
  return DateTime.fromFormat(`${dateString} ${timeString}`, "yyyy-LL-dd HH:mm")
    .toISO();
};
