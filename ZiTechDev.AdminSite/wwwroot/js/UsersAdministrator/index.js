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
	document.getElementById("filterForm").submit();
}