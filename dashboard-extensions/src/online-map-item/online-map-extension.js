"use strict";
exports.__esModule = true;
exports.OnlineMapItemExtension = void 0;
var meta_1 = require("./meta");
var online_map_viewer_1 = require("./online-map-viewer");
var icon_1 = require("./icon");
var OnlineMapItemExtension = /** @class */ (function () {
    function OnlineMapItemExtension(dashboardControl) {
        this.name = meta_1.ONLINE_MAP_EXTENSION_NAME;
        this.metaData = meta_1.onlineMapMeta;
        dashboardControl.registerIcon(icon_1.ONLINE_MAP_ICON);
    }
    OnlineMapItemExtension.prototype.createViewerItem = function (model, element, content) {
        return new online_map_viewer_1.OnlineMapItem(model, element, content);
    };
    ;
    return OnlineMapItemExtension;
}());
exports.OnlineMapItemExtension = OnlineMapItemExtension;
