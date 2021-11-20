//Hàm rút gọn / hiển thị đẩy đủ chuỗi
$(function () {
	$('.limitContent').each(function (i) {
		len = $(this).text().length;
		if (len > 10) {
			$(this).text($(this).text().substr(0, 10) + '...');
		}
		$(this).on('click', function () {
			$(this).text($(this).data('content'));
		})
		$(this).on('dblclick', function () {
			length = $(this).text().length;
			if (length > 10) {
				$(this).text($(this).text().substr(0, 10) + '...');
			}
		})
	});
});

//Hàm đóng mở bộ lọc
function btnFilterClick() {
	if (document.getElementById("filterForm").hidden) {
		document.getElementById("filterForm").hidden = false;
	} else {
		document.getElementById("filterForm").hidden = true;
	}
}

//Hàm chọn trang
function btnPageClick(pageIndex) {
	document.getElementById("txbPageIndex").value = pageIndex;
	document.getElementById("filterForm").submit();
}

//Hàm tăng giảm page size
function pageSizeChanged() {
	var pageSize = document.getElementById("txbPageSizeEnter").value;
	document.getElementById("txbPageSize").value = pageSize;
	document.getElementById("txbPageIndex").value = 1;
	document.getElementById("filterForm").submit();
}

//Hàm tìm kiếm
function btnSearchClick() {
	var keyword = document.getElementById("txbSearch").value;
	document.getElementById("txbKeyword").value = keyword;
	document.getElementById("filterForm").submit();
}

//Hàm ẩn message sau 3s
setTimeout(function () {
	jQuery('.alert').fadeOut('slow')
}, 3000);

//Hàm xác nhận xóa
function btnDeleteConfirmClick() {
	var id = document.getElementById('txbDeleteId').value;
	location.href = '/RolesAdministrator/Delete?roleId=' + id;
}

//Hàm copy chuỗi
function CopyToClipboard(text) {
	const el = document.createElement('textarea');
	el.value = text;
	el.setAttribute('readonly', '');
	el.style.position = 'absolute';
	el.style.left = '-9999px';
	document.body.appendChild(el);
	el.select();
	document.execCommand('copy');
	document.body.removeChild(el);
}