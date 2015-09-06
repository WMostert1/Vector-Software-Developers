//Register delegates that interchange "collapse and expand icons"
$("#collapsibleTable").on("show.bs.collapse", function () {
    $("#collapseIcon").attr("class", "glyphicon glyphicon-collapse-down");
});

$("#collapsibleTable").on("hide.bs.collapse", function () {
    $("#collapseIcon").attr("class", "glyphicon glyphicon-expand");
});


//Register delegate that resets constraints to defaults
$("#resetIcon").on("click", resetConstraints);


//Defaults for view model
var view = $("#view").value,
    against = $("#against").value,
    constraints = new Array(),
    goupBy = $("#groupBy").value;

//AJAX Main Dataset
var dataSet;

var scoutStops;
var treatments;

setFromDate();
setToDate();

function resetConstraints() {
    setFromDate();
    setToDate();
    $("#constraintDateAny").prop("checked", false);
    $("#constraintBlocks").val("");
    $("#constraintBlocks").tagsinput("removeAll");
    $("#constraintSpecies").tagsinput("removeAll");
    $("#constraintLifeStage").tagsinput("removeAll");
    $("#constraintTreesLower").val("1");
    $("#constraintTreesUpper").val("10");
    $("#constraintTreesAny").prop("checked",true);
}

function setFromDate() {
    var date = new XDate().addMonths(-6, true);
    $("#constraintDateFrom").val(date.toString("yyyy-MM-dd"));
};

function setToDate() {
    var date = new XDate();
    $("#constraintDateTo").val(date.toString("yyyy-MM-dd"));
}

//function that will be triggered when a change was made to the filter values
function chartData(points, labels) {
    var x = new Chartist.Line("#chart", {
        series: [
            [
                { x: 1, y: 100 },
                { x: 2, y: 50 },
                { x: 3, y: 25 },
                { x: 5, y: 12.5 },
                { x: 8, y: 6.25 }
            ]
        ]
    }, {
        showPoint: false,
        axisX: {
            type: Chartist.AutoScaleAxis,
            onlyInteger: true
            //labelInterpolationFnc: function(value) {}
        }
    });
}

chartData();
