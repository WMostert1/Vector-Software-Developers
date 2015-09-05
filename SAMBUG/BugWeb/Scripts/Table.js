var dataObj;
var scoutStopObjects  = new Array();
var treatmentObjects;
var speciesAndStagesObjects = new Array();
var speciesNames = new Array();

$(document).ready(function ()
{
    setFromDate();
    setToDate();
    getDataFromServer();
});

function setFromDate() {
    var date = new XDate();
    var newDate = date.addMonths(-6, true);
    $("#timeFrom").val(newDate.toString("yyyy-MM-dd"));

};

function setToDate() {
    var date = new XDate();
    $("#timeTo").val(date.toString("yyyy-MM-dd"));
}


function getDataFromServer() {
    var url = document.getElementById("generateBut").getAttribute("data-url");

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

    for (var i = 0; i < dataObj.ScoutStops.length; i++) {
        stop = dataObj.ScoutStops[i];

        for (var x = 0; x < stop.ScoutBugs.length; x++) {
            bug = stop.ScoutBugs[x];

            addToSpeciesArray(bug.SpeciesSpeciesName, bug.SpeciesLifestage);

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

    setSpecies();
    generate();
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

function generate() {
    $('#table').DataTable({
        data: scoutStopObjects,
        columns: [
            { "title": "Block", data: 'blockName' },
            { "title": "Date", data: 'date' },
            { "title": "Number of Trees", data: 'numOfTrees' },
            { "title": "Bug Species", data: 'speciesName' },
            { "title": "Life stage", data: 'lifestage' },
            { "title": "Number of Bugs", data: 'numOfBugs' },
            { "title": "Comments", data: 'comment' }
        ]
    });
};


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








