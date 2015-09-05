//TODO: Do treatment data
//TODO: Do view by filters
//TODO: Do admin farm
//TODO: Add table dynamically
//TODO:Change starting date back

var dataObj;
var scoutStopObjects  = new Array();
var treatmentObjects;
var speciesAndStagesObjects = new Array();
var speciesNames = new Array();
var dataTables;

$(document).ready(function ()
{
    setFromDate();
    setToDate();
    getDataFromServer();
});

$("#blocks").change(function () {
    generate();
});

$("#timeFrom").change(function () {
    generate();
});

$("#timeto").change(function () {
    generate();
});

$("#species").change(function () {
    var optGroup = $("#species :selected").parent().attr("label");
    var option = $("#species :selected").val();

    if (option !== "all") {
        $(this).blur().find(":selected").text(optGroup + " - " + option);
    }

    generate();
});

$("#species").focus(function () {
    $(this).find("option").each(function () {
        var text = $(this).text().split(" - ");
        $(this).text(text[1]);

    });

});

$("#view").change(function () {
    generate();
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


function getDataFromServer() {
    $.get(url, function (data)
    {
        dataObj = data;
        transformDataToArrays();
    }, "json");
}

//TODO: check if dataObj is null
function transformDataToArrays() {
    var stop;
    var bug;
    var rowObject;
    var date;

    for (var i = 0; i < dataObj.ScoutStops.length; i++) {
        stop = dataObj.ScoutStops[i];

        for (var x = 0; x < stop.ScoutBugs.length; x++) {
            bug = stop.ScoutBugs[x];

            addToSpeciesArray(bug.SpeciesSpeciesName, bug.SpeciesLifestage);

            date = new XDate(stop.Date);
            rowObject =
            {
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

    setSpecies();
    generateFirst();
}

//TODO: change display value to show optgroup and option
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
}

function generateFirst() {
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

function generate() {
    var newData = filterData();
    console.log(newData);
    dataTables.clear().draw();
    dataTables.rows.add(newData);
    dataTables.columns.adjust().draw();
};

function filterData() {
    var block = $("#blocks").val();
    var fromDate = $("#timeFrom").val();
    var toDate = $("#timeTo").val();
    var speciesLifeStage = $("#species").val();
    var speciesName = $("#species :selected").parent().attr("label");

    console.log(speciesName);
    console.log(speciesLifeStage);

    var appliedFilters = scoutStopObjects.filter(function (obj) {
        console.log(obj.speciesName);
        console.log(obj.lifestage);
        if ((obj.blockName === block || block === "all") &&
        (obj.date >= fromDate && obj.date <= toDate) &&
        (speciesLifeStage === "all" || (obj.lifestage === speciesLifeStage && obj.speciesName === speciesName))){
            return true;
        }
    });

    return appliedFilters;
}
function setSpecies() {

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
    }








