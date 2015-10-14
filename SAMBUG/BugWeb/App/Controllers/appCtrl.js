angular.module("appMain")
    /*
    Angular App Controller
    */
    .controller("AppCtrl", ["$scope", "$mdSidenav", "$mdDialog", "$http",
        function($scope, $mdSidenav, $mdDialog, $http) {

            $scope.user = {
                email: "",
                password: "",
                isLoggedIn: isLoggedIn,
                isGrower: isGrower,
                isAdmin: isAdmin
            }

            //-----------event handlers--------------//
            function navigateToHome(event) {
                console.log("navigating");
                window.location = "/";
            };

            function navigateToLogout(event) {
                window.location = "/authentication/logout";
            };

            function showLoginDialog(event) {
                $scope.showDialog("login", event);
            };

            function showRegisterDialog(event) {
                $scope.showDialog("register", event);
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
                    toggleCollapseIcon: toggleCollapseIcon
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
                        { title: "Map" },
                        { title: "Charts" },
                        { title: "Tables" }
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
                            { title: "Blocks" },
                            { title: "Farms" },
                            { title: "Spray Data" }
                        ]
                    }
                ]
            };

           $scope.showDialog = function(type, event) {
                $mdDialog.show({
                    templateUrl: "/App/Views/" + type + "Dialog.html",
                    scope: $scope,
                    parent: angular.element(document.body),
                    targetEvent: event,
                    clickOutsideToClose: true,
                    controller: ["$scope", "$mdDialog", "$mdToast", function(scope, mdDialog, mdToast) {
                            scope.hide = function () {
                                mdDialog.hide();
                            };

                            scope.cancel = function() {
                                mdDialog.cancel();
                            };
                        
                            scope.post = function (event) {
                                event.preventDefault();
                                $http.post("/authentication/login", {
                                    username: scope.user.email,
                                    password: scope.user.password
                                }).success(function(data) {
                                    if (data.success === true) {
                                        mdDialog.hide();
                                        mdToast.show(
                                              mdToast.simple()
                                                .content("You are now logged in")
                                                .position("top right")
                                                .hideDelay(4000)
                                        );
                                        scope.user.isLoggedIn = true;
                                        scope.user.isGrower = data.isGrower;
                                        scope.user.isAdmin = data.isAdmin;
                                    }
                                });
                            }
                        }
                    ]
                });
            }

        }
    ]);