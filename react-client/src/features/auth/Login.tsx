import { useContext, useEffect } from 'react';
import { UserManagerContext } from '../../shared/contexts/UserManagerContext';
import { useNavigate } from 'react-router-dom';

function Login() {
  const userManager = useContext(UserManagerContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!userManager) return;

    async function redirectToLogin() {
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