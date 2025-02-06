import axios from 'axios';
import Specialization from '../../entities/specialization';
import { AxiosResponse } from 'axios';
import Service from '../../entities/service';

const api = axios.create({
  baseURL: import.meta.env.VITE_SERVICES_BASE_URL,
  withCredentials: true,
});

async function GET(id: string | null, specializationId: string | null): Promise<AxiosResponse<Specialization | Specialization[]>> {

  if (id === null) {
    return await api.get<{ data: Service[] }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services?SpecializationId=${specializationId}`);
  } else {
    return await api.get<{ data: Service }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services/${id}`);
  }
}

export { GET }