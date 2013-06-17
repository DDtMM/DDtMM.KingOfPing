/// <reference path="http://code.jquery.com/jquery-1.9.1.js" /> 
var refreshTimerID;
var defaultRefreshDuration = 12000;
var templates = {};

$(document).ready(function () {
    $.ajaxSetup({ cache: false });

    //$('script[type="text/template"]').each(function () {
    //    var $this = $(this);
    //    templates[$this.attr('id')] = $this.html().trim();
    //});
    simpleTemplates.scanForTemplates();
    $('#RefreshButton').click(displaySites);
    $('#RunNowButton').click(runPing);
    $('#autoRefreshInterval').change(setRefreshTimer);
    //$.get('PingService.svc/GetNextRunTime')
    //.done(function (data) { console.log(moment(data)) });
    dgStorage.trackElement($('#autoRefreshInterval')[0]);
    setRefreshTimer();
    displaySites();
});

function updateRefresh() {
    $('#lastRefreshLabel').html('Last Refreshed at ' + moment(Date.now()).format('HH:mm:ss.SSS'));
}

function setRefreshTimer() {
    defaultRefreshDuration = $('#autoRefreshInterval').val();

}

function startDisplaySitesRefreshTimer(timeOutDuration) {
    if (timeOutDuration === undefined) {
        timeOutDuration = defaultRefreshDuration;
    }
    if (refreshTimerID !== null) clearTimeout(refreshTimerID);
    refreshTimerID = setTimeout(displaySites, timeOutDuration);
}

function runPing($targetElement) {
    $.post('PingService.svc/RunNow')
    .done(function (data) {
        displaySites();
       
    })
    .fail(function (data) {
        console.log(data);
    });
}
function formatBlankForNull(text) {
    return (text == null) ? '' : text;
}

function formatDate(isoDate) {
    if (isoDate !== null) {
        return moment(isoDate).calendar();
    }
    return '';
}

//function renderTemplate(data, template, templates, transformFunctions) {
//    var transform;
    
//    return template.replace(/\{\{([^:\}]+)(:{0,1})(.*?)\}\}/g, function (match, key, colon, templateId) {

//        if (templateId != "") {
//            var output = "";
//            for (var i in data[key]) {
//                output += renderTemplate(data[key][i], templates[templateId], templates, transformFunctions);
//            }
//            return output;
//        }
//        return ((transform = transformFunctions[key]) != null) ? transform(data[key], data) : data[key];
//    });
//}


function displayLogs(siteID) {

}

function displaySites() {
    startDisplaySitesRefreshTimer();
    var rootTemplateID = 'siteRowTemplate';
    $targetElement = $('#siteTable tbody');
    $targetElement.empty();
    $.get('PingService.svc/GetSites')
        .done(function (data) {
            //console.log(data);
            var output = simpleTemplates.renderTemplate(data, simpleTemplates.templates['siteTableTemplate']);
            //console.log(output);
            $('siteTable').html(
                
                simpleTemplates.renderTemplate(data, simpleTemplates.templates['siteTableTemplate'])
                );

            //siteRowTemplate = $("#siteRowTemplate").html().trim();
            //var transformFunctions = { };
            //var transform;
            //var value;

            //transformFunctions['StartTime'] = formatDate;
            //transformFunctions['EndTime'] = formatDate;
            //transformFunctions['RunTime'] = formatDate;
            //transformFunctions['NextRun'] = formatDate;
            //transformFunctions['Message'] = formatBlankForNull;

            //for (var i in data) {
            //    var site = data[i];
            //    $targetElement.append($(renderTemplate(site, templates[rootTemplateID],
            //        templates, transformFunctions)));
            //}
            //$targetElement.find(".topLevel:odd").addClass("odd");
            //$targetElement.find(".odd").next(1).addClass('odd');
            updateRefresh();
        })
        .fail(function (data) {
            console.log('fail');
            console.log(data);
        });
}