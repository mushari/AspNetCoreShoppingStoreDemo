
var cancel_btn = function (cancel_btn_id, popover_btn_id) {
    $(document).on('click', cancel_btn_id, function (e) {
        e.preventDefault();

        $('body').on('hidden.bs.popover', function (e) {
            $(e.target).data("bs.popover").inState.click = false;
        });

        $(popover_btn_id).popover('hide');
    });
}

var click_backgroud_cancel_popover = function (popover_btn_id) {
    $('body').on('mousedown', function (e) {
        $(popover_btn_id).each(function () {
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

var add_popover = function (cancel_btn_id, popover_btn_id) {
    $(popover_btn_id).popover();
    cancel_btn(cancel_btn_id, popover_btn_id);
    click_backgroud_cancel_popover(popover_btn_id);
}



var popover_ajax = function (form) {
    
    $(document).on('submit', form.form_id, function (e) {
        e.preventDefault();

        //var formData = form.form_id || $(form.form_id).serialize();
        var formData =  $(form.form_id).serializeArray();


        if (form.done && form.fail) {
            console.log("done and fail not null");
            $.ajax({
                url: form.url,
                type: form.type,
                data: formData,
                contentType: form.contentType || "application/x-www-form-urlencoded"
            }).done(form.done).fail(form.fail);
        } else if (!form.done && !form.fail) {
            console.log("done and fail null");
            $.ajax({
                url: form.url,
                type: form.type,
                data: formData,
                contentType: form.contentType || "application/x-www-form-urlencoded"
            }).done(function (data) {
                location.reload();
            }).fail(function (xhr, status, error) {
                $(form.validation_id).html("error,something failed.");
            });
        } else if (!form.done && form.fail) {
            console.log("done null");
            $.ajax({
                url: form.url,
                type: form.type,
                data: formData,
                contentType: form.contentType || "application/x-www-form-urlencoded"
            }).done(function (data) {
                location.reload();
            }).fail(form.fail);
        } else if (form.done && !form.fail) {
            console.log("fail null");
            $.ajax({
                url: form.url,
                type: form.type,
                data: formData,
                contentType: form.contentType || "application/x-www-form-urlencoded"
            }).done(form.done).fail(function (xhr, status, error) {
                $(form.validation_id).html("error,something failed.");
            });
        }
    });

}


var xeditable_ajax = function (id) {

    $(id).editable({
        success: function (response, newValue) {
            location.reload();
        },
        error: function (response, newValue) {
            console.log(response);

            if (response.status === 500) {
                return 'Service unavailable. Please try later.';
            }
        }
    });
}

