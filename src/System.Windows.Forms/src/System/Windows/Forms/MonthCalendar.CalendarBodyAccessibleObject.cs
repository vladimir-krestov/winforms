// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
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
        internal class CalendarBodyAccessibleObject : MonthCalendarChildAccessibleObject
        {
            // A calendar body is the second in the calendar accessibility tree.
            // Indices start at 1.
            private const int ChildId = 2;

            private readonly CalendarAccessibleObject _calendarAccessibleObject;
            private readonly MonthCalendarAccessibleObject _monthCalendarAccessibleObject;
            private readonly int _calendarIndex;
            private readonly string _initName;
            private readonly int[] _initRuntimeId;
            private LinkedList<CalendarRowAccessibleObject>? _rowsAccessibleObjects;

            public CalendarBodyAccessibleObject(CalendarAccessibleObject calendarAccessibleObject,
                MonthCalendarAccessibleObject monthCalendarAccessibleObject, int calendarIndex)
                : base(monthCalendarAccessibleObject)
            {
                _calendarAccessibleObject = calendarAccessibleObject;
                _monthCalendarAccessibleObject = monthCalendarAccessibleObject;
                _calendarIndex = calendarIndex;
                // Name and RuntimeId don't change if the calendar date range is not changed,
                // otherwise the calendar accessibility tree will be rebuilt.
                // So save these values one time to avoid sending messages to Windows every time
                // or recreating new structures and making extra calculations.
                _initName = _monthCalendarAccessibleObject.GetCalendarPartText(MCGIP.CALENDARHEADER, _calendarIndex);
                _initRuntimeId = new int[]
                {
                    _calendarAccessibleObject.RuntimeId[0],
                    _calendarAccessibleObject.RuntimeId[1],
                    _calendarAccessibleObject.RuntimeId[2],
                    GetChildId()
                };
            }

            public override Rectangle Bounds => _monthCalendarAccessibleObject.GetCalendarPartRectangle(MCGIP.CALENDARBODY, _calendarIndex);

            internal void ClearChildCollection()
            {
                foreach (CalendarRowAccessibleObject row in RowsAccessibleObjects)
                {
                    row.ClearChildCollection();
                }

                _rowsAccessibleObjects = null;
            }

            /// <remark>
            /// A calendar always have 7 or 4 columns depending on its view.
            /// </remark>
            internal override int ColumnCount => _monthCalendarAccessibleObject.CelendarView == MCMV.MONTH ? 7 : 4;

            internal override UiaCore.IRawElementProviderFragment? FragmentNavigate(UiaCore.NavigateDirection direction) =>
                direction switch
                {
                    UiaCore.NavigateDirection.NextSibling => null,
                    UiaCore.NavigateDirection.PreviousSibling => _calendarAccessibleObject.CalendarHeaderAccessibleObject,
                    UiaCore.NavigateDirection.FirstChild => RowsAccessibleObjects.First?.Value,
                    UiaCore.NavigateDirection.LastChild => RowsAccessibleObjects.Last?.Value,
                    _ => base.FragmentNavigate(direction),

                };

            internal override int GetChildId() => ChildId;

            internal override UiaCore.IRawElementProviderSimple[]? GetColumnHeaders()
            {
                // A calendar has column headers (days of week) only in the Month view
                if (_monthCalendarAccessibleObject.CelendarView != MCMV.MONTH)
                {
                    return null;
                }

                return RowsAccessibleObjects.First?.Value.CellsAccessibleObjects.ToArray();
            }

            internal override UiaCore.IRawElementProviderSimple? GetItem(int rowIndex, int columnIndex)
                => RowsAccessibleObjects.FirstOrDefault(r => r.Row == rowIndex)
                ?.CellsAccessibleObjects.FirstOrDefault(c => c.Row == rowIndex && c.Column == columnIndex);

            internal override object? GetPropertyValue(UiaCore.UIA propertyID)
                => propertyID switch
                {
                    UiaCore.UIA.ControlTypePropertyId => UiaCore.UIA.TableControlTypeId,
                    UiaCore.UIA.IsGridPatternAvailablePropertyId => IsPatternSupported(UiaCore.UIA.GridPatternId),
                    UiaCore.UIA.IsTablePatternAvailablePropertyId => IsPatternSupported(UiaCore.UIA.TablePatternId),
                    _ => base.GetPropertyValue(propertyID)
                };

            /// <remark>
            ///  A calendar has row headers (week numbers) if ShowWeekNumbers is true
            ///  and the calendar in the Month view only.
            /// </remark>
            internal override UiaCore.IRawElementProviderSimple[]? GetRowHeaders()
                => _monthCalendarAccessibleObject.ShowWeekNumbers
                && _monthCalendarAccessibleObject.CelendarView == MCMV.MONTH
                ? GetWeekNumbersCells()
                : null;

            private CalendarCellAccessibleObject[]? GetWeekNumbersCells()
            {
                List<CalendarCellAccessibleObject> cells = new();

                for (int i = 0; i < RowCount; i++)
                {
                    CalendarCellAccessibleObject? cell = RowsAccessibleObjects.First?.Value.CellsAccessibleObjects.First?.Value;

                    if (cell is null)
                    {
                        Debug.Fail("The cell must not be null.");
                        return null;
                    }
                }

                return cells.ToArray();
            }

            internal override bool IsPatternSupported(UiaCore.UIA patternId)
                => patternId switch
                {
                    UiaCore.UIA.GridPatternId => true,
                    UiaCore.UIA.TablePatternId => true,
                    _ => base.IsPatternSupported(patternId)
                };

            public override string Name => _initName;

            public override AccessibleObject Parent => _calendarAccessibleObject;

            public override AccessibleRole Role => AccessibleRole.Table;

            internal override int RowCount => RowsAccessibleObjects.Count;

            internal override UiaCore.RowOrColumnMajor RowOrColumnMajor => UiaCore.RowOrColumnMajor.RowMajor;

            // Used LinkedList here instead List because we don't need access to this collection by indices.
            // We always need the first or the last items, or all items (in foreach or LINQ).
            // Adding new items is only from the end of the collection, so it is fast.
            // Also, it is clearly and concisely when getting an item siblings.
            // Next or Previous values returns a real item or null that is the best suited for the task.
            // The main reason of using LinkedList is that to get an item siblings we have to have one more variable
            // that stores a real index of this item in the collection, because _rowIndex doesn't reflect that.
            // Or we would have to get the current index of an item using IndexOf method every time.
            internal LinkedList<CalendarRowAccessibleObject> RowsAccessibleObjects
            {
                get
                {
                    if (_rowsAccessibleObjects is null)
                    {
                        _rowsAccessibleObjects = new();

                        // Day of week cells have "-1" row index
                        int start = _monthCalendarAccessibleObject.CelendarView == MCMV.MONTH ? -1 : 0;
                        // A calendar body always has 6 or 3 columns depending on its view
                        int end = _monthCalendarAccessibleObject.CelendarView == MCMV.MONTH ? 6 : 3;

                        for (int i = start; i < end; i++)
                        {
                            // Don't add a row if it doesn't have cells
                            CalendarRowAccessibleObject row = new(this, _monthCalendarAccessibleObject, _calendarIndex, i);
                            if (row.CellsAccessibleObjects.Count > 0)
                            {
                                _rowsAccessibleObjects.AddLast(row);
                            }
                        }
                    }

                    return _rowsAccessibleObjects;
                }
            }

            internal override int[] RuntimeId => _initRuntimeId;

            public override AccessibleStates State => AccessibleStates.Default;
        }
    }
}
