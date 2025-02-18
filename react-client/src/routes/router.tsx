import {
  createBrowserRouter
} from "react-router-dom";
import { SigninOidc } from '../features/auth/SigninOidc';
import { Login } from '../features/auth/Login';
import { Logout } from '../features/auth/Logout';
import { Register } from '../features/auth/Register';
import Layout from '../shared/ui/Layout'
import { AppointmentsPage } from "../pages/appointments/AppointmentsPage";
import { SilentRenew } from "../features/auth/SilentRenew";
import { OfficesPage } from "../pages/offices/OfficesPage";
import { SpecializationsPage } from "../pages/specializations/SpecializationsPage";
import { ServiceCategoriesPage } from "../pages/serviceCategories/ServiceCategoriesPage";
import { ServicesPage } from "../pages/services/ServicesPage";
import { PrivateRoute } from "../shared/routes/PrivateRoute/index";
import { ForbiddenPage } from "../pages/errors/ForbiddenPage";
import { DoctorsPage } from "../pages/doctors/DoctorsPage";
import { PatientsPage } from "../pages/patients/PatientsPage";
import { ReceptionistsPage } from "../pages/receptionists/ReceptionistsPage";
import ProfilePage from "../pages/profile/ProfilePage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    children: [
      {
        path: "profiles",
        children: [
          {
            path: "doctors",
            element: <PrivateRoute requiredRole="Receptionist"> <DoctorsPage /> </PrivateRoute>
          },
          {
            path: "patients",
            element: <PrivateRoute requiredRole="Receptionist"> <PatientsPage /> </PrivateRoute>
          },
          {
            path: "receptionists",
            element: <PrivateRoute requiredRole="Receptionist"> <ReceptionistsPage /> </PrivateRoute>
          },
          {
            path: "me",
            element: <PrivateRoute requiredRole="Receptionist,Patient,Doctor"> <ProfilePage /> </PrivateRoute>
          }
        ]
      },
      {
        path: "offices",
        element: <PrivateRoute requiredRole="Receptionist"> <OfficesPage /> </PrivateRoute>,
      },
      {
        path: "specializations",
        element: <PrivateRoute requiredRole="Receptionist"> <SpecializationsPage /> </PrivateRoute>,
      },
      {
        path: "service-categories",
        element: <PrivateRoute requiredRole="Receptionist"> <ServiceCategoriesPage /> </PrivateRoute>,
      },
      {
        path: "services",
        element: <PrivateRoute requiredRole="Receptionist"> <ServicesPage /> </PrivateRoute>,
      },
      {
        path: "",
        children: [
          {
            path: "forbidden",
            element: <ForbiddenPage />,
          },
          {
            path: "login",
            element: <Login />,
          },
          {
            path: "silent-renew",
            element: <SilentRenew />,
          },
          {
            path: "logout",
            element: <Logout />,
          },
          {
            path: "register",
            element: <Register />,
          },
          {
            path: "signin-oidc",
            element: <SigninOidc />,
          }
        ]
      },
      {
        path: "",
        children: [
          {
            path: "appointments",
            element: <PrivateRoute requiredRole="Patient"> <AppointmentsPage /> </PrivateRoute>,
          },
        ]
      },
    ]
  }
]);

export default router;