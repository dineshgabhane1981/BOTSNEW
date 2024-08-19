var _arrayGroupData = [];
var obj, url_var;
$(document).ready(function () {
    url_var = document.URL;
    
    $('#ddlSelect').select2();
    $("#btnSaveGroupName").click(OnSave);  

    //console.log($("#hdnGroupCode").val())
    if ($("#hdnGroupCode").val() == null || $("#hdnGroupCode").val() == '') {
        $("#divConfigWAReport").show(1000);
    }
});
function OnSave() {
    var GroupCodeSelected = $("#ddlSelect option:selected").val();
    var GroupId = $("#hdnGroupId").val();

    $("#divLoader").show();
    $.ajax({
        type: "POST",
        //url: '@Url.Action("GetLoginType", "OTPAndLog")',//"/OTPAndLogController/GetTxnLogData",
        url: url_var +"/SaveReportDetails",
        data: '{GroupId: ' + JSON.stringify(GroupId) + ', GroupCode:' + JSON.stringify(GroupCodeSelected) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result == true) {
                toastr.success('Report Started');
                window.location.href = url_var + "/Index";
            } else {
                toastr.error('Something went wrong..');

            }
            $("#divLoader").hide();
        },
        error: function (result) {
            console.log(result.responseText)
            $("#divLoader").hide();
        }
    });

}