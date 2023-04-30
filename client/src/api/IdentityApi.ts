import axios, { Axios, AxiosInstance, AxiosPromise } from 'axios';

class IdentityApi {
  CURRENT_USER_ROUTE: string = 'user';
  GITHUB_SIGNIN_ROUTE: string = 'login-github';

  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: 'https://localhost:44331/identity',
      withCredentials: true,
    });
  }

  async getCurrentUser(): AxiosPromise<string> {
    return await this.client.get(`/${this.CURRENT_USER_ROUTE}`);
  }

  getGithubSigninRoute(): string {
    return `${this.client.defaults.baseURL}/${this.GITHUB_SIGNIN_ROUTE}`;
  }
}

export default new IdentityApi();
