angular.module("appMain")
    .service("tableService", [function () {
        var scoutDataTable;
        var treatmentDataTable;

        var tableOptions = {
                language: {emptyTable: "No data available. See Table Settings."},
                colReorder: true,
                searching: false,
                lengthChange: true,
                pageLength: 25,
                data: [],
                dom: "<'tableContainer'<'tableLengthSettings'l><'tableExportButtons'B>tip>",

                buttons: [
                    {
                        "text": "",
                        "extend": "excelHtml5",
                        "title": "ScoutData" + "_" + new XDate().toString("yyyy-MM-dd")
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
                ]
        }

        var initScoutTable = function (tableId) {
            var scoutTableOptions = angular.copy(tableOptions);
            scoutTableOptions.columns = [
                    { "title": "Farm", data: "farmName" },
                    { "title": "Block", data: "blockName" },
                    { "title": "Date", data: "date" },
                    { "title": "Tree Count", data: "numberOfTrees" },
                    { "title": "Bug Species", data: "speciesName" },
                    { "title": "Life stage", data: "lifeStage" },
                    { "title": "Pest", data: "isPest" },
                    { "title": "Bug Count", data: "numberOfBugs" },
                    { "title": "Comments", data: "comments" }
            ]
            scoutDataTable = $(tableId).DataTable(scoutTableOptions);            
        };

        var initTreatmentTable = function (tableId) {
            var treatmentTableOptions = angular.copy(tableOptions);
            treatmentTableOptions.columns = [
                    { "title": "Farm", data: "farmName" },
                    { "title": "Block", data: "blockName" },
                    { "title": "Date", data: "date" },
                    { "title": "Comments", data: "comments" }
            ]
            treatmentDataTable = $(tableId).DataTable(treatmentTableOptions);
        };

        this.initTables = function(scoutTableId, treatmentTableId){
            initScoutTable(scoutTableId);
            initTreatmentTable(treatmentTableId);
        }

        this.updateScoutTable = function (stops) {
            if (scoutDataTable) {
                scoutDataTable.clear().draw();
                scoutDataTable.rows.add(stops);
                scoutDataTable.draw();
            }
        };

        this.updateTreatmentTable = function (treatments) {
            if (treatmentDataTable) {
                treatmentDataTable.clear().draw();
                treatmentDataTable.rows.add(treatments);
                treatmentDataTable.draw();
            }
        };
    }]);