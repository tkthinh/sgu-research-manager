import { zodResolver } from "@hookform/resolvers/zod";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Typography,
  CircularProgress,
  Grid,
} from "@mui/material";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useEffect } from "react";
import { useForm, Controller } from "react-hook-form";
import { toast } from "react-toastify";
import * as z from "zod";
import { createWorkType, updateWorkType } from "../../../lib/api/workTypesApi";
import { WorkType } from "../../../lib/types/models/WorkType";

// Define validation schema
const schema = z.object({
  name: z.string().min(2, "Tên loại công trình phải có ít nhất 2 ký tự"),
});

type WorkTypeFormData = z.infer<typeof schema>;

interface WorkTypeFormProps {
  open: boolean;
  handleClose: () => void;
  data?: WorkType | null;
}

export default function WorkTypeForm({
  open,
  handleClose,
  data,
}: WorkTypeFormProps) {
  const queryClient = useQueryClient();

  const {
    control,
    handleSubmit,
    setValue,
    reset,
    formState: { errors },
  } = useForm<WorkTypeFormData>({
    resolver: zodResolver(schema),
    defaultValues: { name: "" },
  });

  useEffect(() => {
    if (data) {
      setValue("name", data.name);
    } else {
      reset();
    }
  }, [data, setValue, reset]);

  // Mutation for create and update operations
  const mutation = useMutation({
    mutationFn: async (formData: WorkTypeFormData) => {
      if (data?.id) {
        return updateWorkType(data.id, formData);
      } else {
        return createWorkType(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workTypes"] });
      toast.success(
        data?.id
          ? "Loại công trình đã được cập nhật"
          : "Loại công trình đã được thêm",
      );
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
      toast.error("Đã xảy ra lỗi: " + (error as Error).message);
    },
  });

  // Handle form submission
  const onSubmit = async (formData: WorkTypeFormData) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>
        {data ? "Cập nhật loại công trình" : "Thêm loại công trình"}
      </DialogTitle>
      <DialogContent>
        <Grid container spacing={2} sx={{ mt: 1 }}>
          <Grid item xs={12}>
            <Controller
              name="name"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Tên loại công trình"
                  error={!!errors.name}
                  helperText={errors.name?.message}
                  fullWidth
                  disabled={mutation.isPending}
                />
              )}
            />
          </Grid>
          
          {data && (
            <Grid item xs={12}>
              <Typography
                fontSize={12}
                fontStyle={"italic"}
                sx={{ fontWeight: "300" }}
              >
                - Chỉnh sửa tên loại công trình chỉ ảnh hưởng đến tên hiển thị
              </Typography>
              <Typography
                fontSize={12}
                fontStyle={"italic"}
                sx={{ fontWeight: "300" }}
              >
                - Các cấp công trình, mục đích, và vai trò tác giả thuộc loại này không bị ảnh hưởng
              </Typography>
            </Grid>
          )}
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
          disabled={mutation.isPending}
        >
          {mutation.isPending ? <CircularProgress size={24} /> : (data ? "Cập nhật" : "Thêm mới")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
