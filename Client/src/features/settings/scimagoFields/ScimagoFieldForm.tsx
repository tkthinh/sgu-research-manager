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
import { createScimagoField, updateScimagoField } from "../../../lib/api/scimagoFieldsApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import { ScimagoField } from "../../../lib/types/models/ScimagoField";

// Define validation schema
const schema = z.object({
  name: z.string().min(2, "Tên ngành SCImago phải có ít nhất 2 ký tự"),
  workTypeId: z.string().uuid("Vui lòng chọn loại công trình"),
});

interface ScimagoFieldFormProps {
  open: boolean;
  handleClose: () => void;
  data?: ScimagoField | null;
}

export default function ScimagoFieldForm({ open, handleClose, data }: ScimagoFieldFormProps) {
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
    mutationFn: async (formData: Partial<ScimagoField>) => {
      if (data?.id) {
        return updateScimagoField(data.id, formData);
      } else {
        return createScimagoField(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["scimagoFields"] });
      toast.success(data?.id ? "Ngành SCImago đã được cập nhật" : "Ngành SCImago đã được thêm");
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
      toast.error("Đã xảy ra lỗi: " + (error as Error).message);
    },
  });

  // Handle form submission
  const onSubmit = async (formData: Partial<ScimagoField>) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>{data ? "Cập nhật Ngành SCImago" : "Thêm Ngành SCImago"}</DialogTitle>
      <DialogContent>
        <TextField
          label="Tên ngành SCImago"
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