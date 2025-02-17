import axios from 'axios';
import Specialization from '../../entities/specialization';
import { AxiosResponse } from 'axios';
import CreateSpecializationModel from '../../models/specializations/createSpecializationModel';
import PaginatedList from '../../models/paginatedList';
import EditSpecializationModel from '../../models/specializations/editSpecializationModel';

async function GET(id: string | null, token: string): Promise<AxiosResponse<Specialization | Specialization[]>> {

  if (id === null) {
    return await axios.get<{ data: Specialization[] }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations`,  { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Specialization }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations/${id}`,  { headers: { Authorization: `Bearer ${token}` } });
  }
}

async function GETWithPagination(pageIndex: number | null, pageSize: number | null, token: string): Promise<AxiosResponse<PaginatedList<Specialization>>> {
  let url = `${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations/with-pagination?`

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (pageSize) {
    url += `pageSize=${pageSize}`
  }

  return await axios.get<{ data: PaginatedList<Specialization> }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createSpecializationModel: CreateSpecializationModel, token: string): Promise<AxiosResponse> {
  return await axios.post<CreateSpecializationModel>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations`, createSpecializationModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(editSpecializationModel: EditSpecializationModel, token: string): Promise<AxiosResponse> {
  return await axios.put<EditSpecializationModel>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations`, editSpecializationModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Specializations/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, POST, DELETE, PUT, GETWithPagination }