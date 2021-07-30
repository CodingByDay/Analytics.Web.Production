"use strict";
exports.__esModule = true;
exports.ParameterItemExtension = void 0;
var meta_1 = require("./meta");
var viewer_1 = require("./viewer");
var icon_1 = require("./icon");
var ParameterItemExtension = /** @class */ (function () {
    function ParameterItemExtension(dashboardControl) {
        var _this = this;
        this.dashboardControl = dashboardControl;
        this.name = meta_1.PARAMETER_ITEM_EXTENSION_NAME;
        this.metaData = meta_1.parameterItemMeta;
        this.createViewerItem = function (model, element, content) {
            var parametersExtension = _this.dashboardControl.findExtension("dashboard-parameter-dialog");
            if (!parametersExtension) {
                throw Error('The "dashboard-parameter-dialog" extension does not exist. To register this extension, use the DashboardControl.registerExtension method.');
            }
            return new viewer_1.ParameterItemViewer(model, element, content, parametersExtension);
        };
        dashboardControl.registerIcon(icon_1.PARAMETER_ITEM_ICON);
    }
    ParameterItemExtension.prototype.start = function () {
    };
    return ParameterItemExtension;
}());
exports.ParameterItemExtension = ParameterItemExtension;
