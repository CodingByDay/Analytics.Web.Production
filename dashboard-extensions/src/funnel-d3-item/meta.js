"use strict";
exports.__esModule = true;
exports.funnelMeta = exports.FUNNEL_D3_EXTENSION_NAME = void 0;
var model_1 = require("devexpress-dashboard/model");
var designer_1 = require("devexpress-dashboard/designer");
exports.FUNNEL_D3_EXTENSION_NAME = 'FunnelD3';
exports.funnelMeta = {
    bindings: [{
            propertyName: 'Values',
            dataItemType: 'Measure',
            array: true,
            enableColoring: true,
            displayName: 'Values',
            emptyPlaceholder: 'Set Value',
            selectedPlaceholder: 'Configure Value'
        }, {
            propertyName: 'Arguments',
            dataItemType: 'Dimension',
            array: true,
            enableInteractivity: true,
            enableColoring: true,
            displayName: 'Arguments',
            emptyPlaceholder: 'Set Argument',
            selectedPlaceholder: 'Configure Argument'
        }],
    customProperties: [{
            ownerType: model_1.CustomItem,
            propertyName: 'FillType',
            valueType: 'string',
            defaultValue: 'Solid'
        }, {
            ownerType: model_1.CustomItem,
            propertyName: 'IsCurved',
            valueType: 'boolean',
            defaultValue: false
        }, {
            ownerType: model_1.CustomItem,
            propertyName: 'IsDynamicHeight',
            valueType: 'boolean',
            defaultValue: true
        }, {
            ownerType: model_1.CustomItem,
            propertyName: 'PinchCount',
            valueType: 'number',
            defaultValue: 0
        }],
    optionsPanelSections: [{
            title: 'Settings',
            items: [{
                    dataField: 'FillType',
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        items: [{ text: 'Solid' }, { text: 'Gradient' }]
                    }
                }, {
                    dataField: 'IsCurved',
                    label: {
                        text: 'Curved'
                    },
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        keyExpr: 'value',
                        items: [{
                                value: false,
                                text: 'No'
                            }, {
                                value: true,
                                text: 'Yes'
                            }]
                    }
                }, {
                    dataField: 'IsDynamicHeight',
                    label: {
                        text: 'Dynamic Height'
                    },
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        keyExpr: 'value',
                        items: [{
                                value: false,
                                text: 'No'
                            }, {
                                value: true,
                                text: 'Yes'
                            }]
                    }
                }, {
                    dataField: 'PinchCount',
                    editorType: 'dxNumberBox',
                    editorOptions: {
                        min: 0
                    }
                }]
        }],
    interactivity: {
        filter: true,
        drillDown: true
    },
    icon: exports.FUNNEL_D3_EXTENSION_NAME,
    title: 'Funnel D3',
    index: 3
};
