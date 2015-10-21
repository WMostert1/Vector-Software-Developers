angular.module("appMain")
    .controller("SprayDataCtrl", [
        "$scope", "$mdDialog", "$mdToast", function($scope, $mdDialog, $mdToast) {

            $scope.farms = [
                {
                    farmId: "1",
                    collapseIcon: "expand_more",
                    farmName: "Farm 1",
                    blocks: [
                        {
                            id: "1",
                            name: "Block 1",
                            pestsPerTree: 5.11,
                            lastSpray: 40,
                            nextSpray: 5
                        },
                        {
                            id: "2",
                            name: "Block 2",
                            pestsPerTree: 5,
                            lastSpray: 5,
                            nextSpray: "-"
                        },
                        {
                            id: "3",
                            name: "Block 3",
                            pestsPerTree: 7.1,
                            lastSpray: 2,
                            nextSpray: 5
                        }
                    ]
                },
                {
                    farmId: "2",
                    collapseIcon: "expand_more",
                    farmName: "Farm 2",
                    blocks: [
                        {
                            id: "1",
                            name: "Block A",
                            pestsPerTree: 4,
                            lastSpray: 20,
                            nextSpray: 1
                            
                        },
                        {
                            id: "2",
                            name: "Block B",
                            pestsPerTree: 6.4,
                            lastSpray: 10,
                            nextSpray: 5
                        },
                        {
                            id: "3",
                            name: "Block C",
                            pestsPerTree: 9,
                            lastSpray: 15,
                            nextSpray: "-"
                        }
                    ]
                }
            ];

            //Toggle the icon for collapsing toolbar content
            $scope.toggleCollapseIcon = function (item) {
                if (item.collapseIcon === "expand_more")
                    item.collapseIcon = "expand_less";
                else
                    item.collapseIcon = "expand_more";
            };

            //-----------------------------------------------------AddTreatment--------------------------------------------------------------
            //todo: get new data from server and set new farms object
            $scope.showAddTreatmentDialog = function (event, ctrl, index, farm) {
                $scope.showDialog("addTreatment", event, ctrl, function (date) {
                }, null);
            }

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
