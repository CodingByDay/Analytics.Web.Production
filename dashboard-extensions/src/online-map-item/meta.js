"use strict";
exports.__esModule = true;
exports.onlineMapMeta = exports.ONLINE_MAP_EXTENSION_NAME = void 0;
var model_1 = require("devexpress-dashboard/model");
var designer_1 = require("devexpress-dashboard/designer");
exports.ONLINE_MAP_EXTENSION_NAME = 'OnlineMap';
exports.onlineMapMeta = {
    bindings: [{
            propertyName: 'Latitude',
            dataItemType: 'Dimension',
            array: false,
            enableInteractivity: true,
            displayName: 'Latitude',
            emptyPlaceholder: 'Set Latitude',
            selectedPlaceholder: 'Configure Latitude',
            constraints: {
                allowedTypes: ['Integer', 'Float', 'Double', 'Decimal']
            }
        }, {
            propertyName: 'Longitude',
            dataItemType: 'Dimension',
            array: false,
            enableInteractivity: true,
            displayName: 'Longitude',
            emptyPlaceholder: 'Set Longitude',
            selectedPlaceholder: 'Configure Longitude',
            constraints: {
                allowedTypes: ['Integer', 'Float', 'Double', 'Decimal']
            }
        }],
    customProperties: [{
            ownerType: model_1.CustomItem,
            propertyName: 'Provider',
            valueType: 'string',
            defaultValue: 'Bing'
        }, {
            ownerType: model_1.CustomItem,
            propertyName: 'Type',
            valueType: 'string',
            defaultValue: 'RoadMap'
        }, {
            ownerType: model_1.CustomItem,
            propertyName: 'DisplayMode',
            valueType: 'string',
            defaultValue: 'Markers'
        }],
    optionsPanelSections: [{
            title: 'Custom Options',
            items: [{
                    dataField: 'Provider',
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        items: [{ text: 'Google' }, { text: 'Bing' }]
                    }
                }, {
                    dataField: 'Type',
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        items: [{ text: 'RoadMap' }, { text: 'Satellite' }, { text: 'Hybrid' }]
                    }
                }, {
                    dataField: 'DisplayMode',
                    template: designer_1.FormItemTemplates.buttonGroup,
                    editorOptions: {
                        keyExpr: 'value',
                        items: [{
                                value: 'Markers',
                                text: 'Markers'
                            }, {
                                value: 'Routes',
                                text: 'Routes'
                            }, {
                                value: 'MarkersAndRoutes',
                                text: 'All'
                            }]
                    }
                }]
        }],
    interactivity: {
        filter: true,
        drillDown: false
    },
    icon: exports.ONLINE_MAP_EXTENSION_NAME,
    title: 'Online Map',
    index: 1
};
