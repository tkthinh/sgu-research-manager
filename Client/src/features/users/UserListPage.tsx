import React, { useEffect, useState } from 'react';
import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  IconButton,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  SelectChangeEvent,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Tooltip,
  Typography,
} from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { toast } from 'react-toastify';
import { getUserById } from '../../lib/api/usersApi';
import { 
  getWorksByUserId, 
  updateWorkByAdmin, 
  updateWorkStatus, 
  updateWorkNote,
  deleteWork
} from '../../lib/api/worksApi';
import { User } from '../../lib/types/models/User';
import { Work } from '../../lib/types/models/Work';
import { format } from 'date-fns';
import { vi } from 'date-fns/locale';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import InfoIcon from '@mui/icons-material/Info';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import CancelIcon from '@mui/icons-material/Cancel';
import HistoryIcon from '@mui/icons-material/History';
import { ScoreLevel } from '../../lib/types/enums/ScoreLevel';
import { ProofStatus } from '../../lib/types/enums/ProofStatus';

// Hàm chuyển đổi ScoreLevel thành chuỗi hiển thị
const getScoreLevelText = (scoreLevel: number): string => {
  switch (scoreLevel) {
    case ScoreLevel.One:
      return "1 điểm";
    case ScoreLevel.ZeroPointSevenFive:
      return "0.75 điểm";
    case ScoreLevel.ZeroPointFive:
      return "0.5 điểm";
    case ScoreLevel.TenPercent:
      return "Top 10%";
    case ScoreLevel.ThirtyPercent:
      return "Top 30%";
    case ScoreLevel.FiftyPercent:
      return "Top 50%";
    case ScoreLevel.HundredPercent:
      return "Top 100%";
    default:
      return "-";
  }
};

const UserWorkDetailPage: React.FC = () => {
  const { userId } = useParams<{ userId: string }>();
  const [user, setUser] = useState<User | null>(null);
  const [works, setWorks] = useState<Work[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  
  // Dialog states
  const [openStatusDialog, setOpenStatusDialog] = useState(false);
  const [openNoteDialog, setOpenNoteDialog] = useState(false);
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
  const [openEditDialog, setOpenEditDialog] = useState(false);
  const [selectedWork, setSelectedWork] = useState<Work | null>(null);
  const [newStatus, setNewStatus] = useState<number>(ProofStatus.ChuaXuLy);
  const [newNote, setNewNote] = useState<string>('');
  const [formData, setFormData] = useState({
    workHour: 0,
    authorHour: 0
  });

  useEffect(() => {
    const fetchData = async () => {
      if (!userId) return;
      
      try {
        setLoading(true);
        const [userResponse, worksResponse] = await Promise.all([
          getUserById(userId),
          getWorksByUserId(userId)
        ]);

        if (userResponse.success && worksResponse.success) {
          setUser(userResponse.data);
          setWorks(worksResponse.data || []);
        } else {
          toast.error("Lỗi khi tải dữ liệu");
        }
      } catch (error) {
        console.error("Lỗi khi tải dữ liệu:", error);
        toast.error("Lỗi khi tải dữ liệu");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [userId]);

  const refreshData = async () => {
    if (!userId) return;
    
    try {
      setLoading(true);
      const worksResponse = await getWorksByUserId(userId);
      if (worksResponse.success) {
        setWorks(worksResponse.data || []);
      } else {
        toast.error("Lỗi khi tải lại dữ liệu");
      }
    } catch (error) {
      console.error("Lỗi khi tải lại dữ liệu:", error);
      toast.error("Lỗi khi tải lại dữ liệu");
    } finally {
      setLoading(false);
    }
  };

  const handleBack = () => {
    navigate('/danh-sach-nguoi-dung');
  };

  const formatDate = (dateString?: string) => {
    if (!dateString) return 'N/A';
    try {
      return format(new Date(dateString), 'dd/MM/yyyy', { locale: vi });
    } catch (error) {
      return dateString;
    }
  };

  const showWorkDetails = (work: Work) => {
    if (!work.details) {
      toast.info("Không có thông tin chi tiết cho công trình này");
      return;
    }
    
    const details = work.details || {};
    const detailsArray = Object.entries(details).map(([key, value]) => `${key}: ${value}`);
    toast.info(
      <div>
        <Typography variant="subtitle1">Chi tiết công trình</Typography>
        <ul style={{ marginTop: 8, paddingLeft: 16 }}>
          {detailsArray.map((detail, index) => (
            <li key={index}>{detail}</li>
          ))}
          {detailsArray.length === 0 && <li>Không có thông tin chi tiết</li>}
        </ul>
      </div>,
      { autoClose: false }
    );
  };

  // Status Dialog handlers
  const handleOpenStatusDialog = (work: Work) => {
    setSelectedWork(work);
    const author = work.authors && work.authors.length > 0 ? work.authors[0] : null;
    setNewStatus(author?.proofStatus !== undefined ? author.proofStatus : ProofStatus.ChuaXuLy);
    setOpenStatusDialog(true);
  };

  const handleCloseStatusDialog = () => {
    setOpenStatusDialog(false);
    setSelectedWork(null);
  };

  const handleStatusChange = (event: SelectChangeEvent<number>) => {
    setNewStatus(Number(event.target.value));
  };

  const handleStatusSubmit = async () => {
    if (!selectedWork || !userId) return;
    
    try {
      const author = selectedWork.authors?.find(a => a.userId === userId);
      if (!author) {
        toast.error("Không tìm thấy thông tin tác giả");
        return;
      }
      
      const response = await updateWorkStatus(selectedWork.id, userId, newStatus);
      if (response.success) {
        toast.success("Cập nhật trạng thái thành công");
        await refreshData();
      } else {
        toast.error("Lỗi khi cập nhật trạng thái");
      }
    } catch (error) {
      console.error("Lỗi khi cập nhật trạng thái:", error);
      toast.error("Lỗi khi cập nhật trạng thái");
    } finally {
      handleCloseStatusDialog();
    }
  };

  // Note Dialog handlers
  const handleOpenNoteDialog = (work: Work) => {
    setSelectedWork(work);
    const author = work.authors && work.authors.length > 0 ? work.authors[0] : null;
    setNewNote(author?.note || '');
    setOpenNoteDialog(true);
  };

  const handleCloseNoteDialog = () => {
    setOpenNoteDialog(false);
    setSelectedWork(null);
  };

  const handleNoteChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setNewNote(event.target.value);
  };

  const handleNoteSubmit = async () => {
    if (!selectedWork || !userId) return;
    
    try {
      const author = selectedWork.authors?.find(a => a.userId === userId);
      if (!author) {
        toast.error("Không tìm thấy thông tin tác giả");
        return;
      }
      
      const response = await updateWorkNote(selectedWork.id, userId, newNote);
      if (response.success) {
        toast.success("Cập nhật ghi chú thành công");
        await refreshData();
      } else {
        toast.error("Lỗi khi cập nhật ghi chú");
      }
    } catch (error) {
      console.error("Lỗi khi cập nhật ghi chú:", error);
      toast.error("Lỗi khi cập nhật ghi chú");
    } finally {
      handleCloseNoteDialog();
    }
  };

  // Edit Dialog handlers
  const handleOpenEditDialog = (work: Work) => {
    setSelectedWork(work);
    const author = work.authors?.find(a => a.userId === userId);
    if (author) {
      setFormData({
        workHour: author.workHour || 0,
        authorHour: author.authorHour || 0
      });
    }
    setOpenEditDialog(true);
  };

  const handleCloseEditDialog = () => {
    setOpenEditDialog(false);
    setSelectedWork(null);
  };

  const handleFormChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]: parseFloat(value)
    });
  };

  const handleEditSubmit = async () => {
    if (!selectedWork || !userId) return;
    
    try {
      const response = await updateWorkByAdmin(selectedWork.id, userId, {
        authorRequest: {
          workHour: formData.workHour,
          authorHour: formData.authorHour
        }
      });
      
      if (response.success) {
        toast.success("Cập nhật thông tin công trình thành công");
        await refreshData();
      } else {
        toast.error("Lỗi khi cập nhật thông tin");
      }
    } catch (error) {
      console.error("Lỗi khi cập nhật thông tin:", error);
      toast.error("Lỗi khi cập nhật thông tin");
    } finally {
      handleCloseEditDialog();
    }
  };

  // Delete Dialog handlers
  const handleOpenDeleteDialog = (work: Work) => {
    setSelectedWork(work);
    setOpenDeleteDialog(true);
  };

  const handleCloseDeleteDialog = () => {
    setOpenDeleteDialog(false);
    setSelectedWork(null);
  };

  const handleDeleteWork = async () => {
    if (!selectedWork) return;
    
    try {
      const response = await deleteWork(selectedWork.id);
      if (response.success) {
        toast.success("Xóa công trình thành công");
        await refreshData();
      } else {
        toast.error("Lỗi khi xóa công trình");
      }
    } catch (error) {
      console.error("Lỗi khi xóa công trình:", error);
      toast.error("Lỗi khi xóa công trình");
    } finally {
      handleCloseDeleteDialog();
    }
  };

  // Render status with icon
  const renderProofStatus = (proofStatus?: number) => {
    if (proofStatus === undefined || proofStatus === null) {
      return <div>-</div>;
    }
    
    switch (proofStatus) {
      case ProofStatus.HopLe:
        return (
          <Box sx={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
            <CheckCircleIcon color="success" />
            <span>Hợp lệ</span>
          </Box>
        );
      case ProofStatus.KhongHopLe:
        return (
          <Box sx={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
            <CancelIcon color="error" />
            <span>Không hợp lệ</span>
          </Box>
        );
      case ProofStatus.ChuaXuLy:
        return (
          <Box sx={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
            <HistoryIcon color="action" />
            <span>Chưa xử lý</span>
          </Box>
        );
      default:
        return <div>-</div>;
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
        <IconButton onClick={handleBack} sx={{ mr: 2 }}>
          <ArrowBackIcon />
        </IconButton>
        <Typography variant="h4">
          Danh sách công trình của {user?.fullName || ''}
        </Typography>
      </Box>

      {user && (
        <Box sx={{ mb: 3 }}>
          <Typography variant="subtitle1">
            Mã số viên chức: {user.userName}
          </Typography>
          <Typography variant="subtitle1">
            Đơn vị: {user.departmentName}
          </Typography>
          <Typography variant="subtitle1">
            Ngành: {user.fieldName}
          </Typography>
        </Box>
      )}

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
          <CircularProgress />
        </Box>
      ) : (
        <TableContainer component={Paper}>
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell>STT</TableCell>
                <TableCell>Tên công trình</TableCell>
                <TableCell>Thời gian xuất bản</TableCell>
                <TableCell>Loại công trình</TableCell>
                <TableCell>Cấp công trình</TableCell>
                <TableCell>Thông tin chi tiết</TableCell>
                <TableCell>Số tác giả</TableCell>
                <TableCell>Vai trò tác giả</TableCell>
                <TableCell>Vị trí</TableCell>
                <TableCell>Mục đích quy đổi</TableCell>
                <TableCell>Ngành tính điểm</TableCell>
                <TableCell>Ngành SCImago</TableCell>
                <TableCell>Mức điểm</TableCell>
                <TableCell>Giờ công trình</TableCell>
                <TableCell>Giờ tác giả</TableCell>
                <TableCell>Ghi chú</TableCell>
                <TableCell>Trạng thái</TableCell>
                <TableCell align="center">Thao tác</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {works.map((work, index) => {
                const author = work.authors?.find(a => a.userId === userId);
                return (
                  <TableRow key={work.id} hover>
                    <TableCell>{index + 1}</TableCell>
                    <TableCell>
                      <Tooltip title={work.title}>
                        <span>{work.title.length > 30 ? `${work.title.substring(0, 30)}...` : work.title}</span>
                      </Tooltip>
                    </TableCell>
                    <TableCell>{formatDate(work.timePublished)}</TableCell>
                    <TableCell>{work.workTypeName || '-'}</TableCell>
                    <TableCell>{work.workLevelName || '-'}</TableCell>
                    <TableCell>
                      <IconButton
                        color="info"
                        size="small"
                        onClick={() => showWorkDetails(work)}
                      >
                        <InfoIcon fontSize="small" />
                      </IconButton>
                    </TableCell>
                    <TableCell>{work.totalAuthors || '-'}</TableCell>
                    <TableCell>{author?.authorRoleName || '-'}</TableCell>
                    <TableCell>{author?.position !== undefined ? author.position : '-'}</TableCell>
                    <TableCell>{author?.purposeName || '-'}</TableCell>
                    <TableCell>{author?.fieldName || '-'}</TableCell>
                    <TableCell>{author?.scImagoFieldName || '-'}</TableCell>
                    <TableCell>{author?.scoreLevel !== undefined ? getScoreLevelText(author.scoreLevel) : '-'}</TableCell>
                    <TableCell>
                      <Box 
                        sx={{ 
                          display: 'flex', 
                          alignItems: 'center', 
                          cursor: 'pointer' 
                        }}
                        onClick={() => handleOpenEditDialog(work)}
                      >
                        {author?.workHour !== undefined ? author.workHour : '-'}
                      </Box>
                    </TableCell>
                    <TableCell>
                      <Box 
                        sx={{ 
                          display: 'flex', 
                          alignItems: 'center', 
                          cursor: 'pointer' 
                        }}
                        onClick={() => handleOpenEditDialog(work)}
                      >
                        {author?.authorHour !== undefined ? author.authorHour : '-'}
                      </Box>
                    </TableCell>
                    <TableCell>
                      <Tooltip title={author?.note || 'Không có ghi chú'}>
                        <IconButton
                          color="primary"
                          size="small"
                          onClick={() => handleOpenNoteDialog(work)}
                        >
                          <EditIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>
                    </TableCell>
                    <TableCell>
                      <Box sx={{ cursor: 'pointer' }} onClick={() => handleOpenStatusDialog(work)}>
                        {renderProofStatus(author?.proofStatus)}
                      </Box>
                    </TableCell>
                    <TableCell align="center">
                      <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                        <Tooltip title="Chỉnh sửa công trình">
                          <IconButton
                            color="primary"
                            size="small"
                            onClick={() => handleOpenEditDialog(work)}
                            sx={{ mr: 1 }}
                          >
                            <EditIcon fontSize="small" />
                          </IconButton>
                        </Tooltip>
                        <Tooltip title="Xóa công trình">
                          <IconButton
                            color="error"
                            size="small"
                            onClick={() => handleOpenDeleteDialog(work)}
                          >
                            <DeleteIcon fontSize="small" />
                          </IconButton>
                        </Tooltip>
                      </Box>
                    </TableCell>
                  </TableRow>
                );
              })}
              {works.length === 0 && (
                <TableRow>
                  <TableCell colSpan={18} align="center">
                    Người dùng này chưa có công trình nào
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      {/* Edit Hours Dialog */}
      <Dialog open={openEditDialog} onClose={handleCloseEditDialog}>
        <DialogTitle>Chỉnh sửa thông tin công trình</DialogTitle>
        <DialogContent>
          <Box sx={{ pt: 2 }}>
            <TextField
              fullWidth
              margin="normal"
              label="Giờ công trình"
              name="workHour"
              type="number"
              value={formData.workHour}
              onChange={handleFormChange}
              InputProps={{ inputProps: { min: 0, step: 0.5 } }}
            />
            <TextField
              fullWidth
              margin="normal"
              label="Giờ tác giả"
              name="authorHour"
              type="number"
              value={formData.authorHour}
              onChange={handleFormChange}
              InputProps={{ inputProps: { min: 0, step: 0.5 } }}
            />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseEditDialog}>Hủy</Button>
          <Button onClick={handleEditSubmit} variant="contained" color="primary">
            Cập nhật
          </Button>
        </DialogActions>
      </Dialog>

      {/* Status Dialog */}
      <Dialog open={openStatusDialog} onClose={handleCloseStatusDialog}>
        <DialogTitle>Cập nhật trạng thái công trình</DialogTitle>
        <DialogContent>
          <FormControl fullWidth margin="normal">
            <InputLabel id="status-select-label">Trạng thái</InputLabel>
            <Select
              labelId="status-select-label"
              value={newStatus}
              onChange={handleStatusChange}
              label="Trạng thái"
            >
              <MenuItem value={ProofStatus.HopLe}>Hợp lệ</MenuItem>
              <MenuItem value={ProofStatus.KhongHopLe}>Không hợp lệ</MenuItem>
              <MenuItem value={ProofStatus.ChuaXuLy}>Chưa xử lý</MenuItem>
            </Select>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseStatusDialog}>Hủy</Button>
          <Button onClick={handleStatusSubmit} variant="contained" color="primary">
            Cập nhật
          </Button>
        </DialogActions>
      </Dialog>

      {/* Note Dialog */}
      <Dialog open={openNoteDialog} onClose={handleCloseNoteDialog} fullWidth maxWidth="sm">
        <DialogTitle>Cập nhật ghi chú</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            multiline
            rows={4}
            margin="normal"
            label="Ghi chú"
            value={newNote}
            onChange={handleNoteChange}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseNoteDialog}>Hủy</Button>
          <Button onClick={handleNoteSubmit} variant="contained" color="primary">
            Cập nhật
          </Button>
        </DialogActions>
      </Dialog>

      {/* Delete Dialog */}
      <Dialog open={openDeleteDialog} onClose={handleCloseDeleteDialog}>
        <DialogTitle>Xác nhận xóa công trình</DialogTitle>
        <DialogContent>
          <Typography>Bạn có chắc chắn muốn xóa công trình này không?</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDeleteDialog}>Hủy</Button>
          <Button onClick={handleDeleteWork} variant="contained" color="error">
            Xóa
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default UserWorkDetailPage; 