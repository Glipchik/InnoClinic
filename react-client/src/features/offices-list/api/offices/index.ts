import CreateOfficeModel from "@models/offices/CreateOfficeModel";
import { EditOfficeModel } from "@models/offices/EditOfficeModel";
import OfficeModel from "@models/offices/OfficeModel";
import PaginatedList from "@models/paginatedList";
import officesAxiosInstance from "@shared/api/clients/offices";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

officesAxiosInstance.interceptors.request.use(tokenInterceptor)

const officesApi = {
  getAll: async (pageIndex?: number, pageSize?: number) => {
    return await officesAxiosInstance.get<{ data: PaginatedList<OfficeModel> }>('Offices', { params: {
      pageIndex,
      pageSize
    }});
  },
  post: async (createOfficeModel: CreateOfficeModel) => {
    return await servicesAxiosInstance.post<CreateOfficeModel>('Offices', createOfficeModel);
  },
  edit: async (editOfficeModel: EditOfficeModel) => {
    return await servicesAxiosInstance.put<EditOfficeModel>('Offices', editOfficeModel);
  },
  delete: async (id: string) => {
    return await servicesAxiosInstance.delete(`Offices/${id}`);
  },
}

export default officesApi