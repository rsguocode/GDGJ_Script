using com.game.module.test;
using Com.Game.Module.Manager;
using System.Collections;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/19 11:17:03 
 * function: 消息提示UI
 * *******************************************************/
namespace com.game.Public.Message
{
    public class MessageView : MonoBehaviour
    {
        public static int Index = 1000;
        private UILabel _content;
        private UISprite _backGroundSprite;
        public UISprite _warnIconSprite;
        public UISprite _tipIconSprite;
        private UIPanel _panel;

        public void Init()
        {
            _content = NGUITools.FindInChild<UILabel>(gameObject, "ContentLabel");
            _backGroundSprite = NGUITools.FindInChild<UISprite>(gameObject, "Background");
            _warnIconSprite = NGUITools.FindInChild<UISprite>(gameObject, "WarnIcon");
            _tipIconSprite = NGUITools.FindInChild<UISprite>(gameObject, "TipIcon");
            _panel = gameObject.GetComponent<UIPanel>();
			SetToLayerTopUi();
        }

		//设置对象层级
        private void SetToLayerTopUi()
		{
			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("TopUI")); 
		}

        public void Show(string message)
        {
            SetToLayerTopUi();
            SetDepth();
            _warnIconSprite.gameObject.SetActive(true);
            _tipIconSprite.gameObject.SetActive(false);

            _content.text = message;
            var args = new Hashtable
            {
                {"y", 120},
                {"time", 2.0},
                {"oncomplete", "OnMessageShowComplete"},
                {"islocal", true},
                {"ignoretimescale", true}
            };
            iTween.MoveTo(gameObject, args);
        }



        /// <summary>
        /// 设置深度，保证后出现的在上面
        /// </summary>
        private void SetDepth()
        {
            Index++;
            _panel.depth = Index;
        }

        private void OnMessageShowComplete()
        {
            MessageItemPool.Instance.DeSpawn(transform);
        }
    }
}
