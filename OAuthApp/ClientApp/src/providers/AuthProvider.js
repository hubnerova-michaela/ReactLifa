import React, { createContext, useReducer, useContext, useEffect } from "react";
import { UserManager, WebStorageStateStore, Log } from "oidc-client";
import { IDENTITY_CONFIGURATION, METADATA_OIDC } from "../configuration/authorization";
import axios from "axios";

// reducer actions
export const SET_ACCESS_TOKEN = "SET_ACCESS_TOKEN";
export const CLEAR_ACCESS_TOKEN = "CLEAR_ACCESS_TOKEN";
export const SET_ID_TOKEN = "SET_ID_TOKEN";
export const CLEAR_ID_TOKEN = "CLEAR_ID_TOKEN";
export const USER_EXPIRED = "USER_EXPIRED";
export const USER_FOUND = "USER_FOUND";
export const USER_EXPIRING = "USER_EXPIRING";
export const LOADING_USER = "LOADING_USER";
export const SILENT_RENEW_ERROR = "SILENT_RENEW_ERROR";
export const SESSION_TERMINATED = "SESSION_TERMINATED";
export const LOAD_USER_ERROR = "LOAD_USER_ERROR";
export const USER_SIGNED_OUT = "USER_SIGNED_OUT";

// oidc-client configuration
const userStore = window.localStorage;
Log.logger = console;
Log.level = Log.ERROR;//Log.DEBUG;
const userManager = new UserManager({
    ...IDENTITY_CONFIGURATION,
    userStore: new WebStorageStateStore({ store: userStore }),
    metadata: {
        ...METADATA_OIDC
    }
});

// helper method for parsing JWT
const parseJwt = token => {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace("-", "+").replace("_", "/");
    return JSON.parse(window.atob(base64));
};

// store content
const initialState = {
    userManager: userManager,
    accessToken: null,
    idToken: null,
    userId: null,
    profile: null,
    isUserLoading: false
}

const reducer = (state, action) => {
    switch (action.type) {
        case LOADING_USER:
            return { ...state, isUserLoading: true }
        case SET_ACCESS_TOKEN:
            return { ...state, accessToken: action.payload }
        case CLEAR_ACCESS_TOKEN:
            return { ...state, accessToken: null }
        case SET_ID_TOKEN:
            return { ...state, idToken: action.payload }
        case CLEAR_ID_TOKEN:
            return { ...state, idToken: null }
        case USER_FOUND:
            return { ...state, idToken: action.idToken, accessToken: action.accessToken, userId: action.userId, profile: action.profile, isUserLoading: false }
        case USER_EXPIRED:
        case LOAD_USER_ERROR:
        case SILENT_RENEW_ERROR:
        case USER_SIGNED_OUT:
        case SESSION_TERMINATED:
            return { ...state, idToken: null, accessToken: null, userId: null, profile: null, isUserLoading: false }
        default: {
            return state;
        }
    }
}

export const AuthContext = createContext(initialState);
export const AuthConsumer = AuthContext.Consumer;
export const AuthProvider = props => {
    const store = useReducer(
        reducer,
        initialState
    );
    const [, dispatch] = store;
    useEffect(() => {
        userManager.events.addUserLoaded(user => {
            const tokenData = parseJwt(user.access_token);
            dispatch({
                type: USER_FOUND,
                accessToken: user.access_token,
                idToken: user.id_token,
                userId: tokenData.sub,
                profile: user.profile
            });
            console.info("Uživatel byl přihlášen");
			console.log(user);

			axios({
                method: "post",
                url: "api/User",
                headers: {
                    Authorization: "Bearer " + user.access_token
                },
                data: {
                    Name: user.profile.name,
                    Email: user.profile.email
                }
            })

        });
        userManager.events.addUserUnloaded(() => {
            dispatch({ type: USER_EXPIRED });
            console.info("Informace o přihlášení jsou neplatné.");
        });
        userManager.events.addAccessTokenExpiring(() => {
            dispatch({ type: USER_EXPIRING });
            console.info("Platnost přihlášení brzy vyprší.");
        });
        userManager.events.addAccessTokenExpired(() => {
            dispatch({ type: USER_EXPIRED });
            console.info("Platnost přihlášení vypršela.");
        });
        userManager.events.addSilentRenewError(() => {
            dispatch({ type: SILENT_RENEW_ERROR });
            console.info("Nepodařilo se obnovit přihlášení.");
        });
        userManager.events.addUserSignedOut(() => {
            dispatch({ type: USER_EXPIRED });
            console.info("Uživatel byl odhlášen.");
        });

        userManager.getUser()
            .then((user) => {
                if (user && !user.expired) {
                    let tokenData = parseJwt(user.access_token);
                    dispatch({
                        type: USER_FOUND,
                        accessToken: user.access_token,
                        idToken: user.id_token,
                        userId: tokenData.sub,
                        profile: user.profile
                    });
                } else if (!user || (user && user.expired)) {
                    dispatch({
                        type: USER_EXPIRED
                    });
                }
            })
            .catch(() => {
                dispatch({
                    type: LOAD_USER_ERROR
                });
            });
    }, [dispatch]);
    return (
        <AuthContext.Provider value={store}>
            {props.children}
        </AuthContext.Provider>
    );
}

export const useAuthContext = () => useContext(AuthContext);