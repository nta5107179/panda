/*******************************************************************************
* KindEditor - WYSIWYG HTML Editor for Internet
* Copyright (C) 2006-2011 kindsoft.net
*
* @author Roddy <luolonghao@gmail.com>
* @site http://www.kindsoft.net/
* @licence http://www.kindsoft.net/license.php
*******************************************************************************/

KindEditor.plugin('media', function(K) {
	var self = this, name = 'media', lang = self.lang(name + '.'),
		allowMediaUpload = K.undef(self.allowMediaUpload, true),
		allowFileManager = K.undef(self.allowFileManager, false),
		formatUploadUrl = K.undef(self.formatUploadUrl, true),
		extraParams = K.undef(self.extraFileUploadParams, {}),
		filePostName = K.undef(self.filePostName, 'imgFile'),
		uploadJson = K.undef(self.uploadJson, self.basePath + 'php/upload_json.php');
	self.plugin.media = {
		edit : function() {
			var html = [
				'<div style="padding:20px;">',
				//url
				'<div class="ke-dialog-row">',
				'<label for="keUrl" style="width:60px;">' + lang.url + '</label>',
				'<input class="ke-input-text" type="text" id="keUrl" name="url" value="" style="width:260px;" /> &nbsp;',
				'</div>',
				//width
				'<div class="ke-dialog-row">',
				'<label for="keWidth" style="width:60px;">' + lang.width + '</label>',
				'<input type="text" id="keWidth" class="ke-input-text ke-input-number" name="width" value="400" maxlength="4" />',
			    '<input type="checkbox" name="widthauto" class="ke-inline-block" value="right" />宽度自适应',
				'</div>',
				//height
				'<div class="ke-dialog-row">',
				'<label for="keHeight" style="width:60px;">' + lang.height + '</label>',
				'<input type="text" id="keHeight" class="ke-input-text ke-input-number" name="height" value="320" maxlength="4" />',
				'</div>',
				'</div>'
			].join('');
			var dialog = self.createDialog({
				name : name,
				width : 450,
				height : 230,
				title : self.lang(name),
				body : html,
				yesBtn : {
					name : self.lang('yes'),
					click : function(e) {
						var url = K.trim(urlBox.val()),
							width = widthBox.val(),
							height = heightBox.val();
						/*if (url == 'http://' || K.invalidUrl(url)) {
							alert(self.lang('invalidUrl'));
							urlBox[0].focus();
							return;
						}*/
                        if(!widthautoBox[0].checked){
						    if (!/^\d*$/.test(width)) {
							    alert(self.lang('invalidWidth'));
							    widthBox[0].focus();
							    return;
						    }
                        }
						if (!/^\d*$/.test(height) && height!="") {
							alert(self.lang('invalidHeight'));
							heightBox[0].focus();
							return;
						}
						/*var html = K.mediaImg(self.themesPath + 'common/blank.gif', {
								src : url,
								type : K.mediaType(url),
								width : width,
								height : height,
							});*/
						self.insertHtml(url).hideDialog().focus();
					}
				}
			}),
			div = dialog.div,
			urlBox = K('[name="url"]', div),
			widthBox = K('[name="width"]', div),
			heightBox = K('[name="height"]', div),
			widthautoBox = K('[name="widthauto"]', div);

            urlBox.val("此处复制优酷视频的“通用代码”");

            var setUrl=function(){
                var urlVal = urlBox.val();
                urlVal=urlVal.replace(new RegExp(/width='*\d*%*'*/),"width='"+widthBox.val()+"'");
                urlVal=urlVal.replace(new RegExp(/height='*\d*%*'*/),"height='"+heightBox.val()+"'");
                urlBox.val(urlVal);
            }
            var setWH=function(){
                var urlVal = urlBox.val();
                var wRex=new RegExp(/width='*\d*%*'*/);
                var hRex=new RegExp(/height='*\d*%*'*/);

                widthBox.val(wRex.exec(urlVal).toString().replace(new RegExp(/[^\d]+/ig),""));
                heightBox.val(hRex.exec(urlVal).toString().replace(new RegExp(/[^\d]+/ig),""));
            }

            widthautoBox.click(function(){
                if(this.checked){
                    widthBox.val("100%");
                    widthBox[0].disabled=true;
                    heightBox.val("");
                    urlBox[0].disabled=true;
                    setUrl();
                }else{
                    widthBox.val("400");
                    widthBox[0].disabled=false;
                    heightBox.val("320");
                    urlBox[0].disabled=false;
                    setUrl();
                }
            });
            urlBox.change(function(e) {
			    setWH();
		    });
            widthBox.change(function(e) {
			    setUrl();
		    });
		    heightBox.change(function(e) {
			    setUrl();
		    });

			var img = self.plugin.getSelectedMedia();
			if (img) {
				var attrs = K.mediaAttrs(img.attr('data-ke-tag'));
				urlBox.val(attrs.src);
				widthBox.val(K.removeUnit(img.css('width')) || attrs.width || 0);
				heightBox.val(K.removeUnit(img.css('height')) || attrs.height || 0);
			}
			urlBox[0].focus();
			urlBox[0].select();
		},
		'delete' : function() {
			self.plugin.getSelectedMedia().remove();
			// [IE] 删除图片后立即点击图片按钮出错
			self.addBookmark();
		}
	};
	self.clickToolbar(name, self.plugin.media.edit);
});
