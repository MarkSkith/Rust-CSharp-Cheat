using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using MDriver.MEME;

namespace Impure.Object_Classes
{
    public static class UnityFunctions
    {
        public static int screen_Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        public static int screen_Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        public static ulong UnityPlayer_Address = 0x0;
        public static ulong BaseNetworkable = 0x0;
        public static ulong GameObjectManager = 0x17a6ad8;
        public static ulong BN_Base = 0x28FFD20; //Updated Devblog - v2245

        public static ulong FindTaggedObject(string name)
        {
            ulong pGameObjectManager = Memory.MEMAPI.ReadInt64(UnityPlayer_Address + GameObjectManager);
            ulong firstObject = Memory.MEMAPI.ReadInt64(pGameObjectManager + 0x8);
            ulong nextObject = firstObject;
            do
            {
                ulong gameObject = Memory.MEMAPI.ReadInt64(nextObject + 0x10);
                string gameobjectName = Memory.MEMAPI.ReadString((Memory.MEMAPI.ReadInt64(gameObject + 0x60)), 32, false);
                if (gameobjectName == name)
                {
                    return gameObject;
                }
                nextObject = Memory.MEMAPI.ReadInt64(nextObject + 0x8);
            }
            while (nextObject != 0 && nextObject != firstObject && firstObject != 0);
            return 0;
        }

        public static ulong FindTaggedObjectTAG(short TAG)
        {
            ulong pGameObjectManager = Memory.MEMAPI.ReadInt64(UnityPlayer_Address + GameObjectManager);
            ulong firstObject = Memory.MEMAPI.ReadInt64(pGameObjectManager + 0x8);

            ulong nextObject = firstObject;
            do
            {
                ulong gameObject = Memory.MEMAPI.ReadInt64(nextObject + 0x10);
                short gameobjectName = Memory.MEMAPI.ReadInt16(gameObject + 0x54);
                if (gameobjectName == TAG)
                {
                    return gameObject;
                }
                nextObject = Memory.MEMAPI.ReadInt64(nextObject + 0x8);
            }
            while (nextObject != 0 && nextObject != firstObject && firstObject != 0);
            return 0;
        }

        public static ulong FindActiveObject(string name)
        {
            ulong pGameObjectManager = Memory.MEMAPI.ReadInt64(UnityPlayer_Address + GameObjectManager);
            ulong firstObject = Memory.MEMAPI.ReadInt64(pGameObjectManager + 0x18);
            ulong nextObject = firstObject;
            do
            {
                ulong gameObject = Memory.MEMAPI.ReadInt64(nextObject + 0x10);
                string gameobjectName = Memory.MEMAPI.ReadString((Memory.MEMAPI.ReadInt64(gameObject + 0x60)), 32, false);
                if (gameobjectName == name)
                {
                    return gameObject;
                }
                nextObject = Memory.MEMAPI.ReadInt64(nextObject + 0x8);
            }
            while (nextObject != 0 && nextObject != firstObject && firstObject != 0);
            return 0;
        }

        public static void ScanComponent(ulong obj)
        {
            ulong comps = Memory.MEMAPI.ReadInt64(obj + 0x30);
            for (ulong i = 0x8; i < 0x200; i = i + 0x8)
            {
                ulong comp = Memory.MEMAPI.ReadInt64(comps + i);

                ulong fields = Memory.MEMAPI.ReadInt64(comp + 0x28);

                ulong staticc = Memory.MEMAPI.ReadInt64(fields + 0x0);

                ulong data = Memory.MEMAPI.ReadInt64(staticc + 0x0);

                string stt = Memory.MEMAPI.ReadString(Memory.MEMAPI.ReadInt64(data + 0x48));

                if (stt.Length > 1)
                {
                    Debug.WriteLine("----" + stt + " : " + fields.ToString("X"));
                }
            }

        }

        public static void ScanActiveObject(string name)
        {
            ulong pGameObjectManager = Memory.MEMAPI.ReadInt64(UnityPlayer_Address + GameObjectManager);
            ulong firstObject = Memory.MEMAPI.ReadInt64(pGameObjectManager + 0x18);
            ulong nextObject = firstObject;
            do
            {
                ulong gameObject = Memory.MEMAPI.ReadInt64(nextObject + 0x10);
                string gameobjectName = Memory.MEMAPI.ReadString((Memory.MEMAPI.ReadInt64(gameObject + 0x60)), 32, false);
                if (gameobjectName.ToLower().Contains(name))
                {
                    Debug.WriteLine(gameobjectName + " : " + gameObject.ToString("X"));
                    ScanComponent(gameObject);
                }
                nextObject = Memory.MEMAPI.ReadInt64(nextObject + 0x8);
            }
            while (nextObject != 0 && nextObject != firstObject && firstObject != 0);
        }

        public static ulong DumpObjects(string logif)
        {
            string objtags = "";
            ulong pGameObjectManager = Memory.MEMAPI.ReadInt64(UnityPlayer_Address + GameObjectManager);
            ulong firstObject = Memory.MEMAPI.ReadInt64(pGameObjectManager + 0x8);

            ulong nextObject = firstObject;
            do
            {
                ulong gameObject = Memory.MEMAPI.ReadInt64(nextObject + 0x10);
                string gameobjectName = Memory.MEMAPI.ReadString((Memory.MEMAPI.ReadInt64(gameObject + 0x60)), 32, false);
                short gameobjectTag = Memory.MEMAPI.ReadInt16(gameObject + 0x54);

                objtags += gameobjectName + Environment.NewLine + "    TagID: " + gameobjectTag.ToString() + Environment.NewLine + "    Address: " + gameObject.ToString("X") + Environment.NewLine;
                nextObject = Memory.MEMAPI.ReadInt64(nextObject + 0x8);

                if (gameobjectName.ToLower().Contains(logif))
                {
                    Debug.WriteLine(gameobjectName + " : " + gameObject.ToString("X"));
                }
            }
            while (nextObject != 0 && nextObject != firstObject && firstObject != 0);
            File.WriteAllText("C:/TaggedObjects.txt", objtags);
            Debug.WriteLine("Done Dumping");
            return 0;
        }

        public static ulong DumpActiveObjects(string logif)
        {
            string objtags = "";
            ulong pGameObjectManager = Memory.MEMAPI.ReadInt64(UnityPlayer_Address + GameObjectManager);
            ulong firstObject = Memory.MEMAPI.ReadInt64(pGameObjectManager + 0x18);

            ulong nextObject = firstObject;
            do
            {
                ulong gameObject = Memory.MEMAPI.ReadInt64(nextObject + 0x10);
                string gameobjectName = Memory.MEMAPI.ReadString((Memory.MEMAPI.ReadInt64(gameObject + 0x60)), 32, false);
                objtags += gameobjectName + Environment.NewLine;
                nextObject = Memory.MEMAPI.ReadInt64(nextObject + 0x8);

                if (gameobjectName.ToLower().Contains(logif))
                {
                    Debug.WriteLine(gameobjectName + " : " + gameObject.ToString("X"));
                }
            }
            while (nextObject != 0 && nextObject != firstObject && firstObject != 0);
            File.WriteAllText("C:/ActiveObjects.txt", objtags);
            Debug.WriteLine("Done Dumping");
            return 0;
        }

        public static ulong GetComponent(ulong obj, string classname)
        {
            ulong comps = Memory.MEMAPI.ReadInt64(obj + 0x30);
            for (ulong i = 0x8; i < 0x200; i = i + 0x8)
            {
                ulong comp = Memory.MEMAPI.ReadInt64(comps + i);

                ulong fields = Memory.MEMAPI.ReadInt64(comp + 0x28);

                ulong staticc = Memory.MEMAPI.ReadInt64(fields + 0x0);

                ulong data = Memory.MEMAPI.ReadInt64(staticc + 0x0);

                string stt = Memory.MEMAPI.ReadString(Memory.MEMAPI.ReadInt64(data + 0x48));

                if (stt.Length > 1 && classname.Length == 0)
                {
                    Debug.WriteLine(stt);
                    Debug.WriteLine(fields.ToString("X"));
                }

                if (stt == classname)
                {
                    return fields;
                }
            }

            return 0;
        }

        public static Requests.Vector2.Vector2f WorldToScreen(Requests.Vector3.Vector3f origin, Requests.ViewMatrix viewMatrix)
        {
            Requests.Vector2.Vector2f screenPos = new Requests.Vector2.Vector2f(0);
            Requests.ViewMatrix temp;

            Requests.ViewMatrix.Transpose(ref viewMatrix, out temp);

            Requests.Vector3.Vector3f translationVector = new Requests.Vector3.Vector3f(temp.M41, temp.M42, temp.M43);
            Requests.Vector3.Vector3f up = new Requests.Vector3.Vector3f(temp.M21, temp.M22, temp.M23);
            Requests.Vector3.Vector3f right = new Requests.Vector3.Vector3f(temp.M11, temp.M12, temp.M13);

            float w = Requests.Vector3.Vector3f.Dot(translationVector, origin) + temp.M44;

            if (w < 0.098f)
                return screenPos;

            float y = Requests.Vector3.Vector3f.Dot(up, origin) + temp.M24;
            float x = Requests.Vector3.Vector3f.Dot(right, origin) + temp.M14;


            screenPos.X = (screen_Width / 2) * (1.0f + x / w);
            screenPos.Y = (screen_Height / 2) * (1.0f - y / w);

            return screenPos;
        }

        private struct Matrix34
        {
            public Requests.Vector4.Vector4f vec0;
            public Requests.Vector4.Vector4f vec1;
            public Requests.Vector4.Vector4f vec2;
        };

        public enum ShuffleSel
        {
            XFromX,
            XFromY,
            XFromZ,
            XFromW,

            YFromX = 0x00,
            YFromY = 0x04,
            YFromZ = 0x08,
            YFromW = 0x0C,

            ZFromX = 0x00,
            ZFromY = 0x10,
            ZFromZ = 0x20,
            ZFromW = 0x30,

            WFromX = 0x00,
            WFromY = 0x40,
            WFromZ = 0x80,
            WFromW = 0xC0,

            /*Expand a single element into all elements*/
            ExpandX = XFromX | YFromX | ZFromX | WFromX,
            ExpandY = XFromY | YFromY | ZFromY | WFromY,
            ExpandZ = XFromZ | YFromZ | ZFromZ | WFromZ,
            ExpandW = XFromW | YFromW | ZFromW | WFromW,

            /*Expand a pair of elements (x,y,z,w) -> (x,x,y,y)*/
            ExpandXY = XFromX | YFromX | ZFromY | WFromY,
            ExpandZW = XFromZ | YFromZ | ZFromW | WFromW,

            /*Expand interleaving elements (x,y,z,w) -> (x,y,x,y)*/
            ExpandInterleavedXY = XFromX | YFromY | ZFromX | WFromY,
            ExpandInterleavedZW = XFromZ | YFromW | ZFromZ | WFromW,

            /*Rotate elements*/
            RotateRight = XFromY | YFromZ | ZFromW | WFromX,
            RotateLeft = XFromW | YFromX | ZFromY | WFromZ,

            /*Swap order*/
            Swap = XFromW | YFromZ | ZFromY | WFromX,
        };

        public static unsafe Requests.Vector4.Vector4f Shuffle(this Requests.Vector4.Vector4f v1, ShuffleSel sel)
        {
            float* ptr = (float*)&v1;
            int idx = (int)sel;
            return new Requests.Vector4.Vector4f(*(ptr + ((idx >> 0) & 0x3)), *(ptr + ((idx >> 2) & 0x3)), *(ptr + ((idx >> 4) & 0x3)), *(ptr + ((idx >> 6) & 0x3)));
        }

        public static unsafe Requests.Vector3.Vector3f GetBonePosition(ulong transform)
        {
            GCHandle pIndicesBufff = new GCHandle();
            GCHandle pMatricesBufff = new GCHandle();

            try
            {
                ulong transform_internal = Memory.MEMAPI.ReadInt64(transform + 0x10);
                if (transform_internal == 0x0)
                    return Requests.Vector3.Vector3f.Zero;

                ulong pMatrix = Memory.MEMAPI.ReadInt64(transform_internal + 0x38);

                int index = Memory.MEMAPI.ReadInt32(transform_internal + 0x40);

                if (pMatrix == 0x0)
                    return Requests.Vector3.Vector3f.Zero;

                ulong matrix_list_base = Memory.MEMAPI.ReadInt64(pMatrix + 0x18);
                if (matrix_list_base == 0x0)
                    return Requests.Vector3.Vector3f.Zero;

                ulong dependency_index_table_base = Memory.MEMAPI.ReadInt64(pMatrix + 0x20);
                if (dependency_index_table_base == 0x0)
                    return Requests.Vector3.Vector3f.Zero;


                byte[] pMatricesBuff = Memory.MEMAPI.ReadBytes(matrix_list_base, (sizeof(Matrix34) * index) + sizeof(Matrix34));
                pMatricesBufff = GCHandle.Alloc(pMatricesBuff, GCHandleType.Pinned);
                void* pMatricesBuf = pMatricesBufff.AddrOfPinnedObject().ToPointer();


                byte[] pIndicesBuff = Memory.MEMAPI.ReadBytes(dependency_index_table_base, (sizeof(int) * index + sizeof(int)));
                pIndicesBufff = GCHandle.Alloc(pIndicesBuff, GCHandleType.Pinned);
                void* pIndicesBuf = pIndicesBufff.AddrOfPinnedObject().ToPointer();

                Requests.Vector4.Vector4f result = *(Requests.Vector4.Vector4f*)((UInt64)pMatricesBuf + 0x30 * (UInt64)index);

                int index_relation = *(int*)((UInt64)pIndicesBuf + 0x4 * (UInt64)index);

                Requests.Vector4.Vector4f xmmword_1410D1340 = new Requests.Vector4.Vector4f(-2.0f, 2.0f, -2.0f, 0.0f);
                Requests.Vector4.Vector4f xmmword_1410D1350 = new Requests.Vector4.Vector4f(2.0f, -2.0f, -2.0f, 0.0f);
                Requests.Vector4.Vector4f xmmword_1410D1360 = new Requests.Vector4.Vector4f(-2.0f, -2.0f, 2.0f, 0.0f);

                int tries = 10000;
                int tried = 0;

                try
                {
                    while (index_relation >= 0)
                    {
                        if (tried++ > tries) break;
                        Matrix34 matrix34 = *(Matrix34*)((UInt64)pMatricesBuf + (0x30 * (UInt64)index_relation));

                        Requests.Vector4.Vector4f v10 = matrix34.vec2 * result;
                        Requests.Vector4.Vector4f v11 = (Requests.Vector4.Vector4f)(Shuffle(matrix34.vec1, (ShuffleSel)(0)));
                        Requests.Vector4.Vector4f v12 = (Requests.Vector4.Vector4f)(Shuffle(matrix34.vec1, (ShuffleSel)(0x55)));
                        Requests.Vector4.Vector4f v13 = (Requests.Vector4.Vector4f)(Shuffle(matrix34.vec1, (ShuffleSel)(0x8E)));
                        Requests.Vector4.Vector4f v14 = (Requests.Vector4.Vector4f)(Shuffle(matrix34.vec1, (ShuffleSel)(0xDB)));
                        Requests.Vector4.Vector4f v15 = (Requests.Vector4.Vector4f)(Shuffle(matrix34.vec1, (ShuffleSel)(0xAA)));
                        Requests.Vector4.Vector4f v16 = (Requests.Vector4.Vector4f)(Shuffle(matrix34.vec1, (ShuffleSel)(0x71)));
                        result = (((((((v11 * xmmword_1410D1350) * v13) - ((v12 * xmmword_1410D1360) * v14)) * Shuffle(v10, (ShuffleSel)(-86))) +
                                ((((v15 * xmmword_1410D1360) * v14) - ((v11 * xmmword_1410D1340) * v16)) * Shuffle(v10, (ShuffleSel)(85)))) +
                                (((((v12 * xmmword_1410D1340) * v16) - (v15 * xmmword_1410D1350 * v13)) * Shuffle(v10, (ShuffleSel)(0))) + v10)) + matrix34.vec0);


                        index_relation = *(int*)((UInt64)pIndicesBuf + 0x4 * (UInt64)index_relation);
                    }
                    try { pIndicesBufff.Free(); } catch { }
                    try { pMatricesBufff.Free(); } catch { }
                }
                catch
                {
                    try { pIndicesBufff.Free(); } catch { }
                    try { pMatricesBufff.Free(); } catch { }
                }


                return new Requests.Vector3.Vector3f(result.X, result.Y, result.Z);
            }
            catch
            {
                try { pIndicesBufff.Free(); } catch { }
                try { pMatricesBufff.Free(); } catch { }
                return new Requests.Vector3.Vector3f(0, 0, 0);
            }
        }

        public static Requests.Vector2.Vector2f getBoneScreen(ulong x, Requests.ViewMatrix y)
        {
            Requests.Vector3.Vector3f outputBone = GetBonePosition(x);

            return WorldToScreen(outputBone, y);
        }
    }
}
