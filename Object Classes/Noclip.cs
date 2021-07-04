using MDriver.MEME;
using System;

namespace Impure.Object_Classes
{
    public class Noclip
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetKeyState(int vKey);

        public static void MoveToAngle(ulong Address, Requests.Vector2.Vector2f BodyAngles, float Distance)
        {
            var Myvec3 = Memory.MEMAPI.ReadVector3f(Address);

            var Radians = (Math.PI / 180) * (BodyAngles.Y - 90);

            float MX = Distance * (float)Math.Cos(Radians);
            float MZ = Distance * (float)Math.Sin(Radians);

            Myvec3.X += MX;

            Myvec3.Z -= MZ;

            Memory.MEMAPI.WriteVector3f(Address, Myvec3);
        }

        public void Fly()
        {
            int res = GetKeyState(87);
            if ((res & 0x8000) != 0)
            {

                var MyInput = Memory.MEMAPI.ReadVector2f(Aimbot.Input);
                MoveToAngle(Local.LocalPlayer.TransformAddress, MyInput, 10f);
            }

        }

    }
}
