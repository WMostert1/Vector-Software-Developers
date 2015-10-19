angular.module("appMain")
    .service("mapService", [function () {
        
        function filterDataScout() {
            var farm = $("#farmScout").val();
            var block = $("#blocksScout").val();
            var fromDate = new Date($("#timeFromScout").val());
            var toDate = new Date($("#timeToScout").val());
            var speciesLifeStage = $("#speciesStageScout").val();
            var speciesName = $("#speciesScout").val();

            console.log(fromDate);
            console.log(toDate);

            var appliedFilters = scoutStopObjects.filter(function (obj) {
                if ((obj.farmName === farm || farm === "all") &&
                (obj.blockName === block || block === "all") &&
                ((new Date(obj.date) >= fromDate && new Date(obj.date) <= toDate) || $("#dateAnyScout").is(":checked")) &&
                (speciesLifeStage === "all" || (obj.lifeStage).toString() === speciesLifeStage) &&
                (speciesName === "all" || obj.speciesName === speciesName)) {
                    return true;
                }
            });

            return appliedFilters;
        };

        var map, heatmap;

        function getPoints(stops) {
            var mapsArr = [];
            for (var i = 0; i < stops.length; i++) {
                mapsArr.push({ location: new google.maps.LatLng(stops[i].latitude, stops[i].longitude), weight: stops.numOfBugs });
            }
            return mapsArr;
        }

        this.initMap = function (stops, mapId) {
            console.log(stops);

            if (stops.length < 1)
                return;

            map = new google.maps.Map(document.getElementById(mapId), {
                zoom: 13,
                center: { lat: stops[0].latitude, lng: stops[0].longitude },
                mapTypeId: google.maps.MapTypeId.SATELLITE
            });

            heatmap = new google.maps.visualization.HeatmapLayer({
                data: getPoints(stops),
                dissipating: false,
                map: map
            });

            this.changeRadius(0.001);
        }

        this.updateMap = function (stops) {
            if(heatmap && stops.length > 0)
                heatmap.setData(getPoints(stops));
        }
        
        this.toggleGradient = function () {
            if (!heatmap)
                return;

            var gradient = [
                "rgba(0, 255, 255, 0)",
                "rgba(0, 255, 255, 1)",
                "rgba(0, 191, 255, 1)",
                "rgba(0, 127, 255, 1)",
                "rgba(0, 63, 255, 1)",
                "rgba(0, 0, 255, 1)",
                "rgba(0, 0, 223, 1)",
                "rgba(0, 0, 191, 1)",
                "rgba(0, 0, 159, 1)",
                "rgba(0, 0, 127, 1)",
                "rgba(63, 0, 91, 1)",
                "rgba(127, 0, 63, 1)",
                "rgba(191, 0, 31, 1)",
                "rgba(255, 0, 0, 1)"
            ];

            heatmap.set("gradient", heatmap.get("gradient") ? null : gradient);

        }

        this.changeRadius = function (radius) {
            if (!heatmap)
                return;

            if (heatmap.get("radius"))
                heatmap.set("radius", null);
            heatmap.set("radius", radius);
        }

    }]);

