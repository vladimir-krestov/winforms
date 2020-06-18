// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using Accessibility;
using Xunit;

namespace System.Windows.Forms.Tests.AccessibleObjects
{
    public class TrackBarAccessibleObjectTests : IClassFixture<ThreadExceptionFixture>
    {
        [WinFormsFact]
        public void TrackBarAccessibilityObject_Properties_ReturnsExpected()
        {
            using var ownerControl = new TrackBar
            {
                Value = 5,
            };

            ownerControl.CreateControl();
            Control.ControlAccessibleObject accessibilityObject = Assert.IsAssignableFrom<Control.ControlAccessibleObject>(ownerControl.AccessibilityObject);
            Assert.Equal(ownerControl.Size, accessibilityObject.Bounds.Size);
            Assert.Null(accessibilityObject.DefaultAction);
            Assert.Null(accessibilityObject.Description);
            Assert.Equal(ownerControl.Handle, accessibilityObject.Handle);
            Assert.Null(accessibilityObject.Help);
            Assert.Null(accessibilityObject.KeyboardShortcut);
            Assert.Null(accessibilityObject.Name);
            Assert.Equal(AccessibleRole.Slider, accessibilityObject.Role);
            Assert.Same(ownerControl, accessibilityObject.Owner);
            Assert.NotNull(accessibilityObject.Parent);
            Assert.Equal(AccessibleStates.Focusable, accessibilityObject.State);
            Assert.Equal("50", accessibilityObject.Value);
        }

        [WinFormsTheory]
        [InlineData("100", 10, "100", true)]
        [InlineData("50", 5, "50", true)]
        [InlineData("54", 5, "50", true)]
        [InlineData("56", 5, "50", true)]
        [InlineData("0", 0, "0", true)]
        [InlineData("100", 0, "", false)]
        [InlineData("50", 0, "", false)]
        [InlineData("54", 0, "", false)]
        [InlineData("56", 0, "", false)]
        [InlineData("0", 0, "", false)]
        public void TrackBarAccessibilityObject_Value_Set_GetReturnsExpected(string value, int expected, string expectedValueString, bool createControl)
        {
            using var ownerControl = new TrackBar();
            if (createControl)
            {
                ownerControl.CreateControl();
            }

            Assert.Equal(createControl, ownerControl.IsHandleCreated);
            Control.ControlAccessibleObject accessibilityObject = Assert.IsAssignableFrom<Control.ControlAccessibleObject>(ownerControl.AccessibilityObject);
            Assert.Equal(createControl, ownerControl.IsHandleCreated);
            accessibilityObject.Value = value;
            Assert.Equal(expectedValueString, accessibilityObject.Value);
            Assert.Equal(expected, ownerControl.Value);

            // Set same.
            accessibilityObject.Value = value;
            Assert.Equal(expectedValueString, accessibilityObject.Value);
            Assert.Equal(expected, ownerControl.Value);
        }

        [WinFormsTheory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("NotAnInt")]
        public void TrackBarAccessibilityObject_Value_SetInvalid_ThrowsCOMException(string value)
        {
            using var ownerControl = new TrackBar
            {
                Value = 5
            };

            ownerControl.CreateControl();
            Control.ControlAccessibleObject accessibilityObject = Assert.IsAssignableFrom<Control.ControlAccessibleObject>(ownerControl.AccessibilityObject);
            Assert.Throws<COMException>(() => accessibilityObject.Value = value);
            Assert.Equal("50", accessibilityObject.Value);
            Assert.Equal(5, ownerControl.Value);
        }

        [WinFormsTheory]
        [InlineData("101")]
        [InlineData("-1")]
        public void TrackBarAccessibilityObject_Value_SetOutOfRange_ThrowsArgumentException(string value)
        {
            using var ownerControl = new TrackBar
            {
                Value = 5
            };

            ownerControl.CreateControl();
            Control.ControlAccessibleObject accessibilityObject = Assert.IsAssignableFrom<Control.ControlAccessibleObject>(ownerControl.AccessibilityObject);
            Assert.Throws<ArgumentException>(null, () => accessibilityObject.Value = value);
            Assert.Equal("50", accessibilityObject.Value);
            Assert.Equal(5, ownerControl.Value);
        }

        [WinFormsFact]
        public void TrackBarAccessibilityObject_GetChildCount_ReturnsExpected()
        {
            using var ownerControl = new TrackBar
            {
                Value = 5
            };

            ownerControl.CreateControl();
            Control.ControlAccessibleObject accessibilityObject = Assert.IsAssignableFrom<Control.ControlAccessibleObject>(ownerControl.AccessibilityObject);
            IAccessible iAccessible = accessibilityObject;
            Assert.Equal(3, iAccessible.accChildCount);
            Assert.Equal(-1, accessibilityObject.GetChildCount());
        }
    }
}
