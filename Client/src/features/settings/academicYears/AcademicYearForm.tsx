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
import {
  createAcademicYear,
  updateAcademicYear,
} from "../../../lib/api/academicYearApi";
import { AcademicYear } from "../../../lib/types/models/AcademicYear";

const schema = z
  .object({
    name: z.string().min(2, "Tên năm học phải có ít nhất 2 ký tự"),
    startDate: z.string(),
    endDate: z.string(),
  })
  .refine(
    (data) => new Date(data.endDate) > new Date(data.startDate),
    {
      message: "Ngày kết thúc phải sau ngày bắt đầu",
      path: ["endDate"],
    }
  );

interface AcademicYearFormProps {
  open: boolean;
  handleClose: () => void;
  data?: AcademicYear | null;
}

export default function AcademicYearForm({
  open,
  handleClose,
  data,
}: AcademicYearFormProps) {
  const queryClient = useQueryClient();

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
      startDate: "",
      endDate: "",
    },
  });

  useEffect(() => {
      if (data) {
        setValue("name", data.name);
        setValue("startDate", new Date(data.startDate).toISOString().split('T')[0]);
        setValue("endDate", new Date(data.endDate).toISOString().split('T')[0]);
      } else {
        reset();
      }
    }, [data, open, setValue, reset]);

  const mutation = useMutation({
    mutationFn: async (formData: Partial<AcademicYear>) => {
      if (data?.id) {
        return updateAcademicYear(data.id, formData);
      } else {
        return createAcademicYear(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["academic-years"] });
      toast.success(data?.id ? "Cập nhật thành công" : "Thêm mới thành công");
      handleClose();
    },
    onError: (error) => {
      toast.error("Lỗi khi lưu: " + (error as Error).message);
    },
  });

  const onSubmit = async (data: { name: string; startDate: string; endDate: string }) => {
    await mutation.mutateAsync({
      name: data.name,
      startDate: data.startDate,
      endDate: data.endDate,
    });
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>{data ? "Cập nhật Năm Học" : "Thêm Năm Học"}</DialogTitle>
      <DialogContent>
        <TextField
          label="Tên năm học"
          {...register("name")}
          error={!!errors.name}
          helperText={errors.name?.message}
          fullWidth
          margin="dense"
        />
        <TextField
          label="Ngày bắt đầu"
          type="date"
          {...register("startDate")}
          fullWidth
          margin="dense"
          InputLabelProps={{ shrink: true }}
          error={!!errors.startDate}
          helperText={errors.startDate?.message}
        />
        <TextField
          label="Ngày kết thúc"
          type="date"
          {...register("endDate")}
          fullWidth
          margin="dense"
          InputLabelProps={{ shrink: true }}
          error={!!errors.endDate}
          helperText={errors.endDate?.message}
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
