import {
  createBrowserRouter
} from "react-router-dom"
import { Login } from '@features/auth/login'
import { Logout } from '@features/auth/logout'
import PrivateRoute from "@shared/routes/PrivateRoute/index"
import { SilentRenew } from "@features//auth/silent-renew"
import { Register } from "@features//auth/register"
import { SigninOidc } from "@features//auth/signin-oidc"
import { HomePage } from "@pages/home"
import { ForbiddenPage } from "@pages/errors/forbidden"
import { AppointmentsPage } from "@pages/appointments"
import Layout from "@shared/ui/layout"
import { OfficesPage } from "@pages/offices"
import { EditOfficePage } from "@pages/edit-office"
import { SpecializationsPage } from "@pages/specializations"
import { EditSpecializationPage } from "@pages/edit-specialization"
import { EditServiceCategoryPage } from "@pages/edit-service-category"
import { ServiceCategoriesPage } from "@pages/service-categories"
import { ServicesPage } from "@pages/services"
import { EditServicePage } from "@pages/edit-service"

const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    children: [
      {
        path: "home",
        index: true,
        element: <HomePage />
      },
      {
        path: "offices",
        children: [
          {
            path: "",
            element: <PrivateRoute requiredRole="Receptionist"> <OfficesPage /> </PrivateRoute>,
          },
          {
            path: "edit/:id",
            element: <PrivateRoute requiredRole="Receptionist"> <EditOfficePage /> </PrivateRoute>,
          },
        ]
      },
      {
        path: "specializations",
        children: [
          {
            path: "",
            element: <PrivateRoute requiredRole="Receptionist"> <SpecializationsPage /> </PrivateRoute>,
          },
          {
            path: "edit/:id",
            element: <PrivateRoute requiredRole="Receptionist"> <EditSpecializationPage /> </PrivateRoute>,
          },
        ]
      },
      {
        path: "service-categories",
        children: [
          {
            path: "",
            element: <PrivateRoute requiredRole="Receptionist"> <ServiceCategoriesPage /> </PrivateRoute>,
          },
          {
            path: "edit/:id",
            element: <PrivateRoute requiredRole="Receptionist"> <EditServiceCategoryPage /> </PrivateRoute>,
          },
        ]
      },
      {
        path: "services",
        children: [
          {
            path: "",
            element: <PrivateRoute requiredRole="Receptionist"> <ServicesPage /> </PrivateRoute>,
          },
          {
            path: "edit/:id",
            element: <PrivateRoute requiredRole="Receptionist"> <EditServicePage /> </PrivateRoute>,
          },
        ]
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