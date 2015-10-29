angular.module("appMain")
    .controller("DeleteFarmDialogCtrl", [
                "$scope", "$mdDialog", "$mdToast", "$http", "rootObj", function ($scope, $mdDialog, $mdToast, $http, rootObj) {
                    $scope.farmName = rootObj.farmName;
     
                    $scope.cancel = function () {
                        $mdDialog.cancel();
                    };

                    $scope.post = function (event) {
                        event.preventDefault();

                        $scope.errorMessage = "";

                        if (!event.target.checkValidity) {
                            return false;
                        }

                        $scope.loading = true;
                        $http.delete(event.target.action + "/" + rootObj.farmId)
                            .then(function (response) {
                            $scope.loading = false;
                            $mdToast.show(
                                $mdToast.simple()
                                .content(rootObj.farmName + " was deleted successfully")
                                .position("top right")
                                .hideDelay(1500)
                            );
                            $mdDialog.hide();
                        }, function (response) {
                            $scope.loading = false;
                            if (response.status === 404) {
                                $scope.errorMessage = { farmNotFound: true };
                            }
                            if (response.status === 400) {
                                $scope.errorMessage = { invalidInput: true };
                            }

                        });

                        return true;
                    }
                }
    ]);