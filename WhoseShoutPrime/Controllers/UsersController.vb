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
    Public Class UsersController
        Inherits System.Web.Http.ApiController

        Private db As New ShoutDB3Xtr3m3Entities

        ' GET: api/Users
        Function GetUsers() As IQueryable(Of User)
            Return db.Users
        End Function

        ' GET: api/Users/5
        <ResponseType(GetType(User))>
        Async Function GetUser(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim user As User = Await db.Users.FindAsync(id)
            If IsNothing(user) Then
                Return NotFound()
            End If

            Return Ok(user)
        End Function

        ' PUT: api/Users/5
        <ResponseType(GetType(Void))>
        Async Function PutUser(ByVal id As Integer, ByVal user As User) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = user.Id Then
                Return BadRequest()
            End If

            db.Entry(user).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (UserExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/Users
        <ResponseType(GetType(User))>
        Async Function PostUser(ByVal user As User) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.Users.Add(user)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = user.Id}, user)
        End Function

        ' DELETE: api/Users/5
        <ResponseType(GetType(User))>
        Async Function DeleteUser(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim user As User = Await db.Users.FindAsync(id)
            If IsNothing(user) Then
                Return NotFound()
            End If

            db.Users.Remove(user)
            Await db.SaveChangesAsync()

            Return Ok(user)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function UserExists(ByVal id As Integer) As Boolean
            Return db.Users.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace