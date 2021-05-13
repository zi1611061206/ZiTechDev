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