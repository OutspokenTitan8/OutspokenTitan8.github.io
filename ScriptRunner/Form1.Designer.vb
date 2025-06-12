Imports MaterialSkin
Imports MaterialSkin.Controls
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits MaterialForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        ListBoxScripts = New ListBox()
        ButtonRun = New MaterialButton()
        RichTextBoxOutput = New RichTextBox()
        ButtonStop = New MaterialButton()
        LabelStatus = New MaterialLabel()
        ButtonClearOutput = New MaterialButton()
        ButtonLoadPlayers = New MaterialButton()
        ButtonLoadGuesses = New MaterialButton()
        ButtonSaveGuesses = New MaterialButton()
        ButtonCalculateScores = New MaterialButton()
        DataGridViewGuesses = New DataGridView()
        ButtonOpenGitHubDesktop = New MaterialButton()
        ButtonClearGuesses = New MaterialButton()
        DataGridViewLeaderboard = New DataGridView()
        ButtonSaveLeaderboard = New MaterialButton()
        ButtonAddScoresToLeaderboard = New MaterialButton()
        PictureBoxTrack = New PictureBox()
        TextBoxRaceInfo = New RichTextBox()
        WebView2Q = New Microsoft.Web.WebView2.WinForms.WebView2()
        ButtonShowQ = New MaterialButton()
        ButtonShowR = New MaterialButton()
        WebView2R = New Microsoft.Web.WebView2.WinForms.WebView2()
        btnExportJson = New MaterialButton()
        CType(DataGridViewGuesses, ISupportInitialize).BeginInit()
        CType(DataGridViewLeaderboard, ISupportInitialize).BeginInit()
        CType(PictureBoxTrack, ISupportInitialize).BeginInit()
        CType(WebView2Q, ISupportInitialize).BeginInit()
        CType(WebView2R, ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' ListBoxScripts
        ' 
        ListBoxScripts.ItemHeight = 15
        ListBoxScripts.Location = New Point(59, 239)
        ListBoxScripts.Name = "ListBoxScripts"
        ListBoxScripts.Size = New Size(200, 319)
        ListBoxScripts.TabIndex = 0
        ' 
        ' ButtonRun
        ' 
        ButtonRun.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonRun.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonRun.Depth = 0
        ButtonRun.HighEmphasis = True
        ButtonRun.Icon = Nothing
        ButtonRun.Location = New Point(59, 194)
        ButtonRun.Margin = New Padding(4, 6, 4, 6)
        ButtonRun.MouseState = MouseState.HOVER
        ButtonRun.Name = "ButtonRun"
        ButtonRun.NoAccentTextColor = Color.Empty
        ButtonRun.Size = New Size(64, 36)
        ButtonRun.TabIndex = 2
        ButtonRun.Text = "Run"
        ButtonRun.Type = MaterialButton.MaterialButtonType.Contained
        ButtonRun.UseAccentColor = False
        ' 
        ' RichTextBoxOutput
        ' 
        RichTextBoxOutput.BackColor = Color.FromArgb(CByte(48), CByte(48), CByte(48))
        RichTextBoxOutput.ForeColor = Color.White
        RichTextBoxOutput.Location = New Point(334, 194)
        RichTextBoxOutput.Name = "RichTextBoxOutput"
        RichTextBoxOutput.ReadOnly = True
        RichTextBoxOutput.Size = New Size(379, 484)
        RichTextBoxOutput.TabIndex = 3
        RichTextBoxOutput.Text = ""
        ' 
        ' ButtonStop
        ' 
        ButtonStop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonStop.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonStop.Depth = 0
        ButtonStop.Enabled = False
        ButtonStop.HighEmphasis = True
        ButtonStop.Icon = Nothing
        ButtonStop.Location = New Point(195, 194)
        ButtonStop.Margin = New Padding(4, 6, 4, 6)
        ButtonStop.MouseState = MouseState.HOVER
        ButtonStop.Name = "ButtonStop"
        ButtonStop.NoAccentTextColor = Color.Empty
        ButtonStop.Size = New Size(64, 36)
        ButtonStop.TabIndex = 4
        ButtonStop.Text = "Stop"
        ButtonStop.Type = MaterialButton.MaterialButtonType.Contained
        ButtonStop.UseAccentColor = True
        ' 
        ' LabelStatus
        ' 
        LabelStatus.AutoSize = True
        LabelStatus.Depth = 0
        LabelStatus.Font = New Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel)
        LabelStatus.Location = New Point(334, 119)
        LabelStatus.MouseState = MouseState.HOVER
        LabelStatus.Name = "LabelStatus"
        LabelStatus.Size = New Size(26, 19)
        LabelStatus.TabIndex = 5
        LabelStatus.Text = "Idle"
        ' 
        ' ButtonClearOutput
        ' 
        ButtonClearOutput.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonClearOutput.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonClearOutput.Depth = 0
        ButtonClearOutput.HighEmphasis = True
        ButtonClearOutput.Icon = Nothing
        ButtonClearOutput.Location = New Point(334, 149)
        ButtonClearOutput.Margin = New Padding(4, 6, 4, 6)
        ButtonClearOutput.MouseState = MouseState.HOVER
        ButtonClearOutput.Name = "ButtonClearOutput"
        ButtonClearOutput.NoAccentTextColor = Color.Empty
        ButtonClearOutput.Size = New Size(126, 36)
        ButtonClearOutput.TabIndex = 6
        ButtonClearOutput.Text = "Clear Output"
        ButtonClearOutput.Type = MaterialButton.MaterialButtonType.Outlined
        ButtonClearOutput.UseAccentColor = False
        ' 
        ' ButtonLoadPlayers
        ' 
        ButtonLoadPlayers.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonLoadPlayers.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonLoadPlayers.Depth = 0
        ButtonLoadPlayers.HighEmphasis = True
        ButtonLoadPlayers.Icon = Nothing
        ButtonLoadPlayers.Location = New Point(24, 655)
        ButtonLoadPlayers.Margin = New Padding(4, 6, 4, 6)
        ButtonLoadPlayers.MouseState = MouseState.HOVER
        ButtonLoadPlayers.Name = "ButtonLoadPlayers"
        ButtonLoadPlayers.NoAccentTextColor = Color.Empty
        ButtonLoadPlayers.Size = New Size(125, 36)
        ButtonLoadPlayers.TabIndex = 7
        ButtonLoadPlayers.Text = "Load Players"
        ButtonLoadPlayers.Type = MaterialButton.MaterialButtonType.Contained
        ButtonLoadPlayers.UseAccentColor = False
        ' 
        ' ButtonLoadGuesses
        ' 
        ButtonLoadGuesses.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonLoadGuesses.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonLoadGuesses.Depth = 0
        ButtonLoadGuesses.Enabled = False
        ButtonLoadGuesses.HighEmphasis = True
        ButtonLoadGuesses.Icon = Nothing
        ButtonLoadGuesses.Location = New Point(24, 607)
        ButtonLoadGuesses.Margin = New Padding(4, 6, 4, 6)
        ButtonLoadGuesses.MouseState = MouseState.HOVER
        ButtonLoadGuesses.Name = "ButtonLoadGuesses"
        ButtonLoadGuesses.NoAccentTextColor = Color.Empty
        ButtonLoadGuesses.Size = New Size(126, 36)
        ButtonLoadGuesses.TabIndex = 8
        ButtonLoadGuesses.Text = "Load Guesses"
        ButtonLoadGuesses.Type = MaterialButton.MaterialButtonType.Contained
        ButtonLoadGuesses.UseAccentColor = False
        ' 
        ' ButtonSaveGuesses
        ' 
        ButtonSaveGuesses.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonSaveGuesses.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonSaveGuesses.Depth = 0
        ButtonSaveGuesses.HighEmphasis = True
        ButtonSaveGuesses.Icon = Nothing
        ButtonSaveGuesses.Location = New Point(157, 607)
        ButtonSaveGuesses.Margin = New Padding(4, 6, 4, 6)
        ButtonSaveGuesses.MouseState = MouseState.HOVER
        ButtonSaveGuesses.Name = "ButtonSaveGuesses"
        ButtonSaveGuesses.NoAccentTextColor = Color.Empty
        ButtonSaveGuesses.Size = New Size(124, 36)
        ButtonSaveGuesses.TabIndex = 9
        ButtonSaveGuesses.Text = "Save Guesses"
        ButtonSaveGuesses.Type = MaterialButton.MaterialButtonType.Contained
        ButtonSaveGuesses.UseAccentColor = False
        ' 
        ' ButtonCalculateScores
        ' 
        ButtonCalculateScores.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonCalculateScores.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonCalculateScores.Depth = 0
        ButtonCalculateScores.HighEmphasis = True
        ButtonCalculateScores.Icon = Nothing
        ButtonCalculateScores.Location = New Point(720, 841)
        ButtonCalculateScores.Margin = New Padding(4, 6, 4, 6)
        ButtonCalculateScores.MouseState = MouseState.HOVER
        ButtonCalculateScores.Name = "ButtonCalculateScores"
        ButtonCalculateScores.NoAccentTextColor = Color.Empty
        ButtonCalculateScores.Size = New Size(162, 36)
        ButtonCalculateScores.TabIndex = 10
        ButtonCalculateScores.Text = "Calculate Scores"
        ButtonCalculateScores.Type = MaterialButton.MaterialButtonType.Contained
        ButtonCalculateScores.UseAccentColor = False
        ' 
        ' DataGridViewGuesses
        ' 
        DataGridViewGuesses.BackgroundColor = Color.FromArgb(CByte(40), CByte(40), CByte(40))
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle1.ForeColor = Color.White
        DataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(CByte(90), CByte(90), CByte(90))
        DataGridViewCellStyle1.SelectionForeColor = Color.White
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        DataGridViewGuesses.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewGuesses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = Color.FromArgb(CByte(48), CByte(48), CByte(48))
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle2.ForeColor = Color.White
        DataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(CByte(70), CByte(70), CByte(70))
        DataGridViewCellStyle2.SelectionForeColor = Color.White
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        DataGridViewGuesses.DefaultCellStyle = DataGridViewCellStyle2
        DataGridViewGuesses.EnableHeadersVisualStyles = False
        DataGridViewGuesses.GridColor = Color.DimGray
        DataGridViewGuesses.Location = New Point(24, 700)
        DataGridViewGuesses.Name = "DataGridViewGuesses"
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle3.ForeColor = Color.White
        DataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.True
        DataGridViewGuesses.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        DataGridViewGuesses.Size = New Size(689, 225)
        DataGridViewGuesses.TabIndex = 11
        ' 
        ' ButtonOpenGitHubDesktop
        ' 
        ButtonOpenGitHubDesktop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonOpenGitHubDesktop.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonOpenGitHubDesktop.Depth = 0
        ButtonOpenGitHubDesktop.HighEmphasis = True
        ButtonOpenGitHubDesktop.Icon = Nothing
        ButtonOpenGitHubDesktop.Location = New Point(105, 146)
        ButtonOpenGitHubDesktop.Margin = New Padding(4, 6, 4, 6)
        ButtonOpenGitHubDesktop.MouseState = MouseState.HOVER
        ButtonOpenGitHubDesktop.Name = "ButtonOpenGitHubDesktop"
        ButtonOpenGitHubDesktop.NoAccentTextColor = Color.Empty
        ButtonOpenGitHubDesktop.Size = New Size(116, 36)
        ButtonOpenGitHubDesktop.TabIndex = 14
        ButtonOpenGitHubDesktop.Text = "Open Github"
        ButtonOpenGitHubDesktop.Type = MaterialButton.MaterialButtonType.Contained
        ButtonOpenGitHubDesktop.UseAccentColor = False
        ' 
        ' ButtonClearGuesses
        ' 
        ButtonClearGuesses.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonClearGuesses.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonClearGuesses.Depth = 0
        ButtonClearGuesses.HighEmphasis = True
        ButtonClearGuesses.Icon = Nothing
        ButtonClearGuesses.Location = New Point(157, 655)
        ButtonClearGuesses.Margin = New Padding(4, 6, 4, 6)
        ButtonClearGuesses.MouseState = MouseState.HOVER
        ButtonClearGuesses.Name = "ButtonClearGuesses"
        ButtonClearGuesses.NoAccentTextColor = Color.Empty
        ButtonClearGuesses.Size = New Size(133, 36)
        ButtonClearGuesses.TabIndex = 17
        ButtonClearGuesses.Text = "Clear Guesses"
        ButtonClearGuesses.Type = MaterialButton.MaterialButtonType.Contained
        ButtonClearGuesses.UseAccentColor = False
        ' 
        ' DataGridViewLeaderboard
        ' 
        DataGridViewLeaderboard.BackgroundColor = Color.FromArgb(CByte(40), CByte(40), CByte(40))
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        DataGridViewCellStyle4.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle4.ForeColor = Color.White
        DataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(CByte(90), CByte(90), CByte(90))
        DataGridViewCellStyle4.SelectionForeColor = Color.White
        DataGridViewCellStyle4.WrapMode = DataGridViewTriState.True
        DataGridViewLeaderboard.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        DataGridViewLeaderboard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = Color.FromArgb(CByte(48), CByte(48), CByte(48))
        DataGridViewCellStyle5.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle5.ForeColor = Color.White
        DataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(CByte(70), CByte(70), CByte(70))
        DataGridViewCellStyle5.SelectionForeColor = Color.White
        DataGridViewCellStyle5.WrapMode = DataGridViewTriState.False
        DataGridViewLeaderboard.DefaultCellStyle = DataGridViewCellStyle5
        DataGridViewLeaderboard.EnableHeadersVisualStyles = False
        DataGridViewLeaderboard.GridColor = Color.DimGray
        DataGridViewLeaderboard.Location = New Point(964, 661)
        DataGridViewLeaderboard.Name = "DataGridViewLeaderboard"
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        DataGridViewCellStyle6.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle6.ForeColor = Color.White
        DataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = DataGridViewTriState.True
        DataGridViewLeaderboard.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        DataGridViewLeaderboard.Size = New Size(324, 264)
        DataGridViewLeaderboard.TabIndex = 18
        ' 
        ' ButtonSaveLeaderboard
        ' 
        ButtonSaveLeaderboard.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonSaveLeaderboard.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonSaveLeaderboard.Depth = 0
        ButtonSaveLeaderboard.HighEmphasis = True
        ButtonSaveLeaderboard.Icon = Nothing
        ButtonSaveLeaderboard.Location = New Point(1295, 889)
        ButtonSaveLeaderboard.Margin = New Padding(4, 6, 4, 6)
        ButtonSaveLeaderboard.MouseState = MouseState.HOVER
        ButtonSaveLeaderboard.Name = "ButtonSaveLeaderboard"
        ButtonSaveLeaderboard.NoAccentTextColor = Color.Empty
        ButtonSaveLeaderboard.Size = New Size(163, 36)
        ButtonSaveLeaderboard.TabIndex = 19
        ButtonSaveLeaderboard.Text = "Save Leaderboard"
        ButtonSaveLeaderboard.Type = MaterialButton.MaterialButtonType.Contained
        ButtonSaveLeaderboard.UseAccentColor = False
        ' 
        ' ButtonAddScoresToLeaderboard
        ' 
        ButtonAddScoresToLeaderboard.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonAddScoresToLeaderboard.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonAddScoresToLeaderboard.Depth = 0
        ButtonAddScoresToLeaderboard.HighEmphasis = True
        ButtonAddScoresToLeaderboard.Icon = Nothing
        ButtonAddScoresToLeaderboard.Location = New Point(720, 889)
        ButtonAddScoresToLeaderboard.Margin = New Padding(4, 6, 4, 6)
        ButtonAddScoresToLeaderboard.MouseState = MouseState.HOVER
        ButtonAddScoresToLeaderboard.Name = "ButtonAddScoresToLeaderboard"
        ButtonAddScoresToLeaderboard.NoAccentTextColor = Color.Empty
        ButtonAddScoresToLeaderboard.Size = New Size(237, 36)
        ButtonAddScoresToLeaderboard.TabIndex = 20
        ButtonAddScoresToLeaderboard.Text = "Add to Leaderboard Scores"
        ButtonAddScoresToLeaderboard.Type = MaterialButton.MaterialButtonType.Contained
        ButtonAddScoresToLeaderboard.UseAccentColor = False
        ' 
        ' PictureBoxTrack
        ' 
        PictureBoxTrack.Location = New Point(920, 91)
        PictureBoxTrack.Name = "PictureBoxTrack"
        PictureBoxTrack.Size = New Size(400, 250)
        PictureBoxTrack.SizeMode = PictureBoxSizeMode.Zoom
        PictureBoxTrack.TabIndex = 22
        PictureBoxTrack.TabStop = False
        ' 
        ' TextBoxRaceInfo
        ' 
        TextBoxRaceInfo.BackColor = Color.FromArgb(CByte(48), CByte(48), CByte(48))
        TextBoxRaceInfo.BorderStyle = BorderStyle.FixedSingle
        TextBoxRaceInfo.Font = New Font("Segoe UI", 14F)
        TextBoxRaceInfo.ForeColor = Color.White
        TextBoxRaceInfo.Location = New Point(920, 358)
        TextBoxRaceInfo.Name = "TextBoxRaceInfo"
        TextBoxRaceInfo.ReadOnly = True
        TextBoxRaceInfo.Size = New Size(400, 220)
        TextBoxRaceInfo.TabIndex = 23
        TextBoxRaceInfo.Text = ""
        ' 
        ' WebView2Q
        ' 
        WebView2Q.AllowExternalDrop = True
        WebView2Q.CreationProperties = Nothing
        WebView2Q.DefaultBackgroundColor = Color.White
        WebView2Q.Location = New Point(1622, 119)
        WebView2Q.Name = "WebView2Q"
        WebView2Q.Size = New Size(582, 806)
        WebView2Q.TabIndex = 27
        WebView2Q.ZoomFactor = 1R
        ' 
        ' ButtonShowQ
        ' 
        ButtonShowQ.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonShowQ.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonShowQ.Depth = 0
        ButtonShowQ.HighEmphasis = True
        ButtonShowQ.Icon = Nothing
        ButtonShowQ.Location = New Point(1452, 119)
        ButtonShowQ.Margin = New Padding(4, 6, 4, 6)
        ButtonShowQ.MouseState = MouseState.HOVER
        ButtonShowQ.Name = "ButtonShowQ"
        ButtonShowQ.NoAccentTextColor = Color.Empty
        ButtonShowQ.Size = New Size(151, 36)
        ButtonShowQ.TabIndex = 28
        ButtonShowQ.Text = "Show Qualifying"
        ButtonShowQ.Type = MaterialButton.MaterialButtonType.Contained
        ButtonShowQ.UseAccentColor = False
        ' 
        ' ButtonShowR
        ' 
        ButtonShowR.AutoSizeMode = AutoSizeMode.GrowAndShrink
        ButtonShowR.Density = MaterialButton.MaterialButtonDensity.Default
        ButtonShowR.Depth = 0
        ButtonShowR.HighEmphasis = True
        ButtonShowR.Icon = Nothing
        ButtonShowR.Location = New Point(1479, 167)
        ButtonShowR.Margin = New Padding(4, 6, 4, 6)
        ButtonShowR.MouseState = MouseState.HOVER
        ButtonShowR.Name = "ButtonShowR"
        ButtonShowR.NoAccentTextColor = Color.Empty
        ButtonShowR.Size = New Size(104, 36)
        ButtonShowR.TabIndex = 29
        ButtonShowR.Text = "Show Race"
        ButtonShowR.Type = MaterialButton.MaterialButtonType.Contained
        ButtonShowR.UseAccentColor = False
        ' 
        ' WebView2R
        ' 
        WebView2R.AllowExternalDrop = True
        WebView2R.CreationProperties = Nothing
        WebView2R.DefaultBackgroundColor = Color.White
        WebView2R.Location = New Point(1622, 119)
        WebView2R.Name = "WebView2R"
        WebView2R.Size = New Size(582, 806)
        WebView2R.TabIndex = 30
        WebView2R.ZoomFactor = 1R
        ' 
        ' btnExportJson
        ' 
        btnExportJson.AutoSizeMode = AutoSizeMode.GrowAndShrink
        btnExportJson.Density = MaterialButton.MaterialButtonDensity.Default
        btnExportJson.Depth = 0
        btnExportJson.HighEmphasis = True
        btnExportJson.Icon = Nothing
        btnExportJson.Location = New Point(720, 793)
        btnExportJson.Margin = New Padding(4, 6, 4, 6)
        btnExportJson.MouseState = MouseState.HOVER
        btnExportJson.Name = "btnExportJson"
        btnExportJson.NoAccentTextColor = Color.Empty
        btnExportJson.Size = New Size(118, 36)
        btnExportJson.TabIndex = 31
        btnExportJson.Text = "Export json"
        btnExportJson.Type = MaterialButton.MaterialButtonType.Contained
        btnExportJson.UseAccentColor = False
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(2210, 990)
        Controls.Add(btnExportJson)
        Controls.Add(WebView2R)
        Controls.Add(ButtonShowR)
        Controls.Add(ButtonShowQ)
        Controls.Add(WebView2Q)
        Controls.Add(TextBoxRaceInfo)
        Controls.Add(PictureBoxTrack)
        Controls.Add(ButtonAddScoresToLeaderboard)
        Controls.Add(ButtonSaveLeaderboard)
        Controls.Add(DataGridViewLeaderboard)
        Controls.Add(ButtonClearGuesses)
        Controls.Add(ButtonOpenGitHubDesktop)
        Controls.Add(DataGridViewGuesses)
        Controls.Add(ButtonCalculateScores)
        Controls.Add(ButtonSaveGuesses)
        Controls.Add(ButtonLoadGuesses)
        Controls.Add(ButtonLoadPlayers)
        Controls.Add(ButtonClearOutput)
        Controls.Add(LabelStatus)
        Controls.Add(ButtonStop)
        Controls.Add(RichTextBoxOutput)
        Controls.Add(ButtonRun)
        Controls.Add(ListBoxScripts)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "F1 Race Manager"
        CType(DataGridViewGuesses, ISupportInitialize).EndInit()
        CType(DataGridViewLeaderboard, ISupportInitialize).EndInit()
        CType(PictureBoxTrack, ISupportInitialize).EndInit()
        CType(WebView2Q, ISupportInitialize).EndInit()
        CType(WebView2R, ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents ListBoxScripts As ListBox
    Friend WithEvents ButtonRun As MaterialButton
    Friend WithEvents RichTextBoxOutput As RichTextBox
    Friend WithEvents ButtonStop As MaterialButton
    Friend WithEvents LabelStatus As MaterialLabel
    Friend WithEvents ButtonClearOutput As MaterialButton
    Friend WithEvents ButtonLoadPlayers As MaterialButton
    Friend WithEvents ButtonLoadGuesses As MaterialButton
    Friend WithEvents ButtonSaveGuesses As MaterialButton
    Friend WithEvents ButtonCalculateScores As MaterialButton
    Friend WithEvents DataGridViewGuesses As DataGridView
    Friend WithEvents ButtonOpenGitHubDesktop As MaterialButton
    Friend WithEvents ButtonClearGuesses As MaterialButton
    Friend WithEvents DataGridViewLeaderboard As DataGridView
    Friend WithEvents ButtonSaveLeaderboard As MaterialButton
    Friend WithEvents ButtonAddScoresToLeaderboard As MaterialButton
    Friend WithEvents PictureBoxTrack As PictureBox
    Friend WithEvents TextBoxRaceInfo As RichTextBox
    Friend WithEvents WebView2Q As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents ButtonShowQ As MaterialButton
    Friend WithEvents ButtonShowR As MaterialButton
    Friend WithEvents WebView2R As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents btnExportJson As MaterialButton
End Class
