Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Description
Imports WhoseShoutPrime.WhoseShoutPrime

Namespace Controllers
    Public Class CoffeeDatesController
        Inherits System.Web.Http.ApiController

        Private db As New ShoutDB3Xtr3m3Entities

        ' GET: api/CoffeeDates
        Function GetCoffeeDates() As IQueryable(Of CoffeeDate)
            Return db.CoffeeDates
        End Function

        ' GET: api/CoffeeDates/5
        <ResponseType(GetType(CoffeeDate))>
        Async Function GetCoffeeDate(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim coffeeDate As CoffeeDate = Await db.CoffeeDates.FindAsync(id)
            If IsNothing(coffeeDate) Then
                Return NotFound()
            End If

            Return Ok(coffeeDate)
        End Function

        ' PUT: api/CoffeeDates/5
        <ResponseType(GetType(Void))>
        Async Function PutCoffeeDate(ByVal id As Integer, ByVal coffeeDate As CoffeeDate) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = coffeeDate.Id Then
                Return BadRequest()
            End If

            db.Entry(coffeeDate).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (CoffeeDateExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/CoffeeDates
        <ResponseType(GetType(CoffeeDate))>
        Async Function PostCoffeeDate(ByVal coffeeDate As CoffeeDate) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.CoffeeDates.Add(coffeeDate)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = coffeeDate.Id}, coffeeDate)
        End Function

        ' DELETE: api/CoffeeDates/5
        <ResponseType(GetType(CoffeeDate))>
        Async Function DeleteCoffeeDate(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim coffeeDate As CoffeeDate = Await db.CoffeeDates.FindAsync(id)
            If IsNothing(coffeeDate) Then
                Return NotFound()
            End If

            db.CoffeeDates.Remove(coffeeDate)
            Await db.SaveChangesAsync()

            Return Ok(coffeeDate)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function CoffeeDateExists(ByVal id As Integer) As Boolean
            Return db.CoffeeDates.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace