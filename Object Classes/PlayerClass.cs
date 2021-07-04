using System;
using System.Collections.Generic;
using MDriver.MEME;
using static Impure.Object_Classes.RustStructs;

namespace Impure.Object_Classes
{
    public class PlayerClass
    {

        public PlayerClass(ulong address)
        {
            this._gameObjectAddress = address;
            this._ComponentAddress = Memory.MEMAPI.GetPointer(_gameObjectAddress + 0x30, 0x18, 0x28, 0);
        }

        public bool GC = false;

        public ulong _ComponentAddress = 0x0;
        public ulong _gameObjectAddress = 0x0;
        public ulong _TransformAddress = 0;
        public ulong _BoneDict = 0;
        public ulong _HeadBone = 0;
        public ulong HeldWeapon = 0;
        public int HeldWeaponCategory = 0;

        public float Render_Distance = 500;
        public float Bone_Render_Distance = 350;


        public bool isLocalPlayer = false;
        public bool IsNPC = false;
        public bool IsSleeping = false;
        public bool IsFriend = false;
        public bool IsHitlist = false;

        private Requests.Vector3.Vector3f _Position = Requests.Vector3.Vector3f.Zero;

        private string _gameobjectname = "";
        private string _playername = "";
        private string _helditem = "";
        private bool _visible = true;
        private bool _IsCorpse = true;


        private DateTime LastReadTime = DateTime.Now;

        public static List<string> Friends = new List<string> { "off_white", "Subscriptions", "IHuntJews", "zChewy", "MichealScarn", "Z00M69" };



        public ulong TransformAddress
        {
            get
            {
                if (_TransformAddress != 0)
                {
                    return _TransformAddress;
                }
                else
                {
                    _TransformAddress = Memory.MEMAPI.GetPointer(_gameObjectAddress + 0x30, 0x8, 0x38, 0x90);
                    return _TransformAddress;
                }
            }
            set
            {
                _TransformAddress = value;
            }
        }

        public Requests.Vector3.Vector3f Position
        {
            get
            {
                _Position = Memory.MEMAPI.ReadVector3f(TransformAddress);
                if (_Position == Requests.Vector3.Vector3f.Zero)
                {
                    _TransformAddress = 0;
                    _Position = Memory.MEMAPI.ReadVector3f(TransformAddress);
                }
                return _Position;
            }
            set
            {
                _Position = value;
            }
        }

        public string GameObjectName
        {
            get
            {
                if (_gameobjectname != "")
                {
                    return _gameobjectname;
                }
                else
                {
                    _gameobjectname = Memory.MEMAPI.ReadString((ulong)(Memory.MEMAPI.ReadInt64(_gameObjectAddress + 0x60)), 128, false);
                    _gameobjectname = _gameobjectname.Substring(_gameobjectname.LastIndexOf("/") + 1);
                    //_gameobjectname = _gameobjectname.Replace("_", " ");
                    return _gameobjectname;
                }
            }
            set
            {
            }
        }

        public string PlayerName
        {
            get
            {
                if (_playername != "")
                {
                    return _playername;
                }
                else
                {
                    ulong Base_Addr = Memory.MEMAPI.ReadInt64(_ComponentAddress + BasePlayer.displayName);
                    int length = Memory.MEMAPI.ReadInt32(Base_Addr + 0x10);
                    _playername = Memory.MEMAPI.ReadString(Base_Addr + 0x14, length, true);
                    return _playername;
                }
            }
            set
            {
                _playername = value;
            }
        }

        public string HeldItem
        {
            get
            {
                if (_helditem != "" && LastReadTime.AddSeconds(3) > DateTime.Now)
                {
                    return _helditem;
                }
                else
                {
                    _helditem = "";
                    LastReadTime = DateTime.Now;
                    int activeitem = Memory.MEMAPI.ReadInt32(_ComponentAddress + (BasePlayer.clActiveItem));
                    for (ulong Weapon_Slot = 0; Weapon_Slot <= 9; Weapon_Slot++)
                    {
                        ulong Weaponn = Memory.MEMAPI.ReadInt64(Memory.MEMAPI.GetPointer(_ComponentAddress + BasePlayer.inventory, PlayerInventory.containerBelt, itemList, list, (index + (0x8 * Weapon_Slot))));
                        int length = Memory.MEMAPI.ReadInt32(Memory.MEMAPI.GetPointer(Weaponn + Item.info, ItemDefinition.shortname, m_stringLength));

                        if (length > 0 && length < 32)
                        {
                            string WeaponName = Memory.MEMAPI.ReadString(Memory.MEMAPI.GetPointer(Weaponn + Item.info, ItemDefinition.shortname, m_firstChar), length, true);
                            int Category = Memory.MEMAPI.ReadInt32(Memory.MEMAPI.GetPointer(Weaponn + Item.info, ItemDefinition.category));

                            if (Memory.MEMAPI.ReadInt32(Weaponn + Item.uid) == activeitem && activeitem > 0)
                            {
                                HeldWeapon = Weaponn;
                                HeldWeaponCategory = Category;
                                _helditem = WeaponName;
                            }
                        }
                    }

                    return _helditem;
                }
            }
            set
            {
            }
        }

        public bool IsVisable
        {
            get
            {
                ulong _PlayerModel = Memory.MEMAPI.ReadInt64(_ComponentAddress + BasePlayer.playerModel);
                return Memory.MEMAPI.ReadInt16(_PlayerModel + PlayerModel.visible) > 0;
            }
            set
            {
                _visible = value;
            }
        }

        public bool IsDead
        {
            get
            {
                return Memory.MEMAPI.Readbyte(_ComponentAddress + BaseCombatEntity.lifestate) == 1;
            }
            set
            {
                _visible = value;
            }
        }


        public int flag
        {
            get
            {
                return Memory.MEMAPI.Readbyte(_ComponentAddress + BasePlayer.playerFlags);
            }
            set
            {
            }
        }


        public ulong BoneDict
        {
            get
            {
                if (_BoneDict != 0)
                {
                    return _BoneDict;
                }
                else
                {
                    _BoneDict = Memory.MEMAPI.GetPointer(_ComponentAddress + BaseEntity.model, Model.boneTransforms, 0x20);
                    return _BoneDict;
                }
            }
            set
            {
                _BoneDict = value;
            }
        }

        public ulong HeadBone
        {
            get
            {
                if (_HeadBone != 0)
                {
                    return _HeadBone;
                }
                else
                {
                    _HeadBone = Memory.MEMAPI.GetPointer(_ComponentAddress + BaseEntity.model, Model.headBone, 0x0);
                    return _HeadBone;
                }
            }
            set
            {
                _HeadBone = value;
            }
        }

        public Dictionary<int, ulong> cachedBones = new Dictionary<int, ulong>();

        public ulong tryGetBone(int boneId)
        {
            if (cachedBones.ContainsKey(boneId))
            {
                return cachedBones[boneId];
            }
            else
            {
                ulong boneAddr = Memory.MEMAPI.ReadInt64(BoneDict + (ulong)(boneId * 0x8));
                cachedBones[boneId] = boneAddr;
                return boneAddr;
            }
        }

        ////cached bone POS cause fuck rust
        //public Dictionary<int, Requests.Vector2.Vector2f> BonePOS = new Dictionary<int, Requests.Vector2.Vector2f>();
        //
        //public Requests.Vector2.Vector2f GetCachedBonePOS(int boneId)
        //{
        //    if (BonePOS.ContainsKey(boneId))
        //    {
        //        return BonePOS[boneId];
        //    }
        //    else
        //    {                
        //        return new Requests.Vector2.Vector2f(0);
        //    }
        //}

    }
}
