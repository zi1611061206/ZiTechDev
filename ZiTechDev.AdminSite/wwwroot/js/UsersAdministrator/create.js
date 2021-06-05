//Hàm tự động tạo tên hiển thị dựa trên UserName
function userNameChanged() {
    var userName = document.getElementById('txbUserName').value;
    var displayName = document.getElementById('txbDisplayName').value;
    if (displayName.trim().length > 0)
        return;
    else
        displayName = userName;
    document.getElementById('txbDisplayName').value = displayName;
}

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