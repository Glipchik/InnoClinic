import ServiceModel from "@models/services/serviceModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const servicesApi = {
  getAll: async (specializationId?: string) => {
    return await servicesAxiosInstance.get<{ data: ServiceModel[] }>('', { params: {
      specializationId
    }});
  },
  getById: async (id: string) => {
    return await servicesAxiosInstance.get<{ data: ServiceModel }>(`/${id}`);
  }
}

export default servicesApi