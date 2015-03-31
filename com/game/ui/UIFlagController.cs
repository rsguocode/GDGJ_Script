﻿using System;
using System.Collections.Generic;
using UnityEngine;
using com.u3d.bases.loader;
using com.game.utils;

namespace com.game.ui
{
    [AddComponentMenu("NGUI/UI/Flag Controller")]
    public class UIFlagController : MonoBehaviour
    {
        public const float width = 20f;
        public const float height = 20f;
        public const float space = 5f;

        public const string url = "texture/ui/common/common";
        public string selectedSprite = "selected";
        public string unselectedSprite = "unselected";

        public UIDragPageController controller;
        public int count { get; protected set; }

        protected UIAtlas atlas;
        protected GameObject groupGo;
        protected List<UISprite> sprites = new List<UISprite>();
        protected UIGrid grid;

        void Awake()
        {
            atlas = ResMgr.instance.load(url, typeof(UIAtlas)) as UIAtlas;
            groupGo = NGUITools.AddChild(gameObject);
            groupGo.name = "group";

            grid = groupGo.AddComponent<UIGrid>();
            grid.cellWidth = width + space;
            grid.cellHeight = height + space;
        }

        void Update()
        {
            if (controller != null && controller.target != null)
            {
                if (controller.pageCount != count)
                {
                    Tools.clearChildren(groupGo);
                    sprites.Clear();

                    for (int i = 0; i < controller.pageCount; i++)
                    {
                        addSprite(i.ToString());
                    }

                    count = controller.pageCount;
                    grid.repositionNow = true;
                }

                switchFlag(controller.lastPage);
            }
        }

        public void switchFlag(int index)
        {
            foreach (UISprite sprite in sprites)
            {
                sprite.spriteName = (index == Convert.ToInt32(sprite.name)) ? selectedSprite : unselectedSprite;
            }
        }

        protected void addSprite(string id)
        {
            GameObject child = NGUITools.AddChild(groupGo);
            child.transform.localScale = new Vector3(width, height, 1f);
            child.name = id;

            UISprite sprite = child.AddComponent<UISprite>();
            sprite.atlas = atlas;
            sprite.depth = 100;
            sprites.Add(sprite);
        }
    }

}