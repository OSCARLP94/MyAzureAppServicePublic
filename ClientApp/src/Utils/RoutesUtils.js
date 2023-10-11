import axios from 'axios';
import { PublicClientApplication } from "@azure/msal-browser";
import { ClientSettings } from '../clientsettings';

const msalInstance = new PublicClientApplication(ClientSettings.MsalConfig);

async function CallRoute(route, data, method, bearer = '') {
    const controller = new AbortController();
    const id = setTimeout(() => controller.abort(), 7000)
    let response = null
    switch (method.toUpperCase()) {
        case "GET":
            let routeComplete = data !== null ?
                route + "?" + new URLSearchParams(data)
                : route;

            response = await fetch(routeComplete,
                {
                    method: method,
                    headers: {
                       Authorization: 'Bearer ' + bearer,
                    },
                    credentials: "include",
                    signal: controller.signal,
                });
            break;
        case "GET-FILE":
            let routeCompleteFile = data !== null ?
                route + "?" + new URLSearchParams(data)
                : route;
                
            response = await fetch(routeCompleteFile,
                {
                    method: "GET",
                    headers: {
                        Authorization: 'Bearer ' + bearer,
                    },
                    credentials: "include",
                    signal: controller.signal
                });
            break;
        case "POST-FILE":
            response = await fetch(route,
                {
                    method: "POST",
                    headers: {
                        Authorization: 'Bearer ' + bearer,
                    },
                    signal: controller.signal,
                    credentials: "include",
                    body: data
                });
                break;
        case "POST":
        default:
            response = await fetch(route,
                {
                    method: method,
                    headers: {
                        Accept: 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + bearer,
                    },
                    credentials: "include",
                    signal: controller.signal,
                    body: JSON.stringify(data)
                });
            break;
    }

    clearTimeout(id);
    return response;
}

async function CallRouteAxios(route, data, method, bearer = ''){
    const controller = new AbortController();
    const id = setTimeout(() => controller.abort(), 7000);

    let respAxios = await axios.post(route, data);
    clearTimeout(id);

    return respAxios;
}

async function GetTokenUser(msalInstance) {
    var tokenAZ = await acquireAccessToken(msalInstance);

    return tokenAZ;
}

const acquireAccessToken = async (msalInstance) => {
    const activeAccount = msalInstance.getActiveAccount(); // This will only return a non-null value if you have logic somewhere else that calls the setActiveAccount API
    const accounts = msalInstance.getAllAccounts();

    if (!activeAccount && accounts.length === 0) {
       alert("Users not signed in.")
       return null;
    }
    const request = {
        scopes: ["User.Read"],
        account: activeAccount || accounts[0]
    };

    const authResult = await msalInstance.acquireTokenSilent(request);

    return authResult.accessToken
};


export const ConsumeRoute = {
    CallRoute,
    GetTokenUser,
    CallRouteAxios
}