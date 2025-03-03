import officesAxiosInstance from "@shared/api/clients/offices";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

officesAxiosInstance.interceptors.request.use(tokenInterceptor)

const officesApi = {
  delete: async (id: string) => {
    return await officesAxiosInstance.delete(`Offices/${id}`);
  },
}

export default officesApi