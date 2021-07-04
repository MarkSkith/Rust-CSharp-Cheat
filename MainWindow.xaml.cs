using Impure.Object_Classes;
using Impure.Overlay;
using MDriver.MEME;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Impure.Object_Classes.RustStructs;

namespace Impure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Window self;
        public MainWindow()
        {
            InitializeComponent();
            self = this;
        }

        public static UpdateObjects ObjectUpdater;
        public static Drawing1 Overlay;
        public static Local LocalClass;
        public static Render Renderer;
        public static Aimbot Gamer;
        public static HotKeys hotkey;

        private void MiniButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = Helpers.RandomString(Helpers.Rnd.Next(8, 32));

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

            Overlay = new Drawing1();
            Overlay.Initialize();

            Renderer = new Render();
            ObjectUpdater = new UpdateObjects();
            Gamer = new Aimbot();
            LocalClass = new Local();
            hotkey = new HotKeys();

            Thread th1 = new Thread(ObjectUpdater.UpdateEntityList);
            th1.Start();
            th1.IsBackground = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void aimbotCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_Aimbot = true;
        }

        private void nodeAimCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_Aimbot_Nodes = true;
        }

        private void randomBoneCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_Randomaim = true;
        }

        private void fatBulletCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_FATBULLET2 = true;

        }

        private void runShootCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.sprintaim = true;

        }

        private void jumpAimCB_Checked(object sender, RoutedEventArgs e)
        {
            Memory.MEMAPI.WriteByte(Memory.MEMAPI.GetModuleBase(Requests.ModuleName.GameAssembly) + CanAttack, 0xc3, true);
        }

        private void recoilSliderCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_RecoilScale = true;

        }

        private void playersCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Players = true;
        }

        private void npcCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_NPC = true;
        }

        private void sleepersCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Sleepers = true;
        }

        private void radarCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_RADAR = true;
        }

        private void sulfurOreCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Sulfur = true;
        }

        private void stoneOreCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Stone = true;
        }

        private void metalOreCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Metal = true;
        }

        private void droppedGunsCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_DroppedGuns = true;
        }

        private void droppedItemsCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_DroppedItems = true;
        }

        private void clothCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Hemp = true;
        }

        private void stashTrapsCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Stashes = true;
        }

        private void helicopterCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Heli = true;
        }

        private void foodCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Food = true;
        }

        private void supplyDropCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Supply = true;
        }

        private void bestCratesCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_HighLoot = true;
        }

        private void lowCratesCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_LowLoot = true;
        }

        private void animalsCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Animals = true;
        }

        private void barrelsCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Barrels = true;
        }

        private void deadPlayersCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Bags = true;
        }

        private void vehiclesCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Vehicles = true;
        }

        private void toolCupboardCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.tc = true;
        }

        private void raidsCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.EXPLOSIVE = true;
        }

        private void debugCamCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_Debug = true;
        }

        private void climbHackCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_Spooder = true;
        }

        private void instaEokaCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_Eoka = true;
        }

        private void superJumpCB_Checked(object sender, RoutedEventArgs e)
        {
            ulong PWM_Addr = Memory.MEMAPI.GetPointer(Local.LocalPlayer._ComponentAddress + BasePlayer.movement, 0);
            Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.gravityMultiplier, 1.5f);
        }

        private void walkOnWaterCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_Spooder = true;
        }

        private void farMeleeCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.CB_ExtendedMelee = true;
        }

        private void noSpreadCB_Checked(object sender, RoutedEventArgs e)
        {
            Options.NoSpread = true;
        }

        private void aimbotCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_Aimbot = false;
        }

        private void nodeAimCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_Aimbot_Nodes = false;
        }

        private void randomBoneCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_Randomaim = false;
        }

        private void fatBulletCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_FATBULLET2 = false;

        }

        private void runShootCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.sprintaim = false;

        }

        private void jumpAimCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Memory.MEMAPI.WriteByte(Memory.MEMAPI.GetModuleBase(Requests.ModuleName.GameAssembly) + CanAttack, 0xc3, false);
        }

        private void recoilSliderCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_RecoilScale = false;

        }

        private void playersCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Players = false;
        }

        private void npcCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_NPC = false;
        }

        private void sleepersCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Sleepers = false;
        }

        private void radarCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_RADAR = false;
        }

        private void sulfurOreCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Sulfur = false;
        }

        private void stoneOreCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Stone = false;
        }

        private void metalOreCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Metal = false;
        }

        private void droppedGunsCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_DroppedGuns = false;
        }

        private void droppedItemsCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_DroppedItems = false;
        }

        private void clothCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Hemp = false;
        }

        private void stashTrapsCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Stashes = false;
        }

        private void helicopterCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Heli = false;
        }

        private void foodCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Food = false;
        }

        private void supplyDropCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Supply = false;
        }

        private void bestCratesCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_HighLoot = false;
        }

        private void lowCratesCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_LowLoot = false;
        }

        private void animalsCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Animals = false;
        }

        private void barrelsCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Barrels = false;
        }

        private void deadPlayersCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Bags = false;
        }

        private void vehiclesCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ESP_Vehicles = false;
        }

        private void toolCupboardCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.tc = false;
        }

        private void raidsCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.EXPLOSIVE = false;
        }

        private void debugCamCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_Debug = false;
        }

        private void climbHackCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_Spooder = false;
        }

        private void instaEokaCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_Eoka = false;
        }

        private void superJumpCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ulong PWM_Addr = Memory.MEMAPI.GetPointer(Local.LocalPlayer._ComponentAddress + BasePlayer.movement, 0);
            Memory.MEMAPI.WriteFloat(PWM_Addr + PlayerWalkMovement.gravityMultiplier, 2.5f);
        }

        private void walkOnWaterCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_Spooder = false;
        }

        private void farMeleeCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.CB_ExtendedMelee = false;
        }

        private void noSpreadCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.NoSpread = false;
        }

        private void recoilSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Options.RecoilScale = (float)e.NewValue;
        }
    }
}
