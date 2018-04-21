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

    [RequiresSkill(typeof(MetalworkingSkill), 1)]   
    public partial class IronPipeRecipe : Recipe
    {
        public IronPipeRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<IronPipeItem>(),          
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(MetalworkingEfficiencySkill), 5, MetalworkingEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<OilItem>(typeof(MetalworkingEfficiencySkill), 5, MetalworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(IronPipeRecipe), Item.Get<IronPipeItem>().UILink(), 2, typeof(MetalworkingSpeedSkill));    
            this.Initialize("Iron Pipe", typeof(IronPipeRecipe));

            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }

    [Serialized]
    [Solid, Wall, Constructed]
    [RequiresSkill(typeof(MetalworkingEfficiencySkill), 1)]   
    public partial class IronPipeBlock :
        PipeBlock     
    { }

    [Serialized]
    [MaxStackSize(10)]                                       
    [Weight(2000)]      
    [Currency]              
    public partial class IronPipeItem :
    BlockItem<IronPipeBlock>
    {
        public override string FriendlyName { get { return "Iron Pipe"; } }
        public override string Description { get { return "A pipe for transporting liquids."; } }

    }

}