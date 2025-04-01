import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";
import { useLocation } from "react-router-dom";

interface Props {
  open: boolean;
  onClose: () => void;
}

export default function ProfileIncompletedDialog({ open, onClose }: Props) {
  const location = useLocation();
  if (location.pathname === "/cap-nhat-thong-tin") {
    return null;
  }

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Câp nhật thông tin</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Vui lòng cập nhật thông tin trước khi sử dụng hệ thống.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} autoFocus>
          Câp nhật thông tin
        </Button>
      </DialogActions>
    </Dialog>
  );
}
