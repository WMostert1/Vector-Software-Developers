angular.module("appMain")
    .controller("AddBlockDialogCtrl", [
                "$scope", "$mdDialog", "$mdToast", "$http", "rootObj", function ($scope, $mdDialog, $mdToast, $http, rootObj) {

            console.log(rootObj);

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
                            farmID: rootObj.farmId,
                            blockName: $scope.newBlockName
                        }).then(function (response) {
                            $scope.loading = false;
                                $mdToast.show(
                                    $mdToast.simple()
                                    .content($scope.newBlockName + " has been added successfully")
                                    .position("top right")
                                    .hideDelay(1500)
                                );
                                $mdDialog.hide();
                        }, function () {
                            $scope.loading = false;
                            if (response.status === 409) {
                                $scope.errorMessage = { blockExists: true };
                            }
                            if (response.status === 400) {
                                $scope.errorMessage = { invalidInput: true };
                            }

                        });

                        return true;
                    }
                }
    ]);