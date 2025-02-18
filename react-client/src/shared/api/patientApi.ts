import { AxiosResponse } from 'axios';
import Patient from '../../entities/patient';
import axios from 'axios';
import PaginatedList from '../../models/paginatedList';
import PatientModel from '../../models/patients/PatientModel';
import CreatePatientModel from '../../models/patients/CreatePatientModel';
import EditPatientModel from '../../models/patients/EditPatientModel';

async function GET(id: string | null, token: string): Promise<AxiosResponse<Patient | Patient[]>> {
  if (id === null) {
    return await axios.get<{ data: Patient[] }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api`, { headers: { Authorization: `Bearer ${token}` } });
  } else {
    return await axios.get<{ data: Patient }>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  }
}

async function GETWithPagination(pageIndex: number | null, pageSize: number | null, token: string): Promise<AxiosResponse<PaginatedList<PatientModel>>> {
  let url = `${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients/with-pagination?`

  if (pageIndex) {
    url += `pageIndex=${pageIndex}&`
  }

  if (pageSize) {
    url += `pageSize=${pageSize}`
  }

  return await axios.get<{ data: PatientModel }>(url, { headers: { Authorization: `Bearer ${token}` } });
}

async function POST(createPatientModel: CreatePatientModel, token: string): Promise<AxiosResponse> {
  const formData = new FormData();
  
  Object.entries(createPatientModel).forEach(([key, value]) => {
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

  if (createPatientModel.photo) {
    formData.append("photo", createPatientModel.photo);
  }

  return await axios.post<FormData>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients`, formData, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUTAsPatient(editPatientModel: EditPatientModel, token: string): Promise<AxiosResponse> {
  const formData = new FormData();
  
  Object.entries(editPatientModel).forEach(([key, value]) => {
    if (value !== null && value !== undefined) {
      formData.append(key, value as string);
    }
  });

  if (editPatientModel.photo) {
    formData.append("photo", editPatientModel.photo);
  }

  return await axios.put<FormData>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients`, formData, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(editPatientModel: EditPatientModel, token: string): Promise<AxiosResponse> {
  const formData = new FormData();
  
  Object.entries(editPatientModel).forEach(([key, value]) => {
    if (value !== null && value !== undefined) {
      formData.append(key, value as string);
    }
  });

  if (editPatientModel.photo) {
    formData.append("photo", editPatientModel.photo);
  }

  return await axios.put<FormData>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients/as-receptionist`, formData, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, GETWithPagination, POST, PUT, PUTAsPatient, DELETE }