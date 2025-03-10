import SpecializationModel from "@shared/models/specializations/specializationModel";
import servicesAxiosInstance from "../clients/services";
import tokenInterceptor from "../interceptors/tokenInterceptor";
import PaginatedList from "@models/paginatedList";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const specializationsApi = {
  getAll: async () => {
    return await servicesAxiosInstance.get<SpecializationModel[]>('Specializations');
  },
  getAllWithPagination: async (pageIndex?: number, pageSize?: number) => {
    return await servicesAxiosInstance.get<PaginatedList<SpecializationModel>>('Specializations/with-pagination', { params: {
      pageIndex: pageIndex ?? 1,
      pageSize: pageSize ?? import.meta.env.VITE_PAGE_SIZE
    }});
  },
  getById: async (id: string) => {
    return await servicesAxiosInstance.get<SpecializationModel[]>(`Specializations/${id}`);
  }
}

export default specializationsApi