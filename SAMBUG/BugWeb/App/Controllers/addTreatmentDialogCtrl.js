angular.module("appMain")
    .controller("AddTreatmentDialogCtrl", [
                "$scope", "$mdDialog", "$mdToast", "$http", "obj", function ($scope, $mdDialog, $mdToast, $http, obj) {
                    $scope.newDate = new Date();
                    $scope.cancel = function () {
                        $mdDialog.cancel();
                    };

                    $scope.post = function (event) {
                        event.preventDefault();
                        $scope.errorMessage = "";

                        if ($scope.newDate == null || $scope.newDate === "" || Object.prototype.toString.call($scope.newDate) !== '[object Date]') {
                            console.log("Empty");
                            return false;
                        }

                        $scope.loading = true;
                        if (!event.target.checkValidity) {
                            return false;
                        }

                        $http.post(event.target.action, {
                            BlockID: obj.id,
                            TreatmentDate: new XDate($scope.newDate).toString("yyyy-MM-dd"),
                            TreatmentComments: $scope.comments
                        }).then(function (response) {
                            $scope.loading = false;
                            $mdToast.show(
                                $mdToast.simple()
                                .content("Spray data was added successfully")
                                .position("top right")
                                .hideDelay(1500)
                            );
                            $mdDialog.hide();
                        }, function (response) {
                            $scope.loading = false;
                            if (response.status === 400) {
                                $scope.errorMessage = { invalidInput: true };
                            }

                        });

                        return true;
                    }
                }
    ]);