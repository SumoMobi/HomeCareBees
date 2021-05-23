var _hcb = _hcb || {};
_hcb.pnp = {
    maxPages: number = 344,
    setPage: function (pageNumber) {
        let img = document.getElementById("pnpPage");
        img.src = "/PnP?handler=Page&pageNumber=" + pageNumber;
        let nxt = document.getElementById("Next");
        nxt.disabled = false;
        let prv = document.getElementById("Previous");
        prv.disabled = false;
        if (pageNumber === 1) {
            prv.disabled = true;
        }
        if (pageNumber >= _hcb.pnp.maxPages) {
            nxt.disabled = true;
        }
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
            case 0: //TOC
                pageNumber = 2;
                break;
            case 1:
                pageNumber = 8;
                break;
            case 2:
                pageNumber = 9;
                break;
            case 3:
                pageNumber = 25;
                break;
            case 4:
                pageNumber = 44;
                break;
            case 5:
                pageNumber = 126;
                break;
            case 6:
                pageNumber = 203;
                break;
            case 7:
                pageNumber = 328;
                break;
            case 8:
                pageNumber = 337;
                break;
        }
        _hcb.pnp.setPage(pageNumber);
    },
    toggleHeaderFooter: function () {
        let hdr = document.getElementsByTagName("header")[0];
        let toggle = document.getElementById("Toggle");
        let ftr = document.getElementsByTagName("footer")[0];
        if (hdr.style.display === "") {
            toggle.innerText = "Show header and footer";
            hdr.style.display = "none";
            ftr.style.display = "none";
            return;
        }
        toggle.innerText = "Hide header and footer";
        hdr.style.display = "";
        ftr.style.display = "";
    },
    keydown: function (evt) {
        let page = -1;
    //    if (evt.ctrlKey == false) {
    //        return;
    //    }
    //    if (evt.keyCode === 34) {
    //        page = _hcb.pnp.getCurrentPage() + 1;
    //        _hcb.pnp.setPage(page);
    //        return;
    //    }
    //    if (evt.keyCode === 33) {
    //        page = _hcb.pnp.getCurrentPage() - 1;
    //        _hcb.pnp.setPage(page);
    //        return;
    //    }
    }
};
document.addEventListener("keydown", _hcb.pnp.keydown(event));
