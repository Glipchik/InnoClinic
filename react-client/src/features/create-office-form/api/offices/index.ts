import { CreateOfficeModel } from "@features/create-office-form/models/createOfficeModel";
import officesAxiosInstance from "@shared/api/clients/offices";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

officesAxiosInstance.interceptors.request.use(tokenInterceptor)

const officesApi = {
  post: async (createOfficeModel: CreateOfficeModel) => {
    return await officesAxiosInstance.post<CreateOfficeModel>('Offices', createOfficeModel);
  }
}

export default officesApi