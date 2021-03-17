// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using static Interop;

namespace System.Windows.Forms
{
    public partial class MonthCalendar
    {
        internal class CalendarNextButtonAccessibleObject : CalendarButtonAccessibleObject
        {
            // The "Next" button is the second in the calendar accessibility tree.
            // Indices start at 1.
            private const int ChildId = 2;

            private readonly MonthCalendarAccessibleObject _monthCalendarAccessibleObject;

            public CalendarNextButtonAccessibleObject(MonthCalendarAccessibleObject calendarAccessibleObject)
                : base(calendarAccessibleObject)
            {
                _monthCalendarAccessibleObject = calendarAccessibleObject;
            }

            public override Rectangle Bounds
                => _monthCalendarAccessibleObject.GetCalendarPartRectangle(ComCtl32.MCGIP.NEXT);

            public override string Description => SR.CalendarNextButtonAccessibleObjectDescription;

            internal override UiaCore.IRawElementProviderFragment? FragmentNavigate(UiaCore.NavigateDirection direction)
                => direction switch
                {
                    UiaCore.NavigateDirection.PreviousSibling => _monthCalendarAccessibleObject.PreviousButtonAccessibleObject,
                    UiaCore.NavigateDirection.NextSibling => _monthCalendarAccessibleObject.CalendarsAccessibleObjects.First?.Value,
                    _ => base.FragmentNavigate(direction)
                };

            internal override int GetChildId() => ChildId;

            private protected override bool IsEnabled
                => _monthCalendarAccessibleObject.IsEnabled
                // If there is an opportunity to move to the next dates
                && _monthCalendarAccessibleObject.MaxDate > _monthCalendarAccessibleObject.GetDisplayRange(true).End;

            public override string Name => SR.MonthCalendarNextButtonAccessibleName;
        }
    }
}
