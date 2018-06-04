//根目录
var g_root = "/";

//获取随机数
var static = {
    getRandom: function (min, max)
    {
        return Math.floor(Math.random() * (max - min + 1) + min);
    },
    defalut:null
}