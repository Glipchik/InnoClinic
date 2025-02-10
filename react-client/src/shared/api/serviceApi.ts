import axios from 'axios';
import Specialization from '../../entities/specialization';
import { AxiosResponse } from 'axios';
import Service from '../../entities/service';

async function GET(id: string | null, specializationId: string | null, token: string): Promise<AxiosResponse<Specialization | Specialization[]>> {
  if (id === null) {
    return await axios.get<{ data: Service[] }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services?SpecializationId=${specializationId}`, { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Service }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  }
}

export { GET }