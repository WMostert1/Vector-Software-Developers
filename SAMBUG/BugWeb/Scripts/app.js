angular.module("appMain", ["ngMaterial", "ngMdIcons", "ngResource"])
/*
    Angular App configuration
*/
    .config(function($mdThemingProvider) {
        //configure site theme

        //todo consider customizing the theme
        $mdThemingProvider.theme("default")
            .backgroundPalette("grey", {
               "default": "800"
            })
            .warnPalette("red")
            .primaryPalette("light-green")
            .accentPalette("amber").dark();
    });