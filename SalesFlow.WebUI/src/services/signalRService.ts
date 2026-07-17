import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from "@microsoft/signalr";

import { getAccessToken } from "@/features/auth/services/storageService";

class SignalRService {

  private connection: HubConnection;

  constructor() {

    this.connection =
      new HubConnectionBuilder()

        .withUrl(
          "https://localhost:7259/hubs/salesflow",
          {
            accessTokenFactory: () =>
              getAccessToken() ?? "",
          }
        )

        .withAutomaticReconnect()

        .build();

  }

  async start() {

    if (
      this.connection.state ===
      HubConnectionState.Disconnected
    ) {

      await this.connection.start();

      console.log(
        "SignalR Connected"
      );

    }

  }

  on(
    event: string,
    callback: (...args: any[]) => void
  ) {

    this.connection.on(
      event,
      callback
    );

  }

  off(
    event: string,
    callback?: (...args: any[]) => void
  ) {

    if (callback) {

      this.connection.off(
        event,
        callback
      );

      return;

    }

    this.connection.off(
      event
    );

  }

}

export default new SignalRService();