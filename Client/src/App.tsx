import { Layout } from "./components/shared/ui/Layout";
import { PlaceholderPage } from "./components/shared/ui/PlaceholderPage";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import { AppProvider, useApp } from "./contexts/AppContext"; // Импортируем провайдер и хук
import { createAppTheme } from "./theme";
import {
  NavigationIds,
  PagesDescriptions,
  PagesTitles,
} from "./infrastructure/constants";

function AppContent() {
  const { currentPage, theme } = useApp();
  const appTheme = createAppTheme(theme);
  const renderPage = () => {
    switch (currentPage) {
      case NavigationIds.HOME:
        return (
          <PlaceholderPage
            title={PagesTitles.HOME}
            description={PagesDescriptions.HOME}
          />
        );
      case NavigationIds.TRANSACTIONS:
        return (
          <PlaceholderPage
            title={PagesTitles.TRANSACTIONS}
            description={PagesDescriptions.TRANSACTIONS}
          />
        );
      case NavigationIds.ACCOUNTS:
        return (
          <PlaceholderPage
            title={PagesTitles.ACCOUNTS}
            description={PagesDescriptions.ACCOUNTS}
          />
        );
      case NavigationIds.BUDGET:
        return (
          <PlaceholderPage
            title={PagesTitles.BUDGET}
            description={PagesDescriptions.BUDGET}
          />
        );
      case NavigationIds.GOALS:
        return (
          <PlaceholderPage
            title={PagesTitles.GOALS}
            description={PagesDescriptions.GOALS}
          />
        );
      case NavigationIds.INVEST:
        return (
          <PlaceholderPage
            title={PagesTitles.INVEST}
            description={PagesDescriptions.INVEST}
          />
        );
      case NavigationIds.CALCULATORS:
        return (
          <PlaceholderPage
            title={PagesTitles.CALCULATORS}
            description={PagesDescriptions.CALCULATORS}
          />
        );
      case NavigationIds.LEARNING:
        return (
          <PlaceholderPage
            title={PagesTitles.LEARNING}
            description={PagesDescriptions.LEARNING}
          />
        );
      case NavigationIds.DEV_PANEL:
        return (
          <PlaceholderPage
            title={PagesTitles.DEV_PANEL}
            description={PagesDescriptions.DEV_PANEL}
          />
        );
      case NavigationIds.DEV_LOGS:
        return (
          <PlaceholderPage
            title={PagesTitles.DEV_LOGS}
            description={PagesDescriptions.DEV_LOGS}
          />
        );
      case NavigationIds.DEV_FLAGS:
        return (
          <PlaceholderPage
            title={PagesTitles.DEV_FLAGS}
            description={PagesDescriptions.DEV_FLAGS}
          />
        );
      default:
        return (
          <PlaceholderPage
            title={PagesTitles.DEFAULT}
            description={PagesDescriptions.DEFAULT}
          />
        );
    }
  };
  return (
    <ThemeProvider theme={appTheme}>
      <CssBaseline />
      <Layout>{renderPage()}</Layout>
    </ThemeProvider>
  );
}

function App() {
  return (
    <AppProvider>
      <AppContent />
    </AppProvider>
  );
}

export default App;
