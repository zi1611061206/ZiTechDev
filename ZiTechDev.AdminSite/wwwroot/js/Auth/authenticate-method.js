// Hàm chọn phương thức xác thực
function MethodSelected(provider) {
    document.getElementById('txbProvider').value = provider;
    var form = document.getElementById("form-method");
    form.submit();
}