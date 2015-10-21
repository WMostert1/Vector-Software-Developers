angular.module("appMain")
    .controller("UserManagementCtrl", [
        "$scope", "$rootScope", "$mdDialog", "$mdToast", function ($scope, $rootScope, $mdDialog, $mdToast) {

            $scope.users = [
                {
                    userId: "1",
                    email: "Lynn@gmail.com",
                    roles: [
                        {
                            id: "1",
                            name: "grower"
                        },
                        {
                            id: "2",
                            name: "admin"
                        }
                    ]
                },
                {
                    userId: "2",
                    email: "Phill@gmail.com",
                    roles: [
                        {
                            id: "1",
                            name: "grower"
                        }
                    ]
                },
                {
                    userId: "3",
                    email: "Jane@gmail.com",
                    roles: [
                        {
                            id: "0",
                            name: "admin"
                        }
                    ]
                }
            ];

            //------------------------------------------------EditBlockDialog-----------------------------------------------------------
            $scope.showEditUserRolesDialog = function (event, ctrl, user) {
                $rootScope.userEmail = user.email;
                $rootScope.roles = user.roles;
                $scope.showDialog("editUserRoles", event, ctrl, null, null);
            };

            //-------------------------------------------------Common Helper functions-------------------------------------
            $scope.showDialog = function (type, event, ctrl, hiddenCallback, cancelCallback) {
                $mdDialog.show({
                    templateUrl: "/App/Views/" + type + "Dialog.html",
                    parent: angular.element(document.body),
                    targetEvent: event,
                    clickOutsideToClose: true,
                    controller: ctrl
                }).then(hiddenCallback, cancelCallback);
            }
        }
    ]);
