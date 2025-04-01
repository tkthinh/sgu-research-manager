import { zodResolver } from "@hookform/resolvers/zod";
import {
  Button,
  Checkbox,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControlLabel,
  MenuItem,
  TextField,
} from "@mui/material";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as z from "zod";
import { getDepartments } from "../../../lib/api/departmentsApi";
import { getFields } from "../../../lib/api/fieldsApi";
import { adminUpdateUser } from "../../../lib/api/usersApi";
import { AcademicTitle } from "../../../lib/types/enums/AcademicTitle";
import { OfficerRank } from "../../../lib/types/enums/OfficerRank";
import { Role } from "../../../lib/types/enums/Role";
import { Department } from "../../../lib/types/models/Department";
import { Field } from "../../../lib/types/models/Field";
import { User } from "../../../lib/types/models/User";
import { getAcademicTitle } from "../../../lib/utils/academicTitleMap";
import { getOfficerRank } from "../../../lib/utils/officerRankMap";
import { getRole } from "../../../lib/utils/roleMap";

// Define validation schema (userName is shown but not editable)
const schema = z.object({
  userName: z.string().nonempty("Mã số viên chức không được để trống"),
  email: z.string().email("Email không hợp lệ"),
  phoneNumber: z.string().nonempty("Số điện thoại không được để trống"),
  fullName: z.string().nonempty("Họ và tên không được để trống"),
  academicTitle: z.string().nonempty("Học hàm không được để trống"),
  officerRank: z.string().nonempty("Ngạch công chức không được để trống"),
  departmentId: z.string().optional(),
  fieldId: z.string().optional(),
  specialization: z.string().optional(),
  role: z.string().nonempty("Quyền hạn không được để trống"),
  isApproved: z.boolean(),
});

interface UserFormProps {
  open: boolean;
  handleClose: () => void;
  data?: User | null;
}

export default function UserForm({ open, handleClose, data }: UserFormProps) {
  const queryClient = useQueryClient();

  // State for select options
  const [departments, setDepartments] = useState<Department[]>([]);
  const [fields, setFields] = useState<Field[]>([]);

  useEffect(() => {
    async function fetchData() {
      const departmentResponse = await getDepartments();
      setDepartments(departmentResponse.data || []);

      const fieldResponse = await getFields();
      setFields(fieldResponse.data || []);
    }
    fetchData();
  }, []);

  const {
    register,
    handleSubmit,
    setValue,
    reset,
    control,
    formState: { errors, isSubmitting },
  } = useForm({
    resolver: zodResolver(schema),
    defaultValues: {
      userName: "",
      email: "",
      phoneNumber: "",
      fullName: "",
      academicTitle: "",
      officerRank: "",
      departmentId: "",
      fieldId: "",
      specialization: "",
      role: "",
      isApproved: false,
    },
  });

  useEffect(() => {
    if (data) {
      setValue("userName", data.userName || "");
      setValue("email", data.email || "");
      setValue("phoneNumber", data.phoneNumber || "");
      setValue("fullName", data.fullName || "");
      setValue("academicTitle", data.academicTitle ?? "");
      setValue("officerRank", data.officerRank ?? "");
      setValue("departmentId", data.departmentId ?? "");
      setValue("fieldId", data.fieldId ?? "");
      setValue("specialization", data.specialization || "");
      setValue("role", data.role ?? "");
      setValue("isApproved", data.isApproved ?? false);
    } else {
      reset();
    }
  }, [data, open, setValue, reset]);

  // Mutation for update operation (creation is not allowed)
  const mutation = useMutation({
    mutationFn: async (formData: Partial<User>) => {
      if (data?.id) {
        console.log("Updating user with data:", formData);
        return adminUpdateUser(data.id, formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["users"] });
      toast.success("Người dùng đã được cập nhật");
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
      toast.error("Lỗi khi cập nhật người dùng: " + (error as Error).message);
    },
  });

  const onSubmit = async (formData: Partial<User>) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose} fullWidth>
      <DialogTitle>Cập nhật Người Dùng</DialogTitle>
      <DialogContent>
        {/* userName is shown but disabled */}
        <TextField
          label="Mã số viên chức"
          {...register("userName")}
          error={!!errors.userName}
          helperText={errors.userName?.message}
          fullWidth
          margin="dense"
          disabled
        />
        <TextField
          label="Email"
          {...register("email")}
          error={!!errors.email}
          helperText={errors.email?.message}
          fullWidth
          margin="dense"
          disabled={isSubmitting}
        />
        <TextField
          label="Số điện thoại"
          {...register("phoneNumber")}
          error={!!errors.phoneNumber}
          helperText={errors.phoneNumber?.message}
          fullWidth
          margin="dense"
          disabled={isSubmitting}
        />
        <TextField
          label="Họ và tên"
          {...register("fullName")}
          error={!!errors.fullName}
          helperText={errors.fullName?.message}
          fullWidth
          margin="dense"
          disabled={isSubmitting}
        />
        <Controller
          name="academicTitle"
          control={control}
          defaultValue={data?.academicTitle ?? ""}
          render={({ field }) => (
            <TextField
              select
              label="Học hàm"
              {...field}
              error={!!errors.academicTitle}
              helperText={errors.academicTitle?.message}
              fullWidth
              margin="dense"
              disabled={isSubmitting}
            >
              {Object.entries(AcademicTitle)
                .filter(([key]) => isNaN(Number(key)))
                .map(([key]) => (
                  <MenuItem key={key} value={key}>
                    {getAcademicTitle(key)}
                  </MenuItem>
                ))}
            </TextField>
          )}
        />
        <Controller
          name="officerRank"
          control={control}
          render={({ field }) => (
            <TextField
              select
              label="Ngạch công chức"
              {...field}
              error={!!errors.officerRank}
              helperText={errors.officerRank?.message}
              fullWidth
              margin="dense"
              disabled={isSubmitting}
            >
              {Object.entries(OfficerRank)
                .filter(([key]) => isNaN(Number(key)))
                .map(([key]) => (
                  <MenuItem key={key} value={key}>
                    {getOfficerRank(key)}
                  </MenuItem>
                ))}
            </TextField>
          )}
        />
        <Controller
          name="departmentId"
          control={control}
          render={({ field }) => (
            <TextField
              select
              label="Đơn vị công tác"
              {...field}
              error={!!errors.departmentId}
              helperText={errors.departmentId?.message}
              fullWidth
              margin="dense"
              disabled={isSubmitting}
            >
              <MenuItem value="">-- Chọn đơn vị --</MenuItem>
              {departments.map((dept: any) => (
                <MenuItem key={dept.id} value={dept.id}>
                  {dept.name}
                </MenuItem>
              ))}
            </TextField>
          )}
        />
        <Controller
          name="fieldId"
          control={control}
          render={({ field }) => (
            <TextField
              select
              label="Ngành"
              {...field}
              error={!!errors.fieldId}
              helperText={errors.fieldId?.message}
              fullWidth
              margin="dense"
              disabled={isSubmitting}
            >
              <MenuItem value="">-- Chọn ngành --</MenuItem>
              {fields.map((fieldItem: any) => (
                <MenuItem key={fieldItem.id} value={fieldItem.id}>
                  {fieldItem.name}
                </MenuItem>
              ))}
            </TextField>
          )}
        />
        <TextField
          label="Chuyên ngành"
          {...register("specialization")}
          error={!!errors.specialization}
          helperText={errors.specialization?.message}
          fullWidth
          margin="dense"
          disabled={isSubmitting}
        />
        <Controller
          name="role"
          control={control}
          render={({ field }) => (
            <TextField
              select
              label="Quyền hạn"
              {...field}
              error={!!errors.role}
              helperText={errors.role?.message}
              fullWidth
              margin="dense"
              disabled={isSubmitting}
            >
              {Object.entries(Role)
                .filter(([key]) => isNaN(Number(key)))
                .map(([key]) => (
                  <MenuItem key={key} value={key}>
                    {getRole(key)}
                  </MenuItem>
                ))}
            </TextField>
          )}
        />
        <Controller
          name="isApproved"
          control={control}
          render={({ field }) => (
            <FormControlLabel
              control={
                <Checkbox
                  {...field}
                  checked={field.value}
                  onChange={(e) => field.onChange(e.target.checked)}
                  color="primary"
                  disabled={isSubmitting}
                />
              }
              label="Cho phép truy cập hệ thống"
            />
          )}
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
