import React, { useEffect, useState } from "react";
import { BackupService } from "../../Services/backup-service";
import { HubConnectionBuilder } from "@microsoft/signalr";
import {
  MsalAuthenticationTemplate,
  useMsal,
  useAccount,
} from "@azure/msal-react";
import { InteractionType } from "@azure/msal-browser";
import { ClientSettings } from "../../clientsettings";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import Button from "@mui/material/Button";
import CloudDownloadIcon from "@mui/icons-material/CloudDownload";

export default function Backup() {
  const { instance, accounts, inProgress } = useMsal();
  const account = useAccount(accounts[0] || {});

  const [file, setFile] = useState(null);
  const [fileName, setFileName] = useState("");
  const [backups, setBackups] = useState([]);

  useEffect(() => {
    (async function () {
      if (account) {
        await ConnectNotificationSignalR();
        await LoadList();
      }
    })();
  }, [account, instance]);

  const ConnectNotificationSignalR = async () => {
    const connection = new HubConnectionBuilder()
      .withUrl(ClientSettings.NotifyHub)
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then((result) => {
        connection.on("ReceiveNotification", (message) => {
          alert("Detected blob added or deleted, please reload...");
        });
      })
      .catch((e) => console.log("Connection failed: ", e));
  };

  const LoadList = async () => {
    let list = await BackupService.GetListFiles(instance);
    if (list !== null) setBackups(list);
  };

  const uploadFile = async (e) => {
    setFile(e.target.files[0]);
    setFileName(e.target.files[0].name);
  };

  const saveFile = async (e) => {
    const formData = new FormData();
    formData.append("fileName", fileName);
    formData.append("formFile", file);

    await BackupService.SaveFile(formData, instance);
    document.getElementById("inputfile").value = "";
    setFileName("");
    setFile(null);
    LoadList();
  };

  const downloadFile = async (file) => {
    await BackupService.DownloadFile(file, instance);
  };

  return (
    <MsalAuthenticationTemplate
      interactionType={InteractionType.Redirect}
      authenticationRequest={ClientSettings.LoginRequest}
    >
      <div>
        <h1>Backups files</h1>
        <div>
          <input type="file" id="inputfile" onChange={uploadFile} />
          <Button
            variant="contained"
            color="primary"
            disabled={file === null}
            onClick={(e) => {
              saveFile(e);
            }}
          >
            Save backup
          </Button>
          <p>{fileName}</p>
        </div>
        <div>
          <TableContainer component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
              <TableHead>
                <TableRow>
                  <TableCell align="center">
                    <b>Nombre</b>
                  </TableCell>
                  <TableCell align="center">
                    <b>Modificado</b>
                  </TableCell>
                  <TableCell align="center">
                    <b>Tamaño</b>
                  </TableCell>
                  <TableCell align="center">
                    <b>Opciones</b>
                  </TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {backups.map((item) => (
                  <TableRow
                    key={item.name}
                    sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                  >
                    <TableCell align="center">{item.name}</TableCell>
                    <TableCell align="center">{item.modified}</TableCell>
                    <TableCell align="center">{item.size}</TableCell>
                    <TableCell align="center">
                      <CloudDownloadIcon
                        onClick={async () => downloadFile(item.name)}
                      />
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </div>
      </div>
    </MsalAuthenticationTemplate>
  );
}
