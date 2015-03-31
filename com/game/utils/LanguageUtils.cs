using com.game.manager;
using UnityEngine;
using System.Collections;

namespace Com.Game.Utils
{
    public class LanguageUtils
    {

        public static string GetAttrString(int type)
        {
            string key = string.Empty;
            if (type == 1)
                key = "Equip.Str";
            else if (type == 2)
                key = "Equip.Agi";
            else if (type == 3)
                key = "Equip.Phy";
            else if (type == 4)
                key = "Equip.Wit";
            else if (type == 5)
                key = "Equip.Hp";
            else if (type == 6)
                key = "Equip.Mp";
            else if (type == 7)
                key = "Equip.AttPMin";
            else if (type == 8)
                key = "Equip.AttPMax";
            else if (type == 9)
                key = "Equip.AttMMin";
            else if (type == 10)
                key = "Equip.AttMMax";
            else if (type == 11)
                key = "Equip.DefP";
            else if (type == 12)
                key = "Equip.DefM";
            else if (type == 13)
                key = "Equip.Hit";
            else if (type == 14)
                key = "Equip.Dodge";
            else if (type == 15)
                key = "Equip.Crit";
            else if (type == 16)
                key = "Equip.CritRatio";
            else if (type == 17)
                key = "Equip.Flex";
            else if (type == 18)
                key = "Equip.HurtRe";
            else if (type == 19)
                key = "Equip.Speed";
            else if (type == 20)
                key = "Equip.Luck";
            //Log.info(this, key + "      ");
            return LanguageManager.GetWord(key);
        }

    }
}
