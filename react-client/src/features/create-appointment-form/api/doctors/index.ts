import DoctorModel from "@models/doctors/doctorModel";
import profilesAxiosInstance from "@shared/api/clients/profiles";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

profilesAxiosInstance.interceptors.request.use(tokenInterceptor)

const doctorsApi = {
  getAll: async (specializationId?: string) => {
    return await profilesAxiosInstance.get<DoctorModel[]>('Doctors', { params: {
      specializationId
    }});
  },
  getById: async (id: string) => {
    return await profilesAxiosInstance.get<DoctorModel[]>(`Doctors/${id}`);
  }
}

export default doctorsApi