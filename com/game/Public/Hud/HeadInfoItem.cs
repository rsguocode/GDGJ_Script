using System.Collections.Generic;
using com.game.consts;
using com.game.module.map;
using com.u3d.bases.consts;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/03 11:53:17 
 * function: 头顶信息组件，包括掉血信息，恢复信息
 * *******************************************************/

namespace com.game.Public.Hud
{
    public enum HeadInfoItemType
    {
        TypeNormal,
        TypeCrit,
        TypeDodge,
        TypePetTalentHp,
        TypePetTalentUnbeatable
    }

    public class HeadInfoItem : MonoBehaviour
    {
        private static readonly List<HeadInfoItemType> UpInfoItemTypes = new List<HeadInfoItemType>
        {
            HeadInfoItemType.TypePetTalentHp,
            HeadInfoItemType.TypePetTalentUnbeatable
        };

        private int _dir;
        private UILabel _label;
        private float _moveTime;
        private Transform _selfTransform;
        private float _showTime;
        private Vector3 _startPosition;
        private float _totalMoveTime;
        private float _totalShowTime;
        private HeadInfoItemType _type;
        private float _xSpeed;
        private float _ySpeed;


        public void Init()
        {
            _label = gameObject.GetComponent<UILabel>();
            _selfTransform = transform;
            _totalMoveTime = 0.2f;
        }

        private void Update()
        {
            if (_showTime < _totalShowTime)
            {
                _showTime += Time.deltaTime;
                _moveTime += Time.deltaTime;
                if (_moveTime < _totalMoveTime)
                {
                    _startPosition.x += Time.deltaTime*_xSpeed*_dir;
                    _startPosition.y += Time.deltaTime*_ySpeed;
                }
                _selfTransform.position = _startPosition;
            }
            else
            {
                Dispose();
            }
        }

        public void SetValue(string value, Color color, Vector3 startPosition, int dir, HeadInfoItemType type,
            float totalShowTime, int labelSize)
        {
            _type = type;
            _label.text = value;
            _label.fontSize = labelSize;
            _dir = dir == Directions.Left ? 1 : -1;
            _totalShowTime = totalShowTime;
            SetLabelEffectColor();
            bool result = SetPosition(startPosition);
            if (!result) return;
            SetSpeed();
        }


        private bool SetPosition(Vector3 startPosition)
        {
            startPosition.y = startPosition.y + Random.Range(-0.5f, 0.5f);
            Vector3 pos = MapControl.Instance.MyCamera.MainCamera.WorldToViewportPoint(startPosition);
            if (pos.x <= 0 || pos.x >= 1 || pos.y <= 0 || pos.y >= 1) //屏幕外的不显示
            {
                Dispose();
                return false;
            }
            pos = MapControl.Instance.MyCamera.UiCamera.ViewportToWorldPoint(pos);
            _startPosition = pos;
            _selfTransform.parent = HUDText.Parent.transform;
            _selfTransform.localScale = Vector3.one;
            _selfTransform.localPosition = Vector3.zero;
            _selfTransform.position = _startPosition;
            return true;
        }

        private void SetSpeed()
        {
            _showTime = 0;
            _moveTime = 0;
            if (IsUpHead())
            {
                _xSpeed = 0;
                _ySpeed = 0.5f;
            }
            else
            {
                _xSpeed = Random.Range(0.5f, 2.4f);
                _ySpeed = Random.Range(-1f, 1f);
            }
        }

        private void SetLabelEffectColor()
        {
            switch (_type)
            {
                case HeadInfoItemType.TypeNormal:
                    _label.gradientBottom = ColorConst.NormalGradientBottom;
                    _label.gradientTop = ColorConst.NormalGradientTop;
                    break;
                case HeadInfoItemType.TypeCrit:
                    _label.gradientBottom = ColorConst.CritGradientBottom;
                    _label.gradientTop = ColorConst.CritGradientTop;
                    break;
                case HeadInfoItemType.TypeDodge:
                    _label.gradientBottom = ColorConst.DodgeGradientBottom;
                    _label.gradientTop = ColorConst.DodgeGradientTop;
                    break;
                case HeadInfoItemType.TypePetTalentHp:
                    _label.gradientBottom = ColorConst.AddHpGradientBottom;
                    _label.gradientTop = ColorConst.AddHpGradientTop;
                    break;
                case HeadInfoItemType.TypePetTalentUnbeatable:
                    _label.gradientBottom = ColorConst.UnbeatableGradientBottom;
                    _label.gradientTop = ColorConst.UnbeatableGradientTop;
                    break;
                default:
                    _label.gradientBottom = ColorConst.NormalGradientBottom;
                    _label.gradientTop = ColorConst.NormalGradientTop;
                    break;
            }
        }

        private bool IsUpHead()
        {
            return UpInfoItemTypes.Contains(_type);
        }

        /// <summary>
        ///     销毁操作
        /// </summary>
        public void Dispose()
        {
            HeadInfoItemPool.Instance.DeSpawn(gameObject.transform);
        }
    }
}