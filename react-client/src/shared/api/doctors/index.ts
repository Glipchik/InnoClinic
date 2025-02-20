import Doctor from "../../../entities/doctor";
import tokenInterceptor from "../interceptors/tokenInterceptor";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors`,
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
  return await axios.get<{ data: Doctor[] }>(url);
}

const GETById = async (id: string) => {
  return await axios.get<{ data: Doctor }>(`/${id}`);
}

export { GETAll, GETById }