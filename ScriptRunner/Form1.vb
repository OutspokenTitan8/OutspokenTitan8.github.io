Imports System.IO
Imports System.Diagnostics
Imports System.ComponentModel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Globalization
Imports Microsoft.Web.WebView2.Core
Imports MaterialSkin
Imports MaterialSkin.Controls


Public Class Form1
    Inherits MaterialForm

    Private materialSkinManager As MaterialSkinManager

    Public Sub New()
        InitializeComponent()

        ' Initialize MaterialSkin manager
        materialSkinManager = MaterialSkinManager.Instance
        materialSkinManager.AddFormToManage(Me)
        materialSkinManager.Theme = MaterialSkinManager.Themes.DARK

        ' Optional: Customize color scheme
        materialSkinManager.ColorScheme = New ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE)
    End Sub

    Private scriptFolder As String = "C:\Users\Wills Weaver\Documents\F1QualiWidget\Main_Dashboard\scripts"
    Private runningProcess As Process = Nothing

    Private guessesBindingList As BindingList(Of Guess)

    Private playersFile As String = "players.json"
    Private guessesFile As String = "guesses.json"
    Private raceDataFile As String = "C:\Users\Wills Weaver\Documents\GitHub\OutspokenTitan8.github.io\race_results_game.json"


    Private players As List(Of Player)
    Private guesses As List(Of Guess)
    Private leaderboardScores As List(Of LeaderboardEntry)
    Private leaderboardFile As String = "C:\Users\Wills Weaver\Documents\GitHub\OutspokenTitan8.github.io\leaderboard.json"



    Private seasonData As SeasonData



    Public Class LeaderboardEntry
        Public Property PlayerName As String
        Public Property Score As Double
        Public Property Position As Integer
    End Class


    ' ----- FORM LOAD -----
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Await WebView2Q.EnsureCoreWebView2Async(Nothing)

        Dim htmlFilePath As String = "C:/Users/Wills Weaver/Documents/GitHub/OutspokenTitan8.github.io/wrapper_qualifying_results.html"
        Dim fileUri As String = New Uri(htmlFilePath).AbsoluteUri

        WebView2Q.CoreWebView2.Navigate(fileUri)
        LoadRaceData()
        DisplayNextSessionInfo()
        ButtonStop.Enabled = False
        LabelStatus.Text = "Idle"
        LabelStatus.ForeColor = Color.Black

        Await WebView2R.EnsureCoreWebView2Async(Nothing)

        Dim htmlFilePath2 As String = "C:/Users/Wills Weaver/Documents/GitHub/OutspokenTitan8.github.io/wrapper_race_results.html"
        Dim fileUri2 As String = New Uri(htmlFilePath2).AbsoluteUri

        WebView2R.CoreWebView2.Navigate(fileUri2)
        LoadRaceData()
        DisplayNextSessionInfo()
        ButtonStop.Enabled = False
        LabelStatus.Text = "Idle"
        LabelStatus.ForeColor = Color.Black

        LoadPythonScripts()
        LoadPlayers()
        LoadLeaderboard() ' Load leaderboard before showing it

        If players IsNot Nothing AndAlso players.Count > 0 Then
            LoadRaceDriversAndSetupGuessGrid()
        End If

        WebView2Q.Visible = False
        WebView2R.Visible = False
    End Sub



    Private Sub LoadRaceData()
        Dim jsonPath As String = "C:/Users/Wills Weaver/Documents/GitHub/OutspokenTitan8.github.io/race_data.json" ' Change path if needed
        Dim jsonString As String = File.ReadAllText(jsonPath)

        seasonData = JsonConvert.DeserializeObject(Of SeasonData)(jsonString)
    End Sub

    ' Parse session time like "00:30 BST" + race date "2025-03-16" into UTC DateTime
    Private Function ParseSessionDateTime(raceDate As String, sessionTimeStr As String) As DateTime?
        Try
            Dim timePart As String = sessionTimeStr.Split(" "c)(0) ' e.g. "00:30"
            Dim sessionTime As DateTime = DateTime.ParseExact(timePart, "HH:mm", CultureInfo.InvariantCulture)
            Dim datePart As DateTime = DateTime.ParseExact(raceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
            Dim combined As DateTime = datePart.Date.AddHours(sessionTime.Hour).AddMinutes(sessionTime.Minute)
            Dim combinedUtc As DateTime = combined.AddHours(-1) ' BST to UTC
            Return combinedUtc
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Sub DisplayNextSessionInfo()
        Dim nowUtc As DateTime = DateTime.UtcNow

        Dim nextRace As Race = Nothing
        Dim nextSessionName As String = Nothing
        Dim nextSessionTime As DateTime = DateTime.MaxValue

        For Each race In seasonData.races
            Dim sessions = New Dictionary(Of String, String) From {
            {"FP1", race.sessions.FP1},
            {"FP2", race.sessions.FP2},
            {"FP3", race.sessions.FP3},
            {"Qualifying", race.sessions.Qualifying},
            {"Race", race.sessions.Race}
        }

            If race.IsSprint AndAlso Not String.IsNullOrEmpty(race.SprintTime) Then
                sessions.Add("Sprint", race.SprintTime)
            End If

            For Each kvp In sessions
                Dim sessionName = kvp.Key
                Dim sessionTimeStr = kvp.Value

                Dim sessionDateTimeUtc = ParseSessionDateTime(race.racedate, sessionTimeStr)

                If sessionDateTimeUtc.HasValue Then
                    '     AppendMessage($"Parsed datetime: {sessionDateTimeUtc.Value} UTC (now is {nowUtc})")
                Else
                    '     AppendMessage($"Failed to parse session time for {sessionName} in race {race.name}")
                End If

                If sessionDateTimeUtc.HasValue AndAlso sessionDateTimeUtc.Value > nowUtc Then
                    If sessionDateTimeUtc.Value < nextSessionTime Then
                        nextSessionTime = sessionDateTimeUtc.Value
                        nextRace = race
                        nextSessionName = sessionName
                    End If
                End If
            Next
        Next

        If nextRace Is Nothing Then
            AppendMessage($"No Sessions Found")
            Return
        End If

        ' Load track image
        Try
            Dim request As WebRequest = WebRequest.Create(nextRace.track_image_url)
            Using response As WebResponse = request.GetResponse()
                Using stream As Stream = response.GetResponseStream()
                    PictureBoxTrack.Image = System.Drawing.Image.FromStream(stream)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load track image: " & ex.Message)
        End Try

        ' Build and display race info text
        Dim infoText As New System.Text.StringBuilder()
        infoText.AppendLine($"Next Session: {nextSessionName}")
        infoText.AppendLine($"Round {nextRace.round}: {nextRace.name}")
        infoText.AppendLine($"Circuit: {nextRace.circuit}")
        infoText.AppendLine($"Date: {nextRace.racedate}")

        ' Get session time safely
        Dim sessionValue As String = "N/A"

        If nextSessionName = "Sprint" Then
            If Not String.IsNullOrEmpty(nextRace.SprintTime) Then
                sessionValue = nextRace.SprintTime
            End If
        Else
            Dim propInfo = GetType(Sessions).GetProperty(nextSessionName)
            If propInfo IsNot Nothing Then
                Dim val = propInfo.GetValue(nextRace.sessions)
                If val IsNot Nothing Then
                    sessionValue = val.ToString()
                End If
            End If
        End If

        infoText.AppendLine($"Session Time (BST): {sessionValue}")
        infoText.AppendLine($"Sprint Race: {(If(nextRace.IsSprint, "Yes", "No"))}")

        TextBoxRaceInfo.Text = infoText.ToString()
    End Sub

    Private Sub LoadLeaderboard()
        AppendMessage("Attempting to load leaderboard from: " & leaderboardFile)

        If File.Exists(leaderboardFile) Then
            Try
                Dim json = File.ReadAllText(leaderboardFile)
                leaderboardScores = JsonConvert.DeserializeObject(Of List(Of LeaderboardEntry))(json)
                If leaderboardScores Is Nothing Then
                    leaderboardScores = New List(Of LeaderboardEntry)()
                End If
                AppendMessage("Loaded " & leaderboardScores.Count & " leaderboard entries.")
            Catch ex As Exception
                AppendMessage("Error loading leaderboard: " & ex.Message)
                leaderboardScores = New List(Of LeaderboardEntry)()
            End Try
        Else
            leaderboardScores = New List(Of LeaderboardEntry)()
            AppendMessage("Leaderboard file not found.")
        End If

        ShowLeaderboard() ' <-- Ensure the leaderboard is displayed after loading
    End Sub


    ' ----- LOAD RACE DRIVERS & SETUP GRID -----
    Private Sub LoadRaceDriversAndSetupGuessGrid()
        Dim drivers As List(Of String)

        If File.Exists(raceDataFile) Then
            Try
                Dim json = File.ReadAllText(raceDataFile)
                Dim rawJson = JsonConvert.DeserializeObject(Of JObject)(json)
                Dim positionsDict = rawJson("positions").ToObject(Of Dictionary(Of String, Integer))()
                drivers = positionsDict.Keys.ToList()
            Catch ex As Exception
                AppendMessage("Error reading race results: " & ex.Message)
                drivers = New List(Of String)()
            End Try
        Else
            AppendMessage("Race results file not found. Using default driver list.")
            drivers = New List(Of String) From {"ALB", "ALO", "ANT", "BEA", "BOR", "DOO", "GAS", "HAM", "HAD", "HUL", "LAW", "LEC", "NOR", "OCO", "PIA", "RIC", "RUS", "SAI", "STR", "TSU", "VER"}


        End If

        SetupGuessGrid(drivers)
        LoadGuesses()
    End Sub

    ' ----- SETUP GUESS DATAGRIDVIEW -----
    Private Sub SetupGuessGrid(drivers As List(Of String))
        DataGridViewGuesses.Columns.Clear()

        Dim playerNames = players.Select(Function(p) p.Name).ToList()

        Dim colPlayer As New DataGridViewComboBoxColumn() With {
        .HeaderText = "Player",
        .DataPropertyName = "PlayerName",
        .DataSource = playerNames,
        .Width = 100,
        .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
        .ReadOnly = True
    }
        DataGridViewGuesses.Columns.Add(colPlayer)

        Dim col10th As New DataGridViewComboBoxColumn() With {
        .HeaderText = "10th Place Pick",
        .DataPropertyName = "Pick10th",
        .DataSource = drivers,
        .Width = 120,
        .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
    }
        DataGridViewGuesses.Columns.Add(col10th)

        Dim colDNF As New DataGridViewComboBoxColumn() With {
        .HeaderText = "DNF Pick",
        .DataPropertyName = "PickDNF",
        .DataSource = drivers,
        .Width = 120,
        .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
    }
        DataGridViewGuesses.Columns.Add(colDNF)

        guessesBindingList = New BindingList(Of Guess)(players.Select(Function(p) New Guess With {
        .PlayerName = p.Name,
        .Pick10th = "",
        .PickDNF = ""
    }).ToList())

        ' Double Points checkbox
        Dim colDoublePoints As New DataGridViewCheckBoxColumn() With {
        .HeaderText = "Use Double Points",
        .DataPropertyName = "UsedDoublePoints",
        .Width = 100
    }
        DataGridViewGuesses.Columns.Add(colDoublePoints)

        ' FU checkbox
        Dim colFU As New DataGridViewCheckBoxColumn() With {
        .HeaderText = "Use FU Card",
        .DataPropertyName = "UsedFU",
        .Width = 80
    }
        DataGridViewGuesses.Columns.Add(colFU)

        ' FU Target combobox (select which player to FU)
        Dim colFUTarget As New DataGridViewComboBoxColumn() With {
        .HeaderText = "FU Target Player",
        .DataPropertyName = "FUTargetPlayer",
        .DataSource = players.Select(Function(p) p.Name).ToList(),
        .Width = 120
    }
        DataGridViewGuesses.Columns.Add(colFUTarget)

        With DataGridViewGuesses
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .DefaultCellStyle.WrapMode = DataGridViewTriState.True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .DataSource = guessesBindingList
            .AutoResizeRows()
        End With

        DataGridViewGuesses.DataSource = guessesBindingList
        DataGridViewGuesses.AllowUserToAddRows = False
        DataGridViewGuesses.AllowUserToDeleteRows = False

        ' --- Ensure dropdown opens on first click ---
        RemoveHandler DataGridViewGuesses.CellClick, AddressOf DataGridViewGuesses_CellClick
        AddHandler DataGridViewGuesses.CellClick, AddressOf DataGridViewGuesses_CellClick
    End Sub

    ' Add this handler to your Form1 class:
    Private Sub DataGridViewGuesses_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim dgv = CType(sender, DataGridView)
            Dim col = dgv.Columns(e.ColumnIndex)
            If TypeOf col Is DataGridViewComboBoxColumn AndAlso Not col.ReadOnly Then
                dgv.BeginEdit(True)
                Dim cb = TryCast(dgv.EditingControl, ComboBox)
                If cb IsNot Nothing Then
                    cb.DroppedDown = True
                End If
            End If
        End If
    End Sub

    Private Sub ExportGuessesToJson(filePath As String)
        Try
            ' Serialize the binding list directly
            Dim json As String = JsonConvert.SerializeObject(guessesBindingList, Formatting.Indented)

            File.WriteAllText(filePath, json)

            AppendOutput($"Guesses exported to JSON at: {filePath}")
        Catch ex As Exception
            AppendOutput($"Error exporting to JSON: {ex.Message}")
        End Try
    End Sub

    ' Helper method to safely append text to RichTextBoxOutput from any thread
    Private Sub AppendOutput(text As String)
        If RichTextBoxOutput.InvokeRequired Then
            RichTextBoxOutput.Invoke(Sub() AppendOutput(text))
        Else
            RichTextBoxOutput.AppendText(text & Environment.NewLine)
            RichTextBoxOutput.ScrollToCaret()
        End If
    End Sub


    ' ----- LOAD & SAVE PLAYERS -----
    Private Sub LoadPlayers()
        If File.Exists(playersFile) Then
            Dim json = File.ReadAllText(playersFile)
            players = JsonConvert.DeserializeObject(Of List(Of Player))(json)
        Else
            players = New List(Of Player) From {
                New Player With {.Name = "Alice"},
                New Player With {.Name = "Bob"},
                New Player With {.Name = "Charlie"}
            }
            SavePlayers()
        End If
    End Sub

    Private Sub SavePlayers()
        Dim json = JsonConvert.SerializeObject(players, Formatting.Indented)
        File.WriteAllText(playersFile, json)
    End Sub

    ' ----- LOAD & SAVE GUESSES -----
    Private Sub LoadGuesses()
        If File.Exists(guessesFile) Then
            Dim json = File.ReadAllText(guessesFile)
            guesses = JsonConvert.DeserializeObject(Of List(Of Guess))(json)

            For Each guess In guesses
                Dim match = guessesBindingList.FirstOrDefault(Function(g) g.PlayerName = guess.PlayerName)
                If match IsNot Nothing Then
                    match.Pick10th = guess.Pick10th
                    match.PickDNF = guess.PickDNF
                    match.UsedDoublePoints = guess.UsedDoublePoints
                    match.UsedFU = guess.UsedFU
                    match.FUTargetPlayer = guess.FUTargetPlayer
                End If
            Next

            DataGridViewGuesses.Refresh()
        Else
            guesses = New List(Of Guess)
        End If
    End Sub

    Private Sub SaveGuesses()
        guesses = guessesBindingList.ToList()
        Dim json = JsonConvert.SerializeObject(guesses, Formatting.Indented)
        File.WriteAllText(guessesFile, json)
        AppendMessage("Guesses saved!")
    End Sub

    ' ----- BUTTON HANDLERS FOR GUESSES -----
    Private Sub ButtonSaveGuesses_Click(sender As Object, e As EventArgs) Handles ButtonSaveGuesses.Click
        SaveGuesses()
    End Sub

    Private Sub ButtonLoadPlayers_Click(sender As Object, e As EventArgs) Handles ButtonLoadPlayers.Click
        LoadPlayers()
        AppendMessage("Players loaded: " & String.Join(", ", players.Select(Function(p) p.Name)))
        LoadRaceDriversAndSetupGuessGrid()
        DataGridViewGuesses.Refresh()
    End Sub

    Private Sub ButtonLoadGuesses_Click(sender As Object, e As EventArgs) Handles ButtonLoadGuesses.Click
        LoadGuesses()
        AppendMessage("Guesses loaded.")
    End Sub

    Private Sub ButtonClearGuesses_Click(sender As Object, e As EventArgs) Handles ButtonClearGuesses.Click
        If MessageBox.Show("Are you sure you want to clear all guesses?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            ClearAllGuesses()
        End If
    End Sub

    Private Sub ClearAllGuesses()
        For Each guess As Guess In guessesBindingList
            guess.Pick10th = ""
            guess.PickDNF = ""
            guess.UsedDoublePoints = False
            guess.UsedFU = False
            guess.FUTargetPlayer = ""
        Next

        DataGridViewGuesses.Refresh()
        SaveGuesses()
        AppendMessage("All guesses cleared and saved.")
    End Sub

    ' ----- SCORE CALCULATION -----
    Private Sub ButtonCalculateScores_Click(sender As Object, e As EventArgs) Handles ButtonCalculateScores.Click
        CalculateAllScores()
    End Sub

    Private Sub CalculateAllScores()
        If Not File.Exists(raceDataFile) Then
            AppendMessage("Race results file not found. Cannot calculate scores.")
            Return
        End If

        ' Read race results
        Dim json = File.ReadAllText(raceDataFile)
        Dim rawJson = JsonConvert.DeserializeObject(Of JObject)(json)
        Dim positionsDict = rawJson("positions").ToObject(Of Dictionary(Of String, Integer))()
        Dim retirementsList = rawJson("dnfs").ToObject(Of List(Of String))()

        ' Calculate score for each guess
        For Each guess In guessesBindingList
            Dim score As Integer = 0

            ' 10th place pick score
            If positionsDict.ContainsKey(guess.Pick10th) Then
                Dim actualPos = positionsDict(guess.Pick10th)
                Dim points = Math.Max(0, 10 - Math.Abs(actualPos - 10))
                score += points
            End If

            ' DNF pick score
            Dim dnfIndex As Integer = retirementsList.IndexOf(guess.PickDNF)
            If dnfIndex >= 0 Then
                ' Example: 1st DNF gets 15, 2nd gets 10, 3rd gets 5, others get 2
                Dim dnfPoints() As Integer = {10, 5, 4, 3, 2, 1, 0}
                If dnfIndex < dnfPoints.Length Then
                    score += dnfPoints(dnfIndex)
                Else
                    score += 2
                End If
            End If

            ' Apply Double Points
            If guess.UsedDoublePoints Then
                score *= 2
            End If

            guess.Score = score
        Next

        ' Apply FU cards to half points from the target player's score
        For Each guess In guessesBindingList
            If guess.UsedFU AndAlso Not String.IsNullOrEmpty(guess.FUTargetPlayer) Then
                Dim targetGuess = guessesBindingList.FirstOrDefault(Function(g) g.PlayerName = guess.FUTargetPlayer)
                If targetGuess IsNot Nothing Then
                    targetGuess.Score /= 2
                End If
            End If
        Next

        DataGridViewGuesses.Refresh()
        AppendMessage("Scores calculated.")

    End Sub


    ' ----- APPEND MESSAGE TO TEXTBOX -----

    Private Sub AppendMessage(msg As String)
        If RichTextBoxOutput.InvokeRequired Then
            RichTextBoxOutput.Invoke(Sub() AppendMessage(msg))
        Else
            RichTextBoxOutput.AppendText(msg & Environment.NewLine)
        End If
    End Sub




    Private Sub RunPythonScript(scriptName As String)
        If runningProcess IsNot Nothing AndAlso Not runningProcess.HasExited Then
            AppendMessage("A script is already running.")
            Return
        End If

        Dim scriptPath = Path.Combine(scriptFolder, scriptName)
        If Not File.Exists(scriptPath) Then
            AppendMessage($"Script not found: {scriptPath}")
            Return
        End If

        runningProcess = New Process()
        runningProcess.StartInfo.FileName = "python"
        runningProcess.StartInfo.Arguments = $"""{scriptPath}"""
        runningProcess.StartInfo.RedirectStandardOutput = True
        runningProcess.StartInfo.RedirectStandardError = True
        runningProcess.StartInfo.UseShellExecute = False
        runningProcess.StartInfo.CreateNoWindow = True

        AddHandler runningProcess.OutputDataReceived, AddressOf Process_OutputDataReceived
        AddHandler runningProcess.ErrorDataReceived, AddressOf Process_ErrorDataReceived

        Try
            runningProcess.Start()
            runningProcess.BeginOutputReadLine()
            runningProcess.BeginErrorReadLine()

            ButtonRun.Enabled = False
            ButtonStop.Enabled = True
            LabelStatus.Text = "Running script: " & scriptName
            LabelStatus.ForeColor = Color.Green
        Catch ex As Exception
            AppendMessage("Error starting script: " & ex.Message)
        End Try
    End Sub

    Private Sub Process_OutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(e.Data) Then
            AppendMessage(e.Data)
        End If
    End Sub

    Private Sub Process_ErrorDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(e.Data) Then
            AppendMessage("ERROR: " & e.Data)
        End If
    End Sub

    Private Sub ButtonStop_Click(sender As Object, e As EventArgs) Handles ButtonStop.Click
        If runningProcess IsNot Nothing AndAlso Not runningProcess.HasExited Then
            runningProcess.Kill()
            AppendMessage("Script stopped by user.")
        End If

        ButtonRun.Enabled = True
        ButtonStop.Enabled = False
        LabelStatus.Text = "Idle"
        LabelStatus.ForeColor = Color.Black
    End Sub


    Private Sub LoadPythonScripts()
        ListBoxScripts.Items.Clear()
        If Directory.Exists(scriptFolder) Then
            Dim scripts = Directory.GetFiles(scriptFolder, "*.py").Select(Function(f) Path.GetFileName(f)).ToList()
            For Each script In scripts
                ListBoxScripts.Items.Add(script) ' Add string directly
            Next
        Else
            AppendMessage("Scripts folder not found: " & scriptFolder)
        End If
    End Sub


    ' Then, in your Run button handler:
    Private Sub ButtonRun_Click(sender As Object, e As EventArgs) Handles ButtonRun.Click
        If ListBoxScripts.SelectedItem Is Nothing Then
            AppendMessage("Please select a script to run.")
            Return
        End If
        Dim scriptName = Path.GetFileName(ListBoxScripts.SelectedItem.ToString())
        RunPythonScript(scriptName)
    End Sub


    ' ------------------- LEADERBOARD LOGIC -------------------


    Private Sub SaveLeaderboard()
        ' Build leaderboard dictionary from in-memory leaderboardScores
        Dim leaderboardDict As New Dictionary(Of String, Double)
        If leaderboardScores IsNot Nothing Then
            For Each entry In leaderboardScores
                leaderboardDict(entry.PlayerName) = entry.Score
            Next
        End If

        ' Create a sorted list descending by score
        Dim updatedList = leaderboardDict.Select(Function(kvp) New LeaderboardEntry With {
        .PlayerName = kvp.Key,
        .Score = kvp.Value
    }).OrderByDescending(Function(entry) entry.Score).ToList()

        ' Add Position property to each entry
        Dim position As Integer = 1
        For Each entry In updatedList
            entry.Position = position
            position += 1
        Next

        ' Serialize and save the updated leaderboard including positions
        Dim updatedJson = JsonConvert.SerializeObject(updatedList, Formatting.Indented)
        File.WriteAllText(leaderboardFile, updatedJson)

        AppendMessage("Scores added to leaderboard.")
        LoadLeaderboard() ' Refresh in-memory leaderboard

        ' Sort descending by score
        leaderboardScores = leaderboardScores.OrderByDescending(Function(entry) entry.Score).ToList()

        ' Ensure the directory exists
        Dim dir = Path.GetDirectoryName(leaderboardFile)
        If Not Directory.Exists(dir) Then
            Directory.CreateDirectory(dir)
        End If

        Dim json = JsonConvert.SerializeObject(leaderboardScores, Formatting.Indented)
        File.WriteAllText(leaderboardFile, json)
    End Sub




    Private Sub ShowLeaderboard()
        If leaderboardScores Is Nothing OrElse leaderboardScores.Count = 0 Then
            AppendMessage("No leaderboard data to display.")
            DataGridViewLeaderboard.DataSource = Nothing
            Return
        End If

        ' Sort by score descending and assign position
        Dim sortedList = leaderboardScores.OrderByDescending(Function(e) e.Score).ToList()
        For i As Integer = 0 To sortedList.Count - 1
            sortedList(i).Position = i + 1
        Next

        DataGridViewLeaderboard.Columns.Clear()
        DataGridViewLeaderboard.AutoGenerateColumns = False

        ' Position column
        Dim colPosition As New DataGridViewTextBoxColumn() With {
        .HeaderText = "Position",
        .DataPropertyName = "Position",
        .ReadOnly = True,
        .Width = 60
    }
        DataGridViewLeaderboard.Columns.Add(colPosition)

        ' Player column
        Dim colPlayer As New DataGridViewTextBoxColumn() With {
        .HeaderText = "Player",
        .DataPropertyName = "PlayerName",
        .ReadOnly = True,
        .Width = 150
    }
        DataGridViewLeaderboard.Columns.Add(colPlayer)

        ' Score column
        Dim colScore As New DataGridViewTextBoxColumn() With {
        .HeaderText = "Score",
        .DataPropertyName = "Score",
        .ReadOnly = True,
        .Width = 70
    }
        DataGridViewLeaderboard.Columns.Add(colScore)

        DataGridViewLeaderboard.DataSource = New BindingList(Of LeaderboardEntry)(sortedList)
        DataGridViewLeaderboard.Refresh()
    End Sub


    Private Sub btnExportJson_Click(sender As Object, e As EventArgs) Handles btnExportJson.Click
        Dim folder As String = "C:/Users/Wills Weaver/Documents/GitHub/OutspokenTitan8.github.io"
        If Not IO.Directory.Exists(folder) Then
            IO.Directory.CreateDirectory(folder)
        End If

        Dim fixedPath As String = IO.Path.Combine(folder, "guesses_export.json")
        ExportGuessesToJson(fixedPath)
    End Sub



    Private Sub ButtonAddScoresToLeaderboard_Click(sender As Object, e As EventArgs) Handles ButtonAddScoresToLeaderboard.Click
        ' Load existing leaderboard into a dictionary
        Dim leaderboardDict As New Dictionary(Of String, Double)
        If File.Exists(leaderboardFile) Then
            Dim json = File.ReadAllText(leaderboardFile)
            Dim entries = JsonConvert.DeserializeObject(Of List(Of LeaderboardEntry))(json)
            For Each entry In entries
                leaderboardDict(entry.PlayerName) = entry.Score
            Next
        End If

        ' Add current scores
        For Each guess In guessesBindingList
            If leaderboardDict.ContainsKey(guess.PlayerName) Then
                leaderboardDict(guess.PlayerName) += guess.Score
            Else
                leaderboardDict(guess.PlayerName) = guess.Score
            End If
        Next

        ' Update leaderboardScores and show in DataGridView
        leaderboardScores = leaderboardDict.Select(Function(kvp) New LeaderboardEntry With {
        .PlayerName = kvp.Key,
        .Score = kvp.Value
    }).OrderByDescending(Function(entry) entry.Score).ToList()

        ShowLeaderboard()
    End Sub

    Private Sub ButtonSaveLeaderboard_Click(sender As Object, e As EventArgs) Handles ButtonSaveLeaderboard.Click
        SaveLeaderboard()
        AppendMessage("Leaderboard saved.")
    End Sub

    Private Sub ButtonOpenGitHubDesktop_Click(sender As Object, e As EventArgs) Handles ButtonOpenGitHubDesktop.Click
        Dim githubDesktopPath As String = "C:\Users\Wills Weaver\AppData\Local\GitHubDesktop\GitHubDesktop.exe"
        Try
            If IO.File.Exists(githubDesktopPath) Then
                Process.Start(githubDesktopPath)
            Else
                AppendMessage("GitHub Desktop not found at: " & githubDesktopPath)
            End If
        Catch ex As Exception
            AppendMessage("Failed to open GitHub Desktop: " & ex.Message)
        End Try
    End Sub

    Private Sub ButtonClearOutput_Click(sender As Object, e As EventArgs) Handles ButtonClearOutput.Click
        RichTextBoxOutput.Clear()
    End Sub

    Private Sub ButtonShowQ_Click(sender As Object, e As EventArgs) Handles ButtonShowQ.Click
        WebView2Q.Visible = True
        WebView2R.Visible = False
    End Sub

    Private Sub ButtonShowR_Click(sender As Object, e As EventArgs) Handles ButtonShowR.Click
        WebView2Q.Visible = False
        WebView2R.Visible = True
    End Sub

End Class

' ----- DATA CLASSES -----
Public Class Player
    Public Property Name As String
End Class

Public Class Guess
    Public Property PlayerName As String
    Public Property Pick10th As String
    Public Property PickDNF As String
    Public Property UsedDoublePoints As Boolean
    Public Property UsedFU As Boolean
    Public Property FUTargetPlayer As String
    Public Property Score As Double = 0
End Class


Public Class SeasonData
    Public Property season As Integer
    Public Property races As List(Of Race)
End Class

Public Class Race
    Public Property round As Integer
    Public Property name As String
    Public Property circuit As String
    Public Property racedate As String
    Public Property sessions As Sessions

    <JsonProperty("sprint")>
    Public Property IsSprint As Boolean

    <JsonProperty("Sprint")>
    Public Property SprintTime As String ' optional sprint session time

    Public Property track_image_url As String
End Class

Public Class Sessions
    Public Property FP1 As String
    Public Property FP2 As String
    Public Property FP3 As String
    Public Property Qualifying As String
    Public Property Race As String
End Class

