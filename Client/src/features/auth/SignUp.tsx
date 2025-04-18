import { zodResolver } from "@hookform/resolvers/zod"; 
import { Visibility, VisibilityOff } from "@mui/icons-material";
import {
  Alert,
  Button,
  CircularProgress,
  CssBaseline,
  Divider,
  FormControl,
  FormLabel,
  IconButton,
  InputAdornment,
  InputLabel,
  LinearProgress,
  Link,
  MenuItem,
  Select,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import MuiCard from "@mui/material/Card";
import { styled } from "@mui/material/styles";
import React, { useEffect, useState } from "react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import * as z from "zod";

// API imports
import { signUp } from "../../lib/api/authApi";
import { getDepartments } from "../../lib/api/departmentsApi";
import { getFields } from "../../lib/api/fieldsApi";
import { SignUpRequest } from "../../lib/types/common/Auth";
import { AcademicTitle } from "../../lib/types/enums/AcademicTitle";
import { OfficerRank } from "../../lib/types/enums/OfficerRank";
import { Department } from "../../lib/types/models/Department";
import { Field } from "../../lib/types/models/Field";
import { getAcademicTitle } from "../../lib/utils/academicTitleMap";
import { getOfficerRank } from "../../lib/utils/officerRankMap";

// Styled components
const Card = styled(MuiCard)(({ theme }) => ({
  display: "flex",
  flexDirection: "column",
  alignSelf: "center",
  width: "100%",
  padding: theme.spacing(6),
  gap: theme.spacing(2),
  margin: "auto",
  boxShadow: "0px 5px 15px rgba(0,0,0,0.05)",
  [theme.breakpoints.up("sm")]: {
    width: "450px",
  },
}));

const SignUpContainer = styled(Stack)(({ theme }) => ({
  padding: theme.spacing(2),
  [theme.breakpoints.up("sm")]: {
    padding: theme.spacing(4),
  },
  "&::before": {
    content: '""',
    display: "block",
    position: "absolute",
    zIndex: -1,
    inset: 0,
    backgroundImage:
      "radial-gradient(ellipse at 50% 50%, hsl(210, 100%, 97%), hsl(0, 0%, 100%))",
    backgroundRepeat: "no-repeat",
    ...theme.applyStyles("dark", {
      backgroundImage:
        "radial-gradient(at 50% 50%, hsla(210, 100%, 16%, 0.5), hsl(220, 30%, 5%))",
    }),
  },
}));

// Form schemas

// Update username validation: exactly 9 numeric characters.
const stepOneSchema = z
  .object({
    username: z
      .string()
      .regex(/^\d{9}$/, "Mã số giảng viên phải gồm 9 số"),
    password: z.string().min(6, "Mật khẩu phải có ít nhất 6 ký tự"),
    confirmPassword: z.string().min(6, "Vui lòng xác nhận mật khẩu"),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Mật khẩu không khớp",
    path: ["confirmPassword"],
  });

// Added phoneNumber and specialization fields to step two.
const stepTwoSchema = z.object({
  email: z.string().email("Email không hợp lệ"),
  fullname: z.string().min(2, "Họ và tên là bắt buộc"),
  phoneNumber: z.string().regex(/^(0|\+84)(3[2-9]|5[689]|7[06-9]|8[1-9]|9[0-9])\d{7}$/ , "Vui lòng nhập số điện thoại"),
  specialization: z.string().min(1, "Chuyên ngành là bắt buộc"),
  academicTitle: z.string().min(1, "Vui lòng chọn học hàm / học vị"),
  officerRank: z.string().min(1, "Vui lòng chọn ngạch viên chức"),
  departmentId: z.string().min(1, "Vui lòng chọn phòng ban"),
  fieldId: z.string().min(1, "Vui lòng chọn lĩnh vực"),
});

type StepOneFormValues = z.infer<typeof stepOneSchema>;
type StepTwoFormValues = z.infer<typeof stepTwoSchema>;

const SignUp: React.FC = () => {
  const [step, setStep] = useState<1 | 2>(1);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [fields, setFields] = useState<Field[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  // Calculate progress based on current step
  const progress = step === 1 ? 0 : 50;

  // Setup form for step one
  const stepOneForm = useForm<StepOneFormValues>({
    resolver: zodResolver(stepOneSchema),
    defaultValues: {
      username: "",
      password: "",
      confirmPassword: "",
    },
  });

  // Setup form for step two (with additional fields)
  const stepTwoForm = useForm<StepTwoFormValues>({
    resolver: zodResolver(stepTwoSchema),
    defaultValues: {
      email: "",
      fullname: "",
      phoneNumber: "",
      specialization: "",
      academicTitle: "",
      officerRank: "",
      departmentId: "",
      fieldId: "",
    },
  });

  useEffect(() => {
    async function fetchData() {
      const departmentResponse = await getDepartments();
      setDepartments(departmentResponse.data || []);

      const fieldResponse = await getFields();
      setFields(fieldResponse.data || []);
    }
    fetchData();
  }, []);

  const handleStepOneSubmit: SubmitHandler<StepOneFormValues> = () => {
    setStep(2);
  };

  const handleStepTwoSubmit: SubmitHandler<StepTwoFormValues> = async (data) => {
    setLoading(true);
    setError(null);

    try {
      // Combine data from both forms
      const stepOneData = stepOneForm.getValues();

      const signUpData: SignUpRequest = {
        username: stepOneData.username,
        password: stepOneData.password,
        email: data.email,
        fullname: data.fullname,
        phoneNumber: data.phoneNumber,
        specialization: data.specialization,
        academicTitle: data.academicTitle || "",
        officerRank: data.officerRank || "",
        departmentId: data.departmentId,
        fieldId: data.fieldId,
      };

      const response = await signUp(signUpData);

      if (response.success) {
        // Redirect to login page or show success message
        window.location.href = "/dang-nhap?registered=true";
      } else {
        setError(response.message || "Đăng ký không thành công. Vui lòng thử lại.");
      }
    } catch (err) {
      setError("Có lỗi xảy ra. Vui lòng thử lại sau.");
    } finally {
      setLoading(false);
    }
  };

  const handleBack = () => {
    setStep(1);
  };

  const handleTogglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const handleToggleConfirmPasswordVisibility = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  return (
    <>
      <CssBaseline enableColorScheme />
      <SignUpContainer
        direction="column"
        justifyContent="space-between"
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          gap: 2,
          minHeight: "100vh",
        }}
      >
        <Card variant="outlined">
          <img src="/logo.png" height={36} width={36} alt="logo" />
          <Typography component="h1" variant="h4" fontWeight={600}>
            Đăng ký
          </Typography>
          <LinearProgress
            variant="determinate"
            value={progress}
            sx={{ width: "100%", maxWidth: 450 }}
          />

          {error && <Alert severity="error">{error}</Alert>}

          {step === 1 ? (
            <form onSubmit={stepOneForm.handleSubmit(handleStepOneSubmit)}>
              <Stack spacing={3} sx={{ mt: 2 }}>
                <Controller
                  key="username"
                  name="username"
                  control={stepOneForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <FormLabel htmlFor="username">Mã số giảng viên</FormLabel>
                      <TextField
                        {...field}
                        id="username"
                        placeholder="Nhập mã số giảng viên"
                        fullWidth
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                      />
                    </FormControl>
                  )}
                />

                <Controller
                  key="password"
                  name="password"
                  control={stepOneForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <FormLabel htmlFor="password">Mật khẩu</FormLabel>
                      <TextField
                        {...field}
                        id="password"
                        placeholder="Nhập mật khẩu"
                        type={showPassword ? "text" : "password"}
                        fullWidth
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                        InputProps={{
                          endAdornment: (
                            <InputAdornment position="end">
                              <IconButton onClick={handleTogglePasswordVisibility} edge="end">
                                {showPassword ? <VisibilityOff /> : <Visibility />}
                              </IconButton>
                            </InputAdornment>
                          ),
                        }}
                      />
                    </FormControl>
                  )}
                />

                <Controller
                  key="confirmPassword"
                  name="confirmPassword"
                  control={stepOneForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <FormLabel htmlFor="confirmPassword">
                        Xác nhận mật khẩu
                      </FormLabel>
                      <TextField
                        {...field}
                        id="confirmPassword"
                        placeholder="Nhập lại mật khẩu"
                        type={showConfirmPassword ? "text" : "password"}
                        fullWidth
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                        InputProps={{
                          endAdornment: (
                            <InputAdornment position="end">
                              <IconButton onClick={handleToggleConfirmPasswordVisibility} edge="end">
                                {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                              </IconButton>
                            </InputAdornment>
                          ),
                        }}
                      />
                    </FormControl>
                  )}
                />

                <Button
                  type="submit"
                  variant="contained"
                  color="primary"
                  fullWidth
                  disabled={loading}
                >
                  Tiếp tục
                </Button>
              </Stack>
            </form>
          ) : (
            <form onSubmit={stepTwoForm.handleSubmit(handleStepTwoSubmit)}>
              <Stack spacing={3} sx={{ mt: 2 }}>
                <Controller
                  key="fullname"
                  name="fullname"
                  control={stepTwoForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <FormLabel htmlFor="fullname">Họ và tên</FormLabel>
                      <TextField
                        {...field}
                        id="fullname"
                        placeholder="Nhập họ và tên"
                        fullWidth
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                      />
                    </FormControl>
                  )}
                />
                <Controller
                  key="email"
                  name="email"
                  control={stepTwoForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <FormLabel htmlFor="email">Email</FormLabel>
                      <TextField
                        {...field}
                        id="email"
                        placeholder="Nhập email"
                        type="email"
                        fullWidth
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                      />
                    </FormControl>
                  )}
                />
                <Controller
                  key="phoneNumber"
                  name="phoneNumber"
                  control={stepTwoForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <FormLabel htmlFor="phoneNumber">SĐT</FormLabel>
                      <TextField
                        {...field}
                        id="phoneNumber"
                        placeholder="Nhập số điện thoại"
                        fullWidth
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                      />
                    </FormControl>
                  )}
                />
                <Controller
                  key="specialization"
                  name="specialization"
                  control={stepTwoForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <FormLabel htmlFor="specialization">Chuyên ngành</FormLabel>
                      <TextField
                        {...field}
                        id="specialization"
                        placeholder="Nhập chuyên ngành"
                        fullWidth
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                      />
                    </FormControl>
                  )}
                />

                <Controller
                  key="fieldId"
                  name="fieldId"
                  control={stepTwoForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <InputLabel>Ngành</InputLabel>
                      <Select {...field} label="Ngành">
                        {fields.map((item) => (
                          <MenuItem key={item.id} value={item.id}>
                            {item.name}
                          </MenuItem>
                        ))}
                      </Select>
                      {fieldState.error && (
                        <Typography color="error" variant="caption">
                          {fieldState.error.message}
                        </Typography>
                      )}
                    </FormControl>
                  )}
                />

                <Controller
                  key="departmentId"
                  name="departmentId"
                  control={stepTwoForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <InputLabel>Đơn vị công tác</InputLabel>
                      <Select {...field} label="Đơn vị công tác">
                        {departments.map((dept) => (
                          <MenuItem key={dept.id} value={dept.id}>
                            {dept.name}
                          </MenuItem>
                        ))}
                      </Select>
                      {fieldState.error && (
                        <Typography color="error" variant="caption">
                          {fieldState.error.message}
                        </Typography>
                      )}
                    </FormControl>
                  )}
                />

                <Controller
                  key="academicTitle"
                  name="academicTitle"
                  control={stepTwoForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <InputLabel>Học hàm / học vị</InputLabel>
                      <Select {...field} label="Học hàm / học vị">
                        {Object.entries(AcademicTitle)
                          .filter(([key]) => isNaN(Number(key)))
                          .map(([key]) => (
                            <MenuItem key={key} value={key}>
                              {getAcademicTitle(key)}
                            </MenuItem>
                          ))}
                      </Select>
                      {fieldState.error && (
                        <Typography color="error" variant="caption">
                          {fieldState.error.message}
                        </Typography>
                      )}
                    </FormControl>
                  )}
                />

                <Controller
                  key="officerRank"
                  name="officerRank"
                  control={stepTwoForm.control}
                  render={({ field, fieldState }) => (
                    <FormControl fullWidth error={!!fieldState.error}>
                      <InputLabel>Ngạch viên chức</InputLabel>
                      <Select {...field} label="Ngạch viên chức">
                        {Object.entries(OfficerRank)
                          .filter(([key]) => isNaN(Number(key)))
                          .map(([key]) => (
                            <MenuItem key={key} value={key}>
                              {getOfficerRank(key)}
                            </MenuItem>
                          ))}
                      </Select>
                      {fieldState.error && (
                        <Typography color="error" variant="caption">
                          {fieldState.error.message}
                        </Typography>
                      )}
                    </FormControl>
                  )}
                />

                <Stack direction="row" spacing={2}>
                  <Button
                    variant="outlined"
                    onClick={handleBack}
                    fullWidth
                    disabled={loading}
                  >
                    Quay lại
                  </Button>
                  <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                    fullWidth
                    disabled={loading}
                  >
                    {loading ? <CircularProgress size={24} /> : "Đăng ký"}
                  </Button>
                </Stack>
              </Stack>
            </form>
          )}

          <Divider sx={{ marginY: 2 }} />
          <Typography sx={{ textAlign: "center" }}>
            Đã có tài khoản?{" "}
            <Link
              href="/dang-nhap"
              variant="body2"
              sx={{ alignSelf: "center" }}
            >
              Đăng nhập
            </Link>
          </Typography>
        </Card>
      </SignUpContainer>
    </>
  );
};

export default SignUp;
