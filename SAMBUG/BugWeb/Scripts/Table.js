//TODO: Change starting date back
//TODO: Validate input
//TODO: Extract common between charts and tables and maps
//TODO: Test that dates work (change them to XDATE for comparing)
//TODO: take stripes away for constraints
//TODO: show abrie all plugins and look at changing the order stuff of columns
//TODO: fix whirly thing

//---------------------------Get data from server, the onchange events is also here---------------------------
$(document).ready(function ()
{
    $(".nav-tabs a").click(function () {
        $(this).tab("show");
    });
    setFromDate();
    setToDate();
    getCapturedDataFromServer();
    getSpeciesFromServer();
});

function getCapturedDataFromServer() {
    $.get(recordsUrl, function (data)
    {
        transformToScoutStopArray(data);
        transformToTreatmentArray(data);
        setFarms();
        setBlocks("all");
    }, "json");
};

function setBlocks(blocks) {
    setBlocksScout(blocks);
    setBlocksTreatment(blocks);
}

function getSpeciesFromServer() {
    $.get(speciesUrl, function (data)
    {
        transformToSpeciesArray(data);
        setSpecies();
        setSpeciesInstar("all");
    }, "json");
};

//------------------Set all input to defualt values, initially and throughout execution------------------------
function setFromDate() {
    var date = new XDate();
    var newDate = date.addYears(-10, true);
    $(".dateFrom").val(newDate.toString("yyyy-MM-dd"));
};

function setToDate() {
    var date = new XDate();
    $(".dateTo").val(date.toString("yyyy-MM-dd"));
};

function setFarms() {
    var uniqueArray = flattenDataArray("farmName", scoutStopObjects);
    for (var x = 0; x < uniqueArray.length; x++) {
        $(".farms").append(new Option(uniqueArray[x], uniqueArray[x]));
    }
};

//----------------------------Other helper functions-----------------------------------------------------------
function flattenDataArray(flattenTo, data) {
    var result = [];

    $.each(data, function(index, object) {
        if ($.inArray(object[flattenTo], result) === -1)
            result.push(object[flattenTo]);
    });

    return result;
}








