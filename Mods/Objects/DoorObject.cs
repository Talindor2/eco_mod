﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Audio;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Wires;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using Eco.Gameplay.Rooms;
    using System.Collections.Generic;
    using Eco.Shared.Utils;
    
    public partial class DoorObject : WorldObject, IWireContainer, IVariableAirtight
    {
        [Serialized] public bool OpensOut { get; private set; }
        [Serialized] public bool Open { get; private set; }

        public bool IsAirtight { get { return !this.Open; } }

        WireInput input;
        IEnumerable<WireConnection> IWireContainer.Wires { get { return this.input.SingleItemAsEnumerable(); } }

        protected DoorObject() { }

        public void SetOpen(bool open)
        {
            if (this.Open == open) return;
            this.ToggleOpen();
        }

        public override InteractResult OnActRight(InteractionContext context)
        {
            this.ToggleOpen();
            return InteractResult.Success;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            // determine open direction
            var placerPos = this.Creator.User.Position;
            var toDoor = placerPos - this.Position;
            var facing = this.Rotation.RotateVector(Vector3.Forward);
            this.OpensOut = Vector3.Dot(toDoor, facing) < 0;
        }

        protected override void PostInitialize()
        {
            this.input = WireInput.CreateSignalInput(this, "Open Door", v => this.SetOpen(v == 0f ? false : true));
            base.Initialize();

            this.GetComponent<PropertyAuthComponent>().Initialize(AuthModeType.Inherited);
        }

        public override void SendInitialState(BSONObject bsonObj, INetObjectViewer viewer)
        {
            base.SendInitialState(bsonObj, viewer);
            bsonObj["open"] = this.Open;
            bsonObj["opensOut"] = this.OpensOut;
        }

        private void ToggleOpen()
        {
            if (this.Open)
                AudioManager.PlayAudio("Doors/DoorCloseSfx", this.Position);
            if (!this.Open)
                AudioManager.PlayAudio("Doors/DoorOpenSfx", this.Position);

            this.Open = !this.Open;
            this.RPC("Toggle");

            //If we're tracking atmosphere, treat this as a block change.
            if (!RoomData.Obj.RoomConfig.EmptyBlocksCountAsWindows)
                RoomData.TestLandscapeChange(this.Position3i, !this.Open);
        }
    }
}
