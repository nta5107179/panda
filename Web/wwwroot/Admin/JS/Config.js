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
        if ((static.oldBoxWidth != 0 && static.oldBoxWidth > static.newBoxWidth && static.newBoxWidth < widthSum + _variable_value) || (static.oldBoxWidth == 0 && static.newBoxWidth < widthSum + _variable_value))
        {
            $(".table").css("width", "-webkit-max-content");
            $("#_variable_th").css("width", _variable_value);
        }
        else if ((static.oldBoxWidth != 0 && static.oldBoxWidth < static.newBoxWidth && static.newBoxWidth >= widthSum) || (static.oldBoxWidth == 0 && static.newBoxWidth >= widthSum))
        {
            $(".table").css("width", "100%");
            $("#_variable_th").css("width", "auto");
        }
        static.oldBoxWidth = static. newBoxWidth
    },
    isEmptyJson:function (e) {
        var t;
        for (t in e)
            return false;
        return true;
    },
    CopyRouteQuery: function (e)
    {
        var newJson = {};
        var regex_float = new RegExp(/^\d+\.\d+$/)
        var regex_int = new RegExp(/^\d+$/)
        for (t in e)
        {
            if (regex_float.test(e[t].toString()))
            {
                newJson[t] = parseFloat(e[t]);
            }
            else if (regex_int.test(e[t].toString()))
            {
                newJson[t] = parseInt(e[t]);
            }
            else
            {
                newJson[t] = e[t];
            }
        }
        return newJson;
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

//分页插件
Vue.component("table-pages", {
    props: {
        action: Object
    },
    template: '' +
        '<div class="col-xs-12 _pages">' +
        '   <div class="col-sm-6 hidden-xs">' +
        '       <h5>{{action.limit}} per, page {{action.page}}/{{action.total}}</h5>' +
        '   </div>' +
        '   <div class="col-sm-6 col-xs-12 text-right">' +
        '       <ul class="pagination pagination-sm">' +
        '           <li v-bind:class="{disabled:action.page==1}"><a href="javascript:void(0)" v-on:click="pagePrevious()">&laquo;</a></li>' +
        '           <li v-if="action.page-2>=1"><a href="javascript:void(0)" v-on:click="pageChoose(action.page-2)">{{action.page-2}}</a></li>' +
        '           <li v-if="action.page-1>=1"><a href="javascript:void(0)" v-on:click="pageChoose(action.page-1)">{{action.page-1}}</a></li>' +
        '           <li class="active"><a href="javascript:void(0)" v-on:click="pageChoose(action.page)">{{action.page}}</a></li>' +
        '           <li v-if="action.page+1<=action.total"><a href="javascript:void(0)" v-on:click="pageChoose(action.page+1)">{{action.page+1}}</a></li>' +
        '           <li v-if="action.page+2<=action.total"><a href="javascript:void(0)" v-on:click="pageChoose(action.page+2)">{{action.page+2}}</a></li>' +
        '           <li v-bind:class="{disabled:action.page==action.total}"><a href="javascript:void(0)" v-on:click="pageNext()">&raquo;</a></li>' +
        '       </ul>' +
        '   </div>' +
        '</div>' +
        '',
    methods: {
        pagePrevious: function ()
        {
            if (this.action.page == 1)
                return;
            this.action.page -= 1;
            this.$emit('pagechange')
        },
        pageChoose: function (page)
        {
            this.action.page = page;
            this.$emit('pagechange')
        },
        pageNext: function ()
        {
            if (this.action.page == this.action.total)
                return;
            this.action.page += 1;
            this.$emit('pagechange')
        }
    }
})