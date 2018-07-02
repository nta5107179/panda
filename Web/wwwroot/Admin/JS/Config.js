//根目录
var g_root = "/";

//静态方法
var static = {
    //获取随机数
    getRandom: function (min, max)
    {
        return Math.floor(Math.random() * (max - min + 1) + min);
    },
    isEmptyJson:function (e) {
        var t;
        for (t in e)
            return false;
        return true;
    },
    CopyJson: function (json)
    {
        return JSON.parse(JSON.stringify(json));
    },
    loading: function (type)
    {
        $("#modal_loading").modal(type);
    },
    alert: function (msg, obj)
    {
        $("#modal_alert").find(".modal-body").html(msg);
        $("#modal_alert").modal("show");
        if (typeof (obj) == "function")
        {
            var element = $('<button type="button" class="btn btn-default">确定</button>');
            element.click(function ()
            {
                obj();
                $("#modal_alert").modal("hide");
            });
            $("#modal_alert").find(".modal-footer").html(element);
        }
        else
        {
            $("#modal_alert").find(".modal-footer").html("" +
                '<button type="button" class="btn btn-default" data-dismiss="modal">确定</button>' +
                "");
        }
    },
    confrim: function (msg,obj)
    {
        $("#modal_confrim").find(".modal-body").html(msg);
        $("#modal_confrim").modal("show");
        if (typeof (obj) == "object")
        {
            var html = [];
            for (var i = 0; i < obj.length; i++)
            {
                var element = null;
                if (obj[i].handler != null)
                {
                    (function (i)
                    {
                        element = $('<button type="button" class="btn ' + (obj[i].class != null ? obj[i].class : "btn-default") + '">' + obj[i].title + '</button>');
                        element.click(function ()
                        {
                            obj[i].handler();
                            $("#modal_confrim").modal("hide");
                        });
                    })(i);
                }
                else
                {
                    element = $('<button type="button" class="btn ' + (obj[i].class != null ? obj[i].class : "btn-default") +'" data-dismiss="modal">' + obj[i].title + '</button>');
                }
                html.push(element);
            }
            $("#modal_confrim").find(".modal-footer").html(html);
        }
        else if(typeof(obj)=="function")
        {
            $("#modal_confrim").find(".modal-footer").html("" +
                '<button type="button" class="btn btn-default">确定</button>' +
                '<button type="button" class="btn btn-default" data-dismiss="modal">取消</button>' +
                "");
        }
    },
    defalut:null
}

//项目专用方法
var project = {
    newstypelist_to_treelist: function (newstypelist)
    {
        //解析newstypelist
        var newstypelist_t = [];
        for (var i = 0; i < newstypelist.length; i++)
        {
            newstypelist_t.push({
                id: newstypelist[i].g_newstype.nt_id,
                pid: newstypelist[i].g_newstype.nt_pid,
                name: newstypelist[i].g_newstype.nt_name
            });
        }
        return newstypelist_t;
    }
}

//重写$.ajax
var g_ajax = $.ajax;
$.ajax = function (json)
{
    var form = new FormData();
    for (key in json.data)
    {
        if (json.data[key] != null)
            form.append(key, json.data[key]);
    }
    form.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
    if (json.fileData != null)
    {
        for (key in json.fileData)
        {
            if (json.fileData[key] != null)
                form.append(key, json.fileData[key]);
        }
    }

    g_ajax({
        type: json.type,
        url: json.url,
        data: form,
        processData: false,
        contentType: false,
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
    created: function ()
    {
        this.action.page = Number(this.action.page);
        this.action.limit = Number(this.action.limit);
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
//list:[{id:,name:,pid:}]
Vue.component("select-tree", {
    props: {
        value: Number,
        list: Array
    },
    data: function ()
    {
        return {
            value_in: this.value,
            list_in: []
        }
    },
    watch: {
        list: function ()
        {
            this.init();
        },
        value: function ()
        {
            this.selected();
        },
        value_in: function ()
        {
            this.changed();
        }
    },
    created: function ()
    {
        this.init();
    },
    template: '' +
        '<select class="form-control" v-model="value_in">' +
        '	<option v-bind:value="null">--请选择--</option>' +
        '	<option v-bind:value="0">顶级类型</option>' +
        '	<option v-for="el in list_in" v-bind:value="el.id">{{el.name}}</option>' +
        '</select>' +
        '',
    methods: {
        selected: function ()
        {
            this.value_in = this.value;
        },
        changed: function ()
        {
            this.$emit('input', this.value_in)
        },
        init: function ()
        {
            var sortout = function (list_in, list_t)
            {
                for (var i = list_t.length - 1; i >= 0; i--)
                {
                    if (list_t[i].pid == 0)
                    {
                        list_t[i].lv = 1;
                        var name = "";
                        for (var k = 0; k < list_t[i].lv; k++)
                        {
                            name += "├　";
                        }
                        list_t[i].name = name + list_t[i].name;
                        list_in.splice(0, 0, list_t[i]);
                        list_t.splice(i, 1);
                    }
                    else
                    {
                        for (var j = list_in.length - 1; j >= 0; j--)
                        {
                            if (list_t[i].pid == list_in[j].id)
                            {
                                list_t[i].lv = list_in[j].lv + 1;
                                var name = "";
                                for (var k = 0; k < list_t[i].lv; k++)
                                {
                                    name += "├　";
                                }
                                list_t[i].name = name + list_t[i].name;
                                list_in.splice(j+1, 0, list_t[i]);
                                list_t.splice(i, 1);
                                break;
                            }
                        }
                    }
                }
                if (list_t.length > 0)
                {
                    sortout(list_in, list_t);
                }
            }
            sortout(this.list_in, static.CopyJson(this.list));
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
        '            </div>' +
        '        </div>' +
        '    </div>' +
        '</div>' +
        ''
})

//confrim模态窗插件
Vue.component("modal-confrim", {
    template: '' +
        '<div class="modal fade" id="modal_confrim" tabindex="-1" role="dialog" data-backdrop="false">' +
        '    <div class="modal-dialog">' +
        '        <div class="modal-content">' +
        '            <div class="modal-header">' +
        '                <h4 class="modal-title">消息提示</h4>' +
        '            </div>' +
        '            <div class="modal-body"></div>' +
        '            <div class="modal-footer">' +
        '            </div>' +
        '        </div>' +
        '    </div>' +
        '</div>' +
        ''
})

Vue.prototype.dateformat = function (date, format)
{
    return moment(date).format(format);
}

//日期过滤器
/*Vue.filter('date', function (value)
{
    if (!value) return ''
    value = value.toString()
    return value.charAt(0).toUpperCase() + value.slice(1)
})*/