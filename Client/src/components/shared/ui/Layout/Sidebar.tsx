import React, { useState } from "react";
import {
  NavigationIds,
  NavigationLabels,
  NavigationRoutes,
} from "../../../../infrastructure/constants/Navigation";
import {
  Brightness4,
  Brightness7,
  BugReport,
  Code,
  Flag,
  Logout,
  Settings,
  Wallet,
  PieChart,
  LocalLibrary,
  ModeStandby,
  Calculate,
  TrendingUp,
  ShoppingCart,
  AccountBalance,
  Dashboard,
  DeveloperMode,
  ContentCopy,
  Check,
} from "@mui/icons-material";
import {
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Switch,
  Typography,
  Button,
  Avatar,
  Box,
  Badge,
  Divider,
  Snackbar,
  Alert,
  Menu,
  MenuItem,
} from "@mui/material";
import { useApp } from "../../../../contexts/AppContext";

// Описываем структуру пункта меню с помощью TypeScript Interface
interface NavigationItem {
  id: string;
  label: string;
  icon: React.ReactElement;
  path: string;
}

// Массив пунктов меню - это наши данные
const mainNavigationItems: NavigationItem[] = [
  {
    id: NavigationIds.HOME,
    label: NavigationLabels.HOME,
    icon: <Dashboard />,
    path: NavigationRoutes.HOME,
  },
  {
    id: NavigationIds.TRANSACTIONS,
    label: NavigationLabels.TRANSACTIONS,
    icon: <ShoppingCart />,
    path: NavigationRoutes.TRANSACTIONS,
  },
  {
    id: NavigationIds.ACCOUNTS,
    label: NavigationLabels.ACCOUNTS,
    icon: <AccountBalance />,
    path: NavigationRoutes.ACCOUNTS,
  },
  {
    id: NavigationIds.BUDGET,
    label: NavigationLabels.BUDGET,
    icon: <PieChart />,
    path: NavigationRoutes.BUDGET,
  },
  {
    id: NavigationIds.GOALS,
    label: NavigationLabels.GOALS,
    icon: <ModeStandby />,
    path: NavigationRoutes.GOALS,
  },
  {
    id: NavigationIds.INVEST,
    label: NavigationLabels.INVEST,
    icon: <TrendingUp />,
    path: NavigationRoutes.INVEST,
  },
  {
    id: NavigationIds.CALCULATORS,
    label: NavigationLabels.CALCULATORS,
    icon: <Calculate />,
    path: NavigationRoutes.CALCULATORS,
  },
  {
    id: NavigationIds.LEARNING,
    label: NavigationLabels.LEARNING,
    icon: <LocalLibrary />,
    path: NavigationRoutes.LEARNING,
  },
];

const devNavigationItems: NavigationItem[] = [
  {
    id: NavigationIds.DEV_PANEL,
    label: NavigationLabels.DEV_PANEL,
    icon: <Code />,
    path: NavigationRoutes.DEV_PANEL,
  },
  {
    id: NavigationIds.DEV_LOGS,
    label: NavigationLabels.DEV_LOGS,
    icon: <BugReport />,
    path: NavigationRoutes.DEV_LOGS,
  },
  {
    id: NavigationIds.DEV_FLAGS,
    label: NavigationLabels.DEV_FLAGS,
    icon: <Flag />,
    path: NavigationRoutes.DEV_FLAGS,
  },
];

// React-компонент: функция возвращающая JSX
export const Sidebar: React.FC = () => {
  const {
    currentPage,
    setCurrentPage,
    userName,
    theme: appTheme,
    setTheme,
    devMode,
    toggleDevMode,
  } = useApp();

  const [contextMenu, setContextMenu] = useState<{
    mouseX: number;
    mouseY: number;
    itemId?: string;
    itemLabel?: string;
  } | null>(null);

  const [snackbar, setSnackbar] = useState<{
    open: boolean;
    message: string;
    severity?: "success" | "info" | "warning" | "error";
  }>({ open: false, message: "" });

  const handleThemeToggle = () => {
    setTheme(appTheme === "light" ? "dark" : "light");
  };

  const handleSettingsClick = () => {
    setCurrentPage("settings");
  };

  const handleNavigationClick = (pageId: string) => {
    setCurrentPage(pageId);
    setContextMenu(null);
  };

  const handleContextMenu = (event: React.MouseEvent, item: NavigationItem) => {
    event.preventDefault();
    setContextMenu({
      mouseX: event.clientX + 2,
      mouseY: event.clientY - 6,
      itemId: item.id,
      itemLabel: item.label,
    });
  };

  const handleCloseContextMenu = () => {
    setContextMenu(null);
  };

  const handleCopyId = () => {
    if (contextMenu?.itemId) {
      navigator.clipboard.writeText(contextMenu.itemId);
      setSnackbar({
        open: true,
        message: `ID скопирован: ${contextMenu.itemId}`,
        severity: "success",
      });
      handleCloseContextMenu();
    }
  };

  const handleInspectItem = () => {
    if (contextMenu?.itemId) {
      console.log("Inspecting item:", {
        id: contextMenu.itemId,
        label: contextMenu.itemLabel,
        currentPage,
        devMode,
      });
      setSnackbar({
        open: true,
        message: `Инспектируем: ${contextMenu.itemLabel}`,
        severity: "info",
      });
      handleCloseContextMenu();
    }
  };

  const handleToggleDevModeFromContext = () => {
    toggleDevMode();
    setSnackbar({
      open: true,
      message: `DevMode ${!devMode ? "включен" : "выключен"}`,
      severity: "warning",
    });
    handleCloseContextMenu();
  };

  const handleQuickDevToggle = (event: React.MouseEvent) => {
    if (event.altKey && event.shiftKey) {
      event.preventDefault();
      toggleDevMode();
      setSnackbar({
        open: true,
        message: `DevMode ${!devMode ? "включен" : "выключен"}`,
        severity: "warning",
      });
    }
  };

  return (
    <>
      <Box
        sx={{
          position: "fixed",
          left: 0,
          top: 0,
          height: "100vh",
          width: 256,
          backgroundColor: "background.paper",
          borderRight: 1,
          borderColor: "divider",
          display: "flex",
          flexDirection: "column",
          zIndex: 1000,
        }}
        onContextMenu={handleQuickDevToggle}
      >
        {/* Logo Section с возможностью быстрого переключения devMode */}
        <Box
          sx={{ p: 3, borderBottom: 1, borderColor: "divider" }}
          onContextMenu={(e) => {
            e.preventDefault();
            toggleDevMode();
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
            <Box
              sx={{
                width: 32,
                height: 32,
                backgroundColor: devMode ? "warning.main" : "primary.main",
                borderRadius: 2,
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                flexShrink: 0,
                transition: "background-color 0.3s ease",
              }}
            >
              <Wallet sx={{ color: "white", fontSize: 20 }} />
            </Box>
            <Box>
              <Typography
                variant="h6"
                component="h1"
                sx={{
                  fontWeight: 600,
                  color: "text.primary",
                  lineHeight: 1.2,
                }}
              >
                MoneyTracker
              </Typography>
              {devMode && (
                <Badge
                  sx={{
                    backgroundColor: "warning.main",
                    color: "warning.contrastText",
                    fontSize: "0.7rem",
                    height: 20,
                    mt: 0.5,
                    px: 1,
                    borderRadius: 1,
                  }}
                >
                  DEV MODE
                </Badge>
              )}
            </Box>
          </Box>
        </Box>

        {/* Navigation */}
        <Box sx={{ flex: 1, overflowY: "auto", py: 2 }}>
          <List sx={{ p: 0 }}>
            {mainNavigationItems.map((item) => (
              <ListItem key={item.id} disablePadding sx={{ mb: 0.5 }}>
                <ListItemButton
                  onClick={() => handleNavigationClick(item.id)}
                  onContextMenu={(e) => handleContextMenu(e, item)}
                  selected={currentPage === item.id}
                  sx={{
                    mx: 1,
                    borderRadius: 1,
                    "&.Mui-selected": {
                      backgroundColor: "primary.main",
                      color: "white",
                      "&:hover": {
                        backgroundColor: "primary.dark",
                      },
                    },
                    "&:hover": {
                      backgroundColor: "action.hover",
                    },
                  }}
                >
                  <ListItemIcon
                    sx={{
                      color: currentPage === item.id ? "white" : "inherit",
                      minWidth: 40,
                    }}
                  >
                    {item.icon}
                  </ListItemIcon>
                  <ListItemText
                    primary={item.label}
                    primaryTypographyProps={{ fontSize: "0.9rem" }}
                  />
                </ListItemButton>
              </ListItem>
            ))}
          </List>

          {/* Dev Navigation */}
          {devMode && (
            <>
              <Divider sx={{ my: 2, mx: 2 }} />
              <Box sx={{ px: 2, mb: 1 }}>
                <Typography
                  variant="caption"
                  color="warning.main"
                  fontWeight="bold"
                >
                  🛠️ ДЛЯ РАЗРАБОТЧИКОВ
                </Typography>
              </Box>
              <List sx={{ p: 0 }}>
                {devNavigationItems.map((item) => (
                  <ListItem key={item.id} disablePadding sx={{ mb: 0.5 }}>
                    <ListItemButton
                      onClick={() => handleNavigationClick(item.id)}
                      onContextMenu={(e) => handleContextMenu(e, item)}
                      selected={currentPage === item.id}
                      sx={{
                        mx: 1,
                        borderRadius: 1,
                        border: "1px dashed",
                        borderColor: "warning.light",
                        "&.Mui-selected": {
                          backgroundColor: "warning.main",
                          color: "warning.contrastText",
                          "&:hover": {
                            backgroundColor: "warning.dark",
                          },
                        },
                        "&:hover": {
                          backgroundColor: "warning.light",
                          color: "warning.contrastText",
                        },
                      }}
                    >
                      <ListItemIcon
                        sx={{
                          color:
                            currentPage === item.id
                              ? "warning.contrastText"
                              : "warning.main",
                          minWidth: 40,
                        }}
                      >
                        {item.icon}
                      </ListItemIcon>
                      <ListItemText
                        primary={item.label}
                        primaryTypographyProps={{
                          fontSize: "0.9rem",
                          fontWeight: "medium",
                        }}
                      />
                    </ListItemButton>
                  </ListItem>
                ))}
              </List>
            </>
          )}
        </Box>

        {/* User Section */}
        <Box
          sx={{
            p: 2,
            borderTop: 1,
            borderColor: "divider",
            flexShrink: 0,
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center", gap: 2, mb: 2 }}>
            <Avatar
              sx={{
                bgcolor: devMode ? "warning.main" : "primary.main",
                width: 40,
                height: 40,
                fontSize: "1rem",
                flexShrink: 0,
                transition: "background-color 0.3s ease",
              }}
            >
              {userName.charAt(0).toUpperCase()}
            </Avatar>
            <Box sx={{ minWidth: 0, flex: 1 }}>
              <Typography variant="body2" fontWeight="medium" noWrap>
                {userName}
              </Typography>
              <Typography variant="caption" color="text.secondary" noWrap>
                {devMode ? "developer@moneyTracker" : "user@example.com"}
              </Typography>
            </Box>
          </Box>

          <Box>
            {/* Theme Toggle */}
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
                justifyContent: "space-between",
                mb: 2,
              }}
            >
              <Typography variant="caption" color="text.secondary">
                {appTheme === "light" ? "Светлая" : "Темная"} тема
              </Typography>
              <Box sx={{ display: "flex", alignItems: "center" }}>
                {appTheme === "light" ? (
                  <Brightness4 sx={{ fontSize: 18, mr: 0.5 }} />
                ) : (
                  <Brightness7 sx={{ fontSize: 18, mr: 0.5 }} />
                )}
                <Switch
                  size="small"
                  checked={appTheme === "dark"}
                  onChange={handleThemeToggle}
                />
              </Box>
            </Box>

            {/* Dev Mode Toggle */}
            {devMode && (
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "space-between",
                  mb: 2,
                  p: 1,
                  backgroundColor: "warning.light",
                  borderRadius: 1,
                }}
              >
                <Typography variant="caption" color="warning.dark">
                  Режим разработчика
                </Typography>
                <Switch
                  size="small"
                  checked={devMode}
                  onChange={toggleDevMode}
                  color="warning"
                />
              </Box>
            )}

            {/* Buttons */}
            <Box sx={{ display: "flex", gap: 1 }}>
              <Button
                variant="outlined"
                size="small"
                fullWidth
                startIcon={<Settings />}
                onClick={handleSettingsClick}
                sx={{
                  fontSize: "0.75rem",
                  py: 0.5,
                }}
              >
                Настройки
              </Button>
              <Button
                variant="outlined"
                size="small"
                onClick={() => console.log("Logout")}
                sx={{
                  minWidth: "auto",
                  px: 1,
                  py: 0.5,
                }}
              >
                <Logout fontSize="small" />
              </Button>
            </Box>
          </Box>
        </Box>
      </Box>

      {/* Контекстное меню */}
      <Menu
        open={contextMenu !== null}
        onClose={handleCloseContextMenu}
        anchorReference="anchorPosition"
        anchorPosition={
          contextMenu !== null
            ? { top: contextMenu.mouseY, left: contextMenu.mouseX }
            : undefined
        }
      >
        <MenuItem onClick={handleCopyId}>
          <ListItemIcon>
            <ContentCopy fontSize="small" />
          </ListItemIcon>
          <ListItemText>Копировать ID</ListItemText>
          <Typography variant="body2" color="text.secondary" sx={{ ml: 2 }}>
            {contextMenu?.itemId}
          </Typography>
        </MenuItem>

        <MenuItem onClick={handleInspectItem}>
          <ListItemIcon>
            <Check fontSize="small" />
          </ListItemIcon>
          <ListItemText>Инспектировать</ListItemText>
        </MenuItem>

        <MenuItem onClick={handleToggleDevModeFromContext}>
          <ListItemIcon>
            <DeveloperMode fontSize="small" />
          </ListItemIcon>
          <ListItemText>
            {devMode ? "Выключить" : "Включить"} DevMode
          </ListItemText>
        </MenuItem>

        <Divider />

        <MenuItem onClick={handleCloseContextMenu}>
          <ListItemText>Закрыть</ListItemText>
        </MenuItem>
      </Menu>

      {/* Уведомления */}
      <Snackbar
        open={snackbar.open}
        autoHideDuration={3000}
        onClose={() => setSnackbar({ ...snackbar, open: false })}
        anchorOrigin={{ vertical: "bottom", horizontal: "left" }}
      >
        <Alert
          severity={snackbar.severity}
          variant="filled"
          onClose={() => setSnackbar({ ...snackbar, open: false })}
        >
          {snackbar.message}
        </Alert>
      </Snackbar>
    </>
  );
};
