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