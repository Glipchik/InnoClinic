import axios from "axios";

const officesAxiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_OFFICES_BASE_URL}/api/`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default officesAxiosInstance