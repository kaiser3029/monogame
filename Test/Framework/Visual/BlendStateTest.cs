﻿// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace MonoGame.Tests.Visual
{
    [TestFixture]
    internal class BlendStateTest : VisualTestFixtureBase
    {
        [Test]
        public void ShouldNotBeAbleToSetNullBlendState()
        {
            Game.DrawWith += (sender, e) =>
            {
                Assert.Throws<ArgumentNullException>(() => Game.GraphicsDevice.BlendState = null);
            };
            Game.Run();
        }

        [Test]
        public void ShouldNotBeAbleToMutateStateObjectAfterBindingToGraphicsDevice()
        {
            Game.DrawWith += (sender, e) =>
            {
                var blendState = new BlendState();
                Assert.DoesNotThrow(() => blendState.AlphaBlendFunction = BlendFunction.ReverseSubtract);
#if !XNA
                Assert.DoesNotThrow(() => blendState[0].AlphaBlendFunction = BlendFunction.ReverseSubtract);
#endif

                // Can't mutate after binding.
                Game.GraphicsDevice.BlendState = blendState;
                Assert.Throws<InvalidOperationException>(() => blendState.AlphaBlendFunction = BlendFunction.ReverseSubtract);
#if !XNA
                Assert.Throws<InvalidOperationException>(() => blendState[0].AlphaBlendFunction = BlendFunction.ReverseSubtract);
#endif

                // Even after changing to different BlendState, you still can't mutate a previously-bound object.
                Game.GraphicsDevice.BlendState = BlendState.Opaque;
                Assert.Throws<InvalidOperationException>(() => blendState.AlphaBlendFunction = BlendFunction.ReverseSubtract);
#if !XNA
                Assert.Throws<InvalidOperationException>(() => blendState[0].AlphaBlendFunction = BlendFunction.ReverseSubtract);
#endif
            };
            Game.Run();
        }

        [Test]
        public void ShouldNotBeAbleToMutateDefaultStateObjects()
        {
            Game.DrawWith += (sender, e) =>
            {
                Assert.Throws<InvalidOperationException>(
                    () => BlendState.Additive.AlphaBlendFunction = BlendFunction.ReverseSubtract);
            };
            Game.Run();
        }
    }
}