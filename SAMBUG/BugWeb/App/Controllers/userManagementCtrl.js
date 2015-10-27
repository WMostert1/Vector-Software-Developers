angular.module("appMain")
    .controller("UserManagementCtrl", [
        "$scope", "$rootScope", "$mdDialog", "$mdToast", "editUserRolesService", function ($scope, $rootScope, $mdDialog, $mdToast, editUserRolesService) {

            $scope.users = [];

            function initUsers(users) {
                $scope.users = [];
                var mail;
                var id;
                users.Users.forEach(function(usr) {
                    var user = new Object();
                    var rolesArr = new Array();
                    mail = usr.Email;
                    id = usr.UserID;

                    usr.Roles.forEach(function(rls) {
                        var role = new Object();
                        role.id = rls.RoleType;
                        role.name = rls.RoleDescription;

                        rolesArr.push(role);
                    });

                    user.id = id;
                    user.email = mail;
                    user.roles = rolesArr;
                    $scope.users.push(user);
                });
            }

            editUserRolesService.loadUsers(initUsers);

            //------------------------------------------------EditBlockDialog-----------------------------------------------------------
            $scope.showEditUserRolesDialog = function (event, ctrl, user) {
                $scope.showDialog("editUserRoles", event, ctrl, function (rolesChanged) {
                    if (rolesChanged.changed === true) {
                        editUserRolesService.loadUsers(initUsers);
                    }
                        
                } , null, user);
            };

            //-------------------------------------------------Common Helper functions-------------------------------------
            $scope.showDialog = function (type, event, ctrl, hiddenCallback, cancelCallback, user) {
                $mdDialog.show({
                    templateUrl: "/App/Views/" + type + "Dialog.html",
                    parent: angular.element(document.body),
                    targetEvent: event,
                    clickOutsideToClose: true,
                    controller: ctrl,
                    locals: {user: user}
                }).then(hiddenCallback, cancelCallback);
            }
        }
    ]);
