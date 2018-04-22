namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    
    [Serialized]    
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]                
    [RequireComponent(typeof(LinkComponent))]                   
    [RequireComponent(typeof(CraftingComponent))]               
    [RequireComponent(typeof(SolidGroundComponent))]            
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireRoomContainment]
    [RequireRoomVolume(25)]                              
    public partial class FarmersTableObject : 
        WorldObject    
    {
        public override string FriendlyName { get { return "Farmers Table"; } } 


        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Crafting");                                 



        }

        public override void Destroy()
        {
            base.Destroy();
        }
       
    }

    [Serialized]
    public partial class FarmersTableItem : WorldObjectItem<FarmersTableObject>
    {
        public override string FriendlyName { get { return "Farmers Table"; } } 
        public override string Description  { get { return  "A basic table for creating farming tools and similar products."; } }

        static FarmersTableItem()
        {
            
        }

    }


    [RequiresSkill(typeof(FarmingSkill), 1)]
    public partial class FarmersTableRecipe : Recipe
    {
        public FarmersTableRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<FarmersTableItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<DirtItem>(typeof(BasicCraftingEfficiencySkill), 100, BasicCraftingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<LogItem>(typeof(BasicCraftingEfficiencySkill), 50, BasicCraftingEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<RivetItem>(typeof(BasicCraftingEfficiencySkill), 50, BasicCraftingEfficiencySkill.MultiplicativeStrategy),	
            };
            SkillModifiedValue value = new SkillModifiedValue(10, BasicCraftingSpeedSkill.MultiplicativeStrategy, typeof(BasicCraftingSpeedSkill), Localizer.Do("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(FarmersTableRecipe), Item.Get<FarmersTableItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<FarmersTableItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Farmers Table", typeof(FarmersTableRecipe));
            CraftingComponent.AddRecipe(typeof(WorkbenchObject), this);
        }
    }
}