import { Counter } from "./components/Counter";
import Home from "./Pages/Home/Home";
import Backup from "./Pages/Backup/Backup";
import Post from "./Pages/Post/Post";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/backup',
    element: <Backup />
  },
  {
    path: '/post',
    element: <Post />
  }
];

export default AppRoutes;
