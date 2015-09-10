/*
    Chart settings and other globals
*/
var chartSettings = {};

var viewModelToMemberMap = {
    None: "none",
    Date: "date",
    Block: "blockName",
    Species: "speciesName",
    "Bugs Per Tree": "bugsPerTree",
    "Species Life Stage": "lifeStage"
}

var species;
var scoutStops;
var treatments;


/*
    Bind event handlers that interchange "collapse and expand icons"
*/
$("#collapsibleTable").on("show.bs.collapse", function() {
    $("#collapseIcon").attr("class", "glyphicon glyphicon-collapse-down");
});

$("#collapsibleTable").on("hide.bs.collapse", function() {
    $("#collapseIcon").attr("class", "glyphicon glyphicon-expand");
});


/*
    Bind event handler that resets constraints to defaults
*/
$("#resetIcon").on("click", resetConstraints);


/*
    Bind event handlers that respond to changes made in chart controls/constraints
*/
$(".chartControl").change(updateChartSettings);

$("#constraintTreesLower").change(verifyTreesUpper);
$("#constraintTreesUpper").change(verifyTreesLower);
$("#constraintDateFrom").change(verifyToDate);
$("#constraintDateTo").change(verifyFromDate);
$("#constraintTreesUpper, #constraintTreesLower").keypress(verifyTreesInput);


/*
    Bind event handler that centers Y-Axis Title on window resize
*/
$(window).resize(centerYAxisTitle);


/*
    Initialisations
*/
initFromDate();
initToDate();
loadPests();
loadDataRecords();


/*
    Function Definitions
*/
function loadPests() {
    $.get(pestsUrl, function(data, status) {

        if (status === "error" || status === "timeout") {
            alert("Some information couldn't be retrieved from the server. Please check your connection and try again.");
            return;
        }

        species = $.map(data.Species, function(s) {
            s.Lifestage = mapSpeciesLifeStage(s.Lifestage);
            return s;
        });

        var speciesNames = getUniqueEntries("SpeciesName", data.Species);
        initSuggestions("constraintSpecies", speciesNames);

        //initialise suggestions to be all available lifestages
        updateSuggestions("constraintSpecies", "constraintLifeStage", species, "SpeciesName", "Lifestage");

        $("#constraintSpecies").change(function() {
            updateSuggestions("constraintSpecies", "constraintLifeStage", species, "SpeciesName", "Lifestage");
        });

    }, "Json");
}

function loadDataRecords() {
    $.get(recordsUrl, function(data, status) {

        if (status === "error" || status === "timeout") {
            alert("Some information couldn't be retrieved from the server. Please check your connection and try again.");
            return;
        }

        treatments = flattenTreatments(data.Treatments);
        scoutStops = flattenScoutStops(data.ScoutStops);

        var farmNames = getUniqueEntries("farmName", scoutStops.concat(treatments));

        initSuggestions("constraintFarms", farmNames);

        //initialise suggestions to be all available blocks
        updateSuggestions("constraintFarms", "constraintBlocks", scoutStops.concat(treatments), "farmName", "blockName");
        
        $("#constraintFarms").change(function() {
            updateSuggestions("constraintFarms", "constraintBlocks", scoutStops.concat(treatments), "farmName", "blockName");
        });

        //disable loader, update chart settings, finally plot 
        initChart();

    }, "Json");
}

function mapSpeciesLifeStage(code) {
    if (code === "0") {
        return "Adult";
    } else {
        return "Instar " + code;
    }
}

function getUniqueEntries(flattenTo, data) {
    var result = [];

    $.each(data, function(index, object) {
        if ($.inArray(object[flattenTo], result) === -1)
            result.push(object[flattenTo]);
    });

    return result;
}

function flattenTreatments(data) {
    return $.map(data, function(treatment) {
        return {
            farmName: treatment.BlockFarmFarmName,
            blockName: treatment.BlockBlockName,
            comments: treatment.Comments,
            date: new Date(treatment.Date)
        };
    });
}

function flattenScoutStops(data) {
    var result = [];

    $.each(data, function(index, scoutStop) {
        result = result.concat($.map(scoutStop.ScoutBugs, function(b) {
            return {
                farmName: scoutStop.BlockFarmFarmName,
                blockName: scoutStop.BlockBlockName,
                date: new Date(scoutStop.Date),
                numberOfTrees: scoutStop.NumberOfTrees,
                speciesName: b.SpeciesSpeciesName,
                lifeStage: mapSpeciesLifeStage(b.SpeciesLifestage),
                bugsPerTree: b.NumberOfBugs / scoutStop.NumberOfTrees,
                numberOfBugs: b.NumberOfBugs,
                comments: b.Comments
            };
        }));
    });

    return result;
}

function filterScoutStops(stops, settings) {
    var taggedFarmNames = settings.constraintFarms.split(",");
    var taggedBlockNames = settings.constraintBlocks.split(",");
    var taggedSpeciesNames = settings.constraintSpecies.split(",");
    var taggedLifeStages = settings.constraintLifeStage.split(",");

    return $.grep(stops, function (s) {
       return (settings.constraintDateAny ||
        (s.date.valueOf() >= settings.constraintDateFrom.valueOf() &&
            s.date.valueOf() <= settings.constraintDateTo.valueOf())) &&
        (taggedFarmNames[0] === "" || //if no farms are tagged, select all
            $.inArray(s.farmName, taggedFarmNames) >= 0) &&
        (taggedBlockNames[0] === "" || //ditto
            $.inArray(s.blockName, taggedBlockNames) >= 0) &&
        (taggedSpeciesNames[0] === "" ||
            $.inArray(s.speciesName, taggedSpeciesNames) >= 0) &&
        (taggedLifeStages[0] === "" ||
            $.inArray(s.lifeStage, taggedLifeStages) >= 0) &&
        (settings.constraintTreesAny ||
        (s.numberOfTrees >= settings.constraintTreesLower &&
            s.numberOfTrees <= settings.constraintTreesUpper));
    });
}

function filterTreatments(tr, settings) {
    var taggedFarmNames = settings.constraintFarms.split(",");
    var taggedBlockNames = settings.constraintBlocks.split(",");

    return $.grep(tr, function (t) {
        return (settings.constraintDateAny ||
        (t.date.valueOf() >= settings.constraintDateFrom.valueOf() &&
            t.date.valueOf() <= settings.constraintDateTo.valueOf())) &&
        (taggedFarmNames[0] === "" || //if no farms are tagged, select all
            $.inArray(t.farmName, taggedFarmNames) >= 0) &&
        (taggedBlockNames[0] === "" || //ditto
            $.inArray(t.blockName, taggedBlockNames) >= 0);

    });
}

function initSuggestions(id, data) {
    // constructs the suggestion engine
    var suggestions = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.whitespace,
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        local: data
    });

    $("#" + id).tagsinput({
        typeaheadjs: {
            source: suggestions
        },
        freeInput: false
    });
}


function initChart() {
    $("#chartAndLabelContainer").toggleClass("chartAndLabelContainerOnLoad");
    $("#chartContainer").toggleClass("whirly-loader");
    $(".chartLabel").css("visibility", "visible");


    chartSettings.view = $("#view").val();
    chartSettings.against = $("#against").val();
    chartSettings.grouping = $("#grouping").val();
    chartSettings.constraintShowTreatments = $("#constraintShowTreatments").prop("checked");
    chartSettings.constraintDateFrom = new Date($("#constraintDateFrom").val());
    chartSettings.constraintDateTo = new Date($("#constraintDateTo").val());
    chartSettings.constraintDateAny = $("#constraintDateAny").prop("checked");
    chartSettings.constraintFarms = $("#constraintFarms").val();
    chartSettings.constraintBlocks = $("#constraintBlocks").val();
    chartSettings.constraintSpecies = $("#constraintSpecies").val();
    chartSettings.constraintLifeStage = $("#constraintLifeStage").val();
    chartSettings.constraintTreesLower = parseInt($("#constraintTreesLower").val());
    chartSettings.constraintTreesUpper = parseInt($("#constraintTreesUpper").val());
    chartSettings.constraintTreesAny = $("#constraintTreesAny").prop("checked");

    updateChart(chartSettings);
}

function updateTitles() {
    $("#chartTitle h1").text(
        $("#view").val() + " vs " + $("#against").val()
    );

    $("#yAxisTitle h4").text(
        $("#view").val()
    );

    $("#xAxisTitle h4").text(
        $("#against").val()
    );

    centerYAxisTitle();
}

function updateChartSettings() {
    
    //save data in the correct format depending on the type of input
    switch ($(this).attr("type")) {
    case "checkbox":
        chartSettings[$(this).attr("id")] = $(this).prop("checked");
        break;
    case "date":
        chartSettings[$(this).attr("id")] = new Date($(this).val());
        break;
    case "number":
        chartSettings[$(this).attr("id")] = parseInt($(this).val());
        break;
    default:
        chartSettings[$(this).attr("id")] = $(this).val();
        break;
    }

    updateChart(chartSettings);
}

function initFromDate() {
    $("#constraintDateFrom")
        .val(new XDate().addMonths(-6, true).toString("yyyy-MM-dd"));
};

function initToDate() {
    $("#constraintDateTo").val(new XDate().toString("yyyy-MM-dd"));
}

function plot(filteredScoutStops, filteredTreatments, settings) {
    //todo: must check how labels are going to work, do that here

    var plotParameters = determinePlotParameters(settings, filteredScoutStops);

    //get categorised data ( or unaltered data if no grouping is applied)
    var categorisedStops = groupBy(settings.grouping, filteredScoutStops);

    var x = new Chartist.Line("#chart", {
        //data  
        series: [
            {
                name: "FirstSeries",
                data: [
                    { x: 5},
                    { x: 6}
                ]
            }
        ]
    }, {
//options
        //seriesBarDistance: 20,
        showPoint: false,
        axisX: {
//X axis options
            type: Chartist.FixedScaleAxis,
            //type: Chartist.AutoScaleAxis,
            onlyInteger: true,
    //labelInterpolationFnc: interpolateLabelDate
},
        series: {
//series options
            FirstSeries: {
                linesmooth: false
            }
        }
    });
}

function getBugsPerTree(stop) {
    return stop.numberOfBugs / stop.numberOfTrees;
}

function isCategorical(setting) {
    return (setting !== "Date");
}

function groupBy(setting, stops) {
    //define a mapping between the setting's name 
    //  and the member name in stops
    var memberName = viewModelToMemberMap[setting];

    //note that the first element of each subarray is used to store the category name
    var categorisedStops = [];

    //don't categorise the stops
    if (memberName === "none") {
        stops.unshift(memberName);
        categorisedStops.push(stops);
        return categorisedStops;
    }

    //acquire unique entries to form categories
    var categories = getUniqueEntries(memberName, stops);

    $.each(categories, function (i, c) {
        var category = $.grep(stops, function(s) {
            return s[memberName] === c;
        });

        category.unshift(c);
        categorisedStops.push(category);
    });

    return categorisedStops;
}

function determinePlotParameters(settings, categorisedStops) {
    //mapping to x member
    var xMemberName = viewModelToMemberMap[settings.against];
    
    //mapping to y member
    var yMemberName = viewModelToMemberMap[settings.view];

    //determine chartType
    var chartType = isCategorical(settings.against) ? "Bar" : "Line";

    var parameters = {};
    var labels = [];
    var series = [];

    //for each array of stops in catStops
    //  create a Series object that will be consumed by chartist
    //  ie {name: ...., data: [{x:...,y:...},...]}
    $.each(categorisedStops, function(i, c) {

    });
}

function interpolateLabelDate(value) {
    return new XDate(value).toString("dd MMM yy");
}


/*
    Event Handlers
*/
function verifyTreesUpper() {
    if (parseInt($("#constraintTreesUpper").val()) < parseInt($(this).val())) {
        $("#constraintTreesUpper").val($(this).val());
        $("#constraintTreesUpper").change();
    }
}

function verifyTreesLower() {
    if (parseInt($("#constraintTreesLower").val()) > parseInt($(this).val())) {
        $("#constraintTreesLower").val($(this).val());
        $("#constraintTreesLower").change();
    }
}

function verifyToDate() {
    if ((new XDate($("#constraintDateTo").val())).diffMilliseconds($(this).val()) > 0)
        $("#constraintDateTo").val($(this).val());
}

function verifyFromDate() {
    if ((new XDate($("#constraintDateFrom").val())).diffMilliseconds($(this).val()) < 0)
        $("#constraintDateFrom").val($(this).val());
}

function verifyTreesInput(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function updateChart(settings) {
    console.log(filterScoutStops(scoutStops, settings));
    var filteredScoutStops = filterScoutStops(scoutStops, settings);
    var filteredTreatments = [];

    if (settings.constraintShowTreatments)
        filteredTreatments = filterTreatments(treatments, settings);

    plot(filteredScoutStops, filteredTreatments, settings);

    updateTitles();
}

function resetConstraints() {
    initFromDate();
    initToDate();
    $("#constraintShowTreatments").prop("checked", false);
    $("#constraintDateAny").prop("checked", false);
    $("#constraintDateAny").change();
    $("#constraintFarms").tagsinput("removeAll");
    $("#constraintBlocks").tagsinput("removeAll");
    $("#constraintSpecies").tagsinput("removeAll");
    $("#constraintLifeStage").tagsinput("removeAll");
    $("#constraintTreesLower").val("1");
    $("#constraintTreesLower").change();
    $("#constraintTreesUpper").val("10");
    $("#constraintTreesUpper").change();
    $("#constraintTreesAny").prop("checked", true);
    $("#constraintTreesAny").change();
}

function centerYAxisTitle() {
    $("#yAxisTitle").css("top", 0.5 * $("#chartContainer").height() + $("#yAxisTitle").width() / 2 + "px");
}

function updateSuggestions(determinantId, subjectId, candidates, candidateMemberName, subject) {
    var taggedNames = $("#" + determinantId).val().split(",");

    var newSuggestions;

    if (taggedNames[0] !== "") {
        var taggedObjects = $(candidates).filter(function(index, object) {
            return $.inArray(object[candidateMemberName], taggedNames) >= 0;
        });

        newSuggestions = getUniqueEntries(subject, taggedObjects);
    } else {
        newSuggestions = getUniqueEntries(subject, candidates);
    }

    //remove any tags currently in subject, if any
    if ($("#" + subjectId).val().split(",")[0] !== "")
        $("#" + subjectId).tagsinput("removeAll");
    

    //remove current tagsinput behaviour, if any
    $("#" + subjectId).tagsinput();
    $("#" + subjectId).tagsinput("destroy");

    initSuggestions(subjectId, newSuggestions);

}