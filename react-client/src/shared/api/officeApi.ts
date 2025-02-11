import { AxiosResponse } from 'axios';
import CreateOfficeModel from '../../features/offices/models/CreateOfficeModel';
import axios from 'axios';
import Office from '../../entities/office';
import PaginatedList from '../../models/paginatedList';

async function GET(pageIndex: number | null, padeSize: number | null, token: string): Promise<AxiosResponse<PaginatedList<Office>>> {
  let url = `${import.meta.env.VITE_OFFICES_BASE_URL}/api/Offices?`

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (padeSize) {
    url += `pageSize=${padeSize}`
  }

  return await axios.get<{ data: PaginatedList<Office> }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createOfficeModel: CreateOfficeModel, token: string): Promise<AxiosResponse> {
  return await axios.post<CreateOfficeModel>(`${import.meta.env.VITE_OFFICES_BASE_URL}/api/Offices`, createOfficeModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(office: Office, token: string): Promise<AxiosResponse> {
  return await axios.put<Office>(`${import.meta.env.VITE_OFFICES_BASE_URL}/api/Offices`, office, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_OFFICES_BASE_URL}/api/Offices/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, POST, DELETE, PUT }