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
} from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as z from "zod";
import { createPurpose, updatePurpose } from "../../../lib/api/purposesApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import { Purpose } from "../../../lib/types/models/Purpose";

// Define validation schema
const schema = z.object({
  name: z.string().min(2, "Tên mục đích quy đổi phải có ít nhất 2 ký tự"),
  workTypeId: z.string().uuid("Vui lòng chọn loại công trình"),
});

interface PurposeFormProps {
  open: boolean;
  handleClose: () => void;
  data?: Purpose | null;
}

export default function PurposeForm({ open, handleClose, data }: PurposeFormProps) {
  const queryClient = useQueryClient();

  // Fetch work types
  const { data: workTypesData, isLoading: isLoadingWorkTypes } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    formState: { errors, isSubmitting },
  } = useForm({
    resolver: zodResolver(schema),
    defaultValues: { 
      name: "",
      workTypeId: "",
    },
  });

  useEffect(() => {
    if (data) {
      setValue("name", data.name);
      setValue("workTypeId", data.workTypeId || "");
    } else {
      reset();
    }
  }, [data, open, setValue, reset]);

  // Mutation for create and update operations
  const mutation = useMutation({
    mutationFn: async (formData: Partial<Purpose>) => {
      if (data?.id) {
        return updatePurpose(data.id, formData);
      } else {
        return createPurpose(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["purposes"] });
      toast.success(data?.id ? "Mục đích quy đổi đã được cập nhật" : "Mục đích quy đổi đã được thêm");
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
      toast.error("Đã xảy ra lỗi: " + (error as Error).message);
    },
  });

  // Handle form submission
  const onSubmit = async (formData: Partial<Purpose>) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>{data ? "Cập nhật Mục đích quy đổi" : "Thêm Mục đích quy đổi"}</DialogTitle>
      <DialogContent>
        <TextField
          label="Tên mục đích quy đổi"
          {...register("name")}
          error={!!errors.name}
          helperText={errors.name?.message}
          fullWidth
          margin="dense"
          disabled={isSubmitting}
        />

        <TextField
          select
          label="Loại công trình"
          {...register("workTypeId")}
          error={!!errors.workTypeId}
          helperText={errors.workTypeId?.message}
          fullWidth
          margin="dense"
          disabled={isSubmitting || isLoadingWorkTypes}
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
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isSubmitting}>
          Hủy
        </Button>
        <Button
          onClick={handleSubmit(onSubmit)}
          variant="contained"
          color="primary"
          disabled={isSubmitting || isLoadingWorkTypes}
        >
          {isSubmitting ? <CircularProgress size={24} /> : (data ? "Cập nhật" : "Thêm mới")}
        </Button>
      </DialogActions>
    </Dialog>
  );
} 