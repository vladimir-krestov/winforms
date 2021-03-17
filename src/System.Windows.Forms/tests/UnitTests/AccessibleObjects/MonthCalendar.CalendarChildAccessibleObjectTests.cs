﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using static System.Windows.Forms.MonthCalendar;

namespace System.Windows.Forms.Tests.AccessibleObjects
{
    public class CalendarChildAccessibleObjectTests : IClassFixture<ThreadExceptionFixture>
    {
        [WinFormsFact]
        public void CalendarChildAccessibleObject_ctor_ThrowsException_IfCalendarAccessibleObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SubObject(null));
        }

        private class SubObject : MonthCalendarChildAccessibleObject
        {
            public SubObject(MonthCalendarAccessibleObject calendarAccessibleObject)
                : base(calendarAccessibleObject)
            { }
        }
    }
}
