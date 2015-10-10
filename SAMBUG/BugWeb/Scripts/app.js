var app = angular.module('appMain', [ 'ngMaterial', 'ngResource']);

/*
    Angular App configuration
*/
app.config(function ($mdThemingProvider) {
    //todo consider customizing the theme

    //configure site theme
    $mdThemingProvider.theme("default")
        .primaryPalette("light-green")
        .accentPalette("light-green");

    //to use a dark theme: theme.dark()
});