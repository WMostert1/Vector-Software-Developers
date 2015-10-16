angular.module("reporting", ["ngRoute", "ngMdIcons"])
    .config(["$routeProvider", "$locationProvider", function($routeProvider, $locationProvider) {
        $routeProvider.when("/reporting/map", { templateUrl: "/App/Reporting/Views/map.html", controller: "MapCtrl"});
        $routeProvider.when("/reporting/charts", { templateUrl: "/App/Reporting/Views/charts.html", controller: "ChartsCtrl"});
        $routeProvider.when("/reporting/tables", { templateUrl: "/App/Reporting/Views/tables.html", controller: "TablesCtrl"});
        $routeProvider.otherwise({redirectTo: "/reporting/map"});
            $locationProvider.html5Mode(true);
        }]
);