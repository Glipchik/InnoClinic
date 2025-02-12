import axios from 'axios';
import { AxiosResponse } from 'axios';
import Service from '../../entities/service';
import PaginatedList from '../../models/paginatedList';
import CreateServiceModel from '../../features/services/models/CreateServiceModel';
import ServiceModel from '../../features/services/models/ServiceModel';

async function GET(id: string | null, specializationId: string | null, token: string): Promise<AxiosResponse<Service | Service[]>> {
  if (id === null) {
    let url = `${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services?`

    if (specializationId) {
      url += `SpecializationId=${specializationId}`
    }
    
    return await axios.get<{ data: Service[] }>(url, { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Service }>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  }
}

async function GETWithPagination(pageIndex: number | null, pageSize: number | null, token: string): Promise<AxiosResponse<PaginatedList<Service>>> {
  let url = `${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services/with-pagination?`

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (pageSize) {
    url += `pageSize=${pageSize}`
  }

  return await axios.get<{ data: PaginatedList<Service> }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createServiceModel: CreateServiceModel, token: string): Promise<AxiosResponse> {
  return await axios.post<CreateServiceModel>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services`, createServiceModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(serviceModel: ServiceModel, token: string): Promise<AxiosResponse> {
  return await axios.put<Service>(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services`, serviceModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_SERVICES_BASE_URL}/api/Services/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, POST, DELETE, PUT, GETWithPagination }