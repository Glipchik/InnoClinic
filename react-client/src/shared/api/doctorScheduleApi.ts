import axios from 'axios';
import { AxiosResponse } from 'axios';
import TimeSlot from '../../entities/timeSlot';

async function GET(doctorId: string, date: Date, token: string): Promise<AxiosResponse<TimeSlot[]>> {
  const formattedDate = date.toISOString().split('T')[0];

  return await axios.get<{ data: TimeSlot[] }>(
    `${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments/Schedule?doctorId=${doctorId}&date=${formattedDate}`,
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

export { GET }