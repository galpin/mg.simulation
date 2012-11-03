// Copyright (c) Sahara Force India Formula One Team 2012.

using System;
using Xunit;

namespace Basics.NET.Tests
{
    public class GuardTests
    {
        #region Public Methods

        [Fact]
        public void IsNotNull_WithExpression_ThrowsIfParameterIsNull_Test()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(() => Guard.IsNotNull(obj, () => obj));
        }

        [Fact]
        public void IsNotNull_WithExpression_DoesNotThrowIfParameterIsNotNull_Test()
        {
            var obj = new object();
            Assert.DoesNotThrow(() => Guard.IsNotNull(obj, () => obj));
        }

        [Fact]
        public void IsNotNull_WithString_ThrowsIfParameterIsNull_Test()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(() => Guard.IsNotNull(obj, "obj"));
        }

        [Fact]
        public void IsNotNull_WithString_DoesNotThrowIfParameterIsNotNull_Test()
        {
            var obj = new object();
            Assert.DoesNotThrow(() => Guard.IsNotNull(obj, "obj"));
        }

        #endregion
    }
}