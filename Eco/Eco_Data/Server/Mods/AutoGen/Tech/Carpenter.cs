namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Eco.Core.Utils;
    using Eco.Core.Utils.AtomicAction;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Services;
    using Eco.Shared.Utils;
    using Gameplay.Systems.Tooltip;

    [Serialized]
    public partial class CarpenterSkill : Skill
    {
        public override string FriendlyName { get { return "Carpenter"; } }
        public override string Description { get { return Localizer.Do(""); } }

        public override string Title { get { return Localizer.Do("Carpenter"); } }  
        public override int RequiredPoint { get { return 0; } }
        public override int MaxLevel { get { return 1; } }
    }

}
