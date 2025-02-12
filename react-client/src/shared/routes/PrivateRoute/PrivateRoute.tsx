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
  const [isForbidden, setIsForbidden] = useState<boolean | null>(null);

  useEffect(() => {
    if (userManager) {
      userManager.getUser().then((user) => {
        if (!user || user.expired) {
          setIsAuthorized(false)
        } else {
          setIsAuthorized(true)
          if (!user.profile.role.includes(requiredRole)) {
            setIsForbidden(true)
          } else {
            setIsForbidden(false)
          }
        }
      });
    }
  }, [userManager, requiredRole]);

  if (isAuthorized === null || isForbidden === null) {
    return <Loading label="Checking session..." />;
  }

  if (!isAuthorized) {
    userManager?.signinRedirect()
  }

  if (!isForbidden && isAuthorized) {
    return <Navigate to="/forbidden" />;
  }

  return <>{children}</>;
}

export { PrivateRoute };