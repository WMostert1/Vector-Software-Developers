﻿var treatmentObjects = new Array();
var dataTablesForTreatment;

$(document).ready(function () {
    $("#farmTreatment").change(function () {
        var farm = $("#farmTreatment").val();
        setBlocksTreatment(farm);
        generateTreatment();
    });

    $(".constraintTreatment").change(function () {
        generateTreatment();
    });

    $("#resetIconTreatment").on("click", resetTreatmentConstraints);

    $("#collapsibleTableTreatment").on("show.bs.collapse", function () {
        $("#collapseIconTreatment").attr("class", "glyphicon glyphicon-collapse-down");
    });

    $("#collapsibleTableTreatment").on("hide.bs.collapse", function () {
        $("#collapseIconTreatment").attr("class", "glyphicon glyphicon-expand");
    });
});

//TODO: check if dataObj is empty
function transformToTreatmentArray(dataObj) {
    var treatment;
    var rowObject;
    var date;

    for (var i = 0; i < dataObj.Treatments.length; i++) {
        treatment = dataObj.Treatments[i];

        date = new XDate(treatment.Date);

        rowObject =
        {
            farmName: treatment.BlockFarmFarmName,
            blockName: treatment.BlockBlockName,
            comment: treatment.Comments,
            date: date.toString("yyyy-MM-dd")
        };

        treatmentObjects.push(rowObject);
    }

    generateFirstTimeTreatment();
}

function setBlocksTreatment(farm) {
    $("#blocksTreatment").children().remove();
    $("#blocksTreatment").append(new Option("All Blocks", "all"));
    var array;
    if (farm !== "all") {
        array = treatmentObjects.filter(function (obj) {
            if (obj.farmName === farm) {
                return true;
            }
        });
    } else {
        {
            array = treatmentObjects;
        }
    }

    var uniqueArray = flattenDataArray("blockName", array);

    for (var x = 0; x < uniqueArray.length; x++) {
        $("#blocksTreatment").append(new Option(uniqueArray[x], uniqueArray[x]));
    }
};

function generateFirstTimeTreatment() {
    $("#treatmentTableDiv").toggleClass("whirly-loader");
    $("#treatmentTableDiv").css("margin-top", "30px");
    var data = filterDataTreatment();
    var table = $(document.createElement("table"));
    table.attr("id", "treatmentTable");
/*    table.attr("class", "row-border");*/
    table.attr("class", "display");
    table.attr("style", "border: 1px solid #D8D8D8");
    $("#treatmentTableDiv").append(table);
    dataTablesForTreatment = $("#treatmentTable").DataTable({
        searching: false,
        lengthChange: true,
        pageLength: 25,
        data: data,
        dom: "<'row'<'col-sm-10'l><'col-sm-2'B>>" + "<'row'<'col-sm-12't>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",

        buttons: [
            {
                "text": "",
                "extend": "excelHtml5",
                "title": "TreatmentData" + "_" + new XDate().toString("yyyy-MM-dd")
            },
            {
                "text": "",
                "extend": "pdfHtml5",
                "title": "TreatmentData" + "_" + new XDate().toString("yyyy-MM-dd")
            },
            {
                "extend": "print",
                "text": "",
                "title": "TreatmentData" + "_" + new XDate().toString("yyyy-MM-dd")
            }
        ],

        columns: [
            { "title": "Farm", data: "farmName", "width": "20%" },
            { "title": "Block", data: "blockName", "width": "20%" },
            { "title": "Date", data: "date", "width": "20%" },
            { "title": "Comments", data: "comment", "width":"40%" }
        ]
    });
};

function generateTreatment() {
    var newData = filterDataTreatment();
    dataTablesForTreatment.clear().draw();
    dataTablesForTreatment.rows.add(newData);
    dataTablesForTreatment.columns.adjust().draw();
};

function filterDataTreatment() {
    var farm = $("#farmTreatment").val();
    var block = $("#blocksTreatment").val();
    var fromDate = $("#timeFromTreatment").val();
    var toDate = $("#timeToTreatment").val();

    var appliedFilters = scoutStopObjects.filter(function (obj) {
        if ((obj.farmName === farm || farm === "all") &&
        (obj.blockName === block || block === "all") &&
        ((obj.date >= fromDate && obj.date <= toDate) || $("#dateAnyTreatment").is(":checked"))) {
            return true;
        }
    });

    return appliedFilters;
}

function resetTreatmentConstraints() {
    $("#farmTreatment").val("all");
    $("#blocksTreatment").val("all");
    setFromDate();
    setToDate();
    $("#dateAnyTreatment").prop("checked", true);
}