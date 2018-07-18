using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace Web.Pages.Admin.JS.kindeditor
{
    public class upload_jsonModel : PageModel
    {
        Factory m_factory = new Factory();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            //�ļ�����Ŀ¼·��
            String savePath = Config.g_filepath+"kindeditor/";

            //�ļ�����Ŀ¼URL
            String saveUrl = Config.g_root + Config.g_filepath + "kindeditor/";

            //���������ϴ����ļ���չ��
            Hashtable extTable = new Hashtable();
            extTable.Add("image", Config.g_imgtype);
            extTable.Add("flash", Config.g_flashtype);
            extTable.Add("media", Config.g_mediatype);
            extTable.Add("file", Config.g_filetype);

            IFormFile imgFile = null;
            try
            {
                imgFile = Request.Form.Files["imgFile"];
            }
            catch (Exception e)
            {
                return showError(e.Message);
            }
            if (imgFile == null)
            {
                return showError("��ѡ���ļ���");
            }

            String dirPath = Path.Combine(CoreClass.HttpContext.HostingEnvironment.WebRootPath, savePath);
            if (!Directory.Exists(dirPath))
            {
                return showError("�ϴ�Ŀ¼�����ڡ�");
            }

            String dirName = Request.Query["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return showError("Ŀ¼������ȷ��");
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.OpenReadStream() == null ||
                (dirName == "image" && imgFile.OpenReadStream().Length > Config.g_imgmax) ||
                (dirName == "flash" && imgFile.OpenReadStream().Length > Config.g_flashmax) ||
                (dirName == "media" && imgFile.OpenReadStream().Length > Config.g_mediamax) ||
                (dirName == "file" && imgFile.OpenReadStream().Length > Config.g_filemax))
            {
                return showError("�ϴ��ļ���С�������ơ�");
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split('|'), fileExt.Substring(1).ToLower()) == -1)
            {
                return showError("�ϴ��ļ���չ���ǲ��������չ����\nֻ����" + ((String)extTable[dirName]) + "��ʽ��");
            }

            //�����ļ���
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            String ymd = DateTime.Now.ToString("yyyy-MM-dd");
            dirPath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String newFileName = m_factory.OpFile.g_FileSaveName + fileExt;
            String filePath = dirPath + newFileName;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                imgFile.CopyTo(fileStream);
            }

            String fileUrl = saveUrl + newFileName;

            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            Response.Headers["Content-Type"] = "text/html; charset=UTF-8";

            return new JsonResult(hash);
        }

        private IActionResult showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            Response.Headers["Content-Type"] = "text/html; charset=UTF-8";

            return new JsonResult(hash);
        }

    }
}