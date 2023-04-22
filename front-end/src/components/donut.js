import {React, useEffect, useState} from "react";
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import { Doughnut } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';

ChartJS.register(ArcElement, Tooltip, Legend);
const defaultdata = {
  labels: ['Living room', 'Bedroom'],
  datasets: [
    {
      label: '# of Energy Usage',
      data: [19,5],
      backgroundColor: [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',

      ],
      borderColor: [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
      ],
      borderWidth: 1,
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
      text: 'Energy usage of each room',
    },
  },
};

export default function Donut({ chartData }) {
  const [data, setData] = useState(defaultdata);

  useEffect(() => {
    const data = {
      labels: ['Living room', 'Bedroom'],
      datasets: [
        {
          label: '# of Energy Usage',
          data: chartData,
          backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',

          ],
          borderColor: [
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
          ],
          borderWidth: 1,
        },
      ],
    };

    setData(data);
  }, [chartData]);
  return (
    <Card>
      <CardContent>
        <Doughnut data={data} options={options}/>
      </CardContent>    
    </Card>
  );
}
