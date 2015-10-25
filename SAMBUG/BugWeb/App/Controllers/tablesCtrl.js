angular.module("appMain")
    .controller("TablesCtrl", ["$scope", "$mdSidenav", "commonReportingService", "tableService", function ($scope, $mdSidenav, commonReportingService, tableService) {
        $scope.loading = true;

        $scope.menu = {
            title: "Scout Table Settings"
        }

        //--------constraints refer to scoutConstraints-------//
        $scope.constraints = {
            misc: {
                farms: {
                    title: "Farm"
                },
                blocks: {
                    title: "Block"
                },
                species: {
                    title: "Species"
                },
                lifeStages: {
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
                farms: {
                    title: "Farm"
                },
                blocks: {
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
                farms: $scope.constraints.misc.farms.selected,
                blocks: $scope.constraints.misc.blocks.selected,
                species: $scope.constraints.misc.species.selected,
                lifeStages: $scope.constraints.misc.lifeStages.selected,
                dates: {
                    from: $scope.constraints.dates.from,
                    to: $scope.constraints.dates.to,
                    all: $scope.constraints.dates.all
                }
            }
        }

        var constructTreatmentDataFilter = function () {
            return {
                farms: $scope.treatmentConstraints.misc.farms.selected,
                blocks: $scope.treatmentConstraints.misc.blocks.selected,                
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
            if ($scope.showScoutConstraints) {
                $scope.constraints.misc.farms.selected = [];
                $scope.constraints.misc.blocks.selected = [];
                $scope.constraints.misc.species.selected = [];
                $scope.constraints.misc.lifeStages.selected = [];
                $scope.constraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
                $scope.constraints.dates.to = new Date();
                $scope.constraints.dates.all = false;
            } else {
                $scope.treatmentConstraints.misc.farms.selected = [];
                $scope.treatmentConstraints.misc.blocks.selected = [];
                $scope.treatmentConstraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
                $scope.treatmentConstraints.dates.to = new Date();
                $scope.treatmentConstraints.dates.all = false;
            }
        };
        //----------Watch for changes in scout constraints---------//

        $scope.$watchCollection("constraints.misc.farms.selected", function (newValue) {
            $scope.constraints.misc.blocks.list = commonReportingService.getBlocksForFarms(newValue);
            $scope.constraints.misc.blocks.selected = [];
            updateScoutTable();
        });
                
        $scope.$watchCollection("constraints.misc.blocks.selected", function () {
            updateScoutTable();
        });

        $scope.$watchCollection("constraints.misc.species.selected", function (newValue) {
            $scope.constraints.misc.lifeStages.list = commonReportingService.getLifeStagesForSpecies(newValue);
            $scope.constraints.misc.lifeStages.selected = [];
            updateScoutTable();
        });

        
        $scope.$watchCollection("constraints.misc.lifeStages.selected", function () {
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
        $scope.$watchCollection("treatmentConstraints.misc.farms.selected", function (newValue) {
            $scope.treatmentConstraints.misc.blocks.list = commonReportingService.getBlocksForFarms(newValue);
            $scope.treatmentConstraints.misc.blocks.selected = [];
            updateTreatmentTable();
        });

        $scope.$watchCollection("treatmentConstraints.misc.blocks.selected", function () {
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

        //todo incorporate suggestion engine (bloodhound.js?) to order the suggestions
        $scope.filterSuggestions = function (list, selected, searchText) {
            var excludingSelected = Enumerable.From(list).Except(selected).ToArray();
            if (searchText) {
                return Enumerable.From(excludingSelected).Where(function (i) {
                    return i.name.toLowerCase().indexOf(searchText.toLowerCase()) > -1;
                }).ToArray();
            } else {
                return excludingSelected;
            }
        }

        var scoutstopsDone = false;
        var speciesDone = false;

        var initTablesControls = function () {
            $scope.loading = false;
            $mdSidenav("right").toggle();
            tableService.initTables("#scoutTable", "#treatmentTable");
        }

        commonReportingService.init(function (farms) {
            $scope.constraints.misc.farms.list = farms;
            $scope.constraints.misc.farms.selected = [];
            $scope.treatmentConstraints.misc.farms.list = farms;
            $scope.treatmentConstraints.misc.farms.selected = [];
            commonReportingService.transformDataForTables();

            scoutstopsDone = true;

            if (speciesDone) {
                initTablesControls();
            }

        }, function (species) {
            $scope.constraints.misc.species.list = species;
            $scope.constraints.misc.species.selected = [];

            speciesDone = true;

            if (scoutstopsDone) {
                initTablesControls();
            }

        });
    }]);