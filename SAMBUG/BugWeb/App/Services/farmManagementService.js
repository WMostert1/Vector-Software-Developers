﻿angular.module("appMain")
    .service("farmManagementService", ["$http", "farmsUrlService", function ($http, farmsUrlService) {

        this.loadFarms = function (callback) {
            $http.get(farmsUrlService.getAllFarmsUrl).then(function (response) {
                callback(response.data);
            });


        }

    }]);