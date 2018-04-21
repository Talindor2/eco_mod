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

    [RequiresSkill(typeof(SteelworkingSkill), 4)]   
    [RepairRequiresSkill(typeof(SteelworkingSkill), 3)]
    public partial class ModernHammerRecipe : Recipe
    {
        public ModernHammerRecipe()
        {
            this.Products = new CraftingElement[] { new CraftingElement<ModernHammerItem>() };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<FiberglassItem>(typeof(SteelworkingEfficiencySkill), 20, SteelworkingEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<SteelItem>(typeof(SteelworkingEfficiencySkill), 30, SteelworkingEfficiencySkill.MultiplicativeStrategy),
            };
            this.CraftMinutes = CreateCraftTimeValue(typeof(ModernHammerRecipe), Item.Get<ModernHammerItem>().UILink(), 0.5f, typeof(SteelworkingSpeedSkill));
            this.Initialize("Modern Hammer", typeof(ModernHammerRecipe));
            CraftingComponent.AddRecipe(typeof(FactoryObject), this);
        }
    }
    [Serialized]
    [Weight(1000)]
    [Category("Tool")]
    public partial class ModernHammerItem : HammerItem
    {

        public override string FriendlyName { get { return "Modern Hammer"; } }
        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(10, typeof(CalorieEfficiencySkill), typeof(ModernHammerItem), new ModernHammerItem().UILink());
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        private static SkillModifiedValue skilledRepairCost = new SkillModifiedValue(20, SteelworkingSkill.MultiplicativeStrategy, typeof(SteelworkingSkill), Localizer.Do("repair cost"));
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }


        public override float DurabilityRate { get { return DurabilityMax / 2000f; } }
        
        public override Item RepairItem         {get{ return Item.Get<SteelItem>(); } }
        public override int FullRepairAmount    {get{ return 20; } }
    }
}