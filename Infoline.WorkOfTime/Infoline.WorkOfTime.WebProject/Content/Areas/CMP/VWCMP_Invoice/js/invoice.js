var _invoice = {
    loadCount: 0,
    beforeCurrencyCode: "TRY",
    afterCurrencyCode: "TRY",
    TotalProductPrice: 0,
    SelectedProductDropDown: null,
    Products: null,
    PageLoad: false,
    LoadControl: {
        tevkifat: false,
        stopaj: false,
        items: false,
        loaded: false,
        itemLength: 0,
    },
    EnumDirections: $.ajax({
        url: "/General/GetInvoiceDirectionEnums",
        type: "POST",
        dataType: "JSON",
        async: false
    }).responseJSON,
    EnumDiscountType: $.ajax({
        url: "/General/GetInvoiceDiscountTypeEnums",
        type: "POST",
        dataType: "JSON",
        async: false
    }).responseJSON,
    EnumPaymentType: $.ajax({
        url: "/General/GetInvoicePaymentTypeEnums",
        type: "POST",
        dataType: "JSON",
        async: false
    }).responseJSON,
    EnumInvoiceType: $.ajax({
        url: "/General/GetInvoiceTypeEnums",
        type: "POST",
        dataType: "JSON",
        async: false
    }).responseJSON,
    EnumInvoiceStatus: $.ajax({
        url: "/General/GetInvoiceStatusEnums",
        type: "POST",
        dataType: "JSON",
        async: false
    }).responseJSON,
    ExchangeRates: $.ajax({
        url: '/General/GetExchangeRatesByDate',
        data: { date: new Date().toLocaleString() },
        type: "POST",
        dataType: "JSON",
        async: false
    }).responseJSON,
    Events: {
        showHide: function (_this) {
            var type = $(_this).attr("data-type");
            var buttons = $(_this).parent().find('[data-type="' + type + '"]');

            $.each(buttons, function (i, item) {
                if ($(item).hasClass("hide")) {
                    $(item).removeClass("hide");
                } else {
                    $(item).addClass("hide");
                }
            })

            if ($('#' + type).hasClass("hide")) {
                $('#' + type).removeClass("hide");
            } else {
                $('#' + type).addClass("hide");
            }
        },
        showTextEditor: function (_this) {
            var type = $(_this).attr("data-type");

            if (type == "conditions") {
                $('#descriptionTextEditor').addClass("hide");
                $('#conditionsTextEditor').removeClass("hide");

                $('[data-type="description"][data-button="hide"]').addClass("hide");
                $('[data-type="description"][data-button="show"]').removeClass("hide");

                $('#conditions').trigger("focus");
            } else {
                $('#conditionsTextEditor').addClass("hide");
                $('#descriptionTextEditor').removeClass("hide");

                $('[data-type="conditions"][data-button="hide"]').addClass("hide");
                $('[data-type="conditions"][data-button="show"]').removeClass("hide");

                $('#description').focus();
            }

            $(_this).addClass("hide");
            $(_this).parent().find('[data-button="hide"]').removeClass("hide");
        },
        hideTextEditor: function (_this) {
            var type = $(_this).attr("data-type");

            if (type == "conditions") {
                $('#conditionsTextEditor').addClass("hide");
            } else {
                $('#descriptionTextEditor').addClass("hide");
            }

            $(_this).addClass("hide");
            $(_this).parent().find('[data-button="show"]').removeClass("hide");
        },
        onDataBoundMyCompany: function (e) {
            var value = e.sender.value();
            if (value && value != null && value != "") {
                e.sender.trigger("change");
            }
        },
        onDataBoundTevkifat: function (e) {
            _invoice.LoadControl.tevkifat = true;

            if (_invoice.LoadControl.stopaj == true && _invoice.LoadControl.loaded == false && _model.InvoiceItems && _invoice.LoadControl.itemLength == _model.InvoiceItems.length) {
                _invoice.LoadProducts();
            }
        },
        onDataBoundStopaj: function (e) {
            _invoice.LoadControl.stopaj = true;

            if (_invoice.LoadControl.tevkifat == true && _invoice.LoadControl.loaded == false && _model.InvoiceItems && _invoice.LoadControl.itemLength == _model.InvoiceItems.length) {
                _invoice.LoadProducts();
            }
        },
        onChangeStorage: function (e) {
            var item = this.dataItem();
            $('#customerAdress').val(item.locationId_Title);
        },
        onDataBoundCompany: function (e) {
            if (_model.customerId && _model.customerId != null) {
                this.value(_model.customerId);
                this.trigger("change");
            }
        },
        onChange: function (e) {

            var companyId = e.sender.value();
            var container = $(e.sender.element).parents('[data-content="buysell"]');
            var content = $(container).find('[data-id="detail"]');
            var storage = $(container).find('[data-id="customerStorage"]');

            var selection = "supplier";

            if (e.sender.element.attr("id").includes("customer")) {
                selection = "customer";
            }

            $(storage).addClass("hide");
            $(content).addClass("hide");

            if (companyId && companyId != null && companyId != "") {
                GetJsonDataFromUrl('/General/GetVWCMP_CompanyByCompanyId', { id: companyId }, function (res) {

                    var customerStorage = $('#storageId').data("kendoDropDownList");

                    if (customerStorage && selection == "customer") {
                        $(storage).removeClass("hide");
                    }
                    else {
                        $(content).removeClass("hide");
                    }

                    if (res == null) {
                        return false;
                    }

                    var address = "";

                    if (res.invoiceAddress != null) {
                        address = res.invoiceAddress.length > 50 ? res.invoiceAddress.slice(0, 50) + "..." : res.invoiceAddress;
                        $(content).find('[data-invoice="address"]').attr("title", res.invoiceAddress);
                    }

                    $(content).find('[data-invoice="address"]').text(address);

                    var taxOffice = res.taxOffice != null ? res.taxOffice : "-";
                    var taxNumber = res.taxNumber != null ? res.taxNumber : "-";

                    $(content).find('[data-invoice="taxOfficeNumber"]').text(taxOffice + " / " + taxNumber);
                    $(content).find('[data-invoice="companyId"]').attr("data-href", "/CMP/VWCMP_Company/Update?id=" + res.id);

                    if (selection == "supplier") {
                        $('#supplierAdress').val(res.invoiceAddress);
                        $('#supplierTaxNumber').val(res.taxNumber);
                        $('#supplierTaxOffice').val(res.taxOffice);
                        $('#supplierTitle').val(res.name);
                    }
                    else {
                        $('#customerTaxOffice').val(res.taxOffice);
                        $('#customerAdress').val(res.invoiceAddress);
                        $('#customerTaxNumber').val(res.taxNumber);
                        $('#customerTitle').val(res.name);
                    }
                });
            }

            else {
                $(content).addClass("hide");
            }
        },
        addStopaj: function (elem) {
            var rate = $(elem).attr("data-value");
            var dropdown = $('#StopajRate').data("kendoDropDownList");
            var items = dropdown.dataItems();
            var rateValue = rate + "%";
            var item = $.Enumerable.From(items).Where(a => a.enumDescription == rateValue).FirstOrDefault();
            dropdown.value(item.Id);
            dropdown.trigger("change");

            $('#stopajRow').removeClass("hide");
        },
        removeStopaj: function (elem) {
            var dropdown = $('#StopajRate').data("kendoDropDownList");
            var items = dropdown.dataItems();
            var item = $.Enumerable.From(items).Where(a => a.enumDescription == "0%").FirstOrDefault();
            dropdown.value(item.Id);
            dropdown.trigger("change");
            $('#stopajRow').addClass("hide");
        },
        addTevkifat: function (elem) {
            var rate = $(elem).attr("data-value");
            var dropdown = $('#TevkifatRate').data("kendoDropDownList");
            var items = dropdown.dataItems();
            var rateValue = rate + "/10";
            var item = $.Enumerable.From(items).Where(a => a.enumDescription == rateValue).FirstOrDefault();
            dropdown.value(item.Id);
            dropdown.trigger("change");

            $('#tevkifatRow').removeClass("hide");
        },
        removeTevkifat: function (elem) {
            var dropdown = $('#TevkifatRate').data("kendoDropDownList");
            var items = dropdown.dataItems();
            var item = $.Enumerable.From(items).Where(a => a.enumDescription == "0/10").FirstOrDefault();
            dropdown.value(item.Id);
            dropdown.trigger("change");
            $('#tevkifatRow').addClass("hide");
        },
        addDiscountSubTotalAmount: function (e) {
            $('#subTotalDiscountDiv').removeClass("hide");
            $('#subTotalDiscount').val(0);
        },
        DeleteDiscountSubTotalAmount: function (e) {
            $('#subTotalDiscountDiv').addClass("hide");
            $('#subTotalDiscount').val(0);
            _invoice.CalculateProduct($(this));
        },
        additionalButton: function (_this) {
            var type = $(_this).attr('data-name');
            var rowId = $(_this).parents(".table-row").attr("data-row");
            $('[data-id="' + rowId + '"]').find('[data-content="' + type + '"]').removeClass("hide");
        },
        changeCurrency: function (e) {
            _invoice.beforeCurrencyCode = _invoice.afterCurrencyCode;
            _invoice.afterCurrencyCode = e.sender.dataItem().code;
            if (_invoice.afterCurrencyCode != "TL") {
                $('#liveExchange').removeClass("hide");
            } else {
                $('#liveExchange').addClass("hide");
                _invoice.afterCurrencyCode = "TRY";
            }

            var rate = _invoice.ExchangeRates[_invoice.afterCurrencyCode].BanknoteSelling;
            $("#rateExchange").data("kendoNumericTextBox").value(rate);

            $.each($('[data-item="productId"]'), function (i, item) {
                $(item).attr("data-initial", "true");
                _invoice.SetPriceToProduct($(item));
            });

            _invoice.CalculateProduct($(e.sender.element));
        },
        onDataBoundCurrency: function (e) {
            var dropdown = e.sender;
            var value = dropdown.value();
            if (!value || value == "" || value == null) {
                var lira = dropdown.dataSource.data().find(a => a.code == "TL");
                dropdown.value(lira.id);
            }

            dropdown.trigger("change");

            if (_invoice.PageLoad == false) {
                _invoice.PageLoad = true;
                _invoice.Init();
            }
        },
        onDataBoundKDV: function (e) {
            var dropdown = e.sender;
            _invoice.LoadControl.itemLength++;
            if (_model.InvoiceItems && _model.InvoiceItems.length != 0 && _invoice.LoadControl.loaded == false) {
                if (_invoice.LoadControl.itemLength == _model.InvoiceItems.length && _invoice.LoadControl.tevkifat == true && _invoice.LoadControl.stopaj) {
                    setTimeout(function () {
                        _invoice.LoadProducts();
                    }, 2500)
                }
            }
            else {
                var select = dropdown.dataItems().find(a => a.enumDescription == "8%");
                dropdown.value(select.Id)
            }
        },
        changeKDVRate: function (e) {
            var dropdown = e.sender;
            $(dropdown.element).parents("td").find('[data-item="KDV"]').val(dropdown.dataItem().enumDescription.replace("%", ""));
            _invoice.CalculateProduct(dropdown.element);
        },
        changeStopaj: function (e) {
            _invoice.CalculateProduct();
        },
        changeTevkifat: function (e) {
            _invoice.CalculateProduct();
        },
        changeProduct: function (e) {
            var dropdown = e.sender;
            var priceElem = $(e.sender.element).parents(".table-row").find('[data-item="price"]').data("kendoNumericTextBox");

            if (priceElem) {
                if (priceElem.value() > 0) {

                } else {
                    priceElem.value(0);
                }
            }

            _invoice.SetPriceToProduct(e.sender.element);

            $(e.sender.element).parents(".table-row").find('[data-label="unit"]').text(dropdown.dataItem().unitId_Title);
            $(e.sender.element).parents(".table-row").find('[data-item="unitId"]').val(dropdown.dataItem().unitId);

            var quantityTextBox = $(e.sender.element).parents(".table-row").find('[data-item="quantity"]').data("kendoNumericTextBox");

            if (dropdown.dataItem().unitId_Title == "ADET" || dropdown.dataItem().unitId_Title == "KUTU" || dropdown.dataItem().unitId_Title == "PAKET" || dropdown.dataItem().unitId_Title == "KOLİ" || dropdown.dataItem().unitId_Title == "KİŞİ") {
                quantityTextBox.step(1);
                quantityTextBox.setOptions({ format: "N0" });
                quantityTextBox.element.trigger("focusout");
            }
            else {
                quantityTextBox.step(0.01);
                quantityTextBox.setOptions({ format: "N2" });
                quantityTextBox.element.trigger("focusout");
            }
        },
    },
    GetCrossExchange: function (from, to) {
        return parseFloat(_invoice.ExchangeRates[from].BanknoteSelling) / parseFloat(_invoice.ExchangeRates[to].BanknoteSelling);
    },
    SetPriceToProduct: function (element) {
        var dropdown = $(element).data("kendoDropDownList");
        var priceElem = $(element).parents('.table-row').find('[data-item="price"]').data("kendoNumericTextBox");
        var unitPriceElem = $(element).parents('.table-row').find('[data-label="unitPrice"]');
        var currencyLabelElem = $(element).parents('.table-row').find('[data-cur="currencyUnit"]');

        if (!dropdown) { return false; }

        var productDataItem = dropdown.dataItem();

        unitPriceElem.text("");
        currencyLabelElem.text("");
        if (productDataItem.currentSellingCurrencyId_Title) {
            var unitPrice = productDataItem.currentSellingPrice ? productDataItem.currentSellingPrice : 0;
            unitPriceElem.text(kendo.toString(unitPrice, "N2"));
            currencyLabelElem.text(productDataItem.currentSellingCurrencyId_Title);
        }

        if (productDataItem.id && productDataItem.id != null && productDataItem.id != "") {

            var exchange = _invoice.GetCrossExchange(_invoice.afterCurrencyCode, _invoice.beforeCurrencyCode);

            if (priceElem.value() && priceElem.value() > 0) {
                // priceElem.value(priceElem.value() / exchange);
                priceElem.value(priceElem.value());
            } else {
                priceElem.value(productDataItem.currentSellingPrice / exchange);
            }

            if (element.attr("data-initial") && element.attr("data-initial") == "true") {
                var invoiceItem = $.Enumerable.From(_model.InvoiceItems).Where(a => a.productId == dropdown.value()).FirstOrDefault();
                if (invoiceItem && invoiceItem.price) {
                    priceElem.value(invoiceItem.price);
                }
                element.removeAttr("data-initial");
            }
        } else {
            priceElem.value(0);
        }

        _invoice.CalculateProduct();
    },
    AddRowForProduct: function (control) {


        $("#save").attr("disabled","")
        if (control == true) {
            var row = $('[data-row]').first();
            var product = $(row).find('[data-item=productId]').data("kendoDropDownList");

            if (product && (!product.value() || product.value() == "" || product.value() == null)) {
                MesajWarning("Lütfen önce ürün seçiniz.");
                return false;
            }
        }

        var template = _invoice.GetRowTemplate();
        var length = $('#productsTable tbody tr:not(.table-row-2lines)').length / 2;

        var ids = [];
        $(template).find("input").each(function (e) {
            var id = $(this).attr("id");
            if (id !== undefined) {
                ids.push(id);
                $(this).attr("id", id + length);
            }
        });

        $(template).find("script").each(function () {
            var _this = $(this);
            var script = _this.html();
            ids.forEach(function (id) {
                if (script.includes('jQuery("#' + id + '")')) {
                    script = script.split('#' + id).join('#' + id + length);
                }
            });
            _this.after($('<script/>').append(script));
            _this.remove();
        });


        var dataItems = $(template).find('[data-item]');

        $.each(dataItems, function (i, item) {
            var itemName = $(item).attr("data-item");
            $(item).attr("name", "InvoiceItems[" + length + "]." + itemName);
            if (item.type == "number") {
                $(item).attr("id", newGuid());
            }
        })

        var rowId = newGuid();
        $(template).find('[data-row="InvoiceItems"]').removeAttr("data-row").attr("data-row", rowId);
        $(template).find('[data-id="InvoiceItems"]').removeAttr("data-id").attr("data-id", rowId);

        $(template).find('[data-item="itemOrder"]').val(length + 1);

        $('#productsTable').find("tbody").prepend($(template).html());

        $.each($('[type="number"]'), function (i, item) {
            var id = $(item).attr("id");

            if ($(item).attr("name") != undefined && $(item).attr("name").indexOf("price") > -1) {
                $('#' + id).kendoNumericTextBox({
                    format: "N2",
                    min: 0,
                    value: 0,
                    step: 0.01,
                    decimals: 2,
                    spinners: false
                });
            } else {
                $('#' + id).kendoNumericTextBox({
                    format: "N2",
                    min: 0,
                    value: 0,
                    step: 0.01,
                    spinners: false
                });

            }
            if ($(item).attr("data-item") == "OIV" || $(item).attr("id") == "subTotalDiscount") {
                $('#' + id).data("kendoNumericTextBox").max(100);
            }
        })


    },
    GetRowTemplate: function () {
        var templateRow = $('#rowTemplate');
        var field = $(templateRow)[0].content.querySelector("tbody");
        var importNode = document.importNode(field, true);
        return $(importNode);
    },
    RowDelete: function (_this) {
        var row = $(_this).parents(".table-row");
        var rowId = $(row).attr("data-row");

        $('[data-id="' + rowId + '"]').remove();
        $(row).remove();

        var length = $('[data-row]').length;

        $.each($('[data-row]'), function (i, mainRow) {
            var subRow = $('[data-id="' + $(mainRow).attr("data-row") + '"]');
            var names = $(mainRow).find('[name]');
            var subRowNames = $(subRow).find('[name]');

            $.each($(names), function (j, elem) {
                var name = $(elem).attr("name");
                var field = name.split('.')[1];

                $(elem).attr("name", 'InvoiceItems[' + i + '].' + field);

                if (field == "itemOrder") {
                    $(elem).val(length - i);
                }
            });

            $.each($(subRowNames), function (j, elem) {
                var name = $(elem).attr("name");
                var field = name.split('.')[1];

                $(elem).attr("name", 'InvoiceItems[' + i + '].' + field);
            })
        })

        _invoice.CalculateProduct(_this);
    },
    ClearAdditionalInfo: function (_this) {
        $(_this).parents("td").addClass("hide");
        var elem = $(_this).parents("td").find('[data-item]');
        var numeric = $(elem).data("kendoNumericTextBox");

        numeric ? numeric.value(0) : $(elem).val("");

        _invoice.CalculateProduct(_this);
    },
    WriteTextByDataResult: function (amount, dataResult, type) {

        var _elem = $('[data-result="' + dataResult + '"]');
        $(_elem).empty();

        var price = kendo.toString(amount, "N2") || '0';
        var splittedPrice = price.split(",");

        if (!splittedPrice[1]) {
            splittedPrice[1] = "00";
        }

        var dropdownItem = $('#currencyId').data("kendoDropDownList").dataItem();
        var symbolCurrency = dropdownItem.symbol ? dropdownItem.symbol : "₺";

        if (dataResult == "TLequality") {
            symbolCurrency = "₺";
        }

        var text = '<h3 class="font-bold text-right">' + type + " " + splittedPrice[0] + ',<small>' + splittedPrice[1] + '</small> ' + symbolCurrency + '</h3>';

        if (dataResult == "totalAmount") {
            text = '<h2 class="font-bold text-right" style="color:#ff6a00;">' + type + " " + splittedPrice[0] + ',<small style="color:#ff6a00;">' + splittedPrice[1] + '</small> ' + symbolCurrency + '</h2>';
        }

        $(_elem).append(text);

    },
    GetValue: function (row, item) {
        var value = 0;
        var kendoNumeric = $(row).find('[data-item="' + item + '"]').data("kendoNumericTextBox");

        if (kendoNumeric && !isNaN(kendoNumeric.value()) && kendoNumeric.value() != null) {
            value = kendoNumeric.value();
        }
        return value;
    },
    GetDiscountAmountByTL: function (row, item, totalPrice) {
        var discountType = $(row).find('[data-item="discountType"]').val();
        var discount = _invoice.GetValue($(row), item);

        var discountTL = 0;
        if (discount != null) {
            if (discountType == _invoice.EnumDiscountType.find(a => a.EnumKey == "Yuzde").Key) {
                discountTL = (totalPrice / 100) * discount;
            }
            if (discountType == _invoice.EnumDiscountType.find(a => a.EnumKey == "Tutar").Key) {
                discountTL = discount;
            }
        }

        return discountTL;
    },
    GetSubTotalDiscountByTL: function (totalPriceProducts) {
        var subTotalDiscountTL = 0;

        var subTotalDiscount = $('#subTotalDiscount').val();
        var subTotalDiscountType = $('#subTotalDiscountType').val();

        if (subTotalDiscountType == _invoice.EnumDiscountType.find(a => a.EnumKey == "Yuzde").Key) {
            subTotalDiscountTL = (totalPriceProducts / 100) * parseFloat(subTotalDiscount.replace(",", "."));
        }
        if (subTotalDiscountType == _invoice.EnumDiscountType.find(a => a.EnumKey == "Tutar").Key) {
            subTotalDiscountTL = subTotalDiscount;
        }

        if (isNaN(parseFloat(subTotalDiscountTL))) {
            subTotalDiscountTL = 0;
        }
        return subTotalDiscountTL;
    },
    GetOTVByTL: function (row, priceProduct) {
        var otvTL = 0;
        var otvType = $(row).find('[data-item="OTVType"]').val();
        var otvElem = $(row).find('[data-item="OTV"]').data("kendoNumericTextBox");

        if (otvElem && !isNaN(otvElem.value()) && otvElem.value() != null) {
            if (otvType == _invoice.EnumDiscountType.find(a => a.EnumKey == "Yuzde").Key) {
                otvTL = (priceProduct / 100) * otvElem.value();
            }
            if (otvType == _invoice.EnumDiscountType.find(a => a.EnumKey == "Tutar").Key) {
                otvTL = otvElem.value();
            }
        }

        return otvTL;
    },
    GetOIVByTL: function (row, totalWithOTV, item) {
        var oivTL = 0;
        var oivElem = $(row).find('[data-item="' + item + '"]').data("kendoNumericTextBox");

        if (oivElem && !isNaN(oivElem.value()) && oivElem.value() != null) {
            oivTL = (totalWithOTV / 100) * oivElem.value();
        }

        return oivTL;
    },
    GetKDVByTL: function (row, totalWithOTV, item) {
        var kdvTL = 0;
        var kdvRate = $(row).find('[data-item="' + item + '"]').val();

        if (kdvRate && kdvRate != null && kdvRate != "") {
            kdvTL = (totalWithOTV / 100) * parseFloat(kdvRate);
        }

        if (isNaN(kdvTL)) {
            return 0;
        }

        return kdvTL;
    },
    GetStopajByTL: function (totalSubAmount) {
        var stopaj = 0;
        var stopajRate = $('#StopajRate').data("kendoDropDownList");

        if (stopajRate.dataItem() && stopajRate.dataItem().enumDescription && totalSubAmount != 0) {
            var rate = stopajRate.dataItem().enumDescription.split('%')[0];
            stopaj = (totalSubAmount * rate) / 100;
        }

        if (isNaN(stopaj)) {
            stopaj = 0;
        }

        return stopaj;
    },
    GetTevkifatByTL: function (kdvTotal) {
        var tevkifat = 0;
        var tevkifatDropDown = $('#TevkifatRate').data("kendoDropDownList");

        if (tevkifatDropDown.dataItem() && tevkifatDropDown.dataItem().enumDescription) {
            var tevkifatRate = tevkifatDropDown.dataItem().enumDescription.split('/')[0];
            tevkifat = (kdvTotal * parseInt(tevkifatRate)) / 10;
            $('#tevkifat').val(tevkifatRate);
        }

        if (isNaN(tevkifat)) {
            tevkifat = 0;
        }

        return tevkifat;
    },
    CalculateProduct: function (_this) {
        _invoice.TotalProductPrice = 0;

        $.each($('[data-row]'), function (i, mainRow) {
            var rowId = $(mainRow).attr("data-row");
            var subRow = $('[data-id="' + rowId + '"]');

            var quantity = _invoice.GetValue($(mainRow), "quantity");
            var price = _invoice.GetValue($(mainRow), "price");
            var totalPrice = price * quantity;
            var discountTL = _invoice.GetDiscountAmountByTL($(subRow), "discount", totalPrice);
            var totalAmount = totalPrice - discountTL;
            _invoice.TotalProductPrice += totalAmount;

            $(subRow).find('[data-name="discountTL"]').val(discountTL);
            $(mainRow).find('[data-name="totalPrice"]').val(totalAmount);
        });

        var productObj = [];
        var subTotalDiscountTL = _invoice.GetSubTotalDiscountByTL(_invoice.TotalProductPrice);

        var percentDiscount = 0;
        if (_invoice.TotalProductPrice != 0) {
            percentDiscount = ((parseFloat(subTotalDiscountTL) * 100) / _invoice.TotalProductPrice);
        }

        $('#subTotalDiscountPercent').val(percentDiscount);

        $.each($('[data-row]'), function (i, mainRow) {
            var rowId = $(mainRow).attr("data-row");
            var subRow = $('[data-id="' + rowId + '"]');

            var productId = $(mainRow).find('[data-item="productId"]').data("kendoDropDownList").value();
            var quantity = _invoice.GetValue($(mainRow), "quantity");
            var price = _invoice.GetValue($(mainRow), "price");
            var totalPrice = price * quantity;
            var discountTL = _invoice.GetDiscountAmountByTL($(subRow), "discount", totalPrice);

            var totalAmount = totalPrice - discountTL;

            var subPrice = 0;

            if (_invoice.TotalProductPrice != 0) {
                if (subTotalDiscountTL.toString().indexOf(",") > -1) {
                    subPrice = totalAmount - ((totalAmount / _invoice.TotalProductPrice) * parseFloat(subTotalDiscountTL.replace(",", ".")));
                } else {
                    subPrice = totalAmount - ((totalAmount / _invoice.TotalProductPrice) * parseFloat(subTotalDiscountTL));
                }
            }

            var otv = _invoice.GetOTVByTL($(subRow), subPrice);
            var priceWithOTV = subPrice + otv;

            var kdv = _invoice.GetKDVByTL($(mainRow), priceWithOTV, 'KDV');
            var oiv = _invoice.GetOIVByTL($(subRow), priceWithOTV, 'OIV');
            var total = priceWithOTV + kdv + oiv;

            productObj.push({
                product: productId,
                discount: discountTL,
                subPrice: totalPrice,
                subTotal: subPrice,
                otv: otv,
                kdv: kdv,
                oiv: oiv,
                total: total
            });

            var otvProduct = _invoice.GetOTVByTL($(subRow), totalAmount);
            var priceWithOTVProduct = totalAmount + otvProduct;

            var kdvProduct = _invoice.GetKDVByTL($(mainRow), priceWithOTVProduct, 'KDV');
            var oivProduct = _invoice.GetOIVByTL($(subRow), priceWithOTVProduct, 'OIV');
            var totalProduct = priceWithOTVProduct + kdvProduct + oivProduct;

            $(mainRow).find('[data-label="totalAmount"]').text(kendo.toString(totalProduct, "N2"));
        });

        var subTotalProduct = $.Enumerable.From(productObj).Sum(a => a.subPrice);
        var totalDiscount = $.Enumerable.From(productObj).Sum(a => a.discount);

        $('#subTotal').val(subTotalProduct);
        $('#productDiscounts').val(totalDiscount);

        var totalSubAmount = $.Enumerable.From(productObj).Sum(a => a.subTotal);

        $('#allTaxRow').removeClass("hide");

        $('#totalOTVDiv').removeClass("hide");
        $('#totalOIVDiv').removeClass("hide");
        $('#totalKDVDiv').removeClass("hide");

        $('#productDiscountsDiv').removeClass("hide");

        if ($.Enumerable.From(productObj).Where(a => a.otv > 0).Count() == 0) {
            $('#totalOTVDiv').addClass("hide");
        }

        if ($.Enumerable.From(productObj).Where(a => a.oiv > 0).Count() == 0) {
            $('#totalOIVDiv').addClass("hide");
        }

        if ($.Enumerable.From(productObj).Where(a => a.kdv > 0).Count() == 0) {
            $('#totalKDVDiv').addClass("hide");
        }

        if ($.Enumerable.From(productObj).Where(a => a.kdv > 0 || a.otv > 0 || a.oiv > 0).Count() == 0) {
            $('#allTaxRow').addClass("hide");
        }

        if (totalDiscount == 0) {
            $('#productDiscountsDiv').addClass("hide");
        }

        var totalAmount = $.Enumerable.From(productObj).Sum(a => a.total);

        var kdvTotal = $.Enumerable.From(productObj).Sum(a => a.kdv);
        var otvTotal = $.Enumerable.From(productObj).Sum(a => a.otv);
        var oivTotal = $.Enumerable.From(productObj).Sum(a => a.oiv);

        var stopaj = _invoice.GetStopajByTL(totalSubAmount);
        totalAmount = totalAmount - stopaj;
        var tevkifat = _invoice.GetTevkifatByTL(kdvTotal);

        var stopajDropdown = $('#StopajRate').data("kendoDropDownList");
        var stopajRate = 0;

        if (stopajDropdown && stopajDropdown != null && stopajDropdown.dataItem() != null) {
            stopajRate = stopajDropdown.dataItem().enumDescription.split('%')[0];
        }

        $('#totalOTV').val(otvTotal);
        $('#totalOIV').val(oivTotal);
        $('#totalKDV').val(kdvTotal);
        $('#stopaj').val(stopajRate);
        $('#totalKDV').val(kdvTotal - tevkifat);

        _invoice.WriteTextByDataResult(totalDiscount, "productDiscounts", "-");
        _invoice.WriteTextByDataResult(subTotalDiscountTL, "subTotalDiscount", "-");
        _invoice.WriteTextByDataResult(totalSubAmount, "totalSubAmount", "");
        _invoice.WriteTextByDataResult(stopaj, "stopaj", "-");
        _invoice.WriteTextByDataResult(otvTotal, "otv", "+");
        _invoice.WriteTextByDataResult(oivTotal, "oiv", "+");
        _invoice.WriteTextByDataResult(kdvTotal, "kdv", "+");
        _invoice.WriteTextByDataResult(subTotalProduct, "subTotal", "+");
        _invoice.WriteTextByDataResult(parseFloat(totalAmount) - parseFloat(tevkifat), "totalAmount", "");
        _invoice.WriteTextByDataResult(tevkifat, "tevkifat", "-");

        var dropdownCurrency = $('#currencyId').data("kendoDropDownList");
        var dataItemCurrency = dropdownCurrency.dataItem();

        $('[data-cur="currency"]').text(dataItemCurrency.symbol);

        $('[data-rowType="TL"]').addClass("hide");
        if (dataItemCurrency.code && dataItemCurrency.code != "TL") {
            var exchange = $('#rateExchange').data("kendoNumericTextBox").value();
            exchange = exchange ? exchange : 0;

            $('[data-rowType="TL"]').removeClass("hide");
            $('#TLequality').val(parseFloat(totalAmount - tevkifat) * exchange);

            _invoice.WriteTextByDataResult(parseFloat(totalAmount - tevkifat) * exchange, "TLequality", "");
        }
    },
    Init: function () {
        if (_model.InvoiceItems != null) {
            $('body').loadingModal({ text: 'Lütfen Bekleyin...', animation: 'rotatingPlane', backgroundColor: 'black' });
        }
        if (_model.type == _invoice.EnumInvoiceType.find(a => a.EnumKey == "Teklif").Key) {
            CKEDITOR.config.entities_latin = !0;
            CKEDITOR.config.entities_latin = false;
            CKEDITOR.config.extraAllowedContent = false;

            CKEDITOR.replace('conditions', {
                extraPlugins: 'image2',
                filebrowserImageUploadUrl: '/CMP/VWCMP_Tender/EditorUploadFile?id=' + $("#id").val(),
                height: '120px',
                removeButtons: 'CustomImageUploader',
                removePlugins: 'about'
            });

            CKEDITOR.replace('description', {
                extraPlugins: 'image2',
                filebrowserImageUploadUrl: '/CMP/VWCMP_Tender/EditorUploadFile?id=' + $("#id").val(),
                height: '120px',
                removeButtons: 'CustomImageUploader',
                removePlugins: 'about'
            });

            CKEDITOR.instances.description.setData(_model.description, function () {
                this.checkDirty();
            });

            CKEDITOR.instances.conditions.setData(_model.tenderConditions, function () {
                this.checkDirty();
            });
        }

        _invoice.AddRowForProduct(false);

        $('[name="type"]').trigger("change");

        $('[for="' + $('[name="status"]:checked').attr("id") + '"]').trigger("click");

        $('[name="paymentType"]').trigger("change");

        if (_model.InvoiceItems && _model.InvoiceItems.length > 0) {

            $.each(_model.InvoiceItems, function (i, item) {
                if (_model.InvoiceItems.length - 1 != i) {
                    _invoice.AddRowForProduct(false);
                }
            })
        }
    },
    LoadProducts: function () {
        _invoice.LoadControl.loaded = true;
        var percent = _invoice.EnumDiscountType.find(a => a.EnumKey == "Yuzde");
        $.each(_model.InvoiceItems, function (i, item) {
            var mainRow = $('[name="InvoiceItems[' + i + '].productId"]').parents("tr");
            var subRow = $('[data-id="' + $(mainRow).attr("data-row") + '"]');

            if (item.description && item.description != null) {
                $(subRow).find('[data-content="description"]').removeClass("hide");
            }

            if (item.discount && item.discount != 0) {
                $(subRow).find('[data-content="discount"]').removeClass("hide");
            }

            if (item.OTV && item.OTV != 0) {
                $(subRow).find('[data-content="otv"]').removeClass("hide");
            }

            if (item.OIV && item.OIV != 0) {
                $(subRow).find('[data-content="oiv"]').removeClass("hide");
            }
            DropDownSetValue($('[name="InvoiceItems[' + i + '].productId"]').data("kendoDropDownList"), item.productId);

            _invoice.loadCount++;
            if (_model.InvoiceItems.length == _invoice.loadCount) {
                setTimeout(function () {
                    $('body').loadingModal('destroy');
                }, (_model.InvoiceItems.length > 10 ? 6000 : 2500))
            }

            $('[name="InvoiceItems[' + i + '].id"]').val(item.id);
            //$('[name="InvoiceItems[' + i + '].productId"]').attr("data-initial", true);
            //$('[name="InvoiceItems[' + i + '].productId"]').data("kendoDropDownList").value(item.productId);
            //$('[name="InvoiceItems[' + i + '].productId"]').data("kendoDropDownList").trigger("change");


            $(mainRow).find('[data-item="quantity"]').data("kendoNumericTextBox").value(item.quantity);
            $(mainRow).find('[data-item="price"]').data("kendoNumericTextBox").value(item.price);

            var kdvDropDown = $(mainRow).find('[name="KDVRate"]').data("kendoDropDownList");
            var kdvItems = kdvDropDown.dataItems();

            if (item.KDV != null && item.KDV != undefined) {
                var selectKDV = kdvItems.find(a => a.enumDescription == item.KDV.toString() + "%");
                kdvDropDown.value(selectKDV.Id);
                $(mainRow).find('[data-item="KDV"]').val(item.KDV);
            }
            else {
                var selectKDV = kdvItems.find(a => a.enumDescription == "8%");
                kdvDropDown.value(selectKDV.Id);
                $(mainRow).find('[data-item="KDV"]').val(8);
            }

            $(subRow).find('[name="InvoiceItems[' + i + '].description"]').val(item.description);

            var oiv = item.OIV != null ? item.OIV : 0;
            $(subRow).find('[name="InvoiceItems[' + i + '].OIV"]').data("kendoNumericTextBox").value(oiv);

            var disc = item.discount != null ? item.discount : 0;
            var discountContent = $(subRow).find('[data-content="discount"]');
            $.each($(discountContent).find('li'), function (j, elem) {
                if ($(elem).find('a').text().includes("%") == (item.discountType == percent.Key)) {
                    $(elem).find('a').trigger("click");
                }
            });

            $(subRow).find('[name="InvoiceItems[' + i + '].discount"]').data("kendoNumericTextBox").value(disc);

            var otv = item.OTV != null ? item.OTV : 0;
            var otvContent = $(subRow).find('[data-content="otv"]');
            $.each($(otvContent).find('li'), function (j, elem) {
                if ($(elem).find('a').text().includes("%") == (item.OTVType == percent.Key)) {
                    $(elem).find('a').trigger("click");
                }
            });

            $(subRow).find('[name="InvoiceItems[' + i + '].OTV"]').data("kendoNumericTextBox").value(otv);

            if (_model.InvoiceItems.length - 1 == i) {
                _invoice.CalculateProduct();

                var discountList = $('#subTotalDiscountType').parents(".input-group-btn").find("li");

                $.each(discountList, function (j, elem) {
                    if ($(elem).find('a').text().includes("%") == (item.discountType == percent.Key)) {
                        $(elem).find('a').trigger("click");
                    }
                })
                if (_model.discount != 0 && _model.discount != null) {
                    $('#subTotalDiscountDiv').removeClass("hide");
                    $('#subTotalDiscount').data("kendoNumericTextBox").value(_model.discount);
                }
                else {
                    $('#subTotalDiscount').data("kendoNumericTextBox").value(0);
                }

                if (_model.stopaj && _model.stopaj != 0) {
                    $('#stopajRow').removeClass("hide");

                    var stopajDropDown = $('#StopajRate').data("kendoDropDownList");
                    var stopajItems = stopajDropDown.dataItems();

                    var selectStopaj = stopajItems.find(a => a.enumDescription == _model.stopaj.toString() + "%");

                    stopajDropDown.value(selectStopaj.Id);
                    stopajDropDown.trigger("change");
                }

                if (_model.tevkifat && _model.tevkifat != 0) {
                    $('#tevkifatRow').removeClass("hide");

                    var tevkifatDropDown = $('#TevkifatRate').data("kendoDropDownList");
                    var tevkifatItems = tevkifatDropDown.dataItems();

                    var tevkifatStopaj = tevkifatItems.find(a => a.enumDescription == _model.tevkifat.toString() + "/10");

                    tevkifatDropDown.value(tevkifatStopaj.Id);
                    tevkifatDropDown.trigger("change");
                }
            }
        })

        $('#customerId').data("kendoDropDownList").trigger("change");
        $('#supplierId').data("kendoDropDownList").trigger("change");

        var difference = new Date(_model.expiryDate).getTime() - new Date(_model.issueDate).getTime();
        var difDay = difference / (1000 * 3600 * 24);

        $('[data-id="' + difDay + '"]').trigger("click");

        $('[name="type"]').trigger("change");
        $('[name="paymentType"]').trigger("change");

        _invoice.CalculateProduct();

        $('form').validator("validate");
    }
}

$(document)
    .on("before:submit", 'form', function (e) {
        if (_model.type == _invoice.EnumInvoiceType.find(a => a.EnumKey == "Teklif").Key) {
            $("#conditions").val(CKEDITOR.instances['conditions'].getData());
            $("#description").val(CKEDITOR.instances['description'].getData());
        }
    })
    .on("click", ".myDropDownMenu li a", function (e) {
        var selText = $(this).text();
        $(this).parents('.input-group').find('.dropdown-toggle').html(selText + "  " + '<span class="caret"></span>');
        $(this).parents(".input-group").find('[data-calculate="keyup"]').data("kendoNumericTextBox").max(1000000000);

        $(this).parents(".input-group").find('[data-calculate="keyup"]').data("kendoNumericTextBox").value(0);

        var type = _invoice.EnumDiscountType.find(a => a.EnumKey == "Tutar").Key;
        if (selText == "%") {
            type = _invoice.EnumDiscountType.find(a => a.EnumKey == "Yuzde").Key;
            $(this).parents(".input-group").find('[data-calculate="keyup"]').data("kendoNumericTextBox").max(100);
        }

        $(this).parents(".input-group").find('[data-name="type"]').val(type);

        _invoice.CalculateProduct($(this));
    })
    .on("change", '[name="paymentType"]', function (e) {

        var value = $('[name="paymentType"]:checked').val();

        $('#installmentDiv').addClass("hide");
        $('#expiryDiv').addClass("hide");
        $('#accountDiv').addClass("hide");

        if (value == _invoice.EnumPaymentType.find(a => a.EnumKey == "Taksitli").Key) {
            $('#installmentDiv').removeClass("hide");
        }
        else if (value == _invoice.EnumPaymentType.find(a => a.EnumKey == "Vadeli").Key || value == _invoice.EnumPaymentType.find(a => a.EnumKey == "Cek")) {
            $('#expiryDiv').removeClass("hide");
        }

        else {
            $('#accountDiv').removeClass("hide");
        }
    })
    .on("change", '[name="type"]', function (e) {
        e.preventDefault();
        var elem = $('#sendingDate').data("kendoDatePicker");

        if (elem && (_model.type == _invoice.EnumInvoiceType.find(a => a.EnumKey == "IrsaliyeliFatura").Key || _model.type == _invoice.EnumInvoiceType.find(a => a.EnumKey == "IrsaliyesizFatura").Key)) {

            var value = $('input[name="type"]:checked').val();

            $('#sendingDateDiv').addClass("hide");
            elem.value(null);

            $('#sendingDate').removeAttr("required");

            if (value == _invoice.EnumInvoiceType.find(a => a.EnumKey == "IrsaliyeliFatura").Key) {
                $('#sendingDateDiv').removeClass("hide");
                $('#sendingDate').attr("required", "required");
                elem.value(new Date());
                elem.trigger("change");
            }
        }
    })
    .on("focus", '[data-calculate="keyup"]', function (e) {
        if ($(this).data("kendoNumericTextBox").value() == 0) {
            $(this).data("kendoNumericTextBox").value(null);
        }
    })
    .on("keyup", '[data-calculate="keyup"]', function (e) {
        e.preventDefault();

        var newValue = e.target.value;

        if (newValue[newValue.length - 1] == ",") {
            return;
        }
        if (newValue.charAt(newValue.length - 1) != ",") {
            var dropdown = $(this).data("kendoNumericTextBox");

            // newValue = parseFloat(newValue.replace(",", "."));

            // if (!isNaN(newValue)) {

            if (dropdown.max() && dropdown.max() != null) {
                newValue = newValue > dropdown.max() ? dropdown.max() : newValue;
            }

            dropdown.value(newValue);
            dropdown.trigger("change");
            setTimeout(function () {
                _invoice.CalculateProduct($(this));
            }, 100)
            // }
        }
    })
    .on("click", '.myRadio', function (e) {
        $(this).find('[name="type"]').prop("checked", true);
        $('[name="type"]').trigger("change");
    })
    .on("success", "#VWCMP_CompanyUpdateForm, #VWCMP_CompanyInsertForm", function (e, res) {

        var dropdownCus = $('#customerId').data("kendoDropDownList");

        if (dropdownCus && dropdownCus.value() == res.Object) {
            dropdownCus.trigger("change");
        }

        var dropdownSup = $('#supplierId').data("kendoDropDownList");

        if (dropdownSup && dropdownSup.value() == res.Object) {
            dropdownSup.trigger("change");
        }
    })
    .on("click", '[data-name="filterDate"]', function (e) {

        var _this = $(this);

        $('.filterButtonActive').removeClass("filterButtonActive").addClass("filterButtonPassive");
        $('.fa-check').removeClass('fa fa-check');
        _this.removeClass("filterButtonPassive").addClass("filterButtonActive");
        _this.find('span').addClass("fa fa-check");
        var attr = _this.attr("data-id");
        var issueDate = $('#issueDate').data("kendoDatePicker").value();
        var newDate = new Date(issueDate).addDays(parseInt(attr));
        $('#expiryDate').data("kendoDatePicker").value(newDate);

    })
    .on("change", '[name="status"]', function (e) {
        var value = $('[name="status"]:checked').val();
        $('#statusDiv_0').addClass("hide");
        $('#statusDiv_1').addClass("hide");
        $('#statusDiv_2').addClass("hide");
        if (value == _invoice.EnumInvoiceStatus.find(a => a.EnumKey == "Odendi").Key) {
            $('#statusDiv_0').removeClass("hide");
        }
        else if (value == _invoice.EnumInvoiceStatus.find(a => a.EnumKey == "Odenecek").Key) {
            $('#statusDiv_1').removeClass("hide");
        } else {
            $('#statusDiv_2').removeClass("hide");
        }
    })
    .on("change", '#issueDate', function (e) {
        $(".filterButtonActive").trigger("click");
    })
    .on("change", '[data-item="quantity"]', function () {
        
        var valueArray = [];
        $.each($('[data-item="quantity"]'), function (i, item) {
            valueArray.push(($item).val());
        });
        if (JQuery.inArray(0 || '0' || '', valueArray)==-1) {
            $("#save").removeAttr("disabled");
        }
        else {
            $("#save").attr("disabled","");
        }


          
    });
   
    
