import { AxiosResponse } from 'axios';
import Patient from '../../entities/patient';
import axios from 'axios';
import PaginatedList from '../../models/paginatedList';
import PatientModel from '../../models/patients/PatientModel';
import CreatePatientModel from '../../models/patients/CreatePatientModel';
import UpdatePatientModelByPatient from '../../models/patients/UpdatePatientModelByPatient';

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
  return await axios.post<CreatePatientModel>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients`, createPatientModel, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUTAsPatient(updatePatientModelByPatient: UpdatePatientModelByPatient, token: string): Promise<AxiosResponse> {
  return await axios.put<Patient>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients`, updatePatientModelByPatient, { headers: { Authorization: `Bearer ${token}` } });
}

async function PUT(patient: Patient, token: string): Promise<AxiosResponse> {
  return await axios.put<Patient>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients/as-receptionist`, patient, { headers: { Authorization: `Bearer ${token}` } });
}

async function DELETE(id: string, token: string): Promise<AxiosResponse> {
  return await axios.delete(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients/${id}`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GET, GETWithPagination, POST, PUT, PUTAsPatient, DELETE }