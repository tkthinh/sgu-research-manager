import { zodResolver } from "@hookform/resolvers/zod";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Link,
  TextField,
  Typography,
} from "@mui/material";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useNavigation } from "react-router-dom";
import { toast } from "react-toastify";
import * as z from "zod";
import { createWorkType, updateWorkType } from "../../../lib/api/workTypesApi";
import { WorkType } from "../../../lib/types/models/WorkType";

// Define validation schema
const schema = z.object({
  name: z.string().min(2, "Tên loại công trình phải có ít nhất 2 ký tự"),
});

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
  const navigation = useNavigation();

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
    mutationFn: async (formData: Partial<WorkType>) => {
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
    },
  });

  // Handle form submission
  const onSubmit = async (formData: Partial<WorkType>) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>
        {data ? "Cập nhật Loại Công trình" : "Thêm Loại Công trình"}
      </DialogTitle>
      <DialogContent>
        <TextField
          label="Tên loại công trình"
          {...register("name")}
          error={!!errors.name}
          helperText={errors.name?.message}
          fullWidth
          margin="dense"
          disabled={isSubmitting}
        />
      </DialogContent>
      {data && (
        <>
          <Typography
            fontSize={12}
            fontStyle={"italic"}
            sx={{ px: 4, pb: 2, fontWeight: "300" }}
          >
            - Chỉnh sửa tên công trình chỉ ảnh hưởng đến tên hiển thị
          </Typography>
          <Typography
            fontSize={12}
            fontStyle={"italic"}
            sx={{ px: 4, pb: 2, fontWeight: "300" }}
          >
            - Các cấp công trình thuộc loại sẽ không bị ảnh hưởng. Để chỉnh sửa
            các cấp công trình, truy cập mục
            <Link href="/he-thong/cap-cong-trinh"> Cấp công trình</Link>
          </Typography>
        </>
      )}
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
