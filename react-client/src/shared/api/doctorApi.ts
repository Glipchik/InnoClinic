import { AxiosResponse } from 'axios';
import Doctor from '../../entities/doctor';
import axios from 'axios';
import PaginatedList from '../../models/paginatedList';
import DoctorModel from '../../models/doctors/DoctorModel';
import CreateDoctorModel from '../../models/doctors/CreateDoctorModel';
import UpdateDoctorModelByDoctor from '../../models/doctors/UpdateDoctorModelByDoctor';

async function GET(id: string | null, specializationId: string | null, token: string): Promise<AxiosResponse<Doctor | Doctor[]>> {
  if (id === null) {
    return await axios.get<{ data: Doctor[] }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors?SpecializationId=${specializationId}`, { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Doctor }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  }
}

async function GETWithPagination(specializationId: string | null, officeId: string | null, pageIndex: number, pageSize: number, token: string): Promise<AxiosResponse<PaginatedList<DoctorModel>>> {
  let url = `${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/with-pagination?`

  if (specializationId) {
    url += `specializationId=${specializationId}`
  }

  if (officeId) {
    url += `officeId=${officeId}`
  }

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (pageSize) {
    url += `pageSize=${pageSize}`
  }

  return await axios.get<{ data: DoctorModel }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createDoctorModel: CreateDoctorModel, token: string): Promise<AxiosResponse> {
  return await axios.post<CreateDoctorModel>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors`, createDoctorModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUTAsDoctor(updateDoctorModelByDoctor: UpdateDoctorModelByDoctor, token: string): Promise<AxiosResponse> {
  return await axios.put<Doctor>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors`, updateDoctorModelByDoctor, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(doctor: Doctor, token: string): Promise<AxiosResponse> {
  return await axios.put<Doctor>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/as-receptionist`, doctor, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, GETWithPagination, POST, PUT, PUTAsDoctor, DELETE }