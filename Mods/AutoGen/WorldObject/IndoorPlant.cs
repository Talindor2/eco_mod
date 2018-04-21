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
    [RequireComponent(typeof(HousingComponent))]                          
    [RequireComponent(typeof(SolidGroundComponent))]                           
    public partial class IndoorPlantObject : WorldObject
    {
        public override string FriendlyName { get { return "Indoor Plant"; } } 


        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Misc");                                 
            this.GetComponent<HousingComponent>().Set(IndoorPlantItem.HousingVal);                                



        }

        public override void Destroy()
        {
            base.Destroy();
        }
        static IndoorPlantObject()
        {
            AddOccupancyList(typeof(IndoorPlantObject), new BlockOccupancy(Vector3i.Zero, typeof(WorldObjectBlock)));
        }
    }

    [Serialized]
    public partial class IndoorPlantItem : WorldObjectItem<IndoorPlantObject>
    {
        public override string FriendlyName { get { return "Indoor Plant"; } } 
        public override string Description { get { return "Evergreen!!"; } }

        static IndoorPlantItem()
        {
            
        }
        
        [TooltipChildren] public HousingValue HousingTooltip { get { return HousingVal; } }
        [TooltipChildren] public static HousingValue HousingVal { get { return new HousingValue() 
                                                {
                                                    Category = "General",
                                                    Val = 1,
                                                    TypeForRoomLimit = "Decoration",
                                                    DiminishingReturnPercent = 0.9f
                                                };}}       
    }

    [RequiresSkill(typeof(ClayProductionSkill), 2)]
    public partial class IndoorPlantRecipe : Recipe
    {
        public IndoorPlantRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<IndoorPlantItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<ClayItem>(typeof(ClayProductionEfficiencySkill), 10, ClayProductionEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PlantFibersItem>(typeof(ClayProductionEfficiencySkill), 10, ClayProductionEfficiencySkill.MultiplicativeStrategy),
            };
            SkillModifiedValue value = new SkillModifiedValue(10, ClayProductionSpeedSkill.MultiplicativeStrategy, typeof(ClayProductionSpeedSkill), Localizer.Do("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(IndoorPlantRecipe), Item.Get<IndoorPlantItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<IndoorPlantItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Indoor Plant", typeof(IndoorPlantRecipe));
            CraftingComponent.AddRecipe(typeof(KilnObject), this);
        }
    }
}