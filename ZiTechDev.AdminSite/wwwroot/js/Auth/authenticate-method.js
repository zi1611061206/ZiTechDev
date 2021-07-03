// Hàm chọn phương thức xác thực
function Method2faSelected(provider) {
    document.getElementById('txbProvider').value = provider;
    var form = document.getElementById("form-method");
    form.submit();
}