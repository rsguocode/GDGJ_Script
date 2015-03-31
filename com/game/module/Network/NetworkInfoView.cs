
using com.game.module.test;
using com.u3d.bases.debug;

namespace com.game.module.network
{
        /// <summary>
        /// 初始化加载视图
        /// </summary>
    public class NetworkInfoView : BaseView<NetworkInfoView>
        {

            /// <summary>
            /// 预设路径
            /// </summary>
            public override string url { get { return "UI/NetWork/NetworkView.assetbundle"; } }

            public override ViewLayer layerType
            {
                get { return ViewLayer.TopLayer; }
            }

			public override bool playClosedSound { get { return false; } }

            public override bool waiting{get{return false;}}

            private UISprite info;
            private float delayTime;
            protected override void Init()
            {
                info = FindInChild<UISprite>("icn");
                delayTime = 0.0f;
                info.spriteName = "wifi1";
            }

            public void UpdateDelayInfo(float delayTime)
            {
                if (!ReferenceEquals(gameObject, null))
                {
//                Log.info(this, "delayTime:" + delayTime);
                    if (this.delayTime != delayTime)
                    {
                        this.delayTime = delayTime;

                        if (this.delayTime < 0.1)
                        {
                            info.spriteName = "wifi4";
                        }
                        else if (this.delayTime < 0.3)
                        {
                            info.spriteName = "wifi3";
                        }
                        else
                        {
                            info.spriteName = "wifi2";
                        }
                    }
            }
        }

            public void SetDisConnect()
            {
                if (info != null)
                {
                    info.spriteName = "wifi1";
                    delayTime = 0;
                }
            }

        }

}
