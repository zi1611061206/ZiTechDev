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