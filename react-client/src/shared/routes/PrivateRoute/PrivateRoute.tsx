import { useContext, useEffect, useState } from "react";
import { Navigate } from 'react-router-dom';
import { UserManagerContext } from '../../contexts/UserManagerContext';
import Loading from "../../ui/controls/Loading";

interface PrivateRouteProps {
  requiredRole: string;
  children: React.ReactNode;
}

const PrivateRoute = ({ requiredRole, children }: PrivateRouteProps) => {
  const userManager = useContext(UserManagerContext);
  const [isAuthorized, setIsAuthorized] = useState<boolean | null>(null);

  useEffect(() => {
    if (userManager) {
      userManager.getUser().then((user) => {
        if (!user || user.expired || !user.profile.role.includes(requiredRole)) {
          setIsAuthorized(false);
        } else {
          setIsAuthorized(true);
        }
      });
    }
  }, [userManager, requiredRole]);

  if (isAuthorized === null) {
    return <Loading label="Loading..." />;
  }

  if (!isAuthorized) {
    return <Navigate to="/forbidden" />;
  }

  return <>{children}</>;
}

export { PrivateRoute };