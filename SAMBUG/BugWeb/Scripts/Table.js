//TODO: Do treatment data
//TODO: Do view by filter
//TODO: Do admin farm
//TODO: Add table dynamically
//TODO: Change starting date back
//TODO: Validate input
//TODO: Do csv
//TODO: Change to send farm
//TODO: extract common between charts and tables

var scoutStopObjects  = new Array();
var treatmentObjects;
var speciesNames = new Array();
var dataTables;

$(".constraint").change(function () {
    generate();
});

//TODO: might change the gact that we get data here
$(document).ready(function ()
{
    setFromDate();
    setToDate();
    getCapturedDataFromServer();
    getSpeciesFromServer();
});

function setFromDate() {
    var date = new XDate();
    var newDate = date.addYears(-10, true);
    $("#timeFrom").val(newDate.toString("yyyy-MM-dd"));
};

function setToDate() {
    var date = new XDate();
    $("#timeTo").val(date.toString("yyyy-MM-dd"));
}

function getCapturedDataFromServer() {
    $.get(url, function (data)
    {
        transformDataToArrays(data);
    }, "json");
}

function getSpeciesFromServer() {
    $.get(url, function (data) {
        transformDataToArrays(data);
    }, "json");
}

//TODO: check if dataObj is null
function transformDataToArrays(dataObj) {
    var stop;
    var bug;
    var rowObject;
    var date;

    for (var i = 0; i < dataObj.ScoutStops.length; i++) {
        stop = dataObj.ScoutStops[i];

        for (var x = 0; x < stop.ScoutBugs.length; x++) {
            bug = stop.ScoutBugs[x];

            date = new XDate(stop.Date);
            rowObject =
            {
                farmName: stop.BlockBlockFarmName,
                blockName: stop.BlockBlockName,
                date: date.toString("yyyy-MM-dd"),
                numOfTrees: stop.NumberOfTrees,
                speciesName: bug.SpeciesSpeciesName,
                lifestage: bug.SpeciesLifestage,
                numOfBugs: bug.NumberOfBugs,
                comment: bug.Comments
            };

            scoutStopObjects.push(rowObject);
        }
    }
    generateFirstTime();
}

function generateFirstTime() {
    var data = filterData();
    dataTables = $("#table").DataTable({
        data: data,
        columns: [
            { "title": "Block", data: "blockName" },
            { "title": "Date", data: "date" },
            { "title": "Number of Trees", data: "numOfTrees" },
            { "title": "Bug Species", data: "speciesName" },
            { "title": "Life stage", data: "lifestage" },
            { "title": "Number of Bugs", data: "numOfBugs" },
            { "title": "Comments", data: "comment" }
        ]
    });
};

//TODO: change to also do view by
function generate() {
    var newData = filterData();
    console.log(newData);
    dataTables.clear().draw();
    dataTables.rows.add(newData);
    dataTables.columns.adjust().draw();
};

function filterData() {
    var farm = $("#farm").val();
    var block = $("#blocks").val();
    var fromDate = $("#timeFrom").val();
    var toDate = $("#timeTo").val();
    var speciesLifeStage = $("#species").val();
    var speciesName = $("#species :selected").parent().attr("label");

    var appliedFilters = scoutStopObjects.filter(function (obj) {
        if ((obj.farmName === farm || farm === 'all') &&
        (obj.blockName === block || block === "all") &&
        ((obj.date >= fromDate && obj.date <= toDate) || (document.getElementById("dateAny").checked)) &&
        (speciesLifeStage === "all" || (obj.lifestage === speciesLifeStage && obj.speciesName === speciesName))){
            return true;
        }
    });

    return appliedFilters;
}

/*function setSpecies() {

    var filtered = new Array();
    var name;
    var optGroup;
    var option;
    var select = document.getElementById('species');;

    for (var i = 0; i < speciesNames.length; i++) {
        name = speciesNames[i];

        filtered = speciesAndStagesObjects.filter(function(obj) {
            if (obj.name === name)
                return true;
        });

        optGroup = document.createElement("OPTGROUP");

        optGroup.value = name;
        optGroup.innerHTML = name;
        optGroup.text = name;
        optGroup.innerText = name;
        optGroup.label = name;

        for (var j = 0; j < filtered.length; j++) {
            option = document.createElement("OPTION");
            option.value = filtered[j].stage;
            option.innerHTML = filtered[j].stage;
            optGroup.appendChild(option);
        }


        select.appendChild(optGroup);

        }
    }*/

/*//TODO: change display value to show optgroup and option
function addToSpeciesArray(species, lifestage) {

    var found = false;
    for (var i = 0; i < speciesAndStagesObjects.length; i++) {
        var speciesObj = speciesAndStagesObjects[i];

        if (speciesObj.name === species && speciesObj.stage === lifestage) {
            found = true;
        }
    }

    if (found === false) {
        var toAddObject =
        {
            name: species,
            stage: lifestage
        };

        speciesAndStagesObjects.push(toAddObject);
        speciesNames.push(species);
        speciesNames = _.uniq(speciesNames);
    }
}*/








