import tokenInterceptor from "../interceptors/tokenInterceptor";
import SpecializationModel from "./models/specializationModel";
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

const specializationsApi = {
  GETAll: async () => {
    return await axiosInstance.get<{ data: SpecializationModel[] }>('');
  },
  GETById: async (id: string) => {
    return await axiosInstance.get<{ data: SpecializationModel }>(`/${id}`);
  }
}

export default specializationsApi