import {
  createBrowserRouter
} from "react-router-dom";
import { SigninOidc } from '../features/auth/SigninOidc';
import { Login } from '../features/auth/Login';
import { Logout } from '../features/auth/Logout';
import { Register } from '../features/auth/Register';
import { HomePage } from '../pages/home/HomePage'
import Layout from '../shared/ui/Layout'
import { AppointmentsPage } from "../pages/appointments/AppointmentsPage";
import { SilentRenew } from "../features/auth/SilentRenew";
import { OfficesPage } from "../pages/offices/OfficesPage";
import { SpecializationsPage } from "../pages/specializations/SpecializationsPage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    children: [
      {
        path: "home",
        element: <HomePage />,
      },
      {
        path: "offices",
        element: <OfficesPage />,
      },
      {
        path: "specializations",
        element: <SpecializationsPage />,
      },
      {
        path: "",
        children: [
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
            element: <AppointmentsPage />,
          },
        ]
      },
    ]
  }
]);

export default router;