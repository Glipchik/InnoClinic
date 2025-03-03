import OfficeModel from "@models/offices/OfficeModel";
import PaginatedList from "@models/paginatedList";
import officesAxiosInstance from "@shared/api/clients/offices";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

officesAxiosInstance.interceptors.request.use(tokenInterceptor)

const officesApi = {
  getAll: async (pageIndex?: number, pageSize?: number) => {
    return await officesAxiosInstance.get<PaginatedList<OfficeModel>>('Offices', { params: {
      pageIndex,
      pageSize
    }});
  },
  getById: async (officeId?: string) => {
    return await officesAxiosInstance.get<OfficeModel>(`Offices/${officeId}`);
  },
  delete: async (id: string) => {
    return await servicesAxiosInstance.delete(`Offices/${id}`);
  },
}

export default officesApi