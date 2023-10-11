import { LogLevel } from "@azure/msal-browser";

const NotifyHub = "sorry-private";
const MsalConfig = {
    auth: {
        clientId: "sorry-private",
        authority: "sorry-private",
        redirectUri: "sorry-private", // Cambia esto a tu URI de redirección
    },
    cache: {
      cacheLocation: "localStorage",
      storeAuthStateInCookie: false,
    },
    system: {	
        loggerOptions: {	
            loggerCallback: (level, message, containsPii) => {	
                if (containsPii) {		
                    return;		
                }		
                switch (level) {		
                    case LogLevel.Error:		
                        console.error(message);		
                        return;		
                    case LogLevel.Warning:		
                        console.warn(message);		
                        return;		
                }	
            }	
        }	
    }
  };

const LoginRequest = {
    scopes: ["User.Read"], // Define los alcances necesarios para tu aplicación
  };

export const ClientSettings={
    NotifyHub,
    MsalConfig,
    LoginRequest
}