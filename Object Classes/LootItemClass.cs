using GameOverlay.Drawing;
using Impure.Overlay;
using MDriver.MEME;

namespace Impure.Object_Classes
{
    public class LootItemClass
    {
        public LootItemClass(ulong address)
        {
            this._gameObjectAddress = address;
            this._ComponentAddress = Memory.MEMAPI.GetPointer(_gameObjectAddress + 0x30, 0x18, 0x28, 0);
        }

        public ulong _ComponentAddress = 0x0;
        public ulong _gameObjectAddress = 0x0;
        public ulong _TransformAddress = 0;

        public Requests.Vector3.Vector3f _Position = Requests.Vector3.Vector3f.Zero;

        public bool GC = false;

        private string _gameobjectname = "";
        private string _gameobjectnamecleaned = "";

        public LootType Type = LootType.NULL;
        public SolidBrush Color = Drawing1.Electric_Cyan;

        public float Render_Distance = 250;

        public ulong TransformAddress
        {
            get
            {
                if (_TransformAddress != 0 && Type != LootType.Ore_Bonus && Type != LootType.Animal && Type != LootType.PatrolHeli)
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
                if (_Position == Requests.Vector3.Vector3f.Zero || Type == LootType.Brad || Type == LootType.PatrolHeli || Type == LootType.Vehicle || Type == LootType.Animal || Type == LootType.Ore_Bonus)
                {
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
                    if (_gameobjectname.Contains("/"))
                    {
                        _gameobjectname = _gameobjectname.Substring(_gameobjectname.LastIndexOf("/") + 1);
                    }
                    //_gameobjectname = _gameobjectname.Replace("_", " ");
                    return _gameobjectname;
                }
            }
            set
            {
            }
        }

        public string GameObjectNameCleaned
        {
            get
            {
                if (_gameobjectnamecleaned != "")
                {
                    return _gameobjectnamecleaned;
                }
                else
                {
                    if (GameObjectName.Length <= 1)
                    {
                        //Debug.WriteLine("INVALID NAME WAS FOUND!");
                        return "INVALID";
                    }
                    _gameobjectnamecleaned = GameObjectName;
                    _gameobjectnamecleaned = _gameobjectnamecleaned.Replace(".prefab", "");
                    _gameobjectnamecleaned = _gameobjectnamecleaned.Replace(".entity", "");
                    _gameobjectnamecleaned = _gameobjectnamecleaned.Replace(".deployed", "");
                    _gameobjectnamecleaned = _gameobjectnamecleaned.Replace(" (world)", "");
                    _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("-", "");
                    _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("_", " ");
                    _gameobjectnamecleaned = _gameobjectnamecleaned.Replace(".", " ");
                    if (Type == LootType.Vehicle)
                    {
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("testridablehorse", "Rideable Horse");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("scraptransporthelicopter", "Scrap Heli");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rowboat", "Boat");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rhib", "Rhib");

                    }
                    if (Type == LootType.Weapon)
                    {
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("smg 2", "Custom SMG");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("shotgun waterpipe", "Water Pipe");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("smg thompson", "Tommy");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rifle ak", "AK47");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rifle bolt", "Bolt Rifle");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("pistol python", "Python");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("pistol revolver", "Revolver");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rocket launcher", "Rocket Launcher");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("pistol semiauto", "P2");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rifle semiauto", "SAR");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rifle m39", "M39");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("pistol m92", "M92");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("smg mp5", "MP5");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("pistol nailgun", "Nail Gun");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("shotgun double", "Double-Barrel Shotgun");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("pistol eoka", "Eoka");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rifle l96", "L96");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("rifle lr300", "lr300");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("lmg m249", "M249");
                        _gameobjectnamecleaned = _gameobjectnamecleaned.Replace("shotgun pump", "Pump shotgun");
                    }
                    _gameobjectnamecleaned = (char.ToUpper(_gameobjectnamecleaned[0]) + _gameobjectnamecleaned.Substring(1));
                    return _gameobjectnamecleaned;
                }
            }
            set
            {
                _gameobjectnamecleaned = value;
            }
        }

        public enum LootType
        {
            NULL,
            PatrolHeli,
            Brad,
            Corpse,//done
            Hemp,//done
            Food,//
            Barrel,//done
            Vehicle,//done
            Stash,//done
            Ores,//done
            Ore_Bonus,
            Trap,
            AutoTurret,
            Weapon,//done
            Ammo,//done
            TC,
            World,
            Loot_Basic,//done
            Loot_Elite,//done
            Animal,//done
            Explosion,
            Electrical,
            SupplyDrop,//done
            ShipWreck
        }
    }
}
