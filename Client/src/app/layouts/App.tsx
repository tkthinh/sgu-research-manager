import { ReactRouterAppProvider } from '@toolpad/core/react-router';
import { NAVIGATION } from './Navigation';
import { Outlet } from 'react-router-dom';
import { useMemo, useState } from 'react';
import { Session } from '@toolpad/core';

const BRANDING = {
  logo: <img src='./logo.png' alt='SGU'/>,
  title: 'SGU - NCKH',
}

export default function App() {
  const [session, setSession] = useState<Session | null>({
    user: {
      name: 'Nguyễn Văn A',
      email: 'test@sgu.edu.vn',
    },
  });

  const authentication = useMemo(() => {
    return {
      signIn: () => {
        setSession({
          user: {
            name: 'Nguyễn Văn A',
            email: 'test@sgu.edu.vn',
          },
        });
      },
      signOut: () => {
        setSession(null);
      },
    };
  }, []);

  return (
    <ReactRouterAppProvider navigation={NAVIGATION} branding={BRANDING} session={session} authentication={authentication}>
      <Outlet />
    </ReactRouterAppProvider>
  );
}
