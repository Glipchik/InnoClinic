import { User } from 'oidc-client';
import { Link } from 'react-router-dom';
import Button from '../../shared/ui/controls/Button';

interface UserInfoProps {
  user?: User | null
}

const UserInfo = ({ user } : UserInfoProps) => {
  return (
    <div className="flex w-full h-full items items-center space-x-3 justify-end">
      {user ? (
        <>
          <div className="flex-col h-full space-y-0">
            <p className="text-xl">
              {user.profile.email}
            </p>
            <p className="text-sm text-gray-800">
              {user.profile.role}
            </p>
          </div>
          <li className="list-none"> <Link to="/logout"> <Button className="bg-red-600 hover:bg-red-700"> Logout </Button> </Link> </li>
        </>
      ) : (
        <>
          <li className="list-none"> <Link to="/login"> <Button> Login </Button> </Link> </li>
          <li className="list-none">  <Link to="/register"> <Button> Registration </Button> </Link> </li>
        </>
      )}
    </div>
  )
}

export default UserInfo