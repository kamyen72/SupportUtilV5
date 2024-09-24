function GetMenuRootsV3() {
	//alert("V3 is coming!!");

	$.ajax({
		type: "POST",
		url: "/API/GetMenuV3",
		data: JSON.stringify({ "InputText": "" }),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (response) {
			let txt = response.ReturnText;
			$("#menubar").html(txt);
			//document.getElementById("menuRootsJson").innerText = txt;
		},
		failure: function (response) {
		},
		error: function (response, status) {
		}
	});
}

function showdiv(myname, me) {
	let nname = "#" + myname;
	let state = $(nname).css("display");

	let mytop = $(me).css("top");
	let mycolor = $(me).css("background");
	if (mycolor != null) {
		if (mycolor == "rgb(102, 102, 255) none repeat scroll 0% 0% / auto padding-box border-box") {
			//alert("this is a link");
			let mylink = $(me).attr("data-link");
			if (mylink != null) {
				let flink = "" + mylink + "";
				window.open(flink, '_blank', 'height=' + screen.height + ', width=' + screen.width);
			}
		}
		else {
			let mylink = $(me).attr("data-link");
			if (mylink != null) {
				let flink = "" + mylink + "";
				window.open(flink, '_blank', 'height=' + screen.height + ', width=' + screen.width);
			}
		}
	}

	if (state == "none") {
		$(".l2").css("display", "none");
		let state = $(nname).css("display", "block");
		$(nname).css("top", mytop);
	}
	else {
		$(".l2").css("display", "none");
		let state = $(nname).css("display", "none");
		$(nname).css("top", mytop);
	}
}

function hidediv(me) {
	var newname = "#" + me;
	$(newname).css('display', 'none');
}

function hidemenu() {
	// alert("wooi");

	let s = $("#menubarboss").css("display");
	// alert(s);

	if (s == "none") {
		$("#menubarboss").css("display", "block");
		$("#menubuttdiv").css("left", "810px");
		$("#menubutt").html('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-bar-left" viewBox="0 0 16 16"><path fill-rule="evenodd" d="M12.5 15a.5.5 0 0 1-.5-.5v-13a.5.5 0 0 1 1 0v13a.5.5 0 0 1-.5.5M10 8a.5.5 0 0 1-.5.5H3.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L3.707 7.5H9.5a.5.5 0 0 1 .5.5"/></svg>');
	}
	else {
		$("#menubarboss").css("display", "none");
		$("#menubuttdiv").css("left", "10px");
		$("#menubutt").html('<svg xmlns="http://www.w3.org/2000/svg" width = "16" height = "16" fill = "currentColor" class= "bi bi-arrow-bar-right" viewBox = "0 0 16 16" > <path fill - rule="evenodd" d = "M6 8a.5.5 0 0 0 .5.5h5.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3a.5.5 0 0 0 0-.708l-3-3a.5.5 0 0 0-.708.708L12.293 7.5H6.5A.5.5 0 0 0 6 8m-2.5 7a.5.5 0 0 1-.5-.5v-13a.5.5 0 0 1 1 0v13a.5.5 0 0 1-.5.5" /> </svg>');
	}
}