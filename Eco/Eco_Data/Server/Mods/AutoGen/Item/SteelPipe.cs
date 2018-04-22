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
    using Eco.Gameplay.Pipes.LiquidComponents; 

    [RequiresSkill(typeof(AlloySmeltingSkill), 1)]   
    public partial class SteelPipeRecipe : Recipe
    {
        public SteelPipeRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<SteelPipeItem>(),          
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(AlloySmeltingEfficiencySkill), 5, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<OilItem>(typeof(AlloySmeltingEfficiencySkill), 5, AlloySmeltingEfficiencySkill.MultiplicativeStrategy),
				
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(SteelPipeRecipe), Item.Get<SteelPipeItem>().UILink(), 4, typeof(AlloySmeltingSpeedSkill));    
            this.Initialize("Steel Pipe", typeof(SteelPipeRecipe));

            CraftingComponent.AddRecipe(typeof(BlastFurnaceObject), this);
        }
    }

    [Serialized]
    [Solid, Wall, Constructed]
    [RequiresSkill(typeof(AlloySmeltingEfficiencySkill), 1)]   
    public partial class SteelPipeBlock :
        PipeBlock     
    { }

    [Serialized]
    [MaxStackSize(10)]                                       
    [Weight(2000)]      
    [Currency]              
    public partial class SteelPipeItem :
    BlockItem<SteelPipeBlock>
    {
        public override string FriendlyName { get { return "Steel Pipe"; } }
        public override string Description { get { return "A pipe for transporting liquids."; } }

    }

}