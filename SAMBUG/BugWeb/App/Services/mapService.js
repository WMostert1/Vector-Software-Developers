angular.module("appMain")
    .service("mapService", [function () {
        
        var map, heatmap;

        function getPoints(stops) {
            var mapsArr = [];
            for (var i = 0; i < stops.length; i++) {
                mapsArr.push({ location: new google.maps.LatLng(stops[i].latitude, stops[i].longitude), weight: stops.numOfBugs });
            }
            return mapsArr;
        }

        this.initMap = function (mapId) {
            map = new google.maps.Map(document.getElementById(mapId), {
                zoom: 13,
                mapTypeId: google.maps.MapTypeId.SATELLITE
            });

            heatmap = new google.maps.visualization.HeatmapLayer({
                dissipating: false,
                map: map
            });

            this.changeRadius(0.001);
        }

        this.resize = function() {
            if (map) {
                var center = map.getCenter();
                google.maps.event.trigger(map, "resize");
                map.setCenter(center);
            }
        }

        this.updateMap = function (stops) {
            if (heatmap && stops.length > 0) {
                map.setCenter({ lat: stops[0].latitude, lng: stops[0].longitude });
                heatmap.setData(getPoints(stops));
            }
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

