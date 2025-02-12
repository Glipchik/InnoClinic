import { AxiosResponse } from 'axios';
import axios from 'axios';

async function GETProfilePictureUrl(accountId : string, token : string) {
  return await axios.get<string>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Photos/${accountId}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GETProfilePictureUrl }