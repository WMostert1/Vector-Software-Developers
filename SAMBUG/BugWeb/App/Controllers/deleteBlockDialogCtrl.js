angular.module("appMain")
    .controller("DeleteBlockDialogCtrl", [
                "$scope", "$mdDialog", "$mdToast", "$http", "rootObj", function ($scope, $mdDialog, $mdToast, $http, rootObj) {
                    $scope.blockName = rootObj.name;

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
                        $http.delete(event.target.action + "/" + rootObj.id)
                            .then(function (response) {
                                $scope.loading = false;
                                $mdToast.show(
                                    $mdToast.simple()
                                    .content(rootObj.name + " was deleted successfully")
                                    .position("top right")
                                    .hideDelay(1500)
                                );
                                $mdDialog.hide();
                            }, function (response) {
                                $scope.loading = false;
                                if (response.status === 404) {
                                    $scope.errorMessage = { blockNotFound: true };
                                }
                                if (response.status === 400) {
                                    $scope.errorMessage = { invalidInput: true };
                                }

                            });

                        return true;
                    }
                }
    ]);