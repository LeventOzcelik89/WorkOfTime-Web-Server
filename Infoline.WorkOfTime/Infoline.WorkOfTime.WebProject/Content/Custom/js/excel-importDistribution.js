var dRow = [
        //{ QuarantyStartDate: "Garanti Başlangıç Tarihi", Type: 'DateTime' },
        //{ Name: "Demirbaş Adı", Type: 'string' },
        //{ BarcodeNo: "Barkod Numarası", Type: 'string' },
        { SerialNo: "Seri Numarası", Type: 'string' },
        { IMEINo: "IMEI Numarası", Type: 'string' }
];
var excelData = [];

$(document).on('change', '#tabloColumn select', function(e) {
        var $this = $(this);
        e.preventDefault();
        $.each($("#tabloColumn").find('select'), function(index, item) {
            if ($(item).val() == $($this).val() && $(item).prop("id") != $($this).prop("id")) {
                MesajWarning("bu eşleşme başka veri ile eşleşmiştir.", "Uyarı");
                $this.selectedIndex = 0;
                return false;
            }
        });
    })
    .on('click', '[data-proses="save"]', function (e) {
     
        var control = true;
        e.preventDefault();
        //$.each($("#tabloColumn").find('select'), function(index, item) {
        //    var $this = $(item);
        //    $.each($("#tabloColumn").find('select'), function(index2, item2) {
        //        if ($(item2).val() == $($this).val() && $(item2).prop("id") != $($this).prop("id")) {
        //            MesajWarning("bu eşleşme başka veri ile eşleşmiştir.", "Uyarı");
        //            control = false;
        //            return false;
        //        }
        //    });
        //});

        //$.each($("#tabloColumn").find('select'), function(index, item) {
        //    if (item.selectedIndex == 0) {
        //        MesajWarning("Seçimleri boş geçmeyiniz.", "Uyarı");
        //        control = false;
        //        return false;
        //    }
        //});

        if (control == true) {
            $.each(excelData, function(i, e) {
                $.each(e, function(prop, value) {
                    if (typeof (value) != "undefined" && value != null) {
                        $.each($("#tabloColumn select"), function(index, item) {
                            if ($(item).val() == prop) {
                                if ($('label[data-colunmdb="' + $(item).attr('data-colunmdb') + '"]').attr('data-type') == 'DateTime') {
                                    var rs = FromOADate(value);
                                    e[$(item).attr('data-colunmdb')] = rs;
                                } else {
                                    e[$(item).attr('data-colunmdb')] = value.toString();
                                }
                            }
                        });
                    }
                    delete e[prop];
                });
                e["MaterialStockid"] = $("#MaterialStockid").val();
            });
            var entity = { query: JSON.stringify(excelData), location: $("#Location").val(), distributonId: $("#form-excelwrapper").attr('data-Distributon') };
            $.ajax({
                url: $("#form-excelwrapper").attr('data-saveurl'),
                type: 'POST',
                data: entity,
                dataType: 'json',
                success: function(res) {
                    if (res.Result == true) {
                        //$($("#form-excelwrapper").attr('data-gridname')).data("kendoGrid").dataSource.read();
                        $("#VWINV_DistributionDetails").data("kendoGrid").dataSource.read()
                        $("#form-excelwrapper").css('display', 'none');
                        $("#tabloColumn").empty();
                        $(".k-widget.k-upload").find("ul").remove();
                        $(".k-upload-status-total").remove();
                        MesajSuccess("Excel veri aktarma işlemi başarılı");
                    } else {
                        if (res.Object == "Envanter Kaydı Bulunamadı") {
                            MesajWarning(res.Object);
                        }
                        else {
                            MesajError("Sistemde bir hata oluştu.");
                        }
                      
                    }
                }
            });
        }
    });

function onSuccess(e) {
    var type = e.files[0].extension.substring(1);
    var tabloName = "/Files/" + e.files[0].name;
    alasql.promise('select * from ' + type + '("' + tabloName + '")')
        .then(function (data) {
            var template = "";
            $.each(dRow, function (prop, value) {
                $.each(value, function (p, v) {
                    if (p == "Type") {
                        return;
                    }
                    template += "<div class=\"row\"><div class=\"col-md-6 clearfix\"><label data-type=\"" + value.Type + "\" data-colunmdb=\"" + p + "\">" + v + " <label/></div><div class=\"col-md-6  clearfix\"><select  data-colunmdb=\"" + p + "\" id=\"" + guid() + "\"><option value=\"\">Seçim Yapınız</option>";
                    $.each(data[0], function (index, item) {
                        template += "<option value=\"" + index + "\">" + index + "</option>";
                    });
                    template += "</select></div></div>";
                });
            });
            $('#tabloColumn').append("<form class=\"form-horizontal\" role=\"form\" novalidate=\"true\">" + template + "</form>");
            excelData = data;
            $('#form-excelwrapper').css('display', 'block');
        });
}


function FromOADate(parameters) {
    var oaDate = parameters;
    var date = new Date();
    date.setTime((oaDate - 25569) * 24 * 3600 * 1000);
    return date;
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}