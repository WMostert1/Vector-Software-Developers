$(document).ready(function () {
	$('#btnAdd').click(function () {
		var name = $('#BlockName').val();
		var element = "<tr><td>" + name + "</td></tr>";
		$('table').append(element);
		$('#BlockName').val('');
		return false;
	});
});
