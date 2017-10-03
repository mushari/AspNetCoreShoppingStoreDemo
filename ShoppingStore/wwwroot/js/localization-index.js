
var cancel_btn = function(cancel_btn_id, popover_btn_id) {
    $(document).on('click', cancel_btn_id, function(e) {
        e.preventDefault();

        $('body').on('hidden.bs.popover', function(e) {
            $(e.target).data("bs.popover").inState.click = false;
        });

        $(popover_btn_id).popover('hide');
    });
}

var click_backgroud_cancel_popover = function(popover_btn_id) {
    $('body').on('mousedown', function(e) {
        $(popover_btn_id).each(function() {
            //Check if the popover is active / contain an aria-describedby with a value
            if ($(this).attr('aria-describedby') != null) {
                //Put the value in a variable
                var id = $(this).attr('aria-describedby');
                if (!$(e.target).closest(".popover-content").length &&
                    $(e.target).attr("aria-describedby") != id &&
                    //Look if the click is a child of the popover box or if it's the button itself or a child of the button
                    !$(e.target).closest('[aria-describedby="' + id + '"').length) {
                    //trigger a click as if you clicked the button
                    $('[aria-describedby="' + id + '"]').trigger("click");
                }
            }
        });
    });
}

var addPopover = function(cancel_btn_id, popover_btn_id) {
    $(popover_btn_id).popover();
    cancel_btn(cancel_btn_id, popover_btn_id);
    click_backgroud_cancel_popover(popover_btn_id);
}


var addNewKeyName_ajax = function(form_id,validation_id) {
    $(document).on('submit', form_id, function(e) {
        e.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            url: "api/addNewKeyName",
            type: "POST",
            data: formData
        }).done(function(data) {
            location.reload();
        }).fail(function(xhr, textStatus, errorThrown) {
            console.log(xhr.responseJSON);
            console.log(textStatus);
            console.log(errorThrown);
            if (xhr.responseJSON.keyname != null && xhr.responseJSON.notempty != null) {
                $(validation_id).html(xhr.responseJSON.keyname.value + " " + xhr.responseJSON.notempty.value);
            }
            $(validation_id).html(xhr.responseJSON.value);

            console.log("failed");
        });
    });
}

var addNewCulture_ajax = function(form_id,validation_id) {
    $(document).on('submit', form_id, function(e) {

        var formData = $(this).serializeArray();

        var dataString = "pk=" + formData[0].name + "&culture=" + formData[0].value + "&value=" + formData[1].value;

        $.ajax({
            url: "api/addNewCulture",
            type: "POST",
            data: dataString
        }).done(function(data) {
            location.reload();
        }).fail(function(xhr, textStatus, errorThrown) {
            console.log(xhr.responseJSON);
            console.log(textStatus);
            console.log(errorThrown);
            if (xhr.responseJSON.culture != null &&
                xhr.responseJSON.notempty != null) {
                $(validation_id).html(
                    xhr.responseJSON.culture.value + " " +
                    xhr.responseJSON.notempty.value);
            }
            if (xhr.responseJSON.culture != null &&
                xhr.responseJSON.alreadyhave != null) {
                $(validation_id).html(
                    xhr.responseJSON.culture.value + " " +
                    xhr.responseJSON.alreadyhave.value);
            }
            if (xhr.responseJSON.culture != null &&
                xhr.responseJSON.formatnotright != null) {
                $(validation_id).html(
                    xhr.responseJSON.culture.value + " " +
                    xhr.responseJSON.formatnotright.value);
            }
        });
        e.preventDefault();
    });
}

var deleteKeyName_ajax = function(delete_form_id) {
    $(document).on('submit', delete_form_id, function(e) {

        var formData = $(this).serialize();

        $.ajax({
            url: "api/deleteKeyName",
            type: "DELETE",
            data: formData
        }).done(function(data) {
            location.reload();
        }).fail(function(xhr, textStatus, errorThrown) {
            console.log(xhr.response);
            console.log(textStatus);
            console.log(errorThrown);
        });
        e.preventDefault();
    });
}

var editable_ajax = function(id) {
    $(id).editable({
       
        success: function(response, newValue) {
            location.reload();
        },
        error: function(response, newValue) {
            console.log(response);

            if (response.status === 500) {
                return 'Service unavailable. Please try later.';
            } else {
                if (response.responseJSON.keyname != null &&
                    response.responseJSON.notempty != null) {
                    return response.responseJSON.keyname.value + " " + response.responseJSON.notempty.value;
                }
                return response.responseJSON.value;
            }
        }
    });
}


var deleteCulture_ajax = function(culture_form_id) {
    $(document).on('submit', culture_form_id, function(e) {

        var formData = $(this).serializeArray();
        var dataString = "culture=" + formData[0].name + "&value=" + formData[0].value;

        $.ajax({
            url: "api/deleteCulture",
            type: "DELETE",
            data: dataString
        }).done(function(data) {
            location.reload();
        }).fail(function(xhr, textStatus, errorThrown) {
            console.log(xhr.responseJSON.value);
            console.log(textStatus);
            console.log(errorThrown);
        });
        e.preventDefault();
    });
}
