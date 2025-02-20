import Specialization from "../../../entities/specialization";
import tokenInterceptor from "../interceptors/tokenInterceptor";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

const GETAll = async () => {
  return await axios.get<{ data: Specialization[] }>('');
}

const GETById = async (id: string) => {
  return await axios.get<{ data: Specialization }>(`/${id}`);
}

export { GETAll, GETById }