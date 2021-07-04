using System.Collections.Generic;

namespace Impure.Object_Classes
{
    class CachedList
    {
        //Players
        public static Dictionary<ulong, PlayerClass> Players = new Dictionary<ulong, PlayerClass>();

        public static Dictionary<ulong, PlayerClass> GetSafePlayers(bool _GC = false)
        {
            Dictionary<ulong, PlayerClass> SafeList = new Dictionary<ulong, PlayerClass>();

            foreach (KeyValuePair<ulong, PlayerClass> x in Players)
            {
                if (_GC && x.Value.GC == true)
                {
                    continue;
                }
                SafeList.Add(x.Key, x.Value);
            }
            return SafeList;
        }

        public static PlayerClass SetupPlayer(Dictionary<ulong, PlayerClass> Player_List, ulong key)
        {
            PlayerClass plr;
            if (Player_List.ContainsKey(key))
            {
                plr = Player_List[key];
                plr.GC = false;
            }
            else
            {
                plr = new PlayerClass(key);
                plr.GC = false;
                Player_List[key] = plr;
            }
            return plr;
        }

        //Loot
        public static Dictionary<ulong, LootItemClass> LootItems = new Dictionary<ulong, LootItemClass>();

        public static Dictionary<ulong, LootItemClass> GetSafeLoot(bool _GC = false)
        {
            Dictionary<ulong, LootItemClass> SafeList = new Dictionary<ulong, LootItemClass>();

            foreach (KeyValuePair<ulong, LootItemClass> x in LootItems)
            {
                if (_GC && x.Value.GC == true)
                {
                    continue;
                }
                SafeList.Add(x.Key, x.Value);
            }
            return SafeList;
        }

        public static LootItemClass SetupLoot(Dictionary<ulong, LootItemClass> Loot_List, ulong key)
        {
            LootItemClass LootItem;
            if (Loot_List.ContainsKey(key))
            {
                LootItem = Loot_List[key];
                LootItem.GC = false;
            }
            else
            {
                LootItem = new LootItemClass(key);
                LootItem.GC = false;
                Loot_List[key] = LootItem;
            }
            return LootItem;
        }
    }
}
