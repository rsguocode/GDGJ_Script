/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/14 09:14:06 
 * function: 开发辅助类
 * *******************************************************/
using Com.Game.Module.Copy;
using com.game.module.login;
using com.game.module.test;

namespace com.game.Helper
{
    public static class DevelopHelper
    {
        private static bool _isPaused;

        public static void Init()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playmodeStateChanged = ApplicationPlaymodeStateChangedCallBack;
#endif
        }

        /// <summary>
        ///     编辑器暂停后向服务端发送暂停协议，使调试的过程中不会掉线
        /// </summary>
        private static void ApplicationPlaymodeStateChangedCallBack()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPaused)
            {
                if (!_isPaused)
                {
                    _isPaused = true;
                    Singleton<CopyMode>.Instance.PauseCopy();
                }
            }
            else
            {
                _isPaused = false;
                //Singleton<LoginControl>.Instance.SendHeartMsgDirect();    //先发送心跳，同步服务器时间，然后再做其他事情
                Singleton<CopyMode>.Instance.ResumeCopy();
            }
#endif
        }
    }
}