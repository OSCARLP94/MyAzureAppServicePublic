import { ConsumeRoute } from "../Utils/RoutesUtils";

async function GetListPosts(msalInstance) {
    try {
        var token = await ConsumeRoute.GetTokenUser(msalInstance);
        var response = await ConsumeRoute.CallRoute("Post/GetAll", null, "GET", token);

        let json = await response.clone().json();
        if (response.status == 200 && json.response === true && json.error === "") {
            return json.data;
        }

        return null;
    } catch (err) {
        alert("Error connecting or timeout service: "+err);
        return null;
    }
}

async function RegisterPost(data, msalInstance) {
    try {
        var token = await ConsumeRoute.GetTokenUser(msalInstance);
        var response = await ConsumeRoute.CallRoute("Post/Register", data, "POST", token);

        let json = await response.clone().json();
        if (response.status == 200 && json.response === true && json.error === "") {
            return json.data;
        }

        return null;
    } catch (err) {
        alert("Error connecting or timeout service: "+err);
        return null;
    }
}

export const PostService={
    GetListPosts,
    RegisterPost
};