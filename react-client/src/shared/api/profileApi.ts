import { AxiosResponse } from 'axios';
import axios from 'axios';
import DoctorModel from '../../models/doctors/DoctorModel';
import ReceptionistModel from '../../models/receptionists/ReceptionistModel';
import PatientModel from '../../models/patients/PatientModel';

async function GETProfilePictureUrl(accountId : string, token : string) {
  return await axios.get<string>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Photos/${accountId}`, { headers: { Authorization: `Bearer ${token}` } });
}

async function GETDoctorsProfile(token: string) {
  return await axios.get<DoctorModel>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Doctors/me`, { headers: { Authorization: `Bearer ${token}` } });
}

async function GETPatientsProfile(token: string) {
  return await axios.get<PatientModel>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Patients/me`, { headers: { Authorization: `Bearer ${token}` } });
}

async function GETReceptionistsProfile(token: string) {
  return await axios.get<ReceptionistModel>(`${import.meta.env.VITE_PROFILES_BASE_URL}/api/Receptionists/me`, { headers: { Authorization: `Bearer ${token}` } });
}

export { GETProfilePictureUrl, GETDoctorsProfile, GETPatientsProfile, GETReceptionistsProfile }