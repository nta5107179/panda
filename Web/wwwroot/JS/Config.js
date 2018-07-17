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

//���jquery��������-���л����ύ����Ϊjson
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