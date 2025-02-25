import tokenInterceptor from "../interceptors/tokenInterceptor";
import DoctorModel from "./models/doctorModel";
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

export const doctorsApi = {
  GETAll: async (specializationId?: string) => {
    let url = '?'
    if (specializationId) {
      url += `specializationId=${specializationId}`;
    }
    return await axiosInstance.get<{ data: DoctorModel[] }>(url);
  },
  GETById: async (id: string) => {
    return await axiosInstance.get<{ data: DoctorModel }>(`/${id}`);
  }
}

export default doctorsApi