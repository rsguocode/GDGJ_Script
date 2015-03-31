﻿﻿﻿using System;

public class FormulaUtils
{
    /// <summary>
    /// 计算强化后的值 base * pow(ratio, level)
    /// </summary>
    /// <param name="base">基础属性|基础金额</param>
    /// <param name="level">强化等级</param>
    /// <param name="ratio">比率</param>
    /// <returns>强化后的值</returns>
    public static int strength(int @base, int @level, double ratio)
    {
        return (int)Math.Floor(@base * Math.Pow(ratio, @level));
    }


    /// <summary>
    /// 计算仙术升级花费的阅历 baseCost * pow(ratio, level)
    /// </summary>
    /// <param name="baseCost">消耗阅历单位值</param>
    /// <param name="level">仙术等级</param>
    /// <param name="ratio">消耗阅历单位值等级系数</param>
    /// <returns>消耗的阅历数</returns>
    public static int xianshu(int baseCost, int @level, double ratio)
    {
        return (int)Math.Floor(baseCost * Math.Pow(ratio, @level));
    }


    /// <summary>
    /// 角色系统，属性成长曲线（具体细节CFI/全局数值配置表）
    /// </summary>
    /// <param name="lv">角色等级</param>
    /// <returns></returns>
    private static int f(int lv)
    {
        // 角色成长曲线数值, 这些数值以后读表获取，现在先写死
        int a = 1, b = 4;

        return lv * a + b;
    }


    /// <summary>
    /// 强化公式： 装备基础值 + f(lv) * 属性强化率 * 强化等级
    /// </summary>
    /// <param name="baseValue">装备基础值</param>
    /// <param name="lv">等级</param>
    /// <param name="ratio">属性强化率，比率用整数表示，50表示50%</param>
    /// <returns>强化后的数值</returns>
    public static int strength(int baseValue, int lv, int ratio)
    {
        float percent = 0.01f;
        return (int)Math.Floor(baseValue + f(lv) * ratio * lv * percent);
    }


    /// <summary>
    /// 强化费用公式: 基础强化费用 * f(lv)
    /// </summary>
    /// <param name="baseCost">基础强化费用</param>
    /// <param name="lv">强化等级</param>
    /// <returns></returns>
    public static int stregthCost(int baseCost, int lv)
    {
        return baseCost * f(lv);
    }
}
