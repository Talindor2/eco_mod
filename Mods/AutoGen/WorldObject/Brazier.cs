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
    [RequireComponent(typeof(OnOffComponent))]                   
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]                
    [RequireComponent(typeof(FuelSupplyComponent))]                      
    [RequireComponent(typeof(FuelConsumptionComponent))]                 
    [RequireComponent(typeof(HousingComponent))]                  
    [RequireComponent(typeof(SolidGroundComponent))]            
    public partial class BrazierObject : 
        WorldObject    
    {
        public override string FriendlyName { get { return "Brazier"; } } 

        private static Type[] fuelTypeList = new Type[]
        {
            typeof(LogItem),
            typeof(LumberItem),
            typeof(CharcoalItem),
            typeof(ArrowItem),
            typeof(BoardItem),
            typeof(CoalItem),
        };

        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Lights");                                 
            this.GetComponent<FuelSupplyComponent>().Initialize(2, fuelTypeList);                           
            this.GetComponent<FuelConsumptionComponent>().Initialize(1);                    
            this.GetComponent<HousingComponent>().Set(BrazierItem.HousingVal);                                



        }

        public override void Destroy()
        {
            base.Destroy();
        }
       
    }

    [Serialized]
    public partial class BrazierItem : WorldObjectItem<BrazierObject>
    {
        public override string FriendlyName { get { return "Brazier"; } } 
        public override string Description  { get { return  "A metal stand which can hold burning fuel to provide light."; } }

        static BrazierItem()
        {
            
        }

        [TooltipChildren] public HousingValue HousingTooltip { get { return HousingVal; } }
        [TooltipChildren] public static HousingValue HousingVal { get { return new HousingValue() 
                                                {
                                                    Category = "General",
                                                    Val = 2,                                   
                                                    TypeForRoomLimit = "Lights", 
                                                    DiminishingReturnPercent = 0.8f    
        };}}
    }


    [RequiresSkill(typeof(MetalworkingSkill), 4)]
    public partial class BrazierRecipe : Recipe
    {
        public BrazierRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<BrazierItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(MetalworkingEfficiencySkill), 10, MetalworkingEfficiencySkill.MultiplicativeStrategy), 
				new CraftingElement<OilItem>(typeof(MetalworkingEfficiencySkill), 5, MetalworkingEfficiencySkill.MultiplicativeStrategy),	
            };
            SkillModifiedValue value = new SkillModifiedValue(1, MetalworkingSpeedSkill.MultiplicativeStrategy, typeof(MetalworkingSpeedSkill), Localizer.Do("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(BrazierRecipe), Item.Get<BrazierItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<BrazierItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Brazier", typeof(BrazierRecipe));
            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }
}