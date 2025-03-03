import PaginatedList from "@models/paginatedList";
import SpecializationModel from "@models/specializations/specializationModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const specializationsApi = {
  getAll: async (pageIndex?: number, pageSize?: number) => {
    return await servicesAxiosInstance.get<PaginatedList<SpecializationModel>>('Specializations/with-pagination', { params: {
      pageIndex,
      pageSize
    }});
  },
  getById: async (specializationId?: string) => {
    return await servicesAxiosInstance.get<SpecializationModel>(`Specializations/${specializationId}`);
  },
  delete: async (id: string) => {
    return await servicesAxiosInstance.delete(`Specializations/${id}`);
  },
}

export default specializationsApi