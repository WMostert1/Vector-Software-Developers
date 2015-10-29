angular.module("appMain")
    .controller("EditUserRolesDialogCtrl", [
                "$scope", "$rootScope", "$mdDialog", "$mdToast", "$http", "user", function ($scope, $rootScope, $mdDialog, $mdToast, $http, user) {

                    var grower = false;
                    var admin = false;

                    for (var x = 0; x < user.roles.length; x++) {
                        if (user.roles[x].id === 1) {
                            grower = true;
                        }
                        if (user.roles[x].id === 2) {
                            admin = true;
                        }
                    }

                    $scope.userGrower = grower;
                    $scope.userAdmin = admin;
                    $scope.userName = user.email;

                    $scope.cancel = function () {
                        $mdDialog.cancel();
                    };

                    //The moment the submit button is pressed, this function is called
                    $scope.post = function (event) {
                        event.preventDefault();

                        $scope.errorMessage = "";

                        if (!event.target.checkValidity) {
                            return false;
                        }

                        $scope.loading = true;

                        var changed = false;
                        var toastMsg;

                        //see if the checkbox was changed or not
                        if (admin !== $scope.userAdmin) {
                            changed = true;
                        }

                        //determine toast message based on user action
                        if ($scope.userAdmin) {
                            toastMsg = user.email + " is now an administrator";
                        }
                        else {
                            toastMsg = user.email + " is no longer an administrator";
                        }

                        //if user did actually change a role, we make an http put request to update the roles, and also display a toast
                        if (changed === true) {
                            $http.put(event.target.action, {
                                UserId: user.id,
                                IsAdministrator: $scope.userAdmin
                            }).then(function (response) {
                                $scope.loading = false;

                                if (response.data.Success === true) {
                                    $mdToast.show(
                                        $mdToast.simple()
                                        .content(toastMsg)
                                        .position("top right")
                                        .hideDelay(2000));
                                }
                                else {
                                    $scope.errorMessage = "Could not change user roles";
                                }
                            }, function () {
                                $scope.loading = false;
                                $scope.errorMessage = "Trouble contacting server. Please try again.";
                            });
                        }

                        //whether or not a user actualy changed the roles, we hide the dialog and return a boolean of whether anything changed or not, to know whether wee should request new data from the server
                        $mdDialog.hide({ changed: changed });

                        return true;
                    }
                }
    ]);