Imports MySql.Data.MySqlClient

Public Class Form1
    Dim conn As MySqlConnection
    Dim COMMAND As MySqlCommand
    Private Sub ButtonConnect_Click(sender As Object, e As EventArgs) Handles ButtonConnect.Click

        conn = New MySqlConnection
        conn.ConnectionString = "server=localhost; userid=root; password=root; database=crud_demo_db;"

        Try
            conn.Open()
            MessageBox.Show("Connected")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            conn.Close()
        End Try
    End Sub

    Private Sub ButtonInsert_Click(sender As Object, e As EventArgs) Handles ButtonInsert.Click
        Dim query As String = "INSERT INTO students_tbl (name, age, email) VALUES (@name, @age, @email)"
        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=crud_demo_db;")
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@name", TextBoxName.Text)
                    cmd.Parameters.AddWithValue("@age", CInt(TextBoxAge.Text))
                    cmd.Parameters.AddWithValue("@email", TextBoxEmail.Text)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record Insert Successfully!")
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonRead_Click(sender As Object, e As EventArgs) Handles ButtonRead.Click
        Dim query As String = "SELECT * FROM crud_demo_db.students_tbl;"
        'Dim query As String = "SELECT name, age, email FROM crud_demo_db.students_tbl;" '\\  only show name age email and not show id

        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=crud_demo_db;")
                Dim adapter As New MySqlDataAdapter(query, conn)
                Dim table As New DataTable()    'Table Object
                adapter.Fill(table)     'From Adapter to Table Object
                DataGridView1.DataSource = table    'Display to DataGridView
                DataGridView1.Columns("id").Visible = False ' Hides id column, block this ' for to see id
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonUpdate_Click(sender As Object, e As EventArgs) Handles ButtonUpdate.Click
        Dim query As String = "UPDAte students_tbl SET name=@name, age=@age, email=@email WHERE id=@id"
        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=crud_demo_db;")
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@name", TextBoxName.Text)
                    cmd.Parameters.AddWithValue("@age", CInt(TextBoxAge.Text))
                    cmd.Parameters.AddWithValue("@email", TextBoxEmail.Text)
                    Dim selectedId As Integer = CInt(DataGridView1.CurrentRow.Cells("id").Value)
                    cmd.Parameters.AddWithValue("@id", selectedId)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record Insert Successfully!")
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click
        Dim query As String = "DELETE FROM students_tbl WHERE id=@id"

        Try
            ' Make sure a row is selected
            If DataGridView1.CurrentRow Is Nothing Then
                MessageBox.Show("Please select a record to delete.")
                Exit Sub
            End If

            ' confirming delete
            If MessageBox.Show("Are you sure you want to delete this record?",
                           "Confirm Delete",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning) = DialogResult.No Then
                Exit Sub
            End If

            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=crud_demo_db;")
                conn.Open()
                Using cmd As New MySqlCommand(query, conn) ' get selected id from datagridview
                    Dim selectedId As Integer = CInt(DataGridView1.CurrentRow.Cells("id").Value)
                    cmd.Parameters.AddWithValue("@id", selectedId)

                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record Deleted Successfully!")
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            TextBoxName.Text = selectedRow.Cells("name").Value.ToString()
            TextBoxAge.Text = selectedRow.Cells("age").Value.ToString()
            TextBoxEmail.Text = selectedRow.Cells("email").Value.ToString()

            TextBoxHiddenId.Text = selectedRow.Cells("id").Value.ToString()
        End If
    End Sub
End Class
