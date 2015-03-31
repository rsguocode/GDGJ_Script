﻿﻿﻿using System;
using System.Collections.Generic;
using com.game.utils;
using UnityEngine;
using com.net.p8583;
using com.u3d.bases.loader;

/**国际化内容获取类**/
namespace com.game.i18n
{
    public class I18n
    {
        private IDictionary<string, IDictionary<int, string>> socketDict = null;    //网络|系统提示信息字典
        private IDictionary<string, IDictionary<int, string>> moduleDict = null;    //模块提示|显示文字信息字典
		public static I18n instance=new I18n();
		

		public I18n(){
            socketDict = new Dictionary<string, IDictionary<int, string>>(); 
            moduleDict = new Dictionary<string, IDictionary<int, string>>(); 
        }
		
		public void init(){
            //loadSocketTip();
            //loadModuleTip();
		}

        private void loadSocketTip() 
        {
            //TextAsset tepXML = (TextAsset)ResMgr.instance.load("xml/socket"); 
            //XmlDocument doc = InitConfig.loadXMLByContent(tepXML.ToString());
            //XmlNodeList nodeList = doc.SelectSingleNode("i18n").ChildNodes;
            //parserMsgList(socketDict, nodeList);

            TextAsset tepXML = (TextAsset)ResMgr.instance.load("xml/socket"); 
            XMLNode rootNode=XMLParser.Parse(tepXML.ToString());
            XMLNodeList nodeList = rootNode.GetNodeList("i18n>0>tip");
            //Debug.Log("nodeList.length:"+nodeList.Count);
            parserMsgList(socketDict, nodeList);
        }

        private void loadModuleTip()
        {
            //TextAsset tepXML = (TextAsset)ResMgr.instance.load("xml/module"); 
            //XmlDocument doc = InitConfig.loadXMLByContent(tepXML.ToString());
            //XmlNodeList busNodeList = doc.SelectSingleNode("i18n/businessTip").ChildNodes;
            //XmlNodeList uiNodeList = doc.SelectSingleNode("i18n/uiTip").ChildNodes;
            //parserMsgList(moduleDict, busNodeList);
            //parserMsgList(moduleDict, uiNodeList);

            TextAsset tepXML = (TextAsset)ResMgr.instance.load("xml/module");
            XMLNode rootNode = XMLParser.Parse(tepXML.ToString());
            XMLNodeList busNodeList = rootNode.GetNodeList("i18n>0>businessTip>0>tip");
            XMLNodeList uiNodeList = rootNode.GetNodeList("i18n>0>uiTip>0>tip");
            //Debug.Log("busNodeList.len:" + busNodeList.Count + ",uiNodeList.len:" + uiNodeList.Count);
            parserMsgList(moduleDict, busNodeList);
            parserMsgList(moduleDict, uiNodeList);
        }

        private void parserMsgList(IDictionary<string, IDictionary<int, string>> dict, XMLNodeList nodeList) 
        {
            string key = null;
            string subKey = null;
            XMLNodeList childList = null;
            IDictionary<int, string> subDict = null;

            foreach (XMLNode tipNode in nodeList)
            {
                key = tipNode.GetValue("@key"); 
                childList = tipNode.GetNodeList("msg");
                if (StringUtils.isEmpty(key)) continue;

                subDict = new Dictionary<int, string>();
                foreach (XMLNode msgNode in childList)
                {
                    subKey = msgNode.GetValue("@key");
                    if (StringUtils.isEmpty(subKey)) continue;
                    subDict.Add(int.Parse(subKey), msgNode.GetValue("_text"));
                    //Debug.Log("key：" + key + ",subKey：" + subKey + ",msg：" + msgNode.GetValue("_text"));
                }
                dict.Add(key, subDict);
            }
        }

        /*private void parserMsgList(IDictionary<string, IDictionary<int, string>> dict, XmlNodeList nodeList)
        {
            string key = null;
            string subKey = null;
            XmlNodeList childList = null;
            IDictionary<int, string> subDict = null;

            foreach (XmlNode tipNode in nodeList)
            {
                childList = tipNode.ChildNodes;
                key = tipNode.Attributes["key"].Value;
                if (StringUtils.isEmpty(key)) continue;

                subDict = new Dictionary<int, string>();
                foreach (XmlNode msgNode in childList)
                {
                    subKey = msgNode.Attributes["key"].Value;
                    if (StringUtils.isEmpty(subKey)) continue;

                    subDict.Add(int.Parse(subKey), msgNode.InnerText);
                    //Log.info(this, "-parserMsgList()  key：" + key + ",subKey：" + subKey + ",msg：" + msgNode.InnerText);
                }
                dict.Add(key, subDict);
            }
        }*/


        /**根据代码和索引--取得返回码说明
         * @param code 返回码
         * @param key  子key
         * **/
        public String getSocketTip(String code,int key=1)
        {
            if (StringUtils.isEmpty(code)) return String.Empty;
            IDictionary<int, string> subDict = socketDict.ContainsKey(code) ? socketDict[code] : null;
            return subDict != null && subDict.ContainsKey(key) ? subDict[key] : String.Empty;
		}

        /**根据代码和索引--取得模块Tip|UI面板显示文字
         * @param code 模块代码或者UI代码
         * @param key  子key
         * **/
        public String getModuleTip(String code,int key=1)
        {
            if (StringUtils.isEmpty(code)) return String.Empty;
            IDictionary<int, string> subDict = moduleDict.ContainsKey(code) ? moduleDict[code] : null;
            return subDict != null && subDict.ContainsKey(key) ? subDict[key] : String.Empty;
		}
    }
}
