import { AxiosResponse } from 'axios';
import Receptionist from '../../entities/receptionist';
import axios from 'axios';
import PaginatedList from '../../models/paginatedList';
import ReceptionistModel from '../../models/receptionists/ReceptionistModel';
import CreateReceptionistModel from '../../models/receptionists/CreateReceptionistModel';
import EditReceptionistModel from '../../models/receptionists/EditReceptionistModel';

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
  const formData = new FormData();
  
  Object.entries(createReceptionistModel).forEach(([key, value]) => {
    if (value !== null && value !== undefined) {
      if (typeof value === "object" && value !== null) {
        Object.entries(value).forEach(([nestedKey, nestedValue]) => {
          if (nestedValue !== null && nestedValue !== undefined) {
            formData.append(`${key}.${nestedKey}`, nestedValue as string);
          }
        });
      } else {
        formData.append(key, value as string);
      }
    }
  });

  if (createReceptionistModel.photo) {
    formData.append("photo", createReceptionistModel.photo);
  }

  return await axios.post<FormData>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists`, formData, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(editReceptionistModel: EditReceptionistModel, token: string): Promise<AxiosResponse> {
  const formData = new FormData();
  
  Object.entries(editReceptionistModel).forEach(([key, value]) => {
    if (value !== null && value !== undefined) {
      formData.append(key, value as string);
    }
  });

  if (editReceptionistModel.photo) {
    formData.append("photo", editReceptionistModel.photo);
  }
  
  return await axios.put<FormData>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists/as-receptionist`, formData, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, GETWithPagination, POST, PUT, DELETE }