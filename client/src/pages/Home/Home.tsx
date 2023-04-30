import { IdentityApi } from '../../api';
import { UserContext } from '../../contexts';
import { Container, Box, Typography, CircularProgress } from '@mui/material';
import { useContext, useEffect } from 'react';

const Home = () => {
  const { username, refreshUser, state } = useContext(UserContext);

  useEffect(() => {
    refreshUser();
  }, []);
  
  const textDisplay = (text: string) => (
    <Typography fontWeight={'bold'} fontSize={'36px'}>
      {text}
    </Typography>
  );

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
        {state === 'loading' || state === 'idle' ? (
          <CircularProgress />
        ) : !!username ? (
          textDisplay(`🛂 You are logged in as: ${username}`)
        ) : (
          textDisplay(`⛔️ You are not logged in.`)
        )}
      </Box>
    </Container>
  );
};

export default Home;
