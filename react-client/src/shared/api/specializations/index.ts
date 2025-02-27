import SpecializationModel from "@models/specializations/specializationModel";
import servicesAxiosInstance from "../clients/services";
import tokenInterceptor from "../interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const specializationsApi = {
  getAll: async () => {
    return await servicesAxiosInstance.get<{ data: SpecializationModel[] }>('');
  },
  getById: async (id: string) => {
    return await servicesAxiosInstance.get<{ data: SpecializationModel }>(`/${id}`);
  }
}

export default specializationsApi