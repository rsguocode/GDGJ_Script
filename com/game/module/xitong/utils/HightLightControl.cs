﻿﻿﻿using UnityEngine;
using System.Collections;


/*
 * 按钮高亮 处理 
 */
public class HightLightControl : MonoBehaviour {

    private string spriteName;
    private GameObject hightLight;

	// Use this for initialization
    void Awake()
    {
        Transform tr = transform.FindChild("hightlight");
        if (tr == null)
            tr = transform.FindChild("light");
        hightLight = tr.gameObject;

        if (hightLight)
        {
            hightLight.SetActive(false);
        }
        //hightLight = gameObject.get
    }
    void OnPress(bool isPress)
    {
        if (hightLight)
        {
            if (isPress)
            {
                hightLight.SetActive(true);

            }
            else 
            {
                hightLight.SetActive(false);

            }
        }
       
    }
}
