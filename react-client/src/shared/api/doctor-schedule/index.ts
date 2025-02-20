import TimeSlot from "../../../entities/timeSlot";
import tokenInterceptor from "../interceptors/tokenInterceptor";

const axiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments/Schedule`,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(tokenInterceptor)

const GET = async (doctorId: string | undefined, date: Date | undefined) => {
  const formattedDate = date!.toISOString().split('T')[0];

  return await axios.get<{ data: TimeSlot[] }>(`?doctorId=${doctorId}&date=${formattedDate}`);
}

export { GET }