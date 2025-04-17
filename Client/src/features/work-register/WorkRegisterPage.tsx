import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Typography,
  Container,
  Tooltip,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState, useCallback } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { getWorksWithFilter, registerWorkByAuthor } from "../../lib/api/worksApi";
import { formatMonthYear } from "../../lib/utils/dateUtils";
import { ProofStatus } from "../../lib/types/enums/ProofStatus";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import CancelIcon from "@mui/icons-material/Cancel";
import HistoryIcon from "@mui/icons-material/History";
import { getUserById } from "../../lib/api/usersApi";
import { User } from "../../lib/types/models/User";
import { getScoreLevelText } from '../../lib/utils/scoreLevelUtils';
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { getCurrentAcademicYear } from "../../lib/api/academicYearApi";

export default function WorkRegisterPage() {
  const queryClient = useQueryClient();
  const [coAuthorsMap, setCoAuthorsMap] = useState<Record<string, User[]>>({});
  const { user } = useAuth();
  const [localWorks, setLocalWorks] = useState<any[]>([]);
  const [optimisticUpdates, setOptimisticUpdates] = useState<Map<string, boolean>>(new Map());

  // Fetch năm học hiện tại
  const { data: currentAcademicYear } = useQuery({
    queryKey: ["current-academic-year"],
    queryFn: getCurrentAcademicYear,
  });

  // Fetch works dựa vào filter mặc định
  const { 
    data: worksData, 
    error: worksError, 
    isPending: isLoadingWorks, 
    refetch 
  } = useQuery({
    queryKey: ["works", "registerable-works"],
    queryFn: async () => {
      const filter = {
        academicYearId: currentAcademicYear?.data?.id,
        isCurrentUser: true
      };
      const response = await getWorksWithFilter(filter);
      
      // Log dữ liệu để kiểm tra
      console.log("Dữ liệu từ API:", response.data);
      
      return response;
    },
    enabled: !!currentAcademicYear?.data?.id,
    staleTime: 0,
  });

  // Lấy thông tin đồng tác giả khi có dữ liệu công trình
  useEffect(() => {
    if (worksData?.data && worksData.data.length > 0) {
      const fetchCoAuthors = async () => {
        const newCoAuthorsMap: Record<string, User[]> = {};
        
        for (const work of worksData.data) {
          if (work.coAuthorUserIds && work.coAuthorUserIds.length > 0) {
            const coAuthors: User[] = [];
            
            for (const userId of work.coAuthorUserIds) {
              try {
                const response = await getUserById(userId);
                if (response.success && response.data) {
                  coAuthors.push(response.data);
                }
              } catch (error) {
                console.error("Lỗi khi lấy thông tin đồng tác giả:", error);
              }
            }
            
            newCoAuthorsMap[work.id] = coAuthors;
          }
        }
        
        setCoAuthorsMap(newCoAuthorsMap);
      };

      fetchCoAuthors();
    }
  }, [worksData]);

  // Cập nhật localWorks khi data thay đổi
  useEffect(() => {
    if (worksData?.data) {
      // Log dữ liệu trước khi cập nhật
      console.log("Cập nhật localWorks với dữ liệu:", worksData.data);
      setLocalWorks(worksData.data);
    }
  }, [worksData]);

  // Kiểm tra xem tác giả có đăng ký không, ưu tiên trạng thái optimistic
  const isAuthorRegistered = useCallback((author: any): boolean => {
    if (!author) return false;
    
    // Kiểm tra optimistic update trước
    if (optimisticUpdates.has(author.id)) {
      return optimisticUpdates.get(author.id) as boolean;
    }
    
    // Nếu không có optimistic update, kiểm tra dữ liệu từ API
    // Log để kiểm tra giá trị authorRegistration
    console.log("Kiểm tra trạng thái đăng ký của author:", author);
    return author.authorRegistration != null;
  }, [optimisticUpdates]);

  // Toast notifications
  useEffect(() => {
    if (worksData && worksData.message) {
      toast.success(worksData.message, { toastId: "fetch-works-success" });
    }
  }, [worksData]);

  useEffect(() => {
    if (worksError) {
      toast.error("Có lỗi đã xảy ra: " + (worksError as Error).message, { toastId: "fetch-works-error" });
    }
  }, [worksError]);

  // Handle delete confirmation dialog
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);

  const handleDeleteClick = async (workId: string) => {
    const work = localWorks.find(w => w.id === workId);
    const author = work?.authors?.find((a: any) => a.userId === user?.id);
    if (!author) {
      toast.error("Không tìm thấy thông tin tác giả");
      return;
    }

    // Cập nhật optimistic ngay lập tức
    setOptimisticUpdates(prev => {
      const newMap = new Map(prev);
      newMap.set(author.id, false);
      return newMap;
    });

    try {
      await registerMutation.mutateAsync({ 
        authorId: author.id, 
        registered: false 
      });
    } catch (error) {
      // Lỗi đã được xử lý trong onError của mutation
    }
  };

  const handleDeleteCancel = () => {
    setDeleteId(null);
    setDeleteDialogOpen(false);
  };

  // Register work mutation với optimistic updates
  const registerMutation = useMutation({
    mutationFn: ({ authorId, registered }: { authorId: string, registered: boolean }) => 
      registerWorkByAuthor(authorId, registered),
    onSuccess: (_, variables) => {
      const { authorId, registered } = variables;
      toast.success(registered ? "Đăng ký công trình thành công!" : "Hủy đăng ký công trình thành công!");

      // Cập nhật UI ngay lập tức
      setLocalWorks(prevWorks => {
        const updatedWorks = prevWorks.map(work => {
          if (work.authors && work.authors.length > 0) {
            const authorIndex = work.authors.findIndex(a => a.id === authorId);
            if (authorIndex >= 0) {
              const updatedAuthors = [...work.authors];
              updatedAuthors[authorIndex] = {
                ...updatedAuthors[authorIndex],
                registered: registered
              };
              return { ...work, authors: updatedAuthors };
            }
          }
          return work;
        });
        
        // Log dữ liệu sau khi cập nhật
        console.log("Dữ liệu sau khi cập nhật:", updatedWorks);
        return updatedWorks;
      });

      // Xóa cache và refetch dữ liệu mới
      queryClient.invalidateQueries({ queryKey: ["works", "registerable-works"] });
      
      // Refetch dữ liệu sau một khoảng thời gian ngắn
      setTimeout(() => {
        refetch();
        // Xóa optimistic update sau khi dữ liệu được refetch thành công
        setOptimisticUpdates(prev => {
          const newMap = new Map(prev);
          newMap.delete(authorId);
          return newMap;
        });
      }, 300);
    },
    onError: (error: Error, variables) => {
      const { authorId } = variables;
      
      // Hủy bỏ optimistic update khi có lỗi
      setOptimisticUpdates(prev => {
        const newMap = new Map(prev);
        newMap.delete(authorId);
        return newMap;
      });
      
      toast.error("Lỗi khi cập nhật trạng thái đăng ký: " + error.message);
      
      // Refetch dữ liệu để đảm bảo UI đồng bộ với server
      refetch();
    },
  });

  const handleRegisterClick = async (work: any) => {
    const author = work.authors?.find((a: any) => a.userId === user?.id);
    if (!author) {
      toast.error("Không tìm thấy thông tin tác giả");
      return;
    }

    // Cập nhật optimistic ngay lập tức
    setOptimisticUpdates(prev => {
      const newMap = new Map(prev);
      newMap.set(author.id, true);
      return newMap;
    });

    try {
      await registerMutation.mutateAsync({ 
        authorId: author.id, 
        registered: true 
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
      width: 150,
    },
    {
      field: "timePublished",
      headerName: "Thời gian xuất bản",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        if (!params.value) return <div>-</div>;
        return <div>{formatMonthYear(params.value)}</div>;
      },
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
      field: "totalAuthors",
      headerName: "Số tác giả",
      type: "number",
      width: 140,
      align: "center",
      headerAlign: "left"
    },
    {
      field: "coAuthors",
      headerName: "Đồng tác giả",
      type: "string",
      width: 140,
      renderCell: (params: any) => {
        const workId = params.row.id;
        const coAuthors = coAuthorsMap[workId] || [];
        
        if (coAuthors.length === 0) return <div>-</div>;
        
        const coAuthorsText = `${coAuthors.length} tác giả`;
        
        return (
          <Tooltip 
            title={
              <div>
                <Typography variant="subtitle2">Danh sách đồng tác giả:</Typography>
                {coAuthors.map((user, index) => (
                  <Typography key={index} variant="body2">
                    • {user.fullName} - {user.userName} - {user.departmentName || "Chưa có phòng ban"}
                  </Typography>
                ))}
              </div>
            }
          >
            <div>{coAuthorsText}</div>
          </Tooltip>
        );
      },
    },
    {
      field: "authorRoleName",
      headerName: "Vai trò tác giả",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.authorRoleName : "-"}</div>;
      },
    },
    {
      field: "position",
      headerName: "Vị trí",
      type: "string",
      width: 80,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.position !== undefined && author?.position !== null ? author.position : "-"}</div>;
      },
    },
    {
      field: "purposeName",
      headerName: "Mục đích quy đổi",
      type: "string",
      width: 180,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.purposeName : "-"}</div>;
      },
    },
    {
      field: "scoreLevel",
      headerName: "Mức điểm",
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
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.authorHour !== undefined && author?.authorHour !== null ? author.authorHour : "-"}</div>;
      },
    },
    {
      field: "proofStatus",
      headerName: "Trạng thái",
      type: "string",
      width: 140,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        const proofStatus = author ? author.proofStatus : undefined;
                
        if (proofStatus === undefined || proofStatus === null) {
          return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>-</div>;
        }
        
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
      },
    },
    {
      field: "actions",
      headerName: "Thao tác",
      width: 250,
      renderCell: (params: any) => {
        const work = params.row;
        const author = work.authors?.find((a: any) => a.userId === user?.id);
        const isRegistered = isAuthorRegistered(author);
        const isProcessing = optimisticUpdates.has(author?.id);

        return (
          <Box>
            <Button
              variant="contained"
              color="primary"
              size="small"
              onClick={() => handleRegisterClick(work)}
              disabled={isRegistered || isProcessing}
              sx={{ 
                marginRight: 1,
                '&.Mui-disabled': {
                  backgroundColor: 'rgba(0, 0, 0, 0.12)',
                  color: 'rgba(0, 0, 0, 0.26)'
                }
              }}
            >
              {isProcessing ? <CircularProgress size={24} /> : "Đăng ký"}
            </Button>
            <Button
              variant="contained"
              color="error"
              size="small"
              onClick={() => {
                setDeleteId(work.id);
                setDeleteDialogOpen(true);
              }}
              disabled={!isRegistered || isProcessing}
              sx={{
                '&.Mui-disabled': {
                  backgroundColor: 'rgba(0, 0, 0, 0.12)',
                  color: 'rgba(0, 0, 0, 0.26)'
                }
              }}
            >
              {isProcessing ? <CircularProgress size={24} /> : "Hủy đăng ký"}
            </Button>
          </Box>
        );
      },
    },
  ];

  if (isLoadingWorks) return <CircularProgress />;
  if (worksError) return <p>Lỗi: {(worksError as Error).message}</p>;

  return (
    <Container maxWidth="xl">
      <Box sx={{ display: "flex", justifyContent: "space-between", mb: 2 }}>
        <Typography variant="h6">Danh sách công trình có thể đăng ký</Typography>
        <Box sx={{ display: 'flex', gap: 2 }}>
        </Box>
      </Box>

      {isLoadingWorks ? (
        <Box sx={{ display: "flex", justifyContent: "center", mt: 4 }}>
          <CircularProgress />
        </Box>
      ) : (
        <GenericTable columns={columns} data={localWorks} />
      )}

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận hủy đăng ký</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn hủy đăng ký công trình này không?</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteCancel} color="inherit">
            Hủy
          </Button>
          <Button
            onClick={() => {
              if (deleteId) {
                handleDeleteClick(deleteId);
                handleDeleteCancel();
              }
            }}
            color="error"
            variant="contained"
            disabled={registerMutation.isPending}
          >
            {registerMutation.isPending ? <CircularProgress size={24} /> : "Hủy đăng ký"}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
} 