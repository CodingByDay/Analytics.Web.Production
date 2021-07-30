"use strict";
exports.__esModule = true;
exports.PolarChartItemExtension = void 0;
var icon_1 = require("./icon");
var polar_chart_viewer_1 = require("./polar-chart-viewer");
var meta_1 = require("./meta");
var PolarChartItemExtension = /** @class */ (function () {
    function PolarChartItemExtension(dashboardControl) {
        this.name = meta_1.POLAR_CHART_EXTENSION_NAME;
        this.metaData = meta_1.polarMeta;
        dashboardControl.registerIcon(icon_1.POLAR_CHART_ICON);
    }
    PolarChartItemExtension.prototype.createViewerItem = function (model, element, content) {
        return new polar_chart_viewer_1.PolarChartItem(model, element, content);
    };
    return PolarChartItemExtension;
}());
exports.PolarChartItemExtension = PolarChartItemExtension;
