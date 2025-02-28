import { EditOfficeModel } from "@features/edit-office-form/models/editOfficeModel";
import officesAxiosInstance from "@shared/api/clients/offices";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

officesAxiosInstance.interceptors.request.use(tokenInterceptor)

const officesApi = {
  edit: async (editOfficeModel: EditOfficeModel) => {
    return await officesAxiosInstance.put<EditOfficeModel[]>('Offices', editOfficeModel);
  }
}

export default officesApi