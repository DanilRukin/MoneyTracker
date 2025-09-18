import React, { type ReactNode } from "react";
import { useDevMode } from "../contexts";
import { Box, Paper } from "@mui/material";

interface WithDevToolsProps {
  children: ReactNode;
  componentName: string;
}

export const WithDevTools: React.FC<WithDevToolsProps> = ({
  children,
  componentName,
}) => {
  const { isDevMode } = useDevMode();

  if (!isDevMode) {
    return <>{children}</>;
  }

  return (
    <Box sx={{ position: "relative" }}>
      {children}
      <Paper
        sx={{
          position: "absolute",
          top: 4,
          right: 4,
          px: 1,
          py: 0.5,
          backgroundColor: "warning.light",
          color: "warning.contrastText",
          fontSize: "0.7rem",
          borderRadius: 1,
          zIndex: 1000,
        }}
      >
        🛠️ {componentName}
      </Paper>
    </Box>
  );
};
