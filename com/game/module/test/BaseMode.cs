
/**通讯交互模块--基础类
 * 1、监听Socket数据返回
 * 2、调用SocketAPI发送数据
 * 3、子类必须覆盖netListener()方法，具体用例可以参考LoginMode.cs类netListener()方法
 * **/

using System.Reflection.Emit;

namespace com.game.module.test
{
    //数据更新代理
    public delegate void DataUpateHandler(object sender, int code);
	public delegate void DataUpateHandlerWithParam(object sender, int code, object param);
    
    
    /// <summary>
    /// 数据更新委托
    /// </summary>
    /// <param name="code">响应码</param>
    public delegate void RefreshDelegate(int code);

    public class BaseMode<T> : Singleton<T> where T : new()
    {
		private DataUpateHandler handlList;
		private DataUpateHandlerWithParam handlListWithParam;

        /// <summary>
        /// 是否显示提示小红点
        /// 每个模块具体实现逻辑
        /// </summary>
        public virtual bool ShowTips// tips
        {
            get { return false; }
        }
        public event DataUpateHandler dataUpdated
		{
			add
			{   //先移除，然后添加防止多次添加，有其他的实现 GetInvokeList().Contains();但是应该要using Linq，为了少用高级特效，现在就先这样实现
				handlList -= value;
				handlList += value;
			}

			remove
			{
				handlList -= value;
			}
		}

		public event DataUpateHandlerWithParam dataUpdatedWithParam
		{
			add
			{   //先移除，然后添加防止多次添加，有其他的实现 GetInvokeList().Contains();但是应该要using Linq，为了少用高级特效，现在就先这样实现
				handlListWithParam -= value;
				handlListWithParam += value;
			}
			
			remove
			{
				handlListWithParam -= value;
			}
		}

        //数据更新必须调用的方法，code为业务协议号码
        public void DataUpdate(int code = 0)
        {
            //线程安全处理
			DataUpateHandler localUpdated = handlList;

            if (localUpdated != null)
            {
                foreach (DataUpateHandler handler in localUpdated.GetInvocationList())
                {                   
					//事件处理
					handler(this, code);                   
                }
            }
        }

		public void DataUpdateWithParam(int code = 0, object param = null)
		{
			//线程安全处理
			DataUpateHandlerWithParam localUpdatedWithParam = handlListWithParam;
			
			if (localUpdatedWithParam != null)
			{
				foreach (DataUpateHandlerWithParam handler in localUpdatedWithParam.GetInvocationList())
				{                   
					//事件处理
					handler(this, code, param);                   
				}
			}
		}

        //更新数据委托，由View注册，Mode数据更新执行
        public RefreshDelegate updateView;


    }



}
