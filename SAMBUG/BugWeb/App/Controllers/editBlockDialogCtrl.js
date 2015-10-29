angular.module("appMain")
    .controller("EditBlockDialogCtrl", [
                "$scope", "$mdDialog", "$mdToast", "$http", "rootObj", function ($scope, $mdDialog, $mdToast, $http, rootObj) {

                    //rootObj is block
                    //nextRootObj is farm

                    $scope.newBlockName = rootObj.name;

                    $scope.cancel = function () {
                        $mdDialog.cancel();
                    };

                    $scope.post = function(event) {
                        event.preventDefault();

                        $scope.errorMessage = "";

                        if (!event.target.checkValidity) {
                            return false;
                        }

                        $scope.loading = true;

                        $http.put(event.target.action, {
                            BlockID: rootObj.id,
                            BlockName: $scope.newBlockName
                        }).then(function (response) {
                            $scope.loading = false;
                                $mdToast.show(
                                    $mdToast.simple()
                                    .content(rootObj.name + "  changed to " + $scope.newBlockName)
                                    .position("top right")
                                    .hideDelay(1500)
                                );
                                $mdDialog.hide();
                        }, function () {
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