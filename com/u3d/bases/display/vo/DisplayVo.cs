using System;

/**游戏对象基础Vo类*/
namespace com.u3d.bases.display.vo
{
    public class DisplayVo
    {
		public uint Id;                                          //唯一ID
		public string Name;                                      //对象名称
		public float X;                                          //模型的X坐标：单位为unity单位
        private float _y;                                          //模型的Y坐标：单位为Unity单位
		public string ClothUrl;                                  //衣服资源地址
        public int Type = 0;                                     //对象类型
		public string Key { get { return Type + "_" + Id; } }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        //对象的唯一Key
        public Action<BaseDisplay> ModelLoadCallBack;            //注册模型加载完成的回调事件
    }
}
