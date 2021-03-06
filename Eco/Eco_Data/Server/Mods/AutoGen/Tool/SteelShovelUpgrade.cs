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

	[RequiresSkill(typeof(SteelworkingSkill), 2)]
	public partial class SteelShovelUpgradeRecipe : Recipe
	{
		public SteelShovelUpgradeRecipe()
		{
			this.Products = new CraftingElement[] {
				new CraftingElement<SteelShovelItem>(1),
//				new CraftingElement<IronIngotItem>(2),		//	20
			};
			this.Ingredients = new CraftingElement[]
			{
				new CraftingElement<IronShovelItem>(typeof(MetalworkingEfficiencySkill), 5, MetalworkingEfficiencySkill.MultiplicativeStrategy),
//				new CraftingElement<BoardItem>(typeof(SteelworkingEfficiencySkill), 10, SteelworkingEfficiencySkill.MultiplicativeStrategy),
				new CraftingElement<SteelItem>(typeof(SteelworkingEfficiencySkill), 20, SteelworkingEfficiencySkill.MultiplicativeStrategy),
			};
			this.CraftMinutes = CreateCraftTimeValue(typeof(SteelShovelUpgradeRecipe), Item.Get<SteelShovelItem>().UILink(), 0.75f, typeof(SteelworkingSpeedSkill));
			this.Initialize("Steel Shovel (Upgrade)", typeof(SteelShovelUpgradeRecipe));
			CraftingComponent.AddRecipe(typeof(AnvilObject), this);
		}
	}
}