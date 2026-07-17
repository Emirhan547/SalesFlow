import {
  useEffect,
} from "react";

import signalRService from "@/services/signalRService";

import type { Notification } from "../types/Notification";

type Props = {
  onNotificationReceived: (
    notification: Notification
  ) => void;
};

export function useNotificationRealtime({
  onNotificationReceived,
}: Props) {

  useEffect(() => {

    function handleNotification(
      notification: Notification
    ) {

      onNotificationReceived(
        notification
      );

    }

    signalRService.on(
      "NotificationReceived",
      handleNotification
    );

    return () => {

      signalRService.off(
        "NotificationReceived",
        handleNotification
      );

    };

  }, [onNotificationReceived]);
}