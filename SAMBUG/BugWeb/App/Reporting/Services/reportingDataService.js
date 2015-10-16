angular.module("reporting")
    .service('reportingDataService', ["$http", function ($http) {
        this.fetch = function(url, success) {
            $http.get(url, { cache: true }).then(success);
        };
    }]);