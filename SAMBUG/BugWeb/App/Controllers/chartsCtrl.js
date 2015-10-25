angular.module("appMain")
    .controller("ChartsCtrl", ["$scope", "$mdSidenav", "commonReportingService", "chartService", function ($scope, $mdSidenav, commonReportingService, chartService) {

        $scope.loading = true;
        $scope.someScoutStops = true;

        $scope.menu = {
            title: "Chart Settings"
        }

        $scope.constraints = {
            misc: {
                farms: {
                    title: "Farms"
                },
                blocks: {
                    title: "Blocks"
                },
                species: {
                    title: "Species"
                },
                lifeStages: {
                    title: "Life Stages"
                }
            },
            dates: {
                from: new Date((new XDate()).addWeeks(-2, true)),
                to: new Date(),
                all: false
            }


        }

        var constructDataFilter = function () {
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

        var updateChart = function () {
            var filteredScoutStops = commonReportingService.getScoutStops(constructDataFilter());

            if (filteredScoutStops.length > 0) {
                $scope.someScoutStops = true;
                chartService.updateChart(filteredScoutStops);
            } else
                $scope.someScoutStops = false;


        }

        $scope.defaultSettings = function () {
            $scope.constraints.misc.farms.selected = [];
            $scope.constraints.misc.blocks.selected = [];
            $scope.constraints.misc.species.selected = [];
            $scope.constraints.misc.lifeStages.selected = [];
            $scope.constraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
            $scope.constraints.dates.to = new Date();
            $scope.constraints.dates.all = false;
        };

        $scope.$watchCollection("constraints.misc.farms.selected", function (newValue) {
            if (newValue) {
                $scope.constraints.misc.blocks.list = commonReportingService.getBlocksForFarms(newValue);
                $scope.constraints.misc.blocks.selected = [];
                updateChart();
            }
        });

        $scope.$watchCollection("constraints.misc.blocks.selected", function () {
            updateChart();
        });

        $scope.$watchCollection("constraints.misc.species.selected", function (newValue) {
            if (newValue) {
                $scope.constraints.misc.lifeStages.list = commonReportingService.getLifeStagesForSpecies(newValue);
                $scope.constraints.misc.lifeStages.selected = [];
                updateChart();
            }
        });

        $scope.$watchCollection("constraints.misc.lifeStages.selected", function () {
            updateChart();
        });

        $scope.$watch("constraints.dates.from", function () {
            updateChart();
        });

        $scope.$watch("constraints.dates.to", function () {
            updateChart();
        });

        $scope.$watch("constraints.dates.all", function () {
            updateChart();
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

        var initChartControls = function () {
            chartService.initChart("chart");
            $mdSidenav("right").toggle();
            $scope.loading = false;
        }

        commonReportingService.init(function (farms) {
            $scope.constraints.misc.farms.list = farms;
            $scope.constraints.misc.farms.selected = [];

            scoutstopsDone = true;

            if (speciesDone) {
                initChartControls();
            }

        }, function (species) {
            $scope.constraints.misc.species.list = species;
            $scope.constraints.misc.species.selected = [];

            speciesDone = true;

            if (scoutstopsDone) {
                initChartControls();
            }

        });


    }]);