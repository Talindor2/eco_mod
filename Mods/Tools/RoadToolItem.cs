﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
    using System;
    using System.ComponentModel;
    using Eco.Core.Utils;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Plants;
    using Eco.Shared.Math;
    using Eco.Simulation;
    using Eco.World;
    using Eco.World.Blocks;
    using Gameplay.Skills;

    [Category("Tools")]
    [RepairRequiresSkill(typeof(RoadConstructionSkill), 1)]
    public partial class RoadToolItem : ToolItem
    {
        static IDynamicValue caloriesBurn = new ConstantValue(15);
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        public override Item RepairItem { get { return Item.Get<StoneItem>(); } }
        public override int FullRepairAmount { get { return 10; } }

        private static IDynamicValue skilledRepairCost = new ConstantValue(1);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        public override InteractResult OnActLeft(InteractionContext context)
        {
            if (!context.HasBlock)
                return InteractResult.NoOp;

            Type blockType = this.GetRoadBlock(context.Block);

            if (blockType != null)
            {
                Result result = this.PlayerPlaceBlock(blockType, context.BlockPosition.Value, context.Player, true);
                if (result.Success)
                {
                    Vector3i above = context.BlockPosition.Value + Direction.Up.ToVec();
                    Block aboveBlock = World.GetBlock(above);

                    if (aboveBlock is PlantBlock)
                        EcoSim.PlantSim.DestroyPlant(EcoSim.PlantSim.GetPlant(above), DeathType.Construction);
                }

                return (InteractResult)result;
            }
            else
                return InteractResult.NoOp;
        }

        private Type GetRoadBlock(Block currentBlock)
        {
            if (currentBlock is DirtBlock || currentBlock is MudBlock || currentBlock is GrassBlock || currentBlock is TilledDirtBlock)
                return typeof(DirtRoadBlock);
            else
                return null;
        }
    }
}