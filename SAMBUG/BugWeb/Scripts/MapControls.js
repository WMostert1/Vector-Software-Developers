var map, heatmap;
var stops = {};
function initMap() {
    $.get("../api/reporting/1", function (data) {
        stops = data;

  
    }).done(function () {
        
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 13,
           
            center: { lat: stops.ScoutStops[0].Latitude, lng: stops.ScoutStops[0].Longitude },
            mapTypeId: google.maps.MapTypeId.SATELLITE
        });

        heatmap = new google.maps.visualization.HeatmapLayer({
            data: getPoints(),
            map: map
        });
        
    });
}

function getPoints() {
    var mapsArr = [];
   
    for (var i = 0; i < stops.ScoutStops.length;i++) {
   
        mapsArr.push(new google.maps.LatLng(stops.ScoutStops[i].Latitude, stops.ScoutStops[i].Longitude));
    }

    return mapsArr;

}


function toggleHeatmap() {
    heatmap.setMap(heatmap.getMap() ? null : map);
}

function changeGradient() {
    var gradient = [
      'rgba(0, 255, 255, 0)',
      'rgba(0, 255, 255, 1)',
      'rgba(0, 191, 255, 1)',
      'rgba(0, 127, 255, 1)',
      'rgba(0, 63, 255, 1)',
      'rgba(0, 0, 255, 1)',
      'rgba(0, 0, 223, 1)',
      'rgba(0, 0, 191, 1)',
      'rgba(0, 0, 159, 1)',
      'rgba(0, 0, 127, 1)',
      'rgba(63, 0, 91, 1)',
      'rgba(127, 0, 63, 1)',
      'rgba(191, 0, 31, 1)',
      'rgba(255, 0, 0, 1)'
    ]
    heatmap.set('gradient', heatmap.get('gradient') ? null : gradient);
}

function changeRadius() {
    heatmap.set('radius', heatmap.get('radius') ? null : 40);
}

function changeOpacity() {
    heatmap.set('opacity', heatmap.get('opacity') ? null : 0.2);
}


