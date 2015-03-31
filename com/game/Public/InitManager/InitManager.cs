﻿
using System.Collections;
using com.game.manager;
using com.game.utils;
using Holoville.HOTween;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/03/13 02:09:50 
 * function: 初始化管理，分帧处理，避免卡顿
 * *******************************************************/

namespace com.game.Public.InitManager
{
    public class InitManager : MonoBehaviour
    {

        // Use this for initialization
        private void Start()
        {
            StartCoroutine(Init());
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private IEnumerator Init()
        {
            yield return 0;
            ModelEffectManager.Init();  //模型管理初始化处理
            yield return 0;
            HOTween.Init(true, true, true); //初始化hotween;
            yield return 0;
            gameObject.AddComponent<FPS>();//增加FPS显示
        }
    }
}