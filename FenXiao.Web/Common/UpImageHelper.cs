using Qiniu.IO;
using Qiniu.RS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace FenXiao.Web.Common
{
    public class UpImageHelper
    {
        public string Upload(HttpContext hc)
        {
            var filecollection = hc.Request.Files;
            var fileData = filecollection[0];
            Qiniu.Conf.Config.ACCESS_KEY = "o6njB-MWVByGvLlO5QfAO5r1yxQ2YToBaL99uBlj";
            Qiniu.Conf.Config.SECRET_KEY = "1YFm-nOr3MQkgkiTRg3bzwHZwI4zd9OZo8xfRQMr";
            string bucketName = "ouredaimage";
            PutPolicy put = new PutPolicy(bucketName);
            string uptoken = put.Token();
            PutExtra extra = new PutExtra();
            IOClient client = new IOClient();
            var ext = fileData.FileName.Substring(fileData.FileName.LastIndexOf('.') + 1).ToLower();
            var result = client.Put(uptoken, Guid.NewGuid().ToString() + "." + ext, fileData.InputStream, extra);
            return "http://ouredaimage.qiniudn.com/" + result.key;
        }
    }
    /// <summary>
    /// UEditor编辑器通用上传类
    /// </summary>
    public class UploadHelper
    {
        string _state = "SUCCESS";

        string _url;
        string _currentType;
        string _uploadpath;
        string _filename;
        string _originalName;
        HttpPostedFileBase _uploadFile;

        public Hashtable UpFile(HttpPostedFileBase file, string pathbase, string[] filetype, int size)
        {
            var currUrl = DateTime.Now.ToString("yyyy-MM-dd") + "/";
            pathbase = pathbase + currUrl;

            _uploadpath = HttpContext.Current.Server.MapPath(pathbase);//获取文件上传路径

            try
            {
                _uploadFile = file;
                _originalName = _uploadFile.FileName;

                //目录创建
                CreateFolder();

                //格式验证
                if (CheckType(filetype))
                {
                    _state = "不允许的文件类型";
                }
                //大小验证
                if (CheckSize(size))
                {
                    _state = "文件大小超出网站限制";
                }
                //保存图片
                if (_state == "SUCCESS")
                {
                    _filename = ReName();
                    _uploadFile.SaveAs(_uploadpath + _filename);
                    _url = currUrl + _filename;
                }
            }
            catch (Exception)
            {
                _state = "未知错误";
                _url = "";
            }
            return GetUploadInfo();
        }

        /**
 * 上传涂鸦的主处理方法
  * @param HttpContext
  * @param string
  * @param  string[]
  *@param string
  * @return Hashtable
 */
        public Hashtable UpScrawl(HttpPostedFileBase file, string pathbase, string tmppath, string base64Data)
        {
            var currUrl = DateTime.Now.ToString("yyyy-MM-dd") + "/";
            pathbase = pathbase + currUrl;
            _uploadpath = HttpContext.Current.Server.MapPath(pathbase);//获取文件上传路径
            FileStream fs = null;
            try
            {
                //创建目录
                CreateFolder();
                //生成图片
                _filename = Guid.NewGuid() + ".png";
                fs = File.Create(_uploadpath + _filename);
                byte[] bytes = Convert.FromBase64String(base64Data);
                fs.Write(bytes, 0, bytes.Length);

                _url = currUrl + _filename;
            }
            catch (Exception)
            {
                _state = "未知错误";
                _url = "";
            }
            finally
            {
                if (fs != null) fs.Close();
                DeleteFolder(HttpContext.Current.Server.MapPath(tmppath));
            }
            return GetUploadInfo();
        }

        /**
* 获取文件信息
* @param context
* @param string
* @return string
*/
        public string GetOtherInfo(string field)
        {
            string info = null;
            if (HttpContext.Current.Request.Form[field] != null && !String.IsNullOrEmpty(HttpContext.Current.Request.Form[field]))
            {
                info = field == "fileName" ? HttpContext.Current.Request.Form[field].Split(',')[1] : HttpContext.Current.Request.Form[field];
            }
            return info;
        }

        /**
     * 获取上传信息
     * @return Hashtable
     */
        private Hashtable GetUploadInfo()
        {
            var infoList = new Hashtable { { "state", _state }, { "url", _url } };

            if (_currentType != null)
                infoList.Add("currentType", _currentType);
            if (_originalName != null)
                infoList.Add("originalName", _originalName);
            return infoList;
        }

        /**
     * 重命名文件
     * @return string
     */
        private string ReName()
        {
            return Guid.NewGuid() + GetFileExt();
        }

        /**
     * 文件类型检测
     * @return bool
     */
        private bool CheckType(string[] filetype)
        {
            _currentType = GetFileExt();
            return Array.IndexOf(filetype, _currentType) == -1;
        }

        /**
     * 文件大小检测
     * @param int
     * @return bool
     */
        private bool CheckSize(int size)
        {
            return _uploadFile.ContentLength >= (size * 1024 * 1024);
        }

        /**
     * 获取文件扩展名
     * @return string
     */
        private string GetFileExt()
        {
            string[] temp = _uploadFile.FileName.Split('.');
            return "." + temp[temp.Length - 1].ToLower();
        }

        /**
     * 按照日期自动创建存储文件夹
     */
        private void CreateFolder()
        {
            if (!Directory.Exists(_uploadpath))
            {
                Directory.CreateDirectory(_uploadpath);
            }
        }

        /**
     * 删除存储文件夹
     * @param string
     */
        public void DeleteFolder(string path)
        {
            //if (Directory.Exists(path))
            //{
            //    Directory.Delete(path, true);
            //}
        }
    }
}