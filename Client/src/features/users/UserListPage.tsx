import React, { useEffect, useState } from 'react';
import {
  Box,
  Button,
  CircularProgress,
  FormControl,
  IconButton,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  SelectChangeEvent,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  Tooltip,
  Typography,
} from '@mui/material';
import { User } from '../../lib/types/models/User';
import { getAllUsers, getUsersByDepartmentId } from '../../lib/api/usersApi';
import { getDepartments } from '../../lib/api/departmentsApi';
import { Department } from '../../lib/types/models/Department';
import { toast } from 'react-toastify';
import { format } from 'date-fns';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { useNavigate } from 'react-router-dom';

const UserListPage: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedDepartment, setSelectedDepartment] = useState<string>('');
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const [usersResponse, departmentsResponse] = await Promise.all([
          getAllUsers(),
          getDepartments()
        ]);

        if (usersResponse.success && departmentsResponse.success) {
          setUsers(usersResponse.data || []);
          setDepartments(departmentsResponse.data || []);
        } else {
          toast.error("Lỗi khi tải dữ liệu");
        }
      } catch (error) {
        console.error("Lỗi khi tải dữ liệu:", error);
        toast.error("Lỗi khi tải dữ liệu");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleDepartmentChange = async (event: SelectChangeEvent) => {
    const departmentId = event.target.value;
    setSelectedDepartment(departmentId);
    
    try {
      setLoading(true);
      if (departmentId) {
        const response = await getUsersByDepartmentId(departmentId);
        if (response.success) {
          setUsers(response.data || []);
        } else {
          toast.error("Lỗi khi lọc người dùng theo phòng ban");
        }
      } else {
        const response = await getAllUsers();
        if (response.success) {
          setUsers(response.data || []);
        }
      }
    } catch (error) {
      console.error("Lỗi khi lọc người dùng theo phòng ban:", error);
      toast.error("Lỗi khi lọc người dùng theo phòng ban");
    } finally {
      setLoading(false);
    }
  };

  const handleChangePage = (_event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const getAcademicTitleDisplay = (academicTitle: string) => {
    const map: Record<string, string> = {
      None: "Không có",
      Bachelor: "Cử nhân",
      Master: "Thạc sĩ",
      Doctor: "Tiến sĩ",
      AssociateProfessor: "Phó giáo sư",
      Professor: "Giáo sư",
    };
    return map[academicTitle] || academicTitle;
  };

  const getOfficerRankDisplay = (officerRank: string) => {
    const map: Record<string, string> = {
      None: "Không có",
      Lecturer: "Giảng viên",
      SeniorLecturer: "Giảng viên chính",
      PrincipalLecturer: "Giảng viên cao cấp",
    };
    return map[officerRank] || officerRank;
  };

  const handleEditUser = (userId: string) => {
    toast.info("Chức năng đang được phát triển");
  };

  const handleDeleteUser = (userId: string) => {
    toast.info("Chức năng đang được phát triển");
  };

  const handleViewUserWorks = (userId: string) => {
    navigate(`/cong-trinh-nguoi-dung/${userId}`);
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" gutterBottom>
        Danh sách người dùng
      </Typography>

      <Box sx={{ mb: 3, display: "flex", justifyContent: "flex-end" }}>
        <FormControl sx={{ minWidth: 300 }}>
          <InputLabel id="department-filter-label">Lọc theo đơn vị công tác</InputLabel>
          <Select
            labelId="department-filter-label"
            value={selectedDepartment}
            onChange={handleDepartmentChange}
            label="Lọc theo đơn vị công tác"
          >
            <MenuItem value="">Tất cả</MenuItem>
            {departments.map((department) => (
              <MenuItem key={department.id} value={department.id}>
                {department.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Box>

      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", mt: 3 }}>
          <CircularProgress />
        </Box>
      ) : (
        <>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>STT</TableCell>
                  <TableCell>Họ và tên</TableCell>
                  <TableCell>Mã số viên chức</TableCell>
                  <TableCell>Đơn vị</TableCell>
                  <TableCell>Học hàm/Học vị</TableCell>
                  <TableCell>Ngành</TableCell>
                  <TableCell>Ngạch công chức</TableCell>
                  <TableCell align="center">Thao tác</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {users
                  .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                  .map((user, index) => (
                    <TableRow key={user.id} hover>
                      <TableCell>{page * rowsPerPage + index + 1}</TableCell>
                      <TableCell>{user.fullName}</TableCell>
                      <TableCell>{user.userName}</TableCell>
                      <TableCell>{user.departmentName}</TableCell>
                      <TableCell>{getAcademicTitleDisplay(user.academicTitle)}</TableCell>
                      <TableCell>{user.fieldName}</TableCell>
                      <TableCell>{getOfficerRankDisplay(user.officerRank)}</TableCell>
                      <TableCell align="center">
                        <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                          <Tooltip title="Sửa">
                            <IconButton 
                              color="primary" 
                              size="small" 
                              onClick={() => handleEditUser(user.id)}
                              sx={{ mr: 1 }}
                            >
                              <EditIcon fontSize="small" />
                            </IconButton>
                          </Tooltip>
                          <Tooltip title="Xóa">
                            <IconButton 
                              color="error" 
                              size="small" 
                              onClick={() => handleDeleteUser(user.id)}
                              sx={{ mr: 1 }}
                            >
                              <DeleteIcon fontSize="small" />
                            </IconButton>
                          </Tooltip>
                          <Tooltip title="Xem công trình">
                            <IconButton 
                              color="info" 
                              size="small" 
                              onClick={() => handleViewUserWorks(user.id)}
                            >
                              <VisibilityIcon fontSize="small" />
                            </IconButton>
                          </Tooltip>
                        </Box>
                      </TableCell>
                    </TableRow>
                  ))}
                {users.length === 0 && (
                  <TableRow>
                    <TableCell colSpan={8} align="center">
                      Không có dữ liệu
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </TableContainer>
          <TablePagination
            rowsPerPageOptions={[5, 10, 25, 50]}
            component="div"
            count={users.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
            labelRowsPerPage="Số dòng mỗi trang:"
            labelDisplayedRows={({ from, to, count }) => `${from}-${to} của ${count}`}
          />
        </>
      )}
    </Box>
  );
};

export default UserListPage; 