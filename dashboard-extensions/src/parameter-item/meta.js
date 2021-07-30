"use strict";
exports.__esModule = true;
exports.parameterItemMeta = exports.PARAMETER_ITEM_EXTENSION_NAME = void 0;
var model_1 = require("devexpress-dashboard/model");
var designer_1 = require("devexpress-dashboard/designer");
var onOffButtons = [{ text: 'On' }, { text: 'Off' }];
exports.PARAMETER_ITEM_EXTENSION_NAME = 'ParameterItem';
exports.parameterItemMeta = {
    customProperties: [{
            ownerType: model_1.CustomItem,
            propertyName: 'showHeaders',
            valueType: 'string',
            defaultValue: 'On'
        }, {
            ownerType: model_1.CustomItem,
            propertyName: 'showParameterName',
            valueType: 'string',
            defaultValue: 'On'
        }, {
            ownerType: model_1.CustomItem,
            propertyName: 'automaticUpdates',
            valueType: 'string',
            defaultValue: 'Off'
        }],
    optionsPanelSections: [{
            title: 'Parameters settings',
            items: [{
                    dataField: 'showHeaders',
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        items: onOffButtons
                    }
                }, {
                    dataField: 'showParameterName',
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        items: onOffButtons
                    }
                }, {
                    dataField: 'automaticUpdates',
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        items: onOffButtons
                    }
                }]
        }],
    icon: exports.PARAMETER_ITEM_EXTENSION_NAME,
    title: "Parameters"
};
