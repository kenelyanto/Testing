$('.btnSave').click(function () {
    window.setTimeout(function () {
        $('.btnSave').attr("disabled", "disabled");
        $('.btnSaveAndClose').attr("disabled", "disabled");
    }, 0);
});
$('.btnSaveAndClose').click(function () {
    window.setTimeout(function () {
        $('.btnSave').attr("disabled", "disabled");
        $('.btnSaveAndClose').attr("disabled", "disabled");
    }, 0);
});
function confirmBox(dialog,frm) {
    bootbox.confirm("Are you sure to " + dialog + "?", function (result) {
        if (result) {
            if ($('#loading').length)
                {
                $('#loading').removeClass('hidden');
                }
            frm.submit();
        }
    });
}
