﻿import { ConsumeRoute } from '../Utils/RoutesUtils'

async function GetListFiles(msalInstance) {
    try {
        var token = await ConsumeRoute.GetTokenUser(msalInstance);
        var response = await ConsumeRoute.CallRoute("Backup/GetAll", null, "GET", token);

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

async function DownloadFile(fileName, msalInstance) {
    try {
        var token = await ConsumeRoute.GetTokenUser(msalInstance);
        var response = await ConsumeRoute.CallRoute("Backup/DownloadFile", {fileName: fileName}, "GET-FILE", token);

        if (response.status === 200) {
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', fileName);
            document.body.appendChild(link);
            link.click();
        }else{
            alert(response.error);
        }

    } catch (err) {
        alert("Error connecting or timeout service: "+err);
        console.log(err);
    }
}

async function SaveFile(data, msalInstance){
    try {
        var token = await ConsumeRoute.GetTokenUser(msalInstance);
        var response = await ConsumeRoute.CallRoute("Backup/UploadFile", data, "POST-FILE", token);

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

export const BackupService = {
    GetListFiles,
    SaveFile,
    DownloadFile
} 