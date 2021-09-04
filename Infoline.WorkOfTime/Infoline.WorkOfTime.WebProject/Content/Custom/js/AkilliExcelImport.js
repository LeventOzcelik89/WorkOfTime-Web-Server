var ExcelImport = function (url) {

    var $this = this;
    $this._init = function () {
        $this.matchElement = $('#ImportExcelMatch');
        $this.errorsElement = $('#ImportExcelErrors');
        $this.saveButtonElement = $('#ImportExcelSaveButton');
        $this.containerElement = $('#ImportExcelContainer');
        $this.inputFileElement = $('#ExcelInputFile');
        $this.ControllerURL = url;
    };

    //  Dışarıdan Girilecek.
    $this.postData = null;
    $this.ControllerURL = null;
    //  Result True ise
    $this.successFunction = function () { };

    //  Örnek - Dışarıdan gelecek
    $this.matchedFields = [
        { DBField: 'QuarantyStartDate', Label: 'Garanti Başlangıç Tarihi', Type: 'DateTime', ExcelField: null, Nullable: false },
        //{ DBField: 'Name', Label: 'Envanter Adı', Type: 'string', ExcelField: null, Nullable: false },
        { DBField: 'SerialNo', Label: 'Seri Numarası', Type: 'string', ExcelField: null, Nullable: false },
        { DBField: 'BarcodeNo', Label: 'Barkod Numarası', Type: 'string', ExcelField: null, Nullable: true },
        { DBField: 'IMEINo', Label: 'IMEI Numarası', Type: 'string', ExcelField: null, Nullable: true },
    ];

    //  type = string, date vb || field = bağlantıdan sonra dönen nesne propertysi || title = Propertynin görülecek metni
    $this.ReturnFields = [
        { type: "string", width: 300, field: "Key", title: "Hat" },
        { type: "string", field: "Value", title: "Hata" }
    ];

    $this.excelData = null;
    $this.excelFields = [];

    $this.postFunction = function () {

        var nullFields = $.Enumerable.From(this.matchedFields).Where(function (a) { return a.Nullable == false && (a.ExcelField == null || a.ExcelField == '') });

        if (nullFields.Count() > 0) {
            MesajWarning('Eşleşmemiş alanlar mevcut :<br/>' + nullFields.Select(function (a) { return a.Label }).ToArray().join(', '), 'Bilgilendirme !..');
            return false;
        }

        var bePostData = $.Enumerable.From($this.excelData).Select(function (a) {

            var value = {};
            var valid = false;
            $.each($this.matchedFields, function (i, item) {

                value[item.DBField] = item.Type == 'DateTime'
                    ? $this.ConvertExcelFileDate(a[item.ExcelField])
                    : a[item.ExcelField];

                if (typeof (value[item.DBField]) != 'undefined' && !(item.Type == 'DateTime' && value[item.DBField] == 'Invalid Date')) {
                    valid = true;
                }

            });

            return (valid == true) ? value : null;

        }).Where(function (a) { return a != null }).ToArray();

        var data = { query: JSON.stringify(bePostData) };
        $this.postData.warrantyFirm = $this.postData.warrantyFirm == null ? newGuid() : $this.postData.warrantyFirm;
        if ($this.postData != null) {
            $.each($this.postData, function (i, item) {
                if (item() == '' || item() == null) {
                 //   MesajWarning('Gerekli Alanlar Seçilmedi !...', 'Eksik Veri');
                    return;
                }
                data[i] = item();
            });
        }

        if (bePostData == null || bePostData.length < 1) {
            MesajWarning('Excel Dosyası Seçilmedi !...', 'Eksik Veri');
            return;
        }

        $.ajax({
            url: $this.ControllerURL,
            type: 'POST',
            data: data,
            dataType: 'json',
            beforeSend: function () {
                $('body').loadingModal({ text: "Excel Yükleniyor.. Lütfen Bekleyiniz..", animation: 'rotatingPlane', backgroundColor: 'black' });
            },
            success: function (res) {

                feedback(res.FeedBack);

                if (res.Result == true) {

                    $this.saveButtonElement.hide();

                    var thtml = '<hr />' + '<h2 class="text-center text-danger"><b>Hatalar</b></h2>';
                    thtml += '<h4 class="text-center text-danger">Excel dosyasındaki sorunları çözdükten sonra tekrar yükleyin</h4>';
                    thtml += '<div id="beKendoGrid"></div>';
                    $this.errorsElement.html(thtml);

                    var regExp = new RegExp("\\/Date\\([0-9]+\\)\\/");
                    var JsonDate = function (_d) { return new Date(parseInt(_d.replace('/Date(', '').replace(')/', ''), 10)); }

                    if (res.Object != null) {
                        $.each(res.Object, function (i, item) {
                            if (regExp.test(item)) {
                                var converted = JsonDate(item);
                                if (converted instanceof Date) {
                                    item = converted;
                                }
                            }
                        });
                    }

                    var _fields = [];
                    $.Enumerable.From($this.ReturnFields).Select(function (a) { _fields[a.field] = { 'type': a.type }; }).ToArray();

                    $('#beKendoGrid').kendoGrid({
                        toolbar: [{ name: "excel", text: "Excel'e Aktar" }],
                        excel: {
                            fileName: "Hata Listesi.xlsx",
                            proxyURL: "https://demos.telerik.com/kendo-ui/service/export",
                            filterable: true,
                            allPages: true
                        },
                        dataSource: {
                            data: res.Object,
                            pageSize: 50,
                            schema: {
                                model: {
                                    fields: _fields
                                }
                            },
                        },
                        columns: $this.ReturnFields,
                        height: 550,
                        sortable: true,
                        sortable: true,
                        pageable: {
                            refresh: false,
                            pageSizes: true,
                            pageSizes: [50, 100, 200, "Tümü"],
                            buttonCount: 5
                        },
                    });

                    if ($('#beKendoGrid').data("kendoGrid")._data.length == 0) {
                        $this.errorsElement.slideUp();
                    }

                    $this.SuccessFunction();

                }

                if (res.Result == false) {

                    $this.containerElement.slideUp();
                    $this.matchElement.html(null);
                    $(".k-widget.k-upload").find("ul").remove();
                    $(".k-upload-status-total").remove();

                }

            },
            complete: function (response) {
                $('body').loadingModal('destroy');
            }
        });

    };

    $this.excelFile = null;
    $this.excelFileSelected = function (event) {

        if (event.target.files.length == 0) {
            $this.inputFileElement.val(null);
            $this.containerElement.slideUp();
            return;
        }

        $this.excelFile = [event];
        $this.inputFileElement.val(event.target.files[0].name)

        alasql('SELECT * FROM FILE(?, { headers : true })', $this.excelFile, function (data) {

            $this.excelFields = [];
            $this.excelData = data;

            var res = [], obj = {};
            $.each(data, function (i, item) { $.each(item, function (i1, item1) { res.push(i1); }); });
            $.each($.Enumerable.From(res).Distinct().ToArray(), function (i, item) { obj[item] = data[0][item] });

            $.each(obj, function (i1, item1) {
                $this.excelFields.push(i1);
            });

            var _form = $('<div />')

            $.each($this.matchedFields, function (i1, item1) {

                //  Selectler oluşturuluyor...
                var _selected = $this.guid();
                var elem = "";
                var _select = $('<select class="form-control" id="' + _selected + '" />')
                    .append('<option value="">Seçim Yapın</option>')
                    .append($.Enumerable.From($this.excelFields).Select(function (a) {

                        var res = '<option value="' + a + '"';
                        elem = $.Enumerable.From($this.matchedFields).Where(function (a) { return a.DBField == item1.DBField }).FirstOrDefault();

                        if (elem != null && elem.Label == a) {
                            res += ' selected="selected"';
                        }

                        res += '>' + a + '</option>';
                        elem.ExcelField = a;

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
                    .append($('<div class="col-md-5 clearfix">' + '<label class="control-label ' + (item1.Nullable == false ? 'req' : '') + '">' + item1.Label + '<label/>' + '</   div>'))
                    .append(_selectDiv)
                    .appendTo(_form);

            });

            $this.matchElement.append(_form);
            $this.containerElement.slideDown();

            _form.find('select').trigger('change');

        });

    }

    $this.guid = function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    };

    $this.ConvertExcelFileDate = function (e) {

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

        });

    }

};
