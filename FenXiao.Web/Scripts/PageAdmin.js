﻿(function ($) {

    var thInit = function () {
        for (var i = 0; i < $('.ImageBox .ImageArticle').length; i++) {
            $('.ImageBox .ImageArticle .nav .th').eq(i).html(i + 1);
        }
    }



    var navInit = function () {
        thInit();

        $('.ImageBox .ImageArticle .nav .icon-prev, .ImageBox .ImageArticle .nav .icon-next').show();

        $('.ImageBox .ImageArticle .nav .icon-prev').first().hide();

        $('.ImageBox .ImageArticle .nav .icon-next').last().hide();

    };

    $('.ImageArticle .nav .icon-prev').click(function () {
        var box = $(this).parent().parent();
        $(box).insertBefore(box.prev('.ImageArticle'));
        navInit();
    });

    $('.ImageArticle .nav .icon-next').click(function () {
        var box = $(this).parent().parent();
        $(box).insertAfter(box.next('.ImageArticle'));
        navInit();
    });

    $('.ImageArticle .nav .icon-remove').click(function () {
        $(this).parent().parent().remove();
        navInit();
    });

    navInit();

    var uploaders = [];

    var addImageArticle = function (src) {
        var box = $('.ImageArticle.template.hide').clone(true);
        var bg = box.find('.bgContainer');
        bg.find('input').val(src);
        bg.find('img ').attr('src', src);
        box.removeClass('hide').removeClass('template');
        var id = 'subTitleUploader' + uploaders.length;
        console.log(id);
        box.find('.addSubtitle').attr('id', id);
        box.insertBefore($('#uploader'));
        addSubtitleUploaderInit('#' + id);
        
        
        navInit();
    }



    var AudioUploader = new WebUploader.Uploader({
        swf: 'webuploader/Uploader.swf',
        server: '/Home/Upload',
        pick: {
            id: '#AudioPick',
        },
        auto: true,
        resize: false,
        threads: 1,
        accept: {
            title: 'audio',
            extensions: 'wav,mp3,ogg',
            mimeTypes: 'audio/*'
        }
        
    });

    AudioUploader.on('uploadAccept', function (object, ret) {
        if (ret._raw == "error")
            alert("好像出错了，请刷新页面试试");
        else {
            //_ImageTagAdd(ret._raw);
            //$('.ImageBar').removeClass('hide');
            var box = $('.AudioBox');
            var files = AudioUploader.getFiles();
            console.log(files);
            console.log(ret);
            var filesLength = files.length;
            if (filesLength > 1) {
                AudioUploader.removeFile(files[filesLength - 2].id);
            }
            box.find('.fileName').html(files[filesLength - 1].name);
            box.find('audio').attr('src', ret._raw);
            //box.find('input').attr('value', ret._raw);
            $("#myAudio").attr('value', ret._raw)
        }
    });

    AudioUploader.on('uploadError', function (file) {
        alert("好像出错了，请刷新页面试试");
    });

    

    var uploader = new WebUploader.Uploader({
        swf: 'webuploader/Uploader.swf',
        server: '/Home/Upload',
        pick: {
            id: '#uploader',
        },
        auto: true,
        resize: false,
        threads: 1,
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        }

    });

    uploader.on('uploadAccept', function (object, ret) {
        if (ret._raw == "error")
            alert("好像出错了，请刷新页面试试");
        else {
            addImageArticle(ret._raw);
        }
    });

    uploader.on('uploadError', function (file) {
        alert("好像出错了，请刷新页面试试");
    });

    

    var addSubtitleUploaderInit = function (id) {
        var box = $(id).parent().parent();
        //box.find('.subtitleContainer').img()
        
        var u = new WebUploader.Uploader({
            swf: 'webuploader/Uploader.swf',
            server: '/Home/Upload',
            pick: {
                id: id,
            },
            auto: true,
            resize: false,
            threads: 1,
            accept: {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png',
                mimeTypes: 'image/*'
            }

        });

        u.on('uploadAccept', function (object, ret) {
            if (ret._raw == "error")
                alert("好像出错了，请刷新页面试试");
            else {
                box.find('.subtitleContainer img').attr('src', ret._raw);
                box.find('.subtitleContainer input').attr('value', ret._raw);
                //box.find('.addSubtitle').hide();
            }
        });

        u.on('uploadError', function (file) {
            alert("好像出错了，请刷新页面试试");
        });
        
        uploaders.push(u);
    }

    $('#form')[0].onsubmit = function () {
        if ($('.Audio').val() == "") {
            if (confirm('确定不添加配乐？')) {
                return true;
            } else {
                return false;
            }
        }
        if ($('.ImageBox .ImageArticle').length == 0) {
            alert("请至少选择一张图片");
            return false;
        }
        if ($('#myname').val()=="") {
            alert("请填写名称");
            return false;
        }

    };



})(jQuery);