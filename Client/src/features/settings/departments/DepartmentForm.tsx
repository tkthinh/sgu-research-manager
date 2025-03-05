import { zodResolver } from "@hookform/resolvers/zod";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
} from "@mui/material";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as z from "zod";
import { createDepartment, updateDepartment } from "../../../lib/api/departmentsApi";
import { Department } from "../../../lib/types/models/Department";

// Define validation schema
const schema = z.object({
  name: z.string().min(2, "Tên phòng ban phải có ít nhất 2 ký tự"),
});

interface DepartmentFormProps {
  open: boolean;
  handleClose: () => void;
  data?: Department | null;
}

export default function DepartmentForm({ open, handleClose, data }: DepartmentFormProps) {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    formState: { errors, isSubmitting },
  } = useForm({
    resolver: zodResolver(schema),
    defaultValues: { name: "" },
  });

  useEffect(() => {
    if (data) {
      setValue("name", data.name);
    } else {
      reset();
    }
  }, [data, open, setValue, reset]);

  // Mutation for create and update operations
  const mutation = useMutation({
    mutationFn: async (formData: Partial<Department>) => {
      if (data?.id) {
        return updateDepartment(data.id, formData);
      } else {
        return createDepartment(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["departments"] });
      toast.success(data?.id ? "Phòng ban đã được cập nhật" : "Phòng ban đã được thêm");
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
    },
  });

  // Handle form submission
  const onSubmit = async (formData: Partial<Department>) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>{data ? "Cập nhật Phòng Ban" : "Thêm Phòng Ban"}</DialogTitle>
      <DialogContent>
        <TextField
          label="Tên phòng ban"
          {...register("name")}
          error={!!errors.name}
          helperText={errors.name?.message}
          fullWidth
          margin="dense"
          disabled={isSubmitting}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isSubmitting}>
          Hủy
        </Button>
        <Button
          onClick={handleSubmit(onSubmit)}
          variant="contained"
          color="primary"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Đang lưu..." : "Lưu"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
