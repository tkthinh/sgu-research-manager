import { zodResolver } from "@hookform/resolvers/zod";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Alert, IconButton, InputAdornment } from "@mui/material";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import MuiCard from "@mui/material/Card";
import CircularProgress from "@mui/material/CircularProgress";
import CssBaseline from "@mui/material/CssBaseline";
import Divider from "@mui/material/Divider";
import FormControl from "@mui/material/FormControl";
import FormLabel from "@mui/material/FormLabel";
import Link from "@mui/material/Link";
import Stack from "@mui/material/Stack";
import { styled } from "@mui/material/styles";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import { jwtDecode } from "jwt-decode";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { useLocation, useNavigate } from "react-router-dom";
import * as z from "zod";
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { signIn } from "../../lib/api/authApi";
import { User } from "../../lib/types/models/User";

// Define Zod schema
const schema = z.object({
  username: z
    .string()
    .min(1, "Mã số giảng viên là bắt buộc")
    .max(9, "Mã số giảng viên không hợp lệ"),
  password: z.string().min(6, "Mật khẩu phải có ít nhất 6 ký tự"),
});

type FormData = z.infer<typeof schema>;

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

const SignInContainer = styled(Stack)(({ theme }) => ({
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

export default function SignIn() {
  // Initialize react-hook-form with zod schema
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    mode: "onTouched",
    shouldUnregister: true,
  });

  const { setUser } = useAuth();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showPassword, setShowPassword] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  // Check if URL has ?registered=true and show success alert if it does.
  const searchParams = new URLSearchParams(location.search);
  const registeredSuccess = searchParams.get("registered") === "true";

  const onSubmit = async (data: FormData) => {
    setLoading(true);
    setError(null); // Optional: clear previous errors

    try {
      const response = await signIn({
        username: data.username,
        password: data.password,
      });

      if (response.success) {
        const token = response.data.token;
        const user: User = response.data.user;
        user.role =
          jwtDecode(token)[
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
          ];

        localStorage.setItem("token", token);
        localStorage.setItem("user", JSON.stringify(user));
        localStorage.setItem("expiration", response.data.expiration);
        setUser(user);
        navigate("/");
      }
    } catch (error: any) {
      if (error.response?.data?.message) {
        setError(error.response.data.message);
      } else {
        setError("Đăng nhập thất bại: " + error.message);
      }
    } finally {
      setLoading(false);
    }
  };

  const handleTogglePasswordVisibility = () => {
    setShowPassword((prev) => !prev);
  };

  return (
    <>
      <CssBaseline enableColorScheme />
      <SignInContainer
        direction="column"
        justifyContent="space-between"
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          gap: 2,
        }}
      >
        <Card variant="outlined">
          <img src="/logo.png" height={36} width={36} alt="logo" />

          <Typography component="h1" variant="h4" fontWeight={600}>
            Đăng nhập
          </Typography>

          {/* Success Alert for registration */}
          {registeredSuccess && (
            <Alert severity="success">Đăng ký thành công</Alert>
          )}

          {error && <Alert severity="error">{error}</Alert>}

          <FormControl fullWidth key="username">
            <FormLabel htmlFor="username">Mã số giảng viên</FormLabel>
            <TextField
              fullWidth
              id="username"
              placeholder="Nhập mã số giảng viên"
              {...register("username")}
              error={!!errors.username}
              helperText={errors.username?.message}
            />
          </FormControl>

          <FormControl fullWidth key="password">
            <FormLabel htmlFor="password">Mật khẩu</FormLabel>
            <TextField
              fullWidth
              id="password"
              placeholder="Nhập mật khẩu"
              type={showPassword ? "text" : "password"}
              {...register("password")}
              error={!!errors.password}
              helperText={errors.password?.message}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      onClick={handleTogglePasswordVisibility}
                      edge="end"
                      aria-label="toggle password visibility"
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />
          </FormControl>

          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              gap: 2,
              marginTop: 3,
            }}
          >
            <Button
              type="submit"
              fullWidth
              variant="contained"
              onClick={handleSubmit(onSubmit)}
              disabled={loading}
            >
              {loading ? <CircularProgress size={24} /> : "Đăng nhập"}
            </Button>
          </Box>

          <Divider sx={{ marginY: 2 }} />
          <Typography sx={{ textAlign: "center" }}>
            Chưa có tài khoản?{" "}
            <Link href="/dang-ky" variant="body2" sx={{ alignSelf: "center" }}>
              Đăng ký
            </Link>
          </Typography>
        </Card>
      </SignInContainer>
    </>
  );
}
