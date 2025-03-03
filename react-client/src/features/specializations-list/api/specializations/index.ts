import SpecializationModel from "@models/specializations/specializationModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const specializationsApi = {
  getById: async (specializationId?: string) => {
    return await servicesAxiosInstance.get<SpecializationModel>(`Specializations/${specializationId}`);
  },
  delete: async (id: string) => {
    return await servicesAxiosInstance.delete(`Specializations/${id}`);
  },
}

export default specializationsApi