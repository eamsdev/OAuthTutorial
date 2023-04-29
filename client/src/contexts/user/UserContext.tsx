import { ApiStatus } from '../../hooks';
import { createContext } from 'react';

type UserContextState = {
  username?: string;
  state: ApiStatus;
  refreshUser: () => void;
};

const userContext = createContext<UserContextState>({ state: 'idle', refreshUser: () => {} });

export default userContext;
