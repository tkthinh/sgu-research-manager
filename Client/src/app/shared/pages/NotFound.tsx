import { Button, Typography, Container } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const NotFound = () => {
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
        404
      </Typography>
      <Typography variant="h5" gutterBottom>
        Tài nguyên bạn yêu cầu không tồn tại.
      </Typography>
      <Button variant="contained" color="primary" onClick={handleGoHome}>
        Trở về trang chủ
      </Button>
    </Container>
  );
};

export default NotFound;
