import {
  createBrowserRouter
} from "react-router-dom";
import { SigninOidc } from '../features/auth/SigninOidc';
import { Login } from '../features/auth/Login';
import { Logout } from '../features/auth/Logout';
import { Register } from '../features/auth/Register';
import { HomePage } from '../pages/home/HomePage'
import Layout from '../shared/ui/Layout'

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
        path: "",
        children: [
          {
            path: "login",
            element: <Login />,
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
    ]
  }
]);

export default router;