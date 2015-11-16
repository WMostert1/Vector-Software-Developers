angular.module("appMain")
    .service("treatmentDataService", ["$http", "treatmentDataUrlService", function ($http, treatmentDataUrlService) {

        this.loadTreatments = function (callback) {
            $http.get(treatmentDataUrlService.getTreatmentInfoUrl).then(function (response) {
                callback(response.data);
            });


        }

    }]);