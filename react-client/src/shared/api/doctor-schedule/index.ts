import TimeSlot from "../../../entities/timeSlot";
import tokenInterceptor from "../interceptors/tokenInterceptor";
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments/Schedule`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

export const timeSlotsApi = {
  GET: async (doctorId?: string, date?: Date) => {
    const formattedDate = date!.toISOString().split('T')[0];

    return await axiosInstance.get<{ data: TimeSlot[] }>(`?doctorId=${doctorId}&date=${formattedDate}`);
  }
}

export default timeSlotsApi