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

//添加jquery公共方法-序列化表单提交内容为json
$.fn.serializeJson = function ()
{
    var serializeObj = {};
    var array = this.serializeArray();
    var str = this.serialize();
    $(array).each(function ()
    {
        if (serializeObj[this.name])
        {
            if ($.isArray(serializeObj[this.name]))
            {
                serializeObj[this.name].push(this.value);
            } else
            {
                serializeObj[this.name] = [serializeObj[this.name], this.value];
            }
        } else
        {
            serializeObj[this.name] = this.value;
        }
    });
    return serializeObj;
};