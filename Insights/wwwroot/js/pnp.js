var _hcb = _hcb || {};
_hcb.pnp = {
    maxPages: number = 344,
    getCurrentPage: function () {
        let img = document.getElementById("pnpPage");
        let path = img.attributes["src"].value;
        let pageNumberOffset = "/Pnp?handler=Page&pageNumber=".length;
        let pageNumber = path.substr(pageNumberOffset);
        return pageNumber * 1;
    },
    next: function () {
        let pageNumber = _hcb.pnp.getCurrentPage();
        pageNumber++;
        if (pageNumber > _hcb.pnp.maxPages) {
            return;
        }
        _hcb.pnp.setPage(pageNumber);
    },
    previous: function () {
        let pageNumber = _hcb.pnp.getCurrentPage();
        pageNumber--;
        if (pageNumber < 1) {
            return;
        }
        _hcb.pnp.setPage(pageNumber);
    },
    section: function (sectionNbr) {
        let pageNumber;
        switch (sectionNbr) {
            case "Table of Contents":
                pageNumber = 2;
                break;
            case "1: Introduction":
                pageNumber = 8;
                break;
            case "2: Organization and Administration":
                pageNumber = 9;
                break;
            case "3: Scope of Services":
                pageNumber = 25;
                break;
            case "4: Service Delivery and Client Care":
                pageNumber = 44;
                break;
            case "5: Human Resources":
                pageNumber = 126;
                break;
            case "6: Health and Safety":
                pageNumber = 203;
                break;
            case "7: Financial Management":
                pageNumber = 328;
                break;
            case "8: Quality and Risk Management":
                pageNumber = 337;
                break;
        }
        _hcb.pnp.setPage(pageNumber);
    },
    toggleHeaderFooter: function () {
        let hdr = document.getElementsByTagName("header")[0];
        let toggle = document.getElementById("Toggle");
        let ftr = document.getElementsByTagName("footer")[0];
        if (hdr.style.display == "") {
            toggle.innerText = "Show header and footer";
            hdr.style.display = "none";
            ftr.style.display = "none";
            return;
        }
        toggle.innerText = "Hide header and footer";
        hdr.style.display = "";
        ftr.style.display = "";
    },
    setPage: function (pageNumber) {
        let img = document.getElementById("pnpPage");
        img.src = "/PnP?handler=Page&pageNumber=" + pageNumber;
        window.scrollTo(0, 0);
    }
};