//根目录
var g_root = "/";

//静态方法
var static = {
    //获取随机数
    getRandom: function (min, max)
    {
        return Math.floor(Math.random() * (max - min + 1) + min);
    },
    //将json转换成url参数
    getAction: function (json)
    {
        var arr = [];
        for (var key in json)
        {
            arr.push(key + "=" + json[key]);
        }
        return arr.join("&");
    },
    oldBoxWidth: 0,
    newBoxWidth: 0,
    setTableStyle: function (variable_value)
	{
        var thList = $(".table thead tr th");
        var widthSum = 0;
        var _variable_value = variable_value; //可变宽度最小值
        for (var i = 0; i < thList.length; i++)
        {
            if (thList[i].style.width != "auto")
                widthSum += parseInt(thList[i].style.width.replace("px"));
        }
        static.newBoxWidth = $("#app ._list ._box").width();
        if (static.oldBoxWidth != 0)
        {
            if (static.oldBoxWidth > static.newBoxWidth && static.newBoxWidth < widthSum + _variable_value)
            {
                $(".table").css("width", "max-content");
                $("#_variable_th").css("width", _variable_value);
            }
            else if (static.oldBoxWidth < static.newBoxWidth && static.newBoxWidth > widthSum)
            {
                $(".table").css("width", "100%");
                $("#_variable_th").css("width", "auto");
            }
        }
        static.oldBoxWidth = static. newBoxWidth

    },
    defalut:null
}

//重写$.ajax
var g_ajax = $.ajax;
$.ajax = function (json)
{
    g_ajax({
        type: json.type,
        url: json.url,
        data: static.getAction(json.data) + "&__RequestVerificationToken=" + $("input[name=__RequestVerificationToken]").val(),
        success: json.success
    });
}