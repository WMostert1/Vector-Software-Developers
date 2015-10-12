angular.module("appMain")
    /*
    Angular App Controller
    */
    .controller("AppCtrl", ["$scope", "$mdSidenav", function ($scope, $mdSidenav) {

        $scope.toggleSideNav = function (id) {
            $mdSidenav(id).toggle();
        };

    }]);