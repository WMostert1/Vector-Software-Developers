angular.module("appMain")
    /*
    Angular App Controller
    */
    .controller("AppCtrl", [
        "$scope", "$mdSidenav", "$mdDialog", "userDetails",
        function($scope, $mdSidenav, $mdDialog, userDetails) {

            $scope.user = $scope.user || {
                email: "",
                password: "",
                isLoggedIn: userDetails.isLoggedIn,
                isGrower: userDetails.isGrower,
                isAdmin: userDetails.isAdmin,
                returnUrl: userDetails.returnUrl
            }

            //-----------event handlers--------------//
            function navigateToHome(event) {
                window.location = "/";
            };

            function navigateToEditUserRoles(event) {
                window.location = "/authentication/edituserroles";
            };

            function navigateToLogout(event) {
                window.location = "/authentication/logout";
            };

            function navigateToMap(event) {
                window.location = "/reporting/map";
            };

            function navigateToCharts(event) {
                window.location = "/reporting/charts";
            };

            function navigateToTables(event) {
                window.location = "/reporting/tables";
            };

            function navigateToEditFarms(event) {
                window.location = "/farmmanagement/editfarms";
            }

            function navigateToTreatments(event) {
                window.location = "/farmmanagement/spraydata";
            }

            function showLoginDialog(event) {
                $scope.showDialog("login", event, "LoginDialogCtrl", function(status) {
                    $scope.user.isLoggedIn = status.user.isLoggedIn;
                    $scope.user.isGrower = status.user.isGrower;
                    $scope.user.isAdmin = status.user.isAdmin;
                });
            };

            function showRegisterDialog(event) {
                $scope.showDialog("register", event, "RegisterDialogCtrl", function(status) {
                    $scope.user.isLoggedIn = status.user.isLoggedIn;
                    $scope.user.isGrower = status.user.isGrower;
                    $scope.user.isAdmin = status.user.isAdmin;
                });
            };

            function toggleCollapseIcon(item) {
                if (item.collapseIcon === "expand_more")
                    item.collapseIcon = "expand_less";
                else
                    item.collapseIcon = "expand_more";
            };

            //--------------------------------------//


            //--------------Filters-----------------//

            $scope.onlyApplicableItems = function(value) {
                return value.mustShow();
            };

            //--------------------------------------//
            
            $scope.navigateToAboutUs = function () {
                window.location = "/home/aboutus";
            }

            $scope.toggleSideNav = function(id) {
                $mdSidenav(id).toggle();
            };

            $scope.navMenu = {
                actions: {
                    navigateHome: navigateToHome,
                    navigateToLogout: navigateToLogout,
                    showLoginDialog: showLoginDialog,
                    showRegisterDialog: showRegisterDialog
                },
                items: [
                    {
                        actionName: "navigateHome",
                        mustShow: function() {
                            return true;
                        },
                        icon: "home",
                        title: "Home"
                    },
                    {
                        actionName: "showLoginDialog",
                        mustShow: function() {
                            return !$scope.user.isLoggedIn;
                        },
                        icon: "login",
                        title: "Log In"
                    },
                    {
                        actionName: "showRegisterDialog",
                        mustShow: function() {
                            return !$scope.user.isLoggedIn;
                        },
                        icon: "person",
                        title: "Register"
                    },
                    {
                        actionName: "navigateToLogout",
                        mustShow: function() {
                            return $scope.user.isLoggedIn;
                        },
                        icon: "logout",
                        title: "Log Out"
                    }
                ]
            };

            $scope.managementMenu = {
                actions: {
                    toggleCollapseIcon: toggleCollapseIcon,
                    navigateToMap: navigateToMap,
                    navigateToCharts: navigateToCharts,
                    navigateToTables: navigateToTables,
                    navigateToEditFarms: navigateToEditFarms,
                    navigateToTreatments: navigateToTreatments,
                    navigateToEditUserRoles: navigateToEditUserRoles
                },
                items: [
                    {
                        actionName: "toggleCollapseIcon",
                        mustShow: function() {
                            return $scope.user.isLoggedIn;
                        },
                        icon: "equalizer",
                        collapseIcon: "expand_more",
                        collapseTargetId: "reportingSubmenu",
                        title: "Reporting",
                        subList: [
                            {
                                actionName: "navigateToMap",
                                title: "Map"
                            },
                            {
                                actionName: "navigateToCharts",
                                title: "Charts"
                            },
                            {
                                actionName: "navigateToTables",
                                title: "Tables"
                            }
                        ]
                    },
                    {
                        actionName: "toggleCollapseIcon",
                        mustShow: function() {
                            return $scope.user.isGrower;
                        },                        
                    	icon: "nature",
                   	 	collapseIcon: "expand_more",
                    	collapseTargetId: "farmManagementSubMenu",
                    	title: "Farm Management",
                    	subList: [
                        	{
                            	actionName: "navigateToEditFarms",
                            	title: "Farms & Blocks"
                        	},
                        	{
                            	actionName: "navigateToTreatments",
                            	title: "Spray Data"
                        	}
                        ]
                    }
                ],
                userManagement: {
                    actionName: "navigateToEditUserRoles",
                    icon: "people",
                    title: "User Management"
                }
            };

            $scope.showDialog = function(type, event, ctrl, hiddenCallback, cancelCallback) {
                $mdDialog.show({
                    templateUrl: "/App/Views/" + type + "Dialog.html",
                    parent: angular.element(document.body),
                    targetEvent: event,
                    clickOutsideToClose: true,
                    controller: ctrl
                }).then(hiddenCallback, cancelCallback);
            }

            //if user was redirected to this page, open the login dialog
            if ($scope.user.returnUrl !== "") {
                showLoginDialog(null);
            }

        }
    ]);

