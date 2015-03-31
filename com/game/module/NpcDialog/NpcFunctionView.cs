﻿﻿﻿using com.game.manager;
using com.game.module.test;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/08 11:47:23 
 * function: Npc面板功能信息
 * *******************************************************/

namespace com.game.module.NpcDialog
{
    public class NpcFunctionView : BaseView<NpcFunctionView>
    {
        private UILabel _npcSpeakLabel;
        private uint _currentNpcId;
        private NpcDialogModel _npcDialogModel;

        public new void Init()
        {
            _npcSpeakLabel = FindChild("NpcSpeak/Label").GetComponent<UILabel>();

            _npcDialogModel = Singleton<NpcDialogModel>.Instance;
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            _currentNpcId = _npcDialogModel.CurrentNpcId;
            var sysNpcVo = BaseDataMgr.instance.GetNpcVoById(_currentNpcId);
            _npcSpeakLabel.text = "[00ff00]" + sysNpcVo.name + ":[-]\n" + sysNpcVo.fixspeak;
        }


    }
}