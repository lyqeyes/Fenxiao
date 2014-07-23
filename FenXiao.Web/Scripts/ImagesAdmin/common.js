//var OurEDA = function(){


    $(document).ready(function () {
        $('.subtitle').css('opacity', 0);
        $('.subtitle').eq(0).css('opacity', '1');
    })

    var subtitleShow = function (index) {
        $('.subtitle').eq(index).css('margin-top', '-100px');
        $('.subtitle').eq(index).animate({ 'margin-top': '0', 'opacity': '1' }, 1200);
    }

	/****************	保持图片高度与窗口高度一致	**************************/
	var WindowHeight = $(window).height();
	var imgW  = $('.tag img').width();
	$('.container').height(WindowHeight);
	$('.tag').height(WindowHeight);
	$('.tag img').height(WindowHeight);
	$('.tag').css('margin-left',-(imgW/2)+'px');
	
	$(window).resize(function(){
		WindowHeight = $(window).height();
		$('.container').height(WindowHeight);
		$('.tag').height(WindowHeight);
		$('.tag img').height(WindowHeight);
		$('.tag').css('margin-left',-(imgW/2)+'px');
	});
	
	
	/******************	变量初始化	********************/
	var MouseDown = 0,	//鼠标的状态	0为无须操作状态，1为已按下为弹起状态，需操作状态
		prex = 0,
		formery = 0,	//按下时鼠标的纵坐标
		prey = 0 ,	//当前鼠标纵坐标
		moveDrection = 0,	//鼠标方向，1位向上，2为向下
		prePic = 0,	//当前展示页面
		piclength  = $('.tag').length -1,	//图片数量
		musicSwitch = 1,
		musicremind = 1,
		audio = document.getElementById('music'),
		td = $('.3dtarget'),
		tdth = 1,
		formertdth = 1,
		Minit = 1;
	
	
	(function (){
		$('.tag').css('top',WindowHeight + 'px');
		$('.tag').eq(0).css('top','0');
		
	})();
	
	
	$(window).on('mousedown touchstart', function(e) {	
		if (e.type == "touchstart") {
			 formerx = window.event.touches[0].pageX;
			 formery = window.event.touches[0].pageY;
		 } else {
			 formerx = e.pageX||e.x;
			 formery = e.pageY||e.y;
		 }
		MouseDown = 1;
	});
	
	var a = '';
	var b = '';
	$(window).on('mousemove  touchmove', function(e){ 
		e.preventDefault();
		e.stopPropagation();
		if(MouseDown === 1){
			//prey = evt.pageY;
			if (e.type == "touchmove") {
				 prex = window.event.targetTouches[0].pageX;
				 prey = window.event.targetTouches[0].pageY;
			} else {
				 prex = e.pageX||e.x;
				 prey = e.pageY||e.y;
			}
			//如果没有方向，即刚开始滑动
			if(moveDrection == 0){
				if(Math.abs(formery - prey) < Math.abs(formerx - prex) && prePic == 8){
					moveDrection = 3;
					
					b += moveDrection.toString();
					if(Math.abs(formerx - prex) > 10){
						//a += (Math.abs(formerx - prex)).toString();
						if( formerx - prex > 10){
							/*tdth = formertdth + Math.floor((formerx - prex) /10);
							if(tdth > 35) tdth = 1;
							if(tdth <= 35 && tdth > 0){
									td.attr('src','image/3d/'+tdth+'.jpg');
							}*/
							tdth ++;
							if(tdth > 35) tdth = 1;
							td.attr('src','image/3d/'+tdth+'.jpg');
						}
						else{
							/*tdth = formertdth - Math.floor((prex - formerx) /10);
							if(tdth <= 0) tdth = 35;
							if(tdth <= 35 && tdth > 0){
									td.attr('src','image/3d/'+tdth+'.jpg');
							}*/
							tdth --;
							if(tdth <= 0) tdth = 35;
							td.attr('src','image/3d/'+tdth+'.jpg');
						}
					}
					return ;
				}
				//向上滑
				if(formery - prey > 0){
					if(prePic == piclength) {
						return;
					};
					$('.tag').eq(prePic).css('z-index','1');
					$('.tag').eq(prePic + 1).css('z-index','2');
					moveDrection = 2;
					$('.tag').eq(prePic + 1).css('top',WindowHeight - (formery - prey) +'px');
					//a += '1';
				} 
				//向上滑
				else {
					if(prePic == 0)return;
					moveDrection = 1;
					$('.tag').eq(prePic).css('z-index','1');
					$('.tag').eq(prePic - 1).css('z-index','2');
					//alert(-(WindowHeight - (formery - prey)));
					$('.tag').eq(prePic - 1).css('top',-(WindowHeight - (prey - formery)) +'px');
				}
			}
			//正在向上滑
			else if(moveDrection == 1){
				$('.tag').eq(prePic - 1).css('top',-(WindowHeight - (prey - formery)) +'px');
			}
			else if(moveDrection == 3){
				//a += formerx +' ' +prex +' ';
				if(Math.abs(formerx - prex) > 10){
					// += 'true '; 
					/*if( formerx - prex > 0)
					{
						//a += formertdth + Math.floor((formerx - prex) /500) +' ';
						/*tdth = formertdth + Math.floor((formerx - prex) /10);
						
						if(tdth > 35) tdth %= 35;
						a += tdth + ' true ';
						if(tdth <= 35 && tdth > 0){
								td.attr('src','image/3d/'+tdth+'.jpg');
						}*/
						
							/*tdth ++;
							if(tdth > 35) tdth = 1;
							td.attr('src','image/3d/'+tdth+'.jpg');
					}
					else{
						/*a += tdth + ' false ';
						tdth = formertdth - Math.floor((prex - formerx) /10);
						if(tdth <= 0) tdth = 35;
						if(tdth <= 35 && tdth > 0){
								td.attr('src','image/3d/'+tdth+'.jpg');
						}*/
							/*tdth --;
							if(tdth <=0 ) tdth = 35;
							td.attr('src','image/3d/'+tdth+'.jpg');
					}*/
					if(prex < formertdth){
						tdth ++;
						if(tdth > 35) tdth = 1;
						td.attr('src','image/3d/'+tdth+'.jpg');
					}
					else{
						tdth --;
						if(tdth <= 0) tdth = 35;
						td.attr('src','image/3d/'+tdth+'.jpg');
					}
				}
			}
			//正在向下滑
			else{
				if(prePic <= piclength) {
					$('.tag').eq(prePic + 1).css('top',WindowHeight - (formery - prey) +'px');
				}
			}
		}
		formertdth = prex;
		
	});
	
	
	$(window).on('mouseup  touchend mouseout', function() {
		if(MouseDown == 1 && moveDrection ==3){
			MouseDown = 0;	
			moveDrection = 0;
			formery = 0;
			formerx = 0
			prex = 0;
			prey = 0;
			return ;
		}
		if(MouseDown == 1 && moveDrection != 0){
			if(formery - prey  > 100 ){
				if(prePic < piclength) {
					$('.tag').eq(prePic).css('z-index','0');
					$('.tag').eq(prePic + 1).css('z-index', '1');

					$('.tag').eq(prePic + 1).animate({top:'0'},500,'linear',function(){
					    $('.tag').eq(prePic - 1).css('top', -WindowHeight + 'px');
					    $('.subtitle').eq(prePic - 1).css('opacity', 0);
					});
					subtitleShow(prePic + 1);
					prePic ++;
					if(musicremind){
						$('.remind').css('opacity','0');
					}
				}
				//alert(formery + ' ' + prey);
			}
			else if(prey - formery  > 200){
				if(prePic != 0){
					$('.tag').eq(prePic).css('z-index','0');
					$('.tag').eq(prePic - 1).css('z-index','1');
					$('.tag').eq(prePic - 1).animate({top:'0'},500,'linear',function(){
					    $('.tag').eq(prePic + 1).css('top', WindowHeight + 'px');
					    $('.subtitle').eq(prePic + 1).css('opacity', 0);
					});
					subtitleShow(prePic - 1);

					prePic --;
				}
			}
			else {
				if(moveDrection == 1){
					$('.tag').eq(prePic - 1).animate({top:-WindowHeight +'px'},500);
				}
				else{
					$('.tag').eq(prePic + 1).animate({top:WindowHeight +'px'},500);
				}
			}
		}
		formertdth = tdth;
		MouseDown = 0;	
		moveDrection = 0;
		formery = 0;
		prey = 0;
		if(prePic == 9) {audio.pause();}
		//if(prePic >= piclength) alert(prePic);
	});
	
	
	
	$('.switch').click(function(){
		if(musicSwitch == 1){
			audio.pause();
			musicSwitch = 0;
			$('.switch').css('background-position','0 0');
		}
		else{
			audio.play();
			musicSwitch = 1;
			$('.switch').css('background-position','-60px 0');
		}
		//alert(a);
	});
	
	$(window).load(function(e) {
		var Audio  = document.getElementById('music');
		Audio.play();
	});
	
	$('.tag-text').click(function(){
		$(this).toggleClass('flex-text');
	});
	

//};