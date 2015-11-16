angular.module("appMain")
    .service("editUserRolesService", ["$http", "usersUrlService", function ($http, usersUrlService) {

        this.loadUsers = function (callback) {
            $http.get(usersUrlService.getAllUrl).then(function(response) {
               callback(response.data);
            });


        }

    }]);