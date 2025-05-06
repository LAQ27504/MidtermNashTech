import {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
} from "react";
import { isTokenValid, getUserFromToken } from "../utils/auth"; // Assuming these are correctly defined

interface UserContextType {
  isSuperUser: boolean;
  setIsSuperUser: (v: boolean) => void;
  username: string;
  setUsername: (name: string) => void;
  userId: string;
  setUserId: (id: string) => void;
  isLoading: boolean;
}

const defaultUserContextValue: UserContextType = {
  isSuperUser: false,
  setIsSuperUser: () => {},
  username: "",
  setUsername: () => {},
  userId: "",
  setUserId: () => {},
  isLoading: true,
};

const UserContext = createContext<UserContextType>(defaultUserContextValue);

export const UserProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [isSuperUser, setIsSuperUser] = useState<boolean>(false);
  const [username, setUsername] = useState<string>("");
  const [userId, setUserId] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (token && isTokenValid()) {
      const user = getUserFromToken(token);

      if (user) {
        setUsername(user.name);
        setIsSuperUser(user.role === "SuperUser");
        setUserId(user.nameid);
      }
    }

    setIsLoading(false);
  }, []);

  const contextValue = {
    isSuperUser,
    setIsSuperUser,
    username,
    setUsername,
    userId,
    setUserId,
    isLoading,
  };

  return (
    <UserContext.Provider value={contextValue}>{children}</UserContext.Provider>
  );
};

export const useUserContext = () => useContext(UserContext);
