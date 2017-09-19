Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Management
Imports System.Security

Public Class Form1

    Dim login As Integer
    Dim confirm As Integer
    Dim confirmhwid As Integer
    Dim version As Integer
    Dim appPath As String = My.Application.Info.DirectoryPath
    Dim exePath As String = Application.StartupPath

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigate WebBrowsers
        WebBrowser1.Navigate("http://localhost/userpass.php")
        WebBrowser2.Navigate("http://localhost/usercheck.php")
        WebBrowser3.Navigate("http://localhost/hwid.php")

        ' Load "Remember Me" settings
        If My.Settings.Remember = True Then
            TextBox1.Text = My.Settings.Username
            TextBox2.Text = My.Settings.Password
            CheckBox1.Checked = My.Settings.Remember
        Else
        End If

        ' Generate the HWID
        Dim hw As New clsComputerInfo
        Dim cpu As String
        Dim mb As String
        Dim mac As String
        Dim hwid As String
        cpu = hw.GetProcessorId()
        mb = hw.GetMotherboardID()
        mac = hw.GetMACAddress()
        hwid = cpu + mb + mac
        Dim hwidEncrypted As String = Strings.UCase(hw.getMD5Hash(cpu & mb & mac))
        TextBox3.Text = hwidEncrypted
    End Sub


    Private Class clsComputerInfo
        'Get processor ID
        Friend Function GetProcessorId() As String
            Dim strProcessorID As String = String.Empty
            Dim query As New SelectQuery("Win32_processor")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject
            For Each info In search.Get()
                strProcessorID = info("processorID").ToString()
            Next
            Return strProcessorID
        End Function
        ' Get MAC Address
        Friend Function GetMACAddress() As String
            Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
            Dim moc As ManagementObjectCollection = mc.GetInstances()
            Dim MacAddress As String = String.Empty
            For Each mo As ManagementObject In moc
                If (MacAddress.Equals(String.Empty)) Then
                    If CBool(mo("IPEnabled")) Then MacAddress = mo("MacAddress").ToString()
                    mo.Dispose()
                End If
                MacAddress = MacAddress.Replace(":", String.Empty)
            Next
            Return MacAddress
        End Function
        ' Get Motherboard ID
        Friend Function GetMotherboardID() As String
            Dim strMotherboardID As String = String.Empty
            Dim query As New SelectQuery("Win32_BaseBoard")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject
            For Each info In search.Get()
                strMotherboardID = info("product").ToString()
            Next
            Return strMotherboardID
        End Function
        ' Encrypt HWID
        Friend Function getMD5Hash(ByVal strToHash As String) As String
            Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)
            bytesToHash = md5Obj.ComputeHash(bytesToHash)
            Dim strResult As String = ""
            For Each b As Byte In bytesToHash
                strResult += b.ToString("x2")
            Next
            Return strResult
        End Function
    End Class


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        login = 1

        Try
            WebBrowser1.Document.GetElementById("username").SetAttribute("value", TextBox1.Text)
            WebBrowser1.Document.GetElementById("password").SetAttribute("value", TextBox2.Text)
            WebBrowser1.Document.GetElementById("submit").InvokeMember("click")
        Catch
            MsgBox("Error: Login function not setup right. Yell at the 'dev' until he figures out how to read the README.md", vbCritical)
        End Try

        Try
            WebBrowser2.Document.GetElementById("username").SetAttribute("value", TextBox1.Text)
            WebBrowser2.Document.GetElementById("submit").InvokeMember("click")
        Catch
            MsgBox("Error: Usercheck function not setup right. Yell at the 'dev' until he figures out how to read the README.md", vbCritical)
        End Try

        Try
            WebBrowser3.Document.GetElementById("username").SetAttribute("value", TextBox1.Text)
            WebBrowser3.Document.GetElementById("hwidin").SetAttribute("value", TextBox3.Text)
            WebBrowser3.Document.GetElementById("submit").InvokeMember("click")
        Catch
            MsgBox("Error: HWID function not setup right. Yell at the 'dev' until he figures out how to read the README.md", vbCritical)
        End Try
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        If login = 1 Then
            login = 0
            Try
                If WebBrowser1.DocumentText.Contains("0") Then
                    WebBrowser1.Navigate("http://localhost/userpass.php")
                    WebBrowser2.Navigate("http://localhost/usercheck.php")
                    WebBrowser3.Navigate("http://localhost/hwid.php")
                    MsgBox("Error: Username or password is incorrect", vbCritical)
                    Thread.Sleep(250)
                    Button1.Enabled = True
                    confirm = 0
                ElseIf WebBrowser1.DocumentText.Contains("1") Then
                    confirm = 1
                End If
            Catch ex As Exception
                MsgBox("Error: Forum login not working. Yell at the 'dev' until they learn to read the README.md", vbCritical)
            End Try
        End If
    End Sub

    Private Sub WebBrowser2_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser2.DocumentCompleted
        If confirm = 1 Then
            confirm = 0
            Try
                If WebBrowser2.DocumentText.Contains("4") Then ' Check if they are "admin"
                    confirmhwid = 1
                Else
                    If WebBrowser2.DocumentText.Contains("7") Then ' Check if they are "banned"
                        confirmhwid = 0
                        confirm = 0
                        login = 0

                        WebBrowser1.Navigate("http://localhost/userpass.php")
                        WebBrowser2.Navigate("http://localhost/usercheck.php")
                        WebBrowser3.Navigate("http://localhost/hwid.php")

                        MsgBox("Error: User is banned!", vbCritical)

                        Button1.Enabled = True
                    Else
                        If WebBrowser2.DocumentText.Contains("8") Then ' Check if they are "Premium CS:GO"
                            confirmhwid = 1
                        Else
                            If WebBrowser2.DocumentText.Contains("9") Then ' Check if they are "Premium CS:GO Lite"
                                confirmhwid = 1
                            Else
                                If WebBrowser2.DocumentText.Contains("10") Then ' Check if they are "Premium CS:GO Beta"
                                    confirmhwid = 1
                                Else
                                    If WebBrowser2.DocumentText.Contains("11") Then ' Check if they are "Premium Garry's Mod"
                                        confirmhwid = 1
                                    Else
                                        If WebBrowser2.DocumentText.Contains("2") Then ' Check if they are "Registered"
                                            confirmhwid = 0
                                            confirm = 0
                                            login = 0

                                            WebBrowser1.Navigate("http://localhost/userpass.php")
                                            WebBrowser2.Navigate("http://localhost/usercheck.php")
                                            WebBrowser3.Navigate("http://localhost/hwid.php")

                                            MsgBox("Error: User is banned!", vbCritical)

                                            Button1.Enabled = True
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                MsgBox("Error: usercheck_get.php not working. Yell at the 'dev' until they learn to read the README.md", vbCritical)
            End Try
        End If
    End Sub

    Private Sub WebBrowser3_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser3.DocumentCompleted
        If confirmhwid = 1 Then
            confirmhwid = 0
            Try
                If WebBrowser3.DocumentText.Contains("HWID is correct") Then
                    If (CheckBox1.Checked = True) Then
                        My.Settings.Remember = True
                    ElseIf (CheckBox1.Checked = False) Then
                        My.Settings.Remember = False
                    End If
                    My.Settings.Username = TextBox1.Text
                    My.Settings.Password = TextBox2.Text
                    My.Settings.Save()
                    TextBox1.Focus()
                    Form2.Show()
                    Me.Hide()
                    Button1.Enabled = True
                ElseIf WebBrowser3.DocumentText.Contains("HWID is not correct") Then
                    WebBrowser1.Navigate("http://localhost/userpass.php")
                    WebBrowser2.Navigate("http://localhost/usercheck.php")
                    WebBrowser3.Navigate("http://localhost/hwid.php")
                    MsgBox("Error: HWID is incorrect", vbCritical)
                    Thread.Sleep(250)
                    Button1.Enabled = True
                ElseIf WebBrowser3.DocumentText.Contains("") Then
                    WebBrowser1.Navigate("http://localhost/userpass.php")
                    WebBrowser2.Navigate("http://localhost/usercheck.php")
                    WebBrowser3.Navigate("http://localhost/hwid.php")
                    MsgBox("Error: No user with that name", vbCritical)
                    Thread.Sleep(250)
                    Button1.Enabled = True
                End If
            Catch ex As Exception
                MsgBox("Error: hwid_get.php not working. Yell at the 'dev' until they learn to read the README.md", vbCritical)
            End Try
        End If
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Process.Start("http://localhost/mybb/member.php?action=lostpw")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

    End Sub
End Class