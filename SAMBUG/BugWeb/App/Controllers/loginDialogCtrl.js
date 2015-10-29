angular.module("appMain").controller("LoginDialogCtrl", [
    "$scope", "$mdDialog", "$mdToast", "$http", "userDetails", function($scope, $mdDialog, $mdToast, $http, userDetails) {

        $scope.emailPassword = function () {

        }

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

            $http.post(event.target.action, {
                username: $scope.user.email,
                password: $scope.user.password
            }).then(function(response) {
                $scope.loading = false;
                if (response.data.success === true) {
                    $mdToast.show(
                        $mdToast.simple()
                        .content("You are now logged in")
                        .position("top right")
                        .hideDelay(1500)
                    );

                    //if user was redirected to log in, redirect back to returnUrl
                    if (userDetails.returnUrl !== "") {
                        window.location = userDetails.returnUrl;
                    } else {
                        $mdDialog.hide({
                            user: {
                                isLoggedIn: true,
                                isGrower: response.data.isGrower,
                                isAdmin: response.data.isAdmin
                            }
                        });
                    }
                } else {
                    $scope.errorMessage = {incorrectCredentials: true};
                }
            }, function() {
                $scope.loading = false;
                $scope.errorMessage = {connectionError: true};
            });

            return true;
        }
    }
]);