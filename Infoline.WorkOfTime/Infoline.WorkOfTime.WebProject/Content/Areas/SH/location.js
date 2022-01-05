var control = false;
var selectedFeature = null;

var pageLocation = {
    popup: null,
    defines: {
        data: [],
        filteredData: [],
    },
    dfn: {
        harita: new AkilliHarita('Harita', {
            zoom: 7,
            minZoom: 0,
            altlik: "Default1",
            uiAltlik: false,
            uiDefault: true,
            uiSearch: true,
            uiMinimap: false,
            uiScaleLine: false,
            uiMousePosition: false,
            uiMesurement: false,
            uiInfo: false
        }),
    },
    fn: {
        zoomIn: function (e) {
            $('.ol-zoom-in').trigger("click");
        },
        zoomOut: function (e) {
            $('.ol-zoom-out').trigger("click");
        },
        zoomMove: function (e) {
            $(e).toggleClass("active");
            if ($(e).hasClass("active")) {
                $('[data-tool="DragZoom"]').trigger("click");
            } else {
                $('[data-tool="DragPan"]').trigger("click");
            }
        },
        home: function (e) {
            $('[data-tool="Home"]').trigger("click");
        },
        fullScreen: function (e) {
            $(e).toggleClass("active");
            $('.ol-full-screen-false').trigger("click");
        },
        layersSwip: function (e) {
            $(e).toggleClass("active");
            $('[data-tool="Swipe"]').trigger("click");
        },
        slideTab: function (elem, target) {

            var hasActive = $(elem).hasClass("active");
            $(elem).parent().find("button").removeClass("active");

            if (hasActive) {
                $(".btn-bar.right").animate({ right: -430 }, 200);
            } else {
                $(elem).addClass("active");
                $(".btn-bar.right").animate({ right: -430 }, 200, function () {
                    $('.pan-container').find('.pan-content').hide();
                    $('.pan-container').find("#" + target).show();
                    $(".btn-bar.right").animate({ right: 15 }, 200);
                });
            }
        },
        colorSelect: function (e) {
            if ($('#ColorBox').hasClass("hide")) {
                $('#ColorBox').removeClass("hide");
            } else {
                $('#ColorBox').addClass("hide");
            }
        },
        colorInfoSelect: function (e) {
            if ($('#ColorInfo').hasClass("hide")) {
                $('#ColorInfo').removeClass("hide");
            } else {
                $('#ColorInfo').addClass("hide");
            }
        },
        searchSelect: function (e) {
            if ($('.search-content')[0].style.display == "none") {
                $('[data-target=".search-content"]').trigger("click");
            } else {
                $('.search-content')[0].style.display = "none";
            }
        }
    },
    changePresId: "",
    data: null,
    init: function () {

        $('[data-original-title]').tooltip();

        var svg = '<svg id="Capa_1" data-name="Capa 1" xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="64px" height="64px" viewBox="0 0 512 512"><path fill="#1ab394" d="M256,0A160,160,0,0,0,96,160a158.2,158.2,0,0,0,15.7,69.1C112.2,230.3,256,512,256,512L398.6,232.6A160,160,0,0,0,256,0Zm0,256a96,96,0,1,1,96-96A96,96,0,0,1,256,256Z"/><circle fill="#ededed" cx="256" cy="160" r="96"/><path transform="rotate(180, 0, 0)  translate(-322,-200) scale(0.12)" fill="#1ab394"  d="M1070-88q0-21-2-63h-1067q0 10 0 31t-1 32q0 30 1 37 12 49 64 85t112 53 125 47 97 65q17 22 17 38 0 22-11 73-4 21-10 37t-16 33-16 31q-15 35-33 132-6 38-6 75 0 105 54 168t157 63 158-63 53-168q0-31-7-75-14-89-32-132-6-14-15-31t-16-33-11-37q-11-51-11-73 0-18 17-38 31-36 97-65t125-47 111-53 64-85q2-8 2-37z"/></svg>';
        var firstLocation = new ol.style.Style({
            image: new ol.style.Icon({
                opacity: 1,
                src: 'data:image/svg+xml,' + escape(svg),
                scale: 1.0,
                anchor: [0.5, 64],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
            })
        });

        svg = '<svg id="Capa_1" data-name="Capa 1" xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="64px" height="64px" viewBox="0 0 512 512"><path fill="#ff0202" d="M256,0C115.39,0,0,115.39,0,256s115.39,256,256,256s256-115.39,256-256S396.61,0,256,0z"/></svg>';
        var locations = new ol.style.Style({
            image: new ol.style.Icon({
                opacity: 1,
                src: 'data:image/svg+xml,' + escape(svg),
                scale: 0.18,
                anchor: [0.5, 64],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
            })
        });

        svg = '<svg id="Capa_1" data-name="Capa 1" xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="64px" height="64px" viewBox="0 0 512 512"><path fill="#f8ac59" d="M256,0A160,160,0,0,0,96,160a158.2,158.2,0,0,0,15.7,69.1C112.2,230.3,256,512,256,512L398.6,232.6A160,160,0,0,0,256,0Zm0,256a96,96,0,1,1,96-96A96,96,0,0,1,256,256Z"/><circle fill="#ededed" cx="256" cy="160" r="96"/><path transform="rotate(180, 0, 0)  translate(-322,-200) scale(0.12)" fill="#f8ac59"  d="M1070-88q0-21-2-63h-1067q0 10 0 31t-1 32q0 30 1 37 12 49 64 85t112 53 125 47 97 65q17 22 17 38 0 22-11 73-4 21-10 37t-16 33-16 31q-15 35-33 132-6 38-6 75 0 105 54 168t157 63 158-63 53-168q0-31-7-75-14-89-32-132-6-14-15-31t-16-33-11-37q-11-51-11-73 0-18 17-38 31-36 97-65t125-47 111-53 64-85q2-8 2-37z"/></svg>';
        var lastLocation = new ol.style.Style({
            image: new ol.style.Icon({
                opacity: 1,
                src: 'data:image/svg+xml,' + escape(svg),
                scale: 1.0,
                anchor: [0.5, 64],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
            })
        });



        pageLocation.dfn.harita.style.addCustom("firstLocationStyle", firstLocation);
        pageLocation.dfn.harita.style.addCustom("locationsStyle", locations);
        pageLocation.dfn.harita.style.addCustom("lastLocationStyle", lastLocation);

        pageLocation.dfn.harita.layer.addVector("İlk Konum", "FirstLocation", "firstLocationStyle", "firstLocationStyle", "firstLocationStyle")
        pageLocation.dfn.harita.layer.addVector("Konumlar", "Locations", "locationsStyle", "locationsStyle", "locationsStyle")
        pageLocation.dfn.harita.layer.addVector("Son Konum", "LastLocation", "lastLocationStyle", "lastLocationStyle", "lastLocationStyle")

        pageLocation.dfn.harita.layer.addVector("Action", "Action");

        pageLocation.dfn.harita.feature.events(function (feature, layer) { return true; }, function (feature, layer) { return true; }, function (event, object) { });

    },
    locateFeatureToMap: function () {
        var points = [];

        if (pageLocation.defines.filteredData != undefined) {

            var trackingDatas = pageLocation.defines.filteredData.LocationTrackings;
            if (trackingDatas != null) {

                if (trackingDatas.length > 0) {
                    $.each($.Enumerable.From(trackingDatas).OrderBy(x => x.timeStamp).ToArray(), function (i, data) {

                        if (data.location != null) {
                            if (i == 0) {

                                var feature = pageLocation.dfn.harita.feature.add("FirstLocation", data.id, data.location);
                                if (feature) {
                                    feature.set("properties", data);

                                    points.push(feature.getGeoJSON().geometry.coordinates.join(" "));
                                }
                            }
                            else if (i == trackingDatas.length - 1) {

                                var feature = pageLocation.dfn.harita.feature.add("LastLocation", data.id, data.location);
                                if (feature) {
                                    feature.set("properties", data);

                                    points.push(feature.getGeoJSON().geometry.coordinates.join(" "));
                                }
                            }
                            else {

                                var feature = pageLocation.dfn.harita.feature.add("Locations", data.id, data.location);
                                if (feature) {
                                    feature.set("properties", data);
                                    var coords = pageLocation.dfn.harita.helper.SQLtoGeometry(data.location);
                                    if (coords != null) {
                                        points.push(data.location.replace('POINT (', '').replace(')', ''));
                                    }

                                }
                            }
                        }
                    });
                    var line = "LINESTRING(" + points.join(", ") + ")";
                    pageLocation.dfn.harita.feature.add("Action", "linestring", line);

                    pageLocation.dfn.harita.layer.panTo("Locations");

                }

            }
        }

        pageLocation.dfn.harita.map.updateSize();
    },
    applyFilter: function () {
        pageLocation.dfn.harita.layer.get("FirstLocation")["FirstLocation"].getSource().clear();
        pageLocation.dfn.harita.layer.get("Locations")["Locations"].getSource().clear();
        pageLocation.dfn.harita.layer.get("LastLocation")["LastLocation"].getSource().clear();
        pageLocation.dfn.harita.layer.get("Action")["Action"].getSource().clear();
        pageLocation.dfn.harita.feature.properties.interactionSelect.getFeatures().clear();

        pageLocation.locateFeatureToMap();

        if ($('#searchButton').hasClass("active")) {
            $('#searchButton').trigger("click");
        }

    },
    goLocation: function (_this) {
        var featureId = $(_this).attr("data-personId");

        if (featureId && featureId != null && featureId != "") {
            var feature = pageLocation.dfn.harita.feature.get(featureId)[featureId];
            if (feature) {
                pageLocation.dfn.harita.helper.PanTo(feature.getGeometry().getExtent());
                pageLocation.dfn.harita.map.getView().setZoom(14);

                $("#Harita").trigger("select:feature", { type: "add", element: feature });

                $(".btn-bar.right").animate({ right: -800 }, 200);
            }
            else {
                MesajWarning("Konum bulunamadı.", "Uyarı");
            }
        }
        else {
            MesajWarning("Konum bulunamadı.", "Uyarı");
        }
    },
    onChangePerson: function (_this) {
        var featureId = _this.sender.value();
        var startDateTime = $('#StartDateTime').data('kendoDateTimePicker').value();
        var endDateTime = $('#EndDateTime').data('kendoDateTimePicker').value();
        pageLocation.getDataAndApply(featureId, startDateTime, endDateTime);
    },
    onChangeDate: function () {
        var startDateTime = $('#StartDateTime').data('kendoDateTimePicker').value();
        var endDateTime = $('#EndDateTime').data('kendoDateTimePicker').value();
        var user = $('#userId').val();
        pageLocation.getDataAndApply(user, startDateTime, endDateTime);
    },
    getDataAndApply: function (userId, startDateTime, endDateTime) {
        GetJsonDataFromUrl('/UT/VWUT_LocationConfigUser/GetMapData', { startDate: kendo.toString(startDateTime, 'yyyy-MM-dd HH:mm'), endDate: kendo.toString(endDateTime, 'yyyy-MM-dd HH:mm'), userId: userId }, function (res) {
        
            if (res != null) {
                pageLocation.defines.filteredData = res;
            }
            pageLocation.applyFilter();
        }, "Konumlar yükleniyor..");
    },
    search: function (_this) {
        var type = $(_this).attr("data-content");
        var key = $(_this).val();

        pageLocation.loadPanelData(type, key);
    },
    getPersonDetailForHover: function (data) {
        var person = pageLocation.defines.filteredData;

        var text = '<div class="item">                                                                                  ' +
            '  <div class="head text-center clearfix">                                                                  ' +
            '      <a target="_blank" href="#">                                                                         ' +
            '          <div class="col-xs-12">' + person.FullName + '</div>                                           ' +
            '          <div class="col-xs-12" style="padding-top:0px;"><small> (' + person.Title + ') </small> </div> ' +
            '      </a>                                                                                                 ' +
            '  </div>                                                                                                   ' +
            '   <ul class="list-group clear-list m-t">                                                                  ' +
            '        <li class="list-group-item fist-item">                                                             ' +
            '            <span class="pull-right listGroupValue">                                                       ' +
            pageLocation.parseTimeStamp(data.timeStamp) +
            '            </span>                                                                                        ' +
            '            Konumun Alındığı Tarih                                                                         ' +
            '        </li>                                                                                              ' +
            '    </ul>                                                                                                  ' +
            '</div>';

        return text;
    },
    parseTimeStamp: function (timeStamp) {
        return kendo.toString(new Date(timeStamp * 1), 'dd.MM.yyyy HH:mm');
    }
}

$(document)
    .on("ready", function () {

        pageLocation.init();

        $('[data-target=".search-content"]').attr("data-placement", "left");
        $('[data-target=".search-content"]').tooltip();
    })
    .on("click", '[data-name="filterType"]', function (e) {
        var _this = $(this);

        _this.parent().find('.filterButtonActive').removeClass("filterButtonActive").addClass("filterButtonPassive");
        _this.parent().find('.fa-check').removeClass('fa fa-check');
        _this.removeClass("filterButtonPassive").addClass("filterButtonActive");
        _this.find('span').addClass("fa fa-check");
    })
    .on("hover:feature", "#Harita", function (e, resp) {
        pageLocation.dfn.harita.overlay.remove('slider-station');

        if (resp.type == "add") {
            var elem = resp.element;
            var prop = elem.get("properties");

            if (prop) {
                text = pageLocation.getPersonDetailForHover(prop);
                var geometry = resp.element.getGeometry();
                var coordinate = geometry.getCoordinates();
                var pixel = pageLocation.dfn.harita.map.getPixelFromCoordinate(coordinate);

                var tooltiplocation = "";

                if (pixel[1] < 400)
                    tooltiplocation = "top-right";
                else if (screen.height - pixel[1] < 400)
                    tooltiplocation = "bottom-right";
                else
                    tooltiplocation = "center-right";

                pageLocation.dfn.harita.overlay.add('slider-station', text, '', tooltiplocation, elem.getGeometry().getCoordinates(), [-10, -30]);
                pageLocation.dfn.harita.map.updateSize();
            }
        }
    })
    .on("select:feature", "#Harita", function (e, resp) {
        pageLocation.dfn.harita.overlay.remove('slider-station');

        if (resp.type == "add") {
            selectedFeature = resp.element.getId();
            var prop = resp.element.get("properties");

            if (prop != null) {

                if ($('#selectCompany').hasClass("hide")) {
                    $('#selectCompany').removeClass("hide");
                }

                if ($('#selectCompany').hasClass("active")) {
                    $('#selectCompany').removeClass("active");
                    setTimeout(function () {
                        $('#selectCompany').trigger("click");
                    }, 300)
                }
                else {
                    $('#selectCompany').trigger("click");
                }
                $('#personTemplate').html(null)

                var elem = $($('#selectedPerson').html());
                var personelInfos = pageLocation.defines.filteredData;
                var firstLocationInfo = $.Enumerable.From(pageLocation.defines.filteredData.LocationTrackings).OrderBy(x => x.timeStamp).FirstOrDefault();
                var lastLocationInfo = $.Enumerable.From(pageLocation.defines.filteredData.LocationTrackings).OrderByDescending(x => x.timeStamp).FirstOrDefault();
                var locations = prop.location.split('(')[1].replace(')', "");
                var latitude = locations.split(" ")[1].replace(".", ",").substr(0, 7);
                var longitude = locations.split(" ")[0].replace(".", ",").substr(0, 7);

                elem.find('[data-id="personFullName"]').html(personelInfos.FullName != null ? personelInfos.FullName : "-");
                elem.find('[data-id="personTitle"]').html(personelInfos.Title ? personelInfos.Title : "-");
                elem.find('[data-id="locationDate"]').html(pageLocation.parseTimeStamp(prop.timeStamp));
                elem.find('[data-id="locationInfo"]').html(longitude + " " + latitude);
                elem.find('[data-id="deviceInfo"]').html(prop.deviceId != null ? prop.deviceId : "Cihaz bilgisi bulunamadı.");
                elem.find('[data-id="firstLocationDate"]').html(pageLocation.parseTimeStamp(firstLocationInfo.timeStamp));
                elem.find('[data-id="lastLocationDate"]').html(pageLocation.parseTimeStamp(lastLocationInfo.timeStamp));
                elem.find('[data-id="personelPhoto"]').attr('src', personelInfos.ProfilePhoto);
                $('#personTemplate').append(elem.html());
            }

            pageLocation.dfn.harita.feature.properties.interactionSelect.getFeatures().clear();
        }
    });