import { createTheme } from '@mui/material';
import { grey } from '@mui/material/colors';


declare module '@mui/material/Button' {
  interface ButtonPropsVariantOverrides {
    github: true;
  }
}

const theme = createTheme({
  components: {
    MuiButton: {
      variants: [
        {
          props: { variant: 'github' },
          style: {
            backgroundColor: `${grey[900]}`,
            color: `${grey[100]}`,
            boxShadow:
              '0px 3px 1px -2px rgba(0,0,0,0.2), 0px 2px 2px 0px rgba(0,0,0,0.14), 0px 1px 5px 0px rgba(0,0,0,0.12);',
            '&:hover': {
              backgroundColor: `${grey[800]}`,
            },
          },
        },
      ],
    },
  },
});

export default theme;