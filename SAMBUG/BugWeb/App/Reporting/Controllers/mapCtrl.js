angular.module("reporting")
    .controller("MapCtrl", ["$scope", "reportingDataService", function ($scope, reportingDataService) {
        var data;

        reportingDataService.fetch(function(data) {

        });
        
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