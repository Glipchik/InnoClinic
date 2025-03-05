import { useContext, useEffect } from 'react';
import { UserManagerContext } from '../../../shared/contexts/UserManagerContext';
import { useNavigate } from 'react-router-dom';

const Logout = () => {
  const userManager = useContext(UserManagerContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!userManager) return;
      const redirectToLogout = async () => {
      try {
        await userManager?.signoutRedirect();
        localStorage.removeItem('token')
      } catch (error) {
        console.error("Error while logout:", error);
      }
    }
    redirectToLogout();
  }, [userManager, navigate]);

  return null;
}

export { Logout };