import SpecializationModel from "@shared/models/specializations/specializationModel";
import servicesAxiosInstance from "../clients/services";
import tokenInterceptor from "../interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const specializationsApi = {
  getAll: async () => {
    return await servicesAxiosInstance.get<SpecializationModel[]>('Specializations');
  },
  getById: async (id: string) => {
    return await servicesAxiosInstance.get<SpecializationModel[]>(`Specializations/${id}`);
  }
}

export default specializationsApi