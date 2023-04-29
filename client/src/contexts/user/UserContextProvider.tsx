import { useApi } from '../../hooks';
import UserContext from './UserContext';
import { FC, PropsWithChildren } from 'react';

const UserContextProvider: FC<PropsWithChildren> = ({ children }) => {
  const { callApi, data, state } = useApi(undefined);

  return (
    <UserContext.Provider value={{ username: data as string, state, refreshUser: callApi }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserContextProvider;
