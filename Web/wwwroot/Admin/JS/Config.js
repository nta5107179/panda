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
    CopyJson: function (json)
    {
        var copy_json = {};
        for (key in json)
        {
            copy_json[key] = json[key];
        }
        return copy_json;
    },
    loading: function (type)
    {
        $("#modal_loading").modal(type);
    },
    alert: function (msg)
    {
        $("#modal_alert ").find(".modal-body").html(msg);
        $("#modal_alert").modal("show");
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
        success: json.success,
        error: json.error
    });
}

//分页插件
Vue.component("table-pages", {
    props: {
        action: Object,
        total: Number,
    },
    template: '' +
        '<div class="col-xs-12 _pages">' +
        '   <div class="col-sm-6 hidden-xs">' +
        '       <h5>{{action.limit}} per, page {{action.page}}/{{total}}</h5>' +
        '   </div>' +
        '   <div class="col-sm-6 col-xs-12 text-right">' +
        '       <ul class="pagination pagination-sm">' +
        '           <li v-bind:class="{disabled:action.page==1}"><a href="javascript:void(0)" v-on:click="pagePrevious()">&laquo;</a></li>' +
        '           <li v-if="action.page-2>=1"><a href="javascript:void(0)" v-on:click="pageChoose(action.page-2)">{{action.page-2}}</a></li>' +
        '           <li v-if="action.page-1>=1"><a href="javascript:void(0)" v-on:click="pageChoose(action.page-1)">{{action.page-1}}</a></li>' +
        '           <li class="active"><a href="javascript:void(0)" v-on:click="pageChoose(action.page)">{{action.page}}</a></li>' +
        '           <li v-if="action.page+1<=Math.ceil(total/action.limit)"><a href="javascript:void(0)" v-on:click="pageChoose(action.page+1)">{{action.page+1}}</a></li>' +
        '           <li v-if="action.page+2<=Math.ceil(total/action.limit)"><a href="javascript:void(0)" v-on:click="pageChoose(action.page+2)">{{action.page+2}}</a></li>' +
        '           <li v-bind:class="{disabled:action.page==Math.ceil(total/action.limit) || this.total==0}"><a href="javascript:void(0)" v-on:click="pageNext()">&raquo;</a></li>' +
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
            if (this.action.page == Math.ceil(this.total / this.action.limit) || this.total==0)
                return;
            this.action.page += 1;
            this.$emit('pagechange')
        }
    }
})

//树状select组件
Vue.component("select-tree", {
    props: {
        value: String,
        list: Array
    },
    template: '' +
        '<select class="form-control" v-model.number="value">' +
        '	<option value="">--请选择--</option>' +
        '	<option value="0">顶级类型</option>' +
        '	<option v-for="el in list" v-bind:value="el.id">{{el.name}}</option>' +
        '</select>' +
        '',
    methods: {
        pagePrevious: function ()
        {
            if (this.action.page == 1)
                return;
            this.action.page -= 1;
            this.$emit('pagechange')
        }
    }
})

//载入模态窗插件
Vue.component("modal-loading", {
    template: '' +
        '<div class="modal fade" id="modal_loading" tabindex="-1" role="dialog" data-backdrop="false">' +
        '    <div class="modal-dialog">' +
        '        <div class="modal-content">' +
        '            <div class="modal-header">' +
        '                <h4 class="modal-title">消息提示</h4>' +
        '            </div>' +
        '            <div class="modal-body">正在载入，请稍后...</div>' +
        '            <div class="modal-footer">' +
        '                <i class="fa fa-spinner fa-pulse"></i>' +
        '            </div>' +
        '        </div>' +
        '    </div>' +
        '</div>' +
        ''
})

//消息模态窗插件
Vue.component("modal-alert", {
    template: '' +
        '<div class="modal fade" id="modal_alert" tabindex="-1" role="dialog" data-backdrop="false">' +
        '    <div class="modal-dialog">' +
        '        <div class="modal-content">' +
        '            <div class="modal-header">' +
        '                <h4 class="modal-title">消息提示</h4>' +
        '            </div>' +
        '            <div class="modal-body"></div>' +
        '            <div class="modal-footer">' +
        '                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>' +
        '            </div>' +
        '        </div>' +
        '    </div>' +
        '</div>' +
        ''
})