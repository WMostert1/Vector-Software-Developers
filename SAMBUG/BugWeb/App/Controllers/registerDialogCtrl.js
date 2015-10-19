angular.module("appMain").controller("RegisterDialogCtrl", [
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
            $http.post(event.target.action, {
                username: $scope.user.email,
                usernameConfirmation: $scope.user.emailConfirmation,
                password: $scope.user.password,
                passwordConfirmation: $scope.user.passwordConfirmation
            }).then(function (response) {
                $scope.loading = false;
                if (response.data.success === true) {
                    $mdToast.show(
                        $mdToast.simple()
                        .content("You are now logged in")
                        .position("top right")
                        .hideDelay(1500)
                    );
                    $mdDialog.hide({
                        user: {
                            isLoggedIn: true,
                            isGrower: response.data.isGrower,
                            isAdmin: response.data.isAdmin
                        }
                    });
                } else {
                    if (response.data.userExistsError)
                        $scope.errorMessage = {alreadyRegistered: true};
                    else if (response.data.invalidInputError)
                        $scope.errorMessage = {incorrectCredentials: true};
                }
            }, function () {
                $scope.loading = false;
                $scope.errorMessage = {connectionError: true};
            });

            return true;
        }
    }
]);