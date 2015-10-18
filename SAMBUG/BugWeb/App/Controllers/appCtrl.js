angular.module("appMain")
    /*
    Angular App Controller
    */
    .controller("AppCtrl", ["$scope", "$mdSidenav", "$mdDialog", "$http", "userDetails",
        function($scope, $mdSidenav, $mdDialog, $http, userDetails) {

            $scope.user = {
                email: "",
                password: "",
                isLoggedIn: userDetails.isLoggedIn,
                isGrower: userDetails.isGrower,
                isAdmin: userDetails.isAdmin
            }

            //-----------event handlers--------------//
            function navigateToHome(event) {
                window.location = "/";
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


            //-----------Dialog Controllers---------//
            var LoginDialogCtrl = [
                "$scope", "$mdDialog", "$mdToast", function(scope, mdDialog, mdToast) {
                    scope.hide = function() {
                        mdDialog.hide();
                    };

                    scope.cancel = function() {
                        mdDialog.cancel();
                    };

                    scope.post = function(event) {
                        event.preventDefault();

                        scope.errorMessage = "";

                        if (!event.target.checkValidity) {
                            return false;
                        }

                        scope.loading = true;

                        $http.post(event.target.action, {
                            username: scope.user.email,
                            password: scope.user.password
                        }).then(function success(response) {
                            scope.loading = false;
                            if (response.data.success === true) {
                                mdDialog.hide();
                                mdToast.show(
                                    mdToast.simple()
                                    .content("You are now logged in")
                                    .position("top right")
                                    .hideDelay(4000)
                                );
                                $scope.user.isLoggedIn = true;
                                $scope.user.isGrower = response.data.isGrower;
                                $scope.user.isAdmin = response.data.isAdmin;
                            } else {
                                scope.errorMessage = "The email or password you entered is incorrect";
                            }
                        }, function error() {
                            scope.loading = false;
                            scope.errorMessage = "Trouble contacting server. Please try again.";
                        });

                        return true;
                    }
                }
            ];

            var RegisterDialogCtrl = [
                "$scope", "$mdDialog", "$mdToast", function (scope, mdDialog, mdToast) {
                    scope.hide = function () {
                        mdDialog.hide();
                    };

                    scope.cancel = function () {
                        mdDialog.cancel();
                    };

                    scope.post = function (event) {
                        event.preventDefault();

                        scope.errorMessage = "";

                        if (!event.target.checkValidity) {
                            return false;
                        }

                        scope.loading = true;
                        $http.post(event.target.action, {
                            username: scope.user.email,
                            usernameConfirmation: scope.user.emailConfirmation,
                            password: scope.user.password,
                            passwordConfirmation: scope.user.passwordConfirmation
                        }).then(function success(response) {
                            scope.loading = false;
                            if (response.data.success === true) {
                                mdDialog.hide();
                                mdToast.show(
                                    mdToast.simple()
                                    .content("You are now logged in")
                                    .position("top right")
                                    .hideDelay(4000)
                                );
                                $scope.user.isLoggedIn = true;
                                $scope.user.isGrower = response.data.isGrower;
                                $scope.user.isAdmin = response.data.isAdmin;
                            } else {
                                if (response.data.userExistsError)
                                    scope.errorMessage = "This email address is already registered";
                                else if (response.data.invalidInputError)
                                    scope.errorMessage = "The details you have entered are incorrect";
                            }
                        }, function error() {
                            scope.loading = false;
                            scope.errorMessage = "Trouble contacting server. Please try again.";
                        });

                        return true;
                    }
                }
            ];

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
                    toggleCollapseIcon: toggleCollapseIcon,
                    navigateToMap: navigateToMap,
                    navigateToCharts: navigateToCharts,
                    navigateToTables: navigateToTables
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
                            { title: "Blocks" },
                            { title: "Farms" },
                            { title: "Spray Data" }
                        ]
                }]
            };
            
           $scope.showDialog = function(type, event) {
               $mdDialog.show({
                   templateUrl: "/App/Views/" + type + "Dialog.html",
                   parent: angular.element(document.body),
                   targetEvent: event,
                   clickOutsideToClose: true,
                   controller: type === "login" ? LoginDialogCtrl : RegisterDialogCtrl
           });
            }

        }
    ]);