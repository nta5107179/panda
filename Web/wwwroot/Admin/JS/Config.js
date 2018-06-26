﻿//根目录
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
        /*var newJson = {};
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
        return newJson;*/
        return JSON.parse(JSON.stringify(e));
    },
    CopyJson: function (json)
    {
        return JSON.parse(JSON.stringify(json));
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
        value: String,
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
        '	<option value="">--请选择--</option>' +
        '	<option value="0">顶级类型</option>' +
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
            this.$emit('input', this.value_in.toString())
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
        '                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>' +
        '            </div>' +
        '        </div>' +
        '    </div>' +
        '</div>' +
        ''
})