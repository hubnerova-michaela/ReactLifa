import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Navigate, Redirect } from "react-router";
import { useEffect, useState } from "react";
import { useAuthContext } from "../providers/AuthProvider";

const Profile = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [realities, setRealities] = useState(null);
    const [accessToken] = useAuthContext()
    let history = useNavigate()

    useEffect(() => {
        setIsLoading(true);
        axios.get("api/Reality", {
            headers:{
                "Content-Type": "application/json",
                "Authorization": `Bearer ${accessToken}`

            }
        })
            .then(response => {
                setRealities(response.data);
                setIsLoading(false);
            })

            console.log(accessToken)
    }, []);

    function CreateNewReality() {
        axios.post("api/Reality")
        .then(response => this.setState({RealityName: "New Reality"}),
                Navigate("/reality/{this.RealityId}"));
    
    }

    if (isLoading === true) {
        return "is loading"
    }
    else if (realities && realities.length > 0) {
        return (
            <div>
                <button onClick={CreateNewReality}>New Reality</button>
            </div>
            /*<div className="home-container">
                {realities.sort((a, b) => {return new Date(b.added) - new Date(a.added)}).map((item, index) => <PinCard key={index} item={item} />)}
            </div>*/
        )
    }
    else {
        return (
            <div className="loading">
                <h1>There are no realities yet.</h1>
            </div>
        )
    }
}

export default Profile;