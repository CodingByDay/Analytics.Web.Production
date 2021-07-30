"use strict";
// Third party libraries: 
// "d3js" component from https://d3js.org/ [Copyright(c) 2017 Mike Bostock]
// "d3-funnel" component from https://jakezatecky.github.io/d3-funnel/ [Copyright(c) 2017 Jake Zatecky]
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
exports.FunnelD3Item = void 0;
var common_1 = require("devexpress-dashboard/common");
var D3Funnel = require("d3-funnel");
var $ = require("jquery");
var FunnelD3Item = /** @class */ (function (_super) {
    __extends(FunnelD3Item, _super);
    function FunnelD3Item(model, container, options) {
        var _this = _super.call(this, model, container, options) || this;
        _this.funnelSettings = undefined;
        _this.funnelViewer = null;
        _this.selectionValues = [];
        _this.exportingImage = new Image();
        _this._subscribeProperties();
        return _this;
    }
    FunnelD3Item.prototype.renderContent = function (element, changeExisting) {
        var htmlElement = element instanceof $ ? element.get(0) : element;
        var data = this._getDataSource();
        if (!this._ensureFunnelLibrary(htmlElement))
            return;
        if (!!data) {
            if (!changeExisting || !this.funnelViewer) {
                while (htmlElement.firstChild)
                    htmlElement.removeChild(htmlElement.firstChild);
                this.funnelContainer = document.createElement('div');
                this.funnelContainer.style.margin = '20px';
                this.funnelContainer.style.height = 'calc(100% - 40px)';
                htmlElement.appendChild(this.funnelContainer);
                this.funnelViewer = new D3Funnel(this.funnelContainer);
            }
            this._update(data, this._getFunnelSizeOptions());
        }
        else {
            while (htmlElement.firstChild)
                htmlElement.removeChild(htmlElement.firstChild);
            this.funnelViewer = null;
        }
    };
    ;
    FunnelD3Item.prototype.setSize = function (width, height) {
        _super.prototype.setSize.call(this, width, height);
        this._update(null, this._getFunnelSizeOptions());
    };
    ;
    FunnelD3Item.prototype.setSelection = function (values) {
        _super.prototype.setSelection.call(this, values);
        this._update(this._getDataSource());
    };
    ;
    FunnelD3Item.prototype.clearSelection = function () {
        _super.prototype.clearSelection.call(this);
        this._update(this._getDataSource());
    };
    ;
    FunnelD3Item.prototype.allowExportSingleItem = function () {
        return !this._isIEBrowser();
    };
    ;
    FunnelD3Item.prototype.getExportInfo = function () {
        if (this._isIEBrowser())
            return;
        return {
            image: this._getImageBase64()
        };
    };
    ;
    FunnelD3Item.prototype._getFunnelSizeOptions = function () {
        if (!this.funnelContainer)
            return {};
        return { chart: { width: this.funnelContainer.clientWidth, height: this.funnelContainer.clientHeight } };
    };
    ;
    FunnelD3Item.prototype._getDataSource = function () {
        var _this = this;
        var bindingValues = this.getBindingValue('Values');
        if (bindingValues.length == 0)
            return undefined;
        var data = [];
        this.iterateData(function (dataRow) {
            var values = dataRow.getValue('Values');
            var valueStr = dataRow.getDisplayText('Values');
            var color = dataRow.getColor('Values');
            if (_this._hasArguments()) {
                var labelText = dataRow.getDisplayText('Arguments').join(' - ') + ': ' + valueStr;
                data.push([{ data: dataRow, text: labelText, color: color[0] }].concat(values)); //0 - 'layer' index for color value
            }
            else {
                data = values.map(function (value, index) { return [{ text: bindingValues[index].displayName() + ': ' + valueStr[index], color: color[index] }, value]; });
            }
        });
        return data.length > 0 ? data : undefined;
    };
    ;
    FunnelD3Item.prototype._ensureFunnelLibrary = function (htmlElement) {
        if (!D3Funnel) {
            htmlElement.innerHTML = '';
            var textDiv = document.createElement('div');
            textDiv.style.position = 'absolute';
            textDiv.style.top = '50%';
            textDiv.style.transform = 'translateY(-50%)';
            textDiv.style.width = '95%';
            textDiv.style.color = '#CF0F2E';
            textDiv.style.textAlign = 'center';
            textDiv.innerText = "'D3Funnel' cannot be displayed. You should include 'd3.v3.min.js' and 'd3-funnel.js' libraries.";
            htmlElement.appendChild(textDiv);
            return false;
        }
        return true;
    };
    ;
    FunnelD3Item.prototype._ensureFunnelSettings = function () {
        var _this = this;
        var getSelectionColor = function (hexColor) { return _this.funnelViewer.colorizer.shade(hexColor, -0.5); };
        if (!this.funnelSettings) {
            this.funnelSettings = {
                data: undefined,
                options: {
                    chart: {
                        bottomPinch: this.getPropertyValue('PinchCount'),
                        curve: { enabled: this.getPropertyValue('IsCurved') }
                    },
                    block: {
                        dynamicHeight: this.getPropertyValue('IsDynamicHeight'),
                        fill: {
                            scale: function (index) {
                                var obj = _this.funnelSettings.data[index][0];
                                return obj.data && _this.isSelected(obj.data) ? getSelectionColor(obj.color) : obj.color;
                            },
                            type: this.getPropertyValue('FillType').toLowerCase()
                        }
                    },
                    label: {
                        format: function (label, value) {
                            return label.text;
                        }
                    },
                    events: {
                        click: { block: function (e) { return _this._onClick(e); } }
                    }
                }
            };
        }
        this.funnelSettings.options.block.highlight = this.canDrillDown() || this.canMasterFilter();
        return this.funnelSettings;
    };
    ;
    FunnelD3Item.prototype._onClick = function (e) {
        if (!this._hasArguments() || !e.label)
            return;
        var row = e.label.raw.data;
        if (this.canDrillDown(row))
            this.drillDown(row);
        else if (this.canMasterFilter(row)) {
            this.setMasterFilter(row);
            this._update();
        }
    };
    ;
    FunnelD3Item.prototype._subscribeProperties = function () {
        var _this = this;
        this.subscribe('IsCurved', function (isCurved) { return _this._update(null, { chart: { curve: { enabled: isCurved } } }); });
        this.subscribe('IsDynamicHeight', function (isDynamicHeight) { return _this._update(null, { block: { dynamicHeight: isDynamicHeight } }); });
        this.subscribe('PinchCount', function (count) { return _this._update(null, { chart: { bottomPinch: count } }); });
        this.subscribe('FillType', function (type) { return _this._update(null, { block: { fill: { type: type.toLowerCase() } } }); });
    };
    ;
    FunnelD3Item.prototype._update = function (data, options) {
        this._ensureFunnelSettings();
        if (!!data) {
            this.funnelSettings.data = data;
        }
        if (!!options) {
            $.extend(true, this.funnelSettings.options, options);
        }
        if (!!this.funnelViewer) {
            this.funnelViewer.draw(this.funnelSettings.data, this.funnelSettings.options);
            this._updateExportingImage();
        }
    };
    ;
    FunnelD3Item.prototype._updateExportingImage = function () {
        var svg = this.funnelContainer.firstElementChild, str = new XMLSerializer().serializeToString(svg), encodedData = 'data:image/svg+xml;base64,' + window.btoa(window["unescape"](encodeURIComponent(str)));
        this.exportingImage.src = encodedData;
    };
    ;
    FunnelD3Item.prototype._hasArguments = function () {
        return this.getBindingValue('Arguments').length > 0;
    };
    ;
    FunnelD3Item.prototype._getImageBase64 = function () {
        var canvas = document.createElement('canvas');
        ;
        canvas.width = this.funnelContainer.clientWidth;
        canvas.height = this.funnelContainer.clientHeight;
        canvas.getContext('2d').drawImage(this.exportingImage, 0, 0);
        return canvas.toDataURL().replace('data:image/png;base64,', '');
    };
    FunnelD3Item.prototype._isIEBrowser = function () {
        return navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0;
    };
    return FunnelD3Item;
}(common_1.CustomItemViewer));
exports.FunnelD3Item = FunnelD3Item;
