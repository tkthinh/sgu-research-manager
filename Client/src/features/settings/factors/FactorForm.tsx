import { zodResolver } from "@hookform/resolvers/zod";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  MenuItem,
  CircularProgress,
  Grid,
} from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { useForm, Controller } from "react-hook-form";
import { toast } from "react-toastify";
import * as z from "zod";
import { createFactor, updateFactor } from "../../../lib/api/factorsApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import { getWorkLevelsByWorkTypeId } from "../../../lib/api/workLevelsApi";
import { getPurposesByWorkTypeId } from "../../../lib/api/purposesApi";
import { getAuthorRolesByWorkTypeId } from "../../../lib/api/authorRolesApi";
import { Factor } from "../../../lib/types/models/Factor";
import { ScoreLevel } from "../../../lib/types/enums/ScoreLevel";

// Define validation schema
const schema = z.object({
  workTypeId: z.string().uuid("Vui lòng chọn loại công trình"),
  workLevelId: z.string().uuid("Vui lòng chọn cấp công trình").optional().nullable(),
  purposeId: z.string().uuid("Vui lòng chọn mục đích quy đổi"),
  authorRoleId: z.string().uuid("Vui lòng chọn vai trò tác giả").optional().nullable(),
  scoreLevel: z.number().int().min(1, "Vui lòng chọn mức điểm"),
  convertHour: z.number().min(0, "Giờ quy đổi phải là số dương"),
  maxAllowed: z.number().min(0, "Số tối đa phải là số dương"),
});

type FactorFormData = z.infer<typeof schema>;

interface FactorFormProps {
  open: boolean;
  handleClose: () => void;
  data?: Factor | null;
}

export default function FactorForm({ open, handleClose, data }: FactorFormProps) {
  const queryClient = useQueryClient();
  const [selectedWorkTypeId, setSelectedWorkTypeId] = useState<string>("");

  // Fetch work types
  const { data: workTypesData, isLoading: isLoadingWorkTypes } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  // Fetch dependent data based on workTypeId
  const { data: workLevelsData, isLoading: isLoadingWorkLevels } = useQuery({
    queryKey: ["workLevels", selectedWorkTypeId],
    queryFn: () => getWorkLevelsByWorkTypeId(selectedWorkTypeId),
    enabled: !!selectedWorkTypeId,
  });

  const { data: purposesData, isLoading: isLoadingPurposes } = useQuery({
    queryKey: ["purposes", selectedWorkTypeId],
    queryFn: () => getPurposesByWorkTypeId(selectedWorkTypeId),
    enabled: !!selectedWorkTypeId,
  });

  const { data: authorRolesData, isLoading: isLoadingAuthorRoles } = useQuery({
    queryKey: ["authorRoles", selectedWorkTypeId],
    queryFn: () => getAuthorRolesByWorkTypeId(selectedWorkTypeId),
    enabled: !!selectedWorkTypeId,
  });

  const {
    control,
    handleSubmit,
    setValue,
    reset,
    watch,
    formState: { errors },
  } = useForm<FactorFormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      workTypeId: "",
      workLevelId: null,
      purposeId: "",
      authorRoleId: null,
      scoreLevel: ScoreLevel.One,
      convertHour: 0,
      maxAllowed: 0,
    },
  });

  // Watch workTypeId to update dependent fields
  const watchWorkTypeId = watch("workTypeId");

  useEffect(() => {
    if (watchWorkTypeId !== selectedWorkTypeId) {
      setSelectedWorkTypeId(watchWorkTypeId);
    }
  }, [watchWorkTypeId, selectedWorkTypeId]);

  useEffect(() => {
    if (data) {
      setValue("workTypeId", data.workTypeId);
      setValue("workLevelId", data.workLevelId);
      setValue("purposeId", data.purposeId);
      setValue("authorRoleId", data.authorRoleId || null);
      setValue("scoreLevel", data.scoreLevel);
      setValue("convertHour", data.convertHour);
      setValue("maxAllowed", data.maxAllowed);
      setSelectedWorkTypeId(data.workTypeId);
    } else {
      reset();
    }
  }, [data, setValue, reset]);

  // Reset dependent fields when workTypeId changes
  useEffect(() => {
    if (selectedWorkTypeId && !data) {
      setValue("workLevelId", null);
      setValue("purposeId", "");
      setValue("authorRoleId", null);
    }
  }, [selectedWorkTypeId, setValue, data]);

  // Mutation for create and update operations
  const mutation = useMutation({
    mutationFn: async (formData: FactorFormData) => {
      // Convert workLevelId and authorRoleId từ null thành undefined nếu cần
      const payload: Partial<Factor> = {
        ...formData,
        workLevelId: formData.workLevelId || undefined,
        authorRoleId: formData.authorRoleId || undefined
      };

      if (data?.id) {
        return updateFactor(data.id, payload);
      } else {
        return createFactor(payload);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["factors"] });
      toast.success(data?.id ? "Hệ số quy đổi đã được cập nhật" : "Hệ số quy đổi đã được thêm");
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
      toast.error("Đã xảy ra lỗi: " + (error as Error).message);
    },
  });

  // Handle form submission
  const onSubmit = async (formData: FactorFormData) => {
    await mutation.mutateAsync(formData);
  };

  // Get score level text
  const getScoreLevelText = (level: number) => {
    switch (level) {
      case ScoreLevel.One:
        return "1 điểm";
      case ScoreLevel.ZeroPointSevenFive:
        return "0.75 điểm";
      case ScoreLevel.ZeroPointFive:
        return "0.5 điểm";
      case ScoreLevel.TenPercent:
        return "Top 10%";
      case ScoreLevel.ThirtyPercent:
        return "Top 30%";
      case ScoreLevel.FiftyPercent:
        return "Top 50%";
      case ScoreLevel.HundredPercent:
        return "Top 100%";
      default:
        return "Không xác định";
    }
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
      <DialogTitle>{data ? "Cập nhật Hệ số quy đổi" : "Thêm Hệ số quy đổi"}</DialogTitle>
      <DialogContent>
        <Grid container spacing={2} sx={{ mt: 1 }}>
          <Grid item xs={12}>
            <Controller
              name="workTypeId"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  label="Loại công trình"
                  error={!!errors.workTypeId}
                  helperText={errors.workTypeId?.message}
                  fullWidth
                  disabled={mutation.isPending || isLoadingWorkTypes}
                >
                  {isLoadingWorkTypes ? (
                    <MenuItem disabled>Đang tải...</MenuItem>
                  ) : (
                    workTypesData?.data?.map((workType) => (
                      <MenuItem key={workType.id} value={workType.id}>
                        {workType.name}
                      </MenuItem>
                    ))
                  )}
                </TextField>
              )}
            />
          </Grid>

          <Grid item xs={12} sm={6}>
            <Controller
              name="workLevelId"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  label="Cấp công trình"
                  error={!!errors.workLevelId}
                  helperText={errors.workLevelId?.message}
                  fullWidth
                  disabled={!selectedWorkTypeId || mutation.isPending || isLoadingWorkLevels}
                  value={field.value || ""}
                >
                  <MenuItem value="">Không chọn</MenuItem>
                  {isLoadingWorkLevels ? (
                    <MenuItem disabled>Đang tải...</MenuItem>
                  ) : (
                    workLevelsData?.data?.map((workLevel) => (
                      <MenuItem key={workLevel.id} value={workLevel.id}>
                        {workLevel.name}
                      </MenuItem>
                    ))
                  )}
                </TextField>
              )}
            />
          </Grid>

          <Grid item xs={12} sm={6}>
            <Controller
              name="purposeId"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  label="Mục đích quy đổi"
                  error={!!errors.purposeId}
                  helperText={errors.purposeId?.message}
                  fullWidth
                  disabled={!selectedWorkTypeId || mutation.isPending || isLoadingPurposes}
                >
                  {isLoadingPurposes ? (
                    <MenuItem disabled>Đang tải...</MenuItem>
                  ) : (
                    purposesData?.data?.map((purpose) => (
                      <MenuItem key={purpose.id} value={purpose.id}>
                        {purpose.name}
                      </MenuItem>
                    ))
                  )}
                </TextField>
              )}
            />
          </Grid>

          <Grid item xs={12} sm={6}>
            <Controller
              name="authorRoleId"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  label="Vai trò tác giả"
                  error={!!errors.authorRoleId}
                  helperText={errors.authorRoleId?.message}
                  fullWidth
                  disabled={!selectedWorkTypeId || mutation.isPending || isLoadingAuthorRoles}
                  value={field.value || ""}
                >
                  <MenuItem value="">Không chọn</MenuItem>
                  {isLoadingAuthorRoles ? (
                    <MenuItem disabled>Đang tải...</MenuItem>
                  ) : (
                    authorRolesData?.data?.map((authorRole) => (
                      <MenuItem key={authorRole.id} value={authorRole.id}>
                        {authorRole.name}
                      </MenuItem>
                    ))
                  )}
                </TextField>
              )}
            />
          </Grid>

          <Grid item xs={12} sm={6}>
            <Controller
              name="scoreLevel"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  label="Mức điểm"
                  error={!!errors.scoreLevel}
                  helperText={errors.scoreLevel?.message}
                  fullWidth
                  disabled={mutation.isPending}
                  onChange={(e) => field.onChange(Number(e.target.value))}
                  value={field.value}
                >
                  <MenuItem value={ScoreLevel.One}>{getScoreLevelText(ScoreLevel.One)}</MenuItem>
                  <MenuItem value={ScoreLevel.ZeroPointSevenFive}>{getScoreLevelText(ScoreLevel.ZeroPointSevenFive)}</MenuItem>
                  <MenuItem value={ScoreLevel.ZeroPointFive}>{getScoreLevelText(ScoreLevel.ZeroPointFive)}</MenuItem>
                  <MenuItem value={ScoreLevel.TenPercent}>{getScoreLevelText(ScoreLevel.TenPercent)}</MenuItem>
                  <MenuItem value={ScoreLevel.ThirtyPercent}>{getScoreLevelText(ScoreLevel.ThirtyPercent)}</MenuItem>
                  <MenuItem value={ScoreLevel.FiftyPercent}>{getScoreLevelText(ScoreLevel.FiftyPercent)}</MenuItem>
                  <MenuItem value={ScoreLevel.HundredPercent}>{getScoreLevelText(ScoreLevel.HundredPercent)}</MenuItem>
                </TextField>
              )}
            />
          </Grid>

          <Grid item xs={12} sm={6}>
            <Controller
              name="convertHour"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Giờ quy đổi"
                  type="number"
                  error={!!errors.convertHour}
                  helperText={errors.convertHour?.message}
                  fullWidth
                  disabled={mutation.isPending}
                  inputProps={{ step: 0.5, min: 0 }}
                  onChange={(e) => field.onChange(Number(e.target.value))}
                />
              )}
            />
          </Grid>

          <Grid item xs={12} sm={6}>
            <Controller
              name="maxAllowed"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Số công trình tối đa được phép quy đổi"
                  type="number"
                  error={!!errors.maxAllowed}
                  helperText={errors.maxAllowed?.message}
                  fullWidth
                  disabled={mutation.isPending}
                  inputProps={{ step: 1, min: 0 }}
                  onChange={(e) => field.onChange(Number(e.target.value))}
                />
              )}
            />
          </Grid>
        </Grid>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={mutation.isPending}>
          Hủy
        </Button>
        <Button
          onClick={handleSubmit(onSubmit)}
          variant="contained"
          color="primary"
          disabled={mutation.isPending || isLoadingWorkTypes || (!!selectedWorkTypeId && (isLoadingWorkLevels || isLoadingPurposes || isLoadingAuthorRoles))}
        >
          {mutation.isPending ? <CircularProgress size={24} /> : (data ? "Cập nhật" : "Thêm mới")}
        </Button>
      </DialogActions>
    </Dialog>
  );
} 