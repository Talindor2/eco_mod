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
    [RequireComponent(typeof(PipeComponent))]                    
    [RequireComponent(typeof(AttachmentComponent))]              
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]                
    [RequireComponent(typeof(LinkComponent))]                   
    [RequireComponent(typeof(CraftingComponent))]               
    [RequireComponent(typeof(FuelSupplyComponent))]                      
    [RequireComponent(typeof(FuelConsumptionComponent))]                 
    [RequireComponent(typeof(HousingComponent))]                  
    [RequireComponent(typeof(SolidGroundComponent))]            
    public partial class OilRefineryObject : 
        WorldObject    
    {
        public override string FriendlyName { get { return "Oil Refinery"; } } 

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
            this.GetComponent<MinimapComponent>().Initialize("Crafting");                                 
            this.GetComponent<FuelSupplyComponent>().Initialize(2, fuelTypeList);                           
            this.GetComponent<FuelConsumptionComponent>().Initialize(50);                    
            this.GetComponent<HousingComponent>().Set(OilRefineryItem.HousingVal);                                


            var tankList = new List<LiquidTank>();
            
            tankList.Add(new LiquidProducer("Chimney", typeof(SmogItem), 100,
                    null,                                                                
                    this.Occupancy.Find(x => x.Name == "ChimneyOut"),   
                        (float)(0.25f * SmogItem.SmogItemsPerCO2PPM) / TimeUtil.SecondsPerHour)); 
            
            
            
            this.GetComponent<PipeComponent>().Initialize(tankList);

        }

        public override void Destroy()
        {
            base.Destroy();
        }
       
    }

    [Serialized]
    public partial class OilRefineryItem : WorldObjectItem<OilRefineryObject>
    {
        public override string FriendlyName { get { return "Oil Refinery"; } } 
        public override string Description  { get { return  "Sets of pipes and tanks which refine crude petroleum into usable products."; } }

        static OilRefineryItem()
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
    public partial class OilRefineryRecipe : Recipe
    {
        public OilRefineryRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<OilRefineryItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<BrickItem>(typeof(MechanicsAssemblyEfficiencySkill), 50, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<GearItem>(typeof(MechanicsAssemblyEfficiencySkill), 30, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<IronIngotItem>(typeof(MechanicsAssemblyEfficiencySkill), 100, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<RivetItem>(typeof(MechanicsAssemblyEfficiencySkill), 75, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<OilItem>(typeof(MechanicsAssemblyEfficiencySkill), 20, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<CopperWiringItem>(typeof(MechanicsAssemblyEfficiencySkill), 50, MechanicsAssemblyEfficiencySkill.MultiplicativeStrategy),	
            };
            SkillModifiedValue value = new SkillModifiedValue(50, MechanicsAssemblySpeedSkill.MultiplicativeStrategy, typeof(MechanicsAssemblySpeedSkill), Localizer.Do("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(OilRefineryRecipe), Item.Get<OilRefineryItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<OilRefineryItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Oil Refinery", typeof(OilRefineryRecipe));
            CraftingComponent.AddRecipe(typeof(MachineShopObject), this);
        }
    }
}