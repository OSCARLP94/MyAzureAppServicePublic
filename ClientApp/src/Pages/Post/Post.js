import React, { useEffect, useState } from "react";
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
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import { PostService } from "../../Services/post-service";

export default function Post() {
  const { instance, accounts, inProgress } = useMsal();
  const account = useAccount(accounts[0] || {});

  const [posts, setPosts] = useState([]);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");

  useEffect(() => {
    (async function () {
      if (account) {
        await LoadList();
      }
    })();
  }, [account, instance]);

  const LoadList = async () => {
    let list = await PostService.GetListPosts(instance);
    if (list !== null) setPosts(list);
  };

  const Register = async () => {
    var resp = await PostService.RegisterPost(
      {
        title: title,
        description: description,
        user: "default",
      }, instance);

    if (resp !== null) {
      await LoadList();
      setDescription("");
      setTitle("");
    }
  };

  return (
    <MsalAuthenticationTemplate
      interactionType={InteractionType.Redirect}
      authenticationRequest={ClientSettings.LoginRequest}
    >
      <div>
        <h1>Posts</h1>
        <div>
          <Button
            variant="contained"
            color="primary"
            disabled={title === "" || description === ""}
            onClick={async (e) => {
              await Register(e);
            }}
          >
            Register
          </Button>
          <TextField
            id="outlined-basic"
            label="Tittle"
            variant="outlined"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />
          <TextField
            id="outlined-multiline-static"
            label="Description"
            multiline
            rows={4}
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>
        <div>
          <TableContainer component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
              <TableHead>
                <TableRow>
                  <TableCell align="center">
                    <b>Titulo</b>
                  </TableCell>
                  <TableCell align="center">
                    <b>Modificado</b>
                  </TableCell>
                  <TableCell align="center">
                    <b>Detalle</b>
                  </TableCell>
                  <TableCell align="center">
                    <b>Actions</b>
                  </TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {posts.map((item) => (
                  <TableRow
                    key={item.title}
                    sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                  >
                    <TableCell align="center">{item.title}</TableCell>
                    <TableCell align="center">{item.registerDate}</TableCell>
                    <TableCell align="center">{item.description}</TableCell>
                    <TableCell align="center"></TableCell>
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
