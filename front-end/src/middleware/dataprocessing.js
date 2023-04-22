export function PieDataProcessing(data) {
    const totaltime = [0,0];
    var currenttimebedroom = 0;
    var currenttimelivingroom = 0;
    data.forEach((element) => {
        if (element[3] == 2 && element[2] == "on")
        {
            currenttimelivingroom = Date.parse(element[1]) - currenttimelivingroom;
            totaltime[0] += currenttimelivingroom;

        } else if ( element[3] == 1 && element[2] == "on")
        {
            currenttimebedroom = Date.parse(element[1]) - currenttimebedroom;
            totaltime[1] += currenttimebedroom;
        }
        else if ( element[3] == 2 && element[2] == "off")
        {
            currenttimelivingroom = 0;
        }

        else if ( element[3] == 1 && element[2] == "off"){
            currenttimebedroom = 0;
        }
    });

    return totaltime;
}

export function BarDataProcessing(data) {
    const result  =[0 ,0 ,0 , 0, 0, 0, 0]
    var currenttimebedroom = 0;
    var currenttimelivingroom = 0;

    data.forEach((element) => {
        const day = Date(element[0]).getDay();
        if (element[3] == 2 && element[2] == "on") {
            currenttimelivingroom = Date.parse(element[1]) - currenttimelivingroom;
            result[day] += currenttimelivingroom;

        } else if (element[3] == 1 && element[2] == "on") {
            currenttimebedroom = Date.parse(element[1]) - currenttimebedroom;
            result[day] += currenttimebedroom;
        }
        else if (element[3] == 2 && element[2] == "off") {
            currenttimelivingroom = 0;
        }

        else if (element[3] == 1 && element[2] == "off") {
            currenttimebedroom = 0;
        }
    });

    return result;
}


