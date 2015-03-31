using UnityEngine;
using System.Collections;
using com.u3d.bases.display.controler;
using com.game.data;
using com.u3d.bases.consts;
using com.game;

/* *****************************
 * function : 游戏物理系统
 * ******************************/

namespace com.u3d.bases.physics
{
    public class GamePhysics
    {
        public static float Gravity = 25f;     //世界重力常数

        /// <summary>
        /// 计算抛物运动当前的坐标点
        /// </summary>
        public static void CalculateParabolic(Vector3 _pos, float _xSpeed, float _ySpeed, int _dir, float _gravity, out Vector3 pos, out float ySpeed)
        {
            _pos.x = _pos.x + _xSpeed * _dir * Time.deltaTime;
            _pos.x = AppMap.Instance.mapParser.GetFinalMonsterX(_pos.x);          //控制不让出界
            _pos.y += _ySpeed * Time.deltaTime - Gravity * Time.deltaTime * Time.deltaTime * 0.5f;
            if (_pos.y < _pos.z * 0.2f)//确保目标不掉到地面下
            {
                _pos.y = _pos.z * 0.2f;
            }
            pos = _pos;
            ySpeed = _ySpeed -= _gravity * Time.deltaTime;
        }
    }
}
