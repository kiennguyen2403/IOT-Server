import {React, useEffect, useState} from "react";
import DashboardPage from "../pages/dashboard";
import ControllerPage from "../pages/controller";
import {
    BrowserRouter as Router,
    Routes, //replaces "Switch" used till v5
    Route,
} from "react-router-dom";
import { io } from "socket.io-client";

const socket = io("localhost:5000/", {
    withCredentials: true,
    cors: {
        origin: "http://localhost:3000/",
    },
}).connect();

export default function Navigation() {

    return(
        <Router basename="/" >
            <Routes>
                <Route path="/" element={<DashboardPage />}></Route>
                <Route path="/controller" element={<ControllerPage socket={socket}/>}></Route>
            </Routes>

        </Router>
  )
}
