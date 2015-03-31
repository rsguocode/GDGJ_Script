using UnityEngine;
using com.game.manager;
using com.game.module; 
using com.game.sound;
using com.u3d.bases.debug;

namespace com.game.Public.Message
{
    public class MessageManager
    {

        private static string URL = "UI/Public/MessageView.assetbundle";

        public UISprite tanhao;

        public static MessageView CurItem;

        public static void Init()
        {
            AssetManager.Instance.LoadAsset<GameObject>(URL, LoadMessageViewBack);
       }

        private static void LoadMessageViewBack(GameObject gameObject)
        {
            MessageItemPool.Instance.Init(gameObject.transform);
        }

        public static void Show(string message)
        {
            GameObject view = MessageItemPool.Instance.SpawnMessageItem().gameObject;
            GameObject parent = Viewport.go;
            Transform t = view.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            var messageView = view.GetComponent<MessageView>();
            if (messageView == null)
            {
                messageView = view.AddComponent<MessageView>();
                messageView.Init();
            }
            CurItem = messageView;
            messageView.Show(message);
        }


        /// <summary>
        /// 调用完show调用它，把前面的叹号改为对勾
        /// </summary>
        public  static void SetWarmFalse()
        {
            CurItem._warnIconSprite.gameObject.SetActive(false);
            CurItem._tipIconSprite.gameObject.SetActive(true);
        }
    }
}
