using System.Collections;
using Assets.Scripts.com.game.consts;
using com.game.module.test;
using com.game.Public.Message;
using com.u3d.bases.display.controler;
using com.u3d.bases.display.vo;
using UnityEngine;
using com.u3d.bases.consts;


/**基础角色属性**/
namespace com.game.vo
{
    public class BaseRoleVo : DisplayVo
    {
        public static uint RandomNumber1 = (uint)Random.Range(1, 100000);
        public static uint RandomNumber2 = (uint) Random.Range(101, 999);

		public float toX;               //要去到的x坐标
		public float toY;               //要去到的y坐标
		public uint mapId;				//所在地图id	
		public float attackTime;        //普通攻击间隔 
		public float attackDistance;    //普通攻击距离
		public bool isDie;              //是否死亡[true:死亡,false:非死亡]
        public uint lastHp = 0;         //上一次的血量

		/** 需加密属性信息(人物和怪物均有） */
        private uint _curMp=0;			  //魔法值
        protected uint _curHp=0;		  //生命值
        private int _level=0;             //当前等级
        private uint _str = 0;			  //力量
        private uint _agi = 0;			  //敏捷
        private uint _phy = 0;			  //体质
        private uint _wit = 0;			  //智力
        private uint _hp=0;					//最大生命值
        private uint _mp=0;					//最大魔法值				
        private uint _attPMin=0;			//最小物理攻击
        private uint _attPMax=0;			//最大物理攻击
        private uint _attMMin=0;			//最小魔法攻击
        private uint _attMMax=0;			//最大魔法攻击
        private uint _defP=0;				//物理防御
        private uint _defM=0;				//魔法防御
        private uint _hit=0;				//命中
        private int _dodge=0;				//闪避
        private uint _crit=0;				//暴击
        private uint _critRatio = 0;		//暴击伤害  
        private uint _flex = 0;			//韧性
        private uint _hurtRe = 0;			//伤害抵挡
        private uint _speed = 0;			//x方向移动速度(1秒钟多少像素)
        private uint _luck = 0;			// 幸运值(怪物无此值) 
        private uint _attackDecrease=0;     //伤害减免

        /**加密后的属性,最好能根据协议传递的一个随机数来计算*/
        private uint _curMpEncode1=1;			  //魔法值
        protected uint _curHpEncode1=1;			  //生命值
        private int _levelEncode1=1;               //当前等级
        private uint _strEncode1 = 1;			  //力量
        private uint _agiEncode1 = 1;			  //敏捷
        private uint _phyEncode1 = 1;			  //体质
        private uint _witEncode1 = 1;			  //智力
        private uint _hpEncode1=1;					//最大生命值
        private uint _mpEncode1=1;					//最大魔法值				
        private uint _attPMinEncode1=1;			//最小物理攻击
        private uint _attPMaxEncode1=1;			//最大物理攻击
        private uint _attMMinEncode1=1;			//最小魔法攻击
        private uint _attMMaxEncode1=1;			//最大魔法攻击
        private uint _defPEncode1=1;				//物理防御
        private uint _defMEncode1=1;				//魔法防御
        private uint _hitEncode1=1;				//命中
        private int _dodgeEncode1=1;				//闪避
        private uint _critEncode1=1;				//暴击
        private uint _critRatioEncode1 = 1;		//暴击伤害  
        private uint _flexEncode1 = 1;			//韧性
        private uint _hurtReEncode1 = 1;			//伤害抵挡
        private uint _speedEncode1 = 1;			//x方向移动速度(1秒钟多少像素)
        private uint _luckEncode1 = 1;			// 幸运值(怪物无此值) 
        private uint _attackDecreaseEncode1=1;     //伤害减免

        /**加密后的属性，第二层保护*/
        private uint _curMpEncode2 = 2;			  //魔法值
        protected uint _curHpEncode2 = 2;			  //生命值
        private int _levelEncode2 = 2;               //当前等级
        private uint _strEncode2 = 2;			  //力量
        private uint _agiEncode2 = 2;			  //敏捷
        private uint _phyEncode2 = 2;			  //体质
        private uint _witEncode2 = 2;			  //智力
        private uint _hpEncode2 = 2;					//最大生命值
        private uint _mpEncode2 = 2;					//最大魔法值				
        private uint _attPMinEncode2 = 2;			//最小物理攻击
        private uint _attPMaxEncode2 = 2;			//最大物理攻击
        private uint _attMMinEncode2 = 2;			//最小魔法攻击
        private uint _attMMaxEncode2 = 2; 			//最大魔法攻击
        private uint _defPEncode2=2;				//物理防御
        private uint _defMEncode2=2;				//魔法防御
        private uint _hitEncode2=2;				//命中
        private int _dodgeEncode2=2;				//闪避
        private uint _critEncode2=2;				//暴击
        private uint _critRatioEncode2 = 2;		//暴击伤害  
        private uint _flexEncode2 = 2;			//韧性
        private uint _hurtReEncode2 = 2;			//伤害抵挡
        private uint _speedEncode2 = 2;			//x方向移动速度(1秒钟多少像素)
        private uint _luckEncode2 = 2;			// 幸运值(怪物无此值) 
        private uint _attackDecreaseEncode2=2;     //伤害减免

        /**干扰数值*/
        private int _interferenceNumber1;
        private int _interferenceNumber2;
        private int _interferenceNumber3;


        /**无需加密的属性*/
        private uint _hurtResist;       //受击抵抗，当该值为100时，怪物受击只会掉血不会有受击动作
        public uint HurtDownResist;     //受击倒抵抗，当该值为100时，怪物受击会被击倒
        private bool _isUnbeatable;     //是否无敌
        public bool NeedKeep;           //需要保持当前状态，不被打断，不可移动，不可被击退
        public float MoveRatio = 1;     //受击后退系数
        public byte job;				//职业 GameConst.JOB_XXX。后面移动到PlayerVo里去，现在还有一部分怪物用到了这个属性

        public ActionControler Controller;

        /**保护值**/
        private const int _maxProtectValue = 100;
        private const float _maxRecoverProtectValueTime = 1.5f;
        private float _recoverProtectValueTime;
        private int _protectValue;
        private bool _isProtecting;

        /**技能突进属性**/
        private float _rushSpeed;
        private float _acceleration = -100f;
        private float _distance;
        private int _direction;
        private bool _isRush;

        /**技能移动属性**/
        private float _moveDistance;
        private float _moveTime;
        private float _moveSpeed;
        private int _moveDirection;
        private bool _isMoving;

        /**技能过程中可控制移动属性**/
        private bool _canCtrlMoveDuringSkill;
        private float _moveSpeedDuringSkill;

        /**硬直属性**/
        private float _hitRecoverTime;
        private bool _isHitRecover = false;

        /**力反馈属性**/
        private float _forceFeedBackTime;
        private bool _isForceFeedBack = false;

        /**浮空属性*/
        //TODO 浮空处理，浮空剩余时间，浮空状态
        private float _leftFloatingTime;
        private bool _isFloating;
        private int _currentFloatingNumber;
        public const int MaxFloatingNumber=32;

        /*新浮空属性*/
        private float _floatingXSpeed = 0f;
        private float _floatingYSpeed = 0f;
        private int _atkerDirection;//攻击者方向 1：右 -1：左

        /**比例属性;*/
        private float _rateMoveSpeed = 1;

        /// <summary>
        /// 设置移动速度比例;
        /// </summary>
        public float RateMoveSpeed
        {
            get
            {
                return _rateMoveSpeed;
            }
            set
            {
                _rateMoveSpeed = value;
            }
        }

		public virtual uint CurHp
		{
		    get
		    {
                if (CheckUint(_curHpEncode1, _curHpEncode2,_curHp))
		        {
                    return _curHp;
		        }
		        HandleCheat();
		        return 1;
		    }
			set
			{
			    _curHp = value;
                EncodeUint(out _curHpEncode1,out _curHpEncode2,_curHp);
			}
		}

        public virtual uint CurMp
        {
            get
            {
                if (CheckUint(_curMpEncode1, _curMpEncode2, _curMp))
                {
                    return _curMp;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _curMp = value;
                EncodeUint(out _curMpEncode1, out _curMpEncode2, _curMp);
            }
        }

        public int Level
        {
            get
            {
                if (CheckInt(_levelEncode1, _levelEncode2, _level))
                {
                    return _level;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _level = value;
                Encodeint(out _levelEncode1, out _levelEncode2, _level);
            }
        }

        public uint Str
        {
             get
            {
                if (CheckUint(_strEncode1, _strEncode2, _str))
                {
                    return _str;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _str = value;
                EncodeUint(out _strEncode1, out _strEncode2, _str);
            }
        }


        public uint Agi
        {
            get
            {
                if (CheckUint(_agiEncode1, _agiEncode2, _agi))
                {
                    return _agi;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _agi = value;
                EncodeUint(out _agiEncode1, out _agiEncode2, _agi);
            }
        }

        public uint Phy
        {
            get
            {
                if (CheckUint(_phyEncode1, _phyEncode2, _phy))
                {
                    return _phy;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _phy = value;
                EncodeUint(out _phyEncode1, out _phyEncode2, _phy);
            }
        }

        public uint Wit
        {
            get
            {
                if (CheckUint(_witEncode1, _witEncode2, _wit))
                {
                    return _wit;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _wit = value;
                EncodeUint(out _witEncode1, out _witEncode2, _wit);
            }
        }

        public uint Hp
        {
             get
            {
                if (CheckUint(_hpEncode1, _hpEncode2, _hp))
                {
                    return _hp;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _hp = value;
                EncodeUint(out _hpEncode1, out _hpEncode2, _hp);
            }
        }

        public uint Mp
        {
            get
            {
                if (CheckUint(_mpEncode1, _mpEncode2, _mp))
                {
                    return _mp;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _mp = value;
                EncodeUint(out _mpEncode1, out _mpEncode2, _mp);
            }
        }

        public uint AttPMin
        {
            get
            {
                if (CheckUint(_attPMinEncode1, _attPMinEncode2, _attPMin))
                {
                    return _attPMin;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _attPMin = value;
                EncodeUint(out _attPMinEncode1, out _attPMinEncode2, _attPMin);
            }
        }

        public uint AttPMax
        {
             get
            {
                if (CheckUint(_attPMaxEncode1, _attPMaxEncode2, _attPMax))
                {
                    return _attPMax;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _attPMax = value;
                EncodeUint(out _attPMaxEncode1, out _attPMaxEncode2, _attPMax);
            }
        }

        public uint AttMMin
        {
            get
            {
                if (CheckUint(_attMMinEncode1, _attMMinEncode2, _attMMin))
                {
                    return _attMMin;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _attMMin = value;
                EncodeUint(out _attMMinEncode1, out _attMMinEncode2, _attMMin);
            }
        }

        public uint AttMMax
        {
             get
            {
                if (CheckUint(_attMMaxEncode1, _attMMaxEncode2, _attMMax))
                {
                    return _attMMax;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _attMMax = value;
                EncodeUint(out _attMMaxEncode1, out _attMMaxEncode2, _attMMax);
            }
        }

        public uint DefP
        {
            get
            {
                if (CheckUint(_defPEncode1, _defPEncode2, _defP))
                {
                    return _defP;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _defP = value;
                EncodeUint(out _defPEncode1, out _defPEncode2, _defP);
            }
        }

        public uint DefM
        {
             get
            {
                if (CheckUint(_defMEncode1, _defMEncode2, _defM))
                {
                    return _defM;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _defM = value;
                EncodeUint(out _defMEncode1, out _defMEncode2, _defM);
            }
        }

        public uint Hit
        {
             get
            {
                if (CheckUint(_hitEncode1, _hitEncode2, _hit))
                {
                    return _hit;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _hit = value;
                EncodeUint(out _hitEncode1, out _hitEncode2, _hit);
            }
        }

        public int Dodge
        {
             get
            {
                if (CheckInt(_dodgeEncode1, _dodgeEncode2, _dodge))
                {
                    return _dodge;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _dodge = value;
                Encodeint(out _dodgeEncode1, out _dodgeEncode2, _dodge);
            }
        }

        public uint Crit
        {
            get
            {
                if (CheckUint(_critEncode1, _critEncode2, _crit))
                {
                    return _crit;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _crit = value;
                EncodeUint(out _critEncode1, out _critEncode2, _crit);
            }
        }

        public uint CritRatio
        {
            get
            {
                if (CheckUint(_critRatioEncode1, _critRatioEncode2, _critRatio))
                {
                    return _critRatio;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _critRatio = value;
                EncodeUint(out _critRatioEncode1, out _critRatioEncode2, _critRatio);
            }
        }

        public uint Flex
        {
             get
            {
                if (CheckUint(_flexEncode1, _flexEncode2, _flex))
                {
                    return _flex;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _flex = value;
                EncodeUint(out _flexEncode1, out _flexEncode2, _flex);
            }
        }

        public uint HurtRe
        {
            get
            {
                if (CheckUint(_hurtReEncode1, _hurtReEncode2, _hurtRe))
                {
                    return _hurtRe;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _hurtRe = value;
                EncodeUint(out _hurtReEncode1, out _hurtReEncode2, _hurtRe);
            }
        }

        public uint Speed
        {
             get
            {
                if (CheckUint(_speedEncode1, _speedEncode2, _speed))
                {
                    return _speed;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _speed = value;
                EncodeUint(out _speedEncode1, out _speedEncode2, _speed);
            }
        }

        public uint Luck
        {
            get
            {
                if (CheckUint(_luckEncode1, _luckEncode2, _luck))
                {
                    return _luck;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _luck = value;
                EncodeUint(out _luckEncode1, out _luckEncode2, _luck);
            }
        }

        public uint AttackDecrease
        {
            get
            {
                if (CheckUint(_attackDecreaseEncode1, _attackDecreaseEncode2, _attackDecrease))
                {
                    return _attackDecrease;
                }
                HandleCheat();
                return 1;
            }
            set
            {
                _attackDecrease = value;
                EncodeUint(out _attackDecreaseEncode1, out _attackDecreaseEncode2, _attackDecrease);
            }
        }

        /// <summary>
        ///     Uint类型的加密操作
        /// </summary>
        /// <param name="encodeValue1"></param>
        /// <param name="encodeValue2"></param>
        /// <param name="originalValue"></param>
        private void EncodeUint(out uint encodeValue1, out uint encodeValue2, uint originalValue)
        {
            encodeValue1 = ((originalValue + RandomNumber2) <<4) + RandomNumber1;
            encodeValue2 = ((originalValue + RandomNumber1) <<2) + RandomNumber2;
            _interferenceNumber1 = (int)(encodeValue1 - Random.Range(-99, 99));
            _interferenceNumber2 = (int)(encodeValue2 - Random.Range(-99, 99));
            _interferenceNumber3 = (int)(encodeValue1 - Random.Range(-99, 99));
        }

        /// <summary>
        /// Uint类型的检查
        /// </summary>
        /// <param name="encodeValue1"></param>
        /// <param name="encodeValue2"></param>
        /// <param name="originalValue"></param>
        /// <returns></returns>
        private bool CheckUint(uint encodeValue1, uint encodeValue2, uint originalValue)
        {
            return ((encodeValue1 == ((originalValue + RandomNumber2) << 4) + RandomNumber1) &&
                   (encodeValue2 == ((originalValue + RandomNumber1) << 2) + RandomNumber2))||
                   (encodeValue1==1&&encodeValue2==2&&originalValue==0);
        }

        /// <summary>
        ///     Int类型的加密操作
        /// </summary>
        /// <param name="encodeValue1"></param>
        /// <param name="encodeValue2"></param>
        /// <param name="originalValue"></param>
        private void Encodeint(out int encodeValue1, out int encodeValue2, int originalValue)
        {
            encodeValue1 = ((originalValue + (int)RandomNumber2) << 4) + (int)RandomNumber1;
            encodeValue2 = ((originalValue + (int)RandomNumber1) << 2) + (int)RandomNumber2;
        }

        /// <summary>
        /// Int类型的检查
        /// </summary>
        /// <param name="encodeValue1"></param>
        /// <param name="encodeValue2"></param>
        /// <param name="originalValue"></param>
        /// <returns></returns>
        private bool CheckInt(int encodeValue1, int encodeValue2, int originalValue)
        {
            return ((encodeValue1 == ((originalValue + (int)RandomNumber2) <<4) + (int)RandomNumber1) &&
                   (encodeValue2 == ((originalValue + (int)RandomNumber1) << 2) + (int)RandomNumber2))||
                   (encodeValue1 == 1 && encodeValue2 == 2 && originalValue == 0);
        }

        /// <summary>
        /// 根据类型设置属性值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void SetAttributeValue(int type, int value)
        {
            switch (type)
            {
                case 1:
                    Str = (uint)value;
                    break;
                case 2:
                    Agi = (uint)value;
                    break;
                case 3:
                    Phy = (uint)value;
                    break;
                case 4:
                    Wit = (uint)value;
                    break;
                case 5:
                    CurHp = (uint)value;
                    break;
                case 6:
                    CurMp = (uint)value;
                    break;
                case 7:
                    Mp = (uint)value;
                    break;
                case 8:
                    AttPMin = (uint)value;
                    break;
                case 9:
                    AttPMax = (uint)value;
                    break;
                case 10:
                    AttMMin = (uint)value;
                    break;
                case 11:
                    AttMMax = (uint)value;
                    break;
                case 12:
                    DefP = (uint)value;
                    break;
                case 13:
                    DefM = (uint)value;
                    break;
                case 14:
                    Hit = (uint)value;
                    break;
                case 15:
                    Dodge = (int)value;
                    break;
                case 16:
                    Crit = (uint)value;
                    break;
                case 17:
                    CritRatio = (uint)value;
                    break;
                case 18:
                    Flex = (uint)value;
                    break;
                case 19:
                    HurtRe = (uint)value;
                    break;
                case 20:
                    Speed = (uint)value;
                    break;
                case 21:
                    Luck = (uint)value;
                    break;

            }
        }

        /// <summary>
        /// 根据类型获取属性功能
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public uint GetAttributeValue(int type)
        {
            uint result = 0;
            switch (type)
            {
                case 1:
                    result = Str;
                    break;
                case 2:
                    result = Agi;
                    break;
                case 3:
                    result = Phy;
                    break;
                case 4:
                    result = Wit;
                    break;
                case 5:
                    result = CurHp;
                    break;
                case 6:
                    result = CurMp;
                    break;
                case 7:
                    result = Mp;
                    break;
                case 8:
                    result = AttPMin;
                    break;
                case 9:
                    result = AttPMax;
                    break;
                case 10:
                    result = AttMMin;
                    break;
                case 11:
                    result = AttMMax;
                    break;
                case 12:
                    result = DefP;
                    break;
                case 13:
                    result = DefM;
                    break;
                case 14:
                    result = Hit;
                    break;
                case 15:
                    result = (uint)Dodge;
                    break;
                case 16:
                    result = Crit;
                    break;
                case 17:
                    result = CritRatio;
                    break;
                case 18:
                    result = Flex;
                    break;
                case 19:
                    result = HurtRe;
                    break;
                case 20:
                    result = Speed;
                    break;
                case 21:
                    result = Luck;
                    break;
            }
            return result;
        }

        public uint HurtResist
        {
            get { return _hurtResist; }
            set { _hurtResist = value; }
        }

        public bool IsUnbeatable
        {
            get { return _isUnbeatable; }
            set
            {
                _isUnbeatable = value;
            }
        }

        public float LeftFloatingTime
        {
            get { return _leftFloatingTime; }
            set { _leftFloatingTime = value; }
        }

        public float HitRecoverTime
        {
            get { return _hitRecoverTime; }
            set { _hitRecoverTime = value; }
        }

        public bool IsHitRecover
        {
            get { return _isHitRecover; }
            set
            {
                StopHitRecover();
                if (_isHitRecover != value/* && value*/)
                {
                    _isHitRecover = value;
                    if (_isHitRecover)
                    {
                        CoroutineManager.StartCoroutine(StartHitRecover());
                    }
                }
            }
        }

        public float ForceFeedBackTime
        {
            get { return _forceFeedBackTime; }
            set { _forceFeedBackTime = value; }
        }

        public bool IsForceFeedBack
        {
            get { return _isForceFeedBack; }
            set
            {
                if (_isForceFeedBack != value && value)
                {
                    CoroutineManager.StartCoroutine(StartForceFeedBack());
                }
                _isForceFeedBack = value;
            }
        }

        /*
        public bool IsFloating
                {
                    get { return _isFloating; }
                    set
                    {
                        if (_isFloating != value && value)
                        {
                            CoroutineManager.StartCoroutine(StartFloating());
                        }
                        _isFloating = value;
                    }
                }*/
        

        public int CurrentFloatingNumber
        {
            get { return _currentFloatingNumber; }
            set { _currentFloatingNumber = value; }
        }

        public int ProtectValue
        {
            get { return _protectValue; }
            set 
            {
                if (value <= _maxProtectValue && value >= 0)//保护值在[0-100]的区间内
                {
                    _protectValue = value;
                    if(_protectValue > 0)
                    {
                        if (_protectValue >= _maxProtectValue)
                        {
                            _isProtecting = true;
                        }
                    }
                    else
                    {
                        _isProtecting = false;
                    }
                    _recoverProtectValueTime = _maxRecoverProtectValueTime;
                    CoroutineManager.StartCoroutine(StartRecoverProtectValue());
                }
            }
        }

        public bool IsProtecting
        {
            get { return _isProtecting; }
        }

        public float RushSpeed
        {
            get { return _rushSpeed; }
            set { _rushSpeed = value; }
        }

        public float Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        public float Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        public int Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public bool IsRush
        {
            get { return _isRush; }
            set 
            {
                if (_isRush != value)
                {
                    Controller.MoveController.StopRush();
                    if(value)
                    {
                        Controller.MoveController.StartRush();
                    }
                }
                _isRush = value; 
            }
        }

        public float MoveDistance
        {
            get { return _moveDistance; }
            set { _moveDistance = value; }
        }

        public float MoveTime
        {
            get { return _moveTime; }
            set { _moveTime = value; }
        }

        public int MoveDirection
        {
            get { return _moveDirection; }
            set { _moveDirection = value; }
        }

        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
        }

        public bool IsMoving
        {
            get { return _isMoving; }
            set
            {
                if (_isMoving != value)
                {
                    Controller.MoveController.StopMove();
                    if (value)
                    {
                        Controller.MoveController.StartMove();
                    }
                }
                _isMoving = value;
            }
        }

        public bool CanCtrlMoveDuringSkill
        {
            get { return _canCtrlMoveDuringSkill; }
            set
            {
                if(_canCtrlMoveDuringSkill != value)
                {
                    Controller.MoveController.StopCtrlMoveDuringSkill();
                    if(value)
                    {
                        Controller.MoveController.StartCtrlMoveDuringSkill();
                    }
                }
                _canCtrlMoveDuringSkill = value; 
            }
        }

        public float MoveSpeedDuringSkill
        {
            get { return _moveSpeedDuringSkill; }
            set { _moveSpeedDuringSkill = value; }
        }

        public float FloatingXSpeed
        {
            get { return _floatingXSpeed; }
            set { _floatingXSpeed = value; }
        }

        public float FloatingYSpeed
        {
            get { return _floatingYSpeed; }
            set { _floatingYSpeed = value; }
        }

        public int AtkerDirection
        {
            get { return _atkerDirection; }
            set { _atkerDirection = value; }
        }

        /// <summary>
        /// 浮空处理
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartFloatin()
        {
            Controller.StatuController.SetHurtDownImmediately();
            Controller.StatuController.StopAnimation();
            for (int i = 0; i < 3; i++)
            {
                var pos = Controller.Me.GoCloth.transform.localPosition;
                pos.y += 0.5f;
                Controller.Me.GoCloth.transform.localPosition = pos;
                yield return 0;
            }
            _leftFloatingTime = 0.5f;
            while (_leftFloatingTime>0)
            {
                yield return 0;
                var pos = Controller.Me.GoCloth.transform.localPosition;
                pos.y = 1.5f - (0.5f - _leftFloatingTime) * (0.5f - _leftFloatingTime) * 5f;
                Controller.Me.GoCloth.transform.localPosition = pos;
                _leftFloatingTime -= Time.deltaTime;
            }
            for (int i = 0; i < 3; i++)
            {
                var pos = Controller.Me.GoCloth.transform.localPosition;
                pos.y -= 0.5f;
                if (pos.y <= 0)
                {
                    pos.y = 0;
                }
                Controller.Me.GoCloth.transform.localPosition = pos;
                yield return 0;
            }
            yield return 0;
            _isFloating = false;
            Controller.StatuController.ResetAnimation();
            yield return 0;
        }

        /// <summary>
        /// 硬直处理
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartHitRecover()
        {
            Controller.StatuController.StopAnimation();
            while (_hitRecoverTime > 0)
            {
                yield return 0;
                _hitRecoverTime -= Time.deltaTime;
            }
            _isHitRecover = false;
            Controller.StatuController.ResetAnimation();
        }

        public void StopHitRecover()
        {
            Controller.StatuController.ResetAnimation();
        }

        /// <summary>
        /// 力反馈处理
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartForceFeedBack()
        {
            Controller.StatuController.StopAnimation();
            while (_forceFeedBackTime > 0)
            {
                yield return 0;
                _forceFeedBackTime -= Time.deltaTime;
            }
            _isForceFeedBack = false;
            Controller.StatuController.ResetAnimation();
        }

        /// <summary>
        /// 保护值处理
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartRecoverProtectValue()
        {
            while (_recoverProtectValueTime > 0)
            {
                yield return 0;
                _recoverProtectValueTime -= Time.deltaTime;
            }
            ProtectValue = 0;
        }

        private void HandleCheat()
        {
            MessageManager.Show("系统监测到您正在作弊，请立即终止当前行为，否则将做封号处理！");
        }

    }
}
