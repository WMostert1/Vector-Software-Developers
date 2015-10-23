angular.module("appMain")
    .controller("TablesCtrl", ["$scope", "$mdSidenav", "commonReportingService", "tableService", function ($scope, $mdSidenav, commonReportingService, tableService) {
        $scope.loading = true;
        
        $scope.scoutConstraints = {
            misc: {
                farm: {
                    title: "Farm"
                },
                block: {
                    title: "Block"
                },
                species: {
                    title: "Species"
                },
                lifeStage: {
                    title: "Life Stage"
                }
            },
            dates: {
                from: new Date((new XDate()).addWeeks(-2, true)),
                to: new Date(),
                all: false
            }


        }

        $scope.treatmentConstraints = {
            misc: {
                farm: {
                    title: "Farm"
                },
                block: {
                    title: "Block"
                }                
            },
            dates: {
                from: new Date((new XDate()).addWeeks(-2, true)),
                to: new Date(),
                all: false
            }
        }

        var constructScoutDataFilter = function () {
            return {
                farms: [$scope.scoutConstraints.misc.farm.value],
                blocks: [$scope.scoutConstraints.misc.block.value],
                species: [$scope.scoutConstraints.misc.species.value],
                lifeStages: [$scope.scoutConstraints.misc.lifeStage.value],
                dates: {
                    from: $scope.scoutConstraints.dates.from,
                    to: $scope.scoutConstraints.dates.to,
                    all: $scope.scoutConstraints.dates.all
                }
            }
        }

        var constructTreatmentDataFilter = function () {
            return {
                farms: [$scope.treatmentConstraints.misc.farm.value],
                blocks: [$scope.treatmentConstraints.misc.block.value],                
                dates: {
                    from: $scope.treatmentConstraints.dates.from,
                    to: $scope.treatmentConstraints.dates.to,
                    all: $scope.treatmentConstraints.dates.all
                }
            }
        }

        var updateScoutTable = function () {
            tableService.updateScoutTable(commonReportingService.getScoutStops(constructScoutDataFilter()));
        }

        var updateTreatmentTable = function () {
            tableService.updateTreatmentTable(commonReportingService.getTreatments(constructTreatmentDataFilter()));
        }

        $scope.defaultSettings = function () {
            for (var i = 0; i < $scope.scoutConstraints.misc; i++) {
                $scope.scoutConstraints.misc[i].value = $scope.scoutConstraints.misc[i].list[0];
            }
            $scope.scoutConstraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
            $scope.scoutConstraints.dates.to = new Date();
            $scope.scoutConstraints.dates.all = false;
        };
        //----------Watch for changes in scout constraints---------//

        $scope.$watch("scoutConstraints.misc.farm.value", function (newValue) {
            $scope.scoutConstraints.misc.block.list = commonReportingService.getBlocksForFarms([newValue]);
            $scope.scoutConstraints.misc.block.value = $scope.scoutConstraints.misc.block.list[0];
            updateScoutTable();
        });
                
        $scope.$watch("scoutConstraints.misc.block.value", function () {
            updateScoutTable();
        });

        $scope.$watch("scoutConstraints.misc.species.value", function (newValue) {
            $scope.scoutConstraints.misc.lifeStage.list = commonReportingService.getLifeStagesForSpecies([newValue]);
            $scope.scoutConstraints.misc.lifeStage.value = $scope.scoutConstraints.misc.lifeStage.list[0];
            updateScoutTable();
        });

        
        $scope.$watch("scoutConstraints.misc.lifeStage.value", function () {
            updateScoutTable();
        });

        $scope.$watch("scoutConstraints.dates.from", function () {
            updateScoutTable();
        });

        $scope.$watch("scoutConstraints.dates.to", function () {
            updateScoutTable();
        });

        $scope.$watch("scoutConstraints.dates.all", function () {
            updateScoutTable();
        });

        //----------Watch for changes in treatment constraints---------//
        $scope.$watch("treatmentConstraints.misc.farm.value", function (newValue) {
            $scope.treatmentConstraints.misc.block.list = commonReportingService.getBlocksForFarms([newValue]);
            $scope.treatmentConstraints.misc.block.value = $scope.treatmentConstraints.misc.block.list[0];
            updateTreatmentTable();
        });

        $scope.$watch("treatmentConstraints.misc.block.value", function () {
            updateTreatmentTable();
        });

        $scope.$watch("treatmentConstraints.dates.from", function () {
            updateTreatmentTable();
        });

        $scope.$watch("treatmentConstraints.dates.to", function () {
            updateTreatmentTable();
        });

        $scope.$watch("treatmentConstraints.dates.all", function () {
            updateTreatmentTable();
        });

        $scope.$watch("selectedTab", function (newValue) {
            switch (newValue) {
                case 0:
                    $scope.showScoutConstraints = true;                    
                    break;
                case 1:
                    $scope.showScoutConstraints = false;
                    break;
            }
        });

        var scoutstopsDone = false;
        var speciesDone = false;

        var initTablesControls = function () {
            $scope.loading = false;
            $mdSidenav("right").toggle();
            tableService.initTables("#scoutTable", "#treatmentTable");
        }

       commonReportingService.init(function (farmNames) {
            $scope.scoutConstraints.misc.farm.list = farmNames;
            $scope.scoutConstraints.misc.farm.value = farmNames[0];
            $scope.treatmentConstraints.misc.farm.list = farmNames;
            $scope.treatmentConstraints.misc.farm.value = farmNames[0];
            commonReportingService.transformDataForTables();

            scoutstopsDone = true;

            if (speciesDone) {
                initTablesControls();
            }

        }, function (speciesNames) {
            $scope.scoutConstraints.misc.species.list = speciesNames;
            $scope.scoutConstraints.misc.species.value = speciesNames[0];

            speciesDone = true;

            if (scoutstopsDone) {
                initTablesControls();
            }

        });
    }]);