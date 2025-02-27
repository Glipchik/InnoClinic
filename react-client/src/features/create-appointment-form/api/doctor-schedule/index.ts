import TimeSlot from "@entities/timeSlot";
import appointmentsAxiosInstance from "@shared/api/clients/appointments";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

appointmentsAxiosInstance.interceptors.request.use(tokenInterceptor)

const timeSlotsApi = {
  get: async (doctorId?: string, date?: string) => {
    return await appointmentsAxiosInstance.get<{ data: TimeSlot[] }>('', { params: {
      doctorId,
      date
    }});
  }
}

export default timeSlotsApi