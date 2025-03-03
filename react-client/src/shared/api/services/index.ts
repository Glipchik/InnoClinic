import ServiceModel from "@models/services/serviceModel";
import servicesAxiosInstance from "../clients/services";
import tokenInterceptor from "../interceptors/tokenInterceptor";
import PaginatedList from "@models/paginatedList";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const servicesApi = {
  getAll: async () => {
    return await servicesAxiosInstance.get<ServiceModel[]>('Services');
  },
  getAllWithPagination: async (pageIndex?: number, pageSize?: number) => {
    return await servicesAxiosInstance.get<PaginatedList<ServiceModel>>('Services/with-pagination', { params: {
      pageIndex: pageIndex ?? 1,
      pageSize: pageSize ?? 2
    }});
  },
  getById: async (id: string) => {
    return await servicesAxiosInstance.get<ServiceModel[]>(`Services/${id}`);
  }
}

export default servicesApi