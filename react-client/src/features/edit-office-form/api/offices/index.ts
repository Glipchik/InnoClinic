import { EditOfficeModel } from "@features/edit-office-form/models/editOfficeModel";
import OfficeModel from "@models/offices/OfficeModel";
import officesAxiosInstance from "@shared/api/clients/offices";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

officesAxiosInstance.interceptors.request.use(tokenInterceptor)

const officesApi = {
  edit: async (editOfficeModel: EditOfficeModel) => {
    return await officesAxiosInstance.put<EditOfficeModel[]>('Offices', editOfficeModel);
  },
  getById: async (officeId?: string) => {
    return await officesAxiosInstance.get<OfficeModel>(`Offices/${officeId}`);
  },
}

export default officesApi