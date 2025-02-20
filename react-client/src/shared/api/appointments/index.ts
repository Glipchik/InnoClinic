import Appointment from "../../../entities/appointment";
import tokenInterceptor from "../interceptors/tokenInterceptor";
import CreateAppointmentModel from "./models/createAppointmentModel";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

const GETAll = async () => {
  return await axios.get<{ data: Appointment[] }>('');
}

const GETById = async (id: string) => {
  return await axios.get<{ data: Appointment }>(`/${id}`);
}

const POST = async (createAppointmentModel: CreateAppointmentModel) => {
  return await axios.post<CreateAppointmentModel>('', createAppointmentModel);
}

export { GETAll, GETById, POST }