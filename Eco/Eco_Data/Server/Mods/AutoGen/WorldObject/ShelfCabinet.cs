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
    [RequireComponent(typeof(HousingComponent))]                  
    [RequireComponent(typeof(PublicStorageComponent))]                
    [RequireComponent(typeof(SolidGroundComponent))]            
    public partial class ShelfCabinetObject : 
        WorldObject    
    {
        public override string FriendlyName { get { return "Shelf Cabinet"; } } 


        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Housing");                                 
            this.GetComponent<HousingComponent>().Set(ShelfCabinetItem.HousingVal);                                

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
    public partial class ShelfCabinetItem : WorldObjectItem<ShelfCabinetObject>
    {
        public override string FriendlyName { get { return "Shelf Cabinet"; } } 
        public override string Description  { get { return  "When a shelf and a cabinet aren't enough individually."; } }

        static ShelfCabinetItem()
        {
            
        }

        [TooltipChildren] public HousingValue HousingTooltip { get { return HousingVal; } }
        [TooltipChildren] public static HousingValue HousingVal { get { return new HousingValue() 
                                                {
                                                    Category = "General",
                                                    Val = 2,                                   
                                                    TypeForRoomLimit = "Shelves", 
                                                    DiminishingReturnPercent = 0.7f    
        };}}
    }


    [RequiresSkill(typeof(LumberWoodworkingSkill), 3)]
    public partial class ShelfCabinetRecipe : Recipe
    {
        public ShelfCabinetRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<ShelfCabinetItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LumberItem>(typeof(LumberWoodworkingEfficiencySkill), 20, LumberWoodworkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<PaperItem>(typeof(LumberWoodworkingEfficiencySkill), 100, LumberWoodworkingEfficiencySkill.MultiplicativeStrategy),   
            };
            SkillModifiedValue value = new SkillModifiedValue(5, LumberWoodworkingSpeedSkill.MultiplicativeStrategy, typeof(LumberWoodworkingSpeedSkill), Localizer.Do("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(ShelfCabinetRecipe), Item.Get<ShelfCabinetItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<ShelfCabinetItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Shelf Cabinet", typeof(ShelfCabinetRecipe));
            CraftingComponent.AddRecipe(typeof(SawmillObject), this);
        }
    }
}