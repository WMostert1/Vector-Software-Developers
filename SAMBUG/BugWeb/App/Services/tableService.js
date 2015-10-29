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
                { "title": "Farm", data: "farmName", width: "12%" },
                { "title": "Block", data: "blockName", width: "10%" },
                { "title": "Date", data: "date", width: "10%" },
                { "title": "Tree Count", data: "numberOfTrees", width: "5%" },
                { "title": "Bug Species", data: "speciesName", width: "13%" },
                { "title": "Life Stage", data: "lifeStage", width: "10%" },
                { "title": "Pest", data: "isPest", width: "5%" },
                { "title": "Bug Count", data: "numberOfBugs", width: "5%" },
                { "title": "Comments", data: "comments", width: "30%" }
            ];
            scoutDataTable = $(tableId).DataTable(scoutTableOptions);            
        };

        var initTreatmentTable = function (tableId) {
            var treatmentTableOptions = angular.copy(tableOptions);
            treatmentTableOptions.columns = [
                { "title": "Farm", data: "farmName", width: "20%" },
                { "title": "Block", data: "blockName", width: "20%" },
                { "title": "Date", data: "date", width: "20%" },
                { "title": "Comments", data: "comments", width: "40%" }
            ];
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