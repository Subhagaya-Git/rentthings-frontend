import { useEffect } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { startNotificationHub, stopNotificationHub } from '@/lib/signalr';
import { useAuthStore } from '@/stores';

export function useSignalRNotifications() {
  const { token } = useAuthStore();
  const qc = useQueryClient();

  useEffect(() => {
    if (!token) {
      stopNotificationHub();
      return;
    }

    startNotificationHub(() => {
      qc.invalidateQueries({ queryKey: ['notifications'] });
    }).catch(console.error);

    return () => {
      stopNotificationHub();
    };
  }, [token, qc]);
}
