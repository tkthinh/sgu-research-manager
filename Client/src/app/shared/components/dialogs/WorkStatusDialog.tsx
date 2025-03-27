import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Typography,
  CircularProgress,
  SelectChangeEvent,
} from "@mui/material";
import { ProofStatus } from "../../../../lib/types/enums/ProofStatus";

interface WorkStatusDialogProps {
  open: boolean;
  onClose: () => void;
  status: ProofStatus;
  onStatusChange: (event: SelectChangeEvent<number>) => void;
  onSubmit: () => void;
  isPending: boolean;
}

/**
 * Dialog cập nhật trạng thái công trình tái sử dụng
 */
export default function WorkStatusDialog({
  open,
  onClose,
  status,
  onStatusChange,
  onSubmit,
  isPending,
}: WorkStatusDialogProps) {
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Cập nhật trạng thái công trình</DialogTitle>
      <DialogContent>
        <FormControl fullWidth margin="normal">
          <InputLabel id="status-select-label">Trạng thái</InputLabel>
          <Select
            labelId="status-select-label"
            value={status}
            onChange={onStatusChange}
            label="Trạng thái"
          >
            <MenuItem value={ProofStatus.HopLe}>Hợp lệ</MenuItem>
            <MenuItem value={ProofStatus.KhongHopLe}>Không hợp lệ</MenuItem>
            <MenuItem value={ProofStatus.ChuaXuLy}>Chưa xử lý</MenuItem>
          </Select>
        </FormControl>
        <Typography variant="caption" color="textSecondary" sx={{ mt: 1, display: 'block' }}>
          Dialog sẽ tự động đóng sau khi cập nhật thành công.
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Hủy</Button>
        <Button
          onClick={onSubmit}
          variant="contained"
          color="primary"
          disabled={isPending}
        >
          {isPending ? <CircularProgress size={24} /> : "Cập nhật"}
        </Button>
      </DialogActions>
    </Dialog>
  );
} 