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
            for (var i = 0; i < $scope.constraints.misc; i++) {
                $scope.constraints.misc[i].selected = [$scope.constraints.misc[i].list[0]];
            }
            $scope.constraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
            $scope.constraints.dates.to = new Date();
            $scope.constraints.dates.all = false;
        };

        $scope.$watch("constraints.misc.farms.selected", function (newValue) {
            $scope.constraints.misc.blocks.list = commonReportingService.getBlocksForFarms(newValue);
            $scope.constraints.misc.blocks.selected = [$scope.constraints.misc.blocks.list[0]];
            updateChart();
        });

        $scope.$watch("constraints.misc.blocks.selected", function () {
            updateChart();
        });

        $scope.$watch("constraints.misc.species.selected", function (newValue) {
            $scope.constraints.misc.lifeStages.list = commonReportingService.getLifeStagesForSpecies(newValue);
            $scope.constraints.misc.lifeStages.selected = [$scope.constraints.misc.lifeStages.list[0]];
            updateChart();
        });

        $scope.$watch("constraints.misc.lifeStages.selected", function () {
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


        $scope.filterSuggestions = function (list, searchText) {
            return Enumerable.From(list).Where(function (i) {
                return i.toLowerCase().indexOf(searchText.toLowerCase()) > -1;
            }).ToArray();
        }

        var scoutstopsDone = false;
        var speciesDone = false;

        

        var initChartControls = function () {
            chartService.initChart("chart");
            $mdSidenav("right").toggle();
            $scope.loading = false;
        }

        commonReportingService.init(function (farmNames) {
            $scope.constraints.misc.farms.list = farmNames;
            $scope.constraints.misc.farms.selected = [farmNames[0]];

            scoutstopsDone = true;

            if (speciesDone) {
                initChartControls();
            }

        }, function (speciesNames) {
            $scope.constraints.misc.species.list = speciesNames;
            $scope.constraints.misc.species.selected = [speciesNames[0]];

            speciesDone = true;

            if (scoutstopsDone) {
                initChartControls();
            }

        });


    }]);