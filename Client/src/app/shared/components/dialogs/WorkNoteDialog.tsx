import React from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  Typography,
  CircularProgress,
} from "@mui/material";

interface WorkNoteDialogProps {
  open: boolean;
  onClose: () => void;
  note: string;
  onNoteChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onSubmit: () => void;
  isPending: boolean;
}

/**
 * Dialog cập nhật ghi chú công trình tái sử dụng
 */
export default function WorkNoteDialog({
  open,
  onClose,
  note,
  onNoteChange,
  onSubmit,
  isPending,
}: WorkNoteDialogProps) {
  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle>Cập nhật ghi chú</DialogTitle>
      <DialogContent>
        <TextField
          fullWidth
          multiline
          rows={4}
          margin="normal"
          label="Ghi chú"
          value={note}
          onChange={onNoteChange}
        />
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