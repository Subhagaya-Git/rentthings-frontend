import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import type { Notification } from '@/types';

let connection: HubConnection | null = null;

export async function startNotificationHub(
  onNotification: (notification: Notification) => void,
): Promise<void> {
  const token = localStorage.getItem('rentthings_token');
  if (!token || connection) return;

  connection = new HubConnectionBuilder()
    .withUrl(`/hubs/notifications?access_token=${encodeURIComponent(token)}`)
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

  connection.on('ReceiveNotification', (notification: Notification) => {
    onNotification(notification);
  });

  await connection.start();
}

export async function stopNotificationHub(): Promise<void> {
  if (connection) {
    await connection.stop();
    connection = null;
  }
}
