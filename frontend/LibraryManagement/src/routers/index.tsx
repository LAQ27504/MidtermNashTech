import Home from "@/pages/Home";
import Layout from "@/pages/Layout";
import LoginPage from "@/pages/Login";
import NotFound from "@/pages/NotFound";

export const pathName = {
  home: "",
  login: "/login",
};

export const AppRoutes = [
  { path: "*", element: <NotFound /> },
  { path: pathName.login, element: <LoginPage /> },
  {
    element: <Layout />,
    children: [{ path: pathName.home, element: <Home /> }],
  },
];
