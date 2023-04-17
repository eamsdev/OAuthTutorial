import Home from './pages/Home';
import { Route, Routes } from 'react-router-dom';


const App = () => {
  return (
    <Routes>
      <Route element={<Home />} path="/" />
    </Routes>
  );
};

export default App;