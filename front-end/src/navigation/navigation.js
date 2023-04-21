import React from "react";
import DashboardPage from "../pages/dashboard";
import ControllerPage from "../pages/controller";
import {
    BrowserRouter as Router,
    Routes, //replaces "Switch" used till v5
    Route,
} from "react-router-dom";



export default function Navigation() {
  return(
      <Router basename="/" >
          <Routes>
              <Route path="/" element={<DashboardPage />}></Route>
              <Route path="/controller" element={<ControllerPage />}></Route>
          </Routes>

      </Router>
  )
}
