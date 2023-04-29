import { Home } from './pages/Home';
import { Login } from './pages/Login';
import { Route, Routes } from 'react-router-dom';

const App = () => {
  return (
    <Routes>
      <Route element={<Home />} path="/" />
      <Route element={<Login />} path="/login" />
    </Routes>
  );
};

export default App;
