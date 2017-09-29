var mapiHotel;//Πρόσβαση στο χάρτη google map
var gmarkers = [];//location λόων των markers που μέχρι τώρα προστέθηκαν στο χάρτη

//φόρτωση σελίδας
$(document).ready(function () {
//αρχικές συντεταγμένες φόρτωσης χάρτη google map
    var lati = 40.59958;
    var loni = 22.96318;
    var latlng = new google.maps.LatLng(lati, loni);
    var myOptions = {
        zoom: 2,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
    };
    //handler στο χάρτη
    mapiHotel = new google.maps.Map(document.getElementById("mapi"), myOptions);

    
});

//προσθήκη marker - καλείται όταν γίνεται το πρώτο σχετικό tweet
function addMarker(location) {    
//αφαιρούμε όσους markers τυχόν έχουμε βάλει στο χάρτη
    removeMarkers();

    //προσθήκη marker στο χάρτη
    marker = new google.maps.Marker({
        position: location,
        map: mapiHotel,
        icon: customImage

    });
    //αποθήκευση συντεταγμένων τρέχοντα marker για να μπορούμε μετά να τον αφαιρέσουμε
    gmarkers.push(marker);
    //κεντράρουμε το χάρτη στο σημείο που μπήκε ο marker
    mapiHotel.setCenter(location);
}

//προσθήκη marker με animation, όχι άμεση προσθήκη marker αλλά μετά από το χρόνο που ορίζει το timeout
function addMarkerWithTimeout(position, timeout) {
    window.setTimeout(function () {
        gmarkers.push(new google.maps.Marker({
            position: position,
            map: mapiHotel,
            animation: google.maps.Animation.DROP
        }));
    }, timeout);
}

//προσθήκη marker χωρίς αφαίρεση υπαρχόντων markers με χρονική καθυστέρηση
function addOnlyMarker(location) {
    marker = new google.maps.Marker({
        position: location,
        map: mapiHotel
    });
    gmarkers.push(marker);
    mapiHotel.setCenter(location)
}
//αφαίρεση όλων των markers που έχουν οριστεί μέχρι τώρα
    function removeMarkers(){
        for(i=0; i<gmarkers.length; i++){
            gmarkers[i].setMap(null);
        }
    }