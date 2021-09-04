
var ExcelImport = function (url) {

    var $this = this;

    $this._init = function () {
        $this.matchElement = $('#ImportExcelMatch');
        $this.errorsElement = $('#ImportExcelErrors');
        $this.saveButtonElement = $('#ImportExcelSaveButton');
        $this.containerElement = $('#ImportExcelContainer');
        $this.ControllerURL = url;
    };

    //  Dışarıdan Girilecek.
    $this.postData = null;
    $this.ControllerURL = null;
    //  Result True ise
    $this.successFunction = function () {

    }
    //  Örnek - Dışarıdan gelecek
    $this.matchedFields = [
        { DBField: 'QuarantyStartDate', Label: 'Garanti Başlangıç Tarihi', Type: 'DateTime', ExcelField: null, Nullable: false },
        //{ DBField: 'Name', Label: 'Envanter Adı', Type: 'string', ExcelField: null, Nullable: false },
        { DBField: 'SerialNo', Label: 'Seri Numarası', Type: 'string', ExcelField: null, Nullable: false },
        { DBField: 'BarcodeNo', Label: 'Barkod Numarası', Type: 'string', ExcelField: null, Nullable: true },
        { DBField: 'IMEINo', Label: 'IMEI Numarası', Type: 'string', ExcelField: null, Nullable: true },
    ];

    $this.excelData = null;

    $this.excelFields = [];

    $this.postBefore = function () { };

    $this.postFunction = function () {

        if (typeof ($this.postBefore) === 'function') {
            $this.postBefore();
        }

        var nullFields = $.Enumerable.From(this.matchedFields).Where(function (a) { return a.Nullable == false && a.ExcelField == null });

        if (nullFields.Count() > 0) {
            MesajWarning('Eşleşmemiş alanlar mevcut :\n' + nullFields.Select(function (a) { return a.Label; }).ToArray().join(', '), 'Bilgilendirme !..');
            return false;
        }

        var bePostData = $.Enumerable.From($this.excelData).Select(function (a) {

            var value = {};

            $.each($this.matchedFields, function (i, item) {

                value[item.DBField] = item.Type == 'DateTime'
                    ? $this.ConvertToDate(a[item.ExcelField])
                    : a[item.ExcelField];

            });

            return value;

        }).ToArray();

        var data = { query: JSON.stringify(bePostData) };

        var retrn = false;

        if ($this.postData != null) {
            $.each($this.postData, function (i, item) {
                if (item() == '' || item() == null) {
                    MesajWarning('Gerekli Alanlar Seçilmedi !...', 'Eksik Veri');
                    retrn = true;
                }
                data[i] = item();
            });
        }

        if (retrn == true) { return; }

        if (bePostData == null || bePostData.length < 1) {
            MesajWarning('Excel Dosyası Seçilmedi !...', 'Eksik Veri');
            return;
        }

        $.ajax({
            //url: '/INV/INV_FixtureTariffsInvoiceDetails/InsertExcel',
            url: $this.ControllerURL,
            type: 'POST',
            data: data,
            dataType: 'json',
            beforeSend: function () {
                $('body').loadingModal({ text: "Excel Yükleniyor.. Lütfen Bekleyiniz..", animation: 'rotatingPlane', backgroundColor: 'black' });
            },
            success: function (res) {

                feedback(res.FeedBack);

                if (res.Result == false) {

                    var thtml = '<hr />' + '<h2 class="text-center text-danger"><b>Hatalar</b></h2>';

                    thtml += '<h4 class="text-center text-danger">Excel dosyasındaki sorunları çözdükten sonra tekrar yükleyin</h4>';

                    thtml += '<table id="beKendoGrid">';
                    thtml += '<colgroup>';
                    thtml += '<col />';
                    thtml += '<col />';
                    thtml += '</colgroup>';

                    thtml += '<thead>' + '<tr>';
                    thtml += '<th data-field="Key">' + 'Alan' + '</th>';
                    thtml += '<th data-field="Value">' + 'Hata' + '</th>';
                    thtml += '</tr>';
                    thtml += '</thead>';
                    thtml += '<tbody>';

                    $.each(res.Object, function (i, item) {
                        thtml += '<tr>' + '<td>' + item.Key + '</td>' + '<td>' + item.Value + '</td>' + '</tr>';
                    });

                    thtml += '</tbody>';
                    thtml += '</table>';

                    $this.errorsElement.html(thtml);

                    $this.errorsElement.find('#beKendoGrid').kendoGrid({
                        dataSource: { pageSize: 25 },
                        height: 550,
                        sortable: true,
                        sortable: true,
                        pageable: {
                            refresh: false,
                            pageSizes: true,
                            buttonCount: 1,
                            pageSizes: [25, 50, 250]
                        },

                    });

                }

                if (res.Result == true) {

                    $this.containerElement.slideUp();
                    $this.matchElement.html(null);
                    $(".k-widget.k-upload").find("ul").remove();
                    $(".k-upload-status-total").remove();

                    $this.SuccessFunction();

                }

            },
            complete: function (response) {


                $('body').loadingModal('destroy');

            }
        });

    };

    $this.excelFileSuccessedFunction = function (e) {

        feedback(e.response.FeedBack);

        if (!e.response.Result) {
            return;
        }

        if (e.operation == "remove") {

            $('#ImportExcelMatch').html("");
            $this.containerElement.hide();

            return;

        }

        var fileName = e.response.Object.FileName;
        var fileDir = e.response.Object.FileDir;
        var ext = e.response.Object.Extension;

        alasql.promise('select * from ' + ext + '("' + fileDir + fileName + '")')
            .then(function (data) {

                $this.excelFields = [];
                $this.excelData = data;

                var res = [], obj = {};
                $.each(data, function (i, item) { $.each(item, function (i1, item1) { res.push(i1); }); });
                $.each($.Enumerable.From(res).Distinct().ToArray(), function (i, item) { obj[item] = data[0][item] });

                $.each(obj, function (i1, item1) {
                    $this.excelFields.push(i1);
                });

                var _form = $('<form class="form-horizontal" role="form" novalidate="true">')

                $.each($this.matchedFields, function (i1, item1) {

                    //  Selectler oluşturuluyor...
                    var _select = $('<select class="form-control" id="' + $this.guid() + '" />')
                        .append('<option value=""></option>')
                        .append($.Enumerable.From($this.excelFields).Select(function (a) {

                            var res = '<option value="' + a + '"';
                            var elem = $.Enumerable.From($this.matchedFields).Where(function (a) { return a.DBField == item1.DBField }).FirstOrDefault();

                            if (elem != null && elem.Label == a) {
                                res += ' selected="selected"';
                            }

                            res += '>' + a + '</option>';

                            return res;

                        }).ToArray())
                        .on('change', function (e) {

                            e.preventDefault();

                            var group = $($(this).parents('.form-group').eq(0));
                            var field = $.Enumerable.From($this.matchedFields).Where(function (a) { return a.DBField == group.attr('data-dbfield') }).FirstOrDefault();

                            if (field == null) {
                                $(this).val(null);
                                return;
                            }

                            field.ExcelField = $(this).val();

                            return false;

                        })
                    ;

                    var _selectDiv = $('<div class="col-md-7 clearfix" />')
                        .append(_select);

                    var _group = $('<div class="form-group" data-dbfield="' + item1.DBField + '" data-excelfield="' + item1.ExcelField + '" data-Type="' + item1.Type + '" />')
                        .append($('<div class="col-md-5 clearfix">' + '<label class="control-label ' + (item1.Nullable == false ? 'req' : '') + '">' + item1.Label + '<label/>' + '</div>'))
                        .append(_selectDiv)
                        .appendTo(_form);

                });

                $this.matchElement.append(_form);
                $this.containerElement.slideDown();

                _form.find('select').trigger('change');

            })
        ;
    };

    $this.guid = function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    };

    $this.ConvertToDate = function (e) {

        var oaDate = e;
        var date = new Date();
        date.setTime((oaDate - 25569) * 24 * 3600 * 1000);

        return date;

    };

    $this.init = function () {

        $this._init();

        $this.saveButtonElement.on('click', function (e) {

            e.preventDefault();

            $this.postFunction();

            return false;

        })

    }

};
