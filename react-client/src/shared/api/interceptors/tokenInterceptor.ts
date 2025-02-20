const tokenInterceptor = (config: Axios.AxiosXHRConfig<unknown>) => {
  const accessToken = localStorage.getItem('accessToken');
  config.headers = config.headers || {};
  config.headers['Authorization'] = 'Bearer ' + accessToken;
  return config
}

export default tokenInterceptor;
