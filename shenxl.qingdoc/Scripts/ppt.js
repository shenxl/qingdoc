
var slideUrls = new Array();
var slideThumbUrls = new Array();
var ratio = 0.75;
var curSlide = 1;
$(document).ready(function() {
	$.ajax({
	    url: "/doc/read/" + window.location.pathname.split("/")[3],
	    success: function (data) {
	        var code = data.code;
	        if (code == "success") {
	            var key = data.Key;
	            var pagecount = data.PageCount;
	            var pages = data.htmlContent;
	            $('.container-fluid:first .btn:first').after('<a class="brand" style="text-decoration: none;" href="/doc/download/' + key + '" title="' + data.FileName + '">' + data.FileName + '</a>');

	            // pages
	            var windowW = $(window).width();
	            for (i = 0; i < pages.length; i++) {
	                var page = pages[i];
	                slideUrls[i] = page.url;
	                slideThumbUrls[i] = page.thumbUrl;
	                $('.row-fluid .span2').append('<div class="thumbnail" page="' + (i + 1) + '"><img src="' + page.thumbUrl + '"></div>' + (i + 1) + '/' + pages.length + '<br />');
	                $('#page-selector').append('<option>' + (i + 1) + '</option>');
	            }

	            $('.slide-img').append('<img src="' + slideUrls[0] + '" class="img-polaroid" style="height: 100%;">');

	            resetImgSize();

	            var percent = Math.ceil((curSlide / slideUrls.length) * 100);
	            $('.thumbnail[page="' + curSlide + '"]').addClass('ppt-thumb-border');
	            $('#page-selector').val(curSlide);
	            $('.bottom-paging-progress .bar').width('' + percent + '%');

	            $('.thumbnail').click(function () {
	                var page_num = $(this).attr('page');
	                gotoSlide(page_num);
	            });
	        }else {
	            $('.container-fluid .row-fluid').html('<section><div class="alert alert-error">' + data.desc + '</div></section>');
	        }	
	        clearProgress();
	    }

	});
	//$('.fullscreen-link').toggle($(document).fullScreen() != null);
	//$('.fullscreen-link').click(function(){
	//    $('.slide-img').fullScreen(true);
	////});
	//$(document).bind("fullscreenchange", function() {
	//    var isFullscreen = $(document).fullScreen() ? true : false;
	//    if (isFullscreen) {
	//        $('.slide-img').css('background-color', 'black');
	//        $('.slide-img').contextMenu(true);
	//    } else {
	//        $('.slide-img').css('background-color', '');
	//        $('.slide-img').contextMenu(false);
	//    }
	//});
	
	$('#page-selector').change(function() {
	    var selectNum = $("#page-selector option:selected").text();
	    gotoSlide(selectNum);
	});
	$('.slide-img .ppt-turn-left-mask').click(function () {
	    preSlide();
	});
	$('.slide-img .ppt-turn-right-mask').click(function () {
	    nextSlide();
	});
	
	//$('.slide-img').contextMenu(false);
	
    // Swipe method is NOT supported in IE6, so it should be the last one.
	//$('.slide-img').swipeleft(function() { nextSlide(); });
	//$('.slide-img').swiperight(function() { preSlide(); });

});


$(window).resize(function() {
	resetImgSize();
});

function resetImgSize() {
	var leftW = $('.row-fluid .span2').width() + 20;
	var windowW = $(window).width();
	if (windowW < 768) {
		leftW = -40;
	}
	var ww = $(window).width() - 90 - leftW;
	var wh = $(window).height() - 90;
	if (wh / ww < ratio) {
		$('.slide-img').height(wh);
		$('.slide-img').width(wh / ratio);
	} else {
		$('.slide-img').width(ww);
		$('.slide-img').height(ww * ratio);
	}
}

$(document).keydown(function(event){
	if (event.keyCode == 37 || event.keyCode == 38) {
		preSlide();
	} else if (event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 32){
		nextSlide();
	} else if (event.keyCode == 13) {
		$('.slide-img').toggleFullScreen();
	}
});

function getCurSlide() {
	return curSlide;
}

function preSlide() {
	var preSlide = eval(Number(getCurSlide()) - 1);
	gotoSlide(preSlide);
}

function nextSlide() {
	var nextSlide = eval(Number(getCurSlide()) + 1);
	gotoSlide(nextSlide);
}

function gotoSlide(slide) {
	var slideSum = slideUrls.length;
	if (slide <= 0) {
		slide = 1;
	} else if (slideSum < slide) {
		slide = slideSum;
	}
	curSlide = slide;
	/*
	$(".slide-img img").fadeOut(function() {
		$(this).attr("src", slideUrls[slide - 1]).fadeIn();
	});
	*/
	$(".slide-img img").attr("src", slideUrls[slide - 1]);
	var percent = Math.ceil((curSlide / slideUrls.length) * 100);
	$('.thumbnail').removeClass('ppt-thumb-border');
	$('.thumbnail[page="' + slide + '"]').addClass('ppt-thumb-border');
	$('#page-selector').val(slide);
	$('.bottom-paging-progress .bar').width('' + percent + '%');
}