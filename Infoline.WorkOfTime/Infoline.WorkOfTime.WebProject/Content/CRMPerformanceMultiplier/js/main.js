var _performance = {

    Slider: {
        MakeSliderOption: function (data) {

            var ranges = data.Ranges;

            if (data.Ranges[0] == 0) {
                ranges = data.Ranges.slice(1);
            }

            return {
                start: ranges,
                connect: $.merge(ranges.map(function (item, i) { return true; }), [true]),
                direction: 'ltr',
                tooltips: true,
                step: 1,
                pips: {
                    mode: 'range',
                    density: 1
                },
                range: {
                    min: 0,
                    max: 200
                }
            };

        },
        UpdateSliderRanges: function (data, newValues) {

            data.Ranges = $.merge([0], newValues);

            _performance.Table.UpdateHeaders(data);

        },
        MakeSlider: function (data) {

            if (data.Ranges[0] == 0) {
                data.Ranges = data.Ranges.slice(1);
            }

            var _containerSlider = _performance.container.find('[data-type="' + data.Type + '"] [data-type="SliderContainer"]');

            _containerSlider.find('.Slider').html('<div class="slider"></div>');

            data.Slider = noUiSlider.create(_containerSlider.find('.slider')[0], _performance.Slider.MakeSliderOption(data));

            data.Slider.on('change', function (valuesStr, i, newValues) {

                _performance.Slider.UpdateSliderRanges(data, newValues);

            });

        },

        RemoveLastPoint: function (type) {

            var data = _performance.Data.TableData[type];

            if (data.Ranges.length <= 2) {
                MesajWarning('Daha fazla tanım satırı silemezsiniz', 'Uyarı !.');
                return;
            }

            //  Son value değerini sildik.
            data.Ranges.pop();
            $.each(data.Products, function (i, item) { item.Values.pop(); });

            data.Slider.destroy();
            _performance.Slider.MakeSlider(data);

            _performance.Table.UpdateHeaders(data);
            _performance.Table.RemoveLastColumn(data);

        },
        AddNewPoint: function (type) {

            var data = _performance.Data.TableData[type];

            //  Yeni value değeri ekledik
            data.Ranges.push(data.Ranges[data.Ranges.length - 1] + 5);
            $.each(data.Products, function (i, item) { item.Values.push(0); });

            data.Slider.destroy();
            _performance.Slider.MakeSlider(data);

            _performance.Table.UpdateHeaders(data);
            _performance.Table.AddNewColumn(data);

        },
    },

    Data: {
        TableData: {},
        GetAllData: function () {

            var res = [];

            $.each(_performance.Data.TableData, function (di, ditem) {

                $.each(ditem.Products, function (pi, pitem) {

                    if (pitem.ProductGroupId == undefined) { return; }

                    $.each(pitem.Values, function (vi, vitem) {

                        var elem = {
                            Point: vitem,
                            IsFocus: pitem.IsFocus,
                            TargetGroupType: parseInt(di),
                            MinPercentage: ditem.Ranges[vi],
                            MaxPercentage: ditem.Ranges[vi + 1] != undefined ? ditem.Ranges[vi + 1] : 9999,
                            ProductGroupId: pitem.ProductGroupId
                        };

                        res.push(elem);

                    });

                });

            });

            return res;

        },
        PostData: function () {

            var valid = true;

            $.each(_performance.Data.TableData, function (di, ditem) {

                if ($.Enumerable.From(ditem.Products).Where(function (a) { return a.IsFocus == true; }).Count() > 1) {

                    var _type = $.Enumerable.From(_performance.TargetGroupTypes).Where(function (a) { return a.type == di; }).FirstOrDefault();

                    MesajWarning('Bir performans çarpanı için birden fazla focus tanımı yapamazsınız. Lütfen ' + _type.Name + ' içerisindeki tanımı düzeltin ve yeniden deneyin.', 'Birden Fazla Focus Tanımı !...');

                    valid = false;

                }

            });

            if (valid == false) {
                return false;
            }

            $('#PerformanceMultipliers').val(JSON.stringify(_performance.Data.GetAllData()));

            return true;

        }
    },

    Table: {
        RemoveLastColumn: function (data) {

            var _containerTable = _performance.container.find('[data-type="' + data.Type + '"] [data-type="TableContainer"]');

            $.each(_containerTable.find('tbody tr'), function (i, item) {

                $(item).find('td').eq($(item).find('td').length - 1).remove();

            });

        },
        AddNewColumn: function (data) {

            var _containerTable = _performance.container.find('[data-type="' + data.Type + '"] [data-type="TableContainer"]');

            $.each(_containerTable.find('tbody tr'), function (i, item) {

                $(item).append('<td>' + '<input data-type="numeric" value="' + '0' + '" />' + '</td>');

            });

            $.each(_containerTable.find('[data-type="numeric"]:not(.k-input)'), function (i, item) {

                _performance.fn_MakeNumeric($(item));

            });

        },
        MakeTable: function (data) {

            var _containerTable = _performance.container.find('[data-type="' + data.Type + '"] [data-type="TableContainer"]');

            var res = '<table class="table table-bordered table-custom">' +
                _performance.Table.MakeHeaders(data) +
                '<tbody>';

            $.each(data.Products, function (pi, pitem) {

                res += _performance.Table.GetTableLine(data, pitem);

            });

            //  newLine
            res += _performance.Table.GetTableLine(data, null);
            res += '</tbody></table>';

            _containerTable.html(res);

            $.each(_containerTable.find('[data-type="dropdown"]'), function (i, item) {

                _performance.fn_MakeDropdown($(item));

            });

            $.each(_containerTable.find('[data-type="numeric"]'), function (i, item) {

                _performance.fn_MakeNumeric($(item));

            });

            $.each(_containerTable.find('[data-type="IsFocus"]'), function (i, item) {

                _performance.fn_MakeCheckBox($(item));

            });

        },
        UpdateHeaders: function (data) {

            var _containerTable = _performance.container.find('[data-type="' + data.Type + '"] [data-type="TableContainer"]');
            var newHeaders = _performance.Table.MakeHeaders(data).replace('<thead>', '').replace('</thead>', '');

            _containerTable.find('thead').html(newHeaders);

        },
        MakeHeaders: function (data) {

            if (data.Ranges[0] != 0) {
                data.Ranges = $.merge([0], data.Ranges);
            }

            return '<thead>' +
                '<tr>' +
                '<th width="96">' + 'Focus' + '</th>' +
                '<th width="">' + 'Ürün' + '</th>' +
                data.Ranges.map(function (item, i) {
                    return '<th width="100" class="header">' + item + '% - ' + (data.Ranges[i + 1] == undefined ? ' Üzeri' : kendo.toString(data.Ranges[i + 1] - 0.1, 'N1') + '%') + '</th>';
                }).join('') +
                '</thead>' +
                '</tr>';

        },
        GetTableLine: function (data, product) {

            var res = '<tr>';

            //  Yeni Boş Satır
            if (product == null) {
                product = {
                    IsFocus: false,
                    ProductGroupId: null,
                    ProductGroup_Title: null,
                    Values: data.Ranges.map(function (item, i) { return 0; })
                }
            }

            var id = newGuid();

            res += '<td>' +
                '<span class="label label-info label-custom">' +
                '<input id="' + id + '" type="checkbox" data-type="IsFocus" ' + (product != undefined && product.IsFocus == true ? 'checked="checked"' : '') + ' />' +
                '<label for="' + id + '">Focus</label>' +
                '</span>' +
                '</td>';
            res += '<td><select data-type="dropdown" value="' + (product != undefined ? product.id : null) + '"</select></td>';

            $.each(product.Values, function (i, value) {

                res += '<td>' + '<input data-type="numeric" value="' + (value != null ? value : '') + '" />' + '</td>';

            });

            return res + '</tr>';

        }
    },

    container: null,
    MultiplierDatas: null,

    fn_MakeCheckBox: function (item) {

        if ($(item).attr('data-render') == 'true') {
            return;
        }

        $(item).attr('data-render', 'true');

        $(item).on('change', function () {

            var tds = $(this).parents('tr').eq(0).find('td');
            //  0   IsFocus
            //  1   Product
            //  2,3,4,5,... VALUES

            var data = _performance.Data.TableData[$(this).parents('fieldset').eq(0).attr('data-type')];
            var proc = data.Products[$(this).parents('tr').index()];

            if (proc == undefined) {

                data.Products.push({
                    IsFocus: $(this).prop('checked'),
                    ProductGroupId: null,
                    Values: data.Ranges.map(function (item, i) { return 0; }),
                    ProductGroup_Title: null,
                    id: newGuid()
                });

                //  newLine
                $(this).parents('tbody').eq(0).append(_performance.Table.GetTableLine(data, null))

                $.each($('[data-type="numeric"]:not([data-role="numerictextbox"])'), function (i, item) {
                    _performance.fn_MakeNumeric($(item));
                });

                $.each($('[data-type="dropdown"]:not([data-role="multiselect"])'), function (i, item) {
                    _performance.fn_MakeDropdown($(item));
                });

                $.each($('[data-type="IsFocus"]:not([data-render="true"])'), function (i, item) {
                    _performance.fn_MakeCheckBox($(item));
                });

            } else {

                proc.IsFocus = $(this).prop('checked');

            }

        });

    },
    fn_MakeDropdown: function (item) {

        $(item)
            .attr('style', 'width: 100%;')
            .attr('data-custom', 'true')
            .kendoMultiSelect({
                dataValueField: 'id',
                dataTextField: 'name',
                placeholder: "Lütfen Ürün veya Ürün Grubunu Seçiniz..",
                dataSource: {
                    type: "aspnetmvc-ajax",
                    transport: {
                        read: {
                            url: "/PRD/VWPRD_Product/DataSourceDropDown"
                        },
                    },
                    serverFiltering: true,
                    serverPaging: true,
                    pageSize: 50,
                    page: 1,
                    total: 0,
                    serverSorting: true,
                },
                value: $(item).attr('value'),
                filter: 'contains',
                maxSelectedItems: 1,
                template: function (dataItem) {
                    var _currentPoint = kendo.toString(dataItem.currentSellingPoint == null ? 0 : dataItem.currentSellingPoint, 'N0');
                    return '<label class="label label-xs label-inverse">' + _currentPoint + ' Puan</label>' + dataItem.name;
                },
                change: function () {

                    var tds = this.element.parents('tr').eq(0).find('td');
                    //  0   IsFocus
                    //  1   Product
                    //  2,3,4,5,... VALUES

                    var data = _performance.Data.TableData[this.element.parents('fieldset').eq(0).attr('data-type')];
                    var proc = data.Products[this.element.parents('tr').index()];

                    if (proc == undefined) {

                        data.Products.push({
                            IsFocus: false,
                            ProductGroupId: this.value()[0],
                            Values: data.Ranges.map(function (item, i) { return 0; }),
                            ProductGroup_Title: null,
                            id: newGuid()
                        });

                        //  newLine
                        this.element.parents('tbody').eq(0).append(_performance.Table.GetTableLine(data, null))

                        $.each($('[data-type="numeric"]:not([data-role="numerictextbox"])'), function (i, item) {
                            _performance.fn_MakeNumeric($(item));
                        });

                        $.each($('[data-type="dropdown"]:not([data-role="multiselect"])'), function (i, item) {
                            _performance.fn_MakeDropdown($(item));
                        });

                        $.each($('[data-type="IsFocus"]:not([data-render="true"])'), function (i, item) {
                            _performance.fn_MakeCheckBox($(item));
                        });

                    } else {

                        proc.ProductGroupId = this.value()[0];

                    }

                }
            });

    },
    fn_MakeNumeric: function (item) {

        $(item)
            .attr('style', 'width: 100%;')
            .kendoNumericTextBox({
                format: "N0",
                culture: "tr-TR",
                spinners: false,
                decimals: 0,
                max: 1000000,
                change: function () {

                    var tds = this.element.parents('tr').eq(0).find('td');
                    //  0   IsFocus
                    //  1   Product
                    //  2,3,4,5,... VALUES

                    var data = _performance.Data.TableData[this.element.parents('fieldset').eq(0).attr('data-type')];
                    var proc = data.Products[this.element.parents('tr').index()];

                    if (proc == undefined) {

                        data.Products.push({
                            ProductGroupId: this.value()[0],
                            IsFocus: false,
                            ProductGroup_Title: null,
                            id: newGuid(),
                            Values: data.Ranges.map(function (item, i) { return 0; }),
                        });

                        //  newLine
                        this.element.parents('tbody').eq(0).append(_performance.Table.GetTableLine(data, null))

                        $.each($('[data-type="numeric"]:not([data-role="numerictextbox"])'), function (i, item) {
                            _performance.fn_MakeNumeric($(item));
                        });

                        $.each($('[data-type="dropdown"]:not([data-role="multiselect"])'), function (i, item) {
                            _performance.fn_MakeDropdown($(item));
                        });

                        $.each($('[data-type="IsFocus"]:not([data-render="true"])'), function (i, item) {
                            _performance.fn_MakeCheckBox($(item));
                        });

                    } else {

                        proc.Values[this.element.parents('td').index() - 2] = this.value();

                    }

                    //  var procElem = $(tds[1]).find('select[data-type="dropdown"]').data('kendoMultiSelect');

                }
            });

    },
    fn_CategoryName: function (item) {

        if (item.Category_Title == null || item.Category_Title == '') { return ''; }

        var cats = item.Category_Title.split(' > ');

        var res = '';

        $.each(cats, function (cati, catitem) {

            var _cls = '';

            if (cati % 3 == 0) {
                _cls = 'warning';
            } else if (cati % 3 == 1) {
                _cls = 'info';
            } else if (cati % 3 == 2) {
                _cls = 'primary';
            }

            res += '<label class="label-' + _cls + ' label-xs">' + catitem + '</label>';

        });

        return res == '' ? item.Category_Title : res;

    },

    // [0, 1, 2], //  EnumCRM_PerformanceMultiplierTargetGroupType
    TargetGroupTypes: [ 
        { type: 0, Name: 'Genel Performans Çarpanı' }
        //  { type: 1, Name: 'Takım Lideri Performans Çarpanı' },
        //  { type: 2, Name: 'Satış Sorumlusu Performans Çarpanı' }
    ], 

    init: function () {

        this.container = $('[data-target="MultipliersContainer"]');

        _performance.initOthers();

        GetJsonDataFromUrl('/CRM/VWCRM_PerformanceMultiplier/GetMultipliersByGroupId', { groupId: Model.GroupId }, function (res) {

            _performance.MultiplierDatas = $.Enumerable.From(res).Where(function (a) { return a.ProductGroup_Title != null }).ToArray();

            _performance.initOthers();

        });

    },
    initOthers: function () {

        if (_performance.MultiplierDatas == null) {

            return;
        }

        var _enumMultiplierDatas = $.Enumerable.From(_performance.MultiplierDatas);

        _performance.container.html(null);

        $.each(_performance.TargetGroupTypes, function (i, type) {

            _performance.container.append(
                '<fieldset data-type="' + type.type + '">' +
                '<legend>' + type.Name + '</legend>' +
                '<div class="SliderContainer" data-type="SliderContainer">' +

                '<div class="Slider"></div>' +

                '<div class="SliderButtons">' +
                    '<span class="btn btn-xs btn-primary" data-event="AddNewPoint" data-type="' + type.type + '"><i class="fa fa-plus"></i> Ekle</span>' +
                    '<span class="btn btn-xs btn-danger" data-event="RemoveLastPoint" data-type="' + type.type + '"><i class="fa fa-remove"></i> Sil</span>' +
                '</div>' +

                '</div>' +
                '<div class="TableContainer" data-type="TableContainer"></div>' +
                '</fieldset>');

            var _containerSlider = _performance.container.find('[data-type="' + type.type + '"] [data-type="SliderContainer"]');
            var _containerTable = _performance.container.find('[data-type="' + type.type + '"] [data-type="TableContainer"]');

            var ranges = _enumMultiplierDatas
                .Where(a => a.TargetGroupType == type.type)
                .Select(function (a) { return a.MinPercentage; })
                .OrderBy(function (a) { return a; })
                .Distinct()
                .ToArray();

            //  Kayıt Atılmamış ise girebilmesi için boş tablo oluşturuluyor
            if (ranges.length == 0) { ranges = [0, 60, 80]; }

            var products = _enumMultiplierDatas
                .Where(function (a) { return a.TargetGroupType == type.type })
                .GroupBy(function (a) { return a.ProductGroupId; })
                .Select(function (a) {
                    return {
                        ProductGroupId: a.Key(),
                        IsFocus: a.Max(function (b) { return b.IsFocus; }),
                        id: a.Max(function (b) { return b.ProductGroupId; }),
                        ProductGroup_Title: a.Max(function (b) { return b.ProductGroup_Title; }),
                        Values: $.Enumerable.From(a.source).OrderBy(function (b) { return b.MinPercentage; }).Select(function (b) { return b.Point; }).ToArray()
                    };
                })
                .OrderBy(function (a) { return a.IsFocus == false ? 0 : 1; })
                .ThenBy(function (a) { return a.ProductGroup_Title; })
                .ToArray();

            _performance.Data.TableData[type.type] = {
                Type: type.type,
                Ranges: ranges,
                Products: products,

                Slider: null,
            };

            _performance.Slider.MakeSlider(_performance.Data.TableData[type.type]);

            _performance.Table.MakeTable(_performance.Data.TableData[type.type]);

        });

        _performance.container.find('.SliderButtons [data-event="AddNewPoint"]').on('click', function () {
            _performance.Slider.AddNewPoint($(this).attr('data-type'));
        });

        _performance.container.find('.SliderButtons [data-event="RemoveLastPoint"]').on('click', function () {
            _performance.Slider.RemoveLastPoint($(this).attr('data-type'));
        });

    }

};