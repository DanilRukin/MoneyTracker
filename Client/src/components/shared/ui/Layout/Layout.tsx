// shared/ui/Layout/Layout.tsx
import React from "react";
import { Box } from "@mui/material";
import { Sidebar } from "./Sidebar";

export const Layout: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  return (
    <Box
      sx={{
        display: "flex",
        height: "100vh",
        backgroundColor: "background.default",
        overflow: "hidden",
      }}
    >
      {/* Sidebar */}
      <Sidebar />

      {/* Main Content */}
      <Box
        component="main"
        sx={{
          flex: 1,
          overflow: "auto",
          marginLeft: { xs: 0, md: "256px" },
          width: { xs: "100%", md: "calc(100% - 256px)" },
          minWidth: 0,
        }}
      >
        {/* Контейнер, который будет растягиваться */}
        <Box
          sx={{
            width: "100%",
            minHeight: "100%",
            display: "flex",
            flexDirection: "column",
          }}
        >
          {children}
        </Box>
      </Box>
    </Box>
  );
};
