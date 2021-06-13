//Hàm tự động tạo tên hiển thị dựa trên FullName
function fullNameChanged() {
    var firstName = document.getElementById('txbFirstName').value;
    var middleName = document.getElementById('txbMiddleName').value;
    var lastName = document.getElementById('txbLastName').value;
    var displayName = document.getElementById('txbDisplayName').value;
    if (displayName.trim().length > 0) return;
    var fullName = firstName + middleName + lastName;
    for (var i = 0; i < fullName.length; i++) {
        if (i % 2 == 0) {
            displayName += fullName.charAt(i);
        }
    }
    if (displayName.length > 20) {
        displayName = displayName.slice(0, 19);
    }
    document.getElementById('txbDisplayName').value = displayName;
}