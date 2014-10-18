// 登陆界面 JS 库
//========================
//初始化入口：统一界面自动函数初始化
//========================
function init(){
}

var g_infor = new Array();
//========================
//首页 表单验证插件
//========================
function CheckLog(sub){
	var _type = $("input[name='type']:checked").val();
	var _email = $("input[name='email']").val();
	var _pas  = $("input[name='password']").val();
	//console.log(_type,_email,_pas);
	var _error="";
	//信息验证
	if( _type == "" || typeof(_type)== 'undefined' ){
		_error = "*请选择账户类别！";		
	}
	else if ( _email =="" || typeof(_email)== 'undefined' ){
		_error = "*请填写登录邮箱！"
	}
	else if ( _pas=="" || typeof(_pas)== 'undefined' ){
		_error = "*请填写您的账户密码！"
	}
	else{
		SendLog(_type,_email,_pas);
	}
	//错误信息回填
	$(sub).next().text(_error);
}
//========================
//Log表单发送函数
//========================
function SendLog(_type,_email,_pas){
	console.log(_type,_email,_pas);
}
//========================
//点击注册 界面联动动画
//========================
function ShowReg(obj){
	$("#login_part").slideToggle("fast");
	$("#reg_part").slideToggle("fast");
	if( $(obj).attr("id") == "RegHref" ){
		$("#reg_part input[type='text']").val("");	
		$("#footer_inf").html('已有账号？<a id="LogHref" onclick="ShowReg(this);">返回登录</a>');
	}
	else{
		$("#footer_inf").html('还没有账号？<a id="RegHref" onclick="ShowReg(this);">立即注册</a>')	
	}
}
//========================
//注册3步骤 函数
//========================
function RegThree(_step,sub){
    var _error;
    switch(_step){
        case 1:{
            var _rtype = $("input[name='reg_type']:checked").val();
            var _rname = $("input[name='reg_name']").val();
            var _rlicense = $("input[name='reg_license']").val();
            var _rpermit = $("input[name='reg_permit']").val();
            var _paddress = $("input[name='reg_paddress']").val();
            //相应数据处理
            if( _rtype=="" || typeof(_rtype)=='undefined'){
                _error = "*请选择注册账号的类型";
            }
            else if( _rname == "" || typeof(_rname)=='undefined'){
                _error = "*请填写公司名称";
            }
            else if( _rlicense=="" || typeof(_rlicense)=='undefined' ){
                _error = "*请填写营业执照号码";
            }
            else if( _rpermit=="" || typeof(_rpermit)=='undefined' ){
                _error = "*请填写经营许可号";
            }
            else if (_paddress == "" || typeof (_paddress) == 'undefined') {
                _error = "*请填写公司地址";
            }
            else{
                g_infor.push(_rtype,_rname,_rlicense,_rpermit);
                console.log(g_infor);
                $('#step1').addClass('checked');
                $('#step1_part').hide();
                $('#step2_part').show();
                return true;
            }
            //错误处理
            $(sub).next().text(_error);
            return true;
            };break;
        case 2:{
            var _pname = $("input[name='reg_pname']").val();
            var _pjob = $("input[name='reg_pjob']").val();
            var _pid = $("input[name='reg_pid']").val();
            var _ptel = $("input[name='reg_ptel']").val();
            var _pemail = $("input[name='reg_email']").val();
            var _ppass = $("input[name='reg_password']").val();
            
            //相应数据处理
            if( _pname=="" || typeof(_pname)=='undefined'){
                _error = "*请填写联系人姓名";
            } else if (_pemail == "" || typeof (_pemail) == 'undefined') {
                _error = "*请填写联系人email";
            }
            else if (_ppass == "" || typeof (_ppass) == 'undefined') {
                _error = "*请填写联系人登陆密码";
        }
            else if( _pjob == "" || typeof(_pjob)=='undefined'){
                _error = "*请填写公司名称";
            }
            else if( _pid=="" || typeof(_pid)=='undefined' ){
                _error = "*请填写联系人的身份证号码";
            }
            else if( _ptel=="" || typeof(_ptel)=='undefined' ){
                _error = "*请填写联系电话";
            }
            else{
                g_infor.push(_pname,_pjob,_pid,_ptel,_paddress);
                console.log(g_infor);
                $('#step2').addClass('checked');
                $('#step2_part').hide();
                //批发商需要多上传 保单截图
                if( g_infor[1] == "pfs" ){
                    $(".gurantee").show();
                }
                $('#step3_part').show();
                return true;
            }
            //错误处理
            $(sub).next().text(_error);
        };break;
        case 3: {
            
        };break;
        default:
            break;
    }
}

//========================
//登陆注册模块选择
//========================
$("#choose-pfs").click(function () {
    $("#login-pfs").click();
    $("#type-choose").css("display", "none");
    $("#type-alert").html('批发商登陆入口：　　　<small class="f-lightblue"  style="cursor:pointer;"  onClick="window.location.reload();">返回上一步</small>');
    $("#login-input-part").css("display", "block");
});
$("#choose-lss").click(function () {
    $("#login-lss").click();
    $("#type-choose").css("display", "none");
    $("#type-alert").html('零售商登陆入口：　　　<small class="f-lightblue" style="cursor:pointer;" onClick="window.location.reload();">返回上一步</small>');
    $("#login-input-part").css("display", "block");
});

//========================
//返回上一步骤
//========================
function RegReturn(num) {
    if(num==2){
        $('#step1_part').show();
        $('#step2_part').hide("fast");
    }
    else if (num == 3) {
        $('#step2_part').show();
        $('#step3_part').hide("fast");
    }
    else {

    }
}