var scoutStopObjects = new Array();
var speciesObjects = new Array();
var dataTablesForScout;

$(document).ready(function () {
    $("#farmScout").change(function () {
        var farm = $("#farmScout").val();
        setBlocks(farm);
        generateScout();
    });

    $("#speciesScout").change(function () {
        var species = $("#speciesScout").val();
        setSpeciesInstar(species);
        generateScout();
    });

    $(".constraintScout").change(function () {
        generateScout();
    });

    $("#resetIconScout").on("click", resetScoutConstraints);

    $("#collapsibleTableScout").on("show.bs.collapse", function () {
        $("#collapseIconScout").attr("class", "glyphicon glyphicon-collapse-down");
    });

    $("#collapsibleTableScout").on("hide.bs.collapse", function () {
        $("#collapseIconScout").attr("class", "glyphicon glyphicon-expand");
    });
});

//TODO: check if dataObj is null
function transformToScoutStopArray(dataObj) {
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
                farmName: stop.BlockFarmFarmName,
                blockName: stop.BlockBlockName,
                date: date.toString("yyyy-MM-dd"),
                numOfTrees: (stop.NumberOfTrees).toString(),
                speciesName: bug.SpeciesSpeciesName,
                lifeStage: mapInstarToStringInstar(bug.SpeciesLifestage),
                numOfBugs: (bug.NumberOfBugs).toString(),
                comment: bug.Comments
            };

            scoutStopObjects.push(rowObject);
        }
    }
    generateFirstTimeScout();
}

//TODO: check if dataObj is empty
function transformToSpeciesArray(dataObj) {
    var species;
    var rowObject;

    for (var i = 0; i < dataObj.Species.length; i++) {
        species = dataObj.Species[i];
        rowObject =
        {
            speciesName: species.SpeciesName,
            lifeStage: species.Lifestage
        }

        speciesObjects.push(rowObject);
    }
}

function setSpecies() {
    var uniqueArray = flattenDataArray("speciesName", speciesObjects);
    for (var x = 0; x < uniqueArray.length; x++) {
        $("#speciesScout").append(new Option(uniqueArray[x], uniqueArray[x]));
    }
};

function setSpeciesInstar(species) {
    $("#speciesStageScout").children().remove();
    $("#speciesStageScout").append(new Option("All Instars", "all"));
    var array;
    if (species !== "all") {
        array = speciesObjects.filter(function (obj) {
            if (obj.speciesName === species) {
                return true;
            }
        });
    }
    else {
        array = speciesObjects;
    }

    array = flattenDataArray("lifeStage", array);

    for (var x = 0; x < array.length; x++) {
        $("#speciesStageScout").append(new Option(mapInstarToStringInstar(array[x]), mapInstarToStringInstar(array[x])));
    }
};

function setBlocksScout(farm) {
    $("#blocksScout").children().remove();
    $("#blocksScout").append(new Option("All Blocks", "all"));
    var array;
    if (farm !== "all") {
        array = scoutStopObjects.filter(function (obj) {
            if (obj.farmName === farm) {
                return true;
            }
        });
    } else {
        {
            array = scoutStopObjects;
        }
    }

    var uniqueArray = flattenDataArray("blockName", array);

    for (var x = 0; x < uniqueArray.length; x++) {
        $("#blocksScout").append(new Option(uniqueArray[x], uniqueArray[x]));
    }
};

function generateFirstTimeScout() {
    $("#scoutTableDiv").toggleClass("whirly-loader");
    $("#scoutTableDiv").css("margin-top", "30px");
    var data = filterDataScout();
    var table = $(document.createElement("table"));
    table.attr("id", "scoutTable");
/*    table.attr("class", "row-border");*/
    table.attr("class", "display");
    table.attr("style", "border: 1px solid #D8D8D8");
    $("#scoutTableDiv").append(table);
    dataTablesForScout = $("#scoutTable").DataTable({
        searching: false,
        lengthChange: true,
        pageLength: 25,
        data: data,
        dom: "<'row'<'col-sm-10'l><'col-sm-2'B>>" + "<'row'<'col-sm-12't>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",

        buttons: [
            {
                "text": "",
                "extend": "excelHtml5",
                "title": "ScoutData" + "_" + new XDate().toString("yyyy-MM-dd"),
            },
            {
                "text": "",
                "extend": "pdfHtml5",
                "title": "ScoutData" + "_" + new XDate().toString("yyyy-MM-dd")

            },
            {
                "extend": "print",
                "text": "",
                "title": "ScoutData" + "_" + new XDate().toString("yyyy-MM-dd")
            }
        ],

        columns: [
            { "title": "Farm", data: "farmName", "width": "10%"},
            { "title": "Block", data: "blockName", "width": "10%"},
            { "title": "Date", data: "date", "width": "10%" },
            { "title": "Tree Count", data: "numOfTrees", "width": "4%"},
            { "title": "Bug Species", data: "speciesName", "width": "15%"},
            { "title": "Life stage", data: "lifeStage", "width": "10%" },
            { "title": "Bug Count", data: "numOfBugs", "width": "4%" },
            { "title": "Comments", data: "comment", "width": "33%" }
        ]

    });
};

function generateScout() {
    var newData = filterDataScout();
    dataTablesForScout.clear().draw();
    dataTablesForScout.rows.add(newData);
    dataTablesForScout.draw();
};

function filterDataScout()
{
    var farm = $("#farmScout").val();
    var block = $("#blocksScout").val();
    var fromDate = new Date($("#timeFromScout").val());
    var toDate = new Date($("#timeToScout").val());
    var speciesLifeStage = $("#speciesStageScout").val();
    var speciesName = $("#speciesScout").val();

    console.log(fromDate);
    console.log(toDate);

    var appliedFilters = scoutStopObjects.filter(function (obj) {
        if ((obj.farmName === farm || farm === "all") &&
        (obj.blockName === block || block === "all") &&
        ((new Date(obj.date) >= fromDate && new Date(obj.date) <= toDate) || $("#dateAnyScout").is(":checked")) &&
        (speciesLifeStage === "all" || (obj.lifeStage).toString() === speciesLifeStage) &&
        (speciesName === "all" || obj.speciesName === speciesName)) {
            return true;
        }
    });

    return appliedFilters;
};

function mapInstarToStringInstar(instar) {
    if (instar === 0)
        return "Adult";

    return "Instar " + instar;
}

function resetScoutConstraints() {
    $("#farmScout").val("all");
    $("#blocksScout").val("all");
    $("#speciesScout").val("all");
    $("#speciesStageScout").val("all");
    setFromDate();
    setToDate();
    $("#dateAnyScout").prop("checked", true);
}