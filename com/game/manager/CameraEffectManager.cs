using System.Collections;
using com.game.module.map;
using Com.Game.Module.SystemSetting;
using com.game.module.test;
using Holoville.HOTween;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/26 03:49:25 
 * function: 相机效果管理类
 * *******************************************************/

namespace com.game.manager
{
    public class CameraEffectManager : MonoBehaviour
    {
        private static bool isShake = false;
        private static GameObject _mainCameraGameObject;
        private static Camera _mainCamera;

        
        /// <summary>
        /// 相机震动接口，背景地图玩家跟着震动
        /// </summary>
        /// <param name="delaySeconds">延迟开始时间</param>
        /// <param name="shakeSeconds">震动持续时间</param>
        public static void ShakeCamera(float delaySeconds,float shakeSeconds)
        {
            if (Singleton<SystemSettingMode>.Instance.CritShake)
            {
                TweenShakeCamera(delaySeconds, shakeSeconds);
            }
        }

        /// <summary>
        /// 基于iTween位置震动的相机震动方法
        /// </summary>
        /// <param name="delaySeconds">延迟开始时间</param>
        /// <param name="shakeSeconds">震动持续时间</param>
        public static void TweenShakeCamera(float delaySeconds, float shakeSeconds)
        {
            GameObject mapGameObject = AppMap.Instance.mapParser.MapGameObject;   //由震动相机改成震动地图
            mapGameObject.transform.position = Vector3.zero;
			HOTween.Shake(mapGameObject.transform, shakeSeconds, "position", new Vector3(0.08f, 0.15f, 0f));
        }

        public static void NormalAttackShake()
        {
            if (Singleton<SystemSettingMode>.Instance.CritShake)
            {
                GameObject mapGameObject = AppMap.Instance.mapParser.MapGameObject;   //由震动相机改成震动地图
                mapGameObject.transform.position = Vector3.zero;
                HOTween.Shake(mapGameObject.transform, 0.1f, "position", new Vector3(0.05f, 0.05f, 0f));
            }
        }

        public static void ScaleOutCamera(float stepSize, float toSize)
        {
            CoroutineManager.StartCoroutine(StepScaleOutCamera(stepSize, toSize));
        }


        public static void ScaleInCamera(float stepSize, float toSize)
        {
            CoroutineManager.StartCoroutine(StepScaleOutCamera(stepSize, toSize));
        }

        private static IEnumerator StepScaleOutCamera(float stepSize,float toSize)
        {
            float curSize = MapControl.Instance.MyCamera.MainCamera.orthographicSize;
            while (curSize + stepSize < toSize)
            {
                curSize = curSize + stepSize;
                MapControl.Instance.MyCamera.MainCamera.orthographicSize = curSize;
                yield return 0;
            }
            MapControl.Instance.MyCamera.MainCamera.orthographicSize = toSize;
            yield return 0;
        }

        private static IEnumerator StepScaleInCamera(float stepSize, float toSize)
        {
            float curSize = MapControl.Instance.MyCamera.MainCamera.orthographicSize;
            while (curSize > stepSize + toSize)
            {
                curSize = curSize - stepSize;
                MapControl.Instance.MyCamera.MainCamera.orthographicSize = curSize;
                yield return 0;
            }
            MapControl.Instance.MyCamera.MainCamera.orthographicSize = toSize;
            yield return 0;
        } 
    }
}