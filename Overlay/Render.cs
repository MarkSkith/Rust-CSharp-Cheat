using GameOverlay.Drawing;
using Impure.Object_Classes;
using MDriver.MEME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static Impure.Object_Classes.RustStructs;
using static MDriver.MEME.Requests.Vector2;
using static MDriver.MEME.Requests.Vector3;

namespace Impure.Overlay
{
    public class Render
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetKeyState(int vKey);

        public static int screen_Width = Screen.PrimaryScreen.Bounds.Width;
        public static int screen_Height = Screen.PrimaryScreen.Bounds.Height;
        public Requests.Vector2.Vector2f ScreenCenter = new Requests.Vector2.Vector2f(screen_Width / 2, screen_Height / 2);

        public static ulong Main_Camera = 0;

        public static float aimfov = 100;
        public static float fovchange = 100f;
        public static float FOV_Max = 75f;
        public static float FOV = 75f;
        public static ulong FOV_Address = 0;
        float xb4;
        float yb4;
        float xft;
        float yft;

        float cav;
        float cav2;
        float oldX;
        float oldZ;
        int ctr = 0;
        bool lastn0 = false;

        public static float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static float Scale(int base_size, float Distance)
        {
            float POOP = 2.5f * (float)Math.Pow(0.95, (1 * Distance + -100));
            return Clamp((POOP / 8) + base_size, base_size, base_size * 3f);
        }

        public static float Scale2(int base_size, float Distance)
        {
            float POOP = 2.5f * (float)Math.Pow(0.95, (1 * Distance + -100));
            return (POOP / 8) + base_size;
        }

        public static float Scale3(int base_size, float Distance, float Scale)
        {
            return (base_size / (Distance / (FOV_Max / FOV)) * Scale);
        }

        public Rectangle MakeRect(Vector2f Base_Pos, float Distance, int left, int top, int right, int bottom)
        {
            return new Rectangle(Base_Pos.X - Scale3(left, Distance, 12), Base_Pos.Y - Scale3(top, Distance, 12), Base_Pos.X + Scale3(right, Distance, 12), Base_Pos.Y + Scale3(bottom, Distance, 12));
        }

        public static float GetDistance(Vector3f value1, Vector3f value2)
        {
            float x = value1.X - value2.X;
            float y = value1.X - value2.Y;
            float z = value1.Z - value2.Z;

            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        public static Requests.Vector3.Vector3f[] BULLSHIT = new Vector3f[999999];
        public void Loop()
        {


            System.Threading.Thread.Sleep(2000);
            while (true)
            {
                var gfx = MainWindow.Overlay._graphics; // little shortcut
                gfx.BeginScene(); // call before you start any drawing
                gfx.ClearScene(); // set the background of the scene (can be transparent)

                //try
                //{
                if (Local.LocalPlayer == null)
                {
                    gfx.EndScene();
                    continue;
                }

                if (Requests.IsValidPtr(Main_Camera))
                {
                    FOV = 75;//Memory.MEMAPI.ReadFloat(FOV_Address);
                             //Debug.WriteLine(LocalPlayer._ComponentAddress.ToString("X"));
                             //Draw FPS
                    gfx.DrawText(Drawing1._font, 14, Drawing1.Cerulean, 0, screen_Height - 15, "ESP-FPS:" + gfx.FPS.ToString());

                    //Draw Croshair
                    Point Center;
                    if (Options.CB_X_CROSS) //main crosshair
                    {
                        Center.X = ScreenCenter.X;
                        Center.Y = ScreenCenter.Y;
                        gfx.DrawCrosshair(Drawing1.Yellow_Orange, Center, 5, 2, CrosshairStyle.Plus);
                    }

                    //if (!Options.CB_Y_CROSS)
                    // {
                    //    Center.X = ScreenCenter.X;
                    //    Center.Y = ScreenCenter.Y;
                    //   gfx.DrawCrosshair(Drawing1.Violet, Center, 10, 2, CrosshairStyle.Diagonal);
                    //}

                    if (!Options.CB_C_CROSS)
                    {
                        Center.X = ScreenCenter.X;
                        Center.Y = ScreenCenter.Y;
                        gfx.DrawCrosshair(Drawing1.Yellow_Orange, Center, 5, 2, CrosshairStyle.Gap);
                    }







                    //draw fov
                    gfx.OutlineCircle(Drawing1.Yellow, Drawing1.Yellow, ScreenCenter.X, ScreenCenter.Y, aimfov, 1);



                    //Get View Matrix
                    Requests.ViewMatrix MyMatrix = Requests.ViewMatrix.ReadViewMatrix(Main_Camera);

                    //setup some local player info so we dont read our position a million times.
                    Requests.Vector3.Vector3f LocalPlayerPosition = Memory.MEMAPI.ReadVector3f(Main_Camera + 0x148);

                    //if(BULLSHIT.Length > 0)
                    //{
                    //    for (int Bull = 0; Bull <= BULLSHIT.Length - 1; Bull++)
                    //    {
                    //        if (BULLSHIT[Bull] != Requests.Vector3.Vector3f.Zero)

                    //        {
                    //            var POSSS = UnityFunctions.WorldToScreen(BULLSHIT[Bull], MyMatrix);
                    //            gfx.OutlineCircle(Drawing1.Blue_Green, Drawing1.Blue_Green, POSSS.X, POSSS.Y, 3, 1);
                    //        }
                    //    }
                    //}

                    SolidBrush Color2 = Drawing1.Blue_Violet;
                    SolidBrush Color3 = Drawing1.Black;
                    float miniplyer = 1;
                    float disttot = miniplyer * 75;

                    if (Options.CB_RADAR)
                    {

                        gfx.DrawBox2D(Color3, Color3, 5, 5, 250, 250, 0.1f);
                    }



                    //gfx.DrawLine(Color2, 125, 125, -cav + 125, -cav2 + 125, 5f);


                    gfx.DrawCircle(Color2, 125, 125, 2, 1);




                    xft = LocalPlayerPosition.X;
                    yft = LocalPlayerPosition.Z;
                    //Draw Players
                    if (CachedList.Players != null)
                    {

                        PlayerClass Aimbot_Player = null;
                        float Closest_Player_Distance = 99999f;
                        foreach (KeyValuePair<ulong, PlayerClass> pair in CachedList.GetSafePlayers())
                        {



                            PlayerClass Player = pair.Value;
                            Requests.Vector3.Vector3f PlayerPosition = Player.Position;

                            if (Player.isLocalPlayer)
                            {
                                continue;
                            }

                            if (Player.IsDead)
                            {
                                continue;
                            }

                            if (Player.flag == 16 && !Options.CB_ESP_Sleepers)
                            {
                                continue;
                            }

                            //Debug.WriteLine(Player.flag + " : " + Player.PlayerName);

                            SolidBrush Color = Drawing1.White;
                            if (Player.IsNPC)
                            {
                                Color = Drawing1.Gray;
                            }
                            else if (Player.IsFriend)
                            {
                                Color = Drawing1.Hot_Magenta;
                            }
                            else if (Player.IsVisable)
                            {
                                Color = Drawing1.Bright_Red;
                            }


                            //dotrender


                            if (LocalPlayerPosition.X * miniplyer - PlayerPosition.X * miniplyer + disttot <= 250 && LocalPlayerPosition.X * miniplyer - PlayerPosition.X * miniplyer + disttot >= 0)
                            {
                                gfx.DrawCircle(Color, LocalPlayerPosition.X * miniplyer - PlayerPosition.X * miniplyer + disttot, LocalPlayerPosition.Z * miniplyer - PlayerPosition.Z * miniplyer + disttot, 1.0f, 1);
                            }







                            float Distance = Vector3f.Distance(LocalPlayerPosition, PlayerPosition);

                            if (Distance < Player.Render_Distance)
                            {
                                //Requests.Vector2.Vector2f PlayerScreenPOS = UnityFunctions.WorldToScreen(PlayerPosition, MyMatrix);
                                Vector2f PlayerScreenPOS = (UnityFunctions.getBoneScreen(Player.tryGetBone(4), MyMatrix) + UnityFunctions.getBoneScreen(Player.tryGetBone(15), MyMatrix)) / new Vector2f(2, 2);

                                if (PlayerScreenPOS != Vector2f.Zero)
                                {
                                    if (PlayerScreenPOS.X < 1 || PlayerScreenPOS.Y < 1 || PlayerScreenPOS.X > screen_Width || PlayerScreenPOS.Y > screen_Height)
                                    {
                                        continue;
                                    }

                                    gfx.DrawText(Drawing1._font, 14, Color, PlayerScreenPOS.X, PlayerScreenPOS.Y, Player.PlayerName + " - " + Math.Round(Distance).ToString() + "M" + "\n" + Player.HeldItem);


                                    Requests.Vector2.Vector2f HeadPOS = UnityFunctions.getBoneScreen(Player.HeadBone, MyMatrix);

                                    Rectangle Rect = MakeRect(UnityFunctions.getBoneScreen(Player.tryGetBone(1), MyMatrix), Distance, 50, 50, 50, 50);

                                    gfx.OutlineRectangle(Drawing1.Black, Color, Rect, 1);


                                    float Center_Distance = Vector2f.Distance(ScreenCenter, HeadPOS);

                                    //aimbot stuff
                                    if (Center_Distance < Closest_Player_Distance && !Player.IsFriend && Distance < 250 && Center_Distance < aimfov && Player.IsVisable)
                                    {
                                        Closest_Player_Distance = Center_Distance;
                                        Aimbot_Player = Player;
                                    }


                                    if (Distance < Player.Bone_Render_Distance)
                                    {
                                        Requests.Vector2.Vector2f spineBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(46), MyMatrix);
                                        Requests.Vector2.Vector2f spine2BonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(0), MyMatrix);
                                        Requests.Vector2.Vector2f rShoulderBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(55), MyMatrix);
                                        Requests.Vector2.Vector2f rElbowBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(56), MyMatrix);
                                        Requests.Vector2.Vector2f rWristBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(57), MyMatrix);
                                        Requests.Vector2.Vector2f lShoulderBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(24), MyMatrix);
                                        Requests.Vector2.Vector2f lElbowBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(25), MyMatrix);
                                        Requests.Vector2.Vector2f lWristBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(26), MyMatrix);
                                        Requests.Vector2.Vector2f rThighBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(13), MyMatrix);
                                        Requests.Vector2.Vector2f rKneeBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(14), MyMatrix);
                                        Requests.Vector2.Vector2f rAnkleBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(15), MyMatrix);
                                        Requests.Vector2.Vector2f lThighBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(1), MyMatrix);
                                        Requests.Vector2.Vector2f lKneeBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(2), MyMatrix);
                                        Requests.Vector2.Vector2f lAnkleBonePos = UnityFunctions.getBoneScreen(Player.tryGetBone(3), MyMatrix);

                                        //neck
                                        gfx.DrawLine(Color, spineBonePos.X, spineBonePos.Y, HeadPOS.X, HeadPOS.Y, 2);

                                        // Left Arm
                                        gfx.DrawLine(Color, spineBonePos.X, spineBonePos.Y, lShoulderBonePos.X, lShoulderBonePos.Y, 2);
                                        gfx.DrawLine(Color, lShoulderBonePos.X, lShoulderBonePos.Y, lElbowBonePos.X, lElbowBonePos.Y, 2);
                                        gfx.DrawLine(Color, lElbowBonePos.X, lElbowBonePos.Y, lWristBonePos.X, lWristBonePos.Y, 2);

                                        // Right Arm
                                        gfx.DrawLine(Color, spineBonePos.X, spineBonePos.Y, rShoulderBonePos.X, rShoulderBonePos.Y, 2);
                                        gfx.DrawLine(Color, rShoulderBonePos.X, rShoulderBonePos.Y, rElbowBonePos.X, rElbowBonePos.Y, 2);
                                        gfx.DrawLine(Color, rElbowBonePos.X, rElbowBonePos.Y, rWristBonePos.X, rWristBonePos.Y, 2);

                                        //go down
                                        gfx.DrawLine(Color, spineBonePos.X, spineBonePos.Y, spine2BonePos.X, spine2BonePos.Y, 2);

                                        // Right Leg
                                        gfx.DrawLine(Color, spine2BonePos.X, spine2BonePos.Y, rThighBonePos.X, rThighBonePos.Y, 2);
                                        gfx.DrawLine(Color, rThighBonePos.X, rThighBonePos.Y, rKneeBonePos.X, rKneeBonePos.Y, 2);
                                        gfx.DrawLine(Color, rKneeBonePos.X, rKneeBonePos.Y, rAnkleBonePos.X, rAnkleBonePos.Y, 2);

                                        // Left Leg
                                        gfx.DrawLine(Color, spine2BonePos.X, spine2BonePos.Y, lThighBonePos.X, lThighBonePos.Y, 2);
                                        gfx.DrawLine(Color, lThighBonePos.X, lThighBonePos.Y, lKneeBonePos.X, lKneeBonePos.Y, 2);
                                        gfx.DrawLine(Color, lKneeBonePos.X, lKneeBonePos.Y, lAnkleBonePos.X, lAnkleBonePos.Y, 2);
                                    }
                                }
                            }
                        }
                        if (Aimbot_Player != null)
                        {
                            if (!Aimbot.HasTarget)
                            {
                                Aimbot.TargetPlayer = Aimbot_Player;
                            }
                            Requests.Vector3.Vector3f HEADPOS3D = UnityFunctions.GetBonePosition(Aimbot_Player.HeadBone);
                            Requests.Vector2.Vector2f HEADLINE = UnityFunctions.WorldToScreen(HEADPOS3D, MyMatrix);
                            if (HEADPOS3D != Vector3f.Zero)
                            {
                                gfx.DrawLine(Drawing1.White, ScreenCenter.X, ScreenCenter.Y, HEADLINE.X, HEADLINE.Y, 2);
                                int res = GetKeyState(113);
                                if ((res & 0x8000) != 0)
                                {
                                    if (!PlayerClass.Friends.Contains(Aimbot_Player.PlayerName))
                                    {
                                        PlayerClass.Friends.Add(Aimbot_Player.PlayerName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!Aimbot.HasTarget)
                            {
                                Aimbot.TargetPlayer = null;
                            }
                        }

                    }


                    //Draw Loot
                    if (CachedList.LootItems != null)
                    {
                        Requests.Vector2.Vector2f LootScreenPOS = new Requests.Vector2.Vector2f();
                        foreach (KeyValuePair<ulong, LootItemClass> pair in CachedList.GetSafeLoot())
                        {
                            LootItemClass LootItem = pair.Value;
                            Requests.Vector3.Vector3f LootItemPosition = LootItem.Position;
                            float Distance = Vector3f.Distance(LocalPlayerPosition, LootItemPosition);
                            if (Distance < LootItem.Render_Distance)
                            {
                                LootScreenPOS = UnityFunctions.WorldToScreen(LootItemPosition, MyMatrix);
                                if (LootScreenPOS != Vector2f.Zero)
                                {
                                    if (LootScreenPOS.X < 1 || LootScreenPOS.Y < 1 || LootScreenPOS.X > screen_Width || LootScreenPOS.Y > screen_Height)
                                    {
                                        continue;
                                    }

                                    bool UseBox = false;
                                    Rectangle Rect = new Rectangle(0, 0, 0, 0);

                                    if (LootItem.Type == LootItemClass.LootType.Ores)
                                    {
                                        UseBox = true;
                                        Rect = MakeRect(LootScreenPOS, Distance, 50, 50, 50, 50);
                                    }
                                    else if (LootItem.Type == LootItemClass.LootType.Barrel)
                                    {
                                        UseBox = true;
                                        Rect = MakeRect(LootScreenPOS, Distance, 25, 40, 25, 40);
                                    }
                                    else if (LootItem.Type == LootItemClass.LootType.Hemp)
                                    {
                                        UseBox = true;
                                        Rect = MakeRect(LootScreenPOS, Distance, 15, 30, 15, 30);
                                    }
                                    else if (LootItem.Type == LootItemClass.LootType.Food)
                                    {
                                        UseBox = true;
                                        Rect = MakeRect(LootScreenPOS, Distance, 15, 15, 15, 15);
                                    }
                                    else if (LootItem.Type == LootItemClass.LootType.Loot_Elite)
                                    {
                                        UseBox = true;
                                        Rect = MakeRect(LootScreenPOS, Distance, 35, 20, 35, 20);
                                    }
                                    else if (LootItem.Type == LootItemClass.LootType.Loot_Basic)
                                    {
                                        UseBox = true;
                                        Rect = MakeRect(LootScreenPOS, Distance, 35, 35, 35, 35);
                                    }
                                    //aimbot ore bonus
                                    else if (LootItem.Type == LootItemClass.LootType.Ore_Bonus)
                                    {
                                        if (Distance < 5f)
                                        {
                                            var Init = Local.LocalPlayer.HeldItem;
                                            if (Memory.MEMAPI.Readbyte(LootItem._ComponentAddress + BaseNetworkable.IsDestroyed) == 0 && Local.LocalPlayer.HeldWeaponCategory == 5)
                                            {
                                                Requests.Vector3.Vector3f MyPOS = Memory.MEMAPI.ReadVector3f(Aimbot.Camer_Pos);
                                                Requests.Vector3.Vector3f Target_Vec3 = LootItem.Position;
                                                Target_Vec3.Y -= 0.1f;
                                                if (Options.CB_Aimbot_Nodes)
                                                {
                                                    if (Options.CB_ExtendedMelee && Distance < 5f)
                                                    {
                                                        Memory.MEMAPI.WriteVector2f(Aimbot.Input, Aimbot.CalcAngles(MyPOS, Target_Vec3));
                                                    }
                                                    else if (!Options.CB_ExtendedMelee && Distance < 2f)
                                                    {
                                                        Memory.MEMAPI.WriteVector2f(Aimbot.Input, Aimbot.CalcAngles(MyPOS, Target_Vec3));
                                                    }
                                                }
                                                //Memory.MEMAPI.WriteFloat(Memory.MEMAPI.GetPointer(Local.LocalPlayer.HeldWeapon + HeldEntity.BaseProjectile, BaseMelee.maxDistance), 7f);
                                            }
                                        }
                                        continue;
                                    }

                                    if (UseBox)
                                    {
                                        gfx.OutlineRectangle(LootItem.Color, LootItem.Color, Rect, 1);
                                    }
                                    else
                                    {
                                        gfx.DrawText(Drawing1._font, 14, LootItem.Color, LootScreenPOS.X, LootScreenPOS.Y, LootItem.GameObjectNameCleaned + "\n" + Math.Round(Distance).ToString() + "M");
                                    }
                                }
                            }
                        }
                    }
                }
                //}
                //catch
                //{

                //}
                gfx.EndScene();
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
