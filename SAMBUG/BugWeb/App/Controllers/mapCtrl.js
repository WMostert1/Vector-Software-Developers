angular.module("appMain")
    .controller("MapCtrl", ["$scope", "reportingDataService", "reportingUrlService", function ($scope, reportingDataService, reportingUrlService) {
        reportingDataService.fetch(reportingUrlService.recordsUrl,function(response) {
            console.log(response);
        });

        $scope.constraints = {
            farm: {
                title: "Farm",
                value: "All Farms",
                list: [
                    "All Farms",
                    "FarmA",
                    "FarmB",
                    "FarmC"
                ]
            },
            block: {
                title: "Block",
                value: "All Blocks",
                list: [
                    "All Blocks",
                    "b1",
                    "b2",
                    "b3"
                ]
            },
            species: {
                title: "Species",
                value: "All Species",
                list: [
                    "All Species",
                    "s1",
                    "s2",
                    "s3"
                ]
            },
            lifeStage: {
                title: "Life Cycle Stage",
                value: "All Life Cycle Stages",
                list: [
                    "All Life Cycle Stages",
                    "ls1",
                    "ls2",
                    "ls3"
                ]
            }
        }
        
        $scope.optionsContainer = {
            collapseIcon: "expand_more"
        };

        $scope.toggleCollapseIcon = function () {
            if ($scope.optionsContainer.collapseIcon === "expand_more")
                $scope.optionsContainer.collapseIcon = "expand_less";
            else
                $scope.optionsContainer.collapseIcon = "expand_more";
        };

    }]);