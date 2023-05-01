import { Toolbar, Typography, Button, AppBar } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const NavBar = () => {
  const navigate = useNavigate();

  return (
    <AppBar position="static" color="secondary">
      <Toolbar>
        <Typography variant="h5" fontWeight={'bold'}>
          OAUTHDEMO
        </Typography>

        <Button color="inherit" onClick={() => navigate('/login')} sx={{
          marginLeft: '20px',
        }}>
          Login
        </Button>
        <Button color="inherit" onClick={() => navigate('/')}>
          Home
        </Button>
      </Toolbar>
    </AppBar>
  );
};

export default NavBar;
