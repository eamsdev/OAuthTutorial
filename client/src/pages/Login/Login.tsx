import GitHubIcon from '@mui/icons-material/GitHub';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { Avatar, Box, Button, Checkbox, Container, FormControlLabel, TextField, Typography } from '@mui/material';
import { useState } from 'react';


type LoginForm = {
  email?: string,
  password?: string
}

type FormEvent = React.FormEvent<HTMLFormElement>;
type ChangeEvent = React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>;

const Login = () => {
  const [form, setForm] = useState<LoginForm>({})

  const onSubmit = (evt: FormEvent) => {
    evt.preventDefault();
    console.log(form);
  };

  const onChange = (evt: ChangeEvent) => {
    const { name, value } = evt.target;
    setForm({ ...form, [name]: value });
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
          Sign in
        </Typography>
        <Box component="form" onSubmit={onSubmit} noValidate sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            label="Email Address"
            name="email"
            autoComplete="email"
            autoFocus
            value={form["email"]}
            onChange={onChange}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Password"
            type="password"
            autoComplete="current-password"
            value={form["password"]}
            onChange={onChange}
          />
          <Button type="submit" fullWidth variant="contained" sx={{ mt: 3 }}>
            <Typography textTransform={'none'}>Sign in</Typography>
          </Button>
          <Button
            fullWidth
            variant="github"
            sx={{ mt: 1, mb: 2 }}
            onClick={() => console.warn('TODO: Signin with Github')}
          >
            <Typography textTransform={'none'} mr={1}>
              Sign in with
            </Typography>
            <GitHubIcon />
          </Button>
        </Box>
      </Box>
    </Container>
  );
};

export default Login;