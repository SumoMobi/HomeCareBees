var _hcb = _hcb || {};
_hcb.contactUs = {
    setValueForDummy: function () {
        let checkboxes = document.querySelectorAll("form .form-row input[type=checkbox]");
        var dummyInput = document.getElementById("dummy");
        dummyInput.value = "";
        checkboxes.forEach(function (chkBox) {
            if (chkBox.checked) {
                dummyInput.value = "true";  //Cannot break out of a forEach loop...
            }
        });
    },
    submitClicked: function (evt) {
        let centerCheckBox = document.getElementById("afternoonWednesday"); //Selecting a checkbox somewhere in the middle of the set.
        centerCheckBox.setCustomValidity("");
        centerCheckBox.required = false;
        if (document.getElementById("dummy").value === "") {
            centerCheckBox.required = true; //Add the required attribute so that validation message will hover over center of these checkboxes.
            centerCheckBox.setCustomValidity("Please choose best times to call.");
        }
        let frm = document.getElementsByTagName("form")[0];
        let submitBtn = document.getElementById("submitButton");
        if (frm.checkValidity() === false) { //See if the form passes HTML5 validation.
            submitBtn.click();  //Submit the form so that the error message will show.
            return;
        }
        //Form fields pass validation.
        centerCheckBox.required = false;
        centerCheckBox.setCustomValidity("");

        grecaptcha.ready(function () {
            grecaptcha.execute('6LfHsM0UAAAAAMoFKW3RL7qfBgX8pBW5Z5EY2c2Q', { action: 'contactUs' }).then(function (token) {
                //Go find out if the request passes the reCaptcha test.
                document.getElementById("reCaptchaToken").value = token;    //Need to send this with the form post so that the server end can detect hackers bypassing the
                                                                            //reCaptcha validation part.
                $.post(
                    "api/contactus",
                    token)
                .done(function (data) {
                    if (data == false) {
                        return; //Request failed the reCaptcha test.  Leave the user just where they were.  No message.
                    }
                    ////Record the Google Analytics click-through event.
                    //try {
                    //    ga('send', {
                    //        hitType: 'event',
                    //        eventCategory: 'ContactRequest',
                    //        eventAction: 'submit',
                    //        eventLabel: 'contactRequest',
                    //        eventValue: 1
                    //    });
                    //}
                    //catch { }
                    submitBtn.click();  //Submit the form.
                })
                .fail(function (data) {
                    return; //reCaptcha test could not be completed.  Leave user where they were.
                });
            });
        });

    }
}
$(document).ready(function () {
    $("form .form-row :checkbox").on("click", function () {
        _hcb.contactUs.setValueForDummy();
    });
});
