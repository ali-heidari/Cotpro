
Type.registerNamespace("Cotpro.UI.Web");

Cotpro.UI.Web.MultiUI = function (element) {
    Cotpro.UI.Web.MultiUI.initializeBase(this, [element]);

    this._locale = "en-US";
    this._items = null;
};

Cotpro.UI.Web.MultiUI.prototype = {
    initialize:function() {
        otpro.Web.UI.MultiUI.callBaseMethod(this, "initialize");
    },
    dispose: function () {
        otpro.Web.UI.MultiUI.callBaseMethod(this, "dispose");
    },
    get_locale: function () {
        return this._locale;
    },
    set_locale: function (value) {
        this._locale = value;
    },
    get_items: function () {
        return this._items;
    },
    set_items: function (value) {
        this._items = value;
    },
    LoadControl: function (controlAdress) {

    }
};


Cotpro.UI.Web.MultiUI.registerClass('Cotpro.UI.Web.MultiUI', Sys.UI.Control);

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();