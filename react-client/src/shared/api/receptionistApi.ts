import { AxiosResponse } from 'axios';
import Receptionist from '../../entities/receptionist';
import axios from 'axios';
import PaginatedList from '../../models/paginatedList';
import ReceptionistModel from '../../models/receptionists/ReceptionistModel';
import CreateReceptionistModel from '../../models/receptionists/CreateReceptionistModel';

async function GET(id: string | null, token: string): Promise<AxiosResponse<Receptionist | Receptionist[]>> {
  if (id === null) {
    return await axios.get<{ data: Receptionist[] }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists`, { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Receptionist }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  }
}

async function GETWithPagination(officeId: string | null, pageIndex: number | null, pageSize: number | null, token: string): Promise<AxiosResponse<PaginatedList<ReceptionistModel>>> {
  let url = `${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists/with-pagination?`

  if (officeId) {
    url += `officeId=${officeId}&`
  }

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (pageSize) {
    url += `pageSize=${pageSize}`
  }

  return await axios.get<{ data: ReceptionistModel }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createReceptionistModel: CreateReceptionistModel, token: string): Promise<AxiosResponse> {
  return await axios.post<CreateReceptionistModel>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists`, createReceptionistModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(receptionist: Receptionist, token: string): Promise<AxiosResponse> {
  return await axios.put<Receptionist>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists/as-receptionist`, receptionist, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, GETWithPagination, POST, PUT, DELETE }