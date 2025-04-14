import { zodResolver } from "@hookform/resolvers/zod";
import {
  Box,
  Button,
  CircularProgress,
  Divider,
  MenuItem,
  TextField,
  Typography,
} from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as z from "zod";
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { changePassword } from "../../lib/api/authApi";
import { getDepartments } from "../../lib/api/departmentsApi";
import { getFields } from "../../lib/api/fieldsApi";
import { getMyInfo, getUserById, updateUser } from "../../lib/api/usersApi";
import { AcademicTitle } from "../../lib/types/enums/AcademicTitle";
import { OfficerRank } from "../../lib/types/enums/OfficerRank";
import { Department } from "../../lib/types/models/Department";
import { Field } from "../../lib/types/models/Field";
import { User } from "../../lib/types/models/User";
import { getAcademicTitle } from "../../lib/utils/academicTitleMap";
import { getOfficerRank } from "../../lib/utils/officerRankMap";

const schema = z.object({
  fullName: z.string().min(1, "Họ tên không được để trống"),
  email: z.string().email("Email không hợp lệ"),
  phoneNumber: z
    .string()
    .min(1, "Số điện thoại không được để trống")
    .max(10, "Số điện thoại không hợp lệ"),
  specialization: z.string().min(1, "Chuyên ngành không được để trống"),
  departmentId: z.string().min(1, "Đơn vị công tác không được để trống"),
  fieldId: z.string().min(1, "Ngành không được để trống"),
  academicTitle: z.string().min(1, "Học hàm không được để trống"),
  officerRank: z.string().min(1, "Ngạch công chức không được để trống"),
});

const passwordSchema = z
  .object({
    currentPassword: z.string().min(1, "Mật khẩu hiện tại không được để trống"),
    newPassword: z.string().min(1, "Mật khẩu mới không được để trống"),
    confirmNewPassword: z
      .string()
      .min(1, "Xác nhận mật khẩu mới không được để trống"),
  })
  .refine((data) => data.newPassword === data.confirmNewPassword, {
    message: "Mật khẩu mới không khớp",
    path: ["confirmNewPassword"],
  });

export default function UpdateInfoPage() {
  const [userInfo, setUserInfo] = useState<User | null>(null);
  const { setUser, refreshUserInfo } = useAuth();
  const [departments, setDepartments] = useState<Department[]>([]);
  const [fields, setFields] = useState<Field[]>([]);
  const [loading, setLoading] = useState(false);

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<z.infer<typeof schema>>({
    resolver: zodResolver(schema),
  });

  const {
    register: passwordRegister,
    handleSubmit: handlePasswordSubmit,
    reset: passwordReset,
    formState: { errors: passwordErrors, isSubmitting: isPasswordSubmitting },
  } = useForm<z.infer<typeof passwordSchema>>({
    resolver: zodResolver(passwordSchema),
  });

  // Moved useQuery hook to top-level of the component
  const { data: userData, isLoading: isUserLoading } = useQuery({
    queryKey: ["user"],
    queryFn: getMyInfo,
  });

  // Use useEffect to update state when userData changes
  useEffect(() => {
    if (userData?.success && userData.data) {
      setUserInfo(userData.data);
      reset({
        departmentId:
          userData.data.departmentId === "00000000-0000-0000-0000-000000000000"
            ? ""
            : userData.data.departmentId,
        fieldId:
          userData.data.fieldId === "00000000-0000-0000-0000-000000000000"
            ? ""
            : userData.data.fieldId,
        academicTitle:
          userData.data.academicTitle === "Unknown"
            ? ""
            : userData.data.academicTitle,
        officerRank:
          userData.data.officerRank === "Unknown"
            ? ""
            : userData.data.officerRank,
        fullName: userData.data.fullName,
        email: userData.data.email === "-" ? "" : userData.data.email,
        phoneNumber:
          userData.data.phoneNumber === "-" ? "" : userData.data.phoneNumber,
        specialization:
          userData.data.specialization === "-"
            ? ""
            : userData.data.specialization,
      });
    }
    console.log("userData", userData);
  }, [userData, reset]);

  // Load departments and fields
  useEffect(() => {
    async function fetchData() {
      const departmentResponse = await getDepartments();
      setDepartments(departmentResponse.data || []);

      const fieldResponse = await getFields();
      setFields(fieldResponse.data || []);
    }
    fetchData();
  }, []);

  const onSubmit = async (formData: Partial<User>) => {
    try {
      if (!userInfo) return;
      const res = await updateUser(userInfo.id, formData);
      if (res.success) {
        // Fetch updated user
        const updatedRes = await getUserById(userInfo.id);
        const updatedUser = updatedRes.data;

        setUser(updatedUser);
      }
      
      await refreshUserInfo();
      toast.success("Cập nhật hồ sơ thành công");
    } catch (err) {
      toast.error("Lỗi khi cập nhật hồ sơ");
    }
  };

  const onChangePasswordSubmit = async (
    data: z.infer<typeof passwordSchema>,
  ) => {
    try {
      await changePassword(data.currentPassword, data.newPassword);
      toast.success("Đổi mật khẩu thành công");
      passwordReset();
    } catch (err) {
      toast.error("Lỗi khi đổi mật khẩu");
    }
  };

  if (!userInfo || isUserLoading || loading) return <CircularProgress />;

  return (
    <>
      <Typography variant="h4" fontWeight={500} sx={{ my: 3 }}>
        Cập nhật thông tin cá nhân
      </Typography>
      <Box maxWidth="sm">
        <form onSubmit={handleSubmit(onSubmit)} noValidate>
          <TextField
            label="Họ và tên"
            fullWidth
            margin="dense"
            {...register("fullName")}
            error={!!errors.fullName}
            helperText={errors.fullName?.message}
            disabled={isSubmitting}
          />
          <TextField
            label="Email"
            fullWidth
            margin="dense"
            {...register("email")}
            error={!!errors.email}
            helperText={errors.email?.message}
            disabled={isSubmitting}
          />
          <TextField
            label="Số điện thoại"
            fullWidth
            margin="dense"
            {...register("phoneNumber")}
            disabled={isSubmitting}
          />
          <TextField
            label="Chuyên ngành"
            fullWidth
            margin="dense"
            {...register("specialization")}
            disabled={isSubmitting}
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
                <MenuItem value="">Chọn đơn vị</MenuItem>
                {departments.map((dept) => (
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
                <MenuItem value="">Chọn ngành</MenuItem>
                {fields.map((fieldItem) => (
                  <MenuItem key={fieldItem.id} value={fieldItem.id}>
                    {fieldItem.name}
                  </MenuItem>
                ))}
              </TextField>
            )}
          />

          <Controller
            name="academicTitle"
            control={control}
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

          <Button
            variant="contained"
            type="submit"
            fullWidth
            sx={{ mt: 2 }}
            disabled={isSubmitting}
          >
            {isSubmitting ? "Đang tải..." : "Cập nhật thông tin"}
          </Button>
        </form>
      </Box>
      <Divider sx={{ my: 3 }} />
      <Box maxWidth="sm" sx={{ mt: 3 }}>
        <Typography variant="h4" fontWeight={500} sx={{ mb: 2 }}>
          Đổi mật khẩu
        </Typography>
        <form
          onSubmit={handlePasswordSubmit(onChangePasswordSubmit)}
          noValidate
        >
          <TextField
            label="Mật khẩu hiện tại"
            fullWidth
            margin="dense"
            type="password"
            {...passwordRegister("currentPassword")}
            error={!!passwordErrors.currentPassword}
            helperText={passwordErrors.currentPassword?.message}
            disabled={isPasswordSubmitting}
          />
          <TextField
            label="Mật khẩu mới"
            fullWidth
            margin="dense"
            type="password"
            {...passwordRegister("newPassword")}
            error={!!passwordErrors.newPassword}
            helperText={passwordErrors.newPassword?.message}
            disabled={isPasswordSubmitting}
          />
          <TextField
            label="Xác nhận mật khẩu mới"
            fullWidth
            margin="dense"
            type="password"
            {...passwordRegister("confirmNewPassword")}
            error={!!passwordErrors.confirmNewPassword}
            helperText={passwordErrors.confirmNewPassword?.message}
            disabled={isPasswordSubmitting}
          />
          <Button
            variant="contained"
            type="submit"
            fullWidth
            sx={{ mt: 2 }}
            disabled={isPasswordSubmitting}
          >
            {isPasswordSubmitting ? "Đang tải..." : "Đổi mật khẩu"}
          </Button>
        </form>
      </Box>
    </>
  );
}
