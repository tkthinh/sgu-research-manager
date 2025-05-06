import CancelIcon from "@mui/icons-material/Cancel";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import FileDownloadIcon from "@mui/icons-material/FileDownload";
import FilterListIcon from "@mui/icons-material/FilterList";
import HistoryIcon from "@mui/icons-material/History";
import RestartAltIcon from "@mui/icons-material/RestartAlt";
import SearchIcon from "@mui/icons-material/Search";
import {
  Alert,
  Box,
  Button,
  Checkbox,
  Chip,
  CircularProgress,
  Container,
  Divider,
  FormControl,
  FormControlLabel,
  Grid,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  SelectChangeEvent,
  Stack,
  Tooltip,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import React, { useEffect, useState } from "react";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { getAcademicYears } from "../../lib/api/academicYearApi";
import { exportWorks } from "../../lib/api/excelApi";
import { getWorksWithFilter } from "../../lib/api/worksApi";
import { ProofStatus } from "../../lib/types/enums/ProofStatus";
import { WorkSource } from "../../lib/types/enums/WorkSource";
import { AcademicYear } from "../../lib/types/models/AcademicYear";
import { Work } from "../../lib/types/models/Work";
import { getUserConversionResult } from '../../lib/api/usersApi';

interface FilterParams {
  academicYearId?: string;
  proofStatus?: number;
  source?: number;
  onlyRegisteredWorks: boolean;
  onlyRegisterableWorks: boolean;
  isCurrentUser: boolean;
}

interface ConversionResult {
  userId: string;
  userName: string;
  conversionResults: {
    dutyHourConversion: {
      totalWorks: number;
      totalConvertedHours: number;
      totalCalculatedHours: number;
    };
    overLimitConversion: {
      totalWorks: number;
      totalConvertedHours: number;
      totalCalculatedHours: number;
    };
    researchProductConversion: {
      totalWorks: number;
      totalConvertedHours: number;
      totalCalculatedHours: number;
    };
    totalWorks: number;
    totalCalculatedHours: number;
  };
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
    academicYearId: "",
    proofStatus: undefined,
    source: undefined,
    onlyRegisteredWorks: false,
    onlyRegisterableWorks: false,
    isCurrentUser: true,
  });

  const [conversionResult, setConversionResult] = useState<ConversionResult | null>(null);
  const [loadingConversion, setLoadingConversion] = useState<boolean>(false);

  // Load initial data
  useEffect(() => {
    const fetchData = async () => {
      try {
        const academicYearsResponse = await getAcademicYears();
        setAcademicYears(academicYearsResponse.data || []);
      } catch (error) {
        setError("Lỗi khi tải dữ liệu ban đầu");
        console.error("Error fetching initial data:", error);
      }
    };

    fetchData();
  }, []);

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    setFilter((prev) => ({ ...prev, [name]: checked }));
  };

  const handleSelectChange = (e: SelectChangeEvent<string | number>) => {
    const { name, value } = e.target;

    if (name === "proofStatus" || name === "source") {
      setFilter((prev) => ({
        ...prev,
        [name]: value === "" ? undefined : Number(value),
      }));
    } else {
      setFilter((prev) => ({ ...prev, [name as string]: value }));
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
      console.error("Error fetching works:", error);
      setError("Lỗi khi tải danh sách công trình");
    } finally {
      setLoading(false);
    }
  };

  // Reset filter
  const handleResetFilter = () => {
    setFilter({
      academicYearId: "",
      proofStatus: undefined,
      source: undefined,
      onlyRegisteredWorks: false,
      onlyRegisterableWorks: false,
      isCurrentUser: true,
    });
  };

  // Get source text
  const getSourceText = (source: number) => {
    switch (source) {
      case WorkSource.NguoiDungKeKhai:
        return "Người dùng kê khai";
      case WorkSource.QuanLyNhap:
        return "Quản lý nhập";
      default:
        return "N/A";
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

  const columns: GridColDef[] = [
    {
      field: "stt",
      headerName: "STT",
      width: 70,
      renderCell: (params) => {
        const rowIds = params.api.getAllRowIds();
        const index = rowIds.indexOf(params.id);
        return <div>{index + 1}</div>;
      },
    },
    {
      field: "title",
      headerName: "Tên công trình",
      type: "string",
      width: 250,
    },
    {
      field: "workTypeName",
      headerName: "Loại công trình",
      type: "string",
      width: 170,
    },
    {
      field: "workLevelName",
      headerName: "Cấp công trình",
      type: "string",
      width: 150,
    },
    {
      field: "source",
      headerName: "Nguồn",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        return <div>{getSourceText(params.value)}</div>;
      },
    },
    {
      field: "academicYearName",
      headerName: "Năm học",
      type: "string",
      width: 150,
    },
    {
      field: "proofStatus",
      headerName: "Trạng thái xác minh",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        return currentAuthor
          ? getProofStatusChip(currentAuthor.proofStatus)
          : null;
      },
    },
    {
      field: "isRegistered",
      headerName: "Trạng thái đăng ký",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        const isRegistered = currentAuthor?.authorRegistration != null;

        return (
          <Chip
            label={isRegistered ? "Đã đăng ký" : "Chưa đăng ký"}
            color={isRegistered ? "success" : "default"}
            size="small"
          />
        );
      },
    },
  ];

  // Thêm hàm xử lý export
  const handleExport = async () => {
    try {
      setLoading(true);
      const blob = await exportWorks(filter);

      // Tạo URL cho file blob
      const url = window.URL.createObjectURL(blob);

      // Tạo link tải file
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute(
        "download",
        `KeKhaiCongTrinh_${new Date().toISOString().slice(0, 10)}.xlsx`,
      );

      // Thêm link vào DOM và click
      document.body.appendChild(link);
      link.click();

      // Dọn dẹp
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error("Lỗi khi export:", error);
      setError("Lỗi khi xuất file Excel");
    } finally {
      setLoading(false);
    }
  };

  // Thêm hàm lấy kết quả quy đổi
  const fetchConversionResult = async () => {
    if (!user?.id) return;
    
    try {
      setLoadingConversion(true);
      const response = await getUserConversionResult(user.id);
      if (response.success) {
        setConversionResult(response.data);
      }
    } catch (error) {
      console.error('Lỗi khi lấy kết quả quy đổi:', error);
    } finally {
      setLoadingConversion(false);
    }
  };

  // Gọi API khi component mount và khi user thay đổi
  useEffect(() => {
    fetchConversionResult();
  }, [user?.id]);

  return (
    <Container maxWidth="xl" sx={{ mt: 4, mb: 4 }}>
      {/* Filter Panel */}
      <Paper sx={{ p: 3, mb: 3, borderRadius: 2, boxShadow: 3 }}>
        <Stack
          direction="row"
          justifyContent="space-between"
          alignItems="center"
          mb={2}
        >
          <Typography
            variant="h6"
            display="flex"
            alignItems="center"
            color="primary"
          >
            <FilterListIcon sx={{ mr: 1 }} />
            Bộ lọc
          </Typography>
          <Stack direction="row" spacing={2}>
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
              startIcon={
                loading ? <CircularProgress size={20} /> : <SearchIcon />
              }
            >
              Tìm kiếm
            </Button>
          </Stack>
        </Stack>

        <Divider sx={{ mb: 3 }} />

        <Grid container spacing={3}>
          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth size="small">
              <InputLabel>Năm học</InputLabel>
              <Select
                name="academicYearId"
                value={filter.academicYearId || ""}
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
                value={
                  filter.proofStatus !== undefined ? filter.proofStatus : ""
                }
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
                value={filter.source !== undefined ? filter.source : ""}
                onChange={handleSelectChange}
                label="Nguồn"
              >
                <MenuItem value="">
                  <em>Không chọn</em>
                </MenuItem>
                <MenuItem value={WorkSource.NguoiDungKeKhai}>
                  Người dùng kê khai
                </MenuItem>
                <MenuItem value={WorkSource.QuanLyNhap}>Quản lý nhập</MenuItem>
              </Select>
            </FormControl>
          </Grid>

          <Grid item xs={12} sm={6} md={3}>
            <Box display="flex" flexDirection="column" gap={2}>
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
        </Grid>
      </Paper>

      {/* Results Panel */}
      <Paper sx={{ p: 3, borderRadius: 2, boxShadow: 3, mb: 4 }}>
        <Stack
          direction="row"
          justifyContent="space-between"
          alignItems="center"
          mb={2}
        >
          <Typography variant="h6" display="flex" alignItems="center">
            <span>Kết quả ({works.length} công trình)</span>
            {loading && <CircularProgress size={24} sx={{ ml: 2 }} />}
          </Typography>
          <Button
            variant="outlined"
            color="primary"
            onClick={handleExport}
            startIcon={<FileDownloadIcon />}
            disabled={works.length === 0}
          >
            Xuất Excel
          </Button>
        </Stack>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        {works.length === 0 && !loading ? (
          <Alert severity="info">
            Không có công trình nào được tìm thấy. Vui lòng thay đổi bộ lọc hoặc
            thử lại.
          </Alert>
        ) : (
          <GenericTable columns={columns} data={works} />
        )}
      </Paper>

      {/* Conversion Results Panel */}
      <Paper sx={{ p: 3, borderRadius: 2, boxShadow: 3 }}>
        <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
          <Typography variant="h6" display="flex" alignItems="center">
            <span>Kết quả quy đổi</span>
            {loadingConversion && <CircularProgress size={24} sx={{ ml: 2 }} />}
          </Typography>
        </Stack>

        {conversionResult ? (
          <TableContainer>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Loại quy đổi</TableCell>
                  <TableCell align="right">Số lượng công trình</TableCell>
                  <TableCell align="right">Tổng giờ quy đổi</TableCell>
                  <TableCell align="right">Tổng giờ tính</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                <TableRow>
                  <TableCell>Quy đổi giờ nghĩa vụ</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.dutyHourConversion.totalWorks}</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.dutyHourConversion.totalConvertedHours}</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.dutyHourConversion.totalCalculatedHours}</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>Quy đổi vượt định mức</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.overLimitConversion.totalWorks}</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.overLimitConversion.totalConvertedHours}</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.overLimitConversion.totalCalculatedHours}</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>Sản phẩm NCKH</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.researchProductConversion.totalWorks}</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.researchProductConversion.totalConvertedHours}</TableCell>
                  <TableCell align="right">{conversionResult.conversionResults.researchProductConversion.totalCalculatedHours}</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell><strong>Tổng cộng</strong></TableCell>
                  <TableCell align="right"><strong>{conversionResult.conversionResults.totalWorks}</strong></TableCell>
                  <TableCell align="right">-</TableCell>
                  <TableCell align="right"><strong>{conversionResult.conversionResults.totalCalculatedHours}</strong></TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer>
        ) : (
          <Alert severity="info">
            Không có dữ liệu kết quả quy đổi
          </Alert>
        )}
      </Paper>
    </Container>
  );
};

export default ReportPage;
