"use strict";
exports.__esModule = true;
exports.WebPageItemExtension = void 0;
var meta_1 = require("./meta");
var icon_1 = require("./icon");
var webpage_viewer_1 = require("./webpage-viewer");
var WebPageItemExtension = /** @class */ (function () {
    function WebPageItemExtension(dashboardControl) {
        this.name = meta_1.WEBPAGE_EXTENSION_NAME;
        this.metaData = meta_1.webPageMeta;
        this.createViewerItem = function (model, element, content) {
            return new webpage_viewer_1.WebPageItem(model, element, content);
        };
        dashboardControl.registerIcon(icon_1.WEBPAGE_ICON);
    }
    return WebPageItemExtension;
}());
exports.WebPageItemExtension = WebPageItemExtension;
