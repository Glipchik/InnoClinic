import { AxiosResponse } from 'axios';
import Doctor from '../../entities/doctor';
import axios from 'axios';
import PaginatedList from '../../models/paginatedList';
import DoctorModel from '../../models/doctors/DoctorModel';
import CreateDoctorModel from '../../models/doctors/CreateDoctorModel';
import EditDoctorModelByDoctor from '../../models/doctors/EditDoctorModelByDoctor';
import EditDoctorModel from '../../models/doctors/EditDoctorModel';

async function GET(id: string | null, specializationId: string | null, token: string): Promise<AxiosResponse<Doctor | Doctor[]>> {
  if (id === null) {
    return await axios.get<{ data: Doctor[] }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors?SpecializationId=${specializationId}`, { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Doctor }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  }
}

async function GETWithPagination(specializationId: string | null, officeId: string | null, pageIndex: number | null, pageSize: number | null, token: string): Promise<AxiosResponse<PaginatedList<DoctorModel>>> {
  let url = `${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/with-pagination?`

  if (specializationId) {
    url += `specializationId=${specializationId}&`
  }

  if (officeId) {
    url += `officeId=${officeId}&`
  }

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (pageSize) {
    url += `pageSize=${pageSize}&`
  }

  return await axios.get<{ data: DoctorModel }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createDoctorModel: CreateDoctorModel, token: string): Promise<AxiosResponse> {
  const formData = new FormData();

  Object.entries(createDoctorModel).forEach(([key, value]) => {
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

  if (createDoctorModel.photo) {
    formData.append("photo", createDoctorModel.photo);
  }

  return await axios.post<FormData>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors`, formData, {
    headers: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'multipart/form-data'
    }
  });
}

async function PUTAsDoctor(editDoctorModelByDoctor: EditDoctorModelByDoctor, token: string): Promise<AxiosResponse> {
  const formData = new FormData();

  Object.entries(editDoctorModelByDoctor).forEach(([key, value]) => {
    if (value !== null && value !== undefined) {
      formData.append(key, value as string);
    }
  });

  if (editDoctorModelByDoctor.photo) {
    formData.append("photo", editDoctorModelByDoctor.photo);
  }

  return await axios.put<FormData>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors`, formData, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(editDoctorModel: EditDoctorModel, token: string): Promise<AxiosResponse> {
  const formData = new FormData();

  Object.entries(editDoctorModel).forEach(([key, value]) => {
    if (value !== null && value !== undefined) {
      formData.append(key, value as string);
    }
  });

  if (editDoctorModel.photo) {
    formData.append("photo", editDoctorModel.photo);
  }

  return await axios.put<FormData>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/as-receptionist`, formData, {
    headers: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'multipart/form-data'
    }
  });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, GETWithPagination, POST, PUT, PUTAsDoctor, DELETE }