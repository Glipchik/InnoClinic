import CreateSpecializationModel from "@features/create-specialization-form/models/createSpecializationModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const specializationsApi = {
  post: async (createSpecializationModel: CreateSpecializationModel) => {
    return await servicesAxiosInstance.post<CreateSpecializationModel>('Specializations', createSpecializationModel);
  }
}

export default specializationsApi