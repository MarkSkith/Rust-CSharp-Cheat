using Impure.Object_Classes;
using MDriver.MEME;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using static Impure.Object_Classes.RustStructs;

namespace Impure.Overlay
{
    public class UpdateObjects
    {
        public void UpdateEntityList()
        {
            bool Threads_Started = false;
            while (true)
            {
                //check if game running, if not keep trying to find a new instance of the game.
                Process[] Proc = Process.GetProcessesByName("RustClient");
                if (Proc.Length == 0)
                {
                    Render.Main_Camera = 0;
                    Debug.WriteLine("Proc Closed");
                    while (true)
                    {
                        Process[] pname = Process.GetProcessesByName("RustClient");
                        if (pname.Length > 0)
                        {
                            break;
                        }
                        Thread.Sleep(3000); // pause this app for 3 seconds
                    }

                    Memory.LoadMemory("RustClient");
                    while (Memory.MEMAPI.GetModuleBase(Requests.ModuleName.UnityPlayer) == 0 || Memory.MEMAPI.ReadInt64(Memory.MEMAPI.GetModuleBase(Requests.ModuleName.GameAssembly) + UnityFunctions.BN_Base) == 0)
                    {
                        Thread.Sleep(1000);
                    }
                    UnityFunctions.UnityPlayer_Address = Memory.MEMAPI.GetModuleBase(Requests.ModuleName.UnityPlayer);
                    UnityFunctions.BaseNetworkable = Memory.MEMAPI.ReadInt64(Memory.MEMAPI.GetModuleBase(Requests.ModuleName.GameAssembly) + UnityFunctions.BN_Base);
                    //Debug.WriteLine(UnityFunctions.UnityPlayer_Address.ToString("X"));
                    //Debug.WriteLine(UnityFunctions.BaseNetworkable.ToString("X"));
                }


                //update camera addr
                Render.Main_Camera = Memory.MEMAPI.GetPointer(UnityFunctions.FindTaggedObjectTAG(5) + 0x30, 0x18, 0x2E4);
                Aimbot.Camer_Pos = Render.Main_Camera + 0x148;
                Sky_Dome = UnityFunctions.FindTaggedObject("Sky Dome");
                if (Render.Main_Camera > 1000)
                {
                    var SAFE_PLAYER_DICTIONARY = CachedList.GetSafePlayers(true);
                    foreach (KeyValuePair<ulong, PlayerClass> pair in SAFE_PLAYER_DICTIONARY)
                    {
                        pair.Value.GC = true;
                    }

                    var SAFE_LOOT_DICTIONARY = CachedList.GetSafeLoot(true);
                    foreach (KeyValuePair<ulong, LootItemClass> pair in SAFE_LOOT_DICTIONARY)
                    {
                        pair.Value.GC = true;
                    }

                    //Loop Basenetworkable list for GameObjects
                    long EntCount = Memory.MEMAPI.ReadInt32(Memory.MEMAPI.GetPointer(UnityFunctions.BaseNetworkable + 0xb8, 0x0, 0x10, 0x28, 0x10));
                    //Debug.WriteLine("EntCount : " + EntCount);

                    //save ourself some reads per loop
                    ulong baseaddr = Memory.MEMAPI.GetPointer(UnityFunctions.BaseNetworkable + 0xb8, 0x0, 0x10, 0x28, 0x18, 0x20);

                    for (ulong Entity = 0; Entity <= (ulong)EntCount; Entity++)
                    {
                        ulong component = Memory.MEMAPI.ReadInt64(baseaddr + (0x8 * Entity));
                        ulong gameObject = Memory.MEMAPI.GetPointer(component + 0x10, 0x30, 0x0);
                        string gameobjectName = Memory.MEMAPI.ReadString(Memory.MEMAPI.ReadInt64(gameObject + 0x60));
                        //clean the Name
                        string gameobjectNameClean = "";
                        if (gameobjectName.Contains("/"))
                        {
                            gameobjectNameClean = gameobjectName.Substring(gameobjectName.LastIndexOf("/") + 1);

                        }

                        Int16 tagID = Memory.MEMAPI.ReadInt16(gameObject + 0x54);
                        //get objects
                        //Players And NPCs
                        if (gameobjectName == "LocalPlayer")
                        {
                            var Player = CachedList.SetupPlayer(SAFE_PLAYER_DICTIONARY, gameObject);
                            Player._ComponentAddress = component;
                            Player._gameObjectAddress = gameObject;
                            Player.isLocalPlayer = true;

                            Local.LocalPlayer = Player;

                            //aimbot stuff
                            Aimbot.Input = Memory.MEMAPI.GetPointer(component + BasePlayer.input, PlayerInput.bodyAngles);
                            continue;
                        }

                        if (Options.CB_ESP_Players)
                        {
                            if (tagID == 6 && !gameobjectName.Contains("npc"))
                            {
                                var Player = CachedList.SetupPlayer(SAFE_PLAYER_DICTIONARY, gameObject);
                                Player._ComponentAddress = component;
                                Player._gameObjectAddress = gameObject;

                                if (PlayerClass.Friends.Contains(Player.PlayerName))
                                {
                                    Player.IsFriend = true;
                                }

                                //reset some of the addrs
                                Player.HeadBone = 0;
                                Player.TransformAddress = 0;
                                Player.BoneDict = 0;
                                Player.cachedBones.Clear();
                                //Debug.WriteLine(Player.PlayerName + " : " + gameObject.ToString("X"));
                                continue;
                            }
                        }

                        if (Options.CB_ESP_NPC)
                        {
                            if (tagID == 6 && gameobjectName != "assets/prefabs/npc/scientist/scientistpeacekeeper.prefab")
                            {
                                var Player = CachedList.SetupPlayer(SAFE_PLAYER_DICTIONARY, gameObject);
                                Player._ComponentAddress = component;
                                Player._gameObjectAddress = gameObject;
                                Player.IsNPC = true;
                                Player.PlayerName = "NPC";
                                Player.Render_Distance = 150;
                                Player.Bone_Render_Distance = 75;
                                continue;
                            }
                        }

                        //if(gameobjectName.Contains("autospawn/resource") && !gameobjectNameClean.Contains("ore") && !gameobjectName.Contains("loot"))
                        //{
                        //    var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                        //    LootItem._gameObjectAddress = gameObject;
                        //    LootItem._ComponentAddress = component;
                        //    LootItem.Type = LootItemClass.LootType.Ore_Bonus;
                        //    //Debug.WriteLine(gameobjectName + " : " + component.ToString("X"));
                        //}

                        //ulong nigger = UnityFunctions.FindTaggedObject("sphere");
                        //if (nigger != 0)
                        //{
                        //    var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, nigger);
                        //    LootItem._gameObjectAddress = nigger;
                        //    LootItem._ComponentAddress = component;
                        //    LootItem.Type = LootItemClass.LootType.NULL;
                        //    LootItem.Render_Distance = 9999f;
                        //    continue;
                        //}

                        //World Items
                        if (Options.CB_ESP_Stone || Options.CB_ESP_Sulfur || Options.CB_ESP_Metal)
                        {
                            if (gameobjectNameClean.Contains("ore"))
                            {
                                if (!gameobjectNameClean.Contains("bonus"))
                                {
                                    if (gameobjectNameClean.Contains("stone") && Options.CB_ESP_Stone)
                                    {
                                        var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                        LootItem._gameObjectAddress = gameObject;
                                        LootItem._ComponentAddress = component;
                                        LootItem.Type = LootItemClass.LootType.Ores;

                                        LootItem._Position = new Requests.Vector3.Vector3f(0);
                                        LootItem._Position.Y = LootItem.Position.Y + 0.5f;

                                        LootItem.Color = Drawing1.Blue;
                                    }
                                    if (gameobjectNameClean.Contains("metal") && Options.CB_ESP_Metal)
                                    {
                                        var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                        LootItem._gameObjectAddress = gameObject;
                                        LootItem._ComponentAddress = component;
                                        LootItem.Type = LootItemClass.LootType.Ores;

                                        LootItem._Position = new Requests.Vector3.Vector3f(0);
                                        LootItem._Position.Y = LootItem.Position.Y + 0.5f;

                                        LootItem.Color = Drawing1.Outrageous_Orange;
                                    }
                                    if (gameobjectNameClean.Contains("sulf") && Options.CB_ESP_Sulfur)
                                    {
                                        var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                        LootItem._gameObjectAddress = gameObject;
                                        LootItem._ComponentAddress = component;
                                        LootItem.Type = LootItemClass.LootType.Ores;

                                        LootItem._Position = new Requests.Vector3.Vector3f(0);
                                        LootItem._Position.Y = LootItem.Position.Y + 0.5f;

                                        LootItem.Color = Drawing1.Neon_Carrot;
                                    }
                                    continue;
                                }
                                else
                                {
                                    var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                    LootItem._gameObjectAddress = gameObject;
                                    LootItem._ComponentAddress = component;
                                    LootItem.Type = LootItemClass.LootType.Ore_Bonus;
                                }
                            }
                        }

                        if (Options.CB_ESP_Barrels)
                        {
                            if (gameobjectName.Contains("barrel"))
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Barrel;

                                LootItem._Position = new Requests.Vector3.Vector3f(0);
                                LootItem._Position.Y = LootItem.Position.Y + 0.5f;

                                LootItem.Color = Drawing1.Fluorescent_Red;
                                continue;
                            }
                        }

                        if (Options.tc)
                        {
                            if (
                                // gameobjectName == "assets/prefabs/tools/c4/effects/c4_explosion.prefab" ||
                                //gameobjectName == "assets/prefabs/tools/c4/effects/deploy.prefab" ||
                                gameobjectName == "assets/prefabs/deployable/tool cupboard/cupboard.tool.deployed.prefab"
                            )
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.TC;
                                LootItem.GameObjectNameCleaned = "Tool Cupboared";

                                LootItem.Color = Drawing1.Electric_Violet;
                                LootItem.Render_Distance = 150;
                                continue;
                            }

                        }

                        if (Options.EXPLOSIVE)
                        {
                            if (
                                gameobjectName == "assets/prefabs/weapons/satchelcharge/effects/satchel-charge-explosion.prefab" ||
                                gameobjectName == "assets/prefabs/weapons/satchelcharge/explosive.satchel.entity.prefab" ||
                                gameobjectName == "assets/prefabs/tools/c4/explosive.timed.deployed.prefab" ||
                                gameobjectName == "assets/prefabs/weapons/rocketlauncher/effects/rocket_explosion.prefab" ||
                                gameobjectName == "assets/prefabs/weapons/rocketlauncher/rocket_launcher.entity.prefab" ||
                                gameobjectName == "assets/prefabs/weapons/satchelcharge/explosive.satchel.deployed.prefab"
                            )
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Explosion;
                                LootItem.GameObjectNameCleaned = "ACTIVE RAID";

                                LootItem.Color = Drawing1.Fluorescent_Green;
                                LootItem.Render_Distance = 2000;
                                continue;
                            }

                        }


                        if (Options.CB_ESP_Hemp)
                        {
                            if (gameobjectName.Contains("hemp") && !gameobjectName.Contains("seed"))
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Hemp;

                                LootItem._Position = new Requests.Vector3.Vector3f(0);
                                LootItem._Position.Y = LootItem.Position.Y + 0.5f;

                                LootItem.Color = Drawing1.Screamin_Green;
                                LootItem.Render_Distance = 100;
                                continue;
                            }
                        }

                        if (Options.CB_ESP_Food)
                        {
                            if (gameobjectName.Contains("corn") ||
                                gameobjectName.Contains("pumpkin") ||
                                gameobjectName.Contains("mushrooms") ||
                                gameobjectName.Contains("food") ||
                                gameobjectName.Contains("trash-pile-1") && !gameobjectName.Contains("seed"))
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Food;

                                LootItem._Position = new Requests.Vector3.Vector3f(0);
                                LootItem._Position.Y = LootItem.Position.Y + 0.2f;

                                LootItem.Color = Drawing1.Unmellow_Yellow;
                                LootItem.Render_Distance = 100;
                                continue;
                            }
                        }

                        if (Options.CB_ESP_Vehicles)
                        {
                            if (gameobjectName == "assets/content/vehicles/scrap heli carrier/scraptransporthelicopter.prefab" ||
                                gameobjectName == "assets/content/vehicles/minicopter/minicopter.entity.prefab" ||
                                gameobjectName == "assets/content/vehicles/boats/rowboat/rowboat.prefab" ||
                                gameobjectName == "assets/content/vehicles/boats/rhib/rhib.prefab" ||
                                gameobjectName == "assets/rust.ai/nextai/testridablehorse.prefab")
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Vehicle;

                                LootItem.Color = Drawing1.Electric_Lime;
                                LootItem.Render_Distance = 500;
                                continue;
                            }
                        }

                        if (Options.CB_ESP_DroppedGuns)
                        {
                            if (gameobjectName.Contains("rifle.") ||
                                gameobjectName.Contains("smg.") ||
                                gameobjectName.Contains("shotgun.") ||
                                gameobjectName.Contains("pistol.") ||
                                gameobjectName.Contains("lmg.") ||
                                gameobjectName.Contains("multiplegrenadelauncher") ||
                                gameobjectName.Contains("rocket."))
                            {
                                if (gameobjectName.Contains("(world)") && !gameobjectName.Contains("ammo"))
                                {
                                    var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                    LootItem._gameObjectAddress = gameObject;
                                    LootItem._ComponentAddress = component;
                                    LootItem.Type = LootItemClass.LootType.Weapon;

                                    LootItem._Position = new Requests.Vector3.Vector3f(0);

                                    LootItem.Color = Drawing1.Bright_Teal;
                                    LootItem.Render_Distance = 250;
                                    continue;
                                }
                            }
                        }

                        if (Options.CB_ESP_Ammo)
                        {
                            if (gameobjectName.Contains("ammo."))
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Ammo;

                                LootItem.Color = Drawing1.Bright_Teal;
                                LootItem.Render_Distance = 75;
                                continue;
                            }
                        }

                        if (Options.CB_ESP_Animals)
                        {
                            if (
                                gameobjectName == "assets/rust.ai/agents/wolf/wolf.prefab" ||
                                gameobjectName == "assets/rust.ai/agents/boar/boar.prefab" ||
                                gameobjectName == "assets/rust.ai/agents/bear/bear.prefab" ||
                                gameobjectName == "assets/rust.ai/agents/stag/stag.prefab" ||
                                gameobjectName == "assets/rust.ai/agents/horse/horse.prefab"
                            )
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Animal;

                                LootItem.Color = Drawing1.Electric_Violet;
                                LootItem.Render_Distance = 150;
                                continue;
                            }

                        }

                        if (Options.CB_ESP_HighLoot)
                        {
                            if (
                            gameobjectName == "assets/bundled/prefabs/radtown/crate_elite.prefab" ||
                            gameobjectName == "assets/bundled/prefabs/radtown/crate_normal.prefab" ||
                            gameobjectName == "assets/prefabs/deployable/chinooklockedcrate/codelockedhackablecrate.prefab" ||
                            gameobjectName == "assets/prefabs/npc/m2bradley/bradley_crate.prefab" ||
                            gameobjectName == "assets/prefabs/npc/patrol helicopter/heli_crate.prefab"
                                )
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Loot_Elite;

                                LootItem._Position = new Requests.Vector3.Vector3f(0);
                                LootItem._Position.Y = LootItem.Position.Y + 0.3f;

                                LootItem.Color = Drawing1.Carnation_Pink;
                                LootItem.Render_Distance = 250;
                                continue;
                            }
                        }

                        if (Options.CB_ESP_LowLoot)
                        {
                            if (
                                gameobjectName.Contains("mine") ||
                                gameobjectName == "assets/bundled/prefabs/radtown/crate_basic.prefab" ||
                                gameobjectName == "assets/bundled/prefabs/radtown/crate_normal.prefab" ||
                                gameobjectName == "assets/bundled/prefabs/radtown/crate_normal_2.prefab" ||
                                gameobjectName == "assets/bundled/prefabs/radtown/crate_normal_2_food.prefab" ||
                                gameobjectName == "assets/bundled/prefabs/radtown/crate_normal_2_medical.prefab" ||
                                gameobjectName == "assets/bundled/prefabs/radtown/crate_tools.prefab"
                            )
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Loot_Basic;

                                LootItem._Position = new Requests.Vector3.Vector3f(0);
                                LootItem._Position.Y = LootItem.Position.Y + 0.5f;

                                LootItem.Color = Drawing1.Apricot;
                                LootItem.Render_Distance = 100;
                                continue;
                            }
                        }

                        if (Options.CB_ESP_Supply)
                        {
                            if (
                            gameobjectName == "assets/prefabs/misc/supply drop/supply_drop.prefab")
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.SupplyDrop;

                                LootItem._Position = new Requests.Vector3.Vector3f(0);
                                LootItem._Position.Y = LootItem.Position.Y + 0.5f;

                                LootItem.Color = Drawing1.Bright_Chartreuse;
                                LootItem.Render_Distance = 2000;
                                continue;
                            }
                        }

                        if (Options.CB_ESP_Bags)
                        {
                            if (tagID == 20009)
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Corpse;

                                LootItem.Color = Drawing1.Blue;
                                LootItem.Render_Distance = 100;

                                ulong Base_Addr = Memory.MEMAPI.ReadInt64(LootItem._ComponentAddress + 0x290);
                                int length = Memory.MEMAPI.ReadInt32(Base_Addr + 0x10);
                                if (gameobjectName == "assets/prefabs/misc/item drop/item_drop_backpack.prefab")
                                {
                                    if (length > 0 && length < 32)
                                    {
                                        LootItem.GameObjectNameCleaned = Memory.MEMAPI.ReadString(Base_Addr + 0x14, length, true) + "'s Corpse";
                                    }
                                }
                                continue;
                            }
                        }

                        if (Options.CB_ESP_Traps)
                        {
                            if (gameobjectName == "assets/prefabs/deployable/landmine/landmine.prefab" ||
                                gameobjectName == "assets/prefabs/npc/sam_site_turret/sam_site_turret_deployed.prefab" ||
                                gameobjectName == "assets/prefabs/deployable/single shot trap/guntrap.deployed.prefab" ||
                                gameobjectName == "assets/prefabs/deployable/bear trap/beartrap.prefab" ||
                                gameobjectName == "assets/prefabs/deployable/floor spikes/spikes.floor.prefab" ||
                                gameobjectName == "assets/prefabs/npc/flame turret/flameturret.deployed.prefab")
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.Trap;

                                LootItem.Color = Drawing1.Scarlet;
                                LootItem.Render_Distance = 25;
                                continue;
                            }

                            if (gameobjectName == "assets/prefabs/npc/autoturret/autoturret_deployed.prefab")
                            {
                                var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                LootItem._gameObjectAddress = gameObject;
                                LootItem._ComponentAddress = component;
                                LootItem.Type = LootItemClass.LootType.AutoTurret;

                                LootItem.Color = Drawing1.Scarlet;
                                LootItem.Render_Distance = 100;
                                continue;
                            }

                            if (Options.CB_ESP_Heli)
                            {
                                if (gameobjectName.Contains("assets/prefabs/npc/patrol helicopter/patrolhelicopter.prefab"))
                                {
                                    var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                    LootItem._gameObjectAddress = gameObject;
                                    LootItem._ComponentAddress = component;
                                    LootItem.Type = LootItemClass.LootType.PatrolHeli;
                                    LootItem.GameObjectNameCleaned = "patrolhelicopter";

                                    LootItem.Color = Drawing1.Electric_Orange;
                                    LootItem.Render_Distance = 300;
                                    continue;
                                }
                                if (gameobjectName.Contains("assets/prefabs/npc/m2bradley/bradleyapc.prefab"))
                                {
                                    var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                    LootItem._gameObjectAddress = gameObject;
                                    LootItem._ComponentAddress = component;
                                    LootItem.Type = LootItemClass.LootType.Brad;
                                    LootItem.GameObjectNameCleaned = "bradleyapc";

                                    LootItem.Color = Drawing1.Bright_Green;
                                    LootItem.Render_Distance = 300;
                                    continue;
                                }
                            }
                            if (Options.CB_ESP_Stashes)
                            {
                                if (gameobjectName == "assets/prefabs/deployable/small stash/small_stash_deployed.prefab")
                                {
                                    if (Memory.MEMAPI.ReadInt32(Memory.MEMAPI.GetPointer(gameObject + 0x30, 0x18, 0x28, 0x40, Item.ItemContainerparent, BaseEntity.flags)) == 2048)
                                    {
                                        var LootItem = CachedList.SetupLoot(SAFE_LOOT_DICTIONARY, gameObject);
                                        LootItem._gameObjectAddress = gameObject;
                                        LootItem._ComponentAddress = component;
                                        LootItem.Type = LootItemClass.LootType.Ammo;
                                        LootItem.GameObjectNameCleaned = "Stash";

                                        LootItem.Color = Drawing1.Cerulean;
                                        LootItem.Render_Distance = 200;
                                        continue;
                                    }
                                }
                            }
                        }

                    }

                    //update item lists
                    CachedList.Players = SAFE_PLAYER_DICTIONARY;
                    CachedList.LootItems = SAFE_LOOT_DICTIONARY;
                }
                else
                {
                    Local.LocalPlayer = null;
                    //Debug.WriteLine("Not In Game");
                }

                if (!Threads_Started)
                {
                    Threads_Started = true;

                    Thread th1 = new Thread(MainWindow.LocalClass.Fast_Thread);
                    th1.Start();
                    th1.IsBackground = true;

                    Thread th2 = new Thread(MainWindow.LocalClass.Slow_Thread);
                    th2.Start();
                    th2.IsBackground = true;

                    Thread th3 = new Thread(MainWindow.Renderer.Loop);
                    th3.Start();
                    th3.IsBackground = true;

                    Thread th4 = new Thread(MainWindow.Gamer.GamerChair);
                    th4.Start();
                    th4.IsBackground = true;

                    Thread th5 = new Thread(MainWindow.Gamer.DumpPOS);
                    th5.Start();
                    th5.IsBackground = true;

                    Thread th6 = new Thread(MainWindow.hotkey.KeyLoop);
                    th6.Start();
                    th6.IsBackground = true;

                    Thread th10 = new Thread(MainWindow.LocalClass.RunShoot);
                    th10.Start();
                    th10.IsBackground = true;


                    Thread th8 = new Thread(MainWindow.LocalClass.Speedhack);
                    th8.Start();
                    th8.IsBackground = true;

                    Thread th9 = new Thread(MainWindow.LocalClass.SetFatBullet);
                    th9.Start();
                    th9.IsBackground = true;
                }
                //Save some CPU
                Thread.Sleep(500); //Original 4000 
            }
        }

    }
}
