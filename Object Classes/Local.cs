using System;
using System.Threading;
using static Impure.Object_Classes.RustStructs;
using static Impure.Overlay.Options;
using MDriver.MEME;

namespace Impure.Object_Classes
{
    public class Local
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetKeyState(int vKey);

        public static PlayerClass LocalPlayer = null;
        bool tap = false;
        bool shouldoff = false;


        public void SetFatBullet()
        {
            while (true)
            {

                if (CB_FATBULLET2)
                {
                    ulong held = Memory.MEMAPI.Readbyte(LocalPlayer.HeldWeapon);
                    ulong Baseproj = Memory.MEMAPI.Readbyte(held + HeldEntity.BaseProjectile);
                    ulong CreatedProjectiles = Memory.MEMAPI.Readbyte(Baseproj + BaseProjectile.CreatedProjectiles);
                    ulong CreatedProjArray = Memory.MEMAPI.Readbyte(CreatedProjectiles + 0x10);
                    UInt32 size = Memory.MEMAPI.Readbyte(CreatedProjectiles + 0x18);
                    for (ulong i = 0; i < size; i++)
                    {
                        ulong projectil = Memory.MEMAPI.Readbyte(CreatedProjectiles + (0x20 + (i * 0x8)));
                        Memory.MEMAPI.WriteFloat(projectil + 0x2C, 1.0f);
                    }
                }
                Thread.Sleep(1);
            }
        }

        public void RunShoot()
        {
            while (true)
            {
                if (LocalPlayer != null)
                {
                    if (sprintaim)
                    {
                        Memory.MEMAPI.WriteByte(Memory.MEMAPI.GetModuleBase(Requests.ModuleName.GameAssembly) + BlockSprint, 0xc3, true);
                    }
                }
                {
                    Thread.Sleep(1);
                }
            }
        }



        public void Fast_Thread()
        {
            while (true)
            {


                if (LocalPlayer != null)
                {


                    int Key = GetKeyState(88); //70 = X KEY
                    if ((Key & 0x8000) != 0)
                    {
                        if (tap)
                        {
                            tap = false;
                            CB_wateWrite = !CB_wateWrite;
                            WaterKey();
                        }
                    }
                    else
                    {
                        tap = true;
                    }


                    if (CB_Debug)
                    {
                        Memory.MEMAPI.WriteInt32(LocalPlayer._ComponentAddress + BasePlayer.playerFlags, 260);

                    }



                    if (CB_Spooder)
                    {
                        ulong PWM_Addr = Memory.MEMAPI.GetPointer(LocalPlayer._ComponentAddress + BasePlayer.movement, 0);
                        Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.groundAngle, 0f);
                        Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.groundAngleNew, 0f);
                        Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.maxAngleClimbing, 999f);
                        Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.maxAngleWalking, 999f);
                        Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.groundTime, 99999999999999f);
                    }
                }
                Thread.Sleep(1);
            }
        }
        void WaterKey()
        {
            //Form1.CB_walk.Switched = CB_wateWrite;
            ulong PWM_Addr = Memory.MEMAPI.GetPointer(LocalPlayer._ComponentAddress + BasePlayer.movement, 0);
            if (CB_wateWrite)
            {


                Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.groundAngle, 0f);
                Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.groundAngleNew, 0f);
                Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.gravityMultiplier, 0f);
                Memory.MEMAPI.WriteByte(PWM_Addr + PlayerWalkMovement.Flying, 1);
            }
            else
            {
                Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.gravityMultiplier, 2.5f);
                Memory.MEMAPI.WriteByte(PWM_Addr + PlayerWalkMovement.Flying, 0);
            }

        }


        public void Speedhack()
        {
            while (true)
            {


                if (LocalPlayer != null)
                {


                    int Key = GetKeyState(79); //79 = O KEY
                    if ((Key & 0x8000) != 0)
                    {
                        if (tap)
                        {
                            tap = false;
                            //speedhack = !speedhack;
                            //SpeedKey();
                        }
                    }
                    else
                    {
                        tap = true;
                    }
                }
                Thread.Sleep(1);
            }
        }


        public void Slow_Thread()
        {

            /*   if (Form1.checkBox1.Checked)
               {

               }*/


            if (LocalPlayer != null)
            {


                float LastRecoilScale = 99999f;
                ulong LastHeldWeapon = 0;
                string LastHeldWeaponName = "ASS";

                float recoilPitchMax = 0;
                float recoilPitchMin = 0;
                float recoilYawMax = 0;
                float recoilYawMin = 0;
                float movementPenalty = 0;

                while (true)
                {

                    try
                    {
                        string WeaponName = LocalPlayer.HeldItem;
                        //float baseMult = 2f;
                        //Debug.WriteLine(LocalPlayer.HeldWeapon.ToString("X"));
                        //Debug.WriteLine(WeaponName);
                        //Debug.WriteLine(WeaponName);
                        //Debug.WriteLine(Memory.MEMAPI.ReadFloat(Memory.MEMAPI.GetPointer(Local.LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance)).ToString());
                        //Debug.WriteLine(LocalPlayer.HeldWeaponCategory);
                        if (CB_ExtendedMelee && (LocalPlayer.HeldWeaponCategory == 0 || LocalPlayer.HeldWeaponCategory == 5))
                        {
                            if (WeaponName == "bone.club")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "knife.bone")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "knife.butcher")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "candycaneclub")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3.2f);
                            }
                            else if (WeaponName == "knife.combat")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "longsword")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "mace")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "machete")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "pitchfork")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "salvaged.cleaver")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "salvaged.sword")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "spear.stone")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "spear.wooden")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.2f);
                            }
                            else if (WeaponName == "chainsaw")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "hatchet")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "hatchet")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "jackhammer")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "pickaxe")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "rock")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "axe.salvaged")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "hammer.salvaged")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "icepick.salvaged")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 3f);
                            }
                            else if (WeaponName == "stonehatchet")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "stone.pickaxe")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                            else if (WeaponName == "torch")
                            {
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 5.5f);
                            }
                        }

                        if (CB_RecoilScale && LocalPlayer.HeldWeaponCategory == 0 && !WeaponName.Contains("flame"))
                        {

                            if (LocalPlayer.HeldWeapon != 0 && LocalPlayer.HeldItem != "")
                            {
                                //Debug.WriteLine("3");
                                LastHeldWeapon = LocalPlayer.HeldWeapon;
                                //recoilPitchMax = Memory.MEMAPI.ReadFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax));
                                //recoilPitchMin = Memory.MEMAPI.ReadFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin));
                                //recoilYawMax = Memory.MEMAPI.ReadFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax));
                                //recoilYawMin = Memory.MEMAPI.ReadFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin));
                                //movementPenalty = Memory.MEMAPI.ReadFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty));

                                //no sway
                                Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.aimSway), 0f);

                                //LastRecoilScale = RecoilScale;
                                if (WeaponName == "rifle.ak")
                                {


                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), rifleak.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), rifleak.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), rifleak.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), rifleak.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);

                                }
                                if (WeaponName == "lmg.m249")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), lmgm249.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), lmgm249.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), lmgm249.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), lmgm249.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "pistol.nailgun")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), pistolnailgun.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), pistolnailgun.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), pistolnailgun.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), pistolnailgun.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "rifle.m39")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), riflem39.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), riflem39.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), riflem39.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), riflem39.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "rifle.lr300")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), riflelr300.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), riflelr300.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), riflelr300.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), riflelr300.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "rifle.semiauto")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), riflesemiauto.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), riflesemiauto.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), riflesemiauto.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), riflesemiauto.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "smg.2")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), smg2.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), smg2.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), smg2.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), smg2.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "smg.thompson")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), smgthompson.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), smgthompson.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), smgthompson.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), smgthompson.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "smg.mp5")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), smgmp5.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), smgmp5.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), smgmp5.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), smgmp5.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "pistol.revolver")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), pistolrevolver.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), pistolrevolver.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), pistolrevolver.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), pistolrevolver.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                                else if (WeaponName == "pistol.python")
                                {
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMax), pistolpython.recoilPitchMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilPitchMin), pistolpython.recoilPitchMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMax), pistolpython.recoilYawMax * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.recoilYawMin), pistolpython.recoilYawMin * (RecoilScale / 1000f));
                                    Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseProjectile.recoil, RecoilProperties.movementPenalty), 0);
                                }
                            }
                        }

                        //Eoka

                        if (CB_Eoka && LocalPlayer.HeldItem == "pistol.eoka")
                        {
                            Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, FlintStrikeWeapon.successFraction), 999f, true);
                        }

                        //aimbot stuff
                        if (LastHeldWeaponName != LocalPlayer.HeldItem)
                        {
                            LastHeldWeaponName = LocalPlayer.HeldItem;
                            if (LastHeldWeaponName.Contains("pistol.nailgun"))
                            {
                                Aimbot.DickDrop = Aimbot.NailDrop;
                                Aimbot.Horizontal_Factor = 5f;
                            }
                            else if (LastHeldWeaponName.Contains("rifle.m39"))
                            {
                                Aimbot.DickDrop = Aimbot.M39Drop;
                                Aimbot.Horizontal_Factor = 33f;
                            }
                            else if (LastHeldWeaponName.Contains("rifle.l96"))
                            {
                                Aimbot.DickDrop = Aimbot.RifleDrop;
                                Aimbot.Horizontal_Factor = 75f;
                            }
                            else if (LastHeldWeaponName.Contains("rifle.bolt"))
                            {
                                Aimbot.DickDrop = Aimbot.RifleDrop;
                                Aimbot.Horizontal_Factor = 70f;
                            }
                            else if (LastHeldWeaponName.Contains("rifle.semiauto"))
                            {
                                Aimbot.DickDrop = Aimbot.RifleDrop;
                                Aimbot.Horizontal_Factor = 60f;
                            }
                            else if (LastHeldWeaponName.Contains("rifle."))
                            {
                                Aimbot.DickDrop = Aimbot.RifleDrop;
                                Aimbot.Horizontal_Factor = 60f;
                            }
                            else if (LastHeldWeaponName.Contains("pistol."))
                            {
                                Aimbot.DickDrop = Aimbot.PistolDrop;
                                Aimbot.Horizontal_Factor = 26f;
                            }
                            else if (LastHeldWeaponName.Contains("bow.hunting"))
                            {
                                Aimbot.DickDrop = Aimbot.BowDrop;
                                Aimbot.Horizontal_Factor = 8f;
                            }
                            else if (LastHeldWeaponName.Contains("crossbow"))
                            {
                                Aimbot.DickDrop = Aimbot.CrossyDrop;
                                Aimbot.Horizontal_Factor = 8f;
                            }
                            else if (LastHeldWeaponName.Contains("smg."))
                            {
                                Aimbot.DickDrop = Aimbot.SMGDrop;
                                Aimbot.Horizontal_Factor = 33f;
                            }
                            else if (LastHeldWeaponName.Contains("lmg."))
                            {
                                Aimbot.DickDrop = Aimbot.LMGDrop;
                                Aimbot.Horizontal_Factor = 33f;
                            }
                            else if (LastHeldWeaponName.Contains("shotgun."))
                            {
                                Aimbot.DickDrop = Aimbot.PumpyDrop;
                                Aimbot.Horizontal_Factor = 15f;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                Thread.Sleep(1000);
            }
        }

        #region Recoil Values
        public partial class rifleak
        {
            public static float recoilPitchMax = -10.14f;
            public static float recoilPitchMin = -1.352f;
            public static float recoilYawMax = 2.704f;
            public static float recoilYawMin = -0.676f;
            public static float movementPenalty = 0.5f;

        }

        public partial class riflebolt
        {
            public static float recoilPitchMax = -3f;
            public static float recoilPitchMin = -2f;
            public static float recoilYawMax = 4f;
            public static float recoilYawMin = -4f;
            public static float movementPenalty = 0.5f;
        }

        public partial class smg2
        {
            public static float recoilPitchMax = -15f;
            public static float recoilPitchMin = -2f;
            public static float recoilYawMax = 10f;
            public static float recoilYawMin = -1.5f;
            public static float movementPenalty = 0f;
        }

        public partial class shotgundouble
        {
            public static float recoilPitchMax = -15f;
            public static float recoilPitchMin = -10f;
            public static float recoilYawMax = 15f;
            public static float recoilYawMin = 8f;
            public static float movementPenalty = 0f;
        }

        public partial class riflel96
        {
            public static float recoilPitchMax = -1.5f;
            public static float recoilPitchMin = -1f;
            public static float recoilYawMax = 2f;
            public static float recoilYawMin = -2f;
            public static float movementPenalty = 0.5f;
        }

        public partial class riflelr300
        {
            public static float recoilPitchMax = -12f;
            public static float recoilPitchMin = -2.5f;
            public static float recoilYawMax = 5f;
            public static float recoilYawMin = -1f;
            public static float movementPenalty = 0.2f;
        }

        public partial class lmgm249
        {
            public static float recoilPitchMax = -6f;
            public static float recoilPitchMin = -5f;
            public static float recoilYawMax = 1f;
            public static float recoilYawMin = -1f;
            public static float movementPenalty = 1.25f;
        }

        public partial class riflem39
        {
            public static float recoilPitchMax = -7f;
            public static float recoilPitchMin = -5f;
            public static float recoilYawMax = 1.5f;
            public static float recoilYawMin = -1.5f;
            public static float movementPenalty = 0.5f;
        }

        public partial class pistolm92
        {
            public static float recoilPitchMax = -8f;
            public static float recoilPitchMin = -7f;
            public static float recoilYawMax = 1f;
            public static float recoilYawMin = -1f;
            public static float movementPenalty = 0f;
        }

        public partial class smgmp5
        {
            public static float recoilPitchMax = -10f;
            public static float recoilPitchMin = -2f;
            public static float recoilYawMax = 6f;
            public static float recoilYawMin = -1.25f;
            public static float movementPenalty = 0.2f;
        }

        public partial class pistolnailgun
        {
            public static float recoilPitchMax = -6f;
            public static float recoilPitchMin = -3f;
            public static float recoilYawMax = 1f;
            public static float recoilYawMin = -1f;
            public static float movementPenalty = 0f;
        }

        public partial class shotgunpump
        {
            public static float recoilPitchMax = -14f;
            public static float recoilPitchMin = -10f;
            public static float recoilYawMax = 8f;
            public static float recoilYawMin = 4f;
            public static float movementPenalty = 0f;
        }

        public partial class pistolpython
        {
            public static float recoilPitchMax = -16f;
            public static float recoilPitchMin = -15f;
            public static float recoilYawMax = 2f;
            public static float recoilYawMin = -2f;
            public static float movementPenalty = 0f;
        }

        public partial class pistolrevolver
        {
            public static float recoilPitchMax = -6f;
            public static float recoilPitchMin = -3f;
            public static float recoilYawMax = 1f;
            public static float recoilYawMin = -1f;
            public static float movementPenalty = 0f;
        }

        public partial class pistolsemiauto
        {
            public static float recoilPitchMax = -8f;
            public static float recoilPitchMin = -6f;
            public static float recoilYawMax = 2f;
            public static float recoilYawMin = -2f;
            public static float movementPenalty = 0.5f;
        }

        public partial class riflesemiauto
        {
            public static float recoilPitchMax = -6f;
            public static float recoilPitchMin = -5f;
            public static float recoilYawMax = 1f;
            public static float recoilYawMin = -1f;
            public static float movementPenalty = 0.5f;
        }

        public partial class shotgunspas12
        {
            public static float recoilPitchMax = -14f;
            public static float recoilPitchMin = -10f;
            public static float recoilYawMax = 8f;
            public static float recoilYawMin = 4f;
            public static float movementPenalty = 0f;
        }

        public partial class smgthompson
        {
            public static float recoilPitchMax = -15f;
            public static float recoilPitchMin = -2f;
            public static float recoilYawMax = 10f;
            public static float recoilYawMin = -1.5f;
            public static float movementPenalty = 0f;
        }

        public partial class shotgunwaterpipe
        {
            public static float recoilPitchMax = -14f;
            public static float recoilPitchMin = -10f;
            public static float recoilYawMax = 8f;
            public static float recoilYawMin = 4f;
            public static float movementPenalty = 0f;
        }
        #endregion
    }
}
