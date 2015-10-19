angular.module("appMain")
    .controller("AddFarmDialogCtrl", [
                "$scope", "$mdDialog", "$mdToast", "$http", function ($scope, $mdDialog, $mdToast, $http) {

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

                        //delete when talking to server
                        $mdDialog.hide({newFarmName: $scope.newFarmName, id: "6"});

                        /*$http.post(event.target.action, {
                            
                        }).then(function (response) {
                            $scope.loading = false;
                            if (response.data.success === true) {
                                $mdToast.show(
                                    $mdToast.simple()
                                    .content("Block name changed successfully")
                                    .position("top right")
                                    .hideDelay(1500)
                                );
                                $mdDialog.hide($scope.newBlockName);
                            } else {
                                $scope.errorMessage = "The email or password you entered is incorrect";
                            }
                        }, function () {
                            $scope.loading = false;
                            $scope.errorMessage = "Trouble contacting server. Please try again.";
                        });*/

                        return true;
                    }
                }
    ]);