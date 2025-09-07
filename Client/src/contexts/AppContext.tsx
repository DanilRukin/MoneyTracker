/* eslint-disable react-refresh/only-export-components */
// app/AppContext.tsx
import { createContext, useContext, useState, type ReactNode } from "react";
import { type PaletteMode } from "@mui/material";

interface AppContextType {
  currentPage: string;
  setCurrentPage: (page: string) => void;
  theme: PaletteMode;
  setTheme: (theme: PaletteMode) => void;
  devMode: boolean;
  setDevMode: (enabled: boolean) => void;
  toggleDevMode: () => void;
  userName: string;
  setUserName: (name: string) => void;
}

const AppContext = createContext<AppContextType | null>(null);

interface AppProviderProps {
  children: ReactNode;
}

export const AppProvider: React.FC<AppProviderProps> = ({ children }) => {
  const [currentPage, setCurrentPage] = useState("dashboard");
  const [theme, setTheme] = useState<PaletteMode>("light");
  const [devMode, setDevMode] = useState(false);
  const [userName, setUserName] = useState("Пользователь");
  const toggleDevMode = () => {
    setDevMode((prev) => !prev);
  };

  const contextValue: AppContextType = {
    currentPage,
    setCurrentPage,
    theme,
    setTheme,
    devMode,
    setDevMode,
    toggleDevMode,
    userName,
    setUserName,
  };

  return (
    <AppContext.Provider value={contextValue}>{children}</AppContext.Provider>
  );
};

export const useApp = () => {
  const context = useContext(AppContext);
  if (!context) {
    throw new Error("useApp must be used within AppProvider");
  }
  return context;
};
