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
    [RequireComponent(typeof(SolidGroundComponent))]            
    public partial class DoorObject : 
        WorldObject    
    {
        public override string FriendlyName { get { return "Door"; } } 


        protected override void Initialize()
        {



        }

        public override void Destroy()
        {
            base.Destroy();
        }
       
    }

    [Serialized]
    public partial class DoorItem : WorldObjectItem<DoorObject>
    {
        public override string FriendlyName { get { return "Door"; } } 
        public override string Description  { get { return  "A sturdy wooden door. Can be locked for certain players."; } }

        static DoorItem()
        {
            
        }

    }


    [RequiresSkill(typeof(WoodworkingSkill), 0)]
    public partial class DoorRecipe : Recipe
    {
        public DoorRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<DoorItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<LogItem>(typeof(WoodworkingEfficiencySkill), 6, WoodworkingEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<RivetItem>(typeof(WoodworkingEfficiencySkill), 1, WoodworkingEfficiencySkill.MultiplicativeStrategy),	
            };
            SkillModifiedValue value = new SkillModifiedValue(5, WoodworkingSpeedSkill.MultiplicativeStrategy, typeof(WoodworkingSpeedSkill), Localizer.Do("craft time"));
            SkillModifiedValueManager.AddBenefitForObject(typeof(DoorRecipe), Item.Get<DoorItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<DoorItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Door", typeof(DoorRecipe));
            CraftingComponent.AddRecipe(typeof(CarpentryTableObject), this);
        }
    }
}