using System;
using com.u3d.bases.debug;
using UnityEngine;

/**资源地址拼凑工具类**/
namespace com.game.utils
{
    public class UrlUtils
    {
        /**根据职业和性别--取得角色模型资源地址
         * @param carser[1:武者,2:剑客,3:方士]
         * @param sex   [0:女,1:男]
         * **/
        public static string roleModeUrl(int carser, int sex)
        {
            if (carser < 1 || sex < 0) return string.Empty;
            return "mode/role/" + carser + "_" + sex + "/" + carser + "_" + sex;
        }

		private static bool isIdValid(string id)
		{
			if (StringUtils.isEmpty(id) || ("0" == id))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

        /**根据资源ID--取得怪物模型资源地址
        * @param id 资源ID(对应MonsterVo.url字段)
        * **/
        public static string monsterModeUrl(string id)
        {
			if (!isIdValid(id)) return String.Empty;
            UnityEngine.Debug.Log("****加载怪物monsterModeUrl()，BIP.assetbundle");
			return "Model/Monster/" + id + "/Model/BIP.assetbundle";
        }

		//根据资源ID--取得怪物半身像资源地址
		public static string monsterBustUrl(string id)
		{
			if (!isIdValid(id)) return String.Empty;
            UnityEngine.Debug.Log("****加载怪物monsterBustUrl()，BIP.assetbundle");
			return "Model/Monster/" + id + "_bust/Model/BIP.assetbundle";
		}

        /**根据资源ID--取得NPC模型资源地址
        * @param id 资源ID
        * **/
        public static string npcModeUrl(string id)
        {
			if (!isIdValid(id)) return String.Empty;
			return "Model/Npc/" + id + "/Model/BIP.assetbundle";
        }

		//根据资源ID--取得NPC半身像资源地址
		public static string npcBustUrl(string id)
		{
			if (!isIdValid(id)) return String.Empty;
			return "Model/Npc/" + id + "_bust/Model/BIP.assetbundle";
		}

        /**根据资源ID--取得武器模型资源地址
        * @param id 资源ID
        * **/
        public static string weaponModeUrl(string id)
        {
			if (!isIdValid(id)) return String.Empty;
            return "mode/weapon/" + id + "/" + id;
        }

        /**根据资源ID--取得坐骑模型资源地址
       * @param id 资源ID
       * **/
        public static string horseModeUrl(string id) {
			if (!isIdValid(id)) return String.Empty;
            return "mode/horse/" + id + "/" + id;
        }

         /**
          * 根据资源ID--取得技能特效资源地址
          * @param id 资源ID(对应特效.url字段)
       * **/
        public static string effectSkillUrl(string id)
        {
			if (!isIdValid(id)) return String.Empty;
            return "effect/skill/" + id + "/" + id;
        }

        /**
         * 根据资源ID--取得技能特效资源地址
         * @param id 资源ID(对应特效.url字段)
        * **/
        public static string effectSkillUrlByIdAndType(string id,int type)
        {
			if (!isIdValid(id)) return String.Empty;
            return "effect/skill/" + id + "/model/"  + type;
        }        

		//技能特效路径
		public static string GetSkillEffectUrl(string effectId)
		{
			if (!isIdValid(effectId)) return String.Empty;
			return "Effect/Skill/" + effectId + ".assetbundle";
		}

        /// <summary>
        /// 获取buff特效资源地址;
        /// </summary>
        /// <param name="effectId"></param>
        /// <returns></returns>
        public static string GetBuffEffectUrl(string effectId)
        {
            return GetSkillEffectUrl(effectId);
        }

		//UI特效路径
		public static string GetUIEffectUrl(string effectId)
		{
			if (!isIdValid(effectId)) return String.Empty;				
			return "Effect/UI/" + effectId + ".assetbundle";
		}

		//Main相机特效路径
		public static string GetMainEffectUrl(string effectId)
		{
			if (!isIdValid(effectId)) return String.Empty;				
			return "Effect/Main/" + effectId + ".assetbundle";
		}

		//剧情场景特效路径
		public static string GetStorySceneEffectUrl(string effectId)
		{
			if (!isIdValid(effectId)) return String.Empty;				
			return "Effect/Story/SceneEffect/" + effectId + ".assetbundle";
		}

		//剧情动画片段特效路径
		public static string GetStoryMovieEffectUrl(string effectId)
		{
			if (!isIdValid(effectId)) return String.Empty;				
			return "Effect/Story/Movie/" + effectId + ".assetbundle";
		}

		//获得剧本资源地址
		public static string GetStoryScriptUrl(string name)
		{
			return "Xml/Story/" + name + ".assetbundle";
		}
    }
}
