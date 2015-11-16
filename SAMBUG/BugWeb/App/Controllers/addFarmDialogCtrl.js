angular.module("appMain")
    .controller("AddFarmDialogCtrl", [
                "$scope", "$mdDialog", "$mdToast", "$http", "userDetails", function ($scope, $mdDialog, $mdToast, $http, userDetails) {

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

                        $http.post(event.target.action, {
                            UserID: userDetails.id,
                            FarmName: $scope.newFarmName
                        }).then(function (response) {
                            $scope.loading = false;
                            $mdToast.show(
                                $mdToast.simple()
                                .content($scope.newFarmName + " was added successfully")
                                .position("top right")
                                .hideDelay(1500)
                            );
                            $mdDialog.hide();
                        }, function (response) {
                            $scope.loading = false;
                            if (response.status === 409) {
                                $scope.errorMessage = {farmExists: true};
                            }
                            if (response.status === 400) {
                                $scope.errorMessage = { invalidInput: true };
                            }
                            
                        });

                        return true;
                    }
                }
    ]);