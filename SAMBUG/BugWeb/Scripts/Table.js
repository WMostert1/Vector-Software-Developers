var dataObj;
var scoutStopObjects  = new Array();
var treatmentObjects;
var speciesObjects;

$(document).ready(function ()
{
    setFromDate();
    setToDate();
    getDataFromServer();
});

function getDataFromServer() {
    var url = document.getElementById("generateBut").getAttribute("data-url");

    $.get(url, function (data)
    {
        dataObj = data;
        transform();
    }, "json");
}


function generate() {
    $('#table').DataTable({
        data: scoutStopObjects,
        columns: [
            {"title": "Block", data:'blockName'},
            {"title": "Date", data: 'date' },
            {"title": "Number of Trees", data: 'numOfTrees' },
            {"title": "Bug Species", data: 'speciesName' },
            {"title": "Life stage", data: 'lifestage' },
            {"title": "Number of Bugs", data: 'numOfBugs' },
            {"title": "Comments", data: 'comment' }
        ]
    });
};

//TODO: check if dataObj is null
//TODO: put in species
function transform() {
    var stop;
    var bug;
    var rowObject;

    for (var i = 0; i < dataObj.ScoutStops.length; i++) {
        stop = dataObj.ScoutStops[i];

        console.log(stop.ScoutBugs.length);
        for (var x = 0; x < stop.ScoutBugs.length; x++) {
            bug = stop.ScoutBugs[x];

            rowObject =
            {
                blockName: stop.BlockBlockName,
                date: stop.Date,
                numOfTrees: stop.NumberOfTrees,
                speciesName: bug.SpeciesSpeciesName,
                lifestage: bug.SpeciesLifestage,
                numOfBugs: bug.NumberOfBugs,
                comment: bug.Comments
            };

            scoutStopObjects.push(rowObject);
        }
    }

    generate();
}

function setFromDate() {
    var date = new XDate();
    var newDate = date.addMonths(-6, true);
    $("#timeFrom").val(newDate.toString("yyyy-MM-dd"));

};

function setToDate() {
    var date = new XDate();
    $("#timeTo").val(date.toString("yyyy-MM-dd"));
}







