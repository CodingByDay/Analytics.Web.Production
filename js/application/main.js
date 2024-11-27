initialPayload = [];
updatedPayload = [];
var viewerApiExtension;
var dashboardControl;
let filterSelections = {};

function onDashboardStateChanged(e) {
    let dashboardId = getCookie("dashboard");
    let dState = dashboard.GetDashboardControl().getDashboardState();
    $.ajax({
        type: "POST",
        url: "IndexTenant.aspx/ProcessStateChanged",
        data: JSON.stringify({ state: dState, dashboard: dashboardId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
        },
        error: function (xhr, status, error) {
        }
    });
}

function onItemCaptionToolbarUpdated(s, e) {
    var control = dashboard.GetDashboardControl();
    design = control.isDesignMode();

    if (!design) {
        var list = dashboard.GetParameters().GetParameterList();
        setCookie("params", JSON.stringify(list), 365);
        if (list.length > 0) {
            window.item_caption = e.Options.staticItems[0].text;
            var parameterized_values = regex_return(item_caption);
            if (parameterized_values.length != 0) {
                parameterized_values.forEach((singular) => {
                    const found = list.find(element => element.Name == singular)
                    indexOfElement = list.indexOf(found)
                    if (found != null && indexOfElement != -1) {
                        text_to_replace = "#" + found.Name
                        try {
                            text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value.toLocaleDateString("uk-Uk")
                        } catch (err) {
                            text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value
                        }
                        window.item_caption = window.item_caption.replace(text_to_replace, text_replace);
                        e.Options.staticItems[0].text = window.item_caption;
                    }
                })
            }
        }
    }
}

var firstUse = true;

function ask(e) {
    if (!firstUse) {
    }
    firstUse = false;
}

function regex_return(text_to_search) {
    var re = /(?:^|\W)#(\w+)(?!\w)/g, match, matches = [];
    while (match = re.exec(text_to_search)) {
        matches.push(match[1]);
    }
    return matches;
}
function customizeWidgets(sender, args) {
    var control = dashboard.GetDashboardControl();
    design = control.isDesignMode();

    if (!design) {
        // On load method
        var parName = []
        var collection = dashboard.GetParameters().GetParameterList();
        if (args.ItemName.startsWith("gridDashboardItem") && collection.length > 0) {
            for (var j = 0; j < collection.length; j++) {
                initialPayload.push(dashboard.GetParameters().GetParameterList()[j].Value);
                parName.push(dashboard.GetParameters().GetParameterList()[j].Name);
            }

            var grid = args.GetWidget();
            var columns = grid.option("columns");
            for (var i = 0; i < columns.length; i++) {
                var textToCheck = columns[i].caption;

                window.textNew = textToCheck;
                var parameterized_values = regex_return(textToCheck);
                if (parameterized_values.length != 0) {
                    parameterized_values.forEach((singular) => {
                        const found = parName.find(element => element == singular)
                        indexOfElement = parName.indexOf(found)

                        if (found != null && indexOfElement != -1) {
                            text_to_replace = "#" + found
                            try {
                                text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value.toLocaleDateString("uk-Uk")
                            } catch (err) {
                                text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value
                            }
                            window.textNew = window.textNew.replace(text_to_replace, text_replace);
                            columns[i].caption = window.textNew;
                        }
                    })
                }
            }
            grid.option("columns", columns);
        }

        if (args.ItemName.startsWith("chart")) {
            var chart = args.GetWidget();
           

            var legend = chart.option("legend");
            legend.customizeText = function (arg) {
                window.item_caption = arg.seriesName;
                var list = dashboard.GetParameters().GetParameterList();
                if (list.length > 0) {
                    var parameterized_values = regex_return(arg.seriesName);
                    if (parameterized_values.length != 0) {
                        parameterized_values.forEach((singular) => {
                            const found = list.find(element => element.Name == singular)
                            indexOfElement = list.indexOf(found)
                            if (found != null && indexOfElement != -1) {
                                text_to_replace = "#" + found.Name
                                try {
                                    text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value.toLocaleDateString("uk-Uk")
                                } catch (err) {
                                    text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value
                                }
                                window.item_caption = window.item_caption.replace(text_to_replace, text_replace);
                            }
                        })
                    }
                }
                var splited = window.item_caption.split(" ");
                splited_removed = removeItemOnce(splited)
                return splited_removed.join(" ");
            }
            chart.option("legend", legend);
        }
    }
}




function removeItemOnce(arr) {
        arr.splice(0, 1);

    return arr;
}
function getSafeParsedCookie(cookieName) {
    let cookieValue = getCookie(cookieName);
    if (!cookieValue) {
        return null;
    }
    try {
        return JSON.parse(cookieValue);
    } catch (error) {
        return null;
    }
}
function updateCustomizeWidgets(sender, args) {


    var control = dashboard.GetDashboardControl();
    design = control.isDesignMode();
    if (!design) {
        var parName = []
        var collection = dashboard.GetParameters().GetParameterList();

        setCookie('old', getCookie("new"));
        setCookie('new', JSON.stringify(collection));

        

        if (args.ItemName.startsWith("gridDashboardItem") && collection.length > 0) {
            initialPayload = [];
            for (var j = 0; j < collection.length; j++) {
                initialPayload.push(dashboard.GetParameters().GetParameterList()[j].Value);
                parName.push(dashboard.GetParameters().GetParameterList()[j].Name);
            }
            var grid = args.GetWidget();
            var columns = grid.option("columns");
            for (var i = 0; i < columns.length; i++) {
                var textToCheck = columns[i].caption;
                window.textNew = textToCheck;
                var parameterized_values = regex_return(textToCheck);
                if (parameterized_values.length != 0) {
                    parameterized_values.forEach((singular) => {
                        const found = parName.find(element => element == singular)
                        indexOfElement = parName.indexOf(found)
                        if (found != null && indexOfElement != -1) {
                            text_to_replace = "#" + found
                            try {
                                text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value.toLocaleDateString("uk-Uk")
                            } catch (err) {
                                text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value
                            }
                            window.textNew = window.textNew.replace(text_to_replace, text_replace);
                            columns[i].caption = window.textNew;
                        }
                    })
                }
            }
            grid.option("columns", columns);
        }


        // Here is the error that causes dynamic parameters to be only changed once.

        var items = dashboard.GetDashboardControl().dashboard().items();
        tabItems = []
        window.counter = 0;

        d_old = getSafeParsedCookie('old');
        d_new = getSafeParsedCookie('new');

        for (var i = 0; i < items.length; i++) {
            var iCurrent = items[i];
            item_caption = iCurrent.name();
            for (var j = 0; j < collection.length; j++) {
                try {
                    var sDate = new Date(d_old[j].Value).toLocaleDateString("uk-Uk");
                    if (iCurrent.name().includes(sDate)) {
                        old_v = new Date(d_old[j].Value).toLocaleDateString("uk-Uk");
                        new_v = new Date(d_new[j].Value).toLocaleDateString("uk-Uk");
                        var nName = iCurrent.name().replace(old_v, new_v);
                        iCurrent.name(nName);
                    }
                } catch {
                    var valField = d_old[j].Value;
                    if (iCurrent.name().includes(valField)) {
                        old_v = d_old[j].Value
                        new_v = new d_new[j].Value;
                        var nName = iCurrent.name().replace(old_v, new_v);
                        iCurrent.name(nName);
                    }
                }
            }
        }


        if (args.ItemName.startsWith("chart")) {
            var chart = args.GetWidget();
            var legend = chart.option("legend");
            legend.customizeText = function (arg) {
                window.item_caption = arg.seriesName;
                var list = dashboard.GetParameters().GetParameterList();
                if (list.length > 0) {
                    var parameterized_values = regex_return(arg.seriesName);
                    if (parameterized_values.length != 0) {
                        parameterized_values.forEach((singular) => {
                            const found = list.find(element => element.Name == singular)
                            indexOfElement = list.indexOf(found)
                            if (found != null && indexOfElement != -1) {
                                text_to_replace = "#" + found.Name
                                try {
                                    text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value.toLocaleDateString("uk-Uk")
                                } catch (err) {
                                    text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value
                                }
                                window.item_caption = window.item_caption.replace(text_to_replace, text_replace);
                            }
                        })
                    }
                }
                var splited = window.item_caption.split(" ");
                splited_removed = removeItemOnce(splited)
                return splited_removed.join(" ");
            }
            chart.option("legend", legend);
        }
        if (args.ItemName.startsWith("gridDashboardItem") && collection.length > 2) {
            d_old = JSON.parse(getCookie('old'));
            d_new = JSON.parse(getCookie('new'));
            var grid = args.GetWidget();
            var columns = grid.option("columns");
            for (var i = 0; i < columns.length; i++) {
                var textToCheck = columns[i].name;
                columns[i].name = "Test";
            }
            grid.option("columns", columns);
        }
    }
}

payload = [];

var extension;

/**
 *
 * @param sender
 */

function onBeforeRender(sender) {
    dashboardControl = sender.GetDashboardControl();
    extension = new DevExpress.Dashboard.DashboardPanelExtension(dashboardControl);
    dashboardControl.surfaceLeft(extension.panelWidth);
    dashboardControl.registerExtension(extension);
    dashboardControl.registerExtension(new SaveAsDashboardExtension(dashboardControl));
    dashboardControl.registerExtension(new DeleteDashboardExtension(sender));
    dashboardControl.unregisterExtension("designerToolbar");
    viewerApiExtension = dashboardControl.findExtension('viewerApi');
    if (viewerApiExtension) {
        viewerApiExtension.on('itemWidgetOptionsPrepared', customizeWidgetOptions);
    }
}

function customizeWidgetOptions(e) {

    if (e.dashboardItem instanceof DevExpress.Dashboard.Model.ChartItem) {

        let contentTemplateBase = e.options.tooltip.contentTemplate;
        e.options.tooltip.contentTemplate = function (info, container) {

            var result = contentTemplateBase(info, container);
            let tooltipText = result.innerHTML;
            window.item_caption = tooltipText;

            var list = dashboard.GetParameters().GetParameterList();

            if (list.length > 0) {

                var parameterized_values = regex_return(tooltipText);
                if (parameterized_values.length != 0) {
                    parameterized_values.forEach((singular) => {

                        const found = list.find(element => element.Name == singular)
                        indexOfElement = list.indexOf(found)
                        if (found != null && indexOfElement != -1) {
                            text_to_replace = "#" + found.Name
                            try {
                                text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value.toLocaleDateString("uk-Uk")
                            } catch (err) {
                                text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value
                            }
                            window.item_caption = window.item_caption.replace(text_to_replace, text_replace);

                        }
                    })
                }

            }

            result.innerHTML = window.item_caption;

            container.append(result);
        }
    }
}





let previousViewerMode = "viewer";
function checkViewerMode() {
    var control = dashboard.GetDashboardControl();
    const currentViewerMode = getCurrentViewerMode();

    if (currentViewerMode !== previousViewerMode) {
        control.loadDashboard(getCookie("dashboard"))
        previousViewerMode = currentViewerMode; // Update the previous mode
    }
}
function getCurrentViewerMode() {
    var control = dashboard.GetDashboardControl();

    design = control.isDesignMode();

    if (design) {
        return "designer";
    } else {
        return "viewer";
    }
}

function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

/**
 * Getting the cookie value.
 * @param cname
 */

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

$(document).keypress(function (e) { if (e.keyCode === 13) { e.preventDefault(); return false; } });

$(function () {
    $(':text').bind('keydown', function (e) { // on keydown for all textboxes
        if (e.keyCode == 13) // if this is enter key

            e.preventDefault();
    });
});

/**
 * Change the visibility of the collapsable hamburger menu. */
function toggleVisibilityHide(toHide) {
    var picture = document.getElementById("pic")
    if (toHide == true) {
        picture.style.visibility = "hidden"
    } else {
        picture.style.visibility = "visible"
    }
}

/* Jquery function to handle hamburger clicked */
$(document).ready(function () {
    $("#pic").mouseover(function () {
        var expand = getCookie("expand");
        if (expand == "true") {
            onExpand();
        } else {
            var control = dashboard.GetDashboardControl();
            design = control.isDesignMode();

            if (design == false) {
                onCollapse();
            }
        }
    });
});

function show() {
    $('.dx-overlay-content').show();
    console.log("Show");
    $(".dx-dashboard-surface").attr('style', 'left: 250px !important');
    changePicStateHideIt(true);
}

function hide() {
    $('.dx-overlay-content').hide();
    console.log("hide");
    $(".dx-dashboard-surface").attr('style', 'left: 10px !important');
    changePicStateHideIt(false);
}

function onExpand() {
    var control = dashboard.GetDashboardControl();
    extension.showPanelAsync({}).done(function (e) {
        control.surfaceLeft(e.surfaceLeft);
        setCookie("expand", "false", 365);
    });
}

function onCollapse() {
    var control = dashboard.GetDashboardControl();
    extension.hidePanelAsync({}).done(function (e) {
        control.surfaceLeft(e.surfaceLeft);
        toggleVisibilityHide(false);
        setCookie("expand", "true", 365);
    });
}

function correctTheLoadingState(s, e) {
    window.currentDashboardId = e.DashboardId;

    var control = dashboard.GetDashboardControl();

    design = control.isDesignMode();

    if (design == false) {
        onCollapse();
    }

    if (!design) {
        var list = dashboard.GetParameters().GetParameterList();
        setCookie('old', JSON.stringify(list))
        setCookie('new', JSON.stringify(list))

        var control = dashboard.GetDashboardControl();
        var items = s.GetDashboardControl().dashboard().items();
        tabItems = []
        window.counter = 0;
        for (var i = 0; i < items.length; i++) {
            var iCurrent = items[i];
            item_caption = iCurrent.name();
            var parameterized_values = regex_return(item_caption);
            if (parameterized_values.length != 0) {


                parameterized_values.forEach((singular) => {
                    const found = list.find(element => element.Name == singular)
                    indexOfElement = list.indexOf(found)

                    if (found != null && indexOfElement != -1) {
                        text_to_replace = "#" + found.Name;
                        try {
                            text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value.toLocaleDateString("uk-Uk")
                        } catch (err) {
                            text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value
                        }
                        window.item_caption = window.item_caption.replace(text_to_replace, text_replace);

                        iCurrent.name(window.item_caption);
                    }
                })
            }
        }
    }
}