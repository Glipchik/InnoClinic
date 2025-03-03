import EditServiceModel from "@features/edit-service-form/models/EditServiceModel";
import ServiceModel from "@models/services/serviceModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const servicesApi = {
  edit: async (editServiceModel: EditServiceModel) => {
    return await servicesAxiosInstance.put<EditServiceModel[]>('Services', editServiceModel);
  },
  getById: async (serviceId?: string) => {
    return await servicesAxiosInstance.get<ServiceModel>(`Services/${serviceId}`);
  },
}

export default servicesApi