import { React, useState, useEffect } from "react";
import ResponsiveAppBar from "../components/header";
import Switch from '@mui/material/Switch';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import { io } from "socket.io-client";
import Typography from '@mui/material/Typography';
import { Alert, AlertTitle } from "@mui/material";
import IconButton from '@mui/material/IconButton';
import Collapse from '@mui/material/Collapse';
import Button from '@mui/material/Button';
import CloseIcon from '@mui/icons-material/Close';

const socket = io("localhost:5000/", {
    withCredentials: true,
    cors: {
        origin: "http://localhost:3000/controller",
    },
});


const leds = ["Living room", "Bedroom"]
export default function ControllerPage() {

    const [bedroom, setBedroom] = useState(false);
    const [livingRoom, setLivingRoom] = useState(false);
    const [fan, setFan] = useState(false);
    const [warning, setWarning] = useState(false);
    useEffect(() => {
        socket.on("connect", (data) => {
            if (data) {
                if (data["bed"] == "on") {
                    setBedroom(true);
                }
                else {
                    setBedroom(false);
                }

                if (data["living"] == "on") {
                    setLivingRoom(true);
                }
                else {
                    setLivingRoom(false);
                }
            }
        }

        )
    }, []);


    useEffect(() => {
        socket.on("operate", (data) => {
            if (data.led === 1) {
                setBedroom(data.data === "on");
            }
            else {
                setLivingRoom(data.data === "on");
            }
        });

        socket.on("warning", (data) => {
            if (data) {
                setWarning(true);
            }
        });


    }, [bedroom, livingRoom]);


    return (
        <div>
            <ResponsiveAppBar />
            <div style={{ marginTop: 40 }}>
                <Typography variant="h4" component="div">
                    Controller
                </Typography>
                <div>
                    <Card sx={{ minWidth: 275, maxWidth: 500, margin: 5 }}>
                        <CardContent>
                            <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                Bedroom
                            </Typography>
                        </CardContent>
                        <CardActions>
                            <Switch checked={bedroom} onClick={
                                () => {

                                    setBedroom(!bedroom);
                                    bedroom
                                        ? socket.emit("operate", { 'data': "on", 'led': 1 })
                                        : socket.emit("operate", { 'data': "off", 'led': 1 })
                                }
                            } />
                        </CardActions>
                    </Card>
                    <Card sx={{ minWidth: 275, maxWidth: 500, margin: 5 }}>
                        <CardContent>
                            <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                Livingroom
                            </Typography>
                        </CardContent>
                        <CardActions>
                            <Switch checked={livingRoom} onClick={
                                () => {

                                    setBedroom(!livingRoom);
                                    livingRoom
                                        ? socket.emit("operate", { 'data': "on", 'led': 0 })
                                        : socket.emit("operate", { 'data': "off", 'led': 0 })
                                }
                            } />
                        </CardActions>
                    </Card>
                    <Card sx={{ minWidth: 275, maxWidth: 500, margin: 5 }}>
                        <CardContent>
                            <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                Fan
                            </Typography>
                        </CardContent>
                        <CardActions>
                            <Switch checked={fan} onClick={
                                () => {

                                    setBedroom(!fan);
                                    fan
                                        ? socket.emit("operate", { 'data': "on", 'led': 3 })
                                        : socket.emit("operate", { 'data': "off", 'led': 3 })
                                }
                            } />
                        </CardActions>
                    </Card>
                </div>
            </div>
            <Collapse in={warning}>
                <Alert
                    severity="error"
                    action={
                        <IconButton
                            aria-label="close"
                            color="inherit"
                            size="small"
                            onClick={() => {
                                setWarning(false);
                            }}
                        >
                            <CloseIcon fontSize="inherit" />
                        </IconButton>
                    }
                    sx={{ mb: 2 }}
                >
                    Smoke detected in the house. Be careful!
                </Alert>
            </Collapse>
        </div>
    );
}