import Service from "../../../entities/service";
import tokenInterceptor from "../interceptors/tokenInterceptor";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

const GETAll = async (specializationId?: string) => {
  let url = ''
  if (specializationId) {
    url += specializationId;
  }
  return await axios.get<{ data: Service[] }>(url);
}

const GETById = async (id: string) => {
  return await axios.get<{ data: Service }>(`/${id}`);
}

export { GETAll, GETById }