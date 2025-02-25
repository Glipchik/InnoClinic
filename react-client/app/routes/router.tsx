import {
  createBrowserRouter
} from "react-router-dom"
import { Login } from '../../src/features/auth/login'
import { Logout } from '../../src/features/auth/logout'
import PrivateRoute from "../../src/shared/routes/PrivateRoute/index"
import { SilentRenew } from "../../src/features/auth/silent-renew"
import { Register } from "../../src/features/auth/register"
import { SigninOidc } from "../../src/features/auth/signin-oidc"
import { HomePage } from "../../src/pages/home"
import { ForbiddenPage } from "../../src/pages/errors/forbidden"
import { AppointmentsPage } from "../../src/pages/appointments"
import { OfficesPage } from "../../src/pages/offices/OfficesPage"
import Layout from "../../src/shared/ui/Layout"

const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    children: [
      {
        path: "home",
        element: <HomePage />
      },
      {
        path: "offices",
        element: <PrivateRoute requiredRole="Receptionist"> <OfficesPage /> </PrivateRoute>,
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