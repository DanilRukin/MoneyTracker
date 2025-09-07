/* eslint-disable @typescript-eslint/no-explicit-any */
import { createTheme, type PaletteMode } from "@mui/material";

// Базовые цвета для нашей дизайн-системы
export const colorTokens = {
  primary: {
    50: "#e8f5e9",
    100: "#c8e6c9",
    200: "#a5d6a7",
    300: "#81c784",
    400: "#66bb6a", // Основной нежно-зеленый
    500: "#4caf50",
    600: "#43a047",
    700: "#388e3c",
    800: "#2e7d32",
    900: "#1b5e20",
  },
  grey: {
    50: "#fafafa",
    100: "#f5f5f5",
    200: "#eeeeee",
    300: "#e0e0e0",
    400: "#bdbdbd",
    500: "#9e9e9e",
    600: "#757575",
    700: "#616161",
    800: "#424242",
    900: "#212121",
  },
  success: {
    main: "#2e7d32",
    light: "#4caf50",
    dark: "#1b5e20",
  },
  error: {
    main: "#d32f2f",
    light: "#ef5350",
    dark: "#c62828",
  },
  warning: {
    main: "#ed6c02",
    light: "#ff9800",
    dark: "#e65100",
  },
  info: {
    main: "#0288d1",
    light: "#03a9f4",
    dark: "#01579b",
  },
};

// Функция для создания темы
export const createAppTheme = (mode: PaletteMode) => {
  return createTheme({
    palette: {
      mode,
      // Основные цвета
      primary: {
        main: colorTokens.primary[400], // Нежно-зеленый
        light: colorTokens.primary[300],
        dark: colorTokens.primary[600],
        contrastText: "#ffffff",
      },
      secondary: {
        main: mode === "light" ? colorTokens.grey[800] : colorTokens.grey[200],
        contrastText: mode === "light" ? "#ffffff" : "#000000",
      },
      background: {
        default: mode === "light" ? "#F8F9FA" : "#121212",
        paper: mode === "light" ? "#ffffff" : "#1e1e1e",
      },
      text: {
        primary:
          mode === "light" ? colorTokens.grey[900] : colorTokens.grey[50],
        secondary:
          mode === "light" ? colorTokens.grey[700] : colorTokens.grey[400],
      },
      // Статусные цвета
      success: colorTokens.success,
      error: colorTokens.error,
      warning: colorTokens.warning,
      info: colorTokens.info,
      // Divider
      divider:
        mode === "light" ? "rgba(0, 0, 0, 0.12)" : "rgba(255, 255, 255, 0.12)",
    },
    // Кастомные тени для карточек
    shadows: [
      "none",
      mode === "light"
        ? "0px 2px 4px rgba(0, 0, 0, 0.1)"
        : "0px 2px 4px rgba(255, 255, 255, 0.1)",
      mode === "light"
        ? "0px 4px 8px rgba(0, 0, 0, 0.1)"
        : "0px 4px 8px rgba(255, 255, 255, 0.1)",
      ...Array(22).fill("none"),
    ] as any,
    // Типографика
    typography: {
      fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
      h1: {
        fontSize: "2.5rem",
        fontWeight: 600,
      },
      h2: {
        fontSize: "2rem",
        fontWeight: 600,
      },
      h3: {
        fontSize: "1.75rem",
        fontWeight: 600,
      },
      h4: {
        fontSize: "1.5rem",
        fontWeight: 600,
      },
      h5: {
        fontSize: "1.25rem",
        fontWeight: 500,
      },
      h6: {
        fontSize: "1.125rem",
        fontWeight: 500,
      },
      body1: {
        fontSize: "1rem",
        lineHeight: 1.5,
      },
      body2: {
        fontSize: "0.875rem",
        lineHeight: 1.43,
      },
    },
    // Компоненты
    components: {
      MuiButton: {
        styleOverrides: {
          root: {
            borderRadius: 8,
            textTransform: "none",
            fontWeight: 500,
          },
        },
      },
      MuiCard: {
        styleOverrides: {
          root: {
            borderRadius: 12,
            boxShadow:
              mode === "light"
                ? "0px 2px 8px rgba(0, 0, 0, 0.1)"
                : "0px 2px 8px rgba(255, 255, 255, 0.1)",
          },
        },
      },
      MuiPaper: {
        styleOverrides: {
          root: {
            backgroundImage: "none", // Убираем градиент
          },
        },
      },
      MuiAppBar: {
        styleOverrides: {
          root: {
            backgroundImage: "none",
            boxShadow: "none",
            borderBottom: `1px solid ${
              mode === "light"
                ? "rgba(0, 0, 0, 0.12)"
                : "rgba(255, 255, 255, 0.12)"
            }`,
          },
        },
      },
    },
  });
};

// Тип темы
export type AppTheme = ReturnType<typeof createAppTheme>;
