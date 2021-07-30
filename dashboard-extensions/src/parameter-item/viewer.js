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
exports.ParameterItemViewer = void 0;
var button_1 = require("devextreme/ui/button");
var buttonsStyle = {
    containerHeight: 60,
    height: 40,
    width: 82,
    marginRight: 15,
    marginTop: 10
};
var common_1 = require("devexpress-dashboard/common");
var ParameterItemViewer = /** @class */ (function (_super) {
    __extends(ParameterItemViewer, _super);
    function ParameterItemViewer(model, container, options, parametersExtension) {
        var _this = _super.call(this, model, container, options) || this;
        _this.buttons = [];
        _this.parametersExtension = parametersExtension;
        _this._subscribeProperties();
        _this.parametersExtension.showDialogButton(false);
        _this.parametersExtension.subscribeToContentChanges(function () {
            _this._generateParametersContent();
        });
        _this.dialogButtonSubscribe = _this.parametersExtension.showDialogButton.subscribe(function () {
            _this.parametersExtension.showDialogButton(false);
        });
        return _this;
    }
    ParameterItemViewer.prototype.setSize = function (width, height) {
        _super.prototype.setSize.call(this, width, height);
        this._setGridHeight();
    };
    ;
    ParameterItemViewer.prototype.dispose = function () {
        _super.prototype.dispose.call(this);
        this.parametersContent && this.parametersContent.dispose && this.parametersContent.dispose();
        this.dialogButtonSubscribe.dispose();
        this.parametersExtension.showDialogButton(true);
        this.buttons.forEach(function (button) { return button.dispose(); });
    };
    ParameterItemViewer.prototype.renderContent = function (dxElement, changeExisting, afterRenderCallback) {
        var _this = this;
        var element = dxElement.jquery ? dxElement.get(0) : dxElement;
        if (!changeExisting) {
            element.innerHTML = '';
            this.buttons.forEach(function (button) { return button.dispose(); });
            element.style.overflow = 'auto';
            this.gridContainer = document.createElement('div');
            element.appendChild(this.gridContainer);
            this._generateParametersContent();
            this.buttonContainer = document.createElement('div');
            this.buttonContainer.style.height = buttonsStyle.containerHeight + 'px',
                this.buttonContainer.style.width = buttonsStyle.width * 2 + buttonsStyle.marginRight * 2 + 'px',
                this.buttonContainer.style.cssFloat = 'right';
            element.appendChild(this.buttonContainer);
            this.buttons.push(this._createButton(this.buttonContainer, "Reset", function () {
                _this.parametersContent.resetParameterValues();
            }));
            this.buttons.push(this._createButton(this.buttonContainer, "Submit", function () {
                _this._submitValues();
            }));
            if (this.getPropertyValue('automaticUpdates') != 'Off')
                this.buttonContainer.style.display = 'none';
        }
    };
    ;
    ParameterItemViewer.prototype._generateParametersContent = function () {
        var _this = this;
        this.parametersContent = this.parametersExtension.renderContent(this.gridContainer);
        this.parametersContent.valueChanged.add(function () { return _this._updateParameterValues(); });
        this._setGridHeight();
        this._update({
            showHeaders: this.getPropertyValue('showHeaders'),
            showParameterName: this.getPropertyValue('showParameterName')
        });
    };
    ParameterItemViewer.prototype._submitValues = function () {
        this.parametersContent.submitParameterValues();
        this._update({
            showHeaders: this.getPropertyValue('showHeaders'),
            showParameterName: this.getPropertyValue('showParameterName')
        });
    };
    ParameterItemViewer.prototype._updateParameterValues = function () {
        this.getPropertyValue('automaticUpdates') != 'Off' ? this._submitValues() : null;
    };
    ParameterItemViewer.prototype._setGridHeight = function () {
        var gridHeight = this.contentHeight();
        if (this.getPropertyValue('automaticUpdates') === 'Off')
            gridHeight -= buttonsStyle.containerHeight;
        this.parametersContent.grid.option('height', gridHeight);
    };
    ParameterItemViewer.prototype._createButton = function (container, buttonText, onClick) {
        var button = document.createElement("div");
        button.style.marginRight = buttonsStyle.marginRight + 'px';
        button.style.marginTop = buttonsStyle.marginTop + 'px';
        container.appendChild(button);
        return new (button_1["default"] || window.DevExpress.ui.dxButton)(button, {
            text: buttonText,
            height: buttonsStyle.height + 'px',
            width: buttonsStyle.width + 'px',
            onClick: onClick
        });
    };
    ParameterItemViewer.prototype._subscribeProperties = function () {
        var _this = this;
        this.subscribe('showHeaders', function (showHeaders) { _this._update({ showHeaders: showHeaders }); });
        this.subscribe('showParameterName', function (showParameterName) { _this._update({ showParameterName: showParameterName }); });
        this.subscribe('automaticUpdates', function (automaticUpdates) { _this._update({ automaticUpdates: automaticUpdates }); });
    };
    ;
    ParameterItemViewer.prototype._update = function (options) {
        var _this = this;
        if (!!options.showHeaders) {
            this.parametersContent.grid.option('showColumnHeaders', options.showHeaders === 'On');
        }
        if (!!options.showParameterName) {
            this.parametersContent.valueChanged.empty();
            this.parametersContent.grid.columnOption(0, 'visible', options.showParameterName === 'On');
            this.parametersContent.valueChanged.add(function () { return _this._updateParameterValues(); });
        }
        if (!!options.automaticUpdates) {
            if (options.automaticUpdates == 'Off') {
                this.buttonContainer.style.display = 'block';
            }
            else {
                this.buttonContainer.style.display = 'none';
            }
        }
        this._setGridHeight();
    };
    ;
    return ParameterItemViewer;
}(common_1.CustomItemViewer));
exports.ParameterItemViewer = ParameterItemViewer;
