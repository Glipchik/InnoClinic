import axios from 'axios';
import Specialization from '../../entities/specialization';
import { AxiosResponse } from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_SERVICES_BASE_URL,
  withCredentials: true,
});

async function GET(id: string | null, accessToken: string): Promise<AxiosResponse<Specialization | Specialization[]>> {
  console.log(`accessToken - ${accessToken}`)
  const config = {
    headers: {
      Authorization: `Bearer ${accessToken}`
    },
  };

  if (id === null) {
    return await api.get<{ data: Specialization[] }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations`, config);
  } else {
    return await api.get<{ data: Specialization }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations/${id}`, config);
  }
}

export { GET }