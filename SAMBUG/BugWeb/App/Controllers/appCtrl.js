angular.module("appMain")
    /*
    Angular App Controller
    */
    .controller("AppCtrl", [
        "$scope", "$mdSidenav", "$mdDialog", function($scope, $mdSidenav, $mdDialog) {

            $scope.toggleSideNav = function(id) {
                $mdSidenav(id).toggle();
            };

            $scope.reportingChevron = "expand_more";
            $scope.farmManagementChevron = "expand_more";

            $scope.toggleChevron = function(chevron) {
                if ($scope[chevron] === "expand_more")
                    $scope[chevron] = "expand_less";
                else
                    $scope[chevron] = "expand_more";
            };

            $scope.reportingMenuItems = [
                { title: "Map" },
                { title: "Charts" },
                { title: "Tables" }
            ];

            $scope.farmManagementMenuItems = [
                { title: "Blocks" },
                { title: "Farms" },
                { title: "Spray Data" }
            ];

            $scope.showDialog = function(type, event) {
                $mdDialog.show({
                    templateUrl: "/App/Views/" + type + "Dialog.html",
                    parent: angular.element(document.body),
                    targetEvent: event,
                    clickOutsideToClose: true,
                    controller: ["$scope", "$mdDialog", function(scope, mdDialog) {
                            scope.hide = function() {
                                mdDialog.hide();
                            };

                            scope.cancel = function() {
                                mdDialog.hide();
                            };
                        }
                    ]
                });
            }

        }
    ]);