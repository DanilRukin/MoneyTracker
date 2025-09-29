/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable react-refresh/only-export-components */
// contexts/DevToolsContext.tsx
import React, {
  createContext,
  useContext,
  useState,
  type ReactNode,
} from "react";

interface DebugFormData {
  currentPage: string;
  devMode: boolean;
  theme: string;
  userName: string;
  serviceHealth: any;
  currencyPair: any;
  appState: any;
}

interface DevToolsContextType {
  isDebugFormOpen: boolean;
  openDebugForm: () => void;
  closeDebugForm: () => void;
  debugData: DebugFormData;
  updateDebugData: (data: Partial<DebugFormData>) => void;
}

const DevToolsContext = createContext<DevToolsContextType | undefined>(
  undefined
);

interface DevToolsProviderProps {
  children: ReactNode;
}

export const DevToolsProvider: React.FC<DevToolsProviderProps> = ({
  children,
}) => {
  const [isDebugFormOpen, setIsDebugFormOpen] = useState(false);
  const [debugData, setDebugData] = useState<DebugFormData>({
    currentPage: "",
    devMode: false,
    theme: "light",
    userName: "",
    serviceHealth: {},
    currencyPair: {},
    appState: {},
  });

  const openDebugForm = () => setIsDebugFormOpen(true);
  const closeDebugForm = () => setIsDebugFormOpen(false);

  const updateDebugData = (data: Partial<DebugFormData>) => {
    setDebugData((prev) => ({ ...prev, ...data }));
  };

  return (
    <DevToolsContext.Provider
      value={{
        isDebugFormOpen,
        openDebugForm,
        closeDebugForm,
        debugData,
        updateDebugData,
      }}
    >
      {children}
    </DevToolsContext.Provider>
  );
};

export const useDevTools = () => {
  const context = useContext(DevToolsContext);
  if (context === undefined) {
    throw new Error("useDevTools must be used within a DevToolsProvider");
  }
  return context;
};
