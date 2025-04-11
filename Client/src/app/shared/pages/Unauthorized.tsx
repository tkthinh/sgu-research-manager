import { Button, Typography, Container } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const Unauthorized = () => {
  const navigate = useNavigate();

  const handleGoHome = () => {
    navigate('/');
  };

  return (
    <Container
      maxWidth="md"
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        height: '100vh',
        textAlign: 'center',
      }}
    >
      <Typography variant="h1" component="h1" gutterBottom sx={{ fontWeight: 'bold' }}>
        401
      </Typography>
      <Typography variant="h5" gutterBottom>
        Bạn không có quyền truy cập tài nguyên này.
      </Typography>
      <Button variant="contained" color="primary" onClick={handleGoHome}>
        Trở về trang chủ
      </Button>
    </Container>
  );
};

export default Unauthorized;
