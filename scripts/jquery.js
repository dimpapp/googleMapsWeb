//συντεταγμένες πρώτου tweet
var startLAT;
var startLON;

var markerCluster;//αποθήκευση συστοιχιών σήμανσης

var startDay;//πρώτη μέρα που εμφανίστηκε σχετικό tweet
var endDay;//τελευταία μέρα με σχετικό tweet

var tempoNumber = 0;

var markersArray = new Array();//σημεία ή markers
var markersDate = new Array();//αντίστοιχη ημερομηνία για κάθε σημείο ή marker
var index = 0;//τρέχον αριθμός tweet
var size = 0;//αριθμός tweets συνολικά
var hashTag;
var ola = 0;//υπολογίζει σύνολο tweets
customImage = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png";//η εικόνα που εμφανίστηκε πρώτη φορά κάποιο tweet (1o)

var mr = [];//δομή με markers
var lb = [];//δομή με labels

$(document).ready(function () {

    //καλείται όταν πατηθεί το κουμπί δημοφιλών λέξεων ανά ημέρα
    $('#mapWordsBtnDay').click(function () {
        //αρχικοποίηση μεταβλητών
        index = 0;
        size = 0;
        //ημερομηνία έναρξης και τέλους
        startDay = new Date(2016, 11, 21, 0, 0, 0, 0);
        endDay = new Date(2017, 0, 24, 0, 0, 0, 0);
        //απεικόνιση δημοφιλών λέξεων για την τρέχουσα ημερομηνία (startDay)
        mapPopularWords();
    });

    //καλείται όταν πατηθεί το κουμπί δημοφιλών hashtags ανά ημέρα       
    $('#mapHashsBtnDay').click(function () {
        //αρχικοποίηση μεταβλητών
        index = 0;
        size = 0;
        //ημερομηνία έναρξης και τέλους
        startDay = new Date(2016, 11, 21, 0, 0, 0, 0);
        endDay = new Date(2017, 0, 24, 0, 0, 0, 0);
        //απεικόνιση δημοφιλών hashtag για την τρέχουσα ημερομηνία (startDay)
        mapPopularHash();
    });

    //επιστρέφει για τις 5 πόλεις τα δημοφιλέστερα συνολικά hashtags για όλες τις ημέρες
    $('#mapHashsBtn').click(function () {

        $.ajax({
            type: "GET",
            url: "topHashAll.aspx",//κλήση webservice
            timeout: 1200000,//πόσο να περιμένει 
            dataType: "xml",//επιστρέφει αρχείο xml
            success: function (xml) {
                //αρχικοποίηση μεταβλητών
                index = 0;
                //περνάμε όλο το αρχείο xml
                $(xml).find('root loop').each(function () {
                //συνταταγμένες
                    var lat = $(this).find('lat').text();
                    var lon = $(this).find('lon').text();
                    //αριθμός εμφάνισης
                    var sinolo = $(this).find('sinolo').text();
                    //πόλη
                    var city = $(this).find('city').text();
                    //όνομα hashtag
                    var word = $(this).find('word').text();
                    //αντικατάσταση ',' σε '.'  
                    tempLat = lat.replace(",", ".");
                    tempLon = lon.replace(",", ".");
                    //τρέχον σημείο
                    startPoint = new google.maps.LatLng(tempLat, tempLon);
                    //αποθήκευση στον πίνακα markers
                    markersArray[index] = startPoint;
                    //δημιουργία marker
                    var marker = new google.maps.Marker({
                        position: startPoint,
                        map: mapiHotel
                    });
                    //προσθήκη ετικέτας στο ίδιο σημείο
                    var label = new Label({
                        map: mapiHotel,
                        text: word
                    });
                    //εμφάνιση ετικέτας 
                    label.bindTo('position', marker, 'position');

                    index = index + 1;//πάμε για επόμενο marker
                });

                
            },
            error: function () {//αν κάπου χτυπήσει error, εμφάνιση μηνύματος 
                alert("Παρουσιάστηκε σφάλμα κατά τη λήψη των δεδομένων, δοκιμάστε ξανά αργότερα!");
            }
        });
    });


    //παίρνουμε όλα τα tweets για εμφάνιση σε heat map
    $('#mapHeatBtn').click(function () {

        $.ajax({
            type: "GET",
            url: "getAllTweets.aspx", //κλήση webservice
            timeout: 1200000, //πόσο να περιμένει 
            dataType: "xml",//επιστρέφει αρχείο xml
            success: function (xml) {
                //αρχικοποίηση μεταβλητών 
                index = 0;
                //περνάμε όλο το αρχείο xml
                $(xml).find('root loop').each(function () {
                    //συντεταγμένες 
                    var lat = $(this).find('lat').text();
                    var lon = $(this).find('lon').text();
                    //αντικατάσταση ',' σε '.'  
                    tempLat = lon.replace(",", ".");
                    tempLon = lat.replace(",", ".");

                    startPoint = new google.maps.LatLng(tempLat, tempLon);//παίρνουμε το αντίστοιχο σημείο 
                    markersArray[index] = startPoint;//το αποθηκεύουμε σε δομή πίνακα 
                    index = index + 1;//ενημέρωση δείκτη

                });

                size = index;//ενημέρωση συνόλου εγγραφών στη μεταβλητή size
                index = 0;//μηδενισμός index

                var markers = [];

                //ενεργοποίηση heatmap
                var heatmap = new google.maps.visualization.HeatmapLayer({
                    data: markersArray,//σημεία
                    map: mapiHotel//δείκτης προς χάρτη
                });
                heatmap.set('radius', heatmap.get('radius') ? null : 20);
                
            },
            error: function () {//εμφάνιση μηνύματος σε περίπτωση error 
                alert("Παρουσιάστηκε σφάλμα κατά τη λήψη των δεδομένων, δοκιμάστε ξανά αργότερα!");
            }
        });



    });

    //καλείται όταν πατηθεί το κουμπί δημοφιλών λέξεων για όλες τις μέρες
    $('#mapWordsBtn').click(function () {

        $.ajax({
            type: "GET",
            url: "topWordsAll.aspx", //κλήση webservice
            timeout: 1200000, //πόσο να περιμένει 
            dataType: "xml", //επιστρέφει αρχείο xml
            success: function (xml) {
                //αρχικοποίηση μεταβλητών
                index = 0;
                //περνάμε όλο το αρχείο xml
                $(xml).find('root loop').each(function () {
                    //συντεταγμένες 
                    var lat = $(this).find('lat').text();
                    var lon = $(this).find('lon').text();
                    var sinolo = $(this).find('sinolo').text();
                    var city = $(this).find('city').text();
                    var word = $(this).find('word').text();
                    //αντικατάσταση ',' σε '.' 
                    tempLat = lat.replace(",", ".");
                    tempLon = lon.replace(",", ".");

                    startPoint = new google.maps.LatLng(tempLat, tempLon);
                    markersArray[index] = startPoint;
                    

                    var marker = new google.maps.Marker({
                        position: startPoint,
                        map: mapiHotel
                    });

                    var label = new Label({
                        map: mapiHotel,
                        text:word
                    });
                    label.bindTo('position', marker, 'position');

                    index = index + 1; //πάμε για επόμενο marker
                });

                size = index; //ενημερώνουμε το σύνολο των εγγραφών στη μεταλβητή size
                index = 0; //μηδενίζουμε index

                
            },
            error: function () {//εμφάνιση μηνύματος σε περίπτωση error 
                alert("Παρουσιάστηκε σφάλμα κατά τη λήψη των δεδομένων, δοκιμάστε ξανά αργότερα!");
            }
        });
    });

    //εμφανίζει όλα τα tweets στο χάρτη για όλες τις ημέρες
    $('#mapAllBtn').click(function () {
        
        $.ajax({
            type: "GET",
            url: "getAllTweets.aspx", //κλήση webservice
            timeout: 1200000, //πόσο να περιμένει 
            dataType: "xml", //επιστρέφει αρχείο xml
            success: function (xml) {
                //αρχικοποίηση μεταβλητών
                index = 0;
                //περνάμε όλο το αρχείο xml
                $(xml).find('root loop').each(function () {
                    //συντεταγμένες 
                    var lat = $(this).find('lat').text();
                    var lon = $(this).find('lon').text();
                    //αντικατάσταση ',' σε '.'  
                    tempLat = lon.replace(",", ".");
                    tempLon = lat.replace(",", ".");

                    //δημιουργούμε επόμενο σημείο
                    startPoint = new google.maps.LatLng(tempLat, tempLon);
                    markersArray[index] = startPoint;//το αποθηκεύουμε στη δομή markersArray
                    index = index + 1; //πάμε για επόμενο marker

                });

                size = index; //ενημερώνουμε το σύνολο εγγραφών στη μεταβλητή size
                index = 0; //μηδενίζουμε index

                var markers = [];//αρχικοποίηση δομής 

                //για όλα τα σημεία
                for (i = 0; i < size; i++) {//κανουμε marker
                    var marker = new google.maps.Marker({ 'position': markersArray[i] });
                    markers.push(marker);//τον προσθέτουμε στη δομή markers
                }

                if (markerCluster) {//αν βρέθηκαν σημεία στο χάρτη
                    markerCluster.clearMarkers();//καθαρίζουμε τα σημεία που ήδη φαίνονται στο χάρτη
                }
                //εμφανίζουμε όλα τα σημεία στο χάρτη 
                markerCluster = new MarkerClusterer(mapiHotel, markers, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });

            },
            error: function () { //εμφάνιση μηνύματος σε περίπτωση error 
                alert("Παρουσιάστηκε σφάλμα κατά τη λήψη των δεδομένων, δοκιμάστε ξανά αργότερα!");
            }
        });
    });
    

    //εντοπισμός αρχικού σημείου εμφάνισης hashtag
    $('#buttonFindit').click(function () {       
    var hashTag = $('#hashtags option:selected').val();//επιλεγμένο hashtag στο listbox
    tempoNumber = 0;
        $.ajax({
            type: "GET",
            url: "getStartData.aspx?hashtag=" + hashTag , //κλήση webservice
            dataType: "xml", //επιστρέφει αρχείο xml
            success: function (xml) {
                mini = ($(xml).find('mini').text());//ημερομηνία πρώτου tweet
                maxi = ($(xml).find('maxi').text());//ημερομηνία τελευταίου tweet

                //συντεταγμένες 
                lat = ($(xml).find('lat').text());
                lon = ($(xml).find('lon').text());

                ola = ($(xml).find('arithmos').text());//σύνολο σχετικών tweets
                //μέρα, μήνας, χρόνος για αρχική ημερομηνία (day1) και τελική ημερομηνία (day2)
                day1_1 = ($(xml).find('day1_1').text());
                day1_2 = ($(xml).find('day1_2').text());
                day1_3 = ($(xml).find('day1_3').text());
                day2_1 = ($(xml).find('day2_1').text());
                day2_2 = ($(xml).find('day2_2').text());
                day2_3 = ($(xml).find('day2_3').text());

                //αποθηκεύουμε σχετικές ημερομηνίες σε κρυφά πεδία
                document.getElementById("day1_1").value = day1_1;
                document.getElementById("day1_2").value = day1_2;
                document.getElementById("day1_3").value = day1_3;

                document.getElementById("day2_1").value = day2_1;
                document.getElementById("day2_2").value = day2_2;
                document.getElementById("day2_3").value = day2_3;

                //εμφανίζουμε αρχική και τελική ημερομηνία
                document.getElementById("start").innerHTML = "<font color=red>Αρχικό tweet: </font><font color=navy>" + mini + "</font>";
                document.getElementById("end").innerHTML = "<font color=red>Τελευταίο tweet: </font><font color=navy>" + maxi + "</font>";
                //αντικατάσταση ',' σε '.'  
                startLAT = lon.replace(",",".");
                startLON = lat.replace(",", ".");

                //αρχικό σημείο
                startPoint = new google.maps.LatLng(startLAT, startLON);
                addMarker(startPoint);//προσθήκη marker

                //εμφάνιση κουμπιών για προβολή διάδοσης tweets με το συγκεκριμένο hashtag
                document.getElementById("buttonGo").style.display = "inline";
                document.getElementById("buttonRe").style.display = "none";
                                
            },
            error: function () {//εμφάνιση μηνύματος σε περίπτωση error 
                alert("Παρουσιάστηκε σφάλμα κατά τη λήψη των δεδομένων, δοκιμάστε ξανά αργότερα!");
            }
        });
    });

    //εμφάνιση αρχικού σημείου δημοσίευσης ενός tweet (1ου πριν retweet)
    $('#buttonFindReTweets').click(function () {
    var hashTag = $('#retweets option:selected').val();//επιλεγμένο tweet στο listbox

    tempoNumber = 0;
    $.ajax({
        type: "POST",
        url: "getStartDataRetweet.aspx", //κλήση webservice
        data: {hashtag: hashTag},
        dataType: "xml", //επιστρέφει αρχείο xml
        success: function (xml) {
            mini = ($(xml).find('mini').text());//ημερομηνία πρώτου tweet
           maxi = ($(xml).find('maxi').text());//ημερομηνία τελευταίου tweet

            //συντεταγμένες 
            lat = ($(xml).find('lat').text());
            lon = ($(xml).find('lon').text());
            ola = ($(xml).find('arithmos').text());//σύνολο σχετικών tweets

            //μέρα , μήνας, χρόνος για αρχική ημερομηνία (day1) και τελική ημερομηνία (day2)
            day1_1 = ($(xml).find('day1_1').text());
            day1_2 = ($(xml).find('day1_2').text());
            day1_3 = ($(xml).find('day1_3').text());
            day2_1 = ($(xml).find('day2_1').text());
            day2_2 = ($(xml).find('day2_2').text());
            day2_3 = ($(xml).find('day2_3').text());

            //αποθηκεύουμε σχετικές ημερομηνίες σε κρυφά πεδία
            document.getElementById("day1_1").value = day1_1;
            document.getElementById("day1_2").value = day1_2;
            document.getElementById("day1_3").value = day1_3;

            document.getElementById("day2_1").value = day2_1;
            document.getElementById("day2_2").value = day2_2;
            document.getElementById("day2_3").value = day2_3;

            //εμφανίζουμε αρχική και τελική ημερομηνία
            document.getElementById("start").innerHTML = "<font color=red>Αρχικό tweet: </font><font color=navy>" + mini + "</font>";
            document.getElementById("end").innerHTML = "<font color=red>Τελευταίο tweet: </font><font color=navy>" + maxi + "</font>";
            //αντικατάσταση ',' σε '.'  
            startLAT = lon.replace(",", ".");
            startLON = lat.replace(",", ".");

            //συντεταγμένες  πρώτου tweet
            startPoint = new google.maps.LatLng(startLAT, startLON);
            addMarker(startPoint);//προσθήκη marker

             //εμφάνιση κουμπιών για προβολή διάδοσης tweets 
            document.getElementById("buttonGo").style.display = "none";
            document.getElementById("buttonRe").style.display = "inline";

        }, 
        error: function () { //εμφάνιση μηνύματος σε περίπτωση error 
            alert("Παρουσιάστηκε σφάλμα κατά τη λήψη των δεδομένων, δοκιμάστε ξανά αργότερα!");
        }
    });
});

    //πάτημα κουμπιού GO για να δείξει τη διάδοση των tweets με συγκεκριμένο hashtags
    $('#buttonGo').click(function () {
    hashTag = $('#hashtags option:selected').val();//ποιο το επιλεγμένο hashtag
    //αρχικοποίηση μεταβλητών
    index = 0;
    size = 0;
    //αρχική και τελική ημερομηνία 
    day1_1 = parseInt(document.getElementById("day1_1").value);
    day1_2 = parseInt(document.getElementById("day1_2").value);
    day1_3 = parseInt(document.getElementById("day1_3").value);
    day2_1 = parseInt(document.getElementById("day2_1").value);
    day2_2 = parseInt(document.getElementById("day2_2").value);
    day2_3 = parseInt(document.getElementById("day2_3").value);

    //δήλωση ημερομηνιών
    startDay = new Date(day1_3, day1_2 - 1, day1_1, 0, 0, 0, 0);
    endDay = new Date(day2_3, day2_2-1, day2_1, 0, 0, 0, 0);
    
    //ξεκινάει απεικόνιση διάδοσης tweet με συγκεκριμένο hashtag
    redoLoading();
});


    //πάτημα κουμπιού GO για να δείξει διάδοση retweets από συγκεκριμένο tweet    
    $('#buttonRe').click(function () {
    hashTag = $('#retweets option:selected').val();//ποιο το επιλεγμένο tweet

    //αρχικοποίηση μεταβλητών
    index = 0;
    size = 0;
    //αρχική και τελική ημερομηνία
    day1_1 = parseInt(document.getElementById("day1_1").value);
    day1_2 = parseInt(document.getElementById("day1_2").value);
    day1_3 = parseInt(document.getElementById("day1_3").value);
    day2_1 = parseInt(document.getElementById("day2_1").value);
    day2_2 = parseInt(document.getElementById("day2_2").value);
    day2_3 = parseInt(document.getElementById("day2_3").value);
    //δήλωση ημερομηνιών
    startDay = new Date(day1_3, day1_2 - 1, day1_1, 0, 0, 0, 0);
    endDay = new Date(day2_3, day2_2 - 1, day2_1, 0, 0, 0, 0);

    //ξεκινάει απεικόνιση διάδοσης retweets
    redoLoadingTweets();
});

});

//καθάρισμα χάρτη από markers και labels
function removeMrLb() {
    for (i = 0; i < mr.length; i++) {
        mr[i].setMap(null);
        lb[i].setMap(null);
    }
}

//εμφάνιση δημοφιλών λέξεων ανα ημέρα από κουμπί WordBtnDay
function mapPopularWords() {
    //αν δεν φτάσαμε ακόμη στην τελευταία ημέρα 
    if (startDay <= endDay) {
        size = 0;//αρχικοποίηση size

        $.ajax({
            type: "POST",
            url: "topWordsAll.aspx", //κλήση webservice
            data: { year: startDay.getFullYear(), month: (startDay.getMonth() + 1), day: startDay.getDate() },
            dataType: "xml", //επιστρέφει αρχείο xml
            success: function (xml) {
                removeMrLb();//καθάρισμα χάρτη
                //αρχικοποίηση μεταβλητών
                index = 0;
                //περνάμε όλο το αρχείο xml
                $(xml).find('root loop').each(function () {
                    //συντεταγμένες 
                    var lat = $(this).find('lat').text();
                    var lon = $(this).find('lon').text();
                    //πόσες φορές
                    var sinolo = $(this).find('sinolo').text();
                    //ποια πόλη
                    var city = $(this).find('city').text();
                    //ποια λέξη
                    var word = $(this).find('word').text();
                    //αντικατάσταση ',' σε '.'  
                    tempLat = lat.replace(",", ".");
                    tempLon = lon.replace(",", ".");

                    //συντεταγμένες πόλης
                    startPoint = new google.maps.LatLng(tempLat, tempLon);
                    markersArray[index] = startPoint;//αποθήκευση στη δομή markersArray

                    //δημιουργία  marker
                    var marker = new google.maps.Marker({
                        position: startPoint,
                        map: mapiHotel
                    });

                    //δημιουργία  label
                    var label = new Label({
                        map: mapiHotel,
                        text: word + ' - ' + sinolo + ' tweets'
                    });

                    //εμφάνιση  label
                    label.bindTo('position', marker, 'position');

                    //αποθήκευση marker και label για πιθανή μελοντική αφαίρεση από χάρτη
                    mr.push(marker);
                    lb.push(label);

                    index = index + 1; //πάμε για επόμενο marker
                });

                size = index; //ενημερώνουμε σύνολο εγγραφών στη μεταβλητή size
                index = 0; //μηδενίζουμε  index

                //εμφανίζεται η ενεργή ημερομηνία 
                document.getElementById("popularDay").innerHTML = "<font color=red>Ημέρα:</font> <font color=navy><b>" + startDay.getDate() + "/" + (startDay.getMonth() + 1) + "/" + startDay.getFullYear() + "</b></font>";
                startDay = startDay.addDays(1);//δηλώνουμε επόμενη ημέρα 
                setTimeout(function () { mapPopularWords(); }, 4000);//σε 4 δευτερόλεπτα θα ζητήσουμε popular words για επόμενη ημέρα 
            },
            error: function () {//δεν βρέθηκαν δεδομένα ή άλλο λάθος 
            //εμφανίζεται η ενεργή ημερομηνία 
                document.getElementById("popularDay").innerHTML = "<font color=red>Ημέρα:</font> <font color=navy><b>" + startDay.getDate() + "/" + (startDay.getMonth() + 1) + "/" + startDay.getFullYear() + "</b></font>";
                startDay = startDay.addDays(1);//δηλώνουμε επόμενη ημέρα 
                setTimeout(function () { mapPopularWords(); }, 4000); //σε 4 δευτερόλεπτα θα ζητήσουμε popular words για επόμενη ημέρα        
            }
        });
    }
}

//εμφάνιση δημοφιλών hashtag ανά ημέρα κουμπί HashsBtnDay
function mapPopularHash(){
    //αν δεν φτάσαμε στην τελευταία ημέρα 
    if (startDay <= endDay) {
        size = 0;//αρχικοποίηση size
        
        $.ajax({
            type: "POST",
            url: "topHashAll.aspx", //κλήση webservice
            data: { year: startDay.getFullYear(), month: (startDay.getMonth() + 1), day: startDay.getDate()},
            dataType: "xml", //επιστρέφει αρχείο xml
            success: function (xml) {
                removeMrLb();
                //αρχικοποίηση μεταβλητών
                index = 0;
                //περνάμε όλο το αρχείο xml
                $(xml).find('root loop').each(function () {
                    //συντεταγμένες 
                    var lat = $(this).find('lat').text();
                    var lon = $(this).find('lon').text();
                    var sinolo = $(this).find('sinolo').text();
                    var city = $(this).find('city').text();
                    var word = $(this).find('word').text();
                    //αντικατάσταση ',' σε '.'  
                    tempLat = lat.replace(",", ".");
                    tempLon = lon.replace(",", ".");
                    //συντεταγμένες πόλης
                    startPoint = new google.maps.LatLng(tempLat, tempLon);
                    markersArray[index] = startPoint;//αποθήκευση στη δομή markersArray

                    //δημιουργία  marker
                    var marker = new google.maps.Marker({
                        position: startPoint,
                        map: mapiHotel
                    });
                    //δημιουργία label
                    var label = new Label({
                        map: mapiHotel,
                        text: word + ' - ' + sinolo + ' tweets'
                    });
                    //εμφάνιση  labels
                    label.bindTo('position', marker, 'position');
                    //αποθήκευση marker και label για πιθανή μελοντική αφαίρεση από χάρτη
                    mr.push(marker);
                    lb.push(label);

                    index = index + 1; //πάμε για επόμενο marker
                });

                size = index; //ενημερώνουμε σύνολο εγγραφών στη μεταβλητή size
                index = 0; //μηδενίζουμε  index
                //εμφανίζεται η ενεργή ημερομηνία 
                document.getElementById("popularDay").innerHTML = "<font color=red>Ημέρα:</font> <font color=navy><b>" + startDay.getDate() + "/" + (startDay.getMonth() + 1) + "/" + startDay.getFullYear() + "</b></font>";
                startDay = startDay.addDays(1);//δηλώνουμε επόμενη ημέρα 
                setTimeout(function () { mapPopularHash(); }, 8000);//σε 8 δευτερόλεπτα θα ζητήσουμε popular hashtags για επόμενη ημέρα  
            },
            error: function () {//δεν βρέθηκαν δεδομένα ή άλλο λάθος
                //εμφανίζεται η ενεργή ημερομηνία
                document.getElementById("popularDay").innerHTML = "<font color=red>Ημέρα:</font> <font color=navy><b>" + startDay.getDate() + "/" + (startDay.getMonth() + 1) + "/" + startDay.getFullYear() + "</b></font>";
                startDay = startDay.addDays(1);//δηλώνουμε επόμενη ημέρα 
                setTimeout(function () { mapPopularHash(); }, 8000);     //σε 8 δευτερόλεπτα θα ζητήσουμε popular hashtags για επόμενη ημέρα               
            }
        });
    }
}

//απεικόνιση βελών διάδοσης retweets από κουμπί GO
function redoLoadingTweets() {
    //αν δεν φτάσαμε στην τελευταία ημέρα
    if (startDay <= endDay) {
        size = 0;//αρχικοποίηση size
        
        $.ajax({
            type: "POST",
            url: "getAllByHashtagTweet.aspx", //κλήση webservice
            //σαν μεταβλητές, ποιο το hashcode tweet (hashtag), μέρα, μήνα, χρόνο, σειρά εμφάνισης tweet ( δεν χρσιμοποιείται κάπου άμεσα)
            data: { hashtag: hashTag, year: startDay.getFullYear(), month: (startDay.getMonth() + 1), day: startDay.getDate(), seira: (document.getElementById("retweets").selectedIndex + 1) },
            dataType: "xml", //επιστρέφει αρχείο xml
            success: function (xml) {
                //αρχικοποίηση μεταβλητών
                index = 0;
                //περνάμε όλο το αρχείο xml
                $(xml).find('root loop').each(function () {
                    //ημερομηνία  tweet
                    var imerominia = $(this).find('imerominia').text();

                    //συντεταγμένες
                    var lat = $(this).find('lat').text();
                    var lon = $(this).find('lon').text();

                    //αν θα πρέπει να ζωγραφίσουμε γραμμή 
                    var drawLine = $(this).find('drawLine').text();
                    //συντεταγμένες του tweet από όπου έγινε retweet
                    var fatherlLat = $(this).find('fatherlLat').text();
                    var fatherlLon = $(this).find('fatherlLon').text();

                    //αντικατάσταση ',' σε '.'  
                    tempLat = lon.replace(",", ".");
                    tempLon = lat.replace(",", ".");

                    //σημείο συντεταγένες 
                    startPoint = new google.maps.LatLng(tempLat, tempLon);
                    markersArray[index] = startPoint;//αποθήκευση στη δομή markersArray
                    markersDate[index] = imerominia;//αποθήκευση στη δομή markersDate της σχετικής ημερομηνίας
                    index = index + 1; //πάμε για επόμενο marker

                    //αν είναι να ζωγραφίσουμε γραμμή 
                    if (parseInt(drawLine) > 0) {
                    //αντικατάσταση ',' σε '.'
                        startLat = fatherlLon.replace(",", ".");
                        startLon = fatherlLat.replace(",", ".");

                        //σημείο father tweets
                        fatherPoint = new google.maps.LatLng(startLat, startLon);
                        //ορίζουμε γραμμή
                        var line = new google.maps.Polyline({       
                            path: [
                                fatherPoint,
                                startPoint                                
                            ],
                            strokeColor: "#FF0000",
                            strokeOpacity: 1.0,
                            strokeWeight: 1,
                            icons: [{
                                icon: {path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW},
                                offset: '100%'
                            }],
                            map: mapiHotel
                        });
                    }

                });

                size = index; //ενημερώνουμε σύνολο εγγραφών στη μεταβλητή size
                index = 0; //μηδενίζουμε index

                tempoNumber = tempoNumber + size;//σύνολο tweets


                pushMarkersReTweets();//εμφάνιση marker του νέου σημείου tweet
                startDay = startDay.addDays(1);//δηλώνουμε επόμενη ημέρα
                
                setTimeout(function () { redoLoadingTweets(); }, 3000);//σε 3 δευτερόλεπτα θα ζητήσουμε ίδιες πληροφορίες για επόμενη ημέρα 

            },
            error: function () {
            //εμφάνιση πληροφοριών
                document.getElementById("twitDate").innerHTML = "<font color=red>Ημέρα:</font> <font color=navy>" + startDay.toLocaleDateString() + "</font>";
                document.getElementById("twitNumber").innerHTML = "<font color=red>Ημερήσια retweets:</font> <font color=navy>0</font>";
                document.getElementById("twitTotal").innerHTML = "<font color=red>Σύνολο retweets:</font> <font color=navy>" + tempoNumber.toString() + " / " + ola.toString() + "</font>";
                startDay = startDay.addDays(1);//δηλώνουμε επόμενη ήμέρα 
                setTimeout(function () { redoLoading(); }, 3000);  //σε 3 δευτερόλεπτα θα ζητήσουμε ίδιες πληροφορίες για επόμενη ημέρα                
            }
        });
    }

}

//απεικόνιση διάδοσης hashtags ανα ημέρα από κουμπί GO
function redoLoading() {
    //αν δεν φτάσαμε στην τελευταία ημέρα
    if (startDay <= endDay) {
        size = 0;//αρχικοποιηση size
      
        $.ajax({
            type: "GET",//ως μεταβλητές κωδικός hashtag, χρόνος, μήνας, μέρα και σειρά (δε χρησιμοποιείται πλέον άμεσα)
            url: "getAllByHashtag.aspx?hashtag=" + hashTag + "&year=" + startDay.getFullYear() + "&month=" + (startDay.getMonth() + 1) + "&day=" + startDay.getDate() + "&seira=" + (document.getElementById("hashtags").selectedIndex + 1),
            dataType: "xml", //επιστρέφει αρχείο xml
            success: function (xml) {
                //αρχικοποίηση μεταβλητών
                index = 0;
                //περνάμε όλο το αρχείο xml
                $(xml).find('root loop').each(function () {
                    //ημερομηνία αντίστοιχου tweet
                    var imerominia = $(this).find('imerominia').text();

                    //συντεταγμένες
                    var lat = $(this).find('lat').text();
                    var lon = $(this).find('lon').text();

                    //εμφάνιση γραμμής
                    var drawLine = $(this).find('drawLine').text();
                    //συντεταγμένες father tweet
                    var fatherlLat = $(this).find('fatherlLat').text();
                    var fatherlLon = $(this).find('fatherlLon').text();

                    //αντικατάσταση ',' σε '.'  
                        tempLat = lon.replace(",", ".");
                        tempLon = lat.replace(",", ".");

                        //σημείο στο χάρτη - συντεταγμένες 
                        startPoint = new google.maps.LatLng(tempLat, tempLon);
                         markersArray[index] = startPoint;//αποθήκευση στη δομή markersArray
                    markersDate[index] = imerominia;//αποθήκευση στη δομή markersDate της σχετικής ημερομηνίας
                        index = index + 1; //πάμε για επόμενο marker

                        //αν είναι να ζωγραφίσουμε γραμμή
                        if (parseInt(drawLine) > 0) {
                            //συντεταγμένες father tweet
                            startLat = fatherlLon.replace(",", ".");
                            startLon = fatherlLat.replace(",", ".");

                            //σημείο father
                            fatherPoint = new google.maps.LatLng(startLat, startLon);
                            //απεικόνιση βέλους
                            var line = new google.maps.Polyline({
                                path: [
                                    startPoint,
                                    fatherPoint
                                ],
                                strokeColor: "#FF0000",
                                strokeOpacity: 1.0,
                                strokeWeight: 10,
                                map: mapiHotel
                            });
                        }
                    
                });

                    size = index; //ενημερώνουμε σύνολο εγγραφών στη μεταβλητή size
                    index = 0; //μηδενίζουμε index
                
                tempoNumber = tempoNumber + size;//σύνολο tweets

                
                pushMarkers();//εμφάνιση σχετικών marker για την τρέχουσα ημέρα 
                startDay = startDay.addDays(1);//πάμε στην επόμενη μέρα 



                setTimeout(function () { redoLoading(); }, 3000);//σε 3 δευτερόλεπτα θα ζητήσουμε ίδιες πληροφορίες για επόμενη ημέρα     
                
            },
            error: function () {
            //emfanisi pliroforion
                document.getElementById("twitDate").innerHTML = "<font color=red>Ημέρα:</font> <font color=navy>" + startDay.toLocaleDateString() + "</font>";
                document.getElementById("twitNumber").innerHTML = "<font color=red>Ημερήσια tweets:</font> <font color=navy>0</font>";
                document.getElementById("twitTotal").innerHTML = "<font color=red>Σύνολο tweets:</font> <font color=navy>" + tempoNumber.toString() + " / " + ola.toString() +  "</font>";
                startDay = startDay.addDays(1);//pame epomeni mera
                setTimeout(function () { redoLoading(); }, 3000);    //σε 3 δευτερόλεπτα θα ζητήσουμε ίδιες πληροφορίες για επόμενη ημέρα                  
            }
        });
    }

}

//απεικόνιση στο χάρτη μαζικά των marker ημέρας
function pushMarkersReTweets() {

    var markers = [];//αρχικοποίηση μεταβλητής

    for (i = 0; i < size; i++) {
        var marker = new google.maps.Marker({ 'position': markersArray[i] });//δημιουργούμε markers
        markers.push(marker);//αποθηκεύουμε στη δομή markers

    }
    //αν έχουμε ορίσει ήδη marker cluster το διαγράφουμε 
    if (markerCluster) {
        markerCluster.clearMarkers();
    }
    //δημιουργούμε markercluster
    markerCluster = new MarkerClusterer(mapiHotel, markers, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });

    //ενημέρωση πληροφοριών
    document.getElementById("twitDate").innerHTML = "<font color=red>Ημέρα:</font> <font color=navy>" + startDay.toLocaleDateString() + "</font>";
    document.getElementById("twitNumber").innerHTML = "<font color=red>Ημερήσια retweets:</font> <font color=navy>" + size.toString() + "</font>";
    document.getElementById("twitTotal").innerHTML = "<font color=red>Σύνολο retweets:</font> <font color=navy>" + tempoNumber.toString() + " / " + ola.toString() + "</font>";

}

//απεικόνιση στο χαρτη μαζικά των markers ημέρας
function pushMarkers() {
    
    var markers = [];//αρχικοποίηση μεταβλητής

    for (i = 0; i < size; i++) {
        var marker = new google.maps.Marker({ 'position': markersArray[i] });//δημιουργούμε markers
        markers.push(marker);//αποθηκεύουμε στη δομή markers
    }
    //αν έχουμε ήδη ορίσει marker cluster το διαγράφουμε 
    if (markerCluster) {
        markerCluster.clearMarkers();
    }
    //δημιουργούμε  markercluster
    markerCluster = new MarkerClusterer(mapiHotel, markers, { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
    //ενημέρωση πληροφοριών 
    document.getElementById("twitDate").innerHTML = "<font color=red>Ημέρα:</font> <font color=navy>" + startDay.toLocaleDateString() + "</font>";
    document.getElementById("twitNumber").innerHTML = "<font color=red>Ημερήσια tweets:</font> <font color=navy>" + size.toString() + "</font>";
    document.getElementById("twitTotal").innerHTML = "<font color=red>Σύνολο tweets:</font> <font color=navy>" + tempoNumber.toString() + " / " + ola.toString() + "</font>";
    
}


//συνάρτηση προσθήκης αριθμού ημερών σε ημερομηνία
Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}


//-----------------------------------------------EMFANISI LABEL STO XARTI--------------------------------

// Define the overlay, derived from google.maps.OverlayView
function Label(opt_options) {
    // Initialization
    this.setValues(opt_options);

    // Label specific
    var span = this.span_ = document.createElement('span');
    span.style.cssText = 'position: relative; left: -50%; top: 2px; ' +
                         'white-space: nowrap; border: 1px solid blue; ' +
                         'padding: 2px; background-color: white';

    var div = this.div_ = document.createElement('div');
    div.appendChild(span);
    div.style.cssText = 'position: absolute; display: none';
};
Label.prototype = new google.maps.OverlayView;

// Implement onAdd
Label.prototype.onAdd = function () {
    var pane = this.getPanes().overlayLayer;
    pane.appendChild(this.div_);

    // Ensures the label is redrawn if the text or position is changed.
    var me = this;
    this.listeners_ = [
      google.maps.event.addListener(this, 'position_changed',
          function () { me.draw(); }),
      google.maps.event.addListener(this, 'text_changed',
          function () { me.draw(); })
    ];
};

// Implement onRemove
Label.prototype.onRemove = function () {
    this.div_.parentNode.removeChild(this.div_);

    // Label is removed from the map, stop updating its position/text.
    for (var i = 0, I = this.listeners_.length; i < I; ++i) {
        google.maps.event.removeListener(this.listeners_[i]);
    }
};

// Implement draw
Label.prototype.draw = function () {
    var projection = this.getProjection();
    var position = projection.fromLatLngToDivPixel(this.get('position'));

    var div = this.div_;
    div.style.left = position.x + 'px';
    div.style.top = position.y + 'px';
    div.style.display = 'block';

    this.span_.innerHTML = this.get('text').toString();
};

//-----------------------------------------------EMFANISI LABEL STO XARTI--------------------------------