import React, { useState, useEffect } from 'react';
import {
  Box,
  Container,
  Typography,
  Button,
  Grid,
  Paper,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Checkbox,
  FormControlLabel,
  CircularProgress,
  Table,
  TableContainer,
  TableHead,
  TableBody,
  TableRow,
  TableCell,
  TextField,
  Alert,
  Snackbar,
  SelectChangeEvent,
  Divider,
  Card,
  CardContent,
  Chip,
  List,
  ListItem,
  ListItemText
} from '@mui/material';
import { Work } from '../../lib/types/models/Work';
import { ApiResponse } from '../../lib/types/common/ApiResponse';
import { getAcademicYears } from '../../lib/api/academicYearApi';
import { getDepartments } from '../../lib/api/departmentsApi';
import { ProofStatus } from '../../lib/types/enums/ProofStatus';
import { WorkSource } from '../../lib/types/enums/WorkSource';
import axios from 'axios';

interface FilterParams {
  userId?: string;
  departmentId?: string;
  academicYearId?: string;
  proofStatus?: number;
  source?: number;
  onlyRegisteredWorks: boolean;
  onlyRegisterableWorks: boolean;
  isCurrentUser: boolean;
}

interface Department {
  id: string;
  name: string;
}

interface AcademicYear {
  id: string;
  name: string;
}

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7251/api',
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 30000,
});

// Request interceptor to add the JWT token to the Authorization header
apiClient.interceptors.request.use(
  (config) => {
    // Get the JWT token from localStorage
    const token = localStorage.getItem('token');
    
    // If there's a token, set it in the Authorization header as Bearer token
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// API calls
const getWorksWithFilter = async (filter: FilterParams): Promise<ApiResponse<Work[]>> => {
  const params = new URLSearchParams();
  
  if (filter.userId) params.append('userId', filter.userId);
  if (filter.departmentId) params.append('departmentId', filter.departmentId);
  if (filter.academicYearId) params.append('academicYearId', filter.academicYearId);
  if (filter.proofStatus !== undefined) params.append('proofStatus', filter.proofStatus.toString());
  if (filter.source !== undefined) params.append('source', filter.source.toString());
  
  params.append('onlyRegisteredWorks', filter.onlyRegisteredWorks.toString());
  params.append('onlyRegisterableWorks', filter.onlyRegisterableWorks.toString());
  params.append('isCurrentUser', filter.isCurrentUser.toString());
  
  console.log('Gọi API với params:', params.toString());
  const response = await apiClient.get<ApiResponse<Work[]>>(`/works/filter?${params.toString()}`);
  return response.data;
};

const getMyWorks = async (): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>('/works/my-works');
  return response.data;
};

const getWorkById = async (id: string): Promise<ApiResponse<Work>> => {
  console.log('Gọi API getWorkById:', id);
  const response = await apiClient.get<ApiResponse<Work>>(`/works/${id}`);
  console.log('Response from getWorkById:', response.data);
  return response.data;
};

const getAllMyWorks = async (): Promise<ApiResponse<Work[]>> => {
  const response = await apiClient.get<ApiResponse<Work[]>>('/works/all-my-works');
  return response.data;
};

const TestPage: React.FC = () => {
  // State
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [works, setWorks] = useState<Work[]>([]);
  const [academicYears, setAcademicYears] = useState<AcademicYear[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [selectedWork, setSelectedWork] = useState<Work | null>(null);
  const [responseLog, setResponseLog] = useState<string | null>(null);
  
  // Filter state
  const [filter, setFilter] = useState<FilterParams>({
    userId: '',
    departmentId: '',
    academicYearId: '',
    proofStatus: undefined,
    source: undefined,
    onlyRegisteredWorks: false,
    onlyRegisterableWorks: false,
    isCurrentUser: true,
  });

  // Load initial data
  useEffect(() => {
    const fetchData = async () => {
      try {
        const [academicYearsResponse, departmentsResponse] = await Promise.all([
          getAcademicYears(),
          getDepartments()
        ]);
        
        setAcademicYears(academicYearsResponse.data || []);
        setDepartments(departmentsResponse.data || []);
      } catch (error) {
        setError('Lỗi khi tải dữ liệu ban đầu');
        console.error('Error fetching initial data:', error);
      }
    };
    
    fetchData();
  }, []);

  // Handle form changes
  const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement | { name?: string; value: unknown }>) => {
    const { name, value } = e.target;
    setFilter(prev => ({ ...prev, [name as string]: value }));
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    setFilter(prev => ({ ...prev, [name]: checked }));
  };

  const handleSelectChange = (e: SelectChangeEvent<string | number>) => {
    const { name, value } = e.target;
    
    if (name === 'proofStatus' || name === 'source') {
      setFilter(prev => ({ ...prev, [name]: value === '' ? undefined : Number(value) }));
    } else {
      setFilter(prev => ({ ...prev, [name as string]: value }));
    }
  };

  // Handle API calls
  const fetchWorks = async () => {
    setLoading(true);
    setError(null);
    setResponseLog(null);
    
    try {
      const response = await getWorksWithFilter(filter);
      setWorks(response.data || []);
      setResponseLog(JSON.stringify({
        success: response.success,
        message: response.message,
        count: response.data?.length || 0
      }, null, 2));
    } catch (error) {
      console.error('Error fetching works:', error);
      setError('Lỗi khi tải danh sách công trình');
    } finally {
      setLoading(false);
    }
  };

  const fetchMyWorks = async () => {
    setLoading(true);
    setError(null);
    setResponseLog(null);
    
    try {
      const response = await getMyWorks();
      setWorks(response.data || []);
      setResponseLog(JSON.stringify({
        success: response.success,
        message: response.message,
        count: response.data?.length || 0
      }, null, 2));
    } catch (error) {
      console.error('Error fetching my works:', error);
      setError('Lỗi khi tải danh sách công trình của tôi');
    } finally {
      setLoading(false);
    }
  };

  const fetchAllMyWorks = async () => {
    setLoading(true);
    setError(null);
    setResponseLog(null);
    
    try {
      const response = await getAllMyWorks();
      setWorks(response.data || []);
      setResponseLog(JSON.stringify({
        success: response.success,
        message: response.message,
        count: response.data?.length || 0
      }, null, 2));
    } catch (error) {
      console.error('Error fetching all my works:', error);
      setError('Lỗi khi tải tất cả công trình của tôi');
    } finally {
      setLoading(false);
    }
  };

  const handleViewWorkDetails = async (workId: string) => {
    setLoading(true);
    setError(null);
    setResponseLog(null);
    
    try {
      const response = await getWorkById(workId);
      setSelectedWork(response.data);
      setResponseLog(JSON.stringify({
        success: response.success,
        message: response.message,
        data: {
          id: response.data?.id,
          title: response.data?.title,
          authorsCount: response.data?.authors?.length || 0,
          coAuthorUserIds: response.data?.coAuthorUserIds || []
        }
      }, null, 2));
    } catch (error) {
      console.error('Error fetching work details:', error);
      setError('Lỗi khi tải chi tiết công trình');
    } finally {
      setLoading(false);
    }
  };

  // Format date for display
  const formatDate = (dateString?: string) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('vi-VN');
  };

  const getProofStatusText = (status?: number) => {
    if (status === undefined) return 'Không xác định';
    
    switch (status) {
      case ProofStatus.ChuaXuLy:
        return 'Chưa xử lý';
      case ProofStatus.HopLe:
        return 'Hợp lệ';
      case ProofStatus.KhongHopLe:
        return 'Không hợp lệ';
      default:
        return 'Không xác định';
    }
  };

  const getSourceText = (source?: number) => {
    if (source === undefined) return 'Không xác định';
    
    switch (source) {
      case WorkSource.NguoiDungKeKhai:
        return 'Người dùng kê khai';
      case WorkSource.QuanLyNhap:
        return 'Quản lý nhập';
      default:
        return 'Không xác định';
    }
  };

  return (
    <Container maxWidth="xl" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" gutterBottom>
        Kiểm thử API Works Controller
      </Typography>
      
      <Box mb={4}>
        <Alert severity="info">
          Trang này sử dụng để kiểm thử các API của WorksController. Nếu bạn đã đăng nhập với token hợp lệ, các API sẽ tự động sử dụng thông tin người dùng hiện tại.
        </Alert>
      </Box>
      
      <Grid container spacing={3}>
        {/* Filter Panel */}
        <Grid item xs={12} md={4}>
          <Paper sx={{ p: 2, height: '100%' }}>
            <Typography variant="h6" gutterBottom>
              Bộ lọc
            </Typography>
            
            <FormControlLabel
              control={
                <Checkbox
                  checked={filter.isCurrentUser}
                  onChange={handleCheckboxChange}
                  name="isCurrentUser"
                />
              }
              label="Sử dụng người dùng hiện tại"
            />
            
            {!filter.isCurrentUser && (
              <TextField
                fullWidth
                margin="normal"
                label="User ID"
                name="userId"
                value={filter.userId || ''}
                onChange={handleFilterChange}
                size="small"
                disabled={filter.isCurrentUser}
              />
            )}
            
            <FormControl fullWidth margin="normal" size="small">
              <InputLabel>Khoa/Phòng ban</InputLabel>
              <Select
                name="departmentId"
                value={filter.departmentId || ''}
                onChange={handleSelectChange}
                label="Khoa/Phòng ban"
              >
                <MenuItem value="">
                  <em>Không chọn</em>
                </MenuItem>
                {departments.map((dept) => (
                  <MenuItem key={dept.id} value={dept.id}>
                    {dept.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            
            <FormControl fullWidth margin="normal" size="small">
              <InputLabel>Năm học</InputLabel>
              <Select
                name="academicYearId"
                value={filter.academicYearId || ''}
                onChange={handleSelectChange}
                label="Năm học"
              >
                <MenuItem value="">
                  <em>Không chọn</em>
                </MenuItem>
                {academicYears.map((year) => (
                  <MenuItem key={year.id} value={year.id}>
                    {year.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            
            <FormControl fullWidth margin="normal" size="small">
              <InputLabel>Trạng thái minh chứng</InputLabel>
              <Select
                name="proofStatus"
                value={filter.proofStatus !== undefined ? filter.proofStatus : ''}
                onChange={handleSelectChange}
                label="Trạng thái minh chứng"
              >
                <MenuItem value="">
                  <em>Không chọn</em>
                </MenuItem>
                <MenuItem value={ProofStatus.ChuaXuLy}>Chưa xử lý</MenuItem>
                <MenuItem value={ProofStatus.HopLe}>Hợp lệ</MenuItem>
                <MenuItem value={ProofStatus.KhongHopLe}>Không hợp lệ</MenuItem>
              </Select>
            </FormControl>
            
            <FormControl fullWidth margin="normal" size="small">
              <InputLabel>Nguồn</InputLabel>
              <Select
                name="source"
                value={filter.source !== undefined ? filter.source : ''}
                onChange={handleSelectChange}
                label="Nguồn"
              >
                <MenuItem value="">
                  <em>Không chọn</em>
                </MenuItem>
                <MenuItem value={WorkSource.NguoiDungKeKhai}>Người dùng kê khai</MenuItem>
                <MenuItem value={WorkSource.QuanLyNhap}>Quản lý nhập</MenuItem>
              </Select>
            </FormControl>
            
            <FormControlLabel
              control={
                <Checkbox
                  checked={filter.onlyRegisteredWorks}
                  onChange={handleCheckboxChange}
                  name="onlyRegisteredWorks"
                />
              }
              label="Chỉ lấy công trình đăng ký quy đổi"
            />
            
            <FormControlLabel
              control={
                <Checkbox
                  checked={filter.onlyRegisterableWorks}
                  onChange={handleCheckboxChange}
                  name="onlyRegisterableWorks"
                />
              }
              label="Chỉ lấy công trình có thể đăng ký quy đổi"
            />
            
            <Box mt={3}>
              <Button 
                variant="contained" 
                color="primary" 
                fullWidth 
                onClick={fetchWorks}
                disabled={loading}
              >
                {loading ? <CircularProgress size={24} /> : 'Tìm kiếm theo bộ lọc'}
              </Button>
              
              <Box display="flex" mt={2} gap={2}>
                <Button 
                  variant="outlined" 
                  color="primary" 
                  onClick={fetchMyWorks}
                  disabled={loading}
                  fullWidth
                >
                  Lấy công trình của tôi
                </Button>
                
                <Button 
                  variant="outlined" 
                  color="secondary" 
                  onClick={fetchAllMyWorks}
                  disabled={loading}
                  fullWidth
                >
                  Tất cả công trình
                </Button>
              </Box>
            </Box>
          </Paper>
        </Grid>
        
        {/* Results Panel */}
        <Grid item xs={12} md={8}>
          <Paper sx={{ p: 2, mb: 3 }}>
            <Typography variant="h6" gutterBottom display="flex" alignItems="center" justifyContent="space-between">
              <span>Kết quả ({works.length} công trình)</span>
              {loading && <CircularProgress size={24} />}
            </Typography>
            
            {error && (
              <Alert severity="error" sx={{ mb: 2 }}>
                {error}
              </Alert>
            )}
            
            {responseLog && (
              <Alert severity="success" sx={{ mb: 2 }}>
                <pre style={{ whiteSpace: 'pre-wrap', margin: 0 }}>{responseLog}</pre>
              </Alert>
            )}
            
            {works.length === 0 && !loading ? (
              <Alert severity="info">
                Không có công trình nào được tìm thấy. Vui lòng thay đổi bộ lọc hoặc thử lại.
              </Alert>
            ) : (
              <TableContainer>
                <Table size="small">
                  <TableHead>
                    <TableRow>
                      <TableCell>STT</TableCell>
                      <TableCell>Tiêu đề</TableCell>
                      <TableCell>Loại công trình</TableCell>
                      <TableCell>Năm học</TableCell>
                      <TableCell>Nguồn</TableCell>
                      <TableCell>Đồng tác giả</TableCell>
                      <TableCell>Thao tác</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {works.map((work, index) => (
                      <TableRow key={work.id} hover>
                        <TableCell>{index + 1}</TableCell>
                        <TableCell>{work.title}</TableCell>
                        <TableCell>{work.workTypeName || 'N/A'}</TableCell>
                        <TableCell>{work.academicYearName || 'N/A'}</TableCell>
                        <TableCell>{getSourceText(work.source)}</TableCell>
                        <TableCell>{work.coAuthorUserIds?.length || 0}</TableCell>
                        <TableCell>
                          <Button
                            size="small"
                            onClick={() => handleViewWorkDetails(work.id)}
                          >
                            Chi tiết
                          </Button>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            )}
          </Paper>
          
          {/* Selected Work Details */}
          {selectedWork && (
            <Paper sx={{ p: 2 }}>
              <Typography variant="h6" gutterBottom>
                Chi tiết công trình
              </Typography>
              
              <Grid container spacing={2}>
                <Grid item xs={12}>
                  <Typography variant="h5">{selectedWork.title}</Typography>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2">Loại công trình:</Typography>
                  <Typography variant="body2">{selectedWork.workTypeName || 'N/A'}</Typography>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2">Cấp công trình:</Typography>
                  <Typography variant="body2">{selectedWork.workLevelName || 'N/A'}</Typography>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2">Năm học:</Typography>
                  <Typography variant="body2">{selectedWork.academicYearName || 'N/A'}</Typography>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2">Ngày xuất bản:</Typography>
                  <Typography variant="body2">{formatDate(selectedWork.timePublished)}</Typography>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2">Nguồn:</Typography>
                  <Typography variant="body2">{getSourceText(selectedWork.source)}</Typography>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2">Ngày tạo:</Typography>
                  <Typography variant="body2">{formatDate(selectedWork.createdDate)}</Typography>
                </Grid>
                
                <Grid item xs={12}>
                  <Divider sx={{ my: 2 }} />
                  <Typography variant="subtitle1" fontWeight="bold">
                    Thông tin tác giả và đồng tác giả
                  </Typography>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Card variant="outlined">
                    <CardContent>
                      <Typography variant="subtitle2" gutterBottom>
                        Đồng tác giả ({selectedWork.coAuthorUserIds?.length || 0}):
                      </Typography>
                      {selectedWork.coAuthorUserIds && selectedWork.coAuthorUserIds.length > 0 ? (
                        <List dense>
                          {selectedWork.coAuthorUserIds.map((userId, index) => (
                            <ListItem key={userId} divider={index < selectedWork.coAuthorUserIds!.length - 1}>
                              <ListItemText
                                primary={`Đồng tác giả ${index + 1}`}
                                secondary={userId}
                              />
                            </ListItem>
                          ))}
                        </List>
                      ) : (
                        <Typography variant="body2" color="text.secondary">
                          Không có đồng tác giả
                        </Typography>
                      )}
                    </CardContent>
                  </Card>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Card variant="outlined">
                    <CardContent>
                      <Typography variant="subtitle2" gutterBottom>
                        Tác giả ({selectedWork.authors?.length || 0}):
                      </Typography>
                      {selectedWork.authors && selectedWork.authors.length > 0 ? (
                        <List dense>
                          {selectedWork.authors.map((author, index) => (
                            <ListItem key={author.id} divider={index < selectedWork.authors!.length - 1}>
                              <ListItemText
                                primary={author.authorRoleName || 'Chưa có tên vai trò'}
                                secondary={`UserId: ${author.userId || 'N/A'}, Trạng thái: ${getProofStatusText(author.proofStatus)}`}
                              />
                              {author.authorRoleName?.includes('Chính') && (
                                <Chip size="small" color="primary" label="Tác giả chính" />
                              )}
                            </ListItem>
                          ))}
                        </List>
                      ) : (
                        <Typography variant="body2" color="text.secondary">
                          Không có thông tin tác giả
                        </Typography>
                      )}
                    </CardContent>
                  </Card>
                </Grid>
              </Grid>
              
              <Box mt={2}>
                <Button variant="outlined" onClick={() => setSelectedWork(null)}>
                  Đóng chi tiết
                </Button>
              </Box>
            </Paper>
          )}
        </Grid>
      </Grid>
      
      <Snackbar
        open={!!error}
        autoHideDuration={6000}
        onClose={() => setError(null)}
        message={error}
      />
    </Container>
  );
};

export default TestPage; 