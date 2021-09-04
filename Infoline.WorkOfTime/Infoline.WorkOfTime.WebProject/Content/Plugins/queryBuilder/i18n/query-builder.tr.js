(function(root, factory) {
    if (typeof define == 'function' && define.amd) {
        define(['jquery', 'query-builder'], factory);
    }
    else {
        factory(root.jQuery);
    }
}(this, function($) {
"use strict";

var QueryBuilder = $.fn.queryBuilder;

QueryBuilder.regional['tr'] = {
  "__locale": "Turkish (tr)",
  "__author": "",
  "add_rule": "Kural Ekle",
  "add_group": "Kural Grubu Ekle",
  "delete_rule": "Sil",
  "delete_group": "Sil",
  "conditions": {
    "AND": "Ve",
    "OR": "Veya"
  },
  "operators": {
    "equal": "eşit ( = )",
    "not_equal": "eşit değil ( != )",
    "in": "dahil ( IN )",
    "not_in": "dahil değil ( NOT IN )",
    "less": "küçük ( < )",
    "less_or_equal": "küçük veya eşit ( <= )",
    "greater": "büyük ( > )",
    "greater_or_equal": "büyük veya eşit ( >= )",
    "between": "arasında",
    "not_between": "arasında değil",
    "begins_with": "ile başlayan",
    "not_begins_with": "ile başlamayan",
    "contains": "içeren",
    "not_contains": "içermeyen",
    "ends_with": "ile biten",
    "not_ends_with": "ile bitmeyen",
    "is_empty": "boştur ( '' )",
    "is_not_empty": "boş değildir",
    "is_null": "NULL",
    "is_not_null": "NOT NULL"
  },
  "errors": {
    "no_RefId": "Referans bölgesi seçilmedi",
    "no_times": "Zaman süreci seçilmedi",
    "no_timesOperator": "Süreç operatörü seçilmedi",
    "no_timesInput": "Süreç belirtilmedi",
    "no_tolerance": "Tolerans değeri belirtilmedi",
    "no_filter": "Filtre seçilmedi",
    "empty_group": "Grup bir eleman içermiyor",
    "radio_empty": "seçim yapılmalı",
    "checkbox_empty": "seçim yapılmalı",
    "select_empty": "seçim yapılmalı",
    "string_empty": "Bir metin girilmeli",
    "string_exceed_min_length": "En az {0} karakter girilmeli",
    "string_exceed_max_length": "En fazla {0} karakter girilebilir",
    "string_invalid_format": "Uyumsuz format ({0})",
    "number_nan": "Sayı değil",
    "number_not_integer": "Tam sayı değilr",
    "number_not_double": "Ondalıklı sayı değil",
    "number_exceed_min": "Sayı {0}'den/dan daha büyük olmalı",
    "number_exceed_max": "Sayı {0}'den/dan daha küçük olmalı",
    "number_wrong_step": "{0} veya katı olmalı",
    "datetime_empty": "Tarih Seçilmemiş",
    "datetime_invalid": "Uygun olmayan tarih formatı ({0})",
    "datetime_exceed_min": "{0} Tarihinden daha sonrası olmalı.",
    "datetime_exceed_max": "{0} Tarihinden daha öncesi olmalı.",
    "boolean_not_valid": "Değer Doğru/Yanlış(bool) olmalı",
    "operator_not_multiple": "Operatör {0} birden fazla değer kabul etmiyor"
  },
  "invert": "Ters Çevir"
};

QueryBuilder.defaults({ lang_code: 'tr' });
}));