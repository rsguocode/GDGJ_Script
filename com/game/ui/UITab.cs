﻿﻿using UnityEngine;

namespace com.game.module
{
    /// <summary>
    /// �Զ����ǩ��
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("NGUI/UI/Tab")]
    public class UITab : MonoBehaviour
    {
        public UITabController controller;

        public GameObject target;
        public bool first = false;

        UILabel label;

        void Awake()
        {
            if (controller == null)
            {
                controller = NGUITools.FindInParents<UITabController>(gameObject);
            }

            label = GetComponentInChildren<UILabel>();
            setActive(false);

            if (controller != null)
            {
                controller.addTab(this);
                if (first)
                {
                    controller.switchTab(this);
                }
            }
        }

        void OnPress(bool pressed)
        {
            controller.switchTab(this);
        }

        public void setActive(bool flag)
        {
            NGUITools.SetActive(target, flag);
            if (label != null)
            {
                label.color = flag ? Color.white : NGUITools.ParseColor("EAAE8F", 0);
            }
        }
    }

}