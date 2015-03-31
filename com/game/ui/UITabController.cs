﻿﻿using UnityEngine;
using System.Collections.Generic;

namespace com.game.module
{
    /// <summary>
    /// �Զ����ǩ��������
    /// </summary>
    [AddComponentMenu("NGUI/UI/Tab Controller")]
    public class UITabController : MonoBehaviour
    {
        /// <summary>
        /// ����
        /// </summary>
        public UISprite titleSprite;

        List<UITab> tabs = new List<UITab>();

        public void addTab(UITab tab)
        {
            if (!tabs.Contains(tab)) { tabs.Add(tab); }
        }

        public void switchTab(UITab tab)
        {
            foreach (UITab _tab in tabs)
            {
                if (_tab == tab)
                {
                    _tab.setActive(true);
                    if (titleSprite != null) titleSprite.spriteName = _tab.gameObject.name;
                }
                else
                {
                    _tab.setActive(false);
                }

            }
        }

        public void switchTab(int n)
        {
            if (n >= 0 && n < tabs.Count)
            {
                switchTab(tabs[n]);
            }
        }
    }

}