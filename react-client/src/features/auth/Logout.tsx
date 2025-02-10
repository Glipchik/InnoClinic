import { useContext, useEffect } from 'react';
import { UserManagerContext } from '../../shared/contexts/UserManagerContext';
import { useNavigate } from 'react-router-dom';

function Logout() {
  const userManager = useContext(UserManagerContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!userManager) return;

    async function redirectToLogout() {
      try {
        await userManager?.signoutRedirect();
      } catch (error) {
        console.error("Error while logout:", error);
      }
    }

    redirectToLogout();
  }, [userManager, navigate]);

  return null;
}

export { Logout };