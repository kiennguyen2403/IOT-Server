import { React, useEffect, useState } from "react";
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import { Line } from "react-chartjs-2";
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';


ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
);

const labels = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October','November','December'];
const defaultdata = {
    labels,
    datasets: [
        {
            label: 'Dataset 1',
            data: labels.map(() => 100),
            borderColor: 'rgb(255, 99, 132)',
            backgroundColor: 'rgba(255, 99, 132, 0.5)',
        },
        {
            label: 'Dataset 2',
            data: labels.map(() => 100),
            borderColor: 'rgb(53, 162, 235)',
            backgroundColor: 'rgba(53, 162, 235, 0.5)',
        },
    ],
};
export const options = {
    responsive: true,
    plugins: {
        legend: {
            position: 'top',
        },
        title: {
            display: true,
            text: 'Energy usage of each month',
        },
    },
};

export default function LineChart({ chartData }) {
    const [data, setData] = useState(defaultdata);

    useEffect(() => {
        if (chartData) {
            const data = {
                labels,
                datasets: [
                    {
                        label: 'Living room',
                        data: chartData[0].map((item) => item),
                        borderColor: 'rgb(255, 99, 132)',
                        backgroundColor: 'rgba(255, 99, 132, 0.5)',
                    },
                    {
                        label: 'Bedroom',
                        data: chartData[1].map((item) => item),
                        borderColor: 'rgb(53, 162, 235)',
                        backgroundColor: 'rgba(53, 162, 235, 0.5)',
                    },
                ],
            };
            setData(data);
        }
    }, [chartData]);
    return (
        <Card>
            <CardContent>
                <Line options={options} data={data} width={"30%"} height={"30%"} />
            </CardContent>
        </Card>
    );
}
