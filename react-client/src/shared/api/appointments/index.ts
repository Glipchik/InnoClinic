import CreateAppointmentModel from "../../../features/create-appointment-form/models/createAppointmentModel";
import tokenInterceptor from "../interceptors/tokenInterceptor";
import AppointmentModel from "./models/appointmentModel";
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

const GETAll = async () => {
  return await axiosInstance.get<{ data: AppointmentModel[] }>('');
}

const GETById = async (id: string) => {
  return await axiosInstance.get<{ data: AppointmentModel }>(`/${id}`);
}

const POST = async (createAppointmentModel?: CreateAppointmentModel) => {
  return await axiosInstance.post<CreateAppointmentModel>('', createAppointmentModel);
}

export { GETAll, GETById, POST }