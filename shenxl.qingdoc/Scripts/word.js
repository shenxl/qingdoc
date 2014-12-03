
$(document).ready(function() {
	//文字分页与延时加载逻辑后续实现
    $.ajax({
        url: "/doc/read/" + window.location.pathname.split("/")[3],
        success: function (data) {
            var code = data.code;
            if (code == "success")
            {
                var key = data.Key;
                var pagecount = data.PageCount;
                var pages = data.htmlContent;
                if (pagecount < 3) {
                    $('.bottom-paging-progress').hide();
                    $('.paging-bottom-all').hide();
                }
                $('.navbar-inner .container-fluid .btn-navbar').after('<a class="brand" style="text-decoration: none;" href="/doc/download/' + key + '" title="' + data.FileName + '">' + data.FileName + '</a>');
                clearProgress();
                for (i = 0; i < pages.length; i++) {
                    var page = pages[i];
                    $('.span12 .word-page .word-content').append(page.content);
                    //$('.span12').append('<div class="word-page"><div class="word-content">' + page.content + '</div></div>');
                }
                if (document.createStyleSheet) {
                    document.createStyleSheet('<link rel="stylesheet" href="' + data.styleUrl + '" type="text/css" />');
                } else {
                    $("head").append($('<link rel="stylesheet" href="' + data.styleUrl + '" type="text/css" />'));
                }


                //$(".span12 .word-page .word-content").append(data);
            }

        }
    });
});