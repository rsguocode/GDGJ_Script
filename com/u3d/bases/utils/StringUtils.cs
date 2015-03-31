﻿﻿﻿﻿﻿using System;
using UnityEngine;

/**字符串工具类**/
namespace com.bases.utils
{
    internal class StringUtils
    {
        /**判断传入串是否为空串
         * @return true:空串,false:非空串
         * **/
        internal static bool isEmpty(String param)
        {
            return param != null && param.Trim().Length > 0 ? false : true;
        }

        /**比较两个字符串值是否相等
         * @return true:相等,false:不相等
         * **/
        internal static bool isEquals(String param1, String param2)
        {
            if (param1 == null || param2 == null) return false;
            return param1.Equals(param2);
        }

        /**比较两个字符串值是否相等(空串)
         * @return true:相等,false:不相等
         * **/
        internal static bool isEqualsExcFree(String param1, String param2)
        {
            if (isEmpty(param1) || isEmpty(param2)) return false;
            return param1.Equals(param2);
        }


        internal static void addChild(GameObject parent,GameObject child){

            Transform t = child.transform;
            t.parent = parent.transform;
            t.localPosition = new Vector3(0f, 0f, 0);
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            child.layer = parent.layer;
        }

    }
}
