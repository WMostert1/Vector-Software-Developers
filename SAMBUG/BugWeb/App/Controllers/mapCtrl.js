angular.module("appMain")
    .controller("MapCtrl", ["$scope", "commonReportingService", "mapService", function ($scope, commonReportingService, mapService) {

        $scope.options = {
            radius: 10,
            blueColourGradient: false
        }

        $scope.constraints = {
            misc: {
                farm: {
                    title: "Farm",
                    list: []
                },
                block: {
                    title: "Block",
                    list: []
                },
                species: {
                    title: "Species",
                    list: []
                },
                lifeStage: {
                    title: "Life Cycle Stage",
                    list: []
                }
            },
            dates: {
                from: new Date((new XDate()).addWeeks(-2, true)),
                to: new Date()
            }


        }

        $scope.defaultSettings = function() {
            for (var i = 0; i < $scope.constraints.misc; i++) {
                $scope.constraints.misc[i].value = $scope.constraints.misc[i].list[0];
            }
            $scope.constraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
            $scope.constraints.dates.to = new Date();
            $scope.constraints.dates.all = false;
            $scope.options.radius = 10;
            $scope.options.blueColourGradient = false;
        };
        
        commonReportingService.init(function(farmNames) {
            $scope.constraints.misc.farm.list = farmNames;
            $scope.constraints.misc.farm.value = farmNames[0];
            mapService.initMap(commonReportingService.getScoutStops(), "map");
        }, function(speciesNames) {
            $scope.constraints.misc.species.list = speciesNames;
            $scope.constraints.misc.species.value = speciesNames[0];
        });

        $scope.$watch("constraints.misc.farm.value", function(newValue) {
            $scope.constraints.misc.block.list = commonReportingService.getBlocksForFarms([newValue]);
            $scope.constraints.misc.block.value = $scope.constraints.misc.block.list[0];
            mapService.updateMap(commonReportingService.getScoutStops());
        });

        $scope.$watch("constraints.misc.species.value", function (newValue) {
            $scope.constraints.misc.lifeStage.list = commonReportingService.getLifeStagesForSpecies([newValue]);
            $scope.constraints.misc.lifeStage.value = $scope.constraints.misc.lifeStage.list[0];
        });

        $scope.$watch("options.radius", function (newValue) {
            mapService.changeRadius(newValue);
        });

        $scope.$watch("options.blueColourGradient", function () {
            mapService.toggleGradient();
        });


    }]);