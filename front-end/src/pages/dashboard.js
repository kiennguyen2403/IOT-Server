import { React, useState, useEffect } from "react";
import ResponsiveAppBar from "../components/header";
import Typography from '@mui/material/Typography';
import Chart from "chart.js/auto";
import { CategoryScale } from "chart.js";
import Donut from "../components/donut";
import CustomBar from "../components/bar";
import { Data } from "../mockdata/data";
import Grid from '@mui/material/Grid';
import axios from 'axios'
import { PieDataProcessing, BarDataProcessing } from "../middleware/dataprocessing";
import LineChart from "../components/linechart";


export default function DashboardPage() {
    const [donutChart, setDonutChart] = useState(null);
    const [barChart, setBarChart] = useState(null);
    Chart.register(CategoryScale);


    const getData = async () =>{
        const response = await axios.get('http://localhost:5000/leds');
        setDonutChart(PieDataProcessing(response.data));
        setBarChart(BarDataProcessing(response.data));
    }
    
    useEffect(() => {
        getData()
    }, []);


    return(
        <div >
            <ResponsiveAppBar />
            <div style ={{marginTop: 40}}>
            <Typography variant="h4" component="div">
                Dashboard
            </Typography>
            <Grid container spacing={2} >
                <Grid item xs={4}>
                    <Donut chartData={donutChart}/>
                </Grid>
                <Grid item xs={4}>
                    <CustomBar chartData={barChart}/>
                </Grid>
                <Grid item xs={4}>
                    <LineChart chartData={barChart}/>
                </Grid>
            </Grid>
            </div>
        </div>
    );
}