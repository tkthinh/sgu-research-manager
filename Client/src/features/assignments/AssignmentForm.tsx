import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  MenuItem,
  TextField,
} from "@mui/material";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useEffect } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-toastify";
import { getDepartments } from "../../lib/api/departmentsApi";
import { Assignment } from "../../lib/types/models/Assignment";
import { createAssignment, updateAssignment } from "../../lib/api/assignmentApi";

const schema = z.object({
  managerId: z.string().min(1),
  departmentId: z.string().min(1, "Vui lòng chọn đơn vị được phân công"),
});

interface Props {
  open: boolean;
  handleClose: () => void;
  data?: Assignment | null;
}

export default function AssignmentForm({ open, handleClose, data }: Props) {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    control,
    reset,
    formState: { errors, isSubmitting },
  } = useForm({
    resolver: zodResolver(schema),
    defaultValues: {
      managerId: "",
      departmentId: "",
    },
  });

  const { data: departmentData } = useQuery({
    queryKey: ["departments"],
    queryFn: getDepartments,
  });

  useEffect(() => {
    if (data) {
      reset({
        ...data,
        departmentId:
          data.departmentId === "00000000-0000-0000-0000-000000000000"
            ? ""
            : data.departmentId,
      });
    } else {
      reset();
    }
  }, [data, reset]);

  const mutation = useMutation({
    mutationFn: async (formData: Partial<Assignment>) => {
      const isCreate =
        !data?.id || 
        data.id === "00000000-0000-0000-0000-000000000000" || 
        /^temp-/.test(data.id);
  
      return isCreate
        ? createAssignment(formData)
        : updateAssignment(data.id, formData);
    },
    onSuccess: () => {
      toast.success("Cập nhật phân công thành công");
      queryClient.invalidateQueries({ queryKey: ["assignments"] });
      handleClose();
    },
    onError: (error) => {
      toast.error("Lỗi khi lưu phân công: " + (error as Error).message);
    },
  });

  const onSubmit = async (formData: Partial<Assignment>) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Phân công đơn vị</DialogTitle>
      <DialogContent>
        <TextField
          label="Họ và tên quản lý"
          value={data?.managerFullName || ""}
          fullWidth
          margin="dense"
          disabled
        />
        <TextField
          label="Đơn vị công tác"
          value={data?.managerDepartmentName || ""}
          fullWidth
          margin="dense"
          disabled
        />

        {/* Hidden Manager ID */}
        <input type="hidden" {...register("managerId")} />

        {/* Select Assigned Department */}
        <Controller
          name="departmentId"
          control={control}
          render={({ field }) => (
            <TextField
              select
              label="Đơn vị được phân công"
              {...field}
              error={!!errors.departmentId}
              helperText={errors.departmentId?.message}
              fullWidth
              margin="dense"
              disabled={isSubmitting}
            >
              <MenuItem value="">-- Chọn đơn vị --</MenuItem>
              {departmentData?.data?.map((dep: any) => (
                <MenuItem key={dep.id} value={dep.id}>
                  {dep.name}
                </MenuItem>
              ))}
            </TextField>
          )}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isSubmitting}>Đóng</Button>
        <Button onClick={handleSubmit(onSubmit)} variant="contained" disabled={isSubmitting}>
          {isSubmitting ? "Đang lưu..." : "Lưu"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
