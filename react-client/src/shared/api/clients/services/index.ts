import axios from "axios";

const servicesAxiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_SERVICES_BASE_URL}/api/Doctors`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default servicesAxiosInstance