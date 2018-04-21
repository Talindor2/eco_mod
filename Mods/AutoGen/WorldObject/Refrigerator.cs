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
    [RequireComponent(typeof(PowerGridComponent))]              
    [RequireComponent(typeof(PowerConsumptionComponent))]                     
    [RequireComponent(typeof(HousingComponent))]                  
    [RequireComponent(typeof(PublicStorageComponent))]                
    [RequireComponent(typeof(SolidGroundComponent))]            
    public partial class RefrigeratorObject : 
        WorldObject    
    {
        public override string FriendlyName { get { return "Refrigerator"; } } 


        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Housing");                                 
            this.GetComponent<PowerConsumptionComponent>().Initialize(100);                      
            this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());        
            this.GetComponent<HousingComponent>().Set(RefrigeratorItem.HousingVal);                                

            var storage = this.GetComponent<PublicStorageComponent>();
            storage.Initialize(8);
            storage.Storage.AddRestriction(new NotCarriedRestriction()); // can't store block or large items


        }

        public override void Destroy()
        {
            base.Destroy();
        }
       
    }

    [Serialized]
    public partial class RefrigeratorItem : WorldObjectItem<RefrigeratorObject>
    {
        public override string FriendlyName { get { return "Refrigerator"; } } 
        public override string Description  { get { return  "An icebox from the future with significantly better cooling properties."; } }

        static RefrigeratorItem()
        {
            
        }

        [TooltipChildren] public HousingValue HousingTooltip { get { return HousingVal; } }
        [TooltipChildren] public static HousingValue HousingVal { get { return new HousingValue() 
                                                {
                                                    Category = "Kitchen",
                                                    Val = 10,                                   
                                                    TypeForRoomLimit = "Food Storage", 
                                                    DiminishingReturnPercent = 0.3f    
        };}}
    }


    [RequiresSkill(typeof(IndustrialEngineeringSkill), 3)]
    public partial class RefrigeratorRecipe : Recipe
    {
        public RefrigeratorRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<RefrigeratorItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<SteelItem>(typeof(IndustrialEngineeringEfficiencySkill), 20, IndustrialEngineeringEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<CircuitItem>(typeof(IndustrialEngineeringEfficiencySkill), 5, IndustrialEngineeringEfficiencySkill.MultiplicativeStrategy),   
            };
            SkillModifiedValue value = new SkillModifiedValue(10, IndustrialEngineeringSpeedSkill.MultiplicativeStrategy, typeof(IndustrialEngineeringSpeedSkill), Localizer.Do("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(RefrigeratorRecipe), Item.Get<RefrigeratorItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<RefrigeratorItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Refrigerator", typeof(RefrigeratorRecipe));
            CraftingComponent.AddRecipe(typeof(FactoryObject), this);
        }
    }
}