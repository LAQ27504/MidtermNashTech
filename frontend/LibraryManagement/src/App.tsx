import { useRoutes } from "react-router-dom";
import { index } from "./routers"; // Import the index routes
import { AuthProvider } from "./context/authContext";
import { UserProvider } from "./context/userContext";

function App() {
  const elements = useRoutes(index); // Use the index routes
  return (
    <AuthProvider>
      <UserProvider>
        <div>{elements}</div>
      </UserProvider>
    </AuthProvider>
  );
}

export default App;
