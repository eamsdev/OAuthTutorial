import { ApiStatus } from '../../hooks';
import { createContext } from 'react';

type UserContextState = {
  username?: string;
  state: ApiStatus;
  refreshUser: () => Promise<void>;
};

const userContext = createContext<UserContextState>({
  state: 'idle', refreshUser: () => {
    return Promise.resolve();
  }
});

export default userContext;
