"use strict";
exports.__esModule = true;
exports.polarMeta = exports.POLAR_CHART_EXTENSION_NAME = void 0;
var model_1 = require("devexpress-dashboard/model");
exports.POLAR_CHART_EXTENSION_NAME = 'PolarChart';
exports.polarMeta = {
    bindings: [{
            propertyName: 'measureValue',
            dataItemType: 'Measure',
            displayName: 'Value',
            array: true,
            emptyPlaceholder: 'Set Value',
            selectedPlaceholder: 'Configure Value'
        }, {
            propertyName: 'dimensionValue',
            dataItemType: 'Dimension',
            displayName: 'Argument',
            array: false,
            enableColoring: true,
            enableInteractivity: true,
            emptyPlaceholder: 'Set Argument',
            selectedPlaceholder: 'Configure Argument'
        }],
    interactivity: {
        filter: true
    },
    customProperties: [{
            ownerType: model_1.CustomItem,
            propertyName: 'labelVisibleProperty',
            valueType: 'boolean',
            defaultValue: true
        }],
    optionsPanelSections: [{
            title: 'Labels',
            items: [{
                    dataField: 'labelVisibleProperty',
                    label: {
                        text: 'Display labels'
                    }
                }]
        }],
    icon: exports.POLAR_CHART_EXTENSION_NAME,
    title: 'Polar Chart',
    index: 2
};
