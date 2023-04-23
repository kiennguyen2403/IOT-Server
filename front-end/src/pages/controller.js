import { React, useState, useEffect } from "react";
import ResponsiveAppBar from "../components/header";
import Switch from '@mui/material/Switch';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import { io } from "socket.io-client";
import Typography from '@mui/material/Typography';

const socket = io("localhost:5000/", {
    withCredentials: true,
    cors: {
      origin: "http://localhost:3000/controller",
    },
    });


const leds = ["Living room","Bedroom", "Fan"]
export default function ControllerPage() {

    const [bedroom, setBedroom] = useState(false);
    const [livingRoom, setLivingRoom] = useState(false);

    useEffect(() => {
            socket.on("connect", (data) => {
                if (data)
                {
                    if (data["bed"] == "on")
                    {
                        setBedroom(true);
                    }
                    else
                    {
                        setBedroom(false);
                    }

                    if (data["living"] == "on")
                    {
                        setLivingRoom(true);
                    }
                    else
                    {
                        setLivingRoom(false);
                    }
                }
            }
        )
    },[]);


    useEffect(() => {
        socket.on("operate", (data) => {
            console.log("Command:"+data);
            if (data.led === 1)
            {
                setBedroom(data.data === "on");
            }
            else
            {
                setLivingRoom(data.data === "on");
            }
        });


    }, [bedroom,livingRoom]);


    return(
        <div>
            <ResponsiveAppBar />
            <div style ={{marginTop: 40}}>
            <Typography variant="h4" component="div">
                Controller
            </Typography>
                <div>
                    { leds.map((led) => 
                            <Card sx={{ minWidth: 275, maxWidth: 500, margin: 5 }}>
                                <CardContent>
                                    <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                        {led}
                                    </Typography>
                                </CardContent>
                                <CardActions>
                                <Switch checked={led == "Bedroom" ? bedroom : livingRoom} onClick={
                                    () => {
                                        if (led === "Bedroom")
                                        {
                                            setBedroom(!bedroom);
                                            // bedroom 
                                            // ? axios.post('http://localhost:3001/on/1', {}) 
                                            // : axios.post('http://localhost:3001/off/1', {})

                                            bedroom 
                                            ? socket.emit("operate", {'data': "on", 'led': 1})
                                            : socket.emit("operate", {'data': "off", 'led': 1})
                                        }
                                        else
                                        {
                                            setLivingRoom(!livingRoom);
                                            // livingRoom 
                                            // ? axios.post('http://localhost:3001/on/2', {}) 
                                            // : axios.post('http://localhost:3001/off/2', {})
                                            livingRoom
                                            ? socket.emit("operate", {'data': "on", 'led': 2})
                                            : socket.emit("operate", {'data': "off", 'led': 2})
                                        }
                                    }
                                }/>
                                </CardActions>
                            </Card>)
                    }
                </div>
            </div>
        </div>
    );
}