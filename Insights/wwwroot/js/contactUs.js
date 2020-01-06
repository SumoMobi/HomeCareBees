var _hcb = _hcb || {};
_hcb.contactUs = {
    setValueForDummy: function () {
        var checkboxes = $("form .form-row :checkbox");
        var dummyInput = $("#dummy");
        dummyInput.removeClass("is-invalid");
        dummyInput.val("");
        checkboxes.each(function () {
            if (this.checked) {
                dummyInput.val("true");
                return false;
            }
        });
        if (dummyInput.val() === "") {
            dummyInput.addClass("is-invalid");
        }
    },
    submitClicked: function (evt) {
        var dummyInput = $("#dummy");
        if (dummyInput.val() === "") {
            dummyInput.addClass("is-invalid");
            evt.preventDefault();
            evt.stopPropagation();
            return;
        }
        try {
            ga('send', {
                hitType: 'event',
                eventCategory: 'ContactRequest',
                eventAction: 'submit',
                eventLabel: 'contactRequest',
                eventValue: 1
            });
        }
        catch {}
    }
}
$(document).ready(function () {
    $("form .form-row :checkbox").on("click", function () {
        _hcb.contactUs.setValueForDummy();
    });
});
