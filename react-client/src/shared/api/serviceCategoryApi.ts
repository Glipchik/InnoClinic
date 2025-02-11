import axios from 'axios';
import ServiceCategory from '../../entities/serviceCategory';
import { AxiosResponse } from 'axios';
import CreateServiceCategoryModel from '../../features/serviceCategory/models/createServiceCategoryModel';
import PaginatedList from '../../models/paginatedList';

async function GET(id: string | null, token: string): Promise<AxiosResponse<ServiceCategory | ServiceCategory[]>> {

  if (id === null) {
    return await axios.get<{ data: ServiceCategory[] }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/ServiceCategories`,  { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: ServiceCategory }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/ServiceCategories/${id}`,  { headers: { Authorization: `Bearer ${token}` } });
  }
}

async function GETWithPagination(pageIndex: number | null, pageSize: number | null, token: string): Promise<AxiosResponse<PaginatedList<ServiceCategory>>> {
  let url = `${import.meta.env.VITE_SERVICES_BASE_URL}/api/ServiceCategories/with-pagination?`

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (pageSize) {
    url += `pageSize=${pageSize}`
  }

  return await axios.get<{ data: PaginatedList<ServiceCategory> }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createServiceCategoryModel: CreateServiceCategoryModel, token: string): Promise<AxiosResponse> {
  return await axios.post<CreateServiceCategoryModel>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/ServiceCategories`, createServiceCategoryModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(servicecategory: ServiceCategory, token: string): Promise<AxiosResponse> {
  return await axios.put<ServiceCategory>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/ServiceCategories`, servicecategory, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/ServiceCategories/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, POST, DELETE, PUT, GETWithPagination }