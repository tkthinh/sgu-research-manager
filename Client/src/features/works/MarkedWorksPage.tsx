import {
  Box,
  CircularProgress,
  Paper,
  Switch,
  Tooltip,
  Alert,
  FormControlLabel,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useState, useEffect, useCallback } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { getMyWorks, setMarkedForScoring } from "../../lib/api/worksApi";
import { getUserConversionResult } from "../../lib/api/usersApi"; 
import { Work } from "../../lib/types/models/Work";
import { getScoreLevelText } from '../../lib/utils/scoreLevelUtils';
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { debounce } from 'lodash';

export default function MarkedWorksPage() {
  const queryClient = useQueryClient();
  const [markingAuthorId, setMarkingAuthorId] = useState<string | null>(null);
  const [processingAuthorIds, setProcessingAuthorIds] = useState<Set<string>>(new Set());
  const { user } = useAuth();
  const [localWorks, setLocalWorks] = useState<Work[]>([]);

  // Fetch my works
  const { data, error, isPending, refetch } = useQuery({
    queryKey: ["myWorks"],
    queryFn: getMyWorks,
    staleTime: 0, // Luôn refetch khi cần
  });

  // Cập nhật localWorks khi data thay đổi
  useEffect(() => {
    if (data?.data) {
      setLocalWorks(data.data);
    }
  }, [data]);

  // Fetch conversion result
  const { data: conversionData, isPending: isConversionPending, refetch: refetchConversion } = useQuery({
    queryKey: ["userConversionResult", user?.id],
    queryFn: () => getUserConversionResult(user?.id || ""),
    staleTime: 0,
    enabled: !!user?.id,
  });

  // Mutation để cập nhật trạng thái đánh dấu
  const markForScoringMutation = useMutation({
    mutationFn: (params: { authorId: string; marked: boolean }) => {
      setMarkingAuthorId(params.authorId);
      console.log(`Đánh dấu authorId ${params.authorId} với trạng thái ${params.marked}`);
      return setMarkedForScoring(params.authorId, params.marked);
    },
    onSuccess: (_, variables) => {
      toast.success("Cập nhật trạng thái đánh dấu thành công");
      
      // Cập nhật UI ngay lập tức
      setLocalWorks(prevWorks => {
        return prevWorks.map(work => {
          if (work.authors && work.authors.length > 0) {
            const authorIndex = work.authors.findIndex(a => a.id === variables.authorId);
            if (authorIndex >= 0) {
              // Tạo bản sao của mảng authors
              const updatedAuthors = [...work.authors];
              // Cập nhật thuộc tính authorRegistration của author
              updatedAuthors[authorIndex] = {
                ...updatedAuthors[authorIndex],
                authorRegistration: variables.marked ? {
                  authorId: variables.authorId,
                  academicYearId: '', // ID năm học sẽ được backend xử lý
                } : null
              };
              // Trả về work đã cập nhật với authors mới
              return { ...work, authors: updatedAuthors };
            }
          }
          return work;
        });
      });
      
      // Xóa cache và refetch dữ liệu mới
      queryClient.removeQueries({ queryKey: ["myWorks"] });
      queryClient.removeQueries({ queryKey: ["userConversionResult", user?.id] });
      
      // Bắt buộc refetch dữ liệu sau một khoảng thời gian ngắn
      setTimeout(() => {
        refetch();
        refetchConversion();
        setMarkingAuthorId(null);
        // Xóa authorId khỏi danh sách đang xử lý
        setProcessingAuthorIds(prev => {
          const newSet = new Set(prev);
          newSet.delete(variables.authorId);
          return newSet;
        });
      }, 500);
    },
    onError: (error: any, variables) => {
      console.error("Lỗi khi cập nhật trạng thái đánh dấu:", error);
      // Kiểm tra nếu lỗi chứa thông báo về vượt quá giới hạn
      const errorMessage = error.response?.data?.message || error.message || 'Đã có lỗi xảy ra';
      if (errorMessage.includes("vượt quá giới hạn") || errorMessage.includes("quá giới hạn")) {
        toast.error(`Không thể đánh dấu: ${errorMessage}`, { autoClose: 7000 });
      } else {
        toast.error(`Lỗi khi cập nhật trạng thái đánh dấu: ${errorMessage}`);
      }
      setMarkingAuthorId(null);
      
      // Xóa authorId khỏi danh sách đang xử lý
      setProcessingAuthorIds(prev => {
        const newSet = new Set(prev);
        newSet.delete(variables.authorId);
        return newSet;
      });
      
      // Refetch để đảm bảo hiển thị dữ liệu chính xác
      refetch();
    },
  });

  // eslint-disable-next-line react-hooks/exhaustive-deps
  const debouncedMarkForScoring = useCallback(
    debounce((authorId: string, currentMarked: boolean) => {
      // Thêm authorId vào danh sách đang xử lý
      setProcessingAuthorIds(prev => {
        const newSet = new Set(prev);
        newSet.add(authorId);
        return newSet;
      });
      
      // Gọi mutation với trạng thái ngược lại của currentMarked
      markForScoringMutation.mutateAsync({
        authorId,
        marked: !currentMarked
      }).catch(() => {
        // Lỗi đã được xử lý trong onError của mutation
      });
    }, 300),
    [markForScoringMutation] // Phụ thuộc vào mutation
  );

  const handleMarkForScoring = async (authorId: string, currentMarked: boolean) => {
    // Kiểm tra xem authorId đã đang được xử lý chưa
    if (processingAuthorIds.has(authorId)) {
      console.log(`Author ${authorId} đang được xử lý, bỏ qua lệnh gọi trùng lặp`);
      return;
    }
    
    // Gọi hàm debounced để tránh gọi nhiều lần
    debouncedMarkForScoring(authorId, currentMarked);
  };

  const columns: GridColDef[] = [
    {
      field: "markedForScoring",
      headerName: "Đánh dấu",
      width: 100,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>-</div>;
        const author = params.row.authors && params.row.authors[0];
        if (!author) return <div>-</div>;
        
        // Kiểm tra xem tác giả có được đăng ký (có AuthorRegistration) hay không
        const isMarked = author.authorRegistration != null;
        const authorId = author.id;
        const isLoading = markingAuthorId === authorId || processingAuthorIds.has(authorId);
        
        return (
          <Tooltip 
            title={isMarked 
                ? "Bỏ đánh dấu công trình này" 
                : "Đánh dấu công trình này để tính điểm"
            }
          >
            <span>
              {isLoading ? (
                <CircularProgress size={24} />
              ) : (
                <FormControlLabel
                  control={
                    <Switch
                      checked={isMarked}
                      onChange={() => handleMarkForScoring(authorId, isMarked)}
                      disabled={markForScoringMutation.isPending || processingAuthorIds.size > 0}
                      color="primary"
                    />
                  }
                  label=""
                />
              )}
            </span>
          </Tooltip>
        );
      },
    },
    // {
    //   field: "stt",
    //   headerName: "STT",
    //   width: 50,
    //   renderCell: (params: any) => {
    //     if (!params || !params.row) return null;
    //     const rowIndex = Array.from(params.api.getAllRowIds()).findIndex((id: any) => id === params.row.id);
    //     return <div>{rowIndex + 1}</div>;
    //   },
    // },
    {
      field: "title",
      headerName: "Tên công trình",
      type: "string",
      flex: 1,
      minWidth: 150,
    },
    {
      field: "workTypeName",
      headerName: "Loại công trình",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>-</div>;
        return <div>{params.row.workTypeName || "-"}</div>;
      },
    },
    {
      field: "workLevelName",
      headerName: "Cấp công trình",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>-</div>;
        return <div>{params.row.workLevelName || "-"}</div>;
      },
    },
    {
      field: "purposeName",
      headerName: "Mục đích quy đổi",
      type: "string",
      width: 180,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>-</div>;
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.purposeName : "-"}</div>;
      },
    },
    {
      field: "authorRoleName",
      headerName: "Vai trò tác giả",
      type: "string",
      width: 130,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>-</div>;
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.authorRoleName : "-"}</div>;
      },
    },
    {
      field: "scoreLevel",
      headerName: "Thành tích",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        if (!author || author.scoreLevel === undefined || author.scoreLevel === null) {
          return <div>-</div>;
        }
        return <div>{getScoreLevelText(author.scoreLevel)}</div>;
      },
    },
    {
      field: "authorHour",
      headerName: "Giờ tác giả",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>-</div>;
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.authorHour !== undefined && author?.authorHour !== null ? author.authorHour : "-"}</div>;
      },
    },
    
  ];

  // Chỉ lấy các công trình mà người dùng là tác giả (có thông tin author)
  const filteredWorks = localWorks?.filter((work: Work) => work.authors && work.authors.length > 0) || [];

  if (isPending) return <CircularProgress />;
  if (error) return <p>Lỗi: {(error as Error).message}</p>;

  return (
    <>
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        sx={{ marginBottom: 2 }}
      >
      </Box>
      <Typography variant="h6" sx={{ mb: 2, mt: 3 }}>
        Đánh dấu công trình
      </Typography>
      <Alert severity="info" sx={{ mb: 2 }}>
        Đánh dấu các công trình bạn muốn sử dụng để quy đổi. Lưu ý rằng mỗi loại công trình, cấp và mức điểm có giới hạn số lượng công trình được phép đánh dấu.
      </Alert>
      
      <Paper sx={{ width: "100%", overflow: "hidden", mb: 3 }}>
        <GenericTable 
          columns={columns} 
          data={filteredWorks} 
        />
      </Paper>

      {/* Bảng kết quả quy đổi */}
      <Typography variant="h6" sx={{ mb: 2, mt: 3 }}>
        Kết quả quy đổi
      </Typography>

      {isConversionPending ? (
        <CircularProgress size={24} />
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Mục đích quy đổi</TableCell>
                <TableCell align="center">Số công trình</TableCell>
                <TableCell align="center">Số giờ được quy đổi</TableCell>
                <TableCell align="center">Số giờ được tính</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              <TableRow>
                <TableCell>Quy đổi giờ nghĩa vụ</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.dutyHourConversion?.totalWorks || 0}</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.dutyHourConversion?.totalConvertedHours || 0}</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.dutyHourConversion?.totalCalculatedHours || 0}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Quy đổi vượt định mức</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.overLimitConversion?.totalWorks || 0}</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.overLimitConversion?.totalConvertedHours || 0}</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.overLimitConversion?.totalCalculatedHours || 0}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Sản phẩm của đề tài NCKH</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.researchProductConversion?.totalWorks || 0}</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.researchProductConversion?.totalConvertedHours || 0}</TableCell>
                <TableCell align="center">{conversionData?.data?.conversionResults?.researchProductConversion?.totalCalculatedHours || 0}</TableCell>
              </TableRow>
              <TableRow sx={{ backgroundColor: 'rgba(0, 0, 0, 0.04)' }}>
                <TableCell><strong>Tổng cộng</strong></TableCell>
                <TableCell align="center"><strong>{conversionData?.data?.conversionResults?.totalWorks || 0}</strong></TableCell>
                <TableCell align="center">-</TableCell>
                <TableCell align="center"><strong>{conversionData?.data?.conversionResults?.totalCalculatedHours || 0}</strong></TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </>
  );
} 