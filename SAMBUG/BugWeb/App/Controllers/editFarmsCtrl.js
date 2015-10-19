angular.module("appMain")
    .controller("EditFarmsCtrl", [
        "$scope", "$rootScope", "$mdDialog", "$mdToast", function($scope, $rootScope, $mdDialog, $mdToast) {

            $scope.farms = [
                {
                    farmId: "1",
                    collapseIcon: "expand_more",
                    farmName: "Farm 1",
                    blockNames: [
                    {
                        id: "1",
                        name: "Block 1"
                    },
                    {
                        id: "2",
                        name: "Block 2"
                    },
                    {
                         id: "3",
                         name: "Block 3"
                    }
                    ]
                },
                {
                    farmId: "2",
                    collapseIcon: "expand_more",
                    farmName: "Farm 2",
                    blockNames: [
                    {
                        id: "1",
                        name: "Block A"
                    },
                    {
                        id: "2",
                        name: "Block B"
                    },
                    {
                        id: "3",
                        name: "Block C"
                    }
                    ]
                }
            ];

            //Toggle the icon for collapsing toolbar content
            $scope.toggleCollapseIcon = function(item) {
                if (item.collapseIcon === "expand_more")
                    item.collapseIcon = "expand_less";
                else
                    item.collapseIcon = "expand_more";
            };

            //--------------------------------------------Deleting a farm-------------------------------------------
            $scope.showDeleteFarmDialog = function (event, ctrl, farm) {
                $rootScope.farmToDelete = farm.farmName;
                $scope.showDialog("deleteFarm", event, ctrl, function() {
                    var i = $scope.farms.indexOf(farm);
                    $scope.farms.splice(i, 1);
                }, null);
            }

            //-----------------------------------------------Deleteing a block--------------------------------------------
            $scope.showDeleteBlockDialog = function (event, ctrl, blockIndex, farm) {
                $rootScope.blockToDelete = farm.blockNames[blockIndex].name;
                $scope.showDialog("deleteBlock", event, ctrl, function () {
                    farm.blockNames.splice(blockIndex, 1);
                }, null);
            }


            //------------------------------------------------EditBlockDialog-----------------------------------------------------------
            $scope.showEditBlockDialog = function (event, ctrl, blockIndex, farm) {
                $rootScope.currentBlockName = farm.blockNames[blockIndex].name;
                $scope.showDialog("editBlock", event, ctrl, function(newBlockName) {
                    farm.blockNames[blockIndex].name = newBlockName;
                }, null);
            };

            //todo:hardcoded block idin controller
            //------------------------------------------------AddBlock------------------------------------------------------------------
            $scope.showAddBlockDialog = function(event, ctrl, farm) {
                $scope.showDialog("addBlock", event, ctrl, function (newBlockObject) {
                    farm.blockNames.push({id: newBlockObject.id, name: newBlockObject.newBlockName});
                }, null);
            }

            //todo: hardcoded farm id in controller
            //-----------------------------------------------------AddFarm--------------------------------------------------------------
            $scope.showAddFarmDialog = function (event, ctrl) {
                $scope.showDialog("addFarm", event, ctrl, function (newFarmObject) {
                    console.log(newFarmObject.id);
                    $scope.farms.push({ farmId: newFarmObject.id, farmName: newFarmObject.newFarmName, collapseIcon: "expand_more", blockNames: []});
                }, null);
            }

            //-------------------------------------------------Common Helper functions-------------------------------------
            $scope.showDialog = function(type, event, ctrl, hiddenCallback, cancelCallback) {
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
           