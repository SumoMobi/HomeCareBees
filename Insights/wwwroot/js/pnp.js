/*var _hcb = _hcb || {};
_hcb.pnp = {
    maxPages: number = 3,
    currentPage: function (): number {
        let img = document.getElementById("pnpPage");
        let path = img.attributes["src"].value;
        let pageNumberOffset = "/Pnp?handler=Page&pageNumber=".length;
        let pageNumber = path.substr(pageNumberOffset);
        return pageNumber * 1;
    },
    next: function () {
        let pageNumber = currentPage();
        pageNumber = (pageNumber * 1) + 1;
        if (fileNumber > maxPages) {
            return;
        }
        img.src = "/PnP?handler=Page&pageNumber=" + fileNumber;
        window.scrollTo(0, 0);
    },
    previous: function () {
        let img = document.getElementById("pnpPage");
        let path = img.attributes["src"].value;
        let fileNumberOffset = "/Pnp?handler=Page&pageNumber=".length;
        let fileNumber = path.substr(fileNumberOffset);
        fileNumber = (fileNumber * 1) - 1;
        if (fileNumber < 1) {
            return;
        }
        img.src = "/PnP?handler=Page&pageNumber=" + fileNumber;
        window.scrollTo(0, 0);
    },
    section: function (sectionNbr) {
        let img = document.getElementById("pnpPage");
        let fileNumber;
        switch (sectionNbr) {
            case 0:
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
};*/ 
//# sourceMappingURL=pnp.js.map