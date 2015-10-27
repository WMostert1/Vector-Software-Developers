angular.module("appMain")
    .controller("MapCtrl", ["$scope", "$mdSidenav", "commonReportingService", "mapService", function ($scope, $mdSidenav, commonReportingService, mapService) {

        $scope.loading = true;
        $scope.someScoutStops = true;

        $scope.menu = {
            title: "Map Settings"
        }
        
        $scope.options = {
            radius: 0.001,
            blueColourGradient: false
        }

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

        var constructDataFilter = function() {
            return {
                farms: $scope.constraints.misc.farms.selected,
                blocks: $scope.constraints.misc.blocks.selected,
                species: $scope.constraints.misc.species.selected,
                isPest: [true],
                lifeStages: $scope.constraints.misc.lifeStages.selected,
                dates: {
                    from: $scope.constraints.dates.from,
                    to: $scope.constraints.dates.to,
                    all: $scope.constraints.dates.all
                }
            }
        }

        var updateMap = function () {
            var filteredScoutStops = commonReportingService.getScoutStops(constructDataFilter());

            if (filteredScoutStops.length > 0) {
                $scope.someScoutStops = true;
                mapService.updateMap(filteredScoutStops);
            } else
                $scope.someScoutStops = false;

            
        }

        $scope.defaultSettings = function() {
            $scope.constraints.misc.farms.selected = [];
            $scope.constraints.misc.blocks.selected = [];
            $scope.constraints.misc.species.selected = [];
            $scope.constraints.misc.lifeStages.selected = [];
            $scope.constraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
            $scope.constraints.dates.to = new Date();
            $scope.constraints.dates.all = false;
            $scope.options.radius = 0.001;
            $scope.options.blueColourGradient = false;
        };

        $scope.$watchCollection("constraints.misc.farms.selected", function (newValue) {
            if (newValue) {
                $scope.constraints.misc.blocks.list = commonReportingService.getBlocksForFarms(newValue);
                $scope.constraints.misc.blocks.selected = [];
                updateMap();
            }
        });

        $scope.$watchCollection("constraints.misc.blocks.selected", function () {
            updateMap();
        });

        $scope.$watchCollection("constraints.misc.species.selected", function (newValue) {
            if (newValue) {
                $scope.constraints.misc.lifeStages.list = commonReportingService.getLifeStagesForSpecies(newValue);
                $scope.constraints.misc.lifeStages.selected = [];
                updateMap();
            }
        });

        $scope.$watchCollection("constraints.misc.lifeStages.selected", function () {
            updateMap();
        });

        $scope.$watch("constraints.dates.from", function () {
            updateMap();
        });

        $scope.$watch("constraints.dates.to", function () {
            updateMap();
        });

        $scope.$watch("constraints.dates.all", function () {
            updateMap();
        });

        $scope.$watch("options.blueColourGradient", function () {
            mapService.toggleGradient();
        });

        $scope.$watch("options.radius", function (newValue) {
            mapService.changeRadius(newValue);
        });

        $scope.$watch(function() { return angular.element("#map").is(":visible"); },
            function() {
                mapService.resize();
            }
        );

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

        var initMapControls = function () {
            mapService.initMap("map");
            $mdSidenav("right").toggle();
            $scope.loading = false;
        }

        commonReportingService.init(function (farms) {
            $scope.constraints.misc.farms.list = farms;
            $scope.constraints.misc.farms.selected = [];

            scoutstopsDone = true;

            if (speciesDone) {
                initMapControls();
            }

        }, function (species) {
            $scope.constraints.misc.species.list = species;
            $scope.constraints.misc.species.selected = [];

            speciesDone = true;

            if (scoutstopsDone) {
                initMapControls();
            }

        });


    }]);