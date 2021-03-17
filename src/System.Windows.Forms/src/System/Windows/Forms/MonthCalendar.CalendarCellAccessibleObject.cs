// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using System.Globalization;
using static Interop;
using static Interop.ComCtl32;

namespace System.Windows.Forms
{
    public partial class MonthCalendar
    {
        /// <summary>
        ///  Represents the calendar cell accessible object.
        /// </summary>
        internal class CalendarCellAccessibleObject : CalendarButtonAccessibleObject
        {
            // This const is used to get ChildId.
            // It should take into account previous rows in a row.
            // Indices start at 1.
            private const int ChildIdIncrement = 1;

            private readonly CalendarRowAccessibleObject _calendarRowAccessibleObject;
            private readonly CalendarBodyAccessibleObject _calendarBodyAccessibleObject;
            private readonly MonthCalendarAccessibleObject _monthCalendarAccessibleObject;
            private readonly int _calendarIndex;
            private readonly int _rowIndex;
            private readonly int _columnIndex;
            private readonly string _initName;
            private readonly int[] _initRuntimeId;
            private SelectionRange? _dateRange;

            public CalendarCellAccessibleObject(CalendarRowAccessibleObject calendarRowAccessibleObject,
                CalendarBodyAccessibleObject calendarBodyAccessibleObject,
                MonthCalendarAccessibleObject monthCalendarAccessibleObject,
                int calendarIndex, int rowIndex, int columnIndex, string initName)
                : base(monthCalendarAccessibleObject)
            {
                _calendarRowAccessibleObject = calendarRowAccessibleObject;
                _calendarBodyAccessibleObject = calendarBodyAccessibleObject;
                _monthCalendarAccessibleObject = monthCalendarAccessibleObject;
                _calendarIndex = calendarIndex;
                _rowIndex = rowIndex;
                _columnIndex = columnIndex;
                // Name and RuntimeId don't change if the calendar date range is not changed,
                // otherwise the calendar accessibility tree will be rebuilt.
                // So save these values one time to avoid sending messages to Windows every time
                // or recreating new structures and making extra calculations.
                _initName = initName;
                _initRuntimeId = new int[]
                {
                    _calendarRowAccessibleObject.RuntimeId[0],
                    _calendarRowAccessibleObject.RuntimeId[1],
                    _calendarRowAccessibleObject.RuntimeId[2],
                    _calendarRowAccessibleObject.RuntimeId[3],
                    _calendarRowAccessibleObject.RuntimeId[4],
                    GetChildId()
                };
            }

            public override Rectangle Bounds
                => _monthCalendarAccessibleObject
                .GetCalendarPartRectangle(MCGIP.CALENDARCELL, _calendarIndex, _rowIndex, _columnIndex);

            internal int CalendarIndex => _calendarIndex;

            internal override int Column => _columnIndex;

            internal override UiaCore.IRawElementProviderSimple ContainingGrid => _calendarBodyAccessibleObject;

            internal SelectionRange? DateRange
            {
                get
                {
                    // Calendar headers (week numbers and days of week) have -1 index and they don't have a date range
                    if (_rowIndex == -1 || _columnIndex == -1)
                    {
                        return null;
                    }

                    if (_dateRange is null)
                    {
                        _dateRange = _monthCalendarAccessibleObject
                            .GetCalendarPartDateRange(MCGIP.CALENDARCELL, _calendarIndex, _rowIndex, _columnIndex);
                    }

                    return _dateRange;
                }
            }

            public override string? Description
            {
                get
                {
                    // Only date cells in the Month view have Descriptions that based on cells date ranges
                    if (_rowIndex == -1 || _columnIndex == -1
                        || _monthCalendarAccessibleObject.CelendarView != MCMV.MONTH || DateRange is null)
                    {
                        return null;
                    }

                    DateTime cellDate = DateRange.Start;
                    CultureInfo culture = CultureInfo.CurrentCulture;
                    int weekNumber = culture.Calendar.GetWeekOfYear(cellDate,
                        culture.DateTimeFormat.CalendarWeekRule, _monthCalendarAccessibleObject.FirstDayOfWeek);

                    // Used string.Format here to get the correct value from resources
                    // that should be cosistent with the rest resources values
                    return string.Format(SR.MonthCalendarWeekNumberDescription, weekNumber)
                        + $", {cellDate.ToString("dddd", culture)}";
                }
            }

            internal override UiaCore.IRawElementProviderFragment? FragmentNavigate(UiaCore.NavigateDirection direction) =>
                direction switch
                {
                    UiaCore.NavigateDirection.NextSibling => _calendarRowAccessibleObject.CellsAccessibleObjects.Find(this)?.Next?.Value,
                    UiaCore.NavigateDirection.PreviousSibling => _calendarRowAccessibleObject.CellsAccessibleObjects.Find(this)?.Previous?.Value,
                    _ => base.FragmentNavigate(direction)
                };

            internal override int GetChildId() => ChildIdIncrement + _columnIndex;

            internal override object? GetPropertyValue(UiaCore.UIA propertyID)
                => propertyID switch
                {
                    UiaCore.UIA.ControlTypePropertyId
                        => (_rowIndex == -1 || _columnIndex == -1)
                        ? UiaCore.UIA.HeaderControlTypeId
                        : UiaCore.UIA.DataItemControlTypeId,
                    UiaCore.UIA.IsKeyboardFocusablePropertyId
                        => IsEnabled && _rowIndex != -1 && _columnIndex != -1,
                    UiaCore.UIA.IsGridItemPatternAvailablePropertyId
                        => IsPatternSupported(UiaCore.UIA.GridItemPatternId),
                    UiaCore.UIA.IsTableItemPatternAvailablePropertyId
                        => IsPatternSupported(UiaCore.UIA.TableItemPatternId),
                    _ => base.GetPropertyValue(propertyID)
                };

            private protected override bool HasKeyboardFocus => _monthCalendarAccessibleObject.FocusedCell == this;

            internal override void Invoke()
            {
                if (DateRange is null)
                {
                    return;
                }

                _monthCalendarAccessibleObject.SetSelectionRange(DateRange.Start, DateRange.End);
            }

            internal override bool IsPatternSupported(UiaCore.UIA patternId)
                => patternId switch
                {
                    // Only date cells support TableItem and GridItem patterns.
                    // Cell that have -1 index are headers of a table, not its items.
                    UiaCore.UIA.GridItemPatternId => _rowIndex != -1 && _columnIndex != -1,
                    UiaCore.UIA.TableItemPatternId => _rowIndex != -1 && _columnIndex != -1,
                    _ => base.IsPatternSupported(patternId)
                };

            public override string Name => _initName;

            public override AccessibleObject Parent => _calendarRowAccessibleObject;

            public override AccessibleRole Role
            {
                get
                {
                    if (_rowIndex == -1)
                    {
                        return AccessibleRole.ColumnHeader;
                    }

                    if (_columnIndex == -1)
                    {
                        return AccessibleRole.RowHeader;
                    }

                    return AccessibleRole.Cell;
                }
            }

            internal override int Row => _rowIndex;

            internal override int[] RuntimeId => _initRuntimeId;

            internal override void SetFocus()
            {
                if (_rowIndex == -1 || _columnIndex == -1)
                {
                    // Only date cells can be focused
                    return;
                }

                Invoke();
                // Get the focused cell accessible object and try to raise the focus event for it
                RaiseAutomationEvent(UiaCore.UIA.AutomationFocusChangedEventId);
            }

            public override AccessibleStates State
            {
                get
                {
                    if (_rowIndex == -1 || _columnIndex == -1)
                    {
                        // Headers cells can't be focusable or selectable
                        return AccessibleStates.None;
                    }

                    AccessibleStates state = AccessibleStates.Focusable | AccessibleStates.Selectable;

                    if (_monthCalendarAccessibleObject.Focused && _monthCalendarAccessibleObject.FocusedCell == this)
                    {
                        return state | AccessibleStates.Focused | AccessibleStates.Selected;
                    }

                    // This condition works correctly in Month view only because the cell range is bigger
                    // then the calendar selection range in the rest views.
                    // But in the rest views a user can select only one cell. It means that a focused cell equals one selected cell,
                    // so the correct state will be returned in the codition above for the rest views.
                    if (DateRange is not null && _monthCalendarAccessibleObject.CelendarView == MCMV.MONTH
                        && DateRange.Start >= _monthCalendarAccessibleObject.SelectionRange.Start
                        && DateRange.End <= _monthCalendarAccessibleObject.SelectionRange.End)
                    {
                        state |= AccessibleStates.Selected;
                    }

                    return state;
                }
            }
        }
    }
}
