//todo: check licence requirements of all borrowed code and assets

/*
    Chart settings and other globals
*/
var chartSettings = {};

var viewModelToMemberMap = {
    None: "All Data",
    Date: "date",
    Block: "blockName",
    Species: "speciesName",
    "Bugs per Tree": "bugsPerTree",//this is a quasi-member
    "Number of Bugs": "numberOfBugs",
    "Number of Trees": "numberOfTrees",
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
$("#view").change(saveAggregateType);
$("#constraintTreesLower").change(verifyTreesUpper);
$("#constraintTreesUpper").change(verifyTreesLower);
$("#constraintDateFrom").change(verifyToDate);
$("#constraintDateTo").change(verifyFromDate);
$("#constraintTreesUpper, #constraintTreesLower").keypress(verifyTreesInput);
$(".chartControl").change(updateChartSettings);


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

       // console.log("Initial Data",scoutStops);
        
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

function getUniqueEntries(uniqueFor, data) {
    var result = [];

    $.each(data, function(index, object) {
        if ($.grep(result, function(d) {
            return object[uniqueFor].valueOf() === d.valueOf();
        }).length === 0)
            result.push(object[uniqueFor]);
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
                numberOfTrees: parseInt(scoutStop.NumberOfTrees),
                speciesName: b.SpeciesSpeciesName,
                lifeStage: mapSpeciesLifeStage(b.SpeciesLifestage),
                numberOfBugs: parseInt(b.NumberOfBugs),
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

    return $.grep(stops, function(s) {
        return (settings.constraintDateAny ||
        (s.date.valueOf() >= settings.constraintDateFrom.valueOf() &&
            s.date.valueOf() <= settings.constraintDateTo.valueOf())) &&
        (taggedFarmNames[0] === "" || $.inArray(s.farmName, taggedFarmNames) >= 0) &&
        (taggedBlockNames[0] === "" || $.inArray(s.blockName, taggedBlockNames) >= 0) &&
        (taggedSpeciesNames[0] === "" || $.inArray(s.speciesName, taggedSpeciesNames) >= 0) &&
        (taggedLifeStages[0] === "" || $.inArray(s.lifeStage, taggedLifeStages) >= 0) &&
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
        (taggedFarmNames[0] === "" || $.inArray(t.farmName, taggedFarmNames) >= 0) &&
        (taggedBlockNames[0] === "" || $.inArray(t.blockName, taggedBlockNames) >= 0);
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
    $("#chartContainerOnLoad").toggleClass("chartContainerOnLoad");
    $("#chartContainer").toggleClass("whirly-loader");
    $(".chartLabel").css("visibility", "visible");

    chartSettings.aggregateType = $("#view").find(":selected").parent().attr("label");
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

function updateChartTitle() {
    $("#chartTitle h1").text(
        chartSettings.aggregateType + " " + chartSettings.view + " vs " + chartSettings.against
    );
}

function initFromDate() {
    $("#constraintDateFrom")
        .val(new XDate().addMonths(-6, true).toString("yyyy-MM-dd"));
};

function initToDate() {
    $("#constraintDateTo").val(new XDate().toString("yyyy-MM-dd"));
}

function updateChart(settings) {
    var filteredScoutStops = filterScoutStops(scoutStops, settings);

    //console.log("Constrained Data", filteredScoutStops);

    var filteredTreatments = [];

    if (settings.constraintShowTreatments)
        filteredTreatments = filterTreatments(treatments, settings);

    updateChartTitle();
    plot(filteredScoutStops, filteredTreatments, settings);
}

function plot(filteredScoutStops, filteredTreatments, settings) {
    //todo: must check how labels are going to work, do that here

    //get categorised data (or unaltered data if no grouping is applied)
    var categorisedStops = groupBy(viewModelToMemberMap[settings.grouping], filteredScoutStops);

    //console.log("Grouped by " + settings.grouping, categorisedStops);

    var parameters = determinePlotParameters(settings, categorisedStops, filteredTreatments);

    //console.log("Plotting with this", JSON.stringify(parameters, null, 2));

    var chart = new Chartist[parameters.chartType]("#chart", parameters.data, parameters.options);

}

function determinePlotParameters(settings, categorisedStops, filteredTreatments) {
    //mapping to x member
    var xMemberName = viewModelToMemberMap[settings.against];
    
    //mapping to y member
    //note that this may map to the quasi-member: bugsPerTree
    //  for which there is no corresponding member variable in scoutStops
    var yMemberName = viewModelToMemberMap[settings.view];

    //determine chartType
    var chartType = isCategorical(settings.against) ? "Bar" : "Line";
   
    //Acquire various chart parameters
    var parameters = {};
    parameters.chartType = chartType;
    parameters.options = {}
    parameters.options.plugins = [];
    
    if (chartType === "Line") {
        saveLineChartParameters(parameters, xMemberName, yMemberName, settings.aggregateType, categorisedStops, filteredTreatments);
        if (settings.constraintShowTreatments) {
            addTreatments(parameters, filteredTreatments);
        }
    } else if (chartType === "Bar") {
        saveBarChartParamaters(parameters, xMemberName, yMemberName, settings.aggregateType, categorisedStops, filteredTreatments);
    }

    //Add options common to all charts

    //always start y axis at 0
    parameters.options.axisY.low = 0;

    //For all y data except Bugs per Tree, use integers
    if (yMemberName !== "bugsPerTree")
        parameters.options.axisY.onlyInteger = true;

    //Axis Title Plugin
    setAxesTitles(parameters.options.plugins, settings);

    //Tooltip Plugin
    parameters.options.plugins.push(Chartist.plugins.tooltip(parameters.options));

   return parameters;

}

function saveLineChartParameters(parameters, xMemberName, yMemberName, aggregateType, categorisedStops, filteredTreatments) {
    var seriesArray = [];
    
    $.each(categorisedStops, function(i, c) {
        var s = {};

        //series name is the first element of c
        s.name = c.shift();

        //group each x value with its corresponding y values, sorting x in ascending order
        var sortedXCategorisedPoints = groupBy(xMemberName, c).sort(sortByFirstElement);

       //console.log("Points for " + s.name, sortedXCategorisedPoints);

        s.data = getLineSeriesData(yMemberName, aggregateType, filteredTreatments, sortedXCategorisedPoints);

        seriesArray.push(s);
    });
    
    parameters.data = { series: seriesArray };

    parameters.options.showPoint = true;
    parameters.options.axisY = {};
    parameters.options.axisX = {
        type: Chartist.FixedScaleAxis,
        divisor: 10,
        labelInterpolationFnc: millisToDateString
    };

    parameters.options.tooltipFnc = getTooltipText;

    

}

function getLineSeriesData(yMember, aggregateType, filteredTreatments, sortedXCategorisedPoints) {
    var xy = [];
    
    $.each(sortedXCategorisedPoints, function (i, points) {
        var xValue = points.shift();
        
        //xAndYs from this point only includes ys
        var yAggregate = reduceToSingleY(yMember, aggregateType, points);
        
        xy.push({ x: xValue, y: yAggregate });

    });

    return xy;

}

function addTreatments(parameters, treatments) {
    var newSeries = {
        name: "Treatments",
        data: []
    }

    var vLinePositions = [];

    $.each(treatments, function (i, treatment) {
        newSeries.data.push({ x: treatment.date, y: -1 });
        vLinePositions.push(treatment.date.valueOf());
    });

    parameters.data.series.push(newSeries);
    
    parameters.options.plugins.push(Chartist.plugins.verticalLines({
        positions: vLinePositions
    }));

    parameters.options.series = {
        "Treatments": {
            showLine: false
        }
    };


}

function isCategorical(setting) {
    return (setting !== "Date");
}

function groupBy(memberName, stops) {
    //note that the first element of each subarray is used to store the category name
    var categorisedStops = [];

    //don't categorise the stops
    if (memberName === "All Data") {
        stops.unshift(memberName);
        categorisedStops.push(stops);
        return categorisedStops;
    }

    //acquire unique entries to form categories
    var categories = getUniqueEntries(memberName, stops);

    $.each(categories, function (i, c) {
        var category = $.grep(stops, function (s) {
            return s[memberName].valueOf() === c.valueOf();
        });

        category.unshift(c);
        categorisedStops.push(category);
    });

    return categorisedStops;
}

function reduceToSingleY(yMember, aggregateType, yList) {
    var bugs = 0, trees = 0;
    var yAggregate = 0.0;

    if (yMember === "bugsPerTree") {
        //only averages seem to make sense for bugsPerTree
        $.each(yList, function (j, y) {
            bugs += y.numberOfBugs;
            trees += y.numberOfTrees;
        });

        yAggregate = bugs / trees;
    } else {
        $.each(yList, function (j, y) {
            yAggregate += y[yMember];
        });

        if (aggregateType === "Average")
            yAggregate /= yList.length;
    }

    return yAggregate;
}

function saveBarChartParamaters(parameters, labelBy, yMemberName, aggregateType, categorisedStops) {
        
}

function sortByFirstElement(a, b) {
    if (a[0].valueOf() > b[0].valueOf()) {
        return 1;
    } else if (a[0].valueOf() < b[0].valueOf()) {
        return -1;
    }
    return 0;
}

function sortByPrimitiveValue(a, b) {
    if (a.valueOf() > b.valueOf()) {
        return 1;
    } else if (a.valueOf() < b.valueOf()) {
        return -1;
    }
    return 0;
}

function millisToDateString(value) {
    return new XDate(value).toString("dd MMM yy");
}

function setAxesTitles(plugins, settings) {
    plugins.push(Chartist.plugins.ctAxisTitle({
        axisX: {
            axisTitle: settings.against,
            axisClass: "ct-axis-title",
            offset: {
                x: 0,
                y: 35
            },
            textAnchor: "middle"
        },
        axisY: {
            axisTitle: settings.aggregateType + " " + settings.view,
            axisClass: "ct-axis-title",
            offset: {
                x: 0,
                y: 15
            },
            textAnchor: "middle",
            flipTitle: true
        }
    }));
}

function getTooltipText(meta, values, seriesName, classes) {
    if (classes.indexOf("ct-line") >= 0) {
        return seriesName;
    } else {
        var x = millisToDateString(parseInt(values.split(",")[0]));
        var y = parseFloat(values.split(",")[1]).toFixed(2).toString();
        return seriesName + "<br/>" + x + ", " + y;
    }
}



/*
    Event Handlers
*/
function saveAggregateType() {
    chartSettings.aggregateType = $(this).find(":selected").parent().attr("label");
}

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

    //ensure that data has arrived before attempting to chart the data
    if (scoutStops !== null) {
        updateChart(chartSettings);
    }
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