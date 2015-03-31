using System;
/**系统模块--基础控制器类**/
namespace com.game.module.test
{
    public class BaseControl<T> : Singleton<T> where T : new()
    {
        protected BaseControl():base()
        {
            NetListener();
        }


        /**网络数据监听方法**/
        protected virtual void NetListener()
        {
        }
        
    }

   
}
