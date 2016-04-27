angular.module("appMain")
    .controller("UpdateUserDeviceCtrl", [
                "$scope", "$mdToast", "$http", "rootObj", function ($scope, $mdToast, $http, rootObj) {
                    $scope.updateUserDevice = function (user) {
                       /* var request = {
                            method: "POST",
                            url: "http://192.168.0.11:53249/api/authentication/updateuserdevice",
                            data: { "test": "test" }
                        }*/
                        //$http(request).then(new function () { }, new function () { });
                        $mdToast.show(
                                        $mdToast.simple()
                                        .content(user)
                                        .position("top right")
                                        .hideDelay(1500)
                                    );
                    }
                    //rootObj is block
                    //nextRootObj is farm

                    $scope.id = rootObj.id;

                    $scope.post = function (event) {
                        event.preventDefault();

                      //  $scope.errorMessage = "";

                       // if (!event.target.checkValidity) {
                       //     return false;
                      //  }

                        //  $scope.loading = true;

                       // {
                        //    BlockID: rootObj.id,
                        //    BlockName: $scope.newBlockName
                       // }

                        $http.put("/api/authentication/updateuserdevice", {}).then(function (response) {
                            $scope.loading = false;
                            $mdToast.show(
                                $mdToast.simple()
                                .content( + "  changed to ")
                                .position("top right")
                                .hideDelay(1500)
                            );
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