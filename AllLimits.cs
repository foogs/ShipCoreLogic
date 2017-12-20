using VRageMath;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.Components;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.Entity;
using Sandbox.ModAPI;
using System.IO;
using Sandbox.Game.Entities;
using Sandbox.Game;
using Sandbox.Game.Definitions;
using Sandbox.Game.EntityComponents;
using VRage.Utils;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library;
using VRage;
using Sandbox.Game.World;
using Sandbox.Definitions;
using SpaceEngineers.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders;
using Sandbox.Common.ObjectBuilders;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.Entities.Character.Components;
using VRage.Game.ModAPI.Interfaces;
using System.Timers;
using Sandbox.ModAPI.Interfaces;
using VRage.Collections;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Interfaces.Terminal;
using ProtoBuf;
using SpaceEngineers.Game.ModAPI;

/*
 ░░░░░░░░░░▄▄▄▄▄▄▄▄▄▄▄░░░░░░░░░░
░░░░░▄▄▀▀▀▀░░░░░░░░░░▀▀▄▄░░░░░░
░░░▄▀░░░░░░░░░░░░░░░░░░░▀▀▄░░░░
░░█░░░░░░░░░░░░░░░░░░░░░░░░█░░░
░█░░░░░░░░░░░░░░░░░░░░░▄▀▀▀▀▀█▄
█▀░░░░░░░░░░░░░░░░░░░░█░░░░▄███
█░░░░░░░░░░░░░░░░░░░░░█░░░░▀███
█░░░░░▄▀▀██▀▄░░░░░░░░░█░░░░░░░█
█░░░░█░░████░█░░░░░░░░░▀▄▄▄▄▄█░
█░░░░█░░▀██▀░█░░░░░░░░░░░░░░░█░
█░░░░█░░░░░░░█░░░░░░░░░░░░░░▄▀░
█░░░░▀▄░░░░░▄▀░░▄▄▄▄▄▄▄▄▄░░░█░░
█░░░░░░▀▀▀▀▀░░░░█░█░█░█░█░░▄▀░░
░█░░░░░░░░░░░░░░▀▄█▄█▄█▀░░▄▀░░░
░░█░░░░░░░░░░░░░░░░░░░░░░▄▀░░░░
░░░▀▀▀▄░░░░░░░░░░▄▄▄▄▄▄▀▀░░░░░░
░░░░▄▀░░░░░░░░░▀▀░░▄▀░░░░░░░░░░
░░▄▀░░░░░░░░░░░░░░░█░░░░░░░░░░░
░▄▀░░░░░░░░░░░░░░░░█░░▄▀▀▀█▀▀▄░
░█░░░░█░░█▀▀▀▄░░░░░█▀▀░░░░█░░█░
▄█░░░░▀▀▀░░░░█░░░░░█░░░░▀▀░░░█░
█▀▄░░░░░░░░░░█░░░░░█▄░░░░░░░░█░
█░░▀▀▀▀▀█▄▄▄▄▀░░░░░▀█▀▀▀▄▄▄▄▀░░
█░░░░░░░░░░░░░░░░░░░▀▄░░░░░░░░░
*/

namespace ShipCoreMainBlock
{

    public class AllLimits
    {
        public static AllLimits hackystatic;
        public static Dictionary<MyStringHash, Addons> namelistaddons = new Dictionary<MyStringHash, Addons>(MyStringHash.Comparer) {
                {MyStringHash.GetOrCompute("ShipCore_Add05"),Addons.friend_or_foe_transponder},
                {MyStringHash.GetOrCompute("ShipCore_Add04"),Addons.Сooler},
                {MyStringHash.GetOrCompute("ShipCore_Add03"),Addons.PowerBuffer},
                {MyStringHash.GetOrCompute("ShipCore_Add02"),Addons.Fuse},
                {MyStringHash.GetOrCompute("ShipCore_Add01"),Addons.Stabilizer}   };

        public static Dictionary<Addons, MyStringHash> namelistaddonsreverse = new Dictionary<Addons, MyStringHash>() {
                {Addons.friend_or_foe_transponder,MyStringHash.GetOrCompute("ShipCore_Add05")},
                {Addons.Сooler,MyStringHash.GetOrCompute("ShipCore_Add04")},
                {Addons.PowerBuffer,MyStringHash.GetOrCompute("ShipCore_Add03")},
                {Addons.Fuse,MyStringHash.GetOrCompute("ShipCore_Add02")},
                {Addons.Stabilizer,MyStringHash.GetOrCompute("ShipCore_Add01")}};

        public float MAX_BLOCKS_IN_STATION;
        public float MAX_BLOCKS_IN_LARGE_SHIP;
        public float MAX_BLOCKS_IN_SMALL_SHIP;
        public List<BlockLimitItem> Big_List_of_Limits;
        public Dictionary<Addons, int> installedAddons;
        
        /// <summary>
        /// Загрузка дефолтных лимитов
        /// </summary>
        public AllLimits()
        {
            hackystatic = this;
               MAX_BLOCKS_IN_STATION = 5000.0f;//
            MAX_BLOCKS_IN_LARGE_SHIP = 2500.0f; //Base Limits
            MAX_BLOCKS_IN_SMALL_SHIP = 1000.0f; //  
            Big_List_of_Limits = new List<AllLimits.BlockLimitItem>(){
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockSubtypeId,"MyObjectBuilder_Projector","SmallProjector",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockSubtypeId,"MyObjectBuilder_Projector","LargeProjector",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"Drill","Any",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockSubtypeId,"Refinery","LargeRefinery",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockSubtypeId,"Refinery","Blast Furnace",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"OxygenTank","Any",4,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"ShipWelder","Any",2,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"ShipGrinder","Any",2,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"OxygenGenerator","Any",2,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"OreDetector","Any",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"PistonBase","Any",10,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"MotorAdvancedStator","Any",10,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"MotorStator","Any",6,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"Wheel","Any",30,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"Thrust","Any",50,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockSubtypeId,"ConveyorConnector","ConveyorTube",50,false), //трубы
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"Conveyor","Any",50,false), //конвеерыы
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"ShipConnector","Any",3,false), // коннекторы
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"GravityGenerator","Any",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"LargeGatlingTurret","Any",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"LargeMissileTurret","Any",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"InteriorTurret","Any",1,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"Reactor","Any",4,false),
         new BlockLimitItem(BlockLimitItem.EnforcementMode.BlockTypeId,"Assembler","Any",4,false),
         
          /*
          Ограничения по блокам на объект базовые:
Любой бур 1
Сварки\Пилы = 2
Ассемблеры = 1
Рефайны = 1
дуговая печь = 1
Водородные баки = 2
Генераторы кислорода = 2
Проэкторы=1
Детектор руды= 1
Пистоны = 10
Роторы = 10
Колеса = 12
Любые движки = 50! (в любом случае!!) vip x2
Квадратные конвееры = 50! (в любом случае!!) vip x2
Конвееры любые = 150 (в любом случае!!) vip x2
Сортировщики 20
Коннекторы = 3 (базы x2) vip x3
Генератор гравитации = 1.
Ограничение на вооружение:
Большие корабли: 8 штук на грид в сумме.
Станции: 12 штук на грид в сумме.
Малые корабли: 4 штук на грид в сумме.*/

        };
            installedAddons = null;
            installedAddons = new Dictionary<Addons, int>();

        

        }

        public static void RecalcLimitsWithAddons()
        {
            bool Stabilizer = false;
            bool Fuse = false;
            bool PowerBuffer = false; 
            bool Сooler = false;
            bool friend_or_foe_transponder = false;


            int Stabilizerc = 0;
            int Fusec = 0;
            int PowerBufferc = 0;
            int Сoolerc = 0;
            int friend_or_foe_transponderc = 0;

            foreach (var Addon in hackystatic.installedAddons)
            {
                if(Addon.Key == Addons.friend_or_foe_transponder)
                {
                    if (friend_or_foe_transponder)                       
                        continue;
                    friend_or_foe_transponder = true;
                    hackystatic.MAX_BLOCKS_IN_LARGE_SHIP += 1000;
                    hackystatic.MAX_BLOCKS_IN_STATION += 1000;
                    hackystatic.MAX_BLOCKS_IN_SMALL_SHIP += 400;
                }
                if (Addon.Key == Addons.Fuse)
                {
                    if (Fuse)
                        continue;
                    Fuse = true;
                    hackystatic.MAX_BLOCKS_IN_LARGE_SHIP += 1000;
                    hackystatic.MAX_BLOCKS_IN_STATION += 1000;
                    hackystatic.MAX_BLOCKS_IN_SMALL_SHIP += 400;
                    // +3 бура любых, +3 рефайна,+1 Ассемблер,+ 3 любых реакторов,+50 к кв. конвеерам и к обычным,
                    hackystatic.Big_List_of_Limits.First(x => x.BlockSubtypeId == "LargeRefinery").MaxPerGrid += 3;
                    hackystatic.Big_List_of_Limits.First(x => x.BlockSubtypeId == "Blast Furnace").MaxPerGrid += 3;
                    hackystatic.Big_List_of_Limits.First(x => x.BlockTypeId == "Reactor").MaxPerGrid += 3;
                    hackystatic.Big_List_of_Limits.First(x => x.BlockTypeId == "Drill").MaxPerGrid += 3;


                    hackystatic.Big_List_of_Limits.First(x => x.BlockTypeId == "conver").MaxPerGrid += 50;
                    hackystatic.Big_List_of_Limits.First(x => x.BlockTypeId == "conveerbasic").MaxPerGrid += 50;
                }


            }

        }
        public class BlockLimitItem
        {
            public BlockLimitItem(EnforcementMode mode, string blocktypeid, string blocksubtypeid, uint maxpergrid, bool adminexempt)
            {
                this._mode = mode;
                this._blockTypeId = blocktypeid;
                this._blockSubtypeId = blocksubtypeid;
                this._maxPerGrid = maxpergrid;
                this._adminExempt = adminexempt;
            }
            public enum EnforcementMode
            {
                Off = 0,
                BlockTypeId = 1,
                BlockSubtypeId = 2
            }

            private string _blockTypeId;
            public string BlockTypeId
            {
                get { return _blockTypeId; }
                set { _blockTypeId = value; }
            }

            private string _blockSubtypeId;

            public string BlockSubtypeId
            {
                get { return _blockSubtypeId; }
                set { _blockSubtypeId = value; }
            }

            private uint _maxPerGrid;
            public uint MaxPerGrid
            {
                get { return _maxPerGrid; }
                set { _maxPerGrid = value; }
            }

            private string _maxReachWarning;
            public string MaxReachWarning
            {
                get { return _maxReachWarning; }
                set { _maxReachWarning = value; }
            }

            private string _maxExceedWarning;
            private EnforcementMode _mode;

            public string MaxExceedWarning
            {
                get { return _maxExceedWarning; }
                set { _maxExceedWarning = value; }
            }

            public EnforcementMode Mode
            {
                get { return _mode; }
                set { _mode = value; }
            }

            private bool _adminExempt;
            public bool AdminExempt
            {
                get
                {
                    return _adminExempt;
                }
                set
                {
                    _adminExempt = value;
                }
            }

            public override int GetHashCode()
            {
                return (string.IsNullOrEmpty(_blockSubtypeId) ? string.Empty : _blockSubtypeId).GetHashCode()
                       + (string.IsNullOrEmpty(_blockTypeId) ? string.Empty : _blockTypeId).GetHashCode();
            }
        }


        public enum Addons
        {
            Stabilizer,
            Fuse,
            PowerBuffer,
            Сooler,
            friend_or_foe_transponder
        }
        class Friend_or_foe_transponder
        {
            public void Burst() { }
            public void NotHave() { }
            public void OverLimit() { }
        }

        class Сooler
        {
            public void Burst() { }
            public void NotHave() { }
            public void OverLimit() { }
        }

        class Stabilizer
        {
            public void Burst() { }
            public void NotHave() { }
            public void OverLimit() { }
        }

        class Fuse
        {
            public void Burst() { }
            public void NotHave() { }
            public void OverLimit() { }
        }
        class PowerBuffer
        {
            public void Burst() { }
            public void NotHave() { }
            public void OverLimit() { }
        }



        [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
        public class WBcore : MySessionComponentBase
        {

            public override void Draw()
            {
                base.Draw();

                if (ShipCoreMainBlock.ShipCore.mycore == null) return;
                MyStringId material = MyStringId.GetOrCompute("SquareIgnoreDepth");
                float thickness = 0.05f;
                Vector4 color1 = new Vector4(255, 161, 14, 100);
                Vector4 color2 = new Vector4(255, 0, 0, 250);
                Vector4 color3 = new Vector4(255, 0,255, 250);
                base.UpdateAfterSimulation();
                foreach (var a in ShipCoreMainBlock.ShipCore.addonsPositions)
                {


                    MySimpleObjectDraw.DrawLine(ShipCoreMainBlock.ShipCore.mycore.CubeGrid.GridIntegerToWorld(a), ShipCoreMainBlock.ShipCore.mycore.GetPosition(), material, ref color1, thickness);

                }

            }

        }
        }
}
