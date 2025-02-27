import axios from "axios";

const appointmentsAxiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/`,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default appointmentsAxiosInstance