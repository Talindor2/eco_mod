namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Gameplay.Pipes;

    [RequiresSkill(typeof(MortarProductionSkill), 0)]   
    public partial class MortaredStoneRecipe : Recipe
    {
        public MortaredStoneRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<MortaredStoneItem>(),          
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<StoneItem>(typeof(MortarProductionEfficiencySkill), 8, MortarProductionEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PitchItem>(typeof(MortarProductionEfficiencySkill), 3, MortarProductionEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(MortaredStoneRecipe), Item.Get<MortaredStoneItem>().UILink(), 0.3f, typeof(MortarProductionSpeedSkill));    
            this.Initialize("Mortared Stone", typeof(MortaredStoneRecipe));

            CraftingComponent.AddRecipe(typeof(MasonryTableObject), this);
        }
    }

    [Serialized]
    [Solid, Wall, Constructed,BuildRoomMaterialOption]
    [Tier(1)]                                          
    [RequiresSkill(typeof(MortarProductionEfficiencySkill), 0)]   
    public partial class MortaredStoneBlock :
        Block           
    { }

    [Serialized]
    [MaxStackSize(15)]                           
    [Weight(10000)]      
    [Currency]              
    public partial class MortaredStoneItem :
    BlockItem<MortaredStoneBlock>
    {
        public override string FriendlyName { get { return "Mortared Stone"; } }
        public override string Description { get { return "Used to create tough but rudimentary buildings."; } }


        private static Type[] blockTypes = new Type[] {
            typeof(MortaredStoneStacked1Block),
            typeof(MortaredStoneStacked2Block),
            typeof(MortaredStoneStacked3Block)
        };
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Serialized, Solid] public class MortaredStoneStacked1Block : PickupableBlock { }
    [Serialized, Solid] public class MortaredStoneStacked2Block : PickupableBlock { }
    [Serialized, Solid,Wall] public class MortaredStoneStacked3Block : PickupableBlock { } //Only a wall if it's all 4 MortaredStone
}