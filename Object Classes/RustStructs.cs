namespace Impure.Object_Classes
{
    public static class RustStructs
    {
        //stuff
        public static ulong Sky_Dome = 0x0;
        //functions
        public static ulong DoMovement = 0x487C20; // thick bullet testing
        public static ulong set_DateTime = 0xCE1540; //public void set_DateTime
        public static ulong BlockSprint = 0x2E6E40; // Run and shoot 
        public static ulong CanAttack = 0x2E7C70; //Jump Aim
        public static ulong CanSprint = 0x46DE10;
        public static ulong GetJumpHeight = 0x2EFED0;
        public static ulong DoRicochet = 0x4D5040;
        public static ulong Reflect = 0x4D7580;
        public static ulong IsSwimming = 0x2F42B0;
        public static ulong isAdmin = 0x303680;
        public static ulong CreateProjectile = 0x48C7F0;
        public static ulong UpdateAmbientLight = 0x31ECB0; //maincamera
        public static ulong CanHoldItems = 0x2DE2C0;
        public static ulong GetIndexedSpreadScalar = 0x67B310;

        public static ulong itemList = 0x38;
        public static ulong list = 0x10;
        public static ulong index = 0x10;

        public partial class PlayerInput
        {
            public static ulong bodyAngles = 0x3C;
        }

        public partial class Planner
        {
            public static ulong rotationOffset = 0x1D8;
        }


        public partial class HitInfo
        {
            public static ulong ProjectileDistance = 0xA0;
        }


        public partial class BasePlayer
        {
            public static ulong inventory = 0x5F8;
            public static ulong clActiveItem = 0x560;
            public static ulong movement = 0x4D0;
            public static ulong playerFlags = 0x5E8;
            public static ulong input = 0x4C8;
            public static ulong displayName = 0x638;
            public static ulong userID = 0x628;
            public static ulong clientTeam = 0x530;
            public static ulong playerModel = 0x4A8; //public PlayerModel playerModel
        }

        public partial class BaseNetworkable
        {
            public static ulong IsDestroyed = 0x58; //private bool <IsDestroyed>k__BackingField;
        }

        public partial class PlayerWalkMovement
        {
            public static ulong gravityMultiplier = 0x74;

            public static ulong gravityMultiplierSwimming = 0x78;
            public static ulong maxAngleWalking = 0x7C;
            public static ulong maxAngleClimbing = 0x80;
            public static ulong maxVelocity = 0xA8;

            public static ulong groundAngle = 0xB4;
            public static ulong groundAngleNew = 0xB8;
            public static ulong grounded = 0x130;

            public static ulong clothingWaterSpeedBonus = 0x644;
            public static ulong groundTime = 0xBC;
            public static ulong FallVelocity = 0x4BBE10;
            public static ulong Flying = 0x130;
            public static ulong HandleRunning = 0x4BE190;
        }



        public partial class PlayerInventory
        {
            public static ulong containerMain = 0x20;
            public static ulong containerBelt = 0x28;
            public static ulong containerWear = 0x30;
        }

        public partial class HeldEntity
        {
            public static ulong BaseProjectile = 0x98; //0x98
            public static ulong ViewModel = 0x180;
            public static ulong ItemID = 0x20; //0x20
        }




        public partial class BaseProjectile
        {
            public static ulong CreatedProjectiles = 0x338;
            public static ulong automatic = 0x270;
            public static ulong hasADS = 0x2EC;
            public static ulong recoil = 0x2C0;
            //public static ulong aimSway = 0x2BC;
            public static ulong HeldEntity = 0x98; //private EntityRef heldEntity
            public static ulong CreatedProjArray = 0x10;
            public static ulong size = 0x18;
            public static ulong currentPosition = 0x124;
            public static ulong isRicochet = 0x164;
            public static ulong nextReloadTime = 0x2FC;
            public static ulong startReloadTime = 0x300;

            public static ulong stancePenalty = 0x304;
            public static ulong aimconePenalty = 0x308;
            public static ulong aimSway = 0x2B8;
            public static ulong aimSwaySpeed = 0x2BC;
            public static ulong aimCone = 0x2D0;
            public static ulong hipAimCone = 0x2D4;
            public static ulong aimconePenaltyPerShot = 0x2D8;
            public static ulong aimConePenaltyMax = 0x2DC;
            public static ulong aimconePenaltyRecoverTime = 0x2E0;
            public static ulong aimconePenaltyRecoverDelay = 0x2E4;
            public static ulong stancePenaltyScale = 0x2E8;




        }


        public partial class AttackEntity
        {
            public static ulong repeatDelay = 0x1DC;
        }



        public partial class RecoilProperties
        {

            public static ulong recoilYawMin = 0x18;
            public static ulong ADSScale = 0x30;
            public static ulong recoilYawMax = 0x1C;
            public static ulong recoilPitchMin = 0x20;
            public static ulong recoilPitchMax = 0x24;
            public static ulong movementPenalty = 0x34;
            public static ulong shotsUntilMax = 0x54;
        }

        public partial class Item
        {
            public static ulong info = 0x20;
            public static ulong uid = 0x28;
            public static ulong amount = 0x30;
            public static ulong ItemContainerparent = 0x78;
        }

        public partial class ItemDefinition
        {
            public static ulong itemid = 0x18;
            public static ulong shortname = 0x20;
            public static ulong displayName = 0x28;
            public static ulong category = 0x40;
            public static ulong itemType = 0x4C;
            public static ulong condition = 0x88;
        }


        public partial class WorldItem
        {
            public static ulong item = 0x150; //public Item item;
        }


        public partial class FlintStrikeWeapon
        {
            public static ulong successFraction = 0x340;
        }

        public partial class PlayerModel
        {
            public static ulong visible = 0x248; //internal bool visible;
            public static ulong velocity = 0x1E4; //internal Vector3 velocity
            public static ulong _multiMesh = 0x280; //private SkinnedMultiMesh _multiMesh
        }


        public partial class Model
        {
            public static ulong headBone = 0x28; //public Transform headBone;
            public static ulong boneTransforms = 0x48;
        }


        public partial class SkinnedMultiMesh
        {
            public static ulong boneDict = 0x28; //public SkeletonSkinLod skeletonSkinLod;
        }

        public partial class BoneDictionary
        {
            public static ulong transforms = 0x10; //public Transform[] transforms
            public static ulong names = 0x18; //public string[] names;
        }


        public partial class ViewModel
        {
            public static ulong viewModelPrefab = 0x18; //public GameObjectRef viewModelPrefab;
            public static ulong targetEntity = 0x20; //public HeldEntity targetEntity;
            public static ulong instance = 0x28; //public BaseViewModel instance;
        }


        public partial class BaseViewModel
        {
            public static ulong ironSights = 0x60; //internal IronSights ironSights;
        }


        public partial class IronSights
        {
            public static ulong zoomFactor = 0x2C; //public float zoomFactor;
        }

        public partial class BaseMelee
        {
            public static ulong maxDistance = 0x278; //public float maxDistance;
            public static ulong attackRadius = 0x27C; //public float attackRadius;
            public static ulong blockSprintOnAttack = 0x281; //public bool blockSprintOnAttack;
        }

        public partial class BaseEntity
        {
            //InspectorFlagsAttribute
            public static ulong flags = 0x120; //public global::BaseEntity.Flags flags;
            public static ulong model = 0x118; //public Model model;
            public static ulong ragdoll = 0x70; //protected Ragdoll ragdoll;
        }

        public partial class BaseCombatEntity
        {
            public static ulong lifestate = 0x204;
            public static ulong health = 0x20C; //
        }


        public partial class PlayerTeam
        {
            public static ulong members_List = 0x30;
        }


        public static ulong m_stringLength = 0x10;
        public static ulong m_firstChar = 0x14;

    }
}