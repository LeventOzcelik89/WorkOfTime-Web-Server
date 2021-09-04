$(document)
    .on("ready", function () {
        $('[data-selector="twoHour"]').trigger("click");
    })
     .on("click", '[data-selector]', function (e) {

         e.preventDefault();

         var selector = $(this).attr('data-selector');

         $('[data-show]').hide();
         $('[data-show="' + selector + '"]').show();

         $('[data-role="chart"]').each(function () {
             $(this).data('kendoChart').resize();
         });
     })

  .on('load:grid', '#STN_Station1', function (e, b) {
      var istasyon = $('#STN_Station1').data('kendoGrid').dataSource._pristineTotal.toString();
      $("#STN_Station1 .k-grid-toolbar").empty();
      $("#STN_Station1 .k-grid-toolbar").append('<span style="float: right;padding-left: 5px;padding-top: 6px;font-weight: bold; font-size:initial; ">' + istasyon.toLocaleString() + " İstasyon" + '</span>');
  })
 .on('load:grid', '#STN_Station2', function (e, b) {
     var istasyon = $('#STN_Station2').data('kendoGrid').dataSource._pristineTotal.toString();
     $("#STN_Station2 .k-grid-toolbar").empty();
     $("#STN_Station2 .k-grid-toolbar").append('<span style="float: right;padding-left: 5px;padding-top: 6px;font-weight: bold; font-size:initial; ">' + istasyon.toLocaleString() + " İstasyon" + '</span>');
 })
 .on('load:grid', '#STN_Station3', function (e, b) {
     var istasyon = $('#STN_Station3').data('kendoGrid').dataSource._pristineTotal.toString();
     $("#STN_Station3 .k-grid-toolbar").empty();
     $("#STN_Station3 .k-grid-toolbar").append('<span style="float: right;padding-left: 5px;padding-top: 6px;font-weight: bold; font-size:initial; ">' + istasyon.toLocaleString() + " İstasyon" + '</span>');
 })
 .on('load:grid', '#STN_Station4', function (e, b) {
     var istasyon = $('#STN_Station4').data('kendoGrid').dataSource._pristineTotal.toString();
     $("#STN_Station4 .k-grid-toolbar").empty();
     $("#STN_Station4 .k-grid-toolbar").append('<span style="float: right;padding-left: 5px;padding-top: 6px;font-weight: bold; font-size:initial; ">' + istasyon.toLocaleString() + " İstasyon" + '</span>');
 })
 .on('load:grid', '#STN_Station5', function (e, b) {
     var istasyon = $('#STN_Station5').data('kendoGrid').dataSource._pristineTotal.toString();
     $("#STN_Station5 .k-grid-toolbar").empty();
     $("#STN_Station5 .k-grid-toolbar").append('<span style="float: right;padding-left: 5px;padding-top: 6px;font-weight: bold; font-size:initial; ">' + istasyon.toLocaleString() + " İstasyon" + '</span>');
 })
 .on('load:grid', '#STN_Station6', function (e, b) {
     var istasyon = $('#STN_Station6').data('kendoGrid').dataSource._pristineTotal.toString();
     $("#STN_Station6 .k-grid-toolbar").empty();
     $("#STN_Station6 .k-grid-toolbar").append('<span style="float: right;padding-left: 5px;padding-top: 6px;font-weight: bold; font-size:initial; ">' + istasyon.toLocaleString() + " İstasyon" + '</span>');
 })
.on('load:grid', '#STN_Station7', function (e, b) {
    var istasyon = $('#STN_Station7').data('kendoGrid').dataSource._pristineTotal.toString();
    $("#STN_Station7 .k-grid-toolbar").empty();
    $("#STN_Station7 .k-grid-toolbar").append('<span style="float: right;padding-left: 5px;padding-top: 6px;font-weight: bold; font-size:initial; ">' + istasyon.toLocaleString() + " İstasyon" + '</span>');
})