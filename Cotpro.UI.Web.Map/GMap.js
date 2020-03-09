/// <reference name="MicrosoftAjax.js"/>


Type.registerNamespace("Cotpro.UI.Web.Map");

Cotpro.UI.Web.Map.GMap = function (element) {
    Cotpro.UI.Web.Map.GMap.initializeBase(this, [element]);

    // Define properties
    this._zoom = null;
    this._centerLatitude = null;
    this._centerLongitude = null;
    this._markers = null;

    this._mapObj = null;
    this._infoWindow = null;

    this._popupWebService = "";
    this._webServiceUrl = "";
    this._interval = 5000;

    this._mArray = [];
    this._lines = null;
}

Cotpro.UI.Web.Map.GMap.prototype = {
    initialize: function () {
        Cotpro.UI.Web.Map.GMap.callBaseMethod(this, 'initialize');

        // Create the map
        this.createMap();

        var that = this;
        var s = setInterval(function () { that.AjaxPerformer(that); }, this.get_interval());
    },
    dispose: function () {
        //Add custom dispose actions here
        Cotpro.UI.Web.Map.GMap.callBaseMethod(this, 'dispose');
    },
    get_zoom: function () {
        return this._zoom;
    },
    set_zoom: function (value) {
        if (this._zoom !== value) {
            this._zoom = value;
            this.raisePropertyChanged("zoom");
        }
    },
    get_centerLatitude: function () {
        return this._centerLatitude;
    },
    set_centerLatitude: function (value) {
        if (this._centerLatitude !== value) {
            this._centerLatitude = value;
            this.raisePropertyChanged("centerLatitude");
        }
    },
    get_centerLongitude: function () {
        return this._centerLongitude;
    },
    set_centerLongitude: function (value) {
        if (this._centerLongitude !== value) {
            this._centerLongitude = value;
            this.raisePropertyChanged("centerLongitude");
        }
    },
    get_markers: function () {
        return this._markers;
    },
    set_markers: function (value) {
        if (this._markers !== value) {
            this._markers = value;
            this.raisePropertyChanged("markers");
        }
    },
    get_lines: function () {
        return this._lines;
    },
    set_lines: function (value) {
        if (this._lines !== value) {
            this._lines = value;
            this.raisePropertyChanged("lines");
        }
    },
    get_webServiceUrl: function () {
        return this._webServiceUrl;
    },
    set_webServiceUrl: function (value) {
        if (this._webServiceUrl !== value) {
            this._webServiceUrl = value;
            this.raisePropertyChanged("webServiceUrl");
        }
    },
    get_popupWebService: function () {
        return this._popupWebService;
    },
    set_popupWebService: function (value) {
        if (this._popupWebService !== value) {
            this._popupWebService = value;
            this.raisePropertyChanged("popupWebService");
        }
    },
    get_interval: function () {
        return this._interval;
    },
    set_interval: function (value) {
        if (this._interval !== value) {
            this._interval = value;
            this.raisePropertyChanged("interval");
        }
    },
    get_mArray: function () {
        return this._mArray;
    },
    set_mArray: function (value) {
        if (this._mArray !== value) {
            this._mArray = value;
            this.raisePropertyChanged("mArray");
        }
    },
    createMap: function () {
        //array to store google map markers
        var arr = [];
        // Set the center point, zoom, and type of map
        var centerPoint = new google.maps.LatLng(this.get_centerLatitude(), this.get_centerLongitude());
        var options = {
            zoom: this.get_zoom(),
            center: centerPoint,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        // Create the map, using the above options
        this._mapObj = new google.maps.Map(this._element, options);

        //Show Lines
        this.ShowLines();

        // Get the array of markers and iterate through them
        var markers = this.get_markers();
        if (markers != null) {
            for (var i = 0; i < markers.length; i++) {
                // Create the marker
                var marker = new google.maps.Marker
                (
                    {
                        position: new google.maps.LatLng(markers[i].Latitude, markers[i].Longitude),
                        map: this._mapObj,
                        title: markers[i].Title,
                        icon: markers[i].IconUrl
                    }
                );

                // Save the current context to the 'that' variable
                var that = this;
                (function (marker, infoHtml) {
                    // Add an event handler for the click event on the marker
                    google.maps.event.addListener(marker, 'click', function () {

                        // Check if the info window has been created, and if not create it
                        if (!that._infoWindow) {
                            that._infoWindow = new google.maps.InfoWindow();
                        }

                        // Set the content of the info window
                        that._infoWindow.setContent(marker.getPosition().toString());

                        // Show the info window
                        that._infoWindow.open(that._mapObj, marker);

                    });
                })(marker, markers[i].InfoWindowHtml);
                //A custom array to store google map markers array and InfoWindows
                arr.push({ marker: marker, ID: i, InfoWindow: that._infoWindow });
            }
        }
        this.set_mArray(arr);
    },
    ShowLines: function () {

        var lines = this._lines;

        if (lines != null) {
            for (var i = 0; i < lines.length; i++) {
                var points = [];
                for (var j = 0; j < lines[i].Points.length; j++) {
                    points.push(new google.maps.LatLng(lines[i].Points[j].Latitude, lines[i].Points[j].Longitude));

                }
                // Create the poluline
                var line = new google.maps.Polyline
                (
                    {
                        path: points,
                        geodesic: lines[i].Geodisc,
                        strokeColor: lines[i].Color,
                        strokeOpacity: lines[i].Opacity,
                        strokeWeight: lines[i].Weight
                    }
                );
                line.setMap(this._mapObj);
            }
        }
    },
    PopupUpdate: function (id) {
        var xmlhttp;
        var a, b = "default";
        if (window.XMLHttpRequest) {
            // code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        }
        else {// code for IE6, IE5
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        xmlhttp.onreadystatechange = function () {
            a = xmlhttp.responseText.substr();
            b = a.substring(a.indexOf("http://tempuri.org/") + 21, a.indexOf("</string>"));
            while (b.indexOf("&lt;") >= 0 || b.indexOf("&gt;") >= 0) {
                b = b.replace("&lt;", "<");
                b = b.replace("&gt;", ">");
            }
        }
        xmlhttp.open("POST", this.get_popupWebService(), false);
        xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
        xmlhttp.send("id=" + id);
        return b;
    },
    AjaxPerformer: function (obj) {
        var xmlhttp;
        if (window.XMLHttpRequest) {
            // code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        }
        else {// code for IE6, IE5
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                var buses = JSON.parse(xmlhttp.responseText).d;
                var i, j;
                var mArray = obj.get_mArray();
                for (i = 0; i < buses.length; i++) {
                    for (j = 0; j < mArray.length; j++) {
                        if (mArray[j].ID === buses[i].ID) {
                            obj.MarkerAnimate(mArray[j], buses[i].Location.X, buses[i].Location.Y, obj.get_interval());
                            break;
                        }
                    }
                }

            }
        }
        xmlhttp.open("POST", obj.get_webServiceUrl(), true);
        xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        xmlhttp.send();
    },
    //latitude and longitude are destination coordinates
    MarkerAnimate: function (marker, latitude, longitude, interval) {
        var fPosition = marker.marker.getPosition();
        var distance = (Math.sqrt(Math.pow(latitude - fPosition.lat(), 2) + Math.pow(longitude - fPosition.lng(), 2)));
        var IterationCount = 0;
        var nlat, nlng;
        var u = setInterval(function () {
            IterationCount += (distance / interval) * 20;
            nlat = ((latitude - fPosition.lat()) * IterationCount) / distance;
            nlng = ((longitude - fPosition.lng()) * IterationCount) / distance;
            marker.marker.setPosition(new google.maps.LatLng(nlat + fPosition.lat(), nlng + fPosition.lng()));

            if ((Math.sqrt(Math.pow(nlat, 2) + Math.pow(nlng, 2))) >= distance)
                clearInterval(u);
        }, 20);
    }
}
Cotpro.UI.Web.Map.GMap.registerClass('Cotpro.UI.Web.Map.GMap', Sys.UI.Control);

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();