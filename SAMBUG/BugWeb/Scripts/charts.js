/*
    Chart settings and other globals
*/

//TODO: remember to update chart settings before the first draw of the chart
var chartSettings = {};

var species;
var scoutStops;
var treatments;



/*
    Bind event handlers that interchange "collapse and expand icons"
*/
$("#collapsibleTable").on("show.bs.collapse", function () {
    $("#collapseIcon").attr("class", "glyphicon glyphicon-collapse-down");
});

$("#collapsibleTable").on("hide.bs.collapse", function () {
    $("#collapseIcon").attr("class", "glyphicon glyphicon-expand");
});


/*
    Bind event handler that resets constraints to defaults
*/
$("#resetIcon").on("click", resetConstraints);


/*
    Bind event handlers that respond to changes made in chart controls/constraints
*/
$(".chartControl").change(updateChart);
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

        species = data.Species;

        var speciesNames = flattenDataArray("SpeciesName", data.Species);
        initSuggestions("constraintSpecies", speciesNames);

        $("#constraintSpecies").change(function() {
            updateSuggestions("constraintSpecies", "constraintLifeStage", species, "SpeciesName", "Lifestage");
        });

        //function updateSuggestions(determinantId, subjectId, candidates, candidateMemberName, subject)
        
    }, "Json");
}

function loadDataRecords() {
    $.get(recordsUrl, function (data, status) {

        if (status === "error" || status === "timeout") {
            alert("Some information couldn't be retrieved from the server. Please check your connection and try again.");
            return;
        }

        treatments = data.Treatments;
        scoutStops = flattenScoutStops(data.ScoutStops);

        var farmNames = flattenDataArray("BlockFarmFarmName", scoutStops.concat(treatments));

        initSuggestions("constraintFarms", farmNames);

       $("#constraintFarms").change(function() {
           updateSuggestions("constraintFarms", "constraintBlocks", scoutStops.concat(treatments), "BlockFarmFarmName", "BlockBlockName");
       });

        //disable css spinner, force-update chart settings, finally plot 
        initChart();

    }, "Json");
}

function flattenDataArray(flattenTo, data) {
    var result = [];

    $.each(data, function (index, object) {
        if ($.inArray(object[flattenTo], result) === -1)
            result.push(object[flattenTo]);
    });

    return result;
}

function flattenScoutStops(data) {
    var result = [];

   /* $.each(data.ScoutStops, function (index, scoutStop) {
        $.each(scoutStop.ScoutBugs);
    });*/
    return data;
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
    $(".chartControl").change();
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

function updateChartSettings(element) {
    if ($(element).attr("type") === "checkbox")
        chartSettings[$(element).attr("id")] = $(element).prop("checked");
    else {
        chartSettings[$(element).attr("id")] = $(element).val();
    }

    console.log($(element).attr("id") + " - " + chartSettings[$(element).attr("id")]);
}

function initFromDate() {
    $("#constraintDateFrom")
        .val(new XDate().addMonths(-6, true).toString("yyyy-MM-dd"));
};

function initToDate() {
    $("#constraintDateTo").val(new XDate().toString("yyyy-MM-dd"));
}

function plot(points, labels) {
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



/*
    Event Handlers
*/
function verifyTreesUpper() {
    if (parseInt($("#constraintTreesUpper").val()) < parseInt($(this).val()))
        $("#constraintTreesUpper").val($(this).val());
}

function verifyTreesLower() {
    if (parseInt($("#constraintTreesLower").val()) > parseInt($(this).val()))
        $("#constraintTreesLower").val($(this).val());
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

function updateChart() {
    updateChartSettings(this);
    plot();
    updateTitles();
}

function resetConstraints() {
    initFromDate();
    initToDate();
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
    $("#yAxisTitle").css("top", 0.5 * $("#chartContainer").height() + "px");
}

function updateSuggestions(determinantId, subjectId, candidates, candidateMemberName, subject) {
    var taggedNames = $("#" + determinantId).val().split(",");

    var newSuggestions = "";

    if (taggedNames[0] !== "") {
        var taggedObjects = $(candidates).filter(function (index, object) {
            return $.inArray(object[candidateMemberName], taggedNames) >= 0;
        });

        newSuggestions = flattenDataArray(subject, taggedObjects);
    }

    //remove any tags currently in subject
    $("#" + subjectId).tagsinput("removeAll");

    //remove current tagsinput behaviour, if any
    $("#" + subjectId).tagsinput("destroy");

    initSuggestions(subjectId, newSuggestions);

}