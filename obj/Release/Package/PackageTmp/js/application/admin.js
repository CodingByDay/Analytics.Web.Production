initialPayload = [];
updatedPayload = [];
/**
 * A client side event to update the column header titles based on parameter values.
 * @param sender
 * @param args
 */

function onItemCaptionToolbarUpdated(s, e) {
    var list = dashboard.GetParameters().GetParameterList();
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
                    console.log(window.item_caption)
                    e.Options.staticItems[0].text = window.item_caption;
                } 
            })
        }

    }
}






function regex_return(text_to_search) {
    var re = /(?:^|\W)#(\w+)(?!\w)/g, match, matches = [];
    while (match = re.exec(text_to_search)) {
        matches.push(match[1]);
    }
    return matches;
}
function customizeWidgets(sender, args) {

    var parName = []
    var collection = dashboard.GetParameters().GetParameterList();
   
    if (args.ItemName.startsWith("gridDashboardItem") && collection.length > 3) {
        initialPayload.push(dashboard.GetParameters().GetParameterList()[0].Value);
        initialPayload.push(dashboard.GetParameters().GetParameterList()[1].Value);
        initialPayload.push(dashboard.GetParameters().GetParameterList()[2].Value);
        initialPayload.push(dashboard.GetParameters().GetParameterList()[3].Value);
        parName.push(dashboard.GetParameters().GetParameterList()[0].Name);
        parName.push(dashboard.GetParameters().GetParameterList()[1].Name);
        parName.push(dashboard.GetParameters().GetParameterList()[2].Name);
        parName.push(dashboard.GetParameters().GetParameterList()[3].Name);
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
                           console.log(window.textNew)
                           columns[i].caption = window.textNew;
                    } 
                })
            } 

        }
        grid.option("columns", columns);
    }
}


function updatecustomizeWidgets(sender, args) {
    var parName = []
    var collection = dashboard.GetParameters().GetParameterList();
    if (args.ItemName.startsWith("gridDashboardItem") && collection.length > 2) {
        initialPayload = [];
        initialPayload.push(dashboard.GetParameters().GetParameterList()[0].Value);
        initialPayload.push(dashboard.GetParameters().GetParameterList()[1].Value);
        initialPayload.push(dashboard.GetParameters().GetParameterList()[2].Value);
        initialPayload.push(dashboard.GetParameters().GetParameterList()[3].Value);
        parName.push(dashboard.GetParameters().GetParameterList()[0].Name);
        parName.push(dashboard.GetParameters().GetParameterList()[1].Name);
        parName.push(dashboard.GetParameters().GetParameterList()[2].Name);
        parName.push(dashboard.GetParameters().GetParameterList()[3].Name);      
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
                        console.log(window.textNew)
                        columns[i].caption = window.textNew;
                    } 
                })
            } 
        }
        grid.option("columns", columns);
    }
}




payload = [];

var extension;

/**
 *  
 * @param sender
 */

function onBeforeRender(sender) {

    var dashboardControl = sender.GetDashboardControl();
    extension = new DevExpress.Dashboard.DashboardPanelExtension(dashboardControl);
    dashboardControl.surfaceLeft(extension.panelWidth);
    dashboardControl.registerExtension(extension);

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
  
    var control = dashboard.GetDashboardControl();
    //var list = dashboard.GetParameters().GetParameterList();
    //var control = dashboard.GetDashboardControl();
    //var items = s.GetDashboardControl().dashboard().items();
    //tabItems = []

    //for (var i = 0; i < items.length; i++) {
    //   counter = 0;
    //    var iCurrent = items[i];
    //    console.log(iCurrent.name());
    //    if (iCurrent.name().startsWith("Tab Container")) {
    //        counter += 1;
    //        var item = s.GetDashboardControl().dashboard().findItem(`dashboardTabPage${counter}`);
    //        item.name("*Custom caption");
    //    }
        
    //}






    design = control.isDesignMode();
    if (design == false) {
        onCollapse();
    }
}