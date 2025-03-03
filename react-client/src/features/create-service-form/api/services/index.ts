import CreateServiceModel from "@features/create-service-form/models/createServiceModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const servicesApi = {
  post: async (createServiceModel: CreateServiceModel) => {
    return await servicesAxiosInstance.post<CreateServiceModel>('Services', createServiceModel);
  }
}

export default servicesApi