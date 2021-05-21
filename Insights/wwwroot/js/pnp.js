var _hcb = _hcb || {};
_hcb.pnp = {
    maxPages: number = 344,
    setPage: function (pageNumber) {
        let img = document.getElementById("pnpPage");
        img.src = "/PnP?handler=Page&pageNumber=" + pageNumber;
        window.scrollTo(0, 0);
    },
    getCurrentPage: function () {
        let img = document.getElementById("pnpPage");
        let path = img.attributes["src"].value;
        let pageNumberOffset = "/Pnp?handler=Page&pageNumber=".length;
        let pageNumber = path.substr(pageNumberOffset);
        return pageNumber * 1;
    },
    next: function () {
        let pageNumber = getCurrentPage();
        pageNumber++;
        if (pageNumber > maxPages) {
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
        let img = document.getElementById("pnpPage");
        let fileNumber;
        switch (sectionNbr) {
            case 0: //TOC
                fileNumber = 2;
                break;
            case 1:
                fileNumber = 60;
                break;
            case 2:
                fileNumber = 90;
                break;
            case 3:
                fileNumber = 120;
                break;
            case 4:
                fileNumber = 150;
                break;
            case 5:
                fileNumber = 180;
                break;
            case 6:
                fileNumber = 210;
                break;
            case 7:
                fileNumber = 240;
                break;
            case 8:
                fileNumber = 270;
                break;
        }
        img.src = "pnp?handler=Page&pageNumber=" + fileNumber;
        window.scrollTo(0, 0);
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
    }
};