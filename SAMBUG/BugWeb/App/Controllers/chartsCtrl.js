angular.module("appMain")
    .controller("ChartsCtrl", ["$scope", "$mdSidenav", "commonReportingService", "chartService", function($scope, $mdSidenav, commonReportingService, chartService) {

        $scope.loading = true;
        $scope.someData = true;

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

        $scope.settings = {
            showPoints: true,
            showTrend: true,
            grouped: {
                y: {
                    title: "Y",
                    selected: {title: "Pests per Tree", name: "bugsPerTree", aggregate: "Average", $$mdSelectId: 1 },
                    groups: [
                        {
                            title: "Average",
                            type: "Average",
                            list: [
                                {
                                    title: "Pests per Tree",
                                    name: "bugsPerTree"
                                },
                                {
                                    title: "Number of Pests",
                                    name: "numberOfBugs"
                                },
                                {
                                    title: "Number of Trees",
                                    name: "numberOfTrees"
                                }
                            ]

                        },
                        {
                            title: "Total",
                            type: "Total",
                            list: [
                                {
                                    title: "Number of Pests",
                                    name: "numberOfBugs"
                                },
                                {
                                    title: "Number of Trees",
                                    name: "numberOfTrees"
                                }
                            ]
                        }
                    ]
                }
            },
            ungrouped: {
                x: {
                    title: "X",
                    selected: {title: "Date", name: "date", type: "line", $$mdSelectId: 6},
                    list: [
                        {
                            title: "Date",
                            name: "date",
                            type: "Line"
                        },
                        {
                            title: "Block",
                            name: "blockName",
                            type: "Bar"
                        }, {
                            title: "Species",
                            name: "speciesName",
                            type: "Bar"
                        },
                        {
                            title: "Species Life Stage",
                            name: "lifeStage",
                            type: "Bar"
                        }
                    ]
                },
                series: {
                    title: "Series",
                    selected: {name: "none", type: null, $$mdSelectId: 10},
                    list: [
                        {
                            title: "None",
                            name: "none"
                        },
                        {
                            title: "Block",
                            name: "blockName"
                        },
                        {
                            title: "Species",
                            name: "speciesName"
                        },
                        {
                            title: "Species Life Stage",
                            name: "lifeStage"
                        }
                    ]
                }
            }
        };

        var constructScoutDataFilter = function () {
            return {
                farms: $scope.constraints.misc.farms.selected,
                blocks: $scope.constraints.misc.blocks.selected,
                isPest: [true],
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
                farms: $scope.constraints.misc.farms.selected,
                blocks: $scope.constraints.misc.blocks.selected,
                dates: {
                    from: $scope.constraints.dates.from,
                    to: $scope.constraints.dates.to,
                    all: $scope.constraints.dates.all
                }
            }
        }

        var constructChartSettings = function() {
            return {
                x: $scope.settings.ungrouped.x.selected.name,
                y: $scope.settings.grouped.y.selected.name,
                series: $scope.settings.ungrouped.series.selected.name,
                aggregate: $scope.settings.grouped.y.selected.aggregate,
                type: $scope.settings.ungrouped.x.selected.type,
                xTitle: $scope.settings.ungrouped.x.selected.title,
                yTitle: $scope.settings.grouped.y.selected.title,
                showPoints: $scope.settings.showPoints,
                showTrend: $scope.settings.showTrend
            }
        }

        var updateChart = function () {
            $scope.chartTitle = $scope.settings.grouped.y.selected.aggregate + " " +
                $scope.settings.grouped.y.selected.title + " vs " +
                $scope.settings.ungrouped.x.selected.title;

            var filteredScoutStops = commonReportingService.getScoutStops(constructScoutDataFilter());
            var filteredTreatments = [];

            if ($scope.settings.ungrouped.x.selected.type === "Line" && $scope.settings.showTreatments)
                filteredTreatments = commonReportingService.getTreatments(constructTreatmentDataFilter());

            if (filteredScoutStops.length > 0 || filteredTreatments.length > 0) {
                $scope.someData = true;
                chartService.updateChart("#chart", filteredScoutStops, filteredTreatments, constructChartSettings());
            } else
                $scope.someData = false;

        }

        $scope.defaultSettings = function () {
            $scope.constraints.misc.farms.selected = [];
            $scope.constraints.misc.blocks.selected = [];
            $scope.constraints.misc.species.selected = [];
            $scope.constraints.misc.lifeStages.selected = [];
            $scope.constraints.dates.from = new Date((new XDate()).addWeeks(-2, true));
            $scope.constraints.dates.to = new Date();
            $scope.constraints.dates.all = false;
            $scope.settings.grouped.y.selected = { name: "bugsPerTree", aggregate: "Average", $$mdSelectId: 1 }
            $scope.settings.ungrouped.x.selected = { name: "date", type: "line", $$mdSelectId: 6 }
            $scope.settings.ungrouped.series.selected = { name: "none", type: null, $$mdSelectId: 10 }
            $scope.settings.showTreatments = false;
            $scope.settings.showPoints = true;
            $scope.settings.showTrend = true;
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

        $scope.$watch("settings.grouped.y.selected", function () {
            updateChart();
        });

        $scope.$watch("settings.ungrouped.x.selected", function () {
            updateChart();
        });

        $scope.$watch("settings.ungrouped.series.selected", function () {
            updateChart();
        });

        $scope.$watch("settings.showTreatments", function (newValue) {
            if (newValue === true)
                $scope.settings.showPoints = true;
            updateChart();
        });

        $scope.$watch("settings.showPoints", function (newValue) {
            if (newValue === false)
                $scope.settings.showTreatments = false;
            updateChart();
        });

        $scope.$watch("settings.showTrend", function () {
            updateChart();
        });

        //it makes no sense to group data into the same groups already formed on the x-axis
        $scope.shouldHideItem = function(option, item) {
            return $scope.settings.ungrouped.x.selected.name === item.name && option !== $scope.settings.ungrouped.x ||
                $scope.settings.ungrouped.series.selected.name === item.name && option !== $scope.settings.ungrouped.series;
        }

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

        $scope.allItemsButX = function(item) {
            console.log(item);
            return true;
        }

        var scoutstopsDone = false;
        var speciesDone = false;

        var initChartControls = function () {
            $scope.loading = false;
            $mdSidenav("right").toggle();
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

        window.onresize = function () {
            updateChart();
        }

    }]);