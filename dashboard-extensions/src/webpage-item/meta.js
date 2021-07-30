"use strict";
exports.__esModule = true;
exports.webPageMeta = exports.WEBPAGE_EXTENSION_NAME = void 0;
var model_1 = require("devexpress-dashboard/model");
exports.WEBPAGE_EXTENSION_NAME = 'WebPage';
exports.webPageMeta = {
    bindings: [{
            propertyName: 'Attribute',
            dataItemType: 'Dimension',
            array: false,
            displayName: "Attribute",
            emptyPlaceholder: 'Set Attribute',
            selectedPlaceholder: "Configure Attribute"
        }],
    customProperties: [{
            ownerType: model_1.CustomItem,
            propertyName: 'Url',
            valueType: 'string',
            defaultValue: 'https://en.wikipedia.org/wiki/{0}'
        }],
    optionsPanelSections: [{
            title: 'Custom Options',
            items: [{
                    dataField: 'Url',
                    editorType: 'dxTextBox'
                }]
        }],
    icon: exports.WEBPAGE_EXTENSION_NAME,
    title: "Web Page",
    index: 2
};
