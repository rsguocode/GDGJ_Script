﻿﻿﻿using com.u3d.bases.map;
using UnityEngine;

/**存储--鼠标点中游戏物体**/
namespace com.u3d.bases.display.vo
{
    public class ClickVo
    {
        private readonly BaseMap _map;
        private bool _isClick;              //点中物体状态
        private BaseDisplay _display;       //点中游戏物体

        public ClickVo(BaseMap map) {
            _map = map;
        }

        /**执行回调**/
        public void Call() {
            _isClick = false;
            _map.hitCallback(_display);
        }

        /**执行回调**/
        public void Call(BaseDisplay bd) {
            _isClick = false;
            _map.hitCallback(bd);
        }

        /**保存--鼠标点中游戏物体**/
        public void SaveClicker(BaseDisplay clicker) {
            if (clicker == null) 
            { 
                _isClick = false;
                return; 
            }
            _display = clicker;
            _isClick = true;
        }

        /**判断是否碰撞**/
        public bool IsHit() {
            if (_isClick == false || _display==null) return false;
            GameObject me = _map.me.GoCloth;
            GameObject target = _display.GoCloth;
            if (me == null || target==null) return false;
            float dist = Mathf.Abs(me.transform.position.x - target.transform.position.x); //只判断X轴方向的距离
            bool b = dist <= 0.8f;
            return b;
        }
    }
}
