import CancelIcon from "@mui/icons-material/Cancel";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import FilterListIcon from "@mui/icons-material/FilterList";
import HistoryIcon from "@mui/icons-material/History";
import RestartAltIcon from "@mui/icons-material/RestartAlt";
import SearchIcon from "@mui/icons-material/Search";
import {
  Alert,
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
  Typography,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useQuery } from "@tanstack/react-query";
import React, { useEffect, useState } from "react";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { getAcademicYears } from "../../lib/api/academicYearApi";
import {
  getDepartments,
  getDepartmentsByManagerId,
} from "../../lib/api/departmentsApi";
import { getUsersByDepartmentId } from "../../lib/api/usersApi";
import { getWorksWithFilter } from "../../lib/api/worksApi";
import { ProofStatus } from "../../lib/types/enums/ProofStatus";
import { WorkSource } from "../../lib/types/enums/WorkSource";
import { AcademicYear } from "../../lib/types/models/AcademicYear";
import { Department } from "../../lib/types/models/Department";
import { User } from "../../lib/types/models/User";
import { Work } from "../../lib/types/models/Work";
import { getScoreLevelText } from "../../lib/utils/scoreLevelUtils";

interface FilterParams {
  academicYearId?: string;
  departmentId?: string;
  userId?: string;
  proofStatus?: number;
  source?: number;
  onlyRegisteredWorks: boolean;
}

const StatisticsPage: React.FC = () => {
  // State
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [works, setWorks] = useState<Work[]>([]);
  const [academicYears, setAcademicYears] = useState<AcademicYear[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const { user } = useAuth();

  // Filter state
  const [filter, setFilter] = useState<FilterParams>({
    academicYearId: "",
    departmentId: "",
    userId: "",
    proofStatus: undefined,
    source: undefined,
    onlyRegisteredWorks: false,
  });

  // Load initial data
  useEffect(() => {
    const fetchData = async () => {
      try {
        const [academicYearsResponse, departmentsResponse] = await Promise.all([
          getAcademicYears(),
          user?.role === "Manager"
            ? getDepartmentsByManagerId(user.id)
            : getDepartments(),
        ]);

        setAcademicYears(academicYearsResponse.data || []);
        setDepartments(departmentsResponse.data || []);
      } catch (error) {
        setError("Lỗi khi tải dữ liệu ban đầu");
        console.error("Error fetching initial data:", error);
      }
    };

    fetchData();
  }, [user]);

  // Lấy danh sách người dùng theo phòng ban
  const { data: usersData } = useQuery({
    queryKey: ["users", "department", filter.departmentId],
    queryFn: () => getUsersByDepartmentId(filter.departmentId || ""),
    enabled: !!filter.departmentId,
  });

  const users: User[] = usersData?.data || [];

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

    // Reset userId khi thay đổi department
    if (name === "departmentId") {
      setFilter((prev) => ({ ...prev, userId: "" }));
    }
  };

  // Handle API calls
  const fetchWorks = async () => {
    setLoading(true);
    setError(null);

    try {
      const response = await getWorksWithFilter({
        ...filter,
        isCurrentUser: false,
      });
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
      departmentId: "",
      userId: "",
      proofStatus: undefined,
      source: undefined,
      onlyRegisteredWorks: false,
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
  // const getProofStatusChip = (status: number) => {
  //   switch (status) {
  //     case ProofStatus.HopLe:
  //       return (
  //         <Tooltip title="Hợp lệ">
  //           <Chip
  //             icon={<CheckCircleIcon />}
  //             label="Hợp lệ"
  //             color="success"
  //             size="small"
  //           />
  //         </Tooltip>
  //       );
  //     case ProofStatus.KhongHopLe:
  //       return (
  //         <Tooltip title="Không hợp lệ">
  //           <Chip
  //             icon={<CancelIcon />}
  //             label="Không hợp lệ"
  //             color="error"
  //             size="small"
  //           />
  //         </Tooltip>
  //       );
  //     case ProofStatus.ChuaXuLy:
  //       return (
  //         <Tooltip title="Chưa xử lý">
  //           <Chip
  //             icon={<HistoryIcon />}
  //             label="Chưa xử lý"
  //             color="warning"
  //             size="small"
  //           />
  //         </Tooltip>
  //       );
  //     default:
  //       return null;
  //   }
  // };

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
      field: "departmentName",
      headerName: "Phòng ban",
      type: "string",
      width: 200,
    },
    {
      field: "authorRoleName",
      headerName: "Vai trò tác giả",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        return <div>{currentAuthor ? currentAuthor.authorRoleName : "-"}</div>;
      },
    },
    {
      field: "position",
      headerName: "Vị trí",
      type: "string",
      width: 80,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        return (
          <div>
            {currentAuthor?.position !== undefined &&
            currentAuthor?.position !== null
              ? currentAuthor.position
              : "-"}
          </div>
        );
      },
    },
    {
      field: "purposeName",
      headerName: "Mục đích quy đổi",
      type: "string",
      width: 180,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        return <div>{currentAuthor ? currentAuthor.purposeName : "-"}</div>;
      },
    },
    {
      field: "fieldName",
      headerName: "Ngành tính điểm",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        return <div>{currentAuthor ? currentAuthor.fieldName : "-"}</div>;
      },
    },
    {
      field: "scImagoFieldName",
      headerName: "Ngành SCImago",
      type: "string",
      width: 180,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        return (
          <div>{currentAuthor ? currentAuthor.scImagoFieldName : "-"}</div>
        );
      },
    },
    {
      field: "scoreLevel",
      headerName: "Mức điểm",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        if (
          !currentAuthor ||
          currentAuthor.scoreLevel === undefined ||
          currentAuthor.scoreLevel === null
        ) {
          return <div>-</div>;
        }
        return <div>{getScoreLevelText(currentAuthor.scoreLevel)}</div>;
      },
    },
    {
      field: "workHour",
      headerName: "Giờ công trình",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        return (
          <div>
            {currentAuthor?.workHour !== undefined &&
            currentAuthor?.workHour !== null
              ? currentAuthor.workHour
              : "-"}
          </div>
        );
      },
    },
    {
      field: "authorHour",
      headerName: "Giờ tác giả",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        return (
          <div>
            {currentAuthor?.authorHour !== undefined &&
            currentAuthor?.authorHour !== null
              ? currentAuthor.authorHour
              : "-"}
          </div>
        );
      },
    },
    {
      field: "proofStatus",
      headerName: "Trạng thái",
      type: "string",
      width: 140,
      renderCell: (params: any) => {
        const work = params.row;
        const currentAuthor = work.authors?.find(
          (author) => author.userId === user?.id,
        );
        const proofStatus = currentAuthor?.proofStatus;

        if (proofStatus === undefined || proofStatus === null) {
          return (
            <div style={{ display: "flex", alignItems: "center", gap: "4px" }}>
              -
            </div>
          );
        }

        if (proofStatus === ProofStatus.HopLe) {
          return (
            <div style={{ display: "flex", alignItems: "center", gap: "4px" }}>
              <CheckCircleIcon color="success" />
              Hợp lệ
            </div>
          );
        } else if (proofStatus === ProofStatus.KhongHopLe) {
          return (
            <div style={{ display: "flex", alignItems: "center", gap: "4px" }}>
              <CancelIcon color="error" />
              Không hợp lệ
            </div>
          );
        } else if (proofStatus === ProofStatus.ChuaXuLy) {
          return (
            <div style={{ display: "flex", alignItems: "center", gap: "4px" }}>
              <HistoryIcon color="action" />
              Chưa xử lý
            </div>
          );
        } else {
          return (
            <div style={{ display: "flex", alignItems: "center", gap: "4px" }}>
              -
            </div>
          );
        }
      },
    },
    {
      field: "isRegistered",
      headerName: "Trạng thái đăng ký",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        const isRegistered = author?.authorRegistration != null;

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
              <InputLabel>Phòng ban</InputLabel>
              <Select
                name="departmentId"
                value={filter.departmentId || ""}
                onChange={handleSelectChange}
                label="Phòng ban"
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
          </Grid>

          <Grid item xs={12} sm={6} md={3}>
            <FormControl fullWidth size="small">
              <InputLabel>Người dùng</InputLabel>
              <Select
                name="userId"
                value={filter.userId || ""}
                onChange={handleSelectChange}
                label="Người dùng"
                disabled={!filter.departmentId}
              >
                <MenuItem value="">
                  <em>Không chọn</em>
                </MenuItem>
                {users.map((user) => (
                  <MenuItem key={user.id} value={user.id}>
                    {user.userName} - {user.fullName}
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

          <Grid item xs={12}>
            <FormControlLabel
              control={
                <Checkbox
                  checked={filter.onlyRegisteredWorks}
                  onChange={handleCheckboxChange}
                  name="onlyRegisteredWorks"
                />
              }
              label="Chỉ hiển thị công trình đã đăng ký"
            />
          </Grid>
        </Grid>
      </Paper>

      {/* Results Panel */}
      <Paper sx={{ p: 3, borderRadius: 2, boxShadow: 3 }}>
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
    </Container>
  );
};

export default StatisticsPage;
