import { UserContext } from '../../contexts';
import { Container, Box, Typography } from '@mui/material';
import { useContext, useEffect } from 'react';

const Home = () => {
  const { username } = useContext(UserContext);

  return (
    <Container component="main" maxWidth="sm">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        {!!username ? (
          <Typography fontWeight={'bold'} fontSize={'40px'}>
            🛂 You are logged in as: {username} 🛂
          </Typography>
        ) : (
          <Typography fontWeight={'bold'} fontSize={'40px'}>
            ⛔️You are not logged in.⛔️
          </Typography>
        )}
      </Box>
    </Container>
  );
};

export default Home;
