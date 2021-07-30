"use strict";
exports.__esModule = true;
exports.FunnelD3ItemExtension = void 0;
var icon_1 = require("./icon");
var funnel_d3_viewer_1 = require("./funnel-d3-viewer");
var meta_1 = require("./meta");
var FunnelD3ItemExtension = /** @class */ (function () {
    function FunnelD3ItemExtension(dashboardControl) {
        this.name = meta_1.FUNNEL_D3_EXTENSION_NAME;
        this.metaData = meta_1.funnelMeta;
        dashboardControl.registerIcon(icon_1.FUNNEL_D3_ICON);
    }
    FunnelD3ItemExtension.prototype.createViewerItem = function (model, element, content) {
        return new funnel_d3_viewer_1.FunnelD3Item(model, element, content);
    };
    return FunnelD3ItemExtension;
}());
exports.FunnelD3ItemExtension = FunnelD3ItemExtension;
