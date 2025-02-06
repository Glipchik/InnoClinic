import axios from 'axios';
import { AxiosResponse } from 'axios';
import TimeSlot from '../../entities/timeSlot';

const api = axios.create({
  baseURL: import.meta.env.VITE_APPOINTMENTS_BASE_URL,
  withCredentials: true,
});

async function GET(doctorId: string, date: Date): Promise<AxiosResponse<TimeSlot[]>> {
  const formattedDate = date.toISOString().split('T')[0];

  return await api.get<{ data: TimeSlot[] }>(
    `${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments/Schedule?doctorId=${doctorId}&date=${formattedDate}`
  );
}

export { GET }