angular.module("appMain")
    .controller("MapCtrl", ["$scope", "commonReportingService", "reportingUrlService", function ($scope, commonReportingService, reportingUrlService) {
        $scope.options = {
            radius: 10
        }

        $scope.constraints = {
            misc: {
                farm: {
                    title: "Farm",
                    list: [
                        "All Farms"
                    ]
                },
                block: {
                    title: "Block",
                    list: [
                        "All Blocks"
                    ]
                },
                species: {
                    title: "Species",
                    list: [
                        "All Species"
                    ]
                },
                lifeStage: {
                    title: "Life Cycle Stage",
                    list: [
                        "All Life Cycle Stages"
                    ]
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

    }]);