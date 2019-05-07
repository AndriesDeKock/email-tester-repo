Imports System.Net.Mail

Public Class Form1

    Private Sub btnSend_Click(sender As System.Object, e As System.EventArgs) Handles btnSend.Click

        lblStatus.Text = "Sending email..."
        Application.DoEvents()

        Dim strResults As String = f_sendAutoEmailstr(txtTo.Text, txtFrom.Text, "", "Email Message", _RetrieveEmailBody(), "", txtSMTP.Text,
                                                      txtSMTPuser.Text, txtSMTPPass.Text, chkSSL.Checked, txtDomain.Text)
        If strResults <> "" Then
            lblStatus.Text = ""
            Application.DoEvents()
            MessageBox.Show("There was an error - Error: " & strResults, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            lblStatus.Text = "Email was sent!"
            Application.DoEvents()
        End If

    End Sub

#Region "Functions"

    Private Function _RetrieveEmailBody()

        Dim span As String = "<span lang='en'>"
        Dim body As String = "<BODY style=""font-size:11pt;font-family:Calibri"">"
        Dim body_close As String = "</BODY>"
        Dim span_close As String = "</span>"

        Dim strContent As String = "Good day<br/><br/>This is an email message.<br/><br/>"


        Return strContent

    End Function

    Private Function f_sendAutoEmailstr(ByVal toAdres As String, ByVal fromAdres As String,
                                        ByVal ccAdres As String, ByVal subject As String,
                                        ByVal body As String, ByVal attachmentPath As String,
                                        ByVal smtpServer As String, ByVal smtpUsername As String,
                                        ByVal smtpPassword As String, blSSL As Boolean,
                                        Optional strDomain As String = "") As String

        Dim strReturn As String = ""
        Try
            'create a new e-mail message
            Dim oMsg As New System.Net.Mail.MailMessage(fromAdres, toAdres, subject, body)

            'add the cc address if one is specified
            If Not ccAdres = "" Then
                oMsg.CC.Add(ccAdres)
            End If

            'add the attachment if one is specified
            If Not attachmentPath = "" Then
                Dim oAttch As Attachment = New Attachment(attachmentPath)
                oMsg.Attachments.Add(oAttch)
            End If

            'create the SMTP client with the mail server name or IP
            Dim smtpmail As New SmtpClient(smtpServer)

            oMsg.IsBodyHtml = True

            'if a user name and password is specified, add the credentials
            If Not smtpUsername = "" Then
                Dim smtpUser As New System.Net.NetworkCredential With {
                    .UserName = smtpUsername,
                    .Password = smtpPassword
                }

                'log
                Debug.WriteLine(String.Format("SMTP user: {0}, SMTP Pass: {1}", smtpUsername, smtpPassword))

                If strDomain <> "" Then
                    Debug.WriteLine(String.Format("Domain: {0}", strDomain))
                    smtpUser.Domain = strDomain
                End If

                smtpmail.EnableSsl = blSSL
                smtpmail.Port = 587


                Debug.WriteLine(String.Format("SSL Enabled: {0}, Port: {1}", blSSL.ToString(), smtpmail.Port.ToString()))

                smtpmail.UseDefaultCredentials = False
                Debug.WriteLine(String.Format("Use Default Credentials: {0}", smtpmail.UseDefaultCredentials.ToString()))

                smtpmail.Credentials = smtpUser
            End If

            'send the e-mail
            smtpmail.Send(oMsg)

        Catch ex As Exception
            Debug.WriteLine(String.Format("Error: {0}", ex.Message.ToString()))
            strReturn = ex.Message
        End Try

        Return strReturn

    End Function

#End Region



End Class
