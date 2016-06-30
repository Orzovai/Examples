Imports System.Data.OleDb       'Stellst Methoden zum Aufbau der Verbindung zu Access bereits
Imports System                  'Stellt alles mögliche bereit (grundlegender Namesraum System)

'Hinweise: Falls mehrere Einträge die gleiche Kartenummer haben sollten, wird hier nur der letzte vorkommende behandelt!
'Achte also darauf, dass Kartenummern eindeutig sind

'Von Dir muss angepasst werden...
'1. Die Variable dataBasePath (Pfad zu Deiner .accdb Datenbank)
'2. In Zeile 23 muss die Tabelle "tbl_Kartennummer" durch den Namen Deiner Tabelle ersetzt werden

Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click   'Wird beim Klicken des Buttons ausgeführt
        Dim con As New OleDbConnection                              'Die eigentliche Verbindung zu Access
        Dim cmd As OleDbCommand = con.CreateCommand()               'Der Befehl mit dessen Hilfe von Access gelesen wird

        Dim dataBasePath As String                                  'Variable, die den Pfad zu Deiner Access Datenbank darstellt

        Try                                                          'Fängt eventuelle Fehler ab und leitet sie zu Catch weiter
            dataBasePath = "C:\users\david\Desktop\ADO_VB_Example.accdb"                                           'Der Pfad zu Deiner Datenbank ! HIER DEINEN PFAD ANGEBEN !
            con.ConnectionString = String.Format("PROVIDER=Microsoft.ACE.OLEDB.12.0;DATA SOURCE={0}", dataBasePath)    'Der sogenannte ConnectionString
            con.Open()                                                  'Die Verbindung zur Datenbank wird geöffnet

            cmd.CommandText = String.Format("select * from tbl_Kartennummer where Kartenummer={0};", TextBox1.Text)   'Datenbankabfrage in SQL (eine Sprache, die Access versteht). Dabei wird aus der Kartennummer Textbox gelesen
            cmd.ExecuteNonQuery()                                           'SQL Abfrage wird ausgeführt

            Dim rdr As OleDbDataReader = cmd.ExecuteReader()                'Ein sogenannter DataReader um die Daten, die unsere Abfrage ergeben hat zu lesen

            While (rdr.Read())                                              'Daten lesen...
                TextBox2.Text = rdr.GetString(2)                            '...und in Textboxen schreibe
                TextBox3.Text = rdr.GetString(3)
            End While

            rdr.Close()         'DataReader wieder schließen
            con.Close()         'Verbindung wieder schließen

        Catch ex As Exception       'Fehlerbehandlung

            MessageBox.Show(String.Format("Folgender Fehler ist aufgetreten: {0}", ex.Message))

        End Try

    End Sub
End Class
