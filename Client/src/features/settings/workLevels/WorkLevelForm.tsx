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
import { useEffect } from "react";
import { useForm, Controller } from "react-hook-form";
import { toast } from "react-toastify";
import * as z from "zod";
import { createWorkLevel, updateWorkLevel } from "../../../lib/api/workLevelsApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import { WorkLevel } from "../../../lib/types/models/WorkLevel";

// Define validation schema
const schema = z.object({
  name: z.string().min(2, "Tên cấp công trình phải có ít nhất 2 ký tự"),
  workTypeId: z.string().uuid("Vui lòng chọn loại công trình"),
});

type WorkLevelFormData = z.infer<typeof schema>;

interface WorkLevelFormProps {
  open: boolean;
  handleClose: () => void;
  data?: WorkLevel | null;
}

export default function WorkLevelForm({ open, handleClose, data }: WorkLevelFormProps) {
  const queryClient = useQueryClient();

  // Fetch work types
  const { data: workTypesData, isLoading: isLoadingWorkTypes } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  const {
    control,
    handleSubmit,
    setValue,
    reset,
    formState: { errors },
  } = useForm<WorkLevelFormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      name: "",
      workTypeId: "",
    },
  });

  useEffect(() => {
    if (data) {
      setValue("name", data.name);
      setValue("workTypeId", data.workTypeId);
    } else {
      reset();
    }
  }, [data, setValue, reset]);

  // Mutation for create and update operations
  const mutation = useMutation({
    mutationFn: async (formData: WorkLevelFormData) => {
      if (data?.id) {
        return updateWorkLevel(data.id, formData);
      } else {
        return createWorkLevel(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workLevels"] });
      toast.success(data?.id ? "Cấp công trình đã được cập nhật" : "Cấp công trình đã được thêm");
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
      toast.error("Đã xảy ra lỗi: " + (error as Error).message);
    },
  });

  // Handle form submission
  const onSubmit = async (formData: WorkLevelFormData) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>{data ? "Cập nhật cấp công trình" : "Thêm cấp công trình"}</DialogTitle>
      <DialogContent>
        <form onSubmit={handleSubmit(onSubmit)}>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12}>
              <Controller
                name="name"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Tên cấp công trình"
                    error={!!errors.name}
                    helperText={errors.name?.message}
                    fullWidth
                    disabled={mutation.isPending}
                  />
                )}
              />
            </Grid>

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
          </Grid>
        </form>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={mutation.isPending}>
          Hủy
        </Button>
        <Button
          onClick={handleSubmit(onSubmit)}
          disabled={mutation.isPending}
          variant="contained"
          color="primary"
        >
          {mutation.isPending ? (
            <CircularProgress size={24} />
          ) : data ? (
            "Cập nhật"
          ) : (
            "Thêm mới"
          )}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
