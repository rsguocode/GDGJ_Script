﻿﻿using UnityEngine;

namespace com.game.module
{
    /// <summary>
    /// �Զ��尴ť��
    /// </summary>
    [AddComponentMenu("NGUI/UI/ButtonX")]
    public class UIButtonX : UIWidgetContainer
    {
        public GameObject target;

        void Awake()
        {
            NGUITools.SetActive(target, false);
        }

        void OnPress(bool pressed)
        {
            if (target.activeSelf != pressed)
            {
                NGUITools.SetActive(target, pressed);
            }
        }
    }

}