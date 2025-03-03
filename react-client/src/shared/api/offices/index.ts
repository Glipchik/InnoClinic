import OfficeModel from "@models/offices/OfficeModel";
import PaginatedList from "@models/paginatedList";
import officesAxiosInstance from "@shared/api/clients/offices";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

officesAxiosInstance.interceptors.request.use(tokenInterceptor)

const officesApi = {
  getAll: async (pageIndex?: number, pageSize?: number) => {
    return await officesAxiosInstance.get<PaginatedList<OfficeModel>>('Offices', { params: {
      pageIndex: pageIndex ?? 1,
      pageSize: pageSize ?? 2
    }});
  }
}

export default officesApi