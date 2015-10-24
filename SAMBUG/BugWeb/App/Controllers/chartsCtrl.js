angular.module("appMain")
    .controller("ChartsCtrl", ["$scope", "$mdSidenav", "commonReportingService", "chartService", function ($scope, $mdSidenav, commonReportingService, chartService) {

        $scope.loading = true;
        $scope.someScoutStops = true;

        $scope.menu = {
            title: "Chart Settings"
        }

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

        var constructDataFilter = function () {
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
                $scope.constraints.misc[i].value = $scope.constraints.misc[i].list[0];
            }
            $scope.constraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
            $scope.constraints.dates.to = new Date();
            $scope.constraints.dates.all = false;
        };

        $scope.$watch("constraints.misc.farm.value", function (newValue) {
            $scope.constraints.misc.block.list = commonReportingService.getBlocksForFarms([newValue]);
            $scope.constraints.misc.block.value = $scope.constraints.misc.block.list[0];
            updateChart();
        });

        $scope.$watch("constraints.misc.block.value", function () {
            updateChart();
        });

        $scope.$watch("constraints.misc.species.value", function (newValue) {
            $scope.constraints.misc.lifeStage.list = commonReportingService.getLifeStagesForSpecies([newValue]);
            $scope.constraints.misc.lifeStage.value = $scope.constraints.misc.lifeStage.list[0];
            updateChart();
        });

        $scope.$watch("constraints.misc.lifeStage.value", function () {
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

        var scoutstopsDone = false;
        var speciesDone = false;

        var initChartControls = function () {
            chartService.initChart("chart");
            $mdSidenav("right").toggle();
            $scope.loading = false;
        }

        commonReportingService.init(function (farmNames) {
            $scope.constraints.misc.farm.list = farmNames;
            $scope.constraints.misc.farm.value = farmNames[0];

            scoutstopsDone = true;

            if (speciesDone) {
                initChartControls();
            }

        }, function (speciesNames) {
            $scope.constraints.misc.species.list = speciesNames;
            $scope.constraints.misc.species.value = speciesNames[0];

            speciesDone = true;

            if (scoutstopsDone) {
                initChartControls();
            }

        });


    }]);