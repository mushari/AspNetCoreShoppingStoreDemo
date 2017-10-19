
function readURL(input) {
    for (var i = 0; i < input.files.length; i++) {
        if (input.files[i]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                var tbody = $('#upload_list');
                tbody.append(
                    '<tr><td><img src="' + e.target.result + '" width="100" height="100"/></td></tr>');
            }
            reader.readAsDataURL(input.files[i]);
        }
    }
}