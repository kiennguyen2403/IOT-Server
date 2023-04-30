import {React, useState, useEffect} from 'react';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Bar } from 'react-chartjs-2';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';


ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

export const options = {
  responsive: true,
  plugins: {
    legend: {
      position: 'top',
    },
    title: {
      display: true,
      text: 'Energy usage in one week',
    },
  },
};

const labels = ['Sunday','Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

const defaultdata = {
  labels,
  datasets: [
    {
      label: 'Living room',
      data: labels.map(() => 100),
      backgroundColor: 'rgba(255, 99, 132, 0.5)',
    },
    {
      label: 'Bedroom',
      data: labels.map(() => 100),
      backgroundColor: 'rgba(53, 162, 235, 0.5)',
    },
  ],
};
export default function CustomBar ({ chartData }) {

  const [data, setData] = useState(defaultdata);

  useEffect(() => {
    if (chartData) {
      const data = {
        labels,
        datasets: [
          {
            label: 'Living room',
            data: chartData[0].map((item) => item),
            backgroundColor: 'rgba(255, 99, 132, 0.5)',
          },
          {
            label: 'Bedroom',
            data: chartData[1].map((item) => item),
            backgroundColor: 'rgba(53, 162, 235, 0.5)',
          },
        ],
      };
      setData(data);
    }
  }, [chartData]);
  
  return (
    <Card >
      <CardContent>
        <Bar options={options} data={data} width={"30%"} height={"30%"}/>
      </CardContent>   
    </Card>
  );
};