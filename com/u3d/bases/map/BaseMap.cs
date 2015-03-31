﻿﻿using System.Collections.Generic;
﻿﻿﻿﻿﻿using com.u3d.bases.display;
using com.u3d.bases.display.vo;
using com.u3d.bases.display.character;

namespace com.u3d.bases.map
{
    public class BaseMap
    {
        internal ClickVo clicker;
        internal IList<BaseDisplay> objectList;        //场景所有物体列表 
        internal IList<PlayerDisplay> _playerList;     //场景玩家列表
        protected List<MonsterDisplay> _monsterList;   //场景怪物+Boss列表
        private List<NpcDisplay> _npcList; //NPC列表 
        protected List<PetDisplay> _petDisplayList; //Pet列表 

        protected MeDisplay _me;
        protected MapParser parser;
       

        public BaseMap() {
            clicker = new ClickVo(this);
            parser = new MapParser(this);
            objectList = new List<BaseDisplay>();
            _playerList = new List<PlayerDisplay>();
            _monsterList = new List<MonsterDisplay>();
            _npcList = new List<NpcDisplay>();
            _petDisplayList = new List<PetDisplay>();
        }

        /**本角色游戏对象**/

        public MeDisplay me
        {
            get { return _me; }
            set { _me = value; }
        }
        /**地图解析&信息对象**/
        public MapParser mapParser { get { return parser; } }
        /**点中对象记录Vo**/
        public ClickVo clickVo { get { return clicker; } }
        /**取得当前玩家列表**/
        public IList<PlayerDisplay> playerList { get { return _playerList; } }
        /**取得当前怪物列表**/
        public IList<MonsterDisplay> monsterList { get { return _monsterList; } }

        public List<NpcDisplay> NpcDisplayList {
            get { return _npcList; }
        }

        /**本角色与点中物体碰撞时接口方法**/
        virtual public void hitCallback(BaseDisplay target){}
        /**可否处理鼠标点击
        * @return [true:可处理,false:禁止处理]
        * **/
        virtual public bool monseClickEnable() { return false; }
        /**停止自动寻路**/
        virtual public void stopAutoRoad() { }

        /**发送移动坐标**/
        virtual public void tellServer(float x, float y) { }

        /**停止渲染**/
        internal void stopRender() {
            foreach (BaseDisplay item in objectList) 
            {
                item.IsUsing = false;
            }
        }

        /**移除对象**/
        public bool remove(BaseDisplay display)
        {
            if (display == null) return false;
            display.IsUsing = false;
            if (display is PlayerDisplay) _playerList.Remove((PlayerDisplay)display);
            if (display is MonsterDisplay) _monsterList.Remove((MonsterDisplay)display);
            if (objectList.IndexOf(display) != -1)
            {
                objectList.Remove(display);
                display.Dispose();
                return true;
            }
            display.Dispose();
            return false;
        }

        /**销毁场景中所有游戏对象**/
        public void dispose() {
            _playerList.Clear();
            _monsterList.Clear();
            _npcList.Clear();
            BaseDisplay display = null;
            while (objectList.Count>0)
            {
                display = objectList[0];
                remove(display);
            }
            if (_me != null && _playerList.IndexOf(_me) == -1) _playerList.Add(_me);
        }

        public void ShowWhiteEffect()
        {
            foreach (var monsterDisplay in _monsterList)
            {
                monsterDisplay.ShowWhiteColor();
            }
            foreach (var playerDisplay in _playerList)
            {
                playerDisplay.ShowWhiteColor();
            }
        }

        public void RemoveWhiteEffect()
        {
            foreach (var monsterDisplay in _monsterList)
            {
                monsterDisplay.RemoveWhiteColor();
            }
            foreach (var playerDisplay in _playerList)
            {
                playerDisplay.RemoveWhiteColor();
            }
        }
    }
}
