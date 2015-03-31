﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.bases.utils;

/**资源版本管理器**/
namespace com.u3d.bases.loader
{
    public class VerMgr
    {
        private string _language = "";                    //使用语种(zh_CN|en_US)
        private string _basePath = "";                    //资源基础地址(例如：http://192.168.125.168/Resources/zh_CN/)
		private IDictionary<string,int> versionDict;      //资源版本对应表
		

		public VerMgr(){
			versionDict=new Dictionary<string,int>();
		}

        /**初始化资源基础地址和语种
         * @param basePath 资源基础路径
         * @param language 使用语种
         * **/
        public void init(string basePath, string language) {
            if (StringUtils.isEmpty(basePath)) return;
            if (StringUtils.isEmpty(language)) return;
            _language = language;
            string split = String.Empty;

            basePath = basePath.Replace('\\', '/');
            int index = basePath.LastIndexOf("/");
            if (index != basePath.Length - 1) split = "/";
            _basePath = basePath + split + language + "/";  
        }

        /**取得资源地址--版本号**/
		public int getVersion(string url){
			if(StringUtils.isEmpty(url)) return 0;
            return versionDict.ContainsKey(url)?versionDict[url]:0;
		}

        /**取得不带版本号参数的地址**/
        public string getNotVersionUrl(string url)
        {
            if (StringUtils.isEmpty(url)) return String.Empty;
            return basePath + url;
        }

		/**取得带版本参数的地址**/
		public string getVersionUrl(string url){
			if(StringUtils.isEmpty(url)) return String.Empty;
            int version = getVersion(url);
            return url.IndexOf("?") == -1 ? (basePath + url + "?v=" + version) : (basePath + url + "&v=" + version);
		}

        /**取得Resource下的资源地址Relative**/
        public string getLoadUrl(string url){ 
            if(StringUtils.isEmpty(url)) return String.Empty;
            return _language + "/" + url;
        }

        public string language { get { return _language; } }
        public string basePath { get { return _basePath; } }

    }
}
