import { useRoutes } from "react-router-dom";
import { AppRoutes } from "./routers";

const App = () => {
  const element = useRoutes(AppRoutes);

  return <div>{element}</div>;
};

export default App;
