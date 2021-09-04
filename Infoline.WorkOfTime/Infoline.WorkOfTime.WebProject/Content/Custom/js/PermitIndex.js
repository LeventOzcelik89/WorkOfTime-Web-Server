$(document)
           .on("load:grid", "#VWINV_Permit", function (e, data) {

               $.each($('.k-grid-content [role="row"]').find('[data-event="GridSelector"]'), function (i, item) {

                   var color = $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.TalepEdildi )'
                        ? '#0099ff'
                        : $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.Yonetici1Onay )'
                           ? '#4ef641'
                           : $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.Yonetici2Onay)'
                               ? '#2b9f21'
                                : '#df2317';

                   $(item).parent('td').css('background-color', (color + ' !important'));

               });
           })

    .on("load:grid", "#VWINV_Permit2", function (e, data) {

        $.each($('.k-grid-content [role="row"]').find('[data-event="GridSelector"]'), function (i, item) {

            var color = $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.TalepEdildi )'
                 ? '#0099ff'
                 : $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.Yonetici1Onay )'
                    ? '#4ef641'
                    : $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.Yonetici2Onay)'
                        ? '#2b9f21'
                         : '#df2317';

            $(item).parent('td').css('background-color', (color + ' !important'));

        });
    })

    .on("load:grid", "#VWINV_Permit3", function (e, data) {

        $.each($('.k-grid-content [role="row"]').find('[data-event="GridSelector"]'), function (i, item) {

            var color = $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.TalepEdildi )'
                 ? '#0099ff'
                 : $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.Yonetici1Onay )'
                    ? '#4ef641'
                    : $(item).attr('data-ApproveStatus') == '@((Int32)EnumINV_PermitApproveStatus.Yonetici2Onay)'
                        ? '#2b9f21'
                         : '#df2317';

            $(item).parent('td').css('background-color', (color + ' !important'));

        });
    })

       .on('load:grid', '#VWINV_Permit', function (e, b) {

           var total = $('#VWINV_Permit').data('kendoGrid').dataSource._pristineTotal.toString();
           $('[data-TotalCount="true"] h2 > span:nth-last-child(even)').text(total.toLocaleString() + " Adet");

       })

        .on('load:grid', '#VWINV_Permit2', function (e, b) {

            var wait = $('#VWINV_Permit2').data('kendoGrid').dataSource.total();
            $('[data-WaitCount="true"] h2 > span:nth-last-child(even)').text(wait.toLocaleString() + " Adet");

        })

     .on('load:grid', '#VWINV_Permit3', function (e, b) {

         var red = $('#VWINV_Permit3').data('kendoGrid').dataSource.total();
         $('[data-RedCount="true"] h2 > span:nth-last-child(even)').text(red.toLocaleString() + " Adet");

     })


