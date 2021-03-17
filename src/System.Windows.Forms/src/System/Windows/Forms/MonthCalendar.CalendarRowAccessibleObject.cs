// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using static Interop;
using static Interop.ComCtl32;

namespace System.Windows.Forms
{
    public partial class MonthCalendar
    {
        internal class CalendarRowAccessibleObject : MonthCalendarChildAccessibleObject
        {
            // This const is used to get ChildId.
            // It should take into account previous rows in a calendar body.
            // Indices start at 1.
            private const int ChildIdIncrement = 1;

            private readonly CalendarBodyAccessibleObject _calendarBodyAccessibleObject;
            private readonly MonthCalendarAccessibleObject _monthCalendarAccessibleObject;
            private readonly int _calendarIndex;
            private readonly int _rowIndex;
            private readonly int[] _initRuntimeId;
            private LinkedList<CalendarCellAccessibleObject>? _cellsAccessibleObjects;

            public CalendarRowAccessibleObject(CalendarBodyAccessibleObject calendarBodyAccessibleObject,
                MonthCalendarAccessibleObject monthCalendarAccessibleObject, int calendarIndex, int rowIndex)
                : base(monthCalendarAccessibleObject)
            {
                _calendarBodyAccessibleObject = calendarBodyAccessibleObject;
                _monthCalendarAccessibleObject = monthCalendarAccessibleObject;
                _calendarIndex = calendarIndex;
                _rowIndex = rowIndex;
                // RuntimeId doesn't change if the calendar date range is not changed,
                // otherwise the calendar accessibility tree will be rebuilt.
                // So save this value one time to avoid recreating new structures
                // and making extra calculations every time.
                _initRuntimeId = new int[]
                {
                    _calendarBodyAccessibleObject.RuntimeId[0],
                    _calendarBodyAccessibleObject.RuntimeId[1],
                    _calendarBodyAccessibleObject.RuntimeId[2],
                    _calendarBodyAccessibleObject.RuntimeId[3],
                    GetChildId()
                };
            }

            public override Rectangle Bounds
                => _monthCalendarAccessibleObject.GetCalendarPartRectangle(MCGIP.CALENDARROW, _calendarIndex, _rowIndex);

            // Used LinkedList here instead List because we don't need access to this collection by indices.
            // We always need the first or the last items, or all items (in foreach or LINQ).
            // Adding new items is only from the beginning or end of the collection, so it is fast.
            // Also, it is clearly and concisely when getting item siblings.
            // Next or Previous values returns a real item or null that is the best suited for the task.
            // The main reason of using LinkedList is that to get an item siblings we have to have one more variable
            // that stores a real index of this item in the collection, because _columnIndex doesn't reflect that.
            // Or we would have to get the current index of an item using IndexOf method every time.
            internal LinkedList<CalendarCellAccessibleObject> CellsAccessibleObjects
            {
                get
                {
                    if (_cellsAccessibleObjects is null)
                    {
                        _cellsAccessibleObjects = new();

                        // Week number cells have "-1" column index
                        int start = _monthCalendarAccessibleObject.ShowWeekNumbers ? -1 : 0;
                        // A calendar body always has 7 or 4 columns depending on its view
                        int end = _monthCalendarAccessibleObject.CelendarView == MCMV.MONTH ? 7 : 4;

                        for (int i = start; i < end; i++)
                        {
                            string name = _monthCalendarAccessibleObject.GetCalendarPartText(MCGIP.CALENDARCELL, _calendarIndex, _rowIndex, i);
                            if (!string.IsNullOrEmpty(name))
                            {
                                CalendarCellAccessibleObject cell = new(this, _calendarBodyAccessibleObject, _monthCalendarAccessibleObject, _calendarIndex, _rowIndex, i, name);
                                _cellsAccessibleObjects.AddLast(cell);
                            }
                        }

                        // This is a cpecific case that is because Windows can't get the name
                        // of the first week number cell of the first row if it is limited (MinDate is set).
                        // It seems it is a Windows bug because this cell rectangle is get correctly.
                        if (_calendarIndex == 0 && _rowIndex >= 0
                            && _monthCalendarAccessibleObject.CelendarView == MCMV.MONTH
                            && _monthCalendarAccessibleObject.ShowWeekNumbers
                            && _cellsAccessibleObjects.First is not null
                            && _cellsAccessibleObjects.First.Value.Column != -1
                            && _cellsAccessibleObjects.First.Value.DateRange is not null)
                        {
                            string weekNumber = GetWeekNumber(_cellsAccessibleObjects.First.Value.DateRange.Start);
                            CalendarCellAccessibleObject rowHeaderCell = new(this, _calendarBodyAccessibleObject, _monthCalendarAccessibleObject, _calendarIndex, _rowIndex, -1, weekNumber);
                            _cellsAccessibleObjects.AddFirst(rowHeaderCell);
                        }
                    }

                    return _cellsAccessibleObjects;
                }
            }

            internal void ClearChildCollection() => _cellsAccessibleObjects = null;

            public override string? Description
            {
                get
                {
                    // Only day and week number cells have a description
                    if (_rowIndex == -1 || _monthCalendarAccessibleObject.CelendarView != MCMV.MONTH)
                    {
                        return null;
                    }

                    // Get the first date cell date value to calculate its week number
                    CalendarCellAccessibleObject? cell = CellsAccessibleObjects.FirstOrDefault(c => c.Column != -1);
                    if (cell is null || cell.DateRange is null)
                    {
                        return null;
                    }

                    string weekNumber = GetWeekNumber(cell.DateRange.Start);

                    return string.Format(SR.MonthCalendarWeekNumberDescription, weekNumber);
                }
            }

            internal override UiaCore.IRawElementProviderFragment? FragmentNavigate(UiaCore.NavigateDirection direction)
                => direction switch
                {
                    UiaCore.NavigateDirection.NextSibling
                        => _calendarBodyAccessibleObject.RowsAccessibleObjects.Find(this)?.Next?.Value,
                    UiaCore.NavigateDirection.PreviousSibling
                        => _calendarBodyAccessibleObject.RowsAccessibleObjects.Find(this)?.Previous?.Value,
                    UiaCore.NavigateDirection.FirstChild => CellsAccessibleObjects.First?.Value,
                    UiaCore.NavigateDirection.LastChild => CellsAccessibleObjects.Last?.Value,
                    _ => base.FragmentNavigate(direction)
                };

            internal override int GetChildId() => ChildIdIncrement + _rowIndex;

            internal override object? GetPropertyValue(UiaCore.UIA propertyID)
                => propertyID switch
                {
                    UiaCore.UIA.ControlTypePropertyId => UiaCore.UIA.PaneControlTypeId,
                    _ => base.GetPropertyValue(propertyID)
                };

            private string GetWeekNumber(DateTime date)
                => CultureInfo.CurrentCulture.Calendar
                .GetWeekOfYear(date, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule,
                _monthCalendarAccessibleObject.FirstDayOfWeek).ToString();

            public override string? Name => null; // Rows don't have names like in a native calendar

            public override AccessibleObject Parent => _calendarBodyAccessibleObject;

            public override AccessibleRole Role => AccessibleRole.Row;

            internal override int Row => _rowIndex;

            internal override int[] RuntimeId => _initRuntimeId;
        }
    }
}
