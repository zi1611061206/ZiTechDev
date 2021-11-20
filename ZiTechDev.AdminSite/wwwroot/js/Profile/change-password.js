//Hàm ẩn hiện mật khẩu
function ShowHidePW(elementId) {
    if ($('#_' + elementId).attr("type") == "text") {
        $('#_' + elementId).attr('type', 'password');

        $('#' + elementId).addClass("fa-eye");
        $('#' + elementId).removeClass("fa-eye-slash");
    }
    else if ($('#_' + elementId).attr("type") == "password") {
        $('#_' + elementId).attr('type', 'text');

        $('#' + elementId).removeClass("fa-eye");
        $('#' + elementId).addClass("fa-eye-slash");
    }
}