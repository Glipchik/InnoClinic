import { useContext, useEffect } from "react"
import { UserManagerContext } from "../../../shared/contexts/UserManagerContext"
import { useNavigate } from "react-router-dom"
import { useDispatch } from "react-redux"

function SilentRenew() {
  const userManager = useContext(UserManagerContext)
  const navigate = useNavigate()
  const dispatch = useDispatch()

  useEffect(() => {
    if (!userManager) return
    async function silentRenew() {
      try {
        await userManager!.signinSilentCallback()
      } catch (error) {
        console.error("Silent renew error: ", error)
      }
    }
    silentRenew()
  }, [userManager, navigate, dispatch])

  return null
}

export { SilentRenew }