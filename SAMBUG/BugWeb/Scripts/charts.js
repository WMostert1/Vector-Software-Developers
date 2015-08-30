//chart settings as constants
var options = {
    
}



function loadData() {
    
}


//function that will be triggered when a change was made to the filter values
function chartData(points, labels) {
    new Chartist.Line("#chart", {
        series: [[
          { x: 1, y: 100 },
          { x: 2, y: 50 },
          { x: 3, y: 25 },
          { x: 5, y: 12.5 },
          { x: 8, y: 6.25 }
        ]]
    }, {
        axisX: {
            type: Chartist.AutoScaleAxis,
            onlyInteger: true
            //labelInterpolationFnc: function(value) {}
        }
    });
}

chartData();