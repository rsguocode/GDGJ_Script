﻿﻿﻿using UnityEngine;
using System.Collections;


/*
 * 按钮选中，一直保持高亮状态
 */
public class HightLightAlways : MonoBehaviour {

    private string spriteName;
    private GameObject hightLight;
    // Use this for initialization
    void Awake()
    {
        Transform tr=transform.FindChild("hightlight");
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
            
        }

    }
    public void disableHightLight()
    {
        if(hightLight.activeInHierarchy)
            hightLight.SetActive(false);
    }
    public void enableHightLight()
    {

        hightLight.SetActive(true);
    }
}
