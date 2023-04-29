import { UserContext } from '../../contexts';
import { Container, Box, Typography } from '@mui/material';
import { useContext, useEffect } from 'react';

const Home = () => {
  const { username } = useContext(UserContext);

  const textDisplay = (text: string) => (
    <Typography fontWeight={'bold'} fontSize={'40px'}>
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
        {!!username
          ? textDisplay(`🛂 You are logged in as: ${username} 🛂`)
          : textDisplay(`⛔️You are not logged in.⛔️`)}
      </Box>
    </Container>
  );
};

export default Home;
