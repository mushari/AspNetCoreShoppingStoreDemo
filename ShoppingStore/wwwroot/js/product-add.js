
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

var cancel_btn = function (cancel_btn_id, popover_btn_id) {
    $(document).on('click', cancel_btn_id, function (e) {
        e.preventDefault();

        $('body').on('hidden.bs.popover', function (e) {
            $(e.target).data("bs.popover").inState.click = false;
        });

        $(popover_btn_id).popover('hide');
    });
}

var addPopover = function (cancel_btn_id, popover_btn_id) {
    $(popover_btn_id).popover();
    cancel_btn(cancel_btn_id, popover_btn_id);
    click_backgroud_cancel_popover(popover_btn_id);
}



var addNewCategory_ajax = function (form_id, validation_id) {
    $(document).on('submit', form_id, function (e) {
        e.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            url: "api/addCagetory",
            type: "POST",
            data: formData
        }).done(function (data) {
            location.reload();
        }).fail(function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseJSON);
            console.log(textStatus);
            console.log(errorThrown);
        });
    });
}