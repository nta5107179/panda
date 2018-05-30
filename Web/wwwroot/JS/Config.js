$(document).ready(function ()
{
	//È«¾Örem
	var windowWidth = $(window).width();
	var resize = function ()
	{
		windowWidth = $(window).width();
		windowHeight = $(window).height();
		$("html").css({ fontSize: "14px" });
		if (windowWidth < windowHeight)
		{
			if (windowWidth < 768)
			{
				$("html").css({ fontSize: "12px" });
			}
		}
		setDefaultProduct();
	}
	resize();
	$(window).on("resize", function ()
	{
		resize();
	});

	//ÕÚÕÖ²ã´¥¿ØÀ¹½Ø
	$(".self-cover").on('touchmove', function (event)
	{
		event.preventDefault();
	});
});

//ÉèÖÃ×ó²à²Ëµ¥
function setSelfLeft()
{
	var conver = $(".self-cover");
	var left = $(".self-left");

	left.stop();

	conver.css({ display: "block", height: $("body").outerHeight() });
	left.css({ boxShadow: "#333 -2px 0px 5px" });
	left.animate({ right: "0" });
	
	conver.click(function ()
	{
		left.stop();
		left.animate({ right: "-70%" }, function ()
		{
			conver.css({ display: "none" });
			left.css({ boxShadow: "none" });
		})
	});
}

//Ê×Ò³¹ö¶¯Í¼Æ¬
var productList = null;
function setDefaultProduct()
{
	var default_product = $("#default_product");
	if (default_product != null)
	{
		if (productList == null)
		{
			productList = default_product.find(".item .row .self-col");
		}

		default_product.empty();
		var windowWidth = $(window).width();
		var fct = function (k)
		{
			for (var i = 0; i < (productList.length / k < 1 ? 1 : Math.floor(productList.length / k)) ; i++)
			{
				var item = $("<div class='item'></div>");
				if (i == 0)
					item.addClass("active");
				var row = $("<div class='row'></div>");
				for (var j = i * k; j < i * k + k; j++)
				{
					row.append(productList[j]);
				}
				item.append(row);
				default_product.append(item);
			}
		}
		if (windowWidth < 768)
		{
			fct(1)
		}
		else if (windowWidth >= 768 && windowWidth < 992)
		{
			fct(2)
		}
		else if (windowWidth >= 992 && windowWidth < 1500)
		{
			fct(3)
		}
		else if (windowWidth >= 1500)
		{
			fct(4)
		}
	}
}
