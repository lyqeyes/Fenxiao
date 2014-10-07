/*  跑马灯插件  类
	2014.9.28
	RunLight类 参数：msg消息容器，speed滚动速度.where容器类
	动画部分基于jQuery 动画
*/
function RunLight(where){
	this.msg = [];   //消息容器
	this.speed= 8000;//滚动速度
	this.ul = null;
	this.len = 0;
	temp = this;
    //数据初始化  ajax
	$.ajax(
                     {
                         url: "/Home/RunLight",
                         type: "POST",
                         data:
                             {
                             },
                         beforeSend: function (XMLHttpRequest) {
                         },
                         success: function (data) {
                             var msgs = eval(data);
                             for (var i = 0; i < msgs.length; i++) {
                                 temp.msg.push(msgs[i]);
                             }
                             temp.place(where);
                         },
                         error: function () {
                         }
                     });
};
RunLight.prototype.place = function(where){
	var _container = document.getElementById(where);
	var _ul = document.createElement("ul");
	_container.appendChild(_ul);
	for(var n = 0;n<this.msg.length;n++){
		var li = document.createElement("li");
		li.textContent = this.msg[n];
		_ul.appendChild(li);
	}
	//设置滚动效果
	this.len = this.msg.length;
	this.ul  = _ul;
	var _i = 0 ,len = this.len;
	var _interval = setInterval(function(){
		if(_i < (len-1) ){
			$(_ul).children().eq(_i).slideUp("slow");
			_i++;
		}
		else{
			$(_ul).children().slideDown("fast");
			_i=0;
		}
	},this.speed);
	_ul.onmouseover=function(){
		clearInterval(_interval);
	}
	_ul.onmouseout=function(){
		this.speed = 8000;
		_interval = setInterval(function(){
			if(_i < (len-1) ){
				$(_ul).children().eq(_i).slideUp("slow");
				_i++;
			}
			else{
				$(_ul).children().slideDown("fast");
				_i=0;
			}
		},this.speed); 
	};    
};