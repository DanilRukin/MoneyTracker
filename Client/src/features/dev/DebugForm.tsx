/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState, useEffect } from "react";
import {
  Dialog,
  DialogContent,
  DialogTitle,
  DialogActions,
  Box,
  Typography,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Switch,
  FormControlLabel,
  Tab,
  Tabs,
  Grid,
  Paper,
  Chip,
  Alert,
  IconButton,
  Tooltip,
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@mui/material";
import {
  Close as CloseIcon,
  ExpandMore as ExpandMoreIcon,
  Code as CodeIcon,
  Settings as SettingsIcon,
  BugReport as BugReportIcon,
  Storage as StorageIcon,
  ContentCopy as CopyIcon,
  Refresh as RefreshIcon,
} from "@mui/icons-material";
import { useDevTools } from "../../contexts/DevToolsContext";
import { useApp } from "../../contexts";
import { useDevMode } from "../../contexts";
import { useDevLogger } from "../../hooks";

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

const TabPanel: React.FC<TabPanelProps> = ({ children, value, index }) => {
  return (
    <div hidden={value !== index}>
      {value === index && <Box sx={{ p: 2 }}>{children}</Box>}
    </div>
  );
};

export const DebugForm: React.FC = () => {
  const { isDebugFormOpen, closeDebugForm, debugData, updateDebugData } =
    useDevTools();
  const {
    currentPage,
    setCurrentPage,
    userName,
    setUserName,
    theme,
    setTheme,
  } = useApp();
  const { isDevMode, toggleDevMode } = useDevMode();
  const { devLog } = useDevLogger();

  const [tabValue, setTabValue] = useState(0);
  const [tempData, setTempData] = useState(debugData);

  useEffect(() => {
    if (isDebugFormOpen) {
      // Обновляем данные при открытии формы
      const newData = {
        currentPage,
        devMode: isDevMode,
        theme,
        userName,
        serviceHealth: {},
        currencyPair: {},
        appState: {
          timestamp: new Date().toISOString(),
          userAgent: navigator.userAgent,
          viewport: `${window.innerWidth}x${window.innerHeight}`,
          url: window.location.href,
        },
      };
      setTempData(newData);
      updateDebugData(newData);
      devLog("Debug form opened", newData);
    }
  }, [isDebugFormOpen, currentPage, isDevMode, theme, userName]);

  const handleSave = () => {
    updateDebugData(tempData);

    // Применяем изменения
    if (tempData.currentPage !== currentPage) {
      setCurrentPage(tempData.currentPage);
    }
    if (tempData.userName !== userName) {
      setUserName(tempData.userName);
    }
    if (tempData.theme !== theme) {
      setTheme(tempData.theme as any);
    }

    devLog("Debug form changes applied", tempData);
    closeDebugForm();
  };

  const handleReset = () => {
    setTempData(debugData);
    devLog("Debug form reset");
  };

  const handleCopyToClipboard = () => {
    navigator.clipboard.writeText(JSON.stringify(debugData, null, 2));
    devLog("Debug data copied to clipboard");
  };

  const handleRefreshData = () => {
    const newData = {
      ...tempData,
      appState: {
        ...tempData.appState,
        timestamp: new Date().toISOString(),
        viewport: `${window.innerWidth}x${window.innerHeight}`,
      },
    };
    setTempData(newData);
    devLog("Debug data refreshed");
  };

  const availablePages = [
    "dashboard",
    "transactions",
    "accounts",
    "budget",
    "goals",
    "invest",
    "calculators",
    "learning",
    "settings",
    "dev-panel",
    "currency-service",
  ];

  return (
    <Dialog
      open={isDebugFormOpen}
      onClose={closeDebugForm}
      maxWidth="lg"
      fullWidth
      PaperProps={{
        sx: {
          minHeight: "80vh",
          maxHeight: "90vh",
        },
      }}
    >
      <DialogTitle>
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <BugReportIcon color="primary" />
            <Typography variant="h6">Панель разработчика</Typography>
            <Chip label="Debug" color="warning" size="small" />
          </Box>
          <Box sx={{ display: "flex", gap: 1 }}>
            <Tooltip title="Обновить данные">
              <IconButton onClick={handleRefreshData} size="small">
                <RefreshIcon />
              </IconButton>
            </Tooltip>
            <Tooltip title="Скопировать JSON">
              <IconButton onClick={handleCopyToClipboard} size="small">
                <CopyIcon />
              </IconButton>
            </Tooltip>
            <IconButton onClick={closeDebugForm} size="small">
              <CloseIcon />
            </IconButton>
          </Box>
        </Box>
      </DialogTitle>

      <DialogContent dividers>
        <Tabs
          value={tabValue}
          onChange={(_, newValue) => setTabValue(newValue)}
        >
          <Tab icon={<SettingsIcon />} label="Настройки" />
          <Tab icon={<CodeIcon />} label="Состояние" />
          <Tab icon={<StorageIcon />} label="Данные" />
        </Tabs>

        {/* Вкладка настроек */}
        <TabPanel value={tabValue} index={0}>
          <Grid container spacing={3}>
            <Grid size={{ xs: 12, md: 6 }}>
              <Paper sx={{ p: 2 }}>
                <Typography variant="h6" gutterBottom>
                  Основные настройки
                </Typography>

                <FormControl fullWidth sx={{ mb: 2 }}>
                  <InputLabel>Текущая страница</InputLabel>
                  <Select
                    value={tempData.currentPage}
                    label="Текущая страница"
                    onChange={(e) =>
                      setTempData((prev) => ({
                        ...prev,
                        currentPage: e.target.value,
                      }))
                    }
                  >
                    {availablePages.map((page) => (
                      <MenuItem key={page} value={page}>
                        {page}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>

                <TextField
                  fullWidth
                  label="Имя пользователя"
                  value={tempData.userName}
                  onChange={(e) =>
                    setTempData((prev) => ({
                      ...prev,
                      userName: e.target.value,
                    }))
                  }
                  sx={{ mb: 2 }}
                />

                <FormControl fullWidth sx={{ mb: 2 }}>
                  <InputLabel>Тема</InputLabel>
                  <Select
                    value={tempData.theme}
                    label="Тема"
                    onChange={(e) =>
                      setTempData((prev) => ({
                        ...prev,
                        theme: e.target.value,
                      }))
                    }
                  >
                    <MenuItem value="light">Светлая</MenuItem>
                    <MenuItem value="dark">Темная</MenuItem>
                    <MenuItem value="system">Системная</MenuItem>
                  </Select>
                </FormControl>

                <FormControlLabel
                  control={
                    <Switch
                      checked={tempData.devMode}
                      onChange={(e) =>
                        setTempData((prev) => ({
                          ...prev,
                          devMode: e.target.checked,
                        }))
                      }
                    />
                  }
                  label="Режим разработчика"
                />
              </Paper>
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
              <Paper sx={{ p: 2 }}>
                <Typography variant="h6" gutterBottom>
                  Быстрые действия
                </Typography>

                <Box sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
                  <Button
                    variant="outlined"
                    onClick={toggleDevMode}
                    startIcon={<BugReportIcon />}
                  >
                    {isDevMode ? "Выключить" : "Включить"} DevMode
                  </Button>

                  <Button
                    variant="outlined"
                    onClick={() => {
                      setTempData((prev) => ({
                        ...prev,
                        userName: "Test Developer",
                        currentPage: "dev-panel",
                      }));
                    }}
                  >
                    Установить тестовые данные
                  </Button>

                  <Button
                    variant="outlined"
                    onClick={() => {
                      localStorage.clear();
                      sessionStorage.clear();
                      devLog("Storage cleared");
                    }}
                    color="warning"
                  >
                    Очистить хранилище
                  </Button>
                </Box>
              </Paper>

              <Paper sx={{ p: 2, mt: 2 }}>
                <Typography variant="h6" gutterBottom>
                  Информация
                </Typography>
                <Alert severity="info" sx={{ mb: 2 }}>
                  Используйте Shift+F12 для быстрого доступа к этой панели
                </Alert>
                <Typography variant="body2" color="text.secondary">
                  Эта панель доступна только в режиме разработчика и
                  предназначена для отладки приложения.
                </Typography>
              </Paper>
            </Grid>
          </Grid>
        </TabPanel>

        {/* Вкладка состояния */}
        <TabPanel value={tabValue} index={1}>
          <Grid container spacing={2}>
            <Grid size={{ xs: 12, md: 6 }}>
              <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                  <Typography>Состояние приложения</Typography>
                </AccordionSummary>
                <AccordionDetails>
                  <Box sx={{ fontFamily: "monospace", fontSize: "0.8rem" }}>
                    <div>
                      <strong>Текущая страница:</strong> {currentPage}
                    </div>
                    <div>
                      <strong>Режим разработчика:</strong>{" "}
                      {isDevMode ? "Включен" : "Выключен"}
                    </div>
                    <div>
                      <strong>Тема:</strong> {theme}
                    </div>
                    <div>
                      <strong>Пользователь:</strong> {userName}
                    </div>
                  </Box>
                </AccordionDetails>
              </Accordion>

              <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                  <Typography>Системная информация</Typography>
                </AccordionSummary>
                <AccordionDetails>
                  <Box sx={{ fontFamily: "monospace", fontSize: "0.8rem" }}>
                    <div>
                      <strong>User Agent:</strong> {navigator.userAgent}
                    </div>
                    <div>
                      <strong>Viewport:</strong> {window.innerWidth}x
                      {window.innerHeight}
                    </div>
                    <div>
                      <strong>URL:</strong> {window.location.href}
                    </div>
                    <div>
                      <strong>Время загрузки:</strong>{" "}
                      {debugData.appState?.timestamp}
                    </div>
                  </Box>
                </AccordionDetails>
              </Accordion>
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
              <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                  <Typography>Производительность</Typography>
                </AccordionSummary>
                <AccordionDetails>
                  <Box sx={{ fontFamily: "monospace", fontSize: "0.8rem" }}>
                    <div>
                      <strong>Использование памяти:</strong>{" "}
                      {(performance as any).memory
                        ? `${Math.round(
                            (performance as any).memory.usedJSHeapSize / 1048576
                          )}MB`
                        : "N/A"}
                    </div>
                    <div>
                      <strong>Время загрузки:</strong>{" "}
                      {Math.round(performance.now())}ms
                    </div>
                    <div>
                      <strong>Язык:</strong> {navigator.language}
                    </div>
                    <div>
                      <strong>Онлайн:</strong> {navigator.onLine ? "Да" : "Нет"}
                    </div>
                  </Box>
                </AccordionDetails>
              </Accordion>

              <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                  <Typography>Хранилище</Typography>
                </AccordionSummary>
                <AccordionDetails>
                  <Box sx={{ fontFamily: "monospace", fontSize: "0.8rem" }}>
                    <div>
                      <strong>LocalStorage:</strong> {localStorage.length} items
                    </div>
                    <div>
                      <strong>SessionStorage:</strong> {sessionStorage.length}{" "}
                      items
                    </div>
                    <div>
                      <strong>Cookies:</strong> {document.cookie.length} chars
                    </div>
                  </Box>
                </AccordionDetails>
              </Accordion>
            </Grid>
          </Grid>
        </TabPanel>

        {/* Вкладка данных */}
        <TabPanel value={tabValue} index={2}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Отладочные данные (JSON)
            </Typography>
            <Paper
              variant="outlined"
              sx={{
                p: 2,
                bgcolor: "grey.100",
                fontFamily: "monospace",
                fontSize: "0.75rem",
                maxHeight: 400,
                overflow: "auto",
                whiteSpace: "pre-wrap",
              }}
            >
              {JSON.stringify(debugData, null, 2)}
            </Paper>
          </Paper>
        </TabPanel>
      </DialogContent>

      <DialogActions>
        <Button onClick={handleReset} color="inherit">
          Сбросить
        </Button>
        <Button onClick={closeDebugForm} color="inherit">
          Отмена
        </Button>
        <Button onClick={handleSave} variant="contained">
          Применить изменения
        </Button>
      </DialogActions>
    </Dialog>
  );
};
