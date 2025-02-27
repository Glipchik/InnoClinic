import { UserManagerContext } from '@shared/contexts/UserManagerContext';
import { useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const userManager = useContext(UserManagerContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!userManager) return;
    const redirectToLogin = async () => {
      try {
        await userManager?.signinRedirect();
      } catch (error) {
        console.error("Error while login:", error);
      }
    }
    redirectToLogin();
  }, [userManager, navigate]);

  return null;
}

export { Login };