// 批发商界面JS 库
//========================
//初始化入口：统一界面自动函数初始化
//========================
function init(){
	//公司管理： 按钮组hover二级菜单
	dropMenu("#MemberTable .company_btn","#MemberTable div");
	//线路管理-更多操作的二级菜单
	dropMenu(".moreaction_btn",".table_moreaction");
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
//发布线路： 输入框选中样式
//========================
$("#CheckRouteType input").click(function(){
    $(this).parentsUntil("div").children("li").removeClass("choosed");
	$(this).parent(this).addClass("choosed");
});

//========================
//线路管理： Tab选项卡切换
//========================
$("#ManageNav ul li").click( function(){
	$("#ManageNav ul li").removeClass("choosed");
	$(this).addClass("choosed");
	var _no = $(this).index();
	$("#ManageInfor").find("table").css("display","none");
	$("#ManageInfor").find("table").eq(_no).css("display","table");
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