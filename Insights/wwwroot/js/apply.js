var _hcb = _hcb || {};
_hcb.apply = {
    submitClicked: function (evt) {
        //Validate shifts
        let shifts = document.getElementById("shiftAvailability").getElementsByTagName("INPUT");
        let centerCheckBox = shifts[Math.floor(shifts.length / 2)]; //Selecting a checkbox somewhere in the middle of the set.
        centerCheckBox.required = false;
        centerCheckBox.setCustomValidity("");
        let selectedChecks = document.querySelectorAll("#shiftAvailability input[type='checkbox']:checked");
        if (selectedChecks.length === 0) {
            centerCheckBox.required = true; //Add the required attribute so that validation message will hover over center of these checkboxes.
            centerCheckBox.setCustomValidity("Please choose available shifts.");
        }

        //Validate Signature
        let theSignature = document.getElementById("signature");
        theSignature.setCustomValidity("");
        theSignature.required = false;
        if (theSignature.value.toLowerCase() !== document.getElementById("firstName").value.toLowerCase() + " " + document.getElementById("lastName").value.toLowerCase()) {
            theSignature.required = true;
            theSignature.setCustomValidity("Signature does not match first and last name.");
        }

        let frm = document.getElementsByTagName("form")[0];
        let submitBtn = document.getElementById("submitButton");
        if (frm.checkValidity() === false) { //See if the form passes HTML5 validation.
            submitBtn.click();  //Submit the form so that the error message will show.
            return;
        }
        //Form fields pass validation.
        centerCheckBox.setCustomValidity("");
        centerCheckBox.required = false;
        theSignature.setCustomValidity("");
        theSignature.required = false;

        grecaptcha.ready(function () {
            grecaptcha.execute('6LfHsM0UAAAAAMoFKW3RL7qfBgX8pBW5Z5EY2c2Q', { action: 'apply' }).then(function (token) {
                //Go find out if the request passes the reCaptcha test.
                document.getElementById("reCaptchaToken").value = token;    //Need to send this with the form post so that the server end can detect hackers bypassing the
                //reCaptcha validation part.
                var client = new XMLHttpRequest();
                client.open("POST", "/ReCaptcha", true);
                //                client.setRequestHeader("__requestverificationtoken", document.querySelector('input[name="__RequestVerificationToken"]').value);
                client.setRequestHeader("content-type", "application/x-www-form-urlencoded")
                client.onreadystatechange = function () {
                    if (this.readyState !== 4) {
                        return;
                    }
                    if (this.status !== 200) {
                        return;
                    }
                    // Typical action to be performed when the document is ready:
                    if (this.responseText === "false") {
                        return;
                    }
                    submitBtn.click();  //Submit the form.
                };
                client.send("Token=" + token + "&__RequestVerificationToken=" + document.querySelector('input[name="__RequestVerificationToken"]').value);

                //$.post(
                //    "/ReCaptcha",
                //    token)
                //.done(function (data) {
                //    if (data == false) {
                //        return; //Request failed the reCaptcha test.  Leave the user just where they were.  No message.
                //    }
                //    ////Record the Google Analytics click-through event.
                //    //try {
                //    //    ga('send', {
                //    //        hitType: 'event',
                //    //        eventCategory: 'ContactRequest',
                //    //        eventAction: 'submit',
                //    //        eventLabel: 'contactRequest',
                //    //        eventValue: 1
                //    //    });
                //    //}
                //    //catch { }
                //    submitBtn.click();  //Submit the form.
                //})
                //.fail(function (data) {
                //    return; //reCaptcha test could not be completed.  Leave user where they were.
                //});
            });
        });

    }
}
//$(document).ready(function () {
//    $("form .form-row :checkbox").on("click", function () {
//        _hcb.contactUs.setValueForDummy();
//    });
//});
