﻿﻿﻿﻿﻿
/**加载进度条接口**/
namespace com.u3d.bases.loader
{
    public interface ILoadingBar
    {
        /**显示加载条**/
        void show();
        /**移除加载条**/
        void remove();
        /**更新加载进度**/
        void update(int progreess);
    }
}
