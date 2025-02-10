import { AxiosResponse } from 'axios';
import CreateAppointmentModel from '../../features/appointments/models/CreateAppointmentModel';
import axios from 'axios';
import Appointment from '../../entities/appointment';

async function GET(id: string | null, appointmentId: string | null, token: string): Promise<AxiosResponse<Appointment | Appointment[]>> {
  if (id === null) {
    return await axios.get<{ data: Appointment[] }>(`${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments?AppointmentId=${appointmentId}`, { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Appointment }>(`${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  }
}

async function POST(createAppointmentModel: CreateAppointmentModel, token: string): Promise<AxiosResponse> {
  return await axios.post<CreateAppointmentModel>(`${import.meta.env.VITE_APPOINTMENTS_BASE_URL}/api/Appointments`, createAppointmentModel, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, POST }