# MonthCalendarAccessibleObject expected properties and behavior

This document describes the expected behavior of MonthCalendarAccessibleObject 
when using Inspect, Narrator or AccessibilityInsights and 
expected accessibility properties of all visible items 
of a [MonthCalendar](https://docs.microsoft.com/dotnet/api/system.windows.forms.monthcalendar).


- [Overview](#Overview)
- [Inspect](#Inspect)
    - [Accessibility tree](##Accessibility-tree)
    - [Accessibility properties](##Accessibility-properties)
    - [Accessibility actions](##Accessibility-actions)
    - [ElementProviderFromPoint (get an item when hovering by mouse) behavior](##ElementProviderFromPoint-(get-an-item-when-hovering-by-mouse)-behavior)   
- [Narrator](#Narrator)
- [AccessibilityInsights](#AccessibilityInsights)
- [Behavior test cases](#Behavior-test-cases)
- [Additional dev points that don't affect users](#Additional-dev-points-that-don't-affect-users)
    

# Overview

[MonthCalendar](https://docs.microsoft.com/dotnet/api/system.windows.forms.monthcalendar) is a native controls is provided by Windows. 
After implementation [UIA](https://docs.microsoft.com/dotnet/framework/ui-automation/ui-automation-overview) support on the Winforms side the responsibility for 
MonthCalendar accessible objects is now on Winforms. A MonthCalendar 
accessible object and its children must have correct accessibility 
propeties and actions to make MonthCalendar accessible for customers. 
Testing tools and expected properties and actions in there are described below.
All points have checkboxes, a checked point means that it is supported in the current implementation. 
If a point is unchecked, it needs to be fixed.


# Inspect

## Accessibility tree

MonthCalendar accessibility tree should contain all visible items depending on its view.

<details>
<summary>1. Month view</summary>

![monthcalendar-inspect-month-view-tree][monthcalendar-inspect-month-view-tree]

</details>
</br>

<details>
<summary>2. Year view</summary>

![monthcalendar-inspect-year-view-tree][monthcalendar-inspect-year-view-tree]

</details>
</br>

<details>
<summary>3. Decade view</summary>

![monthcalendar-inspect-decade-view-tree][monthcalendar-inspect-decade-view-tree]

</details>
</br>

<details>
<summary>4. Century view</summary>

![monthcalendar-inspect-century-view-tree][monthcalendar-inspect-century-view-tree]

</details>
</br>

## Accessibility properties

Here are described main accessibility properties of all part of the MonthCalendar accessibility tree depending on its view.

<details>
<summary>1. Month view</summary>
</br>

MonthCalendar:
- [ ] ControlType = "calendar" always
- [ ] IsEnabled = true if the control is enabled
- [ ] Has keyboard focus = true if the control is in focus
- [ ] IsKeyboardFocusable = "true" if the calendar is enabled
- [ ] HelpText = "MonthCaledar(Control)"
- [ ] Correct grid Column and Row count
- [ ] Name is empty if it is not set
- [ ] Role = "table"
- [ ] Value = selected dates (eg. "Saturday, April 10, 2021 - Wednesday, April 14, 2021")
- [ ] Column and row headers = null
- [ ] State = "focusable" + "focused" if the control is in focus
- [ ] Supports Grid, LegacyIAccessible, Table, Value patterns
- [ ] Supports the patterns actions

Previous/Next buttons:
- [ ] Name = "Previous" or "Next"
- [ ] ControlType = "button"
- [ ] IsKeyboardFocusable = "false"
- [ ] IsEnabled = true if the control is enabled
- [ ] Has keyboard focus = false
- [ ] Has default action and description
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Today button:
- [ ] Name = button text (eg. Today 3/20/2021)
- [ ] ControlType = "button"
- [ ] IsKeyboardFocusable = "false"
- [ ] Has keyboard focus = false
- [ ] IsEnabled = true if the control is enabled
- [ ] Has default action and description
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Calendar:
- [ ] IsEnabled = true if the control is enabled
- [ ] Doesn't have a control type like as the native control
- [ ] HasKeyboardFocus is true if the calendar contains the focused cell
- [ ] IsKeyboardFocusable = "true" if the calendar is enabled
- [ ] Has correct GridItem properties
- [ ] Role == "client"
- [ ] State = "focusable, selectable" + has "focused" if the calendar contains the focused cell
- [ ] Doesn't have TableItems columns and rows
- [ ] Supports GridItem, LegacyIAccessible, TableItem patterns

Calendar header button:
- [ ] Name = the button text (eg. March 2021)
- [ ] Has keyboard focus = false
- [ ] IsKeyboardFocusable = false
- [ ] IsEnabled = true if the control is enabled
- [ ] Has default action
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Calendar body:
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "table"
- [ ] Correct grid Column and Row count
- [ ] Role = "table"
- [ ] State = "default"
- [ ] Correct Table row and column headers
- [ ] Supports Grid, LegacyIAccessible, Table patterns

Calendar row:
- [ ] IsEnabled = true if the control is enabled
- [ ] IsKeyboardFocusable = false
- [ ] ControlType = "pane"
- [ ] Role = "row"
- [ ] State = "normal"
- [ ] Description = "Week {number}" for date rows. Description is empty for the header row
- [ ] Don't have a name
- [ ] Supports LegacyIAccessible pattern

Cell of the header row (day of week):
- [ ] Name = the cell text (e.g. "Mon" or "Fri")
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "header"
- [ ] HasKeyboardFocus always false
- [ ] IsKeyboardFocusable = false
- [ ] Correct GridItem pattern properties
- [ ] Role = "column header"
- [ ] State = "normal"
- [ ] Doesn't have a Description
- [ ] Supports LegacyIAccessible pattern

The first cell of date rows (week numbers):
- [ ] Name = the cell text (e.g. "12" or "36" - a week number)
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "header"
- [ ] HasKeyboardFocus always false
- [ ] IsKeyboardFocusable = false
- [ ] Correct GridItem pattern properties
- [ ] Role = "row header"
- [ ] State = "normal"
- [ ] Doesn't have a Description
- [ ] Supports LegacyIAccessible pattern

Date cell:
- [ ] Name = the cell text (e.g. "12" or "31" - a day number)
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "DataItem" ("item" in the accessibility tree)
- [ ] HasKeyboardFocus  = "true" if the cell is focused
- [ ] IsKeyboardFocusable = "true" if the control is enabled
- [ ] Correct GridItem pattern properties
- [ ] Description = "Week {number}, {day of week}" (eg. "Week 10, Friday")
- [ ] Role = "cell"
- [ ] State = "focusable, selectable" if the control is enabled. (the order of the states doesn't matter)
              "selected, focusable, selectable" if the cell is selected
              "focused, selected, focusable, selectable" if the cell is selected and focused
	          Important point: if a user select several cells, all of them should have "selected" state, and only one of them should have "focused" state.
- [ ] Correct TableItem column and row headers items
- [ ] Supports GridItem, LegacyIAccessible, TableItem patterns

</details>
</br>

<details>
<summary>2. Year view</summary>
</br>

MonthCalendar:
- [ ] ControlType = "calendar" always
- [ ] IsEnabled = true if the control is enabled
- [ ] Has keyboard focus = true if the control is in focus
- [ ] IsKeyboardFocusable = "true" if the calendar is enabled
- [ ] HelpText = "MonthCaledar(Control)"
- [ ] Correct grid Column and Row count
- [ ] Name is empty if it is not set
- [ ] Role = "table"
- [ ] Value = a selected month (eg. "September 2022")
- [ ] Column and row headers = null
- [ ] State = "focusable" + "focused" if the control is in focus
- [ ] Supports Grid, LegacyIAccessible, Table, Value patterns
- [ ] Supports the patterns actions

Previous/Next buttons:
- [ ] Name = "Previous" or "Next"
- [ ] ControlType = "button"
- [ ] IsKeyboardFocusable = "false"
- [ ] IsEnabled = true if the control is enabled
- [ ] Has keyboard focus = false
- [ ] Has default action and description
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Today button:
- [ ] Name = button text (eg. Today 3/20/2021)
- [ ] ControlType = "button"
- [ ] IsKeyboardFocusable = "false"
- [ ] Has keyboard focus = false
- [ ] IsEnabled = true if the control is enabled
- [ ] Has default action and description
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Calendar:
- [ ] IsEnabled = true if the control is enabled
- [ ] Doesn't have a control type like as the native control
- [ ] HasKeyboardFocus is true if the calendar contains the focused cell
- [ ] IsKeyboardFocusable = "true" if the calendar is enabled
- [ ] Has correct GridItem properties
- [ ] Role == "client"
- [ ] State = "focusable, selectable" + has "focused" if the calendar contains the focused cell
- [ ] Doesn't have TableItems columns and rows
- [ ] Supports GridItem, LegacyIAccessible, TableItem patterns

Calendar header button:
- [ ] Name = the button text (eg. "2021")
- [ ] Has keyboard focus = false
- [ ] IsKeyboardFocusable = false
- [ ] IsEnabled = true if the control is enabled
- [ ] Has default action
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Calendar body:
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "table"
- [ ] Correct grid Column and Row count
- [ ] Role = "table"
- [ ] State = "default"
- [ ] Correct Table row and column headers
- [ ] Supports Grid, LegacyIAccessible, Table patterns

Calendar row:
- [ ] IsEnabled = true if the control is enabled
- [ ] IsKeyboardFocusable = false
- [ ] ControlType = "pane"
- [ ] Role = "row"
- [ ] State = "normal"
- [ ] Description is empty
- [ ] Don't have a name
- [ ] Supports LegacyIAccessible pattern

Month cell:
- [ ] Name = the cell text (e.g. "May")
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "DataItem" ("item" in the accessibility tree)
- [ ] HasKeyboardFocus  = "true" if the cell is focused
- [ ] IsKeyboardFocusable = "true" if the control is enabled
- [ ] Correct GridItem pattern properties
- [ ] Description is empty
- [ ] Role = "cell"
- [ ] State = "focusable, selectable" if the control is enabled. (the order of the states doesn't matter)
              "focused, selected, focusable, selectable" if the cell is selected and focused
	          Important point: if a user can't select several cells in this view, so only one cell should have "selected" state, and this cell should have "focused" state.
- [ ] Doesn't have TableItem column and row headers items 
- [ ] Supports GridItem, LegacyIAccessible, TableItem patterns

</details>
</br>

<details>
<summary>3. Decade view</summary>
</br>

MonthCalendar:
- [ ] ControlType = "calendar" always
- [ ] IsEnabled = true if the control is enabled
- [ ] Has keyboard focus = true if the control is in focus
- [ ] IsKeyboardFocusable = "true" if the calendar is enabled
- [ ] HelpText = "MonthCaledar(Control)"
- [ ] Correct grid Column and Row count
- [ ] Name is empty if it is not set
- [ ] Role = "table"
- [ ] Value = a selected year (eg. "2020")
- [ ] Column and row headers = null
- [ ] State = "focusable" + "focused" if the control is in focus
- [ ] Supports Grid, LegacyIAccessible, Table, Value patterns
- [ ] Supports the patterns actions

Previous/Next buttons:
- [ ] Name = "Previous" or "Next"
- [ ] ControlType = "button"
- [ ] IsKeyboardFocusable = "false"
- [ ] IsEnabled = true if the control is enabled
- [ ] Has keyboard focus = false
- [ ] Has default action and description
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Today button:
- [ ] Name = button text (eg. Today 3/20/2021)
- [ ] ControlType = "button"
- [ ] IsKeyboardFocusable = "false"
- [ ] Has keyboard focus = false
- [ ] IsEnabled = true if the control is enabled
- [ ] Has default action and description
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Calendar:
- [ ] IsEnabled = true if the control is enabled
- [ ] Doesn't have a control type like as the native control
- [ ] HasKeyboardFocus is true if the calendar contains the focused cell
- [ ] IsKeyboardFocusable = "true" if the calendar is enabled
- [ ] Has correct GridItem properties
- [ ] Role == "client"
- [ ] State = "focusable, selectable" + has "focused" if the calendar contains the focused cell
- [ ] Doesn't have TableItems columns and rows
- [ ] Supports GridItem, LegacyIAccessible, TableItem patterns

Calendar header button:
- [ ] Name = the button text (eg. "2020-2029")
- [ ] Has keyboard focus = false
- [ ] IsKeyboardFocusable = false
- [ ] IsEnabled = true if the control is enabled
- [ ] Has default action
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Calendar body:
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "table"
- [ ] Correct grid Column and Row count
- [ ] Role = "table"
- [ ] State = "default"
- [ ] Correct Table row and column headers
- [ ] Supports Grid, LegacyIAccessible, Table patterns

Calendar row:
- [ ] IsEnabled = true if the control is enabled
- [ ] IsKeyboardFocusable = false
- [ ] ControlType = "pane"
- [ ] Role = "row"
- [ ] State = "normal"
- [ ] Description is empty
- [ ] Don't have a name
- [ ] Supports LegacyIAccessible pattern

Year cell:
- [ ] Name = the cell text (e.g. "2020")
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "DataItem" ("item" in the accessibility tree)
- [ ] HasKeyboardFocus  = "true" if the cell is focused
- [ ] IsKeyboardFocusable = "true" if the control is enabled
- [ ] Correct GridItem pattern properties
- [ ] Description is empty
- [ ] Role = "cell"
- [ ] State = "focusable, selectable" if the control is enabled. (the order of the states doesn't matter)
              "focused, selected, focusable, selectable" if the cell is selected and focused
	          Important point: if a user can't select several cells in this view, so only one cell should have "selected" state, and this cell should have "focused" state.
- [ ] Doesn't have TableItem column and row headers items 
- [ ] Supports GridItem, LegacyIAccessible, TableItem patterns

</details>
</br>

<details>
<summary>4. Century view</summary>
</br>

MonthCalendars:
- [ ] ControlType = "calendar" always
- [ ] IsEnabled = true if the control is enabled
- [ ] Has keyboard focus = true if the control is in focus
- [ ] IsKeyboardFocusable = "true" if the calendar is enabled
- [ ] HelpText = "MonthCaledar(Control)"
- [ ] Correct grid Column and Row count
- [ ] Name is empty if it is not set
- [ ] Role = "table"
- [ ] Value = a selected dacade (eg. "2020-2029")
- [ ] Column and row headers = null
- [ ] State = "focusable" + "focused" if the control is in focus
- [ ] Supports Grid, LegacyIAccessible, Table, Value patterns
- [ ] Supports the patterns actions

Previous/Next buttons:
- [ ] Name = "Previous" or "Next"
- [ ] ControlType = "button"
- [ ] IsKeyboardFocusable = "false"
- [ ] IsEnabled = true if the control is enabled
- [ ] Has keyboard focus = false
- [ ] Has default action and description
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Today button:
- [ ] Name = button text (eg. Today 3/20/2021)
- [ ] ControlType = "button"
- [ ] IsKeyboardFocusable = "false"
- [ ] Has keyboard focus = false
- [ ] IsEnabled = true if the control is enabled
- [ ] Has default action and description
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Calendar:
- [ ] IsEnabled = true if the control is enabled
- [ ] Doesn't have a control type like as the native control
- [ ] HasKeyboardFocus is true if the calendar contains the focused cell
- [ ] IsKeyboardFocusable = "true" if the calendar is enabled
- [ ] Has correct GridItem properties
- [ ] Role == "client"
- [ ] State = "focusable, selectable" + has "focused" if the calendar contains the focused cell
- [ ] Doesn't have TableItems columns and rows
- [ ] Supports GridItem, LegacyIAccessible, TableItem patterns

Calendar header button:
- [ ] Name = the button text (eg. "2000-2099")
- [ ] Has keyboard focus = false
- [ ] IsKeyboardFocusable = false
- [ ] IsEnabled = true if the control is enabled
- [ ] Has default action
- [ ] Role = "push button"
- [ ] State = "normal"
- [ ] Supports Invoke and LegacyIAccessible

Calendar body:
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "table"
- [ ] Correct grid Column and Row count
- [ ] Role = "table"
- [ ] State = "default"
- [ ] Correct Table row and column headers
- [ ] Supports Grid, LegacyIAccessible, Table patterns

Calendar row:
- [ ] IsEnabled = true if the control is enabled
- [ ] IsKeyboardFocusable = false
- [ ] ControlType = "pane"
- [ ] Role = "row"
- [ ] State = "normal"
- [ ] Description is empty
- [ ] Don't have a name
- [ ] Supports LegacyIAccessible pattern

Decade cell:
- [ ] Name = the cell text (e.g. "2020-2029")
- [ ] IsEnabled = true if the control is enabled
- [ ] ControlType = "DataItem" ("item" in the accessibility tree)
- [ ] HasKeyboardFocus  = "true" if the cell is focused
- [ ] IsKeyboardFocusable = "true" if the control is enabled
- [ ] Correct GridItem pattern properties
- [ ] Description is empty
- [ ] Role = "cell"
- [ ] State = "focusable, selectable" if the control is enabled. (the order of the states doesn't matter)
          "focused, selected, focusable, selectable" if the cell is selected and focused
	  Important point: if a user can't select several cells in this view, so only one cell should have "selected" state, and this cell should have "focused" state.
- [ ] Doesn't have TableItem column and row headers items 
- [ ] Supports GridItem, LegacyIAccessible, TableItem patterns

</details>
</br>

## Accessibility actions

Here are descibed accessibility actions of supported patterns.

<details>
<summary>1. Month view</summary>
</br>

MonthCalendar:
- [ ] Focus - focus on the focused cell
- [ ] Grid.GetItem- returns OK for the correct row and column, return FAIL for incorrect arguments
- [ ] Value.SetValue - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the monthcalendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Previous/Next buttons:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Today button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the calendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar header button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (change the calendar view)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar body:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] Grid.GetItem - returns OK for the correct row and column, return FAIL for incorrect arguments
- [ ] LegacyIAccessible.Select - do nothing because the body is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar row:
- [ ] Focus - focus on the focused cell if the row contains it. And do nothing if the row doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the row is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Cell of the header row (day of week):
- [ ] Focus - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the header cell is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

The first cell of date rows (week numbers):
- [ ] Focus - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the header cell is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Date cell:
- [ ] Focus - select the cell
- [ ] LegacyIAccessible.Select - select the cell
- [ ] LegacyIAccessible.DoDefaultAction - select the cell
- [ ] LegacyIAccessible.SetValue - do nothing

</details>
</br>

<details>
<summary>2. Year view</summary>
</br>

MonthCalendar:
- [ ] Focus - focus on the focused cell
- [ ] Grid.GetItem- returns OK for the correct row and column, return FAIL for incorrect arguments
- [ ] Value.SetValue - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the monthcalendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Previous/Next buttons:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Today button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the calendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar header button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (change the calendar view)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar body:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] Grid.GetItem - returns OK for the correct row and column, return FAIL for incorrect arguments
- [ ] LegacyIAccessible.Select - do nothing because the body is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar row:
- [ ] Focus - focus on the focused cell if the row contains it. And do nothing if the row doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the row is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Month cell:
- [ ] Focus - select the cell
- [ ] LegacyIAccessible.Select - select the cell
- [ ] LegacyIAccessible.DoDefaultAction - select the cell
- [ ] LegacyIAccessible.SetValue - do nothing

</details>
</br>

<details>
<summary>3. Decade view</summary>
</br>

MonthCalendar:
- [ ] Focus - focus on the focused cell
- [ ] Grid.GetItem- returns OK for the correct row and column, return FAIL for incorrect arguments
- [ ] Value.SetValue - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the monthcalendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Previous/Next buttons:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Today button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the calendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar header button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (change the calendar view)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar body:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] Grid.GetItem - returns OK for the correct row and column, return FAIL for incorrect arguments
- [ ] LegacyIAccessible.Select - do nothing because the body is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar row:
- [ ] Focus - focus on the focused cell if the row contains it. And do nothing if the row doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the row is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Year cell:
- [ ] Focus - select the cell
- [ ] LegacyIAccessible.Select - select the cell
- [ ] LegacyIAccessible.DoDefaultAction - select the cell
- [ ] LegacyIAccessible.SetValue - do nothing

</details>
</br>

<details>
<summary>4. Century view</summary>
</br>

MonthCalendar:
- [ ] Focus - focus on the focused cell
- [ ] Grid.GetItem- returns OK for the correct row and column, return FAIL for incorrect arguments
- [ ] Value.SetValue - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the monthcalendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Previous/Next buttons:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Today button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the calendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar header button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (change the calendar view)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar body:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] Grid.GetItem - returns OK for the correct row and column, return FAIL for incorrect arguments
- [ ] LegacyIAccessible.Select - do nothing because the body is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar row:
- [ ] Focus - focus on the focused cell if the row contains it. And do nothing if the row doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the row is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Decade cell:
- [ ] Focus - select the cell
- [ ] LegacyIAccessible.Select - select the cell
- [ ] LegacyIAccessible.DoDefaultAction - select the cell
- [ ] LegacyIAccessible.SetValue - do nothing

</details>
</br>

## ElementProviderFromPoint (get an item when hovering by mouse) behavior

- [ ] Returns the MonthCalendar accessible object if mouse doesn't hover any element
- [ ] Returns "Previous" button if mouse on it
- [ ] Returns "Next" button if mouse on it
- [ ] Returns "Today" button if mouse on the text or in left of the text until the MonthCalendar border
- [ ] Returns a header button of a calendar (eg. "March 2021") if mouse on this button
- [ ] Returns a day of week cell (eg. "Sat") if the MonthCalendar in the Month view and mouse on this cell
- [ ] Returns a week number cell (eg. "18") if ShowWeekNumber is true, the MonthCalendar is in the Month view and mouse on this cell
- [ ] Returns a day/month/year/years cell if this cell is visible and mouse on it
- [ ] Returns a gray cell of the next/previous calendar if it is and if mouse on it <details><summary>Screenshot</summary>![monthcalendar-gray-dates-accessible-from-point][monthcalendar-gray-dates-accessible-from-point]</details>
- [ ] Returns a calendar if its Bounds contains mouse coordinates and if in this point there are no any cell <details><summary>Screenshot</summary>![monthcalendar-calendar-accessible-from-point][monthcalendar-calendar-accessible-from-point]</details>
- [ ] Returns the MonthCalendar if MaxDate and MinDate are set and there are no several months (they are invisible) and if mouse on some invisible calendar <details><summary>Screenshot</summary>![monthcalendar-control-accessible-from-point][monthcalendar-control-accessible-from-point]</details>
- [ ] Returns the first week number of a calendar if its first row is partial (a specific case - there is a workaround that fixes a bug of Win API) <details><summary>Screenshot</summary>![monthcalendar-first-weeknumber-accessible-from-point][monthcalendar-first-weeknumber-accessible-from-point]</details>
- [ ] Doesn't return week number cells of the last calendar if MaxDate is set and mouse on them <details><summary>Screenshot</summary>![monthcalendar-last-weeknumbers-accessible-from-point][monthcalendar-last-weeknumbers-accessible-from-point]</details>
- [ ] Returns a correct object that is visible if MaxDate, MinDate, FirstDayOfWeek are changed or display range is changed via "Next"/"Previous" buttons

# Narrator

<details>
<summary>1. Month view</summary>
</br>

- [ ] Announces dates when moving through them
- [ ] Moves through all the accessibility tree nodes in the "Scan" mode
- [ ] Focus on focused sel when the control gets focus
- [ ] Focus on the focused cell if MaxDate, MinDate, FirstDayOfWeek are changed or display range is changed via "Next"/"Previous" buttons

</details>
</br>

<details>
<summary>2. Year view</summary>
</br>

- [ ] Announces dates when moving through them
- [ ] Moves through all the accessibility tree nodes in the "Scan" mode
- [ ] Focus on focused sel when the control gets focus
- [ ] Focus on the focused cell if MaxDate, MinDate, FirstDayOfWeek are changed or display range is changed via "Next"/"Previous" buttons

</details>
</br>

<details>
<summary>3. Decade view</summary>
</br>

- [ ] Announces dates when moving through them
- [ ] Moves through all the accessibility tree nodes in the "Scan" mode
- [ ] Focus on focused sel when the control gets focus
- [ ] Focus on the focused cell if MaxDate, MinDate, FirstDayOfWeek are changed or display range is changed via "Next"/"Previous" buttons

</details>
</br>

<details>
<summary>4. Century view</summary>
</br>

- [ ] Announces dates when moving through them
- [ ] Moves through all the accessibility tree nodes in the "Scan" mode
- [ ] Focus on the focused cell when the control gets focus
- [ ] Focus on the focused cell if MaxDate, MinDate, FirstDayOfWeek are changed or display range is changed via "Next"/"Previous" buttons

</details>
</br>

# AccessibilityInsights

<details>
<summary>1. Month view</summary>
</br>

- [ ] There are no any AI errors.
- [ ] The accessibility tree is correct.
- [ ] AI gets a correct visible accessible object when hovering the mouse (element from point).
- [ ] AI sees correct items patterns and do supported pattern Actions correctly:

MonthCalendar:
- [ ] Focus - focus on the focused cell
- [ ] Grid.GetItem- returns a calendar for the correct row and column, return an error for incorrect arguments
- [ ] Value.SetValue - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the monthcalendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Previous/Next buttons:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Today button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the calendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar header button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (change the calendar view)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar body:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] Grid.GetItem - returns a cell for the correct row and column, return an error for incorrect arguments
- [ ] LegacyIAccessible.Select - do nothing because the body is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar row:
- [ ] Focus - focus on the focused cell if the row contains it. And do nothing if the row doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the row is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Cell of the header row (day of week):
- [ ] Focus - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the header cell is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

The first cell of date rows (week numbers):
- [ ] Focus - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the header cell is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Date cell:
- [ ] Focus - select the cell
- [ ] LegacyIAccessible.Select - select the cell
- [ ] LegacyIAccessible.DoDefaultAction - select the cell
- [ ] LegacyIAccessible.SetValue - do nothing

</details>
</br>

<details>
<summary>2. Year view</summary>
</br>

- [ ] There are no any AI errors
- [ ] The accessibility tree is correct.
- [ ] AI gets a correct visible accessible object when hovering the mouse (element from point).
- [ ] AI sees correct items patterns and do supported pattern Actions correctly:

MonthCalendar:
- [ ] Focus - focus on the focused cell
- [ ] Grid.GetItem- returns a calendar for the correct row and column, return an error for incorrect arguments
- [ ] Value.SetValue - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the monthcalendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Previous/Next buttons:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the calendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar header button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (change the calendar view)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar body
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] Grid.GetItem - returns a cell for the correct row and column, return an error for incorrect arguments
- [ ] LegacyIAccessible.Select - do nothing because the body is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar row:
- [ ] Focus - focus on the focused cell if the row contains it. And do nothing if the row doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the row is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Month cell:
- [ ] Focus - select the cell
- [ ] LegacyIAccessible.Select - select the cell
- [ ] LegacyIAccessible.DoDefaultAction - select the cell
- [ ] LegacyIAccessible.SetValue - do nothing

</details>
</br>

<details>
<summary>3. Decade view</summary>
</br>

- [ ] There are no any AI errors
- [ ] The accessibility tree is correct.
- [ ] AI gets a correct visible accessible object when hovering the mouse (element from point).
- [ ] AI sees correct items patterns and do supported pattern Actions correctly:

MonthCalendar:
- [ ] Focus - focus on the focused cell
- [ ] Grid.GetItem- returns a calendar for the correct row and column, return an error for incorrect arguments
- [ ] Value.SetValue - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the monthcalendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Previous/Next buttons:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the calendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar header button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (change the calendar view)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar body:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] Grid.GetItem - returns a cell for the correct row and column, return an error for incorrect arguments
- [ ] LegacyIAccessible.Select - do nothing because the body is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar row:
- [ ] Focus - focus on the focused cell if the row contains it. And do nothing if the row doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the row is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Year cell:
- [ ] Focus - select the cell
- [ ] LegacyIAccessible.Select - select the cell
- [ ] LegacyIAccessible.DoDefaultAction - select the cell
- [ ] LegacyIAccessible.SetValue - do nothing

</details>
</br>

<details>
<summary>4. Century view</summary>
</br>

- [ ] There are no any AI errors
- [ ] The accessibility tree is correct.
- [ ] AI gets a correct visible accessible object when hovering the mouse (element from point).
- [ ] AI sees correct items patterns and do supported pattern Actions correctly:

MonthCalendar:
- [ ] Focus - focus on the focused cell
- [ ] Grid.GetItem- returns a calendar for the correct row and column, return an error for incorrect arguments
- [ ] Value.SetValue - do nothing
- [ ] LegacyIAccessible.Select - do nothing because the monthcalendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Previous/Next buttons:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (move to previous/next month)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the calendar is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar header button:
- [ ] Focus - the button is not keyboard focusable, so do nothing
- [ ] Invoke.Invoke - click the button (change the calendar view)
- [ ] LegacyIAccessible.Select - do nothing because the button is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - click the button
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar body:
- [ ] Focus - focus on the focused cell if the calendar contains it. And do nothing if the calendar doesn't contain the focusd cell 
- [ ] Grid.GetItem - returns a cell for the correct row and column, return an error for incorrect arguments
- [ ] LegacyIAccessible.Select - do nothing because the body is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Calendar row:
- [ ] Focus - focus on the focused cell if the row contains it. And do nothing if the row doesn't contain the focusd cell 
- [ ] LegacyIAccessible.Select - do nothing because the row is not selectable
- [ ] LegacyIAccessible.DoDefaultAction - do nothing
- [ ] LegacyIAccessible.SetValue - do nothing

Decade cell:
- [ ] Focus - select the cell
- [ ] LegacyIAccessible.Select - select the cell
- [ ] LegacyIAccessible.DoDefaultAction - select the cell
- [ ] LegacyIAccessible.SetValue - do nothing

</details>
</br>

# Behavior test cases

<details>
<summary>1. Month view</summary>
</br>

- [ ] **Case:** Change Today date (set TodayDate of a MonthCalendar)
</br>**Expected:** Nothing happens
- [ ] **Case:** Click on a gray date cell (of the next or previous calendars)
</br>**Expected:** The monthCalendar shanges the display range. It accessibility tree rebuilds.
- [ ] **Case:** Size of the control is changed that the control changes calendars count
</br>**Expected:** The accessibility tree is rebuilt. ElementProviderFromPoint returns visible items correctly
- [ ] **Case:** A calendar of a MonthCalendar has non-full rows
</br>**Expected:** Inspect sees only visible items in that row
- [ ] **Case:** A calendar of a MonthCalendar has some empty rows
</br>**Expected:** These rows are not in the accessibility tree
- [ ] **Case:** The first week number cell in the first calendar in a MonthCalendar is in a non-full row
</br>**Expected:** Inspect sees that cell correctly with the correct name
- [ ] **Case:** The last week number cells of the last non-full calendar have the same values for empty rows
</br>**Expected:** They are not in the accessibility tree
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to right, thereby the focused cell 
will be in right (eg. 15th of September). Set MinDate of the calendar less then the selected range (eg. 1st of September).
</br>**Expected:** The selected range doesn't change. The focused cell doesn't change. 
The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to left, thereby the focused cell 
will be in left (eg. 10th of September). Set MinDate of the calendar less then the selected range (eg. 1st of September).
</br>**Expected:** The selected range doesn't change. The focused cell doesn't change. 
The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to right, thereby the focused cell 
will be in right (eg. 15th of September). Set MaxDate of the calendar more then the selected range (eg. 20th of September).
</br>**Expected:** The selected range doesn't change. The focused cell doesn't change. 
The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to left, thereby the focused cell 
will be in left (eg. 10th of September). Set MaxDate of the calendar more then the selected range (eg. 20th of September).
</br>**Expected:** The selected range doesn't change. The focused cell doesn't change. 
The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to right, thereby the focused cell 
will be in right (eg. 15th of September). Set MinDate of the calendar more then the start of the selected range, 
but less then the end of the selected range (eg. 13th of September).
</br>**Expected:** The selected range changes. The focused cell doesn't change. 
The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to left, thereby the focused cell 
will be in left (eg. 10th of September). Set MinDate of the calendar more then the start of the selected range, 
but less then the end of the selected range (eg. 13th of September).
</br>**Expected:** The selected range changes. The focused cell changes (13th of September). 
The new focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to right, thereby the focused cell 
will be in right (eg. 15th of September). Set MaxDate of the calendar more then the start of the selected range, 
but less then the end of the selected range (eg. 13th of September).
</br>**Expected:** The selected range changes. The focused cell changes (13th of September). 
The new focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to left, thereby the focused cell 
will be in left (eg. 10th of September). Set MaxDate of the calendar more then the start of the selected range, 
but less then the end of the selected range (eg. 13th of September).
</br>**Expected:** The selected range changes. The focused cell cell doesn't change. 
The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to right, thereby the focused cell 
will be in right (eg. 15th of September). Set new FirstDayOfWeek (eg. Friday).
</br>**Expected:** The selected range doesn't change. The focused cell doesn't change. 
The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select some dates (eg. 10-15th of September), move to left, thereby the focused cell 
will be in left (eg. 10th of September). Set new FirstDayOfWeek (eg. Friday).
</br>**Expected:** The selected range doesn't change. The focused cell doesn't change. 
The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** MinDate is more then the selected range. 
</br>**Expected:** The focused cell changes. The new focused cell has "focused" accessibility state.
- [ ] **Case:** MaxDate is less then the selected range. 
</br>**Expected:** The focused cell changes. The new focused cell has "focused" accessibility state.
- [ ] **Case:** A MonthCalendar has 1 calendar.
</br>**Expected:** Accessibility tree has 1 calendar.
- [ ] **Case:** A MonthCalendar has several calendars. 
</br>**Expected:** Accessibility tree has the same count of calendars.
- [ ] **Case:** MinDate is set for a MonthCalendar. 
</br>**Expected:** Dates before MinDate are invisible and are not accessible.
- [ ] **Case:** MaxDate is set for a MonthCalendar. 
</br>**Expected:** Dates after MaxDate are invisible and are not accessible.
- [ ] **Case:** MaxDate and MinDate are set for a MonthCalendar. 
They are has a more date range then the display range of the MonthCalendar.
</br>**Expected:** Accessibility tree has all visible calendars. All dates are accessible.
- [ ] **Case:** MaxDate and MinDate are set for a MonthCalendar. 
They are has a less date range then the display range of the MonthCalendar. 
Thereby the MonthCalendar has several partially visible calendars 
(eg. the MonthCalendar can contain 6 calendars, but 3 of them are visible due MinDate and MinDate). 
</br>**Expected:** Accessibility tree has the count of visible calendars only (eg. 3).
Invisible calendars are not accessible. Invisible dates of partial calendars are not accessible.

</details>
</br>

<details>
<summary>2. Year view</summary>
</br>

- [ ] **Case:** Change Today date (set TodayDate of a MonthCalendar)
</br>**Expected:** Nothing happens
- [ ] **Case:** Click on a gray date cell (of the next or previous calendars)
</br>**Expected:** The monthCalendar shanges the display range. It accessibility tree rebuilds.
- [ ] **Case:** Size of the control is changed that the control changes calendars count
</br>**Expected:** The accessibility tree is rebuilt. ElementProviderFromPoint returns visible items correctly
- [ ] **Case:** A calendar of a MonthCalendar has non-full rows
</br>**Expected:** Inspect sees only visible items in that row
- [ ] **Case:** A calendar of a MonthCalendar has some empty rows
</br>**Expected:** These rows are not in the accessibility tree
- [ ] **Case:** There are no week number and day of week cells in calendars
</br>**Expected:** There are no any invisible items (week number and day of week cells) in the accessibility tree
- [ ] **Case:** Select one month cell (eg. September), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar less then the selected cell (eg. 1st of June).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. September), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar more then the selected cell (eg. 1st of December).
</br>**Expected:** The focused cell changes (eg. December). The new focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. September), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar less then the selected cell (eg. 1st of June).
</br>**Expected:** The focused cell changes (eg. June). The new focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. September), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar more then the selected cell (eg. 1st of December).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. September), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar with the same month (eg. 30th of September).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. September), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar with the same month (eg. 1st of September).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** MinDate is more then the selected range. 
</br>**Expected:** The focused cell changes. The new focused cell has "focused" accessibility state.
- [ ] **Case:** MaxDate is less then the selected range. 
</br>**Expected:** The focused cell changes. The new focused cell has "focused" accessibility state.
- [ ] **Case:** A MonthCalendar has 1 calendar.
</br>**Expected:** Accessibility tree has 1 calendar.
- [ ] **Case:** A MonthCalendar has several calendars. 
</br>**Expected:** Accessibility tree has the same count of calendars.
- [ ]  **Case:** MinDate is set for a MonthCalendar. 
</br>**Expected:** Dates before MinDate are invisible and are not accessible.
- [ ] **Case:** MaxDate is set for a MonthCalendar. 
</br>**Expected:** Dates after MaxDate are invisible and are not accessible.
- [ ] **Case:** MaxDate and MinDate are set for a MonthCalendar. 
They are has a more date range then the display range of the MonthCalendar.
</br>**Expected:** Accessibility tree has all visible calendars. All dates are accessible.
- [ ] **Case:** MaxDate and MinDate are set for a MonthCalendar. 
They are has a less date range then the display range of the MonthCalendar. 
Thereby the MonthCalendar has several partially visible calendars 
(eg. the MonthCalendar can contain 6 calendars, but 3 of them are visible due MinDate and MinDate). 
</br>**Expected:** Accessibility tree has the count of visible calendars only (eg. 3).
Invisible calendars are not accessible. Invisible dates of partial calendars are not accessible.

</details>
</br>

<details>
<summary>3. Decade view</summary>
</br>

- [ ] **Case:** Change Today date (set TodayDate of a MonthCalendar)
</br>**Expected:** Nothing happens
- [ ] **Case:** Click on a gray date cell (of the next or previous calendars)
</br>**Expected:** The monthCalendar shanges the display range. It accessibility tree rebuilds.
- [ ] **Case:** Size of the control is changed that the control changes calendars count
</br>**Expected:** The accessibility tree is rebuilt. ElementProviderFromPoint returns visible items correctly
- [ ] **Case:** A calendar of a MonthCalendar has non-full rows
</br>**Expected:** Inspect sees only visible items in that row
- [ ] **Case:** A calendar of a MonthCalendar has some empty rows
</br>**Expected:** These rows are not in the accessibility tree
- [ ] **Case:** There are no week number and day of week cells in calendars
</br>**Expected:** There are no any invisible items (week number and day of week cells) in the accessibility tree
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar less then the selected cell (eg. 1st of June 2019).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar more then the selected cell (eg. 1st of December 2021).
</br>**Expected:** The focused cell changes (eg. 2021). The new focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar less then the selected cell (eg. 1st of June 2019).
</br>**Expected:** The focused cell changes (eg. 2019). The new focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar more then the selected cell (eg. 1st of December 2021).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar with the same year (eg. 31th of December 2020).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar with the same year (eg. 1st of January 2020).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** MinDate is more then the selected range. 
</br>**Expected:** The focused cell changes. The new focused cell has "focused" accessibility state.
- [ ] **Case:** MaxDate is less then the selected range. 
</br>**Expected:** The focused cell changes. The new focused cell has "focused" accessibility state.
- [ ] **Case:** A MonthCalendar has 1 calendar.
</br>**Expected:** Accessibility tree has 1 calendar.
- [ ] **Case:** A MonthCalendar has several calendars. 
</br>**Expected:** Accessibility tree has the same count of calendars.
- [ ] **Case:** MinDate is set for a MonthCalendar. 
</br>**Expected:** Dates before MinDate are invisible and are not accessible.
- [ ] **Case:** MaxDate is set for a MonthCalendar. 
</br>**Expected:** Dates after MaxDate are invisible and are not accessible.
- [ ] **Case:** MaxDate and MinDate are set for a MonthCalendar. 
They are has a more date range then the display range of the MonthCalendar.
</br>**Expected:** Accessibility tree has all visible calendars. All dates are accessible.
- [ ] **Case:** MaxDate and MinDate are set for a MonthCalendar. 
They are has a less date range then the display range of the MonthCalendar. 
Thereby the MonthCalendar has several partially visible calendars 
(eg. the MonthCalendar can contain 6 calendars, but 3 of them are visible due MinDate and MinDate). 
</br>**Expected:** Accessibility tree has the count of visible calendars only (eg. 3).
Invisible calendars are not accessible. Invisible dates of partial calendars are not accessible.

</details>
</br>

<details>
<summary>4. Century view</summary>
</br>

- [ ] **Case:** Change Today date (set TodayDate of a MonthCalendar)
</br>**Expected:** Nothing happens
- [ ] **Case:** Click on a gray date cell (of the next or previous calendars)
</br>**Expected:** The monthCalendar shanges the display range. It accessibility tree rebuilds.
- [ ] **Case:** Size of the control is changed that the control changes calendars count
</br>**Expected:** The accessibility tree is rebuilt. ElementProviderFromPoint returns visible items correctly
- [ ] **Case:** A calendar of a MonthCalendar has non-full rows
</br>**Expected:** Inspect sees only visible items in that row
- [ ] **Case:** A calendar of a MonthCalendar has some empty rows
</br>**Expected:** These rows are not in the accessibility tree
- [ ] **Case:** There are no week number and day of week cells in calendars
</br>**Expected:** There are no any invisible items (week number and day of week cells) in the accessibility tree
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar less then the selected cell (eg. 1st of June 2019).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar more then the selected cell (eg. 1st of December 2021).
</br>**Expected:** The focused cell changes (eg. 2021). The new focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar less then the selected cell (eg. 1st of June 2019).
</br>**Expected:** The focused cell changes (eg. 2019). The new focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar more then the selected cell (eg. 1st of December 2021).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MinDate of the calendar with the same year (eg. 31th of December 2020).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** Select one month cell (eg. 2020), user can't select several cell in this view, 
so the selected cell is focused. Set MaxDate of the calendar with the same year (eg. 1st of January 2020).
</br>**Expected:** The focused cell doesn't change. The focused cell has "focused" accessibility state (check Inspect).
- [ ] **Case:** MinDate is more then the selected range. 
</br>**Expected:** The focused cell changes. The new focused cell has "focused" accessibility state.
- [ ] **Case:** MaxDate is less then the selected range. 
</br>**Expected:** The focused cell changes. The new focused cell has "focused" accessibility state.
- [ ] **Case:** A MonthCalendar has 1 calendar.
</br>**Expected:** Accessibility tree has 1 calendar.
- [ ] **Case:** A MonthCalendar has several calendars. 
</br>**Expected:** Accessibility tree has the same count of calendars.
- [ ] **Case:** MinDate is set for a MonthCalendar. 
</br>**Expected:** Dates before MinDate are invisible and are not accessible.
- [ ] **Case:** MaxDate is set for a MonthCalendar. 
</br>**Expected:** Dates after MaxDate are invisible and are not accessible.
- [ ] **Case:** MaxDate and MinDate are set for a MonthCalendar. 
They are has a more date range then the display range of the MonthCalendar.
</br>**Expected:** Accessibility tree has all visible calendars. All dates are accessible.
- [ ] **Case:** MaxDate and MinDate are set for a MonthCalendar. 
They are has a less date range then the display range of the MonthCalendar. 
Thereby the MonthCalendar has several partially visible calendars 
(eg. the MonthCalendar can contain 6 calendars, but 3 of them are visible due MinDate and MinDate). 
</br>**Expected:** Accessibility tree has the count of visible calendars only (eg. 3).
Invisible calendars are not accessible. Invisible dates of partial calendars are not accessible.

</details>
</br>

# Additional dev points that don't affect users

- Rebuild the accessibility tree if the calendar View, MaxDate, MinDate, TodayDate, Size are changed
or Next or Previous buttons is clicked (the display range is changed).
- Invoke method of button accessible objects of MonthCalednar simulates mouse moving, click and then return the mouse position back.
Windows doesn't provide API to do click MonthCalendar buttons via messages.

[monthcalendar-inspect-month-view-tree]: ../images/monthcalendar-inspect-month-view-tree.png
[monthcalendar-inspect-year-view-tree]: ../images/monthcalendar-inspect-year-view-tree.png
[monthcalendar-inspect-decade-view-tree]: ../images/monthcalendar-inspect-decade-view-tree.png
[monthcalendar-inspect-century-view-tree]: ../images/monthcalendar-inspect-century-view-tree.png
[monthcalendar-gray-dates-accessible-from-point]: ../images/monthcalendar-gray-dates-accessible-from-point.png
[monthcalendar-calendar-accessible-from-point]: ../images/monthcalendar-calendar-accessible-from-point.png
[monthcalendar-control-accessible-from-point]: ../images/monthcalendar-control-accessible-from-point.png
[monthcalendar-first-weeknumber-accessible-from-point]: ../images/monthcalendar-first-weeknumber-accessible-from-point.png
[monthcalendar-last-weeknumbers-accessible-from-point]: ../images/monthcalendar-last-weeknumbers-accessible-from-point.png