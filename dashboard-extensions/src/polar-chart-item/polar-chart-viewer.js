"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
exports.__esModule = true;
exports.PolarChartItem = void 0;
var common_1 = require("devexpress-dashboard/common");
var $ = require("jquery");
var polar_chart_1 = require("devextreme/viz/polar_chart");
var PolarChartItem = /** @class */ (function (_super) {
    __extends(PolarChartItem, _super);
    function PolarChartItem(model, container, options) {
        var _this = _super.call(this, model, container, options) || this;
        _this.dxPolarWidget = null;
        _this.dxPolarWidgetSettings = null;
        return _this;
    }
    PolarChartItem.prototype._getDataSource = function () {
        var data = [];
        if (this.getBindingValue('measureValue').length > 0) {
            this.iterateData(function (dataRow) {
                var dataItem = {
                    arg: dataRow.getValue('dimensionValue')[0] || "",
                    color: dataRow.getColor()[0],
                    clientDataRow: dataRow
                };
                var measureValues = dataRow.getValue('measureValue');
                for (var i = 0; i < measureValues.length; i++) {
                    dataItem["measureValue" + i] = measureValues[i];
                }
                data.push(dataItem);
            });
        }
        return data;
    };
    PolarChartItem.prototype._getDxPolarWidgetSettings = function () {
        var _this = this;
        var series = [];
        var dataSource = this._getDataSource();
        var measureValueBindings = this.getBindingValue('measureValue');
        for (var i = 0; i < measureValueBindings.length; i++) {
            series.push({ valueField: "measureValue" + i, name: measureValueBindings[i].displayName() });
        }
        return {
            dataSource: dataSource,
            series: series,
            useSpiderWeb: true,
            resolveLabelOverlapping: "hide",
            pointSelectionMode: "multiple",
            commonSeriesSettings: {
                type: "line",
                label: {
                    visible: this.getPropertyValue("labelVisibleProperty")
                }
            },
            "export": {
                enabled: false
            },
            tooltip: {
                enabled: false
            },
            onPointClick: function (e) {
                var point = e.target;
                _this.setMasterFilter(point.data.clientDataRow);
            }
        };
    };
    PolarChartItem.prototype.renderContent = function (element, changeExisting) {
        var htmlElement = element instanceof $ ? element.get(0) : element;
        if (!changeExisting) {
            while (htmlElement.firstChild)
                htmlElement.removeChild(htmlElement.firstChild);
            this.dxPolarWidget = new (polar_chart_1["default"] || window.DevExpress.viz.dxPolarChart)(htmlElement, this._getDxPolarWidgetSettings());
        }
        else {
            this.dxPolarWidget.option(this._getDxPolarWidgetSettings());
        }
        this.updateSelection();
    };
    PolarChartItem.prototype.setSelection = function (values) {
        _super.prototype.setSelection.call(this, values);
        this.updateSelection();
    };
    PolarChartItem.prototype.updateSelection = function () {
        var series = this.dxPolarWidget.getAllSeries();
        for (var i = 0; i < series.length; i++) {
            var points = series[i].getAllPoints();
            for (var j = 0; j < points.length; j++) {
                if (this.isSelected(points[j].data.clientDataRow))
                    points[j].select();
                else
                    points[j].clearSelection();
            }
        }
    };
    PolarChartItem.prototype.clearSelection = function () {
        _super.prototype.clearSelection.call(this);
        this.dxPolarWidget.clearSelection();
    };
    PolarChartItem.prototype.setSize = function (width, height) {
        _super.prototype.setSize.call(this, width, height);
        this.dxPolarWidget.render();
    };
    return PolarChartItem;
}(common_1.CustomItemViewer));
exports.PolarChartItem = PolarChartItem;
