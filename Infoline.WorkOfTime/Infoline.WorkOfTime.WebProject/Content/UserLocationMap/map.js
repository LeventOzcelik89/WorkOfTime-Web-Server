var control = false;
var selectedFeature = null;

var page = {
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
                $(".btn-bar.right").animate({ right: -280 }, 200);
            } else {
                $(elem).addClass("active");
                $(".btn-bar.right").animate({ right: -280 }, 200, function () {
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

    
        var firstLocation = new ol.style.Style({
            image: new ol.style.Icon({
                opacity: 1,
                src: '/Content/Custom/img/PersonsBackImage/greenMarker.png',
                scale: 1.0,
                anchor: [0.5, 64],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
            })
        });

        var locations = new ol.style.Style({
            image: new ol.style.Icon({
                opacity: 1,
                src: '/Content/Custom/img/PersonsBackImage/redMarker.png',
                scale: 0.18,
                anchor: [0.5, 64],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
            })
        });

        var lastLocation = new ol.style.Style({
            image: new ol.style.Icon({
                opacity: 1,
                src: '/Content/Custom/img/PersonsBackImage/orangeMarker.png',
                scale: 1.0,
                anchor: [0.5, 64],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
            })
        });



        page.dfn.harita.style.addCustom("firstLocationStyle", firstLocation);
        page.dfn.harita.style.addCustom("locationsStyle", locations);
        page.dfn.harita.style.addCustom("lastLocationStyle", lastLocation);

        page.dfn.harita.layer.addVector("İlk Konum", "FirstLocation", "firstLocationStyle", "firstLocationStyle", "firstLocationStyle")
        page.dfn.harita.layer.addVector("Konumlar", "Locations", "locationsStyle", "locationsStyle", "locationsStyle")
        page.dfn.harita.layer.addVector("Son Konum", "LastLocation", "lastLocationStyle", "lastLocationStyle", "lastLocationStyle")

        page.dfn.harita.layer.addVector("Action", "Action");

        page.dfn.harita.feature.events(function (feature, layer) { return true; }, function (feature, layer) { return true; }, function (event, object) { });

    },
    locateFeatureToMap: function () {
        var points = [];

        if (page.defines.filteredData != undefined) {

            var trackingDatas = page.defines.filteredData.LocationTrackings;
            if (trackingDatas != null) {

                if (trackingDatas.length > 0) {
                    $.each($.Enumerable.From(trackingDatas).OrderBy(x => x.timeStamp).ToArray(), function (i, data) {

                        if (data.location != null) {
                            if (i == 0) {

                                var feature = page.dfn.harita.feature.add("FirstLocation", data.id, data.location);
                                if (feature) {
                                    feature.set("properties", data);

                                    points.push(feature.getGeoJSON().geometry.coordinates.join(" "));
                                }
                            }
                            else if (i == trackingDatas.length - 1) {

                                var feature = page.dfn.harita.feature.add("LastLocation", data.id, data.location);
                                if (feature) {
                                    feature.set("properties", data);

                                    points.push(feature.getGeoJSON().geometry.coordinates.join(" "));
                                }
                            }
                            else {

                                var feature = page.dfn.harita.feature.add("Locations", data.id, data.location);
                                if (feature) {
                                    feature.set("properties", data);
                                    var coords = page.dfn.harita.helper.SQLtoGeometry(data.location);
                                    if (coords != null) {
                                        points.push(data.location.replace('POINT (', '').replace(')', ''));
                                    }

                                }
                            }
                        }
                    });
                    var line = "LINESTRING(" + points.join(", ") + ")";
                    page.dfn.harita.feature.add("Action", "linestring", line);

                    page.dfn.harita.layer.panTo("Locations");

                }

            }
        }
    },
    applyFilter: function () {
        page.dfn.harita.layer.get("FirstLocation")["FirstLocation"].getSource().clear();
        page.dfn.harita.layer.get("Locations")["Locations"].getSource().clear();
        page.dfn.harita.layer.get("LastLocation")["LastLocation"].getSource().clear();
        page.dfn.harita.layer.get("Action")["Action"].getSource().clear();
        page.dfn.harita.feature.properties.interactionSelect.getFeatures().clear();

        page.locateFeatureToMap();

        if ($('#searchButton').hasClass("active")) {
            $('#searchButton').trigger("click");
        }

    },
    goLocation: function (_this) {
        var featureId = $(_this).attr("data-personId");

        if (featureId && featureId != null && featureId != "") {
            var feature = page.dfn.harita.feature.get(featureId)[featureId];
            if (feature) {
                page.dfn.harita.helper.PanTo(feature.getGeometry().getExtent());
                page.dfn.harita.map.getView().setZoom(14);

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
    onChangeDate: function () {
        var startDateTime = $('#StartDateTime').data('kendoDateTimePicker').value();
        var endDateTime = $('#EndDateTime').data('kendoDateTimePicker').value();
        var personId = $('#userId').data('kendoDropDownList').value();
        if (personId != undefined && personId != "") {
            page.getDataAndApply(personId, startDateTime, endDateTime);
            //page.applyFilter();
        }
    },
    getDataAndApply: function (userId, startDateTime, endDateTime) {
        GetJsonDataFromUrl('/SH/SH_UserLocationTracking/GetMapData', { startDate: kendo.toString(startDateTime, 'yyyy-MM-dd HH:mm'), endDate: kendo.toString(endDateTime, 'yyyy-MM-dd HH:mm'), userId: userId }, function (res) {
            page.defines.filteredData = res;
            page.applyFilter();
        }, "Konumlar yükleniyor..");
    },
    search: function (_this) {
        var type = $(_this).attr("data-content");
        var key = $(_this).val();

        page.loadPanelData(type, key);
    },
    getPersonDetailForHover: function (data) {
        var person = page.defines.filteredData;
        var locations = data.location.split('(')[1].replace(')', "");
        var latitude = locations.split(" ")[1].replace(".", ",").substr(0, 7);
        var longitude = locations.split(" ")[0].replace(".", ",").substr(0, 7);
        var firstLocationInfo = $.Enumerable.From(page.defines.filteredData.LocationTrackings).OrderBy(x => x.timeStamp).FirstOrDefault();
        var lastLocationInfo = $.Enumerable.From(page.defines.filteredData.LocationTrackings).OrderByDescending(x => x.timeStamp).FirstOrDefault();
        var text = '<div class="item">                                                                                  ' +
            '  <div class="head text-center clearfix">                                                                  ' +
            '          <div class="col-xs-12">Konum Bilgileri</div>                                                     ' +
            '  </div>                                                                                                   ' +
            '   <ul class="list-group clear-list m-t">                                                                  ' +
            '        <li class="list-group-item fist-item">                                                             ' +
            '            <span class="pull-right listGroupValue">                                                       ' +
            page.parseTimeStamp(data.timeStamp)                                                                           +
            '            </span>                                                                                        ' +
            '            Konumun Alındığı Tarih :                                                                       ' +
            '        </li>                                                                                              ' +
            '        <li class="list-group-item fist-item">                                                             ' +
            '            <span class="pull-right listGroupValue">                                                       ' +
            longitude +" "+ latitude                                                                                      +
            '            </span>                                                                                        ' +
            '            Konum Bilgisi :                                                                                ' +
            '        </li>                                                                                              ' +
            '        <li class="list-group-item fist-item">                                                             ' +
            '            <span class="pull-right listGroupValue">                                                       ' +
            (data.deviceId != null ? data.deviceId : "Cihaz bilgisi bulunamadı.")                                           +
            '            </span>                                                                                        ' +
            '            Cihaz Bilgisi :                                                                                ' +
            '        </li>                                                                                              ' +
            '        <li class="list-group-item fist-item">                                                             ' +
            '            <span class="pull-right listGroupValue">                                                       ' +
            page.parseTimeStamp(firstLocationInfo.timeStamp)                                                              +
            '            </span>                                                                                        ' +
            '            İlk Konum Tarihi :                                                                             ' +
            '        </li>                                                                                              ' +
            '        <li class="list-group-item fist-item">                                                             ' +
            '            <span class="pull-right listGroupValue">                                                       ' +
            page.parseTimeStamp(lastLocationInfo.timeStamp)                                                               +
            '            </span>                                                                                        ' +
            '            Son Konum Tarihi :                                                                             ' +
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
        page.init();
        page.onChangeDate();
    })
    .on("click", '[data-name="filterType"]', function (e) {
        var _this = $(this);

        _this.parent().find('.filterButtonActive').removeClass("filterButtonActive").addClass("filterButtonPassive");
        _this.parent().find('.fa-check').removeClass('fa fa-check');
        _this.removeClass("filterButtonPassive").addClass("filterButtonActive");
        _this.find('span').addClass("fa fa-check");
    })
    .on("hover:feature", "#Harita", function (e, resp) {
        page.dfn.harita.overlay.remove('slider-station');
        if (resp.type == "add") {
            var elem = resp.element;
            var prop = elem.get("properties");

            if (prop) {
                text = page.getPersonDetailForHover(prop);
                var geometry = resp.element.getGeometry();
                var coordinate = geometry.getCoordinates();
                var pixel = page.dfn.harita.map.getPixelFromCoordinate(coordinate);

                var tooltiplocation = "";

                if (pixel[1] < 400)
                    tooltiplocation = "top-right";
                else if (screen.height - pixel[1] < 400)
                    tooltiplocation = "bottom-right";
                else
                    tooltiplocation = "center-right";

                page.dfn.harita.overlay.add('slider-station', text, '', tooltiplocation, elem.getGeometry().getCoordinates(), [-10, -30]);
                page.dfn.harita.map.updateSize();
            }
        }
    })
    .on("select:feature", "#Harita", function (e, resp) {
        page.dfn.harita.overlay.remove('slider-station');

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
            }

            page.dfn.harita.feature.properties.interactionSelect.getFeatures().clear();
        }
    });