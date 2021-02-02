var app = new Vue({
    el: '#app',
    data: {
        userInfo: {},
        realname: '',
        contact: '',
        banner: '',
        alipay: '',
        showImg: '',                 //上传预览图片

        isEditing: false,            //是否编辑模式

        isShowCoverImg: false,       //是否显示图片整体预览

        isFetched: false
    },
    created: function () {
        this.getData();
    },
    methods: {
        //获取数据
        getData: function () {
            var that = this;
            globalLoading.show();
            $.when(getUserInfo())
                .then(function (userInfo) {
                    userInfo = userInfo || [];
                    that.userInfo = userInfo[0] || {};
                })
                .fail(function (e) {
                    $.toast(e.msg || '未知错误');
                })
                .always(function () {
                    globalLoading.hide();
                    that.isFetched = true;
                    that.isEditing = false;
                });
        },
        //检查文件是否是合法图片
        checkFile: function (file) {
            if (file.type.indexOf('image') === -1) {
                return false;
            }
            var allImgExt = ".jpg|.jpeg|.bmp|.png|";
            var extName = file.name.substring(file.name.lastIndexOf(".")).toLowerCase();//（把路径中的所有字母全部转换为小写）
            if (allImgExt.indexOf(extName + "|") === -1) {
                $.toast('仅支持JPG、PNG、JPEG、BMP格式');
                return false;
            }
            return true;
        },
        //选择图片
        selectImage: function (e) {
            var that = this,
                file = e.target.files[0];
            if (file && this.checkFile(file)) {
                that.banner = file;
                //把上传文件转化成base64以上传
                var reader = new FileReader();
                reader.addEventListener('load', function () {
                    that.showImg = reader.result;
                }, false);
                reader.readAsDataURL(file);
            }
            e.target.value = '';
        },
        //保存用户资料
        saveUserInfo: function () {
            var that = this;
            var data = new FormData();
            if (['realname', 'contact','alipay', 'banner'].every(function (key) {
                return !that[key];
            })) {
                return;
            }
            ['realname', 'contact','alipay', 'banner'].forEach(function (key) {
                if (that[key]) {
                    data.append(key, that[key]);
                }
            });

            globalLoading.show();
            $.when(saveUserInfo(data))
                .then(function () {
                    that.getData();
                })
                .fail(function (e) {
                    $.toast(e.msg || '未知错误');
                })
                .always(function () {
                    globalLoading.hide();
                });
        }
    },
    watch: {
        isEditing: function (val) {
            if (val) {
                var userInfo = this.userInfo;
                this.realname = userInfo.realname;
                this.contact = userInfo.contact;
                this.banner = '';
            } else {
                this.showImg = '';
            }
        }
    },
    filters: {
        banner: function (val) {
            val = val || '';
            if (val.indexOf('http') === -1) {
                return 'https://shop.jnlxsm.net' + val;
            }
            return val;
        }
    }
});
