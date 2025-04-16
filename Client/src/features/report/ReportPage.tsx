import {
  Box,
  Button,
  CircularProgress,
  Container,
  FormControl,
  Grid,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
  Typography,
  Tooltip,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState, useMemo, useCallback } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { 
  getAllMyWorks, 
  getAllMyWorksByAcademicYearId, 
  getMyWorks, 
  getRegisteredWorks,
  getRegisteredWorksByAcademicYear
} from "../../lib/api/worksApi";
import { formatMonthYear } from "../../lib/utils/dateUtils";
import { ProofStatus } from "../../lib/types/enums/ProofStatus";
import { WorkSource } from "../../lib/types/enums/WorkSource";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import CancelIcon from "@mui/icons-material/Cancel";
import HistoryIcon from "@mui/icons-material/History";
import FilterListIcon from '@mui/icons-material/FilterList';
import RestartAltIcon from '@mui/icons-material/RestartAlt';
import { getScoreLevelText } from '../../lib/utils/scoreLevelUtils';
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { getAcademicYears, getCurrentAcademicYear } from "../../lib/api/academicYearApi";
import { AcademicYear } from "../../lib/types/models/AcademicYear";
import { Work } from "../../lib/types/models/Work";
import { GridRowParams } from "@mui/x-data-grid";

// Filter options
enum FilterType {
  ALL = "all",
  REGISTERED = "registered", 
  SOURCE_IMPORT = "source_import",
  SOURCE_USER = "source_user",
  PROOF_VALID = "proof_valid",
  PROOF_INVALID = "proof_invalid",
  PROOF_PENDING = "proof_pending",
  MY_WORKS = "my_works"
}

export default function ReportPage() {
  const queryClient = useQueryClient();
  const { user } = useAuth();
  const [academicYearId, setAcademicYearId] = useState<string>("");
  const [filterType, setFilterType] = useState<FilterType>(FilterType.ALL);
  const [filteredWorks, setFilteredWorks] = useState<Work[]>([]);

  // Hàm kiểm tra công trình đã đăng ký quy đổi
  const isUserRegisteredAuthor = (params: GridRowParams<Work>) => {
    const work = params.row;
    const author = work.authors?.find(
      (a) => a.userId === user?.id && a.authorRegistration !== null && a.authorRegistration !== undefined
    );
    console.log('Author:', author, 'User ID:', user?.id, 'Registration:', author?.authorRegistration);
    return !!author;
  };

  // Lấy danh sách năm học
  const { data: academicYears, isLoading: isLoadingAcademicYears } = useQuery({
    queryKey: ["academicYears"],
    queryFn: async () => {
      const response = await getAcademicYears();
      return response.data;
    },
    enabled: true,
    staleTime: 60000, // Cache 1 phút để cải thiện hiệu suất
  });

  // Lấy năm học hiện tại
  const { data: currentAcademicYear } = useQuery({
    queryKey: ["currentAcademicYear"],
    queryFn: async () => {
      const response = await getCurrentAcademicYear();
      return response.data;
    },
    staleTime: 60000, // Cache 1 phút để cải thiện hiệu suất
  });

  // Thiết lập năm học mặc định và tải dữ liệu khi component được load
  useEffect(() => {
    if (currentAcademicYear) {
      setAcademicYearId(currentAcademicYear.id);
    } else if (academicYears && academicYears.length > 0) {
      setAcademicYearId(academicYears[0].id);
    }
  }, [currentAcademicYear, academicYears]);

  // Lấy tất cả công trình của người dùng theo năm học - cải thiện với staleTime và retry
  const { 
    data: works, 
    isLoading: isLoadingWorks, 
    isError, 
    error, 
    refetch
  } = useQuery({
    queryKey: ["allMyWorks", academicYearId],
    queryFn: async () => {
      if (!academicYearId) {
        const response = await getAllMyWorks();
        return response.data;
      } else {
        const response = await getAllMyWorksByAcademicYearId(academicYearId);
        return response.data;
      }
    },
    enabled: !!academicYearId,
    staleTime: 30000, // Cache 30 giây
    retry: 2, // Thử lại 2 lần nếu có lỗi
    refetchOnWindowFocus: false, // Không refetch khi focus lại cửa sổ
  });

  // Lấy danh sách công trình đã đăng ký quy đổi
  const { 
    data: registeredWorks, 
    isLoading: isLoadingRegisteredWorks,
    refetch: refetchRegistered
  } = useQuery({
    queryKey: ["registeredWorks", academicYearId],
    queryFn: async () => {
      try {
        let response;
        if (!academicYearId) {
          response = await getRegisteredWorks();
        } else {
          response = await getRegisteredWorksByAcademicYear(academicYearId);
        }
        console.log("API Response for registered works:", response);
        console.log("Author registration data:", response.data?.map(work => 
          work.authors?.map(author => ({
            workTitle: work.title,
            authorId: author.id,
            userId: author.userId,
            registration: author.authorRegistration
          }))
        ));
        return response.data;
      } catch (error) {
        console.error("Error fetching registered works:", error);
        return [];
      }
    },
    staleTime: 30000, // Cache 30 giây
    refetchOnWindowFocus: false,
    enabled: filterType === FilterType.REGISTERED && !!academicYearId
  });

  // Xử lý lọc công trình
  useEffect(() => {
    if (!works && filterType !== FilterType.REGISTERED) {
      setFilteredWorks([]);
      return;
    }

    let result: Work[] = [];

    switch (filterType) {
      case FilterType.REGISTERED:
        // Sử dụng API mới để lấy các công trình đã đăng ký quy đổi
        if (registeredWorks) {
          console.log("Using registered works:", registeredWorks);
          result = registeredWorks;
        }
        break;
      case FilterType.SOURCE_IMPORT:
        // Lọc công trình do admin import
        result = works ? works.filter((work) => work.source === WorkSource.QuanLyNhap) : [];
        break;
      case FilterType.SOURCE_USER:
        // Lọc công trình do người dùng kê khai
        result = works ? works.filter((work) => work.source === WorkSource.NguoiDungKeKhai) : [];
        break;
      case FilterType.PROOF_VALID:
        // Lọc công trình hợp lệ
        result = works ? works.filter(
          (work) => 
            work.authors && 
            work.authors[0] && 
            work.authors[0].proofStatus === ProofStatus.HopLe
        ) : [];
        break;
      case FilterType.PROOF_INVALID:
        // Lọc công trình không hợp lệ
        result = works ? works.filter(
          (work) => 
            work.authors && 
            work.authors[0] && 
            work.authors[0].proofStatus === ProofStatus.KhongHopLe
        ) : [];
        break;
      case FilterType.PROOF_PENDING:
        // Lọc công trình chưa xử lý
        result = works ? works.filter(
          (work) => 
            work.authors && 
            work.authors[0] && 
            work.authors[0].proofStatus === ProofStatus.ChuaXuLy
        ) : [];
        break;
      case FilterType.MY_WORKS:
        // Lọc công trình như ở trang công trình (api getmywork)
        // Các công trình người dùng tự kê khai, không lấy công trình được admin import
        result = works ? works.filter(
          (work) => work.source === WorkSource.NguoiDungKeKhai
        ) : [];
        break;
      case FilterType.ALL:
      default:
        // Hiển thị tất cả
        result = works ? [...works] : [];
        break;
    }

    setFilteredWorks(result);
  }, [works, filterType, registeredWorks]);

  // Refetch dữ liệu khi academicYearId thay đổi
  useEffect(() => {
    if (academicYearId) {
      refetch();
      if (filterType === FilterType.REGISTERED) {
        refetchRegistered();
      }
    }
  }, [academicYearId, refetch, refetchRegistered, filterType]);

  const handleAcademicYearChange = (event: SelectChangeEvent) => {
    setAcademicYearId(event.target.value as string);
  };

  const handleFilterChange = (event: SelectChangeEvent) => {
    setFilterType(event.target.value as FilterType);
  };

  const handleResetFilter = () => {
    setFilterType(FilterType.ALL);
  };

  const columns: GridColDef[] = [
    {
      field: "title",
      headerName: "Tên công trình",
      width: 150,
      renderCell: (params: any) => {
        return (
          <Tooltip title={params.value} placement="top-start">
            <Typography
              variant="body2"
              sx={{
                overflow: "hidden",
                textOverflow: "ellipsis",
                display: "-webkit-box",
                WebkitLineClamp: 2,
                WebkitBoxOrient: "vertical",
              }}
            >
              {params.value}
            </Typography>
          </Tooltip>
        );
      },
    },
    {
      field: "timePublished",
      headerName: "Thời gian xuất bản",
      width: 170,
      renderCell: (params: any) => {
        try {
          if (!params.row?.timePublished) return <div>-</div>;
          return <div>{formatMonthYear(params.row.timePublished)}</div>;
        } catch (error) {
          return <div>-</div>;
        }
      },
    },
    {
      field: "workTypeName",
      headerName: "Loại công trình",
      width: 170,
    },
    {
      field: "workLevelName",
      headerName: "Cấp công trình",
      width: 170,
    },
    {
      field: "academicYearName",
      headerName: "Năm học",
      width: 130,
    },
    {
      field: "source",
      headerName: "Nguồn",
      width: 150,
      renderCell: (params: any) => {
        try {
          if (params.row?.source === undefined) return <div>-</div>;
          return <div>{params.row.source === WorkSource.QuanLyNhap ? "Quản lý nhập" : "Người dùng kê khai"}</div>;
        } catch (error) {
          return <div>-</div>;
        }
      },
    },
    {
      field: "authors",
      headerName: "Vai trò tác giả",
      width: 150,
      renderCell: (params: any) => {
        try {
          if (!params.row?.authors || params.row.authors.length === 0) return <div>-</div>;
          const currentUserAuthor = params.row.authors.find(
            (author: any) => author.userId === user?.id
          );
          return <div>{currentUserAuthor?.authorRoleName || "-"}</div>;
        } catch (error) {
          return <div>-</div>;
        }
      },
    },
    {
      field: "scoreLevel",
      headerName: "Mức điểm",
      width: 170,
      renderCell: (params: any) => {
        try {
          if (!params.row?.authors || params.row.authors.length === 0) return <div>-</div>;
          const currentUserAuthor = params.row.authors.find(
            (author: any) => author.userId === user?.id
          );
          if (currentUserAuthor?.scoreLevel === undefined) return <div>-</div>;
          return <div>{getScoreLevelText(currentUserAuthor.scoreLevel)}</div>;
        } catch (error) {
          return <div>-</div>;
        }
      },
    },
    {
      field: "proofStatus",
      headerName: "Trạng thái",
      type: "string",
      width: 140,
      renderCell: (params: any) => {
        try {
          if (!params.row?.authors) return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>-</div>;
                  
          // Lấy proofStatus từ author đầu tiên
          const author = params.row.authors[0];
          const proofStatus = author ? author.proofStatus : undefined;
                  
          if (proofStatus === undefined || proofStatus === null) {
            return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>-</div>;
          }
          
          // Kiểm tra giá trị và áp dụng trạng thái tương ứng
          if (proofStatus === ProofStatus.HopLe) {
            return (
              <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                <CheckCircleIcon color="success" />
                Hợp lệ
              </div>
            );
          } else if (proofStatus === ProofStatus.KhongHopLe) {
            return (
              <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                <CancelIcon color="error" />
                Không hợp lệ
              </div>
            );
          } else if (proofStatus === ProofStatus.ChuaXuLy) {
            return (
              <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                <HistoryIcon color="action" />
                Chưa xử lý
              </div>
            );
          } else {
            return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>-</div>;
          }
        } catch (error) {
          return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>-</div>;
        }
      },
    },
    {
      field: "isRegistered",
      headerName: "Đã đăng ký",
      width: 130,
      renderCell: (params: any) => {
        try {
          if (!params.row?.authors || params.row.authors.length === 0) {
            return (
              <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                <CancelIcon color="error" />
                Chưa đăng ký
              </div>
            );
          }
          
          // Tìm tác giả hiện tại
          const currentUserAuthor = params.row.authors.find(
            (author: any) => author.userId === user?.id
          );
          
          if (!currentUserAuthor) {
            return (
              <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                <CancelIcon color="error" />
                Chưa đăng ký
              </div>
            );
          }
          
          // Kiểm tra authorRegistration
          const isRegistered = currentUserAuthor.authorRegistration !== null && 
                              currentUserAuthor.authorRegistration !== undefined;
          
          console.log(`Work: ${params.row.title}, AuthorID: ${currentUserAuthor.id}, Registration:`, currentUserAuthor.authorRegistration);
          
          return isRegistered ? (
            <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
              <CheckCircleIcon color="success" />
              Đã đăng ký
            </div>
          ) : (
            <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
              <CancelIcon color="error" />
              Chưa đăng ký
            </div>
          );
        } catch (error) {
          console.error("Error rendering isRegistered cell:", error);
          return (
            <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
              <CancelIcon color="error" />
              Chưa đăng ký
            </div>
          );
        }
      },
    },
  ];

  return (
    <Container maxWidth="xl">
      <Box sx={{ py: 3 }}>

        <Grid container spacing={2} sx={{ mb: 3, mt: 1 }}>
          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth>
              <InputLabel id="academic-year-label">Năm học</InputLabel>
              <Select
                labelId="academic-year-label"
                value={academicYearId}
                label="Năm học"
                onChange={handleAcademicYearChange}
                disabled={isLoadingAcademicYears}
              >
                {academicYears?.map((year: AcademicYear) => (
                  <MenuItem key={year.id} value={year.id}>
                    {year.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth>
              <InputLabel id="filter-label">Lọc theo</InputLabel>
              <Select
                labelId="filter-label"
                value={filterType}
                label="Lọc theo"
                onChange={handleFilterChange}
              >
                <MenuItem value={FilterType.ALL}>Tất cả công trình</MenuItem>
                <MenuItem value={FilterType.REGISTERED}>Đã đăng ký quy đổi</MenuItem>
                <MenuItem value={FilterType.SOURCE_IMPORT}>Admin import</MenuItem>
                <MenuItem value={FilterType.SOURCE_USER}>Người dùng kê khai</MenuItem>
                <MenuItem value={FilterType.PROOF_VALID}>Trạng thái: Hợp lệ</MenuItem>
                <MenuItem value={FilterType.PROOF_INVALID}>Trạng thái: Không hợp lệ</MenuItem>
                <MenuItem value={FilterType.PROOF_PENDING}>Trạng thái: Chưa xử lý</MenuItem>
                <MenuItem value={FilterType.MY_WORKS}>Chỉ công trình tự kê khai</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} sm={12} md={6} sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'center', gap: 1 }}>
            <Button 
              startIcon={<FilterListIcon />} 
              variant="outlined"
              onClick={() => filterType === FilterType.REGISTERED ? refetchRegistered() : refetch()}
              disabled={isLoadingWorks || isLoadingRegisteredWorks}
            >
              Cập nhật
            </Button>
            <Button 
              startIcon={<RestartAltIcon />} 
              color="secondary" 
              onClick={handleResetFilter}
              disabled={filterType === FilterType.ALL}
            >
              Xóa bộ lọc
            </Button>
          </Grid>
        </Grid>

        {isError && (
          <Typography color="error" sx={{ mb: 2 }}>
            Đã có lỗi xảy ra: {(error as Error)?.message || "Không thể tải dữ liệu"}
          </Typography>
        )}

        {isLoadingWorks || isLoadingRegisteredWorks ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
            <CircularProgress />
          </Box>
        ) : (
          <GenericTable
            data={filteredWorks || []}
            columns={columns}
          />
        )}

        <Box sx={{ mt: 2, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant="subtitle2" color="text.secondary">
            Tổng số công trình: {filteredWorks?.length || 0}
          </Typography>
        </Box>
      </Box>
    </Container>
  );
} 