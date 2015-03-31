using UnityEngine;
using System.Collections;

namespace Com.Game.Utils
{
    public class ShaderUtils
    {

        public static void RefreshShader(Material material,string shaderName)
        {
            Shader kOrgShader;
            Shader kRmapShader;

            kOrgShader = material.shader;
            if (kOrgShader != null)
            {
                kRmapShader = Shader.Find(shaderName);
                if (kRmapShader != null)
                {
                    material.shader = kRmapShader;
                }
            }
                    
        }

    }
}
