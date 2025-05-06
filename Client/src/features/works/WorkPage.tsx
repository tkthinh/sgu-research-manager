import AddIcon from "@mui/icons-material/Add";
import {
  Box,
  Button,
  CircularProgress,
  Container,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Typography,
} from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import WorkUpdateDialog from "../../app/shared/components/dialogs/WorkUpdateDialog";
import WorksCollapsibleTable from "../../app/shared/components/tables/WorksCollapsibleTable";
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { useSystemStatus } from "../../hooks/useSystemStatus";
import { useWorkFormData } from "../../hooks/useWorkData";
import { useWorkDialogs } from "../../hooks/useWorkDialogs";
import { getCurrentAcademicYear } from "../../lib/api/academicYearApi";
import { getUserById } from "../../lib/api/usersApi";
import { deleteWork, getWorksWithFilter } from "../../lib/api/worksApi";
import { User } from "../../lib/types/models/User";

export default function WorksPage() {
  const queryClient = useQueryClient();
  const [_, setCoAuthorsMap] = useState<Record<string, User[]>>({});
  const { user } = useAuth();
  const { isSystemOpen, canEditWork, canDeleteWork } = useSystemStatus();

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
    refetch,
  } = useQuery({
    queryKey: ["works", "my-works"],
    queryFn: async () => {
      const filter = {
        academicYearId: currentAcademicYear?.data?.id,
        isCurrentUser: true,
      };
      return getWorksWithFilter(filter);
    },
    enabled: !!currentAcademicYear?.data?.id,
    staleTime: 0,
  });

  // Sử dụng hook để lấy dữ liệu form
  const formData = useWorkFormData();

  // Sử dụng hook để quản lý các dialog và logic cập nhật công trình
  const {
    selectedWork,
    openUpdateDialog,
    activeTab,
    createWorkMutation,
    updateWorkMutation,
    handleOpenUpdateDialog,
    handleCloseUpdateDialog,
    handleUpdateSubmit,
    setActiveTab,
  } = useWorkDialogs({
    userId: user?.id ?? "",
    worksData,
    refetchWorks: refetch,
    isAuthorPage: true,
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

  // Toast notifications
  useEffect(() => {
    if (worksData && worksData.message) {
      toast.success(worksData.message, { toastId: "fetch-works-success" });
    }
  }, [worksData]);

  useEffect(() => {
    if (worksError) {
      toast.error("Có lỗi đã xảy ra: " + (worksError as Error).message, {
        toastId: "fetch-works-error",
      });
    }
  }, [worksError]);

  // Handle delete confirmation dialog
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);

  const handleDeleteClick = (id: string) => {
    setDeleteId(id);
    setDeleteDialogOpen(true);
  };

  const handleDeleteCancel = () => {
    setDeleteId(null);
    setDeleteDialogOpen(false);
  };

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: (id: string) => deleteWork(id),
    onSuccess: () => {
      toast.success("Xóa công trình thành công!");
      queryClient.invalidateQueries({ queryKey: ["works", "my-works"] });
      setTimeout(() => {
        refetch();
      }, 100);
      setDeleteDialogOpen(false);
    },
    onError: (error: Error) => {
      toast.error("Lỗi khi xóa công trình: " + error.message);
    },
  });

  const handleDeleteConfirm = async () => {
    if (deleteId) {
      try {
        await deleteMutation.mutateAsync(deleteId);
      } catch (error) {
        // Lỗi đã được xử lý trong onError của mutation
      }
    }
  };

  // const columns: GridColDef[] = [
  //   {
  //     field: "stt",
  //     headerName: "STT",
  //     width: 70,
  //     renderCell: (params) => {
  //       const rowIds = params.api.getAllRowIds();
  //       const index = rowIds.indexOf(params.id);
  //       return <div>{index + 1}</div>;
  //     },
  //   },
  //   {
  //     field: "title",
  //     headerName: "Tên công trình",
  //     type: "string",
  //     width: 250,
  //   },
  //   {
  //     field: "timePublished",
  //     headerName: "Thời gian xuất bản",
  //     type: "string",
  //     width: 150,
  //     renderCell: (params: any) => {
  //       if (!params.value) return <div>-</div>;
  //       return <div>{formatMonthYear(params.value)}</div>;
  //     },
  //   },
  //   {
  //     field: "workTypeName",
  //     headerName: "Loại công trình",
  //     type: "string",
  //     width: 170,
  //   },
  //   {
  //     field: "workLevelName",
  //     headerName: "Cấp công trình",
  //     type: "string",
  //     width: 150,
  //   },
  //   {
  //     field: "details",
  //     headerName: "Thông tin chi tiết",
  //     type: "string",
  //     width: 300,
  //     renderCell: (params: any) => {
  //       if (!params.row.details) return <div>-</div>;

  //       const details = params.row.details;
  //       const detailsText = Object.entries(details)
  //         .map(([key, value]) => `${key}: ${String(value)}`)
  //         .join('\n');

  //       return (
  //         <Tooltip
  //           title={
  //             <div style={{ whiteSpace: 'pre-line', fontSize: '0.8rem' }}>
  //               {detailsText}
  //             </div>
  //           }
  //           arrow
  //         >
  //           <div style={{
  //             overflow: 'hidden',
  //             textOverflow: 'ellipsis',
  //             whiteSpace: 'nowrap',
  //             width: '100%'
  //           }}>
  //             {Object.entries(details).map(([key, value], index) => (
  //               <span key={index}>
  //                 {index > 0 && '; '}
  //                 <b>{key}</b>: {String(value)}
  //               </span>
  //             ))}
  //           </div>
  //         </Tooltip>
  //       );
  //     },
  //   },
  //   {
  //     field: "totalAuthors",
  //     headerName: "Số tác giả",
  //     type: "number",
  //     width: 140,
  //     align: "center",
  //     headerAlign: "left"
  //   },
  //   {
  //     field: "totalMainAuthors",
  //     headerName: "Số tác giả chính",
  //     type: "number",
  //     width: 140,
  //     align: "center",
  //     headerAlign: "left"
  //   },
  //   {
  //     field: "coAuthors",
  //     headerName: "Đồng tác giả",
  //     type: "string",
  //     width: 140,
  //     renderCell: (params: any) => {
  //       const workId = params.row.id;
  //       const coAuthors = coAuthorsMap[workId] || [];

  //       if (coAuthors.length === 0) return <div>-</div>;

  //       const coAuthorsText = `${coAuthors.length} tác giả`;

  //       return (
  //         <Tooltip
  //           title={
  //             <div>
  //               <Typography variant="subtitle2">Danh sách đồng tác giả:</Typography>
  //               {coAuthors.map((user, index) => (
  //                 <Typography key={index} variant="body2">
  //                   • {user.fullName} - {user.userName} - {user.departmentName || "Chưa có phòng ban"}
  //                 </Typography>
  //               ))}
  //             </div>
  //           }
  //         >
  //           <div>{coAuthorsText}</div>
  //         </Tooltip>
  //       );
  //     },
  //   },
  //   {
  //     field: "authorRoleName",
  //     headerName: "Vai trò tác giả",
  //     type: "string",
  //     width: 150,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       return <div>{currentAuthor ? currentAuthor.authorRoleName : "-"}</div>;
  //     },
  //   },
  //   {
  //     field: "position",
  //     headerName: "Vị trí",
  //     type: "string",
  //     width: 80,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       return <div>{currentAuthor?.position !== undefined && currentAuthor?.position !== null ? currentAuthor.position : "-"}</div>;
  //     },
  //   },
  //   {
  //     field: "purposeName",
  //     headerName: "Mục đích quy đổi",
  //     type: "string",
  //     width: 180,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       return <div>{currentAuthor ? currentAuthor.purposeName : "-"}</div>;
  //     },
  //   },
  //   {
  //     field: "fieldName",
  //     headerName: "Ngành tính điểm",
  //     type: "string",
  //     width: 150,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       return <div>{currentAuthor ? currentAuthor.fieldName : "-"}</div>;
  //     },
  //   },
  //   {
  //     field: "scImagoFieldName",
  //     headerName: "Ngành SCImago",
  //     type: "string",
  //     width: 180,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       return <div>{currentAuthor ? currentAuthor.scImagoFieldName : "-"}</div>;
  //     },
  //   },
  //   {
  //     field: "scoreLevel",
  //     headerName: "Mức điểm",
  //     type: "string",
  //     width: 150,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       if (!currentAuthor || currentAuthor.scoreLevel === undefined || currentAuthor.scoreLevel === null) {
  //         return <div>-</div>;
  //       }
  //       return <div>{getScoreLevelText(currentAuthor.scoreLevel)}</div>;
  //     },
  //   },
  //   {
  //     field: "workHour",
  //     headerName: "Giờ công trình",
  //     type: "string",
  //     width: 120,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       return <div>{currentAuthor?.workHour !== undefined && currentAuthor?.workHour !== null ? currentAuthor.workHour : "-"}</div>;
  //     },
  //   },
  //   {
  //     field: "authorHour",
  //     headerName: "Giờ tác giả",
  //     type: "string",
  //     width: 120,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       return <div>{currentAuthor?.authorHour !== undefined && currentAuthor?.authorHour !== null ? currentAuthor.authorHour : "-"}</div>;
  //     },
  //   },
  //   {
  //     field: "note",
  //     headerName: "Ghi chú",
  //     type: "string",
  //     width: 150,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       const noteText = currentAuthor?.note || "";

  //       if (!noteText) {
  //         return <div>-</div>;
  //       }

  //       return (
  //         <Tooltip
  //           title={
  //             <div style={{ whiteSpace: 'pre-line', fontSize: '0.8rem' }}>
  //               {noteText}
  //             </div>
  //           }
  //           arrow
  //         >
  //           <div style={{
  //             overflow: 'hidden',
  //             textOverflow: 'ellipsis',
  //             whiteSpace: 'nowrap',
  //             width: '100%'
  //           }}>
  //             {noteText}
  //           </div>
  //         </Tooltip>
  //       );
  //     },
  //   },
  //   {
  //     field: "proofStatus",
  //     headerName: "Trạng thái",
  //     type: "string",
  //     width: 140,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       const proofStatus = currentAuthor?.proofStatus;

  //       if (proofStatus === undefined || proofStatus === null) {
  //         return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>-</div>;
  //       }

  //       if (proofStatus === ProofStatus.HopLe) {
  //         return (
  //           <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
  //             <CheckCircleIcon color="success" />
  //             Hợp lệ
  //           </div>
  //         );
  //       } else if (proofStatus === ProofStatus.KhongHopLe) {
  //         return (
  //           <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
  //             <CancelIcon color="error" />
  //             Không hợp lệ
  //           </div>
  //         );
  //       } else if (proofStatus === ProofStatus.ChuaXuLy) {
  //         return (
  //           <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
  //             <HistoryIcon color="action" />
  //             Chưa xử lý
  //           </div>
  //         );
  //       } else {
  //         return <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>-</div>;
  //       }
  //     },
  //   },
  //   {
  //     field: "actions",
  //     headerName: "Thao tác",
  //     width: 200,
  //     renderCell: (params: any) => {
  //       const work = params.row;
  //       const currentAuthor = work.authors?.find(author => author.userId === user?.id);
  //       const proofStatus = currentAuthor?.proofStatus;
  //       const isLocked = work.isLocked;

  //       // Kiểm tra xem có tác giả khác đã được chấm hợp lệ không
  //       const hasOtherValidAuthors = work.authors?.some(
  //         author => author.userId !== user?.id && author.proofStatus === 0
  //       );

  //       // Nếu công trình đã bị khóa và tác giả hiện tại đã được chấm hợp lệ
  //       if (isLocked && proofStatus === 0) {
  //         return (
  //           <Box>
  //             <Button
  //               variant="contained"
  //               color="primary"
  //               size="small"
  //               onClick={() => handleOpenUpdateDialog(work)}
  //               disabled={true}
  //               sx={{ mr: 1 }}
  //             >
  //               Sửa
  //             </Button>
  //             <Button
  //               variant="contained"
  //               color="error"
  //               size="small"
  //               onClick={() => handleDeleteClick(work.id)}
  //               disabled={true}
  //             >
  //               Xóa
  //             </Button>
  //           </Box>
  //         );
  //       }

  //       // Nếu công trình đã bị khóa và tác giả hiện tại chưa được chấm hoặc không hợp lệ
  //       if (isLocked) {
  //         return (
  //           <Box>
  //             <Button
  //               variant="contained"
  //               color="primary"
  //               size="small"
  //               onClick={() => handleOpenUpdateDialog(work)}
  //               disabled={false}
  //               sx={{ mr: 1 }}
  //             >
  //               Sửa
  //             </Button>
  //             <Button
  //               variant="contained"
  //               color="error"
  //               size="small"
  //               onClick={() => handleDeleteClick(work.id)}
  //               disabled={!canDeleteWork(proofStatus, isLocked, hasOtherValidAuthors)}
  //             >
  //               Xóa
  //             </Button>
  //           </Box>
  //         );
  //       }

  //       // Nếu công trình chưa bị khóa
  //       const canEdit = canEditWork(proofStatus, isLocked);
  //       return (
  //         <Box>
  //           <Button
  //             variant="contained"
  //             color="primary"
  //             size="small"
  //             onClick={() => handleOpenUpdateDialog(work)}
  //             disabled={!canEdit}
  //             sx={{ mr: 1 }}
  //           >
  //             Sửa
  //           </Button>
  //           <Button
  //             variant="contained"
  //             color="error"
  //             size="small"
  //             onClick={() => handleDeleteClick(work.id)}
  //             disabled={!canDeleteWork(proofStatus, isLocked, hasOtherValidAuthors)}
  //           >
  //             Xóa
  //           </Button>
  //         </Box>
  //       );
  //     },
  //   },
  // ];

  if (isLoadingWorks) return <CircularProgress />;
  if (worksError) return <p>Lỗi: {(worksError as Error).message}</p>;

  return (
    <Container maxWidth="xl">
      <Box sx={{ display: "flex", justifyContent: "space-between", mb: 2 }}>
        <Typography variant="h6">Danh sách công trình</Typography>
        <Box sx={{ display: "flex", gap: 2 }}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => handleOpenUpdateDialog(undefined)}
            startIcon={<AddIcon />}
            disabled={!isSystemOpen}
          >
            Thêm công trình
          </Button>
        </Box>
      </Box>

      {isLoadingWorks ? (
        <CircularProgress />
      ) : (
        <WorksCollapsibleTable
          works={worksData.data || []}
          userId={user?.id ?? ""}
          onEdit={handleOpenUpdateDialog}
          onDelete={handleDeleteClick}
          canEditWork={canEditWork}
          canDeleteWork={canDeleteWork}
        />
      )}

      {/* Sử dụng component dialog tái sử dụng */}
      <WorkUpdateDialog
        open={openUpdateDialog}
        onClose={handleCloseUpdateDialog}
        selectedWork={selectedWork}
        activeTab={activeTab}
        setActiveTab={setActiveTab}
        onSubmit={handleUpdateSubmit}
        isPending={createWorkMutation.isPending || updateWorkMutation.isPending}
        workTypes={formData.workTypes}
        workLevels={formData.workLevels}
        authorRoles={formData.authorRoles}
        purposes={formData.purposes}
        scimagoFields={formData.scimagoFields}
        fields={formData.fields}
      />

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Xác nhận xóa</DialogTitle>
        <DialogContent>
          <Typography>
            Bạn có chắc chắn muốn xóa công trình này không?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteCancel} color="inherit">
            Hủy
          </Button>
          <Button
            onClick={handleDeleteConfirm}
            color="error"
            variant="contained"
            disabled={deleteMutation.isPending}
          >
            {deleteMutation.isPending ? <CircularProgress size={24} /> : "Xóa"}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}
