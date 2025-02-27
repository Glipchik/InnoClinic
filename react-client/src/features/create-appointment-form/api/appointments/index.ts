import AppointmentModel from "@features/create-appointment-form/models/appointmentModel";
import CreateAppointmentModel from "@features/create-appointment-form/models/createAppointmentModel";
import appointmentsAxiosInstance from "@shared/api/clients/appointments";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

appointmentsAxiosInstance.interceptors.request.use(tokenInterceptor)

const appointmentsApi = {
  getAll: async () => {
    return await appointmentsAxiosInstance.get<{ data: AppointmentModel[] }>('');
  },
  getById: async (id: string) => {
    return await appointmentsAxiosInstance.get<{ data: AppointmentModel }>(`/${id}`);
  },
  post: async (createAppointmentModel?: CreateAppointmentModel) => {
    return await appointmentsAxiosInstance.post<CreateAppointmentModel>('', createAppointmentModel);
  }
}

export default appointmentsApi