import { useContext, useEffect } from "react"
import { UserManagerContext } from "../../../shared/contexts/UserManagerContext"
import { useNavigate } from "react-router-dom"

const SigninOidc = () => {
  const userManager = useContext(UserManagerContext)
  const navigate = useNavigate()

  useEffect(() => {
    if (!userManager) return
    async function signinAsync() {
      try {
        const user = await userManager!.signinRedirectCallback()
        if (user) {
          navigate("/")
        } else {
          navigate("/login")
        }
      } catch (error) {
        console.error("Error during sign-in:", error)
      }
    }
    signinAsync()
  }, [userManager, navigate])

  return null
}

export { SigninOidc }