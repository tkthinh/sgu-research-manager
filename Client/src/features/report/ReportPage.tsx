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
  Alert,
  SelectChangeEvent,
  Chip,
  Tooltip,
} from '@mui/material';
import { Work } from '../../lib/types/models/Work';
import { getAcademicYears } from '../../lib/api/academicYearApi';
import { ProofStatus } from '../../lib/types/enums/ProofStatus';
import { WorkSource } from '../../lib/types/enums/WorkSource';
import { getWorksWithFilter } from '../../lib/api/worksApi';
import { useAuth } from '../../app/shared/contexts/AuthContext';
import { AcademicYear } from '../../lib/types/models/AcademicYear';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import CancelIcon from '@mui/icons-material/Cancel';
import HistoryIcon from '@mui/icons-material/History';
import FilterListIcon from '@mui/icons-material/FilterList';
import RestartAltIcon from '@mui/icons-material/RestartAlt';

interface FilterParams {
  academicYearId?: string;
  proofStatus?: number;
  source?: number;
  onlyRegisteredWorks: boolean;
  onlyRegisterableWorks: boolean;
  isCurrentUser: boolean;
}

const ReportPage: React.FC = () => {
  // State
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [works, setWorks] = useState<Work[]>([]);
  const [academicYears, setAcademicYears] = useState<AcademicYear[]>([]);
  const { user } = useAuth();
  
  // Filter state
  const [filter, setFilter] = useState<FilterParams>({
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
        const academicYearsResponse = await getAcademicYears();
        setAcademicYears(academicYearsResponse.data || []);
      } catch (error) {
        setError('Lỗi khi tải dữ liệu ban đầu');
        console.error('Error fetching initial data:', error);
      }
    };
    
    fetchData();
  }, []);

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
    
    try {
      const response = await getWorksWithFilter(filter);
      setWorks(response.data || []);
    } catch (error) {
      console.error('Error fetching works:', error);
      setError('Lỗi khi tải danh sách công trình');
    } finally {
      setLoading(false);
    }
  };

  // Reset filter
  const handleResetFilter = () => {
    setFilter({
      academicYearId: '',
      proofStatus: undefined,
      source: undefined,
      onlyRegisteredWorks: false,
      onlyRegisterableWorks: false,
      isCurrentUser: true,
    });
  };

  // Format date for display
  const formatDate = (dateString?: string) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('vi-VN');
  };

  // Get source text
  const getSourceText = (source: number) => {
    switch (source) {
      case WorkSource.NguoiDungKeKhai:
        return 'Người dùng kê khai';
      case WorkSource.QuanLyNhap:
        return 'Quản lý nhập';
      default:
        return 'N/A';
    }
  };

  // Get proof status chip
  const getProofStatusChip = (status: number) => {
    switch (status) {
      case ProofStatus.HopLe:
        return (
          <Tooltip title="Hợp lệ">
            <Chip
              icon={<CheckCircleIcon />}
              label="Hợp lệ"
              color="success"
              size="small"
            />
          </Tooltip>
        );
      case ProofStatus.KhongHopLe:
        return (
          <Tooltip title="Không hợp lệ">
            <Chip
              icon={<CancelIcon />}
              label="Không hợp lệ"
              color="error"
              size="small"
            />
          </Tooltip>
        );
      case ProofStatus.ChuaXuLy:
        return (
          <Tooltip title="Chưa xử lý">
            <Chip
              icon={<HistoryIcon />}
              label="Chưa xử lý"
              color="warning"
              size="small"
            />
          </Tooltip>
        );
      default:
        return null;
    }
  };

  return (
    <Container maxWidth="xl" sx={{ mt: 4, mb: 4 }}>
      {/* Filter Panel */}
      <Paper sx={{ p: 2, mb: 3 }}>
        <Typography variant="h6" gutterBottom display="flex" alignItems="center">
          <FilterListIcon sx={{ mr: 1 }} />
          Bộ lọc
        </Typography>
        
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth size="small">
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
          </Grid>

          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth size="small">
              <InputLabel>Trạng thái xác minh</InputLabel>
              <Select
                name="proofStatus"
                value={filter.proofStatus !== undefined ? filter.proofStatus : ''}
                onChange={handleSelectChange}
                label="Trạng thái xác minh"
              >
                <MenuItem value="">
                  <em>Không chọn</em>
                </MenuItem>
                <MenuItem value={ProofStatus.HopLe}>Hợp lệ</MenuItem>
                <MenuItem value={ProofStatus.KhongHopLe}>Không hợp lệ</MenuItem>
                <MenuItem value={ProofStatus.ChuaXuLy}>Chưa xử lý</MenuItem>
              </Select>
            </FormControl>
          </Grid>

          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth size="small">
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
          </Grid>

          <Grid item xs={12} sm={6} md={3}>
            <Box display="flex" flexDirection="column" gap={1}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={filter.onlyRegisteredWorks}
                    onChange={handleCheckboxChange}
                    name="onlyRegisteredWorks"
                  />
                }
                label="Công trình đã đăng ký"
              />
              
              <FormControlLabel
                control={
                  <Checkbox
                    checked={filter.onlyRegisterableWorks}
                    onChange={handleCheckboxChange}
                    name="onlyRegisterableWorks"
                  />
                }
                label="Công trình có thể đăng ký"
              />
            </Box>
          </Grid>

          <Grid item xs={12} display="flex" justifyContent="flex-end" gap={2}>
            <Button
              variant="outlined"
              color="primary"
              onClick={handleResetFilter}
              startIcon={<RestartAltIcon />}
            >
              Đặt lại
            </Button>
            <Button 
              variant="contained" 
              color="primary" 
              onClick={fetchWorks}
              disabled={loading}
              startIcon={loading ? <CircularProgress size={20} /> : <FilterListIcon />}
            >
              Tìm kiếm
            </Button>
          </Grid>
        </Grid>
      </Paper>
      
      {/* Results Panel */}
      <Paper sx={{ p: 2 }}>
        <Typography variant="h6" gutterBottom display="flex" alignItems="center" justifyContent="space-between">
          <span>Kết quả ({works.length} công trình)</span>
          {loading && <CircularProgress size={24} />}
        </Typography>
        
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
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
                  <TableCell>Công trình</TableCell>
                  <TableCell>Loại công trình</TableCell>
                  <TableCell>Năm học</TableCell>
                  <TableCell>Trạng thái</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {works.map((work, index) => (
                  <TableRow key={work.id} hover>
                    <TableCell>{index + 1}</TableCell>
                    <TableCell>{work.title}</TableCell>
                    <TableCell>{work.workTypeName || 'N/A'}</TableCell>
                    <TableCell>{work.academicYearName || 'N/A'}</TableCell>
                    <TableCell>
                      {work.authors?.map(author => 
                        author.userId === user?.id ? getProofStatusChip(author.proofStatus) : null
                      )}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </Paper>
    </Container>
  );
};

export default ReportPage; 