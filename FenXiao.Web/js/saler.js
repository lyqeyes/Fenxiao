// 批发商界面JS 库
//========================
//初始化入口：统一界面自动函数初始化
//========================
function init(){
	//公司管理： 按钮组hover二级菜单
	dropMenu("#MemberTable .company_btn","#MemberTable div");
}
//更多操作 二级菜单点击实现
function ClickMoreAction(obj) {
    if ($(obj).next("div").css("display") == "none") {
        $(obj).next("div").show();
    }
    else {
        $(obj).next("div").hide();
    }
}
function CloseMoreAction(obj) {
    $(obj).parents("#TableMoreAction").hide();
}
//========================
//  顶部控制面板  通用组件级别
//========================
function C_ShowMessageBox(){
	$("#c_user").slideUp("fast");
	$("#c_message_box").slideToggle("fast");
}
function C_ShowUserBox(){
	$("#c_message_box").slideUp("fast");
	$("#c_user").slideToggle("fast");
}
//========================
//  左侧控制面板  通用组件级别
//========================
$(".left .nav li").click(function () {
	if( $(this).children("ul").css("display") == "list-item" ){
	
	}
	else {
	    console.log("显示");
	    $(this).children("ul").stop(false,false).slideToggle("2000");
	}
});
$(".nav li.active .title").click(function(event){
	event.preventDefault();
});

//========================
//  二级菜单
//========================
function dropMenu(obj,tableobj){
	$(obj).each(function(){
		var theSpan = $(this);
		var theMenu = theSpan.next();
		var tarHeight = theMenu.height();
		theMenu.css({height:0,opacity:0});
		theSpan.hover(
			function(){
				theMenu.stop().show().animate({height:tarHeight,opacity:1},200);
			},
			function(){
				theMenu.stop().animate({height:0,opacity:0},200,function(){
					$(this).css({display:"none"});
				});
			}
		);
	});
	$(tableobj).hover(function(){
		$(this).stop().show().animate({height:tarHeight,opacity:1},400);
		},function(){
		$(this).stop().show().animate({height:0,opacity:1},400);
		});

	console.log("测试");
}



//========================
//发布线路： 输入框选中样式
//========================
$("#CheckRouteType input").click(function(){
	$(this).parent(this).parent("ul").children("li").removeClass("choosed");
	$(this).parent(this).addClass("choosed");
});


//========================
//线路的订单管理： Tab选项卡切换
//========================
$("#OrderManageNav ul li").click( function(){
	$("#OrderManageNav ul li").removeClass("choosed");
	$(this).addClass("choosed");
	var _no = $(this).index();
	$("#OrderInfor").find("table").css("display","none");
	$("#OrderInfor").find("table").eq(_no).css("display","table");
});
//========================
//线路的订单管理： 搜索条件toggle
//========================
function ShowSearchDetails(obj) {
    var _dis = $("#SecondLink_Details").css("display");
    if (_dis == "block") {
        $("#SecondLink_Details").toggle("fast");
        $(obj).html("更多搜索条件<small>∨</small>");
    }
    else {
        $("#SecondLink_Details").toggle("fast");
        $(obj).html("精简搜索条件<small>∧</small>");
    }
}
//========================
//字数控制: 线路名称 25字限制
//========================
function NumCheck(obj){
    var length = 0;
    length = obj.value.length || 0;
    if (length > 25) {
        alert("对不起，线路名称不能超过25个字符。");
        obj.value = obj.value.substr(0, 25);
    }
}