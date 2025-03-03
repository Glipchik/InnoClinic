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
        if (user === null || user.expired) {
          setIsAuthorized(false)
          setIsForbidden(false)
        } else {
          setIsAuthorized(true)
          setIsForbidden(user.profile.role.includes(requiredRole))
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

export default PrivateRoute