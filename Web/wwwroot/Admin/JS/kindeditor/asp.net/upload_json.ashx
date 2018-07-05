﻿<%@ webhandler Language="C#" class="Upload" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;

public class Upload : IHttpHandler
{
	private Factory m_factory = new Factory();
	private HttpContext context;

	public void ProcessRequest(HttpContext context)
	{
		//文件保存目录路径
        String savePath = Config.g_newsfile;

		//文件保存目录URL
        String saveUrl = Config.g_newsfile;

		//定义允许上传的文件扩展名
		Hashtable extTable = new Hashtable();
		extTable.Add("image", Config.g_img);
		extTable.Add("flash", Config.g_flash);
        extTable.Add("media", Config.g_media);
		extTable.Add("file", Config.g_file);

		this.context = context;

        HttpPostedFile imgFile = null;
        try
        {
            imgFile = context.Request.Files["imgFile"];
        }
        catch(Exception e)
        {
            showError(e.Message);
        }
		if (imgFile == null)
		{
			showError("请选择文件。");
		}

		String dirPath = context.Server.MapPath(savePath);
		if (!Directory.Exists(dirPath))
		{
			showError("上传目录不存在。");
		}

		String dirName = context.Request.QueryString["dir"];
		if (String.IsNullOrEmpty(dirName)) {
			dirName = "image";
		}
		if (!extTable.ContainsKey(dirName)) {
			showError("目录名不正确。");
		}

		String fileName = imgFile.FileName;
		String fileExt = Path.GetExtension(fileName).ToLower();

        if (imgFile.InputStream == null ||
            (dirName == "image" && imgFile.InputStream.Length > Config.g_imgmax) ||
            (dirName == "flash" && imgFile.InputStream.Length > Config.g_flashmax) ||
            (dirName == "media" && imgFile.InputStream.Length > Config.g_mediamax) ||
            (dirName == "file" && imgFile.InputStream.Length > Config.g_filemax))
		{
			showError("上传文件大小超过限制。");
		}

		if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split('|'), fileExt.Substring(1).ToLower()) == -1)
		{
			showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
		}

		//创建文件夹
		if (!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}
		String ymd = DateTime.Now.ToString("yyyy-MM-dd");
		dirPath += ymd + "/";
		saveUrl += ymd + "/";
		if (!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}

		String newFileName = m_factory.OpFile.g_FileSaveName + fileExt;
		String filePath = dirPath + newFileName;

		imgFile.SaveAs(filePath);

		String fileUrl = saveUrl + newFileName;

		Hashtable hash = new Hashtable();
		hash["error"] = 0;
		hash["url"] = fileUrl;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

	private void showError(string message)
	{
		Hashtable hash = new Hashtable();
		hash["error"] = 1;
		hash["message"] = message;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

	public bool IsReusable
	{
		get
		{
			return true;
		}
	}
}