import { React, useState, useEffect } from "react";
import ResponsiveAppBar from "../components/header";
import Switch from '@mui/material/Switch';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import { connect, io } from "socket.io-client";
import Typography from '@mui/material/Typography';
import { Alert, AlertTitle } from "@mui/material";
import IconButton from '@mui/material/IconButton';
import Collapse from '@mui/material/Collapse';
import Button from '@mui/material/Button';
import CloseIcon from '@mui/icons-material/Close';
import FormGroup from '@mui/material/FormGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Box from '@mui/material/Box';
import Slider from '@mui/material/Slider';





export default function ControllerPage() {
    const [bedroom, setBedroom] = useState(false);
    const [livingRoom, setLivingRoom] = useState(false);
    const [warning, setWarning] = useState(false);
    const [isTimer, setIsTimer] = useState(false);
    const [socketInstance, setSocketInstance] = useState(null);



    const valuetext = (value) => {
        if (socketInstance)
        {
            socketInstance.emit("message", { 'data': "timer", 'time': value });
        }
    }

    useEffect(() => {
        const socket = io("localhost:5000/", {
            withCredentials: true,
            cors: {
                origin: "http://localhost:3000/",
            },
        });


        socket.on("connect", (data) => {
            console.log("connected")
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
        });

        setSocketInstance(socket)
        return () => {
            socket.disconnect();
        };
    }, []);


    useEffect(() => {
        if (socketInstance) {
            socketInstance.on("message", (data) => {
                console.log(data)
                if (data) {
                    if (data["bed"] === "on" || data === "bedroom1") {
                        setBedroom(true);
                    }
                    else if (data["bed"] === "off" || data === "bedroom0") {
                        setBedroom(false);
                    }

                    if (data["living"] === "on" || data === "livingroom1") {
                        setLivingRoom(true);
                    }
                    else if (data["living"] === "off" || data === "livingroom0") {
                        setLivingRoom(false);
                    }

                    if (data === "fire1")
                    {
                        setWarning(true);
                    } else if (data === "fire0")
                    {
                        setWarning(false);
                    }


                }
            });
        }

    }, [bedroom, livingRoom]);

    return (
        <div>
            <ResponsiveAppBar />
            <div style={{ marginTop: 40 }}>
                <Typography variant="h4" component="div">
                    Controller
                </Typography>
                <div>
                    <FormGroup style={{ margin: 10, marginLeft: 50 }}>
                        <FormControlLabel
                            control={<Checkbox
                                checked={isTimer}
                                onClick={() => { 
                                    setIsTimer(!isTimer);
                                    if (isTimer && socketInstance){
                                        socketInstance.emit("message", { 'data': "timer", 'time': "disable" })
                                    }
                                }}
                                style={{ margin:2}} />}
                            label="Timer mode" />
                        <Collapse in={isTimer}>
                            <Box sx={{ width: 300, marginLeft: 2 }}>
                                <Slider
                                    aria-label="Temperature"
                                    defaultValue={30}
                                    onChangeCommitted={(event, value) => { valuetext(value) }}
                                    valueLabelDisplay="auto"
                                    step={1}
                                    marks
                                    min={1}
                                    max={8}
                                />
                            </Box>
                        </Collapse>
                    </FormGroup>
                    <Card sx={{ minWidth: 275, maxWidth: 500, margin: 5 }}>
                        <CardContent>
                            <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                Bedroom
                            </Typography>
                        </CardContent>
                        <CardActions>
                            <Switch checked={bedroom} onClick={
                                () => {
                                    if (socketInstance) {
                                        setBedroom(!bedroom);
                                        !bedroom
                                            ? socketInstance.emit("message", { 'data': "on", 'led': 1 })
                                            : socketInstance.emit("message", { 'data': "off", 'led': 1 })
                                    }
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
                                    if (socketInstance) {
                                        setLivingRoom(!livingRoom);
                                        !livingRoom
                                            ? socketInstance.emit("message", { 'data': "on", 'led': 2 })
                                            : socketInstance.emit("message", { 'data': "off", 'led': 2 })
                                    }
                                }
                            } />
                        </CardActions>
                    </Card>
                    {/* <Card sx={{ minWidth: 275, maxWidth: 500, margin: 5 }}>
                        <CardContent>
                            <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                Fan
                            </Typography>
                        </CardContent>
                        <CardActions>
                            <Switch checked={fan} onClick={
                                () => {

                                    // setFan(!fan);
                                    // !fan
                                    //     ? socket.emit("operate", { 'data': "on", 'led': 3 })
                                    //     : socket.emit("operate", { 'data': "off", 'led': 3 })
                                }
                            } />
                        </CardActions>
                    </Card> */}
                </div>
            </div>
            <Collapse in={warning} style={{bottom:0, marginTop:"9%"}}>
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