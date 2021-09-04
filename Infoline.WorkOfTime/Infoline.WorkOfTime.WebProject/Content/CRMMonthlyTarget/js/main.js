var _target = {

    Data: {
        TableData: {},
        GetAllData: function () {

            var res = [];
            
            $.each(_target.Data.TableData, function (di, ditem) {

                $.each(ditem.Products, function (pi, pitem) {

                    if (pitem.ProductGroupId == undefined || pitem.ProductGroupId == []) { return; }

                    $.each(pitem.ProductGroupId, function (pgi, pgitem) {

                        var elem = {

                            ProductGroupId: pgitem,
                            TargetGroupType: parseInt(di),
                            IsFocus: pitem.IsFocus,
                            IsMultipleFocus: pitem.IsMultipleFocus,
                            RowId: pitem.RowId,

                            TargetPoint: pitem.TargetPoint,
                            TargetPercent: pitem.TargetPercent,
                            BonusPercentage: pitem.BonusPercentage,

                        };

                        res.push(elem);

                    });

                });

            });

            return res;

        },
        PostData: function () {

            $('#MonthlyTargets').val(JSON.stringify(_target.Data.GetAllData()));

            return true;

        }
    },

    Table: {
        RemoveLastColumn: function (data) {

            var _containerTable = _target.container.find('[data-type="' + data.Type + '"] [data-type="TableContainer"]');

            $.each(_containerTable.find('tbody tr'), function (i, item) {

                $(item).find('td').eq($(item).find('td').length - 1).remove();

            });

        },
        AddNewColumn: function (data) {

            var _containerTable = _target.container.find('[data-type="' + data.Type + '"] [data-type="TableContainer"]');

            $.each(_containerTable.find('tbody tr'), function (i, item) {

                $(item).append('<td>' + '<input data-type="numeric" value="' + '0' + '" />' + '</td>');

            });

            $.each(_containerTable.find('[data-type="numeric"]:not(.k-input)'), function (i, item) {

                _target.fn_MakeNumeric($(item));

            });

        },
        MakeTable: function (data) {

            var _containerTable = _target.container.find('[data-type="' + data.Type + '"] [data-type="TableContainer"]');

            var res = '<table class="table table-bordered table-custom">' +
                _target.Table.MakeHeaders(data) +
                '<tbody>';

            $.each(data.Products, function (pi, pitem) {

                res += _target.Table.GetTableLine(data, pitem);

            });

            //  newLine
            res += _target.Table.GetTableLine(data, null);
            res += '</tbody></table>';

            _containerTable.html(res);

            $.each(_containerTable.find('[data-type="dropdown"]'), function (i, item) {

                _target.fn_MakeDropdown($(item));
                $(item).trigger('change');

            });

            $.each(_containerTable.find('[data-type="numeric"]'), function (i, item) {

                _target.fn_MakeNumeric($(item));
                $(item).trigger('change');

            });

            $.each(_containerTable.find('[data-type="IsFocus"], [data-type="IsMultipleFocus"]'), function (i, item) {

                _target.fn_MakeCheckBox($(item));

            });

        },
        UpdateHeaders: function (data) {

            var _containerTable = _target.container.find('[data-type="' + data.Type + '"] [data-type="TableContainer"]');
            var newHeaders = _target.Table.MakeHeaders(data).replace('<thead>', '').replace('</thead>', '');

            _containerTable.find('thead').html(newHeaders);

        },
        MakeHeaders: function (data) {

            //if (data.Ranges[0] != 0) {
            //    data.Ranges = $.merge([0], data.Ranges);
            //}

            return '<thead>' +
                '<tr>' +
                '<th width="127">' + 'Toplu Focus' + '</th>' +
                '<th width="96">' + 'Focus' + '</th>' +
                '<th width="">' + 'Ürün' + '</th>' +
                '<th width="150">' + 'Puan' + '</th>' +
                '<th width="200">' + 'Min. Hedef (%)' + '</th>' +
                '<th width="150">' + 'Hediye (%)' + '</th>' +
                '</thead>' +
                '</tr>';

        },
        GetTableLine: function (data, product) {

            var res = '<tr>';

            //  Yeni Boş Satır
            if (product == null) {

                product = {
                    ProductGroupId: [],
                    IsFocus: false,
                    IsMultipleFocus: false,
                    RowId: newGuid(),
                    ProductGroup_Title: null,
                    id: null,

                    TargetPoint: 0,
                    TargetPercent: 0,
                    BonusPercentage: 0
                };

            }

            var id = newGuid();

            res += '<td>' +
                '<span class="label label-warning label-custom label-custom2">' +
                '<input id="' + id + '" type="checkbox" data-type="IsMultipleFocus" ' + (product != undefined && product.IsMultipleFocus == true ? 'checked="checked"' : '') + ' />' +
                '<label for="' + id + '">Toplu Focus</label>' +
                '</span>' +
                '</td>';

            id = newGuid();

            res += '<td>' +
                '<span class="label label-info label-custom">' +
                '<input id="' + id + '" type="checkbox" data-type="IsFocus" ' + (product != undefined && product.IsFocus == true ? 'checked="checked"' : '') + ' />' +
                '<label for="' + id + '">Focus</label>' +
                '</span>' +
                '</td>';

            res += '<td><select data-type="dropdown" autocomplete="off" value="' + (product != undefined ? product.ProductGroupId.join(',') : '') + '"</select></td>';

            res += '<td>' + '<input data-col="TargetPoint" data-type="numeric" value="' + (product != undefined ? product.TargetPoint : '') + '" />' + '</td>';
            res += '<td>' + '<input data-col="TargetPercent" data-type="numeric" max="100" value="' + (product != undefined ? product.TargetPercent : '') + '" />' + '</td>';
            res += '<td>' + '<input data-col="BonusPercentage" data-type="numeric" max="100" value="' + (product != undefined ? product.BonusPercentage : '') + '" />' + '</td>';

            return res + '</tr>';

        }
    },

    container: null,
    TargetDatas: null,

    fn_MakeCheckBox: function (item) {

        if ($(item).attr('data-render') == 'true') {
            return;
        }

        $(item).attr('data-render', 'true');

        $(item).on('change', function () {

            var tds = $(this).parents('tr').eq(0).find('td');

            var data = _target.Data.TableData[$(this).parents('fieldset').eq(0).attr('data-type')];
            var proc = data.Products[$(this).parents('tr').index()];

            if (proc == undefined) {

                var elem = {

                    ProductGroupId: [],
                    ProductGroup_Title: null,
                    IsFocus: false,
                    IsMultipleFocus: false,
                    RowId: newGuid(),
                    //  IsFocus: $(this).prop('checked'),
                    //  IsMultipleFocus: $(this).prop('checked'),
                    id: null,

                    TargetPoint: 0,
                    TargetPercent: 0,
                    BonusPercentage: 0

                };

                elem[$(this).attr('data-type')] = $(this).prop('checked');
                data.Products.push(elem);

                //  newLine
                $(this).parents('tbody').eq(0).append(_target.Table.GetTableLine(data, null))

                $.each($('[data-type="numeric"]:not([data-role="numerictextbox"])'), function (i, item) {
                    _target.fn_MakeNumeric($(item));
                });

                $.each($('[data-type="dropdown"]:not([data-role="multiselect"])'), function (i, item) {
                    _target.fn_MakeDropdown($(item));
                });

                $.each($('[data-type="IsFocus"]:not([data-render="true"]), [data-type="IsMultipleFocus"]:not([data-render="true"])'), function (i, item) {
                    _target.fn_MakeCheckBox($(item));
                });

            } else {

                proc[$(this).attr('data-type')] = $(this).prop('checked');

            }


            //  Is Multiple Func
            if ($(this).attr('data-type') == 'IsMultipleFocus') {

                var elem = $(this).parents('tr').eq(0).find('[data-type="dropdown"]').data('kendoMultiSelect');

                if ($(this).prop('checked')) {

                    elem.setOptions({
                        maxSelectedItems: 9999,
                        autoClose: false
                    });

                    $(this).parents('tr').eq(0).find('[data-type="IsFocus"]').prop('checked', true).trigger('change');

                } else {

                    elem.setOptions({
                        maxSelectedItems: 1,
                        autoClose: true
                    });

                    if (elem.value().length > 1) {
                        elem.value([elem.value()[0]]);
                    }

                }

            } else if ($(this).attr('data-type') == 'IsFocus' && !$(this).prop('checked') ) {

                $(this).parents('tr').eq(0).find('[data-type="IsMultipleFocus"]').prop('checked', false).trigger('change');

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
                filter: 'contains',
                value: $(item).attr('value').split(','),
                maxSelectedItems: 1,
                template: function (dataItem) {
                    var _currentPoint = kendo.toString(dataItem.currentSellingPoint == null ? 0 : dataItem.currentSellingPoint, 'N0');
                    return '<label class="label label-xs label-inverse">' + _currentPoint + ' Puan</label>' + dataItem.name;
                },
                change: function () {

                    var data = _target.Data.TableData[this.element.parents('fieldset').eq(0).attr('data-type')];
                    var proc = data.Products[this.element.parents('tr').index()];

                    if (proc == undefined) {

                        data.Products.push({

                            ProductGroupId: this.value(),
                            ProductGroup_Title: null,
                            IsFocus: false,
                            IsMultipleFocus: false,
                            RowId: newGuid(),
                            id: null,
                            TargetPoint: 0,
                            TargetPercent: 0,
                            BonusPercentage: 0

                        });

                        //  newLine
                        this.element.parents('tbody').eq(0).append(_target.Table.GetTableLine(data, null))

                        $.each($('[data-type="numeric"]:not([data-role="numerictextbox"])'), function (i, item) {
                            _target.fn_MakeNumeric($(item));
                        });

                        $.each($('[data-type="dropdown"]:not([data-role="multiselect"])'), function (i, item) {
                            _target.fn_MakeDropdown($(item));
                        });

                        $.each($('[data-type="IsFocus"]:not([data-render="true"]), [data-type="IsMultipleFocus"]:not([data-render="true"])'), function (i, item) {
                            _target.fn_MakeCheckBox($(item));
                        });

                    } else {
                        proc.ProductGroupId = this.value();
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

                    var data = _target.Data.TableData[this.element.parents('fieldset').eq(0).attr('data-type')];
                    var proc = data.Products[this.element.parents('tr').index()];

                    if (proc == undefined) {

                        data.Products.push({

                            ProductGroupId: [],
                            ProductGroup_Title: null,
                            IsFocus: false,
                            IsMultipleFocus: false,
                            RowId: newGuid(),

                            id: null,

                            TargetPoint: 0,
                            TargetPercent: 0,
                            BonusPercentage: 0

                        });

                        //  newLine
                        this.element.parents('tbody').eq(0).append(_target.Table.GetTableLine(data, null))

                        $.each($('[data-type="numeric"]:not([data-role="numerictextbox"])'), function (i, item) {
                            _target.fn_MakeNumeric($(item));
                        });

                        $.each($('[data-type="dropdown"]:not([data-role="multiselect"])'), function (i, item) {
                            _target.fn_MakeDropdown($(item));
                        });

                        $.each($('[data-type="IsFocus"]:not([data-render="true"]), [data-type="IsMultipleFocus"]:not([data-render="true"])'), function (i, item) {
                            _target.fn_MakeCheckBox($(item));
                        });

                    } else {

                        proc[this.element.attr('data-col')] = this.value();

                    }

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

    TargetGroupTypes: [
        { type: 0, Name: 'Genel Performans Hedef Tablosu' },
    ],

    init: function () {

        this.container = $('[data-target="TargetsContainer"]');

        _target.initOthers();

        GetJsonDataFromUrl('/CRM/VWCRM_MonthlyTarget/GetTargetsByGroupId', { groupId: Model.GroupId }, function (res) {

            //  _target.TargetDatas = $.Enumerable.From(res).Where(function (a) { return a.ProductGroup_Title != null }).ToArray();

            _target.TargetDatas = $.Enumerable.From(res)
                .Where(function (a) { return a.ProductGroup_Title != null; })
                .GroupBy(function (a) { return a.RowId; })
                .Select(function (a) {
                    return {
                        ProductGroupId: a.source.map(function (item, i) { return item.ProductGroupId; }),
                        ProductGroup_Title: 'Array',

                        RowId: a.Key(),
                        IsMultipleFocus: a.Max(function (b) { return b.IsMultipleFocus; }),
                        BonusPercentage: a.Max(function (b) { return b.BonusPercentage; }),
                        CPeriod: a.Max(function (b) { return b.CPeriod; }),
                        GroupId: a.Max(function (b) { return b.GroupId; }),
                        IsFocus: a.Max(function (b) { return b.IsFocus; }),
                        TargetGroupType: a.Max(function (b) { return b.TargetGroupType; }),
                        TargetPercent: a.Max(function (b) { return b.TargetPercent; }),
                        TargetPoint: a.Max(function (b) { return b.TargetPoint; }),
                        id: a.Max(function (b) { return b.id; })
                    }
                }).ToArray();

            _target.initOthers();

        });

    },
    initOthers: function () {

        if (_target.TargetDatas == null) {
            return;
        }

        var _enumTargetDatas = $.Enumerable.From(_target.TargetDatas);

        _target.container.html(null);

        $.each(_target.TargetGroupTypes, function (i, type) {

            _target.container.append(
                '<fieldset data-type="' + type.type + '">' +

                '<legend>' + type.Name + '</legend>' +

                '<div class="TableContainer" data-type="TableContainer"></div>' +

                '</fieldset>');

            var _containerTable = _target.container.find('[data-type="' + type.type + '"] [data-type="TableContainer"]');

            var products = _enumTargetDatas
                .Where(function (a) { return a.TargetGroupType == type.type })
                .Select(function (a) {
                    return {
                        ProductGroupId: a.ProductGroupId,
                        IsFocus: a.IsFocus,
                        IsMultipleFocus: a.IsMultipleFocus,
                        id: a.ProductGroupId,
                        ProductGroup_Title: a.ProductGroup_Title,
                        RowId: a.RowId,

                        TargetPoint: a.TargetPoint,
                        TargetPercent: a.TargetPercent,
                        BonusPercentage: a.BonusPercentage
                    };
                })
                .OrderBy(function (a) { return (a.IsFocus == true || a.IsMultipleFocus == true) ? 1 : 0; })
                .ThenBy(function (a) { return a.ProductGroup_Title; })
                .ToArray();

            _target.Data.TableData[type.type] = {
                Type: type.type,
                Products: products
            };

            _target.Table.MakeTable(_target.Data.TableData[type.type]);

        });

    }

};