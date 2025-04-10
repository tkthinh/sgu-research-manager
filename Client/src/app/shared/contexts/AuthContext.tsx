import { createContext, useContext, useState, useEffect } from "react";
import { User } from "../../../lib/types/models/User";
import { getMyInfo } from "../../../lib/api/usersApi";
import { jwtDecode } from "jwt-decode";

interface AuthContextType {
  user: User | null;
  setUser: (user: User | null) => void;
  signOut: () => void;
  loading: boolean;
  refreshUserInfo: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  const refreshUserInfo = async () => {
    const token = localStorage.getItem("token");
    if (!token) {
      setLoading(false);
      return;
    }

    try {
      const response = await getMyInfo();
      if (response.success) {
        const userData = response.data;
        // Add role from token
        userData.role = jwtDecode(token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        setUser(userData);
      }
    } catch (error) {
      console.error("Failed to fetch user info:", error);
    } finally {
      setLoading(false);
    }
  };

  // Fetch user data on initial load
  useEffect(() => {
    refreshUserInfo();
  }, []);

  const signOut = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("expiration");
    setUser(null);
    window.location.href = "/dang-nhap";
  };

  return (
    <AuthContext.Provider value={{ user, setUser, signOut, loading, refreshUserInfo }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
