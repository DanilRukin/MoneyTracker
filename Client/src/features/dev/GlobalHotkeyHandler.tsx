import { useDevMode } from "../../contexts";
import { useDevTools } from "../../contexts/DevToolsContext";
import { useShiftF12 } from "../../hooks/useShiftF12";

export const GlobalHotkeyHandler: React.FC = () => {
  const { isDevMode } = useDevMode();
  const { openDebugForm } = useDevTools();

  useShiftF12(() => {
    if (isDevMode) {
      openDebugForm();
    }
  }, [isDevMode, openDebugForm]);

  return null; // Этот компонент не рендерит ничего
};
