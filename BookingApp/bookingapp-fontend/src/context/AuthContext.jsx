import { createContext, useContext, useMemo, useState } from "react";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem("token"));
  const [userRole, setUserRole] = useState(localStorage.getItem("role"));
  const [userEmail, setUserEmail] = useState(localStorage.getItem("email"));

  const login = (authData) => {
    localStorage.setItem("token", authData.accessToken);
    localStorage.setItem("role", authData.role);
    localStorage.setItem("email", authData.email);

    setToken(authData.accessToken);
    setUserRole(authData.role);
    setUserEmail(authData.email);
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("email");

    setToken(null);
    setUserRole(null);
    setUserEmail(null);
  };

  const value = useMemo(
    () => ({
      token,
      userRole,
      userEmail,
      isAuthenticated: !!token,
      isAdmin: userRole === "Admin",
      login,
      logout,
    }),
    [token, userRole, userEmail]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  return useContext(AuthContext);
}