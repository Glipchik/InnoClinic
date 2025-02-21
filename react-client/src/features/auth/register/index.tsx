import { useContext, useEffect } from 'react';
import { UserManagerContext } from '../../../shared/contexts/UserManagerContext';
import { useNavigate } from 'react-router-dom';

const Register = () => {
  const userManager = useContext(UserManagerContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!userManager) return;
    async function redirectToRegister() {
      try {
        await userManager?.signinRedirect();
      } catch (error) {
        console.error("Error while login:", error);
      }
    }
    redirectToRegister();
  }, [userManager, navigate]);

  return null;
}

export { Register };