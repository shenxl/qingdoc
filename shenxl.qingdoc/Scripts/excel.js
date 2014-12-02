
$(document).ready(function() {
	
    $.ajax({
        url: "/File/ReadDocument/" + window.location.pathname.split("/")[3],
        success: function (data) {
            var code = data.code;
            if (code == "success")
            {
                var key = data.Key;
                var pagecount = data.pagecount;
                var pages = data.htmlContent;
                $('.navbar-inner .container-fluid .btn-navbar').after('<a class="brand" style="text-decoration: none;" href="/doc/download/' + key + '" title="' + data.FileName + '">' + data.FileName + '</a>');
                var isDropDown = pages.length > 4;
                if (isDropDown) {
                    var dropDownMenu = '<li class="dropdown">' +
                    '<a href="#" class="dropdown-toggle" data-toggle="dropdown">' +
                    '表单' +
                    '<b class="caret"></b>' +
                    '</a>' +
                    '<ul class="dropdown-menu">' +
                    '<!-- DROP DOWN EXCEL TAB TITLE(s) HERE -->' +
                    '</ul>' +
                    '</li>';
                    $('.excel-tab-title').append(dropDownMenu);
                }
                for (i = 0; i < pages.length; i++) {
                    var page = pages[i];
                    // tab navigation & tab content
                    if (0 == i) {
                        $('.excel-tab-title' + (isDropDown ? ' .dropdown .dropdown-menu' : '')).append('<li class="active"><a href="#tab' + (i + 1) + '" data-toggle="tab">' + page.title + '</a></li>');
                        $('.tab-content').append('<div class="tab-pane fade in active" id="tab' + (i + 1) + '">' + page.content + '</div>');
                    } else {
                        $('.excel-tab-title' + (isDropDown ? ' .dropdown .dropdown-menu' : '')).append('<li><a href="#tab' + (i + 1) + '" data-toggle="tab">' + page.title + '</a></li>');
                        $('.tab-content').append('<div class="tab-pane fade in" id="tab' + (i + 1) + '">' + page.content + '</div>');
                    }
                }
                var dropDownMenuHeight = $('.excel-tab-title .dropdown-menu').height();
                var windowHeight = $(window).height();
                if (dropDownMenuHeight > (windowHeight - 80)) {
                    $('.excel-tab-title .dropdown-menu').height(windowHeight - 80);
                    $('.excel-tab-title .dropdown-menu').addClass('pre-scrollable');
                }

                if (document.createStyleSheet) {
                    document.createStyleSheet('<link rel="stylesheet" href="' + data.styleUrl + '" type="text/css" />');
                } else {
                    $("head").append($('<link rel="stylesheet" href="' + data.styleUrl + '" type="text/css" />'));
                }
            }
            clearProgress();
            //$('.tab-content').append(data);
        }
    });

    $("#ExcelTabTitle a").click(function (e) {
        alert("here");
        e.preventDefault();//阻止a链接的跳转行为
        $(this).tab('show');//显示当前选中的链接及关联的content
    })

});