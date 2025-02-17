import { AxiosResponse } from 'axios';
import CreateOfficeModel from '../../models/offices/CreateOfficeModel';
import axios from 'axios';
import Office from '../../entities/office';
import PaginatedList from '../../models/paginatedList';
import { EditOfficeModel } from '../../models/offices/EditOfficeModel';

async function GET(pageIndex: number | null, pageSize: number | null, token: string): Promise<AxiosResponse<PaginatedList<Office>>> {
  let url = `${import.meta.env.VITE_OFFICES_BASE_URL}/api/Offices?`

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (pageSize) {
    url += `pageSize=${pageSize}`
  }

  return await axios.get<{ data: PaginatedList<Office> }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createOfficeModel: CreateOfficeModel, token: string): Promise<AxiosResponse> {
  return await axios.post<CreateOfficeModel>(`${import.meta.env.VITE_OFFICES_BASE_URL}/api/Offices`, createOfficeModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(editOfficeModel: EditOfficeModel, token: string): Promise<AxiosResponse> {
  return await axios.put<Office>(`${import.meta.env.VITE_OFFICES_BASE_URL}/api/Offices`, editOfficeModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_OFFICES_BASE_URL}/api/Offices/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, POST, DELETE, PUT }