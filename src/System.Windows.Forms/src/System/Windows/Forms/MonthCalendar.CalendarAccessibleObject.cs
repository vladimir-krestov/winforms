// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using System.Linq;
using static Interop;
using static Interop.ComCtl32;

namespace System.Windows.Forms
{
    public partial class MonthCalendar
    {
        /// <summary>
        ///  Represents the calendar body accessible object.
        /// </summary>
        internal class CalendarAccessibleObject : MonthCalendarChildAccessibleObject
        {
            // This const is used to get ChildId.
            // It should take into account "Next" and "Previous" buttons.
            // Indices start at 1.
            private const int ChildIdIncrement = 3;

            private readonly MonthCalendarAccessibleObject _monthCalendarAccessibleObject;
            private readonly int _calendarIndex;
            private readonly string _initName;
            private CalendarBodyAccessibleObject? _calendarBodyAccessibleObject;
            private CalendarHeaderAccessibleObject? _calendarHeaderAccessibleObject;
            private SelectionRange? _dateRange;

            public CalendarAccessibleObject(MonthCalendarAccessibleObject calendarAccessibleObject, int calendarIndex, string initName)
                : base(calendarAccessibleObject)
            {
                _monthCalendarAccessibleObject = calendarAccessibleObject;
                _calendarIndex = calendarIndex;
                // Name doesn't change if the calendar date range is not changed,
                // otherwise the calendar accessibility tree will be rebuilt.
                // So save this value one time to avoid sending messages to Windows every time.
                _initName = initName;
            }

            public override Rectangle Bounds
                => _monthCalendarAccessibleObject.GetCalendarPartRectangle(MCGIP.CALENDAR, _calendarIndex);

            internal CalendarBodyAccessibleObject CalendarBodyAccessibleObject
                => _calendarBodyAccessibleObject ??= new(this, _monthCalendarAccessibleObject, _calendarIndex);

            internal CalendarHeaderAccessibleObject CalendarHeaderAccessibleObject
                => _calendarHeaderAccessibleObject ??= new(this, _monthCalendarAccessibleObject, _calendarIndex);

            internal override int Column => _calendarIndex % _monthCalendarAccessibleObject.ColumnCount;

            internal override UiaCore.IRawElementProviderSimple? ContainingGrid => _monthCalendarAccessibleObject;

            internal SelectionRange? DateRange
            {
                get
                {
                    if (_dateRange is null)
                    {
                        SelectionRange? dateRange = _monthCalendarAccessibleObject.GetCalendarPartDateRange(MCGIP.CALENDAR, _calendarIndex);
                        if (dateRange is null)
                        {
                            return null;
                        }

                        // Add gray dates of the previous or next calendars
                        SelectionRange displayRange = _monthCalendarAccessibleObject.GetDisplayRange(false);
                        if (_calendarIndex == 0 && displayRange.Start < dateRange.Start)
                        {
                            dateRange.Start = displayRange.Start;
                        }

                        if (_monthCalendarAccessibleObject.CalendarsAccessibleObjects.Last?.Value == this
                            && displayRange.End > dateRange.End)
                        {
                            dateRange.End = displayRange.End;
                        }

                        _dateRange = dateRange;
                    }

                    return _dateRange;
                }
            }

            internal override UiaCore.IRawElementProviderFragment? FragmentNavigate(UiaCore.NavigateDirection direction)
                => direction switch
                {
                    UiaCore.NavigateDirection.NextSibling => _monthCalendarAccessibleObject.CalendarsAccessibleObjects
                        .Find(this)?.Next?.Value
                        ?? (_monthCalendarAccessibleObject.ShowToday
                        ? (AccessibleObject)_monthCalendarAccessibleObject.TodayLinkAccessibleObject
                        : null),
                    UiaCore.NavigateDirection.PreviousSibling => _monthCalendarAccessibleObject.CalendarsAccessibleObjects
                        .Find(this)?.Previous?.Value
                        ?? (AccessibleObject)_monthCalendarAccessibleObject.NextButtonAccessibleObject,
                    UiaCore.NavigateDirection.FirstChild => CalendarHeaderAccessibleObject,
                    UiaCore.NavigateDirection.LastChild => CalendarBodyAccessibleObject,
                    _ => base.FragmentNavigate(direction),
                };

            internal override int GetChildId() => ChildIdIncrement + _calendarIndex;

            internal override UiaCore.IRawElementProviderSimple[]? GetColumnHeaderItems() => null;

            internal MonthCalendarChildAccessibleObject? GetChildFromPoint(MCHITTESTINFO hitTestInfo)
            {
                CalendarRowAccessibleObject? row = CalendarBodyAccessibleObject.RowsAccessibleObjects.FirstOrDefault(r => r.Row == hitTestInfo.iRow);
                CalendarCellAccessibleObject? cell = row?.CellsAccessibleObjects.FirstOrDefault(c => c.Column == hitTestInfo.iCol);

                if (cell is not null)
                {
                    return cell;
                }

                if (row is not null)
                {
                    return row;
                }

                return CalendarBodyAccessibleObject;
            }

            internal override object? GetPropertyValue(UiaCore.UIA propertyID)
                => propertyID switch
                {
                    UiaCore.UIA.IsKeyboardFocusablePropertyId => IsEnabled,
                    _ => base.GetPropertyValue(propertyID)
                };

            internal override UiaCore.IRawElementProviderSimple[]? GetRowHeaderItems() => null;

            private protected override bool HasKeyboardFocus
                => _monthCalendarAccessibleObject.FocusedCell?.CalendarIndex == _calendarIndex;

            internal override bool IsPatternSupported(UiaCore.UIA patternId)
                => patternId switch
                {
                    UiaCore.UIA.GridItemPatternId => true,
                    UiaCore.UIA.TableItemPatternId => true,
                    _ => base.IsPatternSupported(patternId)
                };

            public override string Name => _initName;

            public override AccessibleObject Parent => _monthCalendarAccessibleObject;

            public override AccessibleRole Role => AccessibleRole.Client;

            internal override int Row => _calendarIndex / _monthCalendarAccessibleObject.ColumnCount;

            public override AccessibleStates State
            {
                get
                {
                    if (!IsEnabled)
                    {
                        return AccessibleStates.None;
                    }

                    AccessibleStates state = AccessibleStates.Focusable | AccessibleStates.Selectable;

                    if (HasKeyboardFocus)
                    {
                        state |= AccessibleStates.Focused | AccessibleStates.Selected;
                    }

                    return state;
                }
            }
        }
    }
}
