import ServiceModel from "@models/services/serviceModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const servicesApi = {
  getById: async (serviceId?: string) => {
    return await servicesAxiosInstance.get<ServiceModel>(`Services/${serviceId}`);
  },
  delete: async (id: string) => {
    return await servicesAxiosInstance.delete(`Services/${id}`);
  },
}

export default servicesApi