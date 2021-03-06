﻿angular.module("appMain")
    .controller("SprayDataCtrl", [
        "$scope", "$mdDialog", "$mdToast", "treatmentDataService", function($scope, $mdDialog, $mdToast, treatmentDataService) {

            $scope.farms = [];

            function initTreatments(treatments) {
                $scope.farms = [];
                var nextDate;
                var lastDate;
                var pests;
                
                treatments.Farms.forEach(function (frm) {
                    var blocks = new Array();

                    frm.Blocks.forEach(function (blck) {
                        if (blck.BlockName === "Test W") {
                        }
                            
                        if (blck.LastTreatment === "-") {
                            lastDate = "-";
                        } else {
                            lastDate = Math.floor(new XDate(blck.LastTreatment).diffWeeks(new XDate()));
                        }

                        if (blck.NextTreatment === "-") {
                            nextDate = "-";
                        }
                        else
                        {
                            nextDate = Math.floor(new XDate().diffWeeks(new XDate(blck.NextTreatment)));
                        }

                        if (blck.PestsPerTree === -1) {
                            pests = "-";
                        } else {
                            pests = blck.PestsPerTree.toFixed(2);
                        }
                            
                        if (lastDate === 0) {
                            lastDate = "This week";
                        }
                        else if (lastDate === 1) {
                            lastDate = "Last week";
                        }
                        else if (lastDate !== "-") {
                            lastDate = lastDate + " weeks ago";
                        }

                        if (nextDate === 0) {
                            nextDate = "This week";
                        }
                        else if (nextDate === 1) {
                            nextDate = "Next week";
                        }
                        else if (nextDate !== "-") {
                            nextDate = "In " + nextDate + " weeks";
                        }

                        blocks.push({ id: blck.BlockID, name: blck.BlockName, pestsPerTree: pests, lastSpray: lastDate, nextSpray: nextDate });
                    });

                    $scope.farms.push({ farmId: frm.FarmID, collapseIcon: "expand_more", farmName: frm.FarmName, blocks: blocks });
                });
            }

            treatmentDataService.loadTreatments(initTreatments);

            //Toggle the icon for collapsing toolbar content
            $scope.toggleCollapseIcon = function (item) {
                if (item.collapseIcon === "expand_more")
                    item.collapseIcon = "expand_less";
                else
                    item.collapseIcon = "expand_more";
            };

            //-----------------------------------------------------AddTreatment--------------------------------------------------------------
            //todo: get new data from server and set new farms object
            $scope.showAddTreatmentDialog = function (event, ctrl, block) {
                $scope.showDialog("addTreatment", event, ctrl, function () {
                    treatmentDataService.loadTreatments(initTreatments);
                }, null, block);
            }

            $scope.goToHistory = function () {
                window.location = "/reporting/tables";
            }

            //-------------------------------------------------Common Helper functions-------------------------------------
            $scope.showDialog = function (type, event, ctrl, hiddenCallback, cancelCallback, obj) {
                $mdDialog.show({
                    templateUrl: "/App/Views/" + type + "Dialog.html",
                    parent: angular.element(document.body),
                    targetEvent: event,
                    clickOutsideToClose: true,
                    controller: ctrl,
                    locals: { obj: obj }
                }).then(hiddenCallback, function() {
                });
            }
        }
    ]);
