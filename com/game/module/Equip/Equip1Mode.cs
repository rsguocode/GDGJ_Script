using com.game.data;
using com.game.module.test;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Equip
{
    public class Equip1Mode : BaseMode<Equip1Mode>
    {

        public readonly int UPDATE_EQUIP_DESTORY = 1;
        public readonly int UPDATE_EQUIP_INLAY = 2;//镶嵌
        public readonly int UPDATE_EQUIP_INHERIT = 3;//继承
        public readonly int UPDATE_EQUIP_STREN = 4;//强化
        public readonly int UPDATE_EQUIP_MERGE = 5;//充灵
        public readonly int UPDATE_EQUIP_REFINE = 6; //精炼
        public readonly int UPDATE_STREN_RATE = 7;//强化概率加成

        public uint IdPro;//装备概率的Id
        public byte Rate;//概率

        public int StrenCode;
        public void UpdateEquipDestroy()
        {
            this.DataUpdate(this.UPDATE_EQUIP_DESTORY);
        }
        public void UpdateEquipInlay()
        {
            this.DataUpdate(this.UPDATE_EQUIP_INLAY);
        }
        public void UpdateEquipInherit()
        {
            this.DataUpdate(this.UPDATE_EQUIP_INHERIT);
        }
        public void UpdateEquipStren(int strenCode)
        {
            this.StrenCode = strenCode;
            if (this.StrenCode == 0)
                Rate = 0;
            this.DataUpdate(this.UPDATE_EQUIP_STREN);
        }
        public void UpdateEquipMerge()
        {
            this.DataUpdate(this.UPDATE_EQUIP_MERGE);
        }
        public void UpdateEquipRefine()
        {
            this.DataUpdate(this.UPDATE_EQUIP_REFINE);
        }

        public void UpdateStrenProp(uint id, byte rate)
        {
            this.IdPro = id;
            this.Rate = rate;
            this.DataUpdate(this.UPDATE_STREN_RATE);
        }


        


        

    }
}

