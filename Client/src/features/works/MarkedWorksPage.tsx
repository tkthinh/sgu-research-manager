import {
  Box,
  CircularProgress,
  Paper,
  Checkbox,
  Tooltip,
  Alert,
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
import { useState, useEffect, useCallback, useMemo } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { getMyWorks, setMarkedForScoring } from "../../lib/api/worksApi";
import { getUserConversionResult } from "../../lib/api/usersApi"; 
import { Work } from "../../lib/types/models/Work";
import { getScoreLevelText } from '../../lib/utils/scoreLevelUtils';
import { useAuth } from "../../app/shared/contexts/AuthContext";

export default function MarkedWorksPage() {
  const queryClient = useQueryClient();
  const [markingAuthorId, setMarkingAuthorId] = useState<string | null>(null);
  const [optimisticUpdates, setOptimisticUpdates] = useState<Map<string, boolean>>(new Map());
  const { user } = useAuth();
  
  // Fetch my works với caching cải tiến
  const { data, error, isPending, refetch } = useQuery({
    queryKey: ["myWorks"],
    queryFn: getMyWorks,
    staleTime: 30000, // Cache trong 30 giây để tránh refetch không cần thiết
    refetchOnWindowFocus: false, // Không refetch khi focus lại cửa sổ
  });

  // Fetch conversion result với caching tương tự
  const { data: conversionData, isPending: isConversionPending, refetch: refetchConversion } = useQuery({
    queryKey: ["userConversionResult", user?.id],
    queryFn: () => getUserConversionResult(user?.id || ""),
    staleTime: 30000,
    refetchOnWindowFocus: false,
    enabled: !!user?.id,
  });

  // Kiểm tra xem tác giả có được đánh dấu không, ưu tiên trạng thái optimistic
  const isAuthorMarked = useCallback((author: any): boolean => {
    if (!author) return false;
    
    // Kiểm tra optimistic update trước
    if (optimisticUpdates.has(author.id)) {
      return optimisticUpdates.get(author.id) as boolean;
    }
    
    // Nếu không có optimistic update, kiểm tra dữ liệu từ API
    return author.authorRegistration !== null && author.authorRegistration !== undefined;
  }, [optimisticUpdates]);

  // Mutation để cập nhật trạng thái đánh dấu với optimistic updates
  const markForScoringMutation = useMutation({
    mutationFn: (params: { authorId: string; marked: boolean }) => {
      setMarkingAuthorId(params.authorId);
      
      // Cập nhật optimistic ngay lập tức
      setOptimisticUpdates(prev => {
        const newMap = new Map(prev);
        newMap.set(params.authorId, params.marked);
        return newMap;
      });
      
      return setMarkedForScoring(params.authorId, params.marked);
    },
    onSuccess: (_, variables) => {
      const { authorId } = variables;
      toast.success("Cập nhật trạng thái đánh dấu thành công");
      
      // Xóa cache cũ
      queryClient.invalidateQueries({ queryKey: ["myWorks"] });
      queryClient.invalidateQueries({ queryKey: ["userConversionResult", user?.id] });
      
      // Đặt thời gian ngắn hơn để cải thiện trải nghiệm người dùng
      setTimeout(() => {
        Promise.all([refetch(), refetchConversion()]).then(() => {
          // Xóa optimistic update sau khi dữ liệu được refetch thành công
          setOptimisticUpdates(prev => {
            const newMap = new Map(prev);
            newMap.delete(authorId);
            return newMap;
          });
          setMarkingAuthorId(null);
        });
      }, 300); // Giảm thời gian chờ xuống còn 300ms
    },
    onError: (error: any, variables) => {
      const { authorId } = variables;
      
      // Hủy bỏ optimistic update khi có lỗi
      setOptimisticUpdates(prev => {
        const newMap = new Map(prev);
        newMap.delete(authorId);
        return newMap;
      });
      
      // Kiểm tra nếu lỗi chứa thông báo về vượt quá giới hạn
      const errorMessage = error.response?.data?.message || error.message || 'Đã có lỗi xảy ra';
      if (errorMessage.includes("vượt quá giới hạn") || errorMessage.includes("quá giới hạn")) {
        toast.error(`Không thể đánh dấu: ${errorMessage}`, { autoClose: 7000 });
      } else {
        toast.error(`Lỗi khi cập nhật trạng thái đánh dấu: ${errorMessage}`);
      }
      setMarkingAuthorId(null);
      
      // Refetch dữ liệu để đảm bảo UI đồng bộ với server
      refetch();
    },
  });

  const handleMarkForScoring = useCallback(async (authorId: string, isCurrentlyMarked: boolean) => {
    try {
      await markForScoringMutation.mutateAsync({
        authorId,
        marked: !isCurrentlyMarked
      });
    } catch (error) {
      // Lỗi đã được xử lý trong onError của mutation
    }
  }, [markForScoringMutation]);

  // Tạo columns bên ngoài render cycle để tối ưu performance
  const columns: GridColDef[] = useMemo(() => [
    {
      field: "markedForScoring",
      headerName: "Đánh dấu",
      width: 100,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>-</div>;
        const author = params.row.authors?.find((a: any) => a.userId === user?.id);
        if (!author) return <div>-</div>;
        
        const isMarked = isAuthorMarked(author);
        const authorId = author.id;
        const isLoading = markingAuthorId === authorId;
        
        return (
          <Tooltip 
            title={isMarked 
                ? "Bỏ đánh dấu công trình này" 
                : "Đánh dấu công trình này để tính điểm"
            }
          >
            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
              {isLoading ? (
                <CircularProgress size={20} />
              ) : (
                <Checkbox
                  checked={isMarked}
                  onChange={() => handleMarkForScoring(authorId, isMarked)}
                  disabled={markForScoringMutation.isPending}
                  color="primary"
                />
              )}
            </div>
          </Tooltip>
        );
      },
    },
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
        const author = params.row.authors?.find((a: any) => a.userId === user?.id);
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
        const author = params.row.authors?.find((a: any) => a.userId === user?.id);
        return <div>{author ? author.authorRoleName : "-"}</div>;
      },
    },
    {
      field: "scoreLevel",
      headerName: "Thành tích",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors?.find((a: any) => a.userId === user?.id);
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
        const author = params.row.authors?.find((a: any) => a.userId === user?.id);
        return <div>{author?.authorHour !== undefined && author?.authorHour !== null ? author.authorHour : "-"}</div>;
      },
    },
  ], [user?.id, isAuthorMarked, markingAuthorId, markForScoringMutation.isPending, handleMarkForScoring]);

  // Trước tính toán filteredWorks chỉ khi data thay đổi
  const filteredWorks = useMemo(() => data?.data?.filter((work: Work) => 
    work.authors && work.authors.some(author => author.userId === user?.id)
  ) || [], [data?.data, user?.id]);

  // Preload dữ liệu khi trang vừa tải xong
  useEffect(() => {
    const preloadData = async () => {
      try {
        // Invalidate và refetch dữ liệu ngay lập tức khi component mount
        await Promise.all([
          queryClient.prefetchQuery({
            queryKey: ["myWorks"],
            queryFn: getMyWorks
          }),
          user?.id ? queryClient.prefetchQuery({
            queryKey: ["userConversionResult", user?.id],
            queryFn: () => getUserConversionResult(user?.id || "")
          }) : Promise.resolve()
        ]);
      } catch (error) {
        console.error("Lỗi khi preload dữ liệu:", error);
      }
    };
    
    preloadData();
  }, [queryClient, user?.id]);

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
        Đăng ký quy đổi giờ nghiên cứu khoa học
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