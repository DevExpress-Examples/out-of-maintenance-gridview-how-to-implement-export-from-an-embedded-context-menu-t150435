Imports System
Imports System.Collections.Generic

Namespace Export.Models
	Public Class InMemoryModel
		Private Const DataItemsCount As Integer = 100

		Public Property ID() As Integer
		Public Property Text() As String
		Public Property Quantity() As Integer
		Public Property Price() As Decimal

		Public Shared Function GetTypedListModel() As List(Of InMemoryModel)
			Dim typedList As New List(Of InMemoryModel)()

			Dim randomizer As New Random()

			For index As Integer = 0 To DataItemsCount - 1
				typedList.Add(New InMemoryModel() With {.ID = index, .Text = "Text" & index.ToString(), .Quantity = randomizer.Next(1, 50), .Price = CDec(Math.Round((randomizer.NextDouble() * 100), 2))})
			Next index

			Return typedList
		End Function
	End Class
End Namespace