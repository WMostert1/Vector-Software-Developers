angular.module("appMain")
    .controller("TablesCtrl", ["$scope", "$mdSidenav", "commonReportingService", "tableService", function ($scope, $mdSidenav, commonReportingService, tableService) {
        $scope.loading = true;

        $scope.menu = {
            title: "Scout Table Settings"
        }

        //--------constraints refer to scoutConstraints-------//
        $scope.constraints = {
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
                farms: [$scope.constraints.misc.farm.value],
                blocks: [$scope.constraints.misc.block.value],
                species: [$scope.constraints.misc.species.value],
                lifeStages: [$scope.constraints.misc.lifeStage.value],
                dates: {
                    from: $scope.constraints.dates.from,
                    to: $scope.constraints.dates.to,
                    all: $scope.constraints.dates.all
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
            for (var i = 0; i < $scope.constraints.misc; i++) {
                $scope.constraints.misc[i].value = $scope.constraints.misc[i].list[0];
            }
            $scope.constraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
            $scope.constraints.dates.to = new Date();
            $scope.constraints.dates.all = false;
        };
        //----------Watch for changes in scout constraints---------//

        $scope.$watch("constraints.misc.farm.value", function (newValue) {
            $scope.constraints.misc.block.list = commonReportingService.getBlocksForFarms([newValue]);
            $scope.constraints.misc.block.value = $scope.constraints.misc.block.list[0];
            updateScoutTable();
        });
                
        $scope.$watch("constraints.misc.block.value", function () {
            updateScoutTable();
        });

        $scope.$watch("constraints.misc.species.value", function (newValue) {
            $scope.constraints.misc.lifeStage.list = commonReportingService.getLifeStagesForSpecies([newValue]);
            $scope.constraints.misc.lifeStage.value = $scope.constraints.misc.lifeStage.list[0];
            updateScoutTable();
        });

        
        $scope.$watch("constraints.misc.lifeStage.value", function () {
            updateScoutTable();
        });

        $scope.$watch("constraints.dates.from", function () {
            updateScoutTable();
        });

        $scope.$watch("constraints.dates.to", function () {
            updateScoutTable();
        });

        $scope.$watch("constraints.dates.all", function () {
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
                    $scope.menu.title = "Scout Table Settings";
                    $scope.showScoutConstraints = true;                    
                    break;
                case 1:
                    $scope.menu.title = "Spray Table Settings";
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
            $scope.constraints.misc.farm.list = farmNames;
            $scope.constraints.misc.farm.value = farmNames[0];
            $scope.treatmentConstraints.misc.farm.list = farmNames;
            $scope.treatmentConstraints.misc.farm.value = farmNames[0];
            commonReportingService.transformDataForTables();

            scoutstopsDone = true;

            if (speciesDone) {
                initTablesControls();
            }

        }, function (speciesNames) {
            $scope.constraints.misc.species.list = speciesNames;
            $scope.constraints.misc.species.value = speciesNames[0];

            speciesDone = true;

            if (scoutstopsDone) {
                initTablesControls();
            }

        });
    }]);