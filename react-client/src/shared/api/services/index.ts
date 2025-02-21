import tokenInterceptor from "../interceptors/tokenInterceptor";
import ServiceModel from "./models/serviceModel";
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

const servicesApi = {
  GETAll: async (specializationId?: string) => {
    let url = ''
    if (specializationId) {
      url += specializationId;
    }
    return await axiosInstance.get<{ data: ServiceModel[] }>(url);
  },
  GETById: async (id: string) => {
    return await axiosInstance.get<{ data: ServiceModel }>(`/${id}`);
  }
}

export default servicesApi