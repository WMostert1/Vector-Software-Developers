/*
    Chart settings and other globals
*/

//TODO: remember to update chart settings before the first draw of the chart
var chartSettings = {};

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
    Bind event handler that responds to changes made in chart controls/constraints
*/
$(".chartControl").change(updateChartSettings);


/*
    Bind event handler that centers Y-Axis Title on window resize
*/
$(window).resize(centerYAxisTitle);




/*
    Initialisations
*/
initChartSettings();
loadSpeciesData();
loadDataRecords();


/*
    Function Definitions
*/
function initChartSettings() {
    setFromDate();
    setToDate();
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

function loadSpeciesData() {
    $.get(speciesUrl, function(data, status) {

        if (status === "error" || status === "timeout") {
            alert("Some information couldn't be retrieved from the server. Please check your connection and try again.");
            return;
        }

        var lifeStages = flattenSpecies("Lifestage", data);
        var speciesNames = flattenSpecies("SpeciesName", data);

        initSuggestions("constraintSpecies", speciesNames);
        initSuggestions("constraintLifeStage", lifeStages);

    }, "Json");
}

function loadDataRecords() {
    

    $.get(recordsUrl, function (data, status) {

        if (status === "error" || status === "timeout") {
            alert("Some information couldn't be retrieved from the server. Please check your connection and try again.");
            return;
        }
        console.log(data);
        treatments = data.Treatments;
        scoutStops = flattenScoutStops(data);

        var blockNames = extractBlocks(data);
        initSuggestions("constraintBlocks", blockNames);

        $("#chartContainer").toggleClass("chartContainerOnLoad");
        $("#chart").toggleClass("whirly-loader");
        $(".chartLabel").css("visibility", "visible");
        
        chartData();

    }, "Json");
}

function flattenScoutStops(data) {
    var result = [];

   /* $.each(data.ScoutStops, function (index, scoutStop) {
        $.each(scoutStop.ScoutBugs);
    });*/
}

function extractBlocks(data) {
    var result = [];

    $.each(data.ScoutStops, function (index, scoutStop) {
        if ($.inArray(scoutStop.BlockBlockName, result) === -1)
            result.push(scoutStop.BlockBlockName);
    });

    return result;
}

function flattenSpecies(flattenTo, data) {
    var result = [];

    $.each(data.Species, function (index, species) {
        if ($.inArray(species[flattenTo], result) === -1)
            result.push(species[flattenTo]);
    });

    return result;
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

function updateChart() {
    
}

function verifyChartSettings() {

}


function setFromDate() {
    $("#constraintDateFrom")
        .val(new XDate().addMonths(-6, true).toString("yyyy-MM-dd"));
};

function setToDate() {
    $("#constraintDateTo").val(new XDate().toString("yyyy-MM-dd"));
}






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



/*
    Event Handlers
*/
function updateChartSettings() {
    if ($(this).attr("type") === "checkbox")
        chartSettings[$(this).attr("id")] = $(this).prop("checked");
    else {
        chartSettings[$(this).attr("id")] = $(this).val();
    }

    console.log($(this).attr("id") + " - " + chartSettings[$(this).attr("id")]);

    try {
        verifyChartSettings();
        updateChart();
        updateTitles();
    } catch (e) {
        //e.name and e.message
    }
}

function resetConstraints() {
    setFromDate();
    setToDate();
    $("#constraintDateAny").prop("checked", false);
    $("#constraintDateAny").change();
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
