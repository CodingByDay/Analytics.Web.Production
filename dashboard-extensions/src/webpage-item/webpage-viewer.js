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
exports.WebPageItem = void 0;
var common_1 = require("devexpress-dashboard/common");
var $ = require("jquery");
var WebPageItem = /** @class */ (function (_super) {
    __extends(WebPageItem, _super);
    function WebPageItem(model, container, options) {
        var _this = _super.call(this, model, container, options) || this;
        _this._iframe = undefined;
        return _this;
    }
    WebPageItem.prototype.renderContent = function (element, changeExisting, afterRenderCallback) {
        var attribute;
        var $element = $(element);
        if (!changeExisting || !this._iframe) {
            this._iframe = $('<iframe>', {
                attr: {
                    width: '100%',
                    height: '100%'
                },
                style: 'border: none;'
            });
            $element.append(this._iframe);
        }
        this.iterateData(function (row) {
            if (!attribute) {
                attribute = row.getDisplayText('Attribute')[0];
            }
        });
        this._iframe.attr('src', this.getPropertyValue('Url').replace('{0}', attribute));
    };
    return WebPageItem;
}(common_1.CustomItemViewer));
exports.WebPageItem = WebPageItem;
