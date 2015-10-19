angular.module("appMain", ["ngMaterial", "ngMdIcons", "ngMessages"])
/*
    Angular App configuration
*/
    .config(function($mdThemingProvider) {
        //configure site theme

        //todo consider customizing the theme
        $mdThemingProvider.theme("default")
            .backgroundPalette("grey")
            .warnPalette("red")
            .primaryPalette("light-green")
            .accentPalette("amber").dark();

    });