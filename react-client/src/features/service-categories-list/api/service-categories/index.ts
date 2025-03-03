import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const serviceCategoriesApi = {
  delete: async (id: string) => {
    return await servicesAxiosInstance.delete(`ServiceCategories/${id}`);
  },
}

export default serviceCategoriesApi