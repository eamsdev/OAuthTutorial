import { AxiosPromise } from 'axios';
import { useCallback, useState } from 'react';

type ApiError = {
  message: string;
  statusCode?: number;
  request: any;
};

type ApiStatus = 'loading' | 'fulfilled' | 'errored' | 'idle';

const useApi = <BodyType, ReturnType>(
  apiFunction: (body?: BodyType) => AxiosPromise<ReturnType>,
  onSuccess?: (response: ReturnType) => void,
  onError?: (error: ApiError) => void,
) => {
  const [state, setState] = useState<ApiStatus>('idle');
  const [data, setData] = useState<ReturnType | undefined>();
  const [error, setError] = useState<ApiError | undefined>();

  const callApi = useCallback(
    async (body?: BodyType) => {
      if (state === 'loading') return;

      try {
        const response = await apiFunction(body);

        setData(response.data);
        setState('fulfilled');
        onSuccess && onSuccess(response.data);
      } catch (e: any) {
        const apiError: ApiError = {
          message: e.message,
          statusCode: e?.response?.status,
          request: body,
        };

        setError(apiError);
        setState('errored');
        onError && onError(apiError);
      }
    },
    [apiFunction, onSuccess, onError, state],
  );

  return { state, data, error, callApi };
};

export default useApi;
export { ApiStatus };
