// 零售商 js代码
//========================
//初始化入口：统一界面自动函数初始化
//========================
function init(){
	//公司管理： 按钮组hover二级菜单
	dropMenu("#MemberTable .company_btn","#MemberTable div");
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
/*var overtime, outtime,obj_temp;
$(".nav li").hover(function () {
    clearTimeout(outtime);
    obj_temp = $(this);
    overtime = setTimeout(function () {
        if (obj_temp.children("ul").css("display") == "list-item") {
            return;
        }
        else {

            obj_temp.children("ul").toggle("1500");
        }
    }, 300);
}, function () {
    clearTimeout(overtime);
    obj_temp = $(this);
    outtime = setTimeout(function () {
        if (obj_temp.children("ul").css("display") == "list-item") {
            return;
        }
        else {

            obj_temp.children("ul").toggle("1500");
        }
    }, 300);
});*/
$(".left .nav li").click(function () {
    if ($(this).children("ul").css("display") == "list-item") {

    }
    else {
        console.log("显示");
        $(this).children("ul").stop(false, false).slideToggle("2000");
    }
});
$(".nav li.active .title").click(function (event) {
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
}
//========================
//  线路搜索条件  选中样式
//========================
$("#SearchAdition input[type='radio']").bind("click",function(){
	$(this).parents("ul").children("li").removeClass("choosed");
	$(this).parent(this).addClass("choosed");
});
//========================
//  线路搜索框显示  再次搜索
//========================
$("#SearchMore").click(function(){
	if($("#SearchPart").css("display") == "none"){
		$("#SearchMore").text("↑收起搜索框");
	}
	else{
		$("#SearchMore").text("↓点击这里，重新搜索");
	}
	$("#SearchPart").slideToggle();
});
$("#AddSearchAdition").click(function(){
	if($("#SearchAdition").css("display") == "none"){
		$(this).text("↑收起搜索条件框");
	}
	else{
		$(this).text("↓点击这里，添加更多搜索条件");
	}
	$("#SearchAdition").slideToggle();
	$("#AditionTab").slideToggle();
});

//========================
//线路管理 当前线路的订单管理： Tab选项卡切换
//========================
$("#ManageNav ul li").click( function(){
	$("#ManageNav ul li").removeClass("choosed");
	$(this).addClass("choosed");
	var _no = $(this).index();
	$("#ManageInfor").find("table").css("display","none");
	$("#ManageInfor").find("table").eq(_no).css("display","table");
});

//======================== 
//左侧菜单栏选择函数
//========================
function LeftMenuSelect(Index)
{
    var Count = 6;
    for (var i = 0; i < Count; i++)
    {
        $("#Menu").children("li").eq(i).removeClass("active");
    }
    if (Index >= 0 && Index < Count)
    {
        $("#Menu").children("li").eq(Index).addClass("active");
    }
}
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