/* eslint-disable react-refresh/only-export-components */
import React, {
  useState,
  useContext,
  createContext,
  type ReactNode,
} from "react";

interface DevModeContextType {
  isDevMode: boolean;
  toggleDevMode: () => void;
  enableDevMode: () => void;
  disableDevMode: () => void;
}

const DevModeContext = createContext<DevModeContextType | undefined>(undefined);

interface DevModeProviderProps {
  children: ReactNode;
}

export const DevModeProvider: React.FC<DevModeProviderProps> = ({
  children,
}) => {
  const [isDevMode, setIsDevMode] = useState(false);
  const toggleDevMode = () => {
    setIsDevMode((prev) => !prev);
    console.log(`DevMode ${!isDevMode ? "enabled" : "disabled"}`);
  };

  const enableDevMode = () => {
    setIsDevMode(true);
  };

  const disableDevMode = () => {
    setIsDevMode(false);
  };

  return (
    <DevModeContext.Provider
      value={{ isDevMode, toggleDevMode, enableDevMode, disableDevMode }}
    >
      {children}
    </DevModeContext.Provider>
  );
};

export const useDevMode = (): DevModeContextType => {
  const context = useContext(DevModeContext);
  if (context === undefined)
    throw new Error("useDevMode must be used within a DevModeProvider");
  return context;
};
