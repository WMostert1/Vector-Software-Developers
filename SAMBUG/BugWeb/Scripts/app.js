var app = angular.module("appMain", ["ngMaterial", "ngMdIcons", "ngResource"]);

/*
    Angular App configuration
*/
app.config(function ($mdThemingProvider) {
    //todo consider customizing the theme

    //configure site theme
    $mdThemingProvider.theme("default")
        .backgroundPalette("grey", {
            "default": "800"
        })
        .warnPalette("red")
        .primaryPalette("light-green")
        .accentPalette("amber").dark();

    //to use a dark theme: theme.dark()
});