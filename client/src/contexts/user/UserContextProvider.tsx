import { IdentityApi } from '../../api';
import { useApi } from '../../hooks';
import UserContext from './UserContext';
import { FC, PropsWithChildren } from 'react';

const UserContextProvider: FC<PropsWithChildren> = ({ children }) => {
  const { callApi, data, state } = useApi(() => IdentityApi.getCurrentUser());
  
  return (
    <UserContext.Provider value={{ username: data, state, refreshUser: callApi }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserContextProvider;
