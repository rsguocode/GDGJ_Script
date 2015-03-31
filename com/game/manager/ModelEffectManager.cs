/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/27 02:36:33 
 * function: 模型特效管理类，用于便捷的显示各种模型特效
 *           如buff效果，受击效果等
 * *******************************************************/

using com.u3d.bases.debug;
using UnityEngine;

namespace com.game.manager
{
    public class ModelEffectManager
    {
        private static readonly Color White = new Color(1,1,1,1);
        private static readonly Shader PureColorShader = Shader.Find("Sprites/PureColor");
        private static readonly Material PurecolorMaterial = new Material(PureColorShader);
        private static readonly Shader SpriteDefaultShader = Shader.Find("Sprites/Default");
        private static readonly Material SpriteDefaultMaterial = new Material(SpriteDefaultShader);
        private const string ModelShaderName = "Mobile/UnlitCullOff (Supports Lightmap)";

        public static void Init()
        {
        }

        public static void ShowSpriteColorEffect(GameObject model,Color color)
        {
            if(model==null) return;
            SpriteRenderer[] sprites = model.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = color;
            }
            SkinnedMeshRenderer[] renderers = model.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (var renderer in renderers)
            {
                var materials = renderer.materials;
                foreach (var material in materials)
                {
                    if (material.shader.name == ModelShaderName)
                    {
                        material.color = color;
                    }
                }
            }
        }

        public static void RemoveSpriteColorEffect(GameObject model)
        {
            if (model == null) return;
            SpriteRenderer[] sprites = model.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = White;
            }
            SkinnedMeshRenderer[] renderers = model.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (var renderer in renderers)
            {
                var materials = renderer.materials;
                foreach (var material in materials)
                {
                    if (material.shader.name == ModelShaderName)
                    {
                        material.color = White;
                    }
                }
            }
        }

        public static void ShowPuleColor(GameObject model)
        {
            SpriteRenderer[] sprites = model.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.material = PurecolorMaterial;
            }
        }

        public static void RemovePuleColor(GameObject model)
        {
            SpriteRenderer[] sprites = model.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.material = SpriteDefaultMaterial;
            }
        }

        public static void ChangeAlpha(GameObject model,float alpha)
        {
            if (model == null)
            {
                return;
            }
            SpriteRenderer[] sprites = model.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer sprite in sprites)
            {
                Color color = sprite.color;
                color.a = alpha;
                sprite.color = color;
            }
        }
    }
}