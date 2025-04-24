import Home from "@/pages/Home";
import Layout from "@/pages/Layout";
import LoginPage from "@/pages/Login";
import NotFound from "@/pages/NotFound";
import Register from "@/pages/Register";

export const pathName = {
  home: "",
  login: "/login",
  register: "/register",
};

export const AppRoutes = [
  { path: "*", element: <NotFound /> },
  { path: pathName.login, element: <LoginPage /> },
  { path: pathName.register, element: <Register /> },
  {
    element: <Layout />,
    children: [{ path: pathName.home, element: <Home /> }],
  },
];
