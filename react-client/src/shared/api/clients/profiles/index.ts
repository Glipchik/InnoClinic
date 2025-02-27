import axios from "axios";

const profilesAxiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default profilesAxiosInstance