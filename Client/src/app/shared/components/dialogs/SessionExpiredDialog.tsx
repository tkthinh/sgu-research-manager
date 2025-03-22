import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Button,
} from "@mui/material";

interface Props {
  open: boolean;
  onClose: () => void;
}

export default function SessionExpiredDialog({ open, onClose }: Props) {
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Phiên đăng nhập đã hết hạn</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Vui lòng đăng nhập lại để tiếp tục sử dụng hệ thống.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} autoFocus>
          Đăng nhập lại
        </Button>
      </DialogActions>
    </Dialog>
  );
}
