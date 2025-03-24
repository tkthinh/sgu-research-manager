import {
  Box,
  CircularProgress,
  Paper,
  Switch,
  Tooltip,
  Alert,
  FormControlLabel
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { getMyWorks, setMarkedForScoring } from "../../lib/api/worksApi";
import { Work } from "../../lib/types/models/Work";
import { ScoreLevel } from "../../lib/types/enums/ScoreLevel";
import { getScoreLevelText } from '../../lib/utils/scoreLevelUtils';

export default function MarkedWorksPage() {
  const queryClient = useQueryClient();
  const [markingAuthorId, setMarkingAuthorId] = useState<string | null>(null);

  // Fetch my works
  const { data, error, isPending, refetch } = useQuery({
    queryKey: ["myWorks"],
    queryFn: getMyWorks,
    staleTime: 0, // Luôn refetch khi cần
  });

  // Mutation để cập nhật trạng thái đánh dấu
  const markForScoringMutation = useMutation({
    mutationFn: (params: { authorId: string; marked: boolean }) => {
      setMarkingAuthorId(params.authorId);
      console.log(`Đánh dấu authorId ${params.authorId} với trạng thái ${params.marked}`);
      return setMarkedForScoring(params.authorId, params.marked);
    },
    onSuccess: () => {
      toast.success("Cập nhật trạng thái đánh dấu thành công");
      
      // Xóa cache và refetch dữ liệu mới
      queryClient.removeQueries({ queryKey: ["myWorks"] });
      
      // Bắt buộc refetch dữ liệu ngay lập tức
      setTimeout(() => {
        refetch();
        setMarkingAuthorId(null);
      }, 100);
    },
    onError: (error: any) => {
      console.error("Lỗi khi cập nhật trạng thái đánh dấu:", error);
      // Kiểm tra nếu lỗi chứa thông báo về vượt quá giới hạn
      const errorMessage = error.response?.data?.message || error.message || 'Đã có lỗi xảy ra';
      if (errorMessage.includes("vượt quá giới hạn") || errorMessage.includes("quá giới hạn")) {
        toast.error(`Không thể đánh dấu: ${errorMessage}`, { autoClose: 7000 });
      } else {
        toast.error(`Lỗi khi cập nhật trạng thái đánh dấu: ${errorMessage}`);
      }
      setMarkingAuthorId(null);
    },
  });

  const handleMarkForScoring = async (authorId: string, currentMarked: boolean) => {
    try {
      await markForScoringMutation.mutateAsync({
        authorId,
        marked: !currentMarked
      });
    } catch (error) {
      // Lỗi đã được xử lý trong onError của mutation
    }
  };

  const columns: GridColDef[] = [
    {
      field: "stt",
      headerName: "STT",
      width: 70,
      renderCell: (params: any) => {
        if (!params || !params.row) return null;
        const rowIndex = Array.from(params.api.getAllRowIds()).findIndex((id: any) => id === params.row.id);
        return <div>{rowIndex + 1}</div>;
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
        if (!params || !params.row) return <div>Không xác định</div>;
        return <div>{params.row.workTypeName || "Không xác định"}</div>;
      },
    },
    {
      field: "workLevelName",
      headerName: "Cấp công trình",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>Không xác định</div>;
        return <div>{params.row.workLevelName || "Không xác định"}</div>;
      },
    },
    {
      field: "purposeName",
      headerName: "Mục đích quy đổi",
      type: "string",
      width: 180,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>Không xác định</div>;
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.purposeName : "Không xác định"}</div>;
      },
    },
    {
      field: "authorRoleName",
      headerName: "Vai trò tác giả",
      type: "string",
      width: 130,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>Không xác định</div>;
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.authorRoleName : "Không xác định"}</div>;
      },
    },
    {
      field: "scoreLevel",
      headerName: "Mức điểm",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        if (!author || author.scoreLevel === undefined || author.scoreLevel === null) {
          return <div>Không xác định</div>;
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
        if (!params || !params.row) return <div>Không xác định</div>;
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.authorHour !== undefined && author?.authorHour !== null ? author.authorHour : "Không xác định"}</div>;
      },
    },
    {
      field: "markedForScoring",
      headerName: "Đánh dấu",
      width: 120,
      renderCell: (params: any) => {
        if (!params || !params.row) return <div>-</div>;
        const author = params.row.authors && params.row.authors[0];
        if (!author) return <div>-</div>;
        
        const isMarked = author.markedForScoring === true;
        const authorId = author.id;
        const isLoading = markingAuthorId === authorId;
        
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
                      disabled={markForScoringMutation.isPending}
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
  ];

  // Chỉ lấy các công trình mà người dùng là tác giả (có thông tin author)
  const filteredWorks = data?.data?.filter((work: Work) => work.authors && work.authors.length > 0) || [];

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
      
      <Alert severity="info" sx={{ mb: 2 }}>
        Đánh dấu các công trình bạn muốn sử dụng để quy đổi. Lưu ý rằng mỗi loại công trình, cấp và mức điểm có giới hạn số lượng công trình được phép đánh dấu.
      </Alert>
      
      <Paper sx={{ width: "100%", overflow: "hidden" }}>
        <GenericTable 
          columns={columns} 
          data={filteredWorks} 
        />
      </Paper>
    </>
  );
} 