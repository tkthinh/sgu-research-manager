import { Box, Button } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function Setting() {
  const navigate = useNavigate();

  return (
    <>
      <Box display="flex" gap={2} mb={4}>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Hệ số chấm điểm
        </Button>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Vai trò tác giả
        </Button>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Mục đích quy đổi
        </Button>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Tình trạng minh chứng
        </Button>
      </Box>
      <Box display="flex" gap={2} mb={4}>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Loại công trình
        </Button>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Cấp công trình
        </Button>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Tình trạng công trình
        </Button>
      </Box>
      <Box display="flex" gap={2} mb={4}>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Ngành
        </Button>
        <Button variant="outlined" onClick={() => navigate("/")} size="large" sx={{width: 250}}>
          Đơn vị
        </Button>
      </Box>
    </>
  );
}
