angular.module("appMain")
    .controller("EditFarmsCtrl", [
        "$scope", "$mdDialog", "$mdToast", "farmManagementService", function ($scope, $mdDialog, $mdToast, farmManagementService) {

            $scope.farms = [];

            function initFarms(farms) {
                $scope.farms = [];
                farms.Farms.forEach(function(frm) {
                    var blocks = new Array();

                    frm.Blocks.forEach(function(blck) {
                        blocks.push({ id: blck.BlockID, name: blck.BlockName });
                    });
                    $scope.farms.push({ farmId: frm.FarmID, collapseIcon: "expand_more", farmName: frm.FarmName, blockNames: blocks });
                });
            }

            farmManagementService.loadFarms(initFarms);

            //Toggle the icon for collapsing toolbar content
            $scope.toggleCollapseIcon = function(item) {
                if (item.collapseIcon === "expand_more")
                    item.collapseIcon = "expand_less";
                else
                    item.collapseIcon = "expand_more";
            };

            //--------------------------------------------Deleting a farm dialog-------------------------------------------
            $scope.showDeleteFarmDialog = function (event, ctrl, farm) {
                $scope.showDialog("deleteFarm", event, ctrl, function() {
                        farmManagementService.loadFarms(initFarms);
                }, null, farm);
            }

            //-----------------------------------------------Deleteing a block dialog--------------------------------------------
            $scope.showDeleteBlockDialog = function (event, ctrl, block) {
                $scope.showDialog("deleteBlock", event, ctrl, function () {
                    farmManagementService.loadFarms(initFarms);
                }, null, block);
            }


            //------------------------------------------------EditBlockDialog-----------------------------------------------------------
            $scope.showEditBlockDialog = function (event, ctrl, block) {
                $scope.showDialog("editBlock", event, ctrl, function() {
                    farmManagementService.loadFarms(initFarms);
                }, null, block);
            };

            //todo:hardcoded block idin controller
            //------------------------------------------------AddBlock dialog------------------------------------------------------------------
            $scope.showAddBlockDialog = function(event, ctrl, farm) {
                $scope.showDialog("addBlock", event, ctrl, function () {
                        farmManagementService.loadFarms(initFarms);
                }, null, farm);
            }

            //todo: hardcoded farm id in controller
            //-----------------------------------------------------AddFarm dialog--------------------------------------------------------------
            $scope.showAddFarmDialog = function (event, ctrl) {
                $scope.showDialog("addFarm", event, ctrl, function () {
                        farmManagementService.loadFarms(initFarms);
                }, null, null);
            }

            //-------------------------------------------------Common Helper functions-------------------------------------
            $scope.showDialog = function(type, event, ctrl, hiddenCallback, cancelCallback, obj) {
                $mdDialog.show({
                    templateUrl: "/App/Views/" + type + "Dialog.html",
                    parent: angular.element(document.body),
                    targetEvent: event,
                    clickOutsideToClose: true,
                    controller: ctrl,
                    locals: {rootObj : obj}
                }).then(hiddenCallback, cancelCallback);
            }
        }
    ]);
           