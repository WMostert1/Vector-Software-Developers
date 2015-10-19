//------------------Set all input to defualt values, initially and throughout execution------------------------
function setFarms() {
    var uniqueArray = flattenDataArray("farmName", scoutStopObjects);
    for (var x = 0; x < uniqueArray.length; x++) {
        $(".farms").append(new Option(uniqueArray[x], uniqueArray[x]));
    }
};

function getCapturedDataFromServer() {
    $.get(recordsUrl, function (data) {
        transformToScoutStopArray(data);
        setFarms();
        setBlocks("all");

    }, "json");
};



function setBlocks(blocks) {
    setBlocksScout(blocks);
}

function getSpeciesFromServer() {
    $.get(speciesUrl, function (data) {
        transformToSpeciesArray(data);
        setSpecies();
        setSpeciesInstar("all");
    }, "json");
};



//----------------------------Other helper functions-----------------------------------------------------------
function flattenDataArray(flattenTo, data) {
    var result = [];

    $.each(data, function (index, object) {
        if ($.inArray(object[flattenTo], result) === -1)
            result.push(object[flattenTo]);
    });

    return result;
}

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
                comment: bug.Comments,
                Latitude: stop.Latitude,
                Longitude : stop.Longitude
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
    var data = filterDataScout();
    generateMap(data);
    //init map
    
};

function generateScout() {
    var newData = filterDataScout();
    generateMap(newData);
    //update map
};

function filterDataScout() {
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

//MAP INITIALIZATION

var map, heatmap;

function initMap() {
    //---------------------------Get data from server, the onchange events is also here---------------------------
    $(document).ready(function () {
        setFromDate();
        setToDate();
        getCapturedDataFromServer();
        getSpeciesFromServer();

    });
}

function generateMap(stops) {
    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 13,
       
        center: { lat: stops[0].Latitude, lng: stops[0].Longitude },
        mapTypeId: google.maps.MapTypeId.SATELLITE
    });

    heatmap = new google.maps.visualization.HeatmapLayer({
        data: getPoints(stops),
        map: map
    });

     changeRadius(30);

}

function getPoints(stops) {
    var mapsArr = [];
   
    for (var i = 0; i < stops.length; i++) {
        
        mapsArr.push({ location: new google.maps.LatLng(stops[i].Latitude, stops[i].Longitude), weight: stops.numOfBugs });
        
    }

    return mapsArr;

}


function changeGradient() {
    var gradient = [
        "rgba(0, 255, 255, 0)",
        "rgba(0, 255, 255, 1)",
        "rgba(0, 191, 255, 1)",
        "rgba(0, 127, 255, 1)",
        "rgba(0, 63, 255, 1)",
        "rgba(0, 0, 255, 1)",
        "rgba(0, 0, 223, 1)",
        "rgba(0, 0, 191, 1)",
        "rgba(0, 0, 159, 1)",
        "rgba(0, 0, 127, 1)",
        "rgba(63, 0, 91, 1)",
        "rgba(127, 0, 63, 1)",
        "rgba(191, 0, 31, 1)",
        "rgba(255, 0, 0, 1)"
    ];

    heatmap.set("gradient", heatmap.get("gradient") ? null : gradient);
  
}

function changeRadius(radius) {
    if (heatmap.get("radius"))
        heatmap.set("radius",null);

    heatmap.set("radius", radius);
}


/*$("#radius_slider").change(function () {
 
    $("#radius_label").text("Radius : " + $(this).val());
    changeRadius($(this).val());
});*/



