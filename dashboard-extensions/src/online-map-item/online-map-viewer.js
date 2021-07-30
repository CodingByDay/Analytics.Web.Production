"use strict";
/// See the DevExtreme documentation to learn more about the Map UI widget settings.
/// https://js.devexpress.com/Documentation/16_2/ApiReference/UI_Widgets/dxMap/
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
exports.OnlineMapItem = void 0;
var common_1 = require("devexpress-dashboard/common");
var map_1 = require("devextreme/ui/map");
var OnlineMapItem = /** @class */ (function (_super) {
    __extends(OnlineMapItem, _super);
    function OnlineMapItem(model, container, options) {
        var _this = _super.call(this, model, container, options) || this;
        _this.mapViewer = null;
        return _this;
    }
    OnlineMapItem.prototype.setSize = function (width, height) {
        _super.prototype.setSize.call(this, width, height);
        var contentWidth = this.contentWidth(), contentHeight = this.contentHeight();
        this.mapViewer.option('width', contentWidth);
        this.mapViewer.option('height', contentHeight);
    };
    OnlineMapItem.prototype.setSelection = function (values) {
        _super.prototype.setSelection.call(this, values);
        this._updateSelection();
    };
    ;
    OnlineMapItem.prototype.clearSelection = function () {
        _super.prototype.clearSelection.call(this);
        this._updateSelection();
    };
    OnlineMapItem.prototype.renderContent = function (element, changeExisting, afterRenderCallback) {
        var _this = this;
        var markers = [], routes = [], mode = this.getPropertyValue('DisplayMode'), showMarkers = mode === 'Markers' || mode === 'MarkersAndRoutes' || this.canMasterFilter(), showRoutes = mode === 'Routes' || mode === 'MarkersAndRoutes';
        if (this.getBindingValue('Latitude').length > 0 && this.getBindingValue('Longitude').length > 0) {
            this.iterateData(function (row) {
                var latitude = row.getValue('Latitude')[0];
                var longitude = row.getValue('Longitude')[0];
                if (latitude && longitude) {
                    if (showMarkers) {
                        markers.push({
                            location: { lat: latitude, lng: longitude },
                            iconSrc: _this.isSelected(row) ? "https://js.devexpress.com/Demos/RealtorApp/images/map-marker.png" : null,
                            onClick: function (args) { _this._onClick(row); },
                            tag: row
                        });
                    }
                    if (showRoutes) {
                        routes.push([latitude, longitude]);
                    }
                }
            });
        }
        var autoAdjust = markers.length > 1 || routes.length > 1, options = {
            provider: this.getPropertyValue('Provider').toLowerCase(),
            type: this.getPropertyValue('Type').toLowerCase(),
            controls: true,
            zoom: autoAdjust ? 1000 : 1,
            autoAdjust: autoAdjust,
            width: this.contentWidth(),
            height: this.contentHeight(),
            // Use the template below to authenticate the application within the required map provider.
            //key: { 
            //    bing: 'BINGAPIKEY',
            //    google: 'GOOGLEAPIKEY'
            //},             
            markers: markers,
            routes: routes.length > 0 ? [{
                    weight: 6,
                    color: 'blue',
                    opacity: 0.5,
                    mode: '',
                    locations: routes
                }] : []
        };
        if (changeExisting && this.mapViewer) {
            this.mapViewer.option(options);
        }
        else {
            this.mapViewer = new (map_1["default"] || window.DevExpress.ui.dxMap)(element, options);
        }
    };
    OnlineMapItem.prototype._onClick = function (row) {
        this.setMasterFilter(row);
        this._updateSelection();
    };
    OnlineMapItem.prototype._updateSelection = function () {
        var _this = this;
        var markers = this.mapViewer.option('markers');
        markers.forEach(function (marker) {
            marker.iconSrc = _this.isSelected(marker.tag) ? "https://js.devexpress.com/Demos/RealtorApp/images/map-marker.png" : null;
        });
        this.mapViewer.option('autoAdjust', false);
        this.mapViewer.option('markers', markers);
    };
    return OnlineMapItem;
}(common_1.CustomItemViewer));
exports.OnlineMapItem = OnlineMapItem;
