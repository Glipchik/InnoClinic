import { AxiosResponse } from 'axios';
import Doctor from '../../entities/doctor';
import axios from 'axios';

async function GET(id: string | null, specializationId: string | null, token: string): Promise<AxiosResponse<Doctor | Doctor[]>> {
  if (id === null) {
    return await axios.get<{ data: Doctor[] }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors?SpecializationId=${specializationId}`, { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Doctor }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  }
}

export { GET }