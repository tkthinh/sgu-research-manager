import {
  Box,
  Button,
  CircularProgress,
  Container,
  FormControl,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Typography
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { getDepartments, getDepartmentsByManagerId } from "../../lib/api/departmentsApi";
import { getUsersByDepartmentId } from "../../lib/api/usersApi";
import { Department } from "../../lib/types/models/Department";
import RemoveRedEyeIcon from "@mui/icons-material/RemoveRedEye";
import { useAuth } from "../../app/shared/contexts/AuthContext";

export default function WorkScorePage() {
  const navigate = useNavigate();
  const [selectedDepartmentId, setSelectedDepartmentId] = useState<string>("");
  const { user } = useAuth();
  
  // Lấy danh sách phòng ban dựa trên role
  const {
    data: departmentsData,
    isLoading: isLoadingDepartments,
    error: departmentsError
  } = useQuery({
    queryKey: ["departments", user?.role, user?.id],
    queryFn: async () => {
      if (user?.role === "Manager") {
        return await getDepartmentsByManagerId(user.id);
      }
      return await getDepartments();
    },
    enabled: !!user // Chỉ gọi API khi đã có thông tin user
  });

  // Lấy danh sách người dùng theo phòng ban
  const {
    data: usersData,
    isLoading: isLoadingUsers,
    error: usersError
  } = useQuery({
    queryKey: ["users", "department", selectedDepartmentId],
    queryFn: () => getUsersByDepartmentId(selectedDepartmentId),
    enabled: !!selectedDepartmentId // Chỉ gọi API khi đã chọn phòng ban
  });

  // Xử lý khi thay đổi phòng ban
  const handleDepartmentChange = (event: any) => {
    setSelectedDepartmentId(event.target.value);
  };

  // Xử lý khi nhấn nút xem công trình
  const handleViewWorks = (userId: string) => {
    navigate(`/cham-diem/user/${userId}`);
  };

  // Định nghĩa các cột cho bảng người dùng
  const columns: GridColDef[] = [
    {
      field: "userName",
      headerName: "Mã số viên chức",
      width: 150,
    },
    {
      field: "fullName",
      headerName: "Họ và tên",
      width: 250,
    },
    {
      field: "departmentName",
      headerName: "Đơn vị công tác",
      width: 250,
    },
    {
      field: "fieldName",
      headerName: "Ngành",
      width: 200,
      renderCell: (params) => {
        return <div>{params.row.fieldName || "Chưa phân ngành"}</div>;
      },
    },
    {
      field: "actions",
      headerName: "Thao tác",
      width: 170,
      renderCell: (params) => (
        <Button
          variant="contained"
          color="primary"
          size="small"
          onClick={() => handleViewWorks(params.row.id)}
          startIcon={<RemoveRedEyeIcon />}
        >
          Xem công trình
        </Button>
      ),
    },
  ];

  // Hiển thị trạng thái loading
  if (isLoadingDepartments) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" height="50vh">
        <CircularProgress />
      </Box>
    );
  }

  // Hiển thị lỗi nếu có
  if (departmentsError) {
    return (
      <Box p={3}>
        <Typography color="error">
          Lỗi khi tải danh sách phòng ban: {(departmentsError as Error).message}
        </Typography>
      </Box>
    );
  }

  const departments: Department[] = departmentsData?.data || [];

  return (
    <Container maxWidth="xl">
      <Box sx={{ mb: 4 }}>
        <Typography variant="h5" gutterBottom>
          Danh sách người dùng theo phòng ban
        </Typography>

        <FormControl fullWidth sx={{ mt: 2 }}>
          <InputLabel id="department-select-label">Chọn phòng ban</InputLabel>
          <Select
            labelId="department-select-label"
            id="department-select"
            value={selectedDepartmentId}
            label="Chọn phòng ban"
            onChange={handleDepartmentChange}
          >
            <MenuItem value="">
              <em>Chọn phòng ban</em>
            </MenuItem>
            {departments.map((department) => (
              <MenuItem key={department.id} value={department.id}>
                {department.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Box>

      {selectedDepartmentId ? (
        isLoadingUsers ? (
          <Box display="flex" justifyContent="center" py={4}>
            <CircularProgress />
          </Box>
        ) : usersError ? (
          <Typography color="error">
            Lỗi khi tải danh sách người dùng: {(usersError as Error).message}
          </Typography>
        ) : (
          <>
            <Typography variant="h6" mb={2}>
              Danh sách người dùng thuộc phòng ban
            </Typography>
            <Paper sx={{ width: "100%", overflow: "hidden" }}>
              <Box sx={{ height: 500 }}>
                <GenericTable
                  columns={columns}
                  data={usersData?.data || []}
                />
              </Box>
            </Paper>
          </>
        )
      ) : (
        <Typography variant="subtitle1" color="text.secondary" textAlign="center" py={4}>
          Vui lòng chọn phòng ban để xem danh sách người dùng
        </Typography>
      )}
    </Container>
  );
} 