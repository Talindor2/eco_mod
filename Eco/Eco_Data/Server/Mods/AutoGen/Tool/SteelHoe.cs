namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Gameplay.Pipes;

    [RequiresSkill(typeof(SteelworkingSkill), 3)]   
    [RepairRequiresSkill(typeof(SteelworkingSkill), 1)]
    public partial class SteelHoeRecipe : Recipe
    {
        public SteelHoeRecipe()
        {
            this.Products = new CraftingElement[] { new CraftingElement<SteelHoeItem>() };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<BoardItem>(typeof(SteelworkingEfficiencySkill), 10, SteelworkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SteelItem>(typeof(SteelworkingEfficiencySkill), 20, SteelworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(SteelHoeRecipe), Item.Get<SteelHoeItem>().UILink(), 0.5f, typeof(SteelworkingSpeedSkill));
            this.Initialize("Steel Hoe", typeof(SteelHoeRecipe));
            CraftingComponent.AddRecipe(typeof(AnvilObject), this);
        }
    }
    [Serialized]
    [Weight(1000)]
    [Category("Tool")]
    public partial class SteelHoeItem : HoeItem
    {

        public override string FriendlyName { get { return "Steel Hoe"; } }
        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(15, typeof(HoeEfficiencySkill), typeof(SteelHoeItem), new SteelHoeItem().UILink());
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(10, SteelworkingSkill.MultiplicativeStrategy, typeof(SteelworkingSkill), Localizer.Do("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }


        public override float DurabilityRate { get { return DurabilityMax / 750f; } }
        
        public override Item RepairItem         {get{ return Item.Get<SteelItem>(); } }
        public override int FullRepairAmount    {get{ return 10; } }
    }
}