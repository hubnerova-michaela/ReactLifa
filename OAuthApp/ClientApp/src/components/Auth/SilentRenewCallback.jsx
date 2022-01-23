import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider";

const SilentRenewCallback = props => {
    const [{ userManager }] = useAuthContext();
    let navigate = useNavigate();
    userManager.signinSilentCallback();
    navigate("/");
    return null;
}

export default SilentRenewCallback;