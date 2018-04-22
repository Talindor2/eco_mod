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
    [RequireComponent(typeof(PowerGridComponent))]              
    [RequireComponent(typeof(PowerConsumptionComponent))]                     
    [RequireComponent(typeof(HousingComponent))]                  
    [RequireComponent(typeof(SolidGroundComponent))]            
    public partial class PumpJackObject : 
        WorldObject    
    {
        public override string FriendlyName { get { return "Pump Jack"; } } 


        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Crafting");                                 
            this.GetComponent<PowerConsumptionComponent>().Initialize(100);                      
            this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());        
            this.GetComponent<HousingComponent>().Set(PumpJackItem.HousingVal);                                



        }

        public override void Destroy()
        {
            base.Destroy();
        }
       
    }

    [Serialized]
    public partial class PumpJackItem : WorldObjectItem<PumpJackObject>
    {
        public override string FriendlyName { get { return "Pump Jack"; } } 
        public override string Description  { get { return  "Drill, baby! Drill!"; } }

        static PumpJackItem()
        {
            
        }

        [TooltipChildren] public HousingValue HousingTooltip { get { return HousingVal; } }
        [TooltipChildren] public static HousingValue HousingVal { get { return new HousingValue() 
                                                {
                                                    Category = "Industrial",
                                                    TypeForRoomLimit = "", 
        };}}
    }


    [RequiresSkill(typeof(MechanicalEngineeringSkill), 4)]
    public partial class PumpJackRecipe : Recipe
    {
        public PumpJackRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<PumpJackItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<ConcreteItem>(typeof(MechanicsAssemblyEfficiencySkill), 25, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearboxItem>(typeof(MechanicsAssemblyEfficiencySkill), 25, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PistonItem>(typeof(MechanicsAssemblyEfficiencySkill), 10, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<RivetItem>(typeof(MechanicsAssemblyEfficiencySkill), 15, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<SteelItem>(typeof(MechanicsAssemblyEfficiencySkill), 20, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),	
            };
            SkillModifiedValue value = new SkillModifiedValue(50, MechanicsAssemblySpeedSkill.MultiplicativeStrategy, typeof(MechanicsAssemblySpeedSkill), Localizer.Do("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(PumpJackRecipe), Item.Get<PumpJackItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<PumpJackItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Pump Jack", typeof(PumpJackRecipe));
            CraftingComponent.AddRecipe(typeof(MachineShopObject), this);
        }
    }
}