import axios from "axios";

const profilesAxiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_PROFILES_BASE_URL}/api/`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default profilesAxiosInstance