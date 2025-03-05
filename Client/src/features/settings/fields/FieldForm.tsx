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
import { createField, updateField } from "../../../lib/api/fieldsApi";
import { Field } from "../../../lib/types/models/Field";

// Define validation schema
const schema = z.object({
  name: z.string().min(2, "Tên ngành phải có ít nhất 2 ký tự"),
});

interface FieldFormProps {
  open: boolean;
  handleClose: () => void;
  data?: Field | null;
}

export default function FieldForm({ open, handleClose, data }: FieldFormProps) {
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
    mutationFn: async (formData: Partial<Field>) => {
      if (data?.id) {
        return updateField(data.id, formData);
      } else {
        return createField(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fields"] });
      toast.success(data?.id ? "Ngành đã được cập nhật" : "Ngành đã được thêm");
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
    },
  });

  // Handle form submission
  const onSubmit = async (formData: Partial<Field>) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>{data ? "Cập nhật Ngành" : "Thêm Ngành"}</DialogTitle>
      <DialogContent>
        <TextField
          label="Tên ngành"
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
