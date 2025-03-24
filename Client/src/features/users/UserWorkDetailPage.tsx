import {
  Box,
  Button,
  CircularProgress,
  Container,
  Paper,
  Tooltip,
  Typography,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useQuery } from "@tanstack/react-query";
import { format } from "date-fns";
import { vi } from "date-fns/locale";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import GenericTable from "../../app/shared/components/tables/DataTable";
import { getUserById } from "../../lib/api/usersApi";
import { getWorksByUserId } from "../../lib/api/worksApi";
import { ProofStatus } from "../../lib/types/enums/ProofStatus";
import { ScoreLevel } from "../../lib/types/enums/ScoreLevel";
import { User } from "../../lib/types/models/User";
import EditIcon from "@mui/icons-material/Edit";
import AssignmentTurnedInIcon from "@mui/icons-material/AssignmentTurnedIn";
import NoteIcon from "@mui/icons-material/Note";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import CancelIcon from "@mui/icons-material/Cancel";
import HistoryIcon from "@mui/icons-material/History";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { useWorkFormData } from "../../hooks/useWorkData";
import { useWorkDialogs } from "../../hooks/useWorkDialogs";
import WorkStatusDialog from "../../app/shared/components/dialogs/WorkStatusDialog";
import WorkNoteDialog from "../../app/shared/components/dialogs/WorkNoteDialog";
import WorkUpdateDialog from "../../app/shared/components/dialogs/WorkUpdateDialog";
import { getScoreLevelText } from '../../lib/utils/scoreLevelUtils';

export default function UserWorkDetailPage() {
  const { userId } = useParams<{ userId: string }>();
  const navigate = useNavigate();
  const [coAuthorsMap, setCoAuthorsMap] = useState<Record<string, User[]>>({});

  // Sử dụng hook để lấy dữ liệu form
  const formData = useWorkFormData();

  // Fetch user information
  const {
    data: userData,
    isLoading: isLoadingUser,
    error: userError,
  } = useQuery({
    queryKey: ["users", userId],
    queryFn: () => getUserById(userId || ""),
    enabled: !!userId,
  });

  // Fetch user's works
  const {
    data: worksData,
    isLoading: isLoadingWorks,
    error: worksError,
    refetch: refetchWorks,
  } = useQuery({
    queryKey: ["works", "user", userId],
    queryFn: () => getWorksByUserId(userId || ""),
    enabled: !!userId,
  });

  // Lấy thông tin đồng tác giả khi có dữ liệu công trình
  useEffect(() => {
    if (worksData?.data && worksData.data.length > 0) {
      const fetchCoAuthors = async () => {
        const newCoAuthorsMap: Record<string, User[]> = {};
        
        for (const work of worksData.data) {
          if (work.coAuthorUserIds && work.coAuthorUserIds.length > 0) {
            const coAuthors: User[] = [];
            
            // Lấy thông tin từng đồng tác giả
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

  // Sử dụng hook để quản lý các dialog và logic cập nhật công trình
  const {
    selectedWork,
    openUpdateDialog, 
    openStatusDialog,
    openNoteDialog,
    newStatus,
    newNote,
    activeTab,
    updateWorkMutation,
    createWorkMutation,
    handleOpenUpdateDialog,
    handleCloseUpdateDialog,
    handleOpenStatusDialog,
    handleCloseStatusDialog,
    handleOpenNoteDialog,
    handleCloseNoteDialog,
    handleStatusChange,
    handleNoteChange,
    handleStatusSubmit,
    handleNoteSubmit,
    handleUpdateSubmit,
    setActiveTab,
  } = useWorkDialogs({
    userId,
    worksData,
    refetchWorks,
    isAuthorPage: false
  });

  const handleGoBack = () => {
    navigate(-1);
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
      field: "timePublished",
      headerName: "Thời gian xuất bản",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        if (!params.value) return <div>-</div>;
        try {
          const formattedDate = format(new Date(params.value), "dd/MM/yyyy", { locale: vi });
          return <div>{formattedDate}</div>;
        } catch (error) {
          return <div>{params.value}</div>;
        }
      },
    },
    {
      field: "workTypeName",
      headerName: "Loại công trình",
      type: "string",
      width: 150,
    },
    {
      field: "workLevelName",
      headerName: "Cấp công trình",
      type: "string",
      width: 150,
    },
    {
      field: "details",
      headerName: "Thông tin chi tiết",
      type: "string",
      width: 300,
      renderCell: (params: any) => {
        if (!params.row.details) return <div>-</div>;
        
        const details = params.row.details;
        const detailsText = Object.entries(details)
          .map(([key, value]) => `${key}: ${String(value)}`)
          .join('\n');
        
        return (
          <Tooltip 
            title={
              <div style={{ whiteSpace: 'pre-line', fontSize: '0.8rem' }}>
                {detailsText}
              </div>
            } 
            arrow
          >
            <div style={{ 
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              whiteSpace: 'nowrap',
              width: '100%'
            }}>
              {Object.entries(details).map(([key, value], index) => (
                <span key={index}>
                  {index > 0 && '; '}
                  <b>{key}</b>: {String(value)}
                </span>
              ))}
            </div>
          </Tooltip>
        );
      },
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
      field: "totalMainAuthors",
      headerName: "Số tác giả chính",
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
      field: "fieldName",
      headerName: "Ngành tính điểm",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.fieldName : "-"}</div>;
      },
    },
    {
      field: "scImagoFieldName",
      headerName: "Ngành SCImago",
      type: "string",
      width: 180,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author ? author.scImagoFieldName : "-"}</div>;
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
          return <div>-</div>;
        }
        return <div>{getScoreLevelText(author.scoreLevel)}</div>;
      },
    },
    {
      field: "workHour",
      headerName: "Giờ công trình",
      type: "string",
      width: 120,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        return <div>{author?.workHour !== undefined && author?.workHour !== null ? author.workHour : "-"}</div>;
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
      field: "note",
      headerName: "Ghi chú",
      type: "string",
      width: 150,
      renderCell: (params: any) => {
        const author = params.row.authors && params.row.authors[0];
        const noteText = author?.note || "";
    
        // Nếu không có ghi chú, hiển thị "-"
        if (!noteText) {
          return <div>-</div>;
        }
    
        return (
          <Tooltip 
            title={
              <div style={{ whiteSpace: 'pre-line', fontSize: '0.8rem' }}>
                {noteText}
              </div>
            } 
            arrow
          >
            <div style={{ 
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              whiteSpace: 'nowrap',
              width: '100%'
            }}>
              {noteText}
            </div>
          </Tooltip>
        );
      },
    },
    {
      field: "proofStatus",
      headerName: "Trạng thái",
      type: "string",
      width: 140,
      renderCell: (params: any) => {
        // Lấy proofStatus từ author đầu tiên
        const author = params.row.authors && params.row.authors[0];
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
      },
    },
    {
      field: "actions",
      headerName: "Thao tác",
      width: 350,
      renderCell: (params: any) => (
        <Box display="flex" gap={1} alignItems="center" height="100%">
          <Button
            variant="contained"
            color="primary"
            size="small"
            onClick={() => handleOpenUpdateDialog(params.row)}
            startIcon={<EditIcon />}
          >
            Cập nhật
          </Button>
          <Button
            variant="contained"
            color="info"
            size="small"
            onClick={() => handleOpenStatusDialog(params.row)}
            startIcon={<AssignmentTurnedInIcon />}
          >
            Trạng thái
          </Button>
          <Button
            variant="contained"
            color="secondary"
            size="small"
            onClick={() => handleOpenNoteDialog(params.row)}
            startIcon={<NoteIcon />}
          >
            Ghi chú
          </Button>
        </Box>
      ),
    },
  ];

  if (isLoadingUser || isLoadingWorks) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" height="50vh">
        <CircularProgress />
      </Box>
    );
  }

  if (userError || worksError) {
    return (
      <Box p={3}>
        <Typography color="error">
          {userError ? "Lỗi khi tải thông tin người dùng: " + (userError as Error).message : ""}
          {worksError ? "Lỗi khi tải danh sách công trình: " + (worksError as Error).message : ""}
        </Typography>
      </Box>
    );
  }

  const user = userData?.data;
  const works = worksData?.data || [];

  return (
    <Container maxWidth="xl">
      <Box sx={{ display: "flex", alignItems: "center", mb: 2 }}>
        <Button 
          variant="outlined" 
          startIcon={<ArrowBackIcon />} 
          onClick={handleGoBack}
          sx={{ mr: 2 }}
        >
          Quay lại
        </Button>
        <Typography variant="h4">Chấm điểm công trình - {user?.fullName}</Typography>
      </Box>

      <Box sx={{ mb: 3 }}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6" gutterBottom>
            Thông tin người dùng
          </Typography>
          <Box sx={{ display: "flex", flexWrap: "wrap", gap: 2 }}>
            <Typography variant="body1">
              <strong>Mã số viên chức:</strong> {user?.userName}
            </Typography>
            <Typography variant="body1">
              <strong>Họ và tên:</strong> {user?.fullName}
            </Typography>
            <Typography variant="body1">
              <strong>Đơn vị công tác:</strong> {user?.departmentName || "Chưa có phòng ban"}
            </Typography>
            <Typography variant="body1">
              <strong>Ngành:</strong> {user?.fieldName || "Chưa phân ngành"}
            </Typography>
          </Box>
        </Paper>
      </Box>

      <Box sx={{ mb: 2 }}>
        <Typography variant="h6" gutterBottom>
          Danh sách công trình
        </Typography>
        <Paper sx={{ width: "100%", overflow: "hidden" }}>
          <Box sx={{ height: 600 }}>
            <GenericTable columns={columns} data={works} />
          </Box>
        </Paper>
      </Box>

      {/* Sử dụng component dialog tái sử dụng */}
      <WorkStatusDialog 
        open={openStatusDialog}
        onClose={handleCloseStatusDialog}
        status={newStatus}
        onStatusChange={(event) => handleStatusChange(Number(event.target.value))}
        onSubmit={handleStatusSubmit}
        isPending={updateWorkMutation.isPending}
      />

      <WorkNoteDialog
        open={openNoteDialog}
        onClose={handleCloseNoteDialog}
        note={newNote}
        onNoteChange={(event) => handleNoteChange(event.target.value)}
        onSubmit={handleNoteSubmit}
        isPending={updateWorkMutation.isPending}
      />

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
    </Container>
  );
} 