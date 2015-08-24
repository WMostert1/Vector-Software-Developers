var map, heatmap;

function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 13,
        center: { lat: -25.771141, lng: 28.224394 },
        mapTypeId: google.maps.MapTypeId.SATELLITE
    });

    heatmap = new google.maps.visualization.HeatmapLayer({
        data: getPoints(),
        map: map
    });
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
    heatmap.set('radius', heatmap.get('radius') ? null : 20);
}

function changeOpacity() {
    heatmap.set('opacity', heatmap.get('opacity') ? null : 0.2);
}

// Heatmap data: 500 Points
function getPoints() {
    var scoutStops = [{
        "Block": { "ScoutStops": [], "Treatments": [], "BlockID": 0, "FarmID": 0, "LastModifiedID": 0 },
        "ScoutBugs": [],
        "User": { "Farms": [], "ScoutStops": [], "LastModifiedID": 0, "RoleID": 0, "UserID": 0 },
        "BlockID": 3,
        "LastModifiedID": 3,
        "Latitude": -25.771141,
        "Longitude": 28.224394,
        "NumberOfTrees": 5,
        "ScoutStopID": 1,
        "UserID": 3
    }];

    var mapsArr = [];
  for(var stop in scoutStops)
    mapsArr.push(new google.maps.LatLng(stop.Latitude, stop.Longitude));
    
    return mapsArr;

    }

