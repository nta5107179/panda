//��Ŀ¼
var g_root = "/";

//��̬����
var static = {
    //��ȡ�����
    getRandom: function (min, max)
    {
        return Math.floor(Math.random() * (max - min + 1) + min);
    },
    //��jsonת����url����
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
        var _variable_value = variable_value; //�ɱ�����Сֵ
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

//��д$.ajax
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