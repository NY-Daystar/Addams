using Addams.Core;
using Addams.Core.Exceptions;
using Addams.Core.Logs;
using Addams.Core.Utils;
using Addams.Manager;
using Addams.Tabs;
using System.Globalization;
using LM = Addams.Core.Logs.LoggerManager;
using TABS = Addams.Tabs.TabIndex;

namespace Addams;

public partial class AddamsView : Form
{
    private bool _tabSettingsLoaded = false;
    private bool _tabExportLoaded = false;

    private AddamsConfig Configuration = new();

    private ExportTab Exporter = new();
    private ShowTab Shower = new();
    private SettingsTab Settings = new();

    private ThemeManager? ThemeManager;

    public bool IsAuthentified;

    private readonly CancellationTokenSource _cts = new();

    public AddamsView()
    {
        InitializeComponent();
        SwitchUpdate();
        LoadApplication();
    }

    private async Task LoadApplication()
    {
        Text = AddamsCore.Application;
        titleLabel.Text = AddamsCore.Application;
        versionLabel.Text = $"{Language.GetString("Version")} v{AddamsCore.Version}";

        LM.OnLog += DisplayLog;

        bool redirectToSettings = false;
        try
        {
            AddamsCore.Launch();
            Configuration = AddamsCore.GetConfiguration();
            SetCulture();
            LM.Log(Language.GetString("String80"));

            ApplyTexts();

            var authStatus = await AddamsCore.CheckTokenAsync(_cts);
            var retry = authStatus.Exception?.RetryAfter;
            if (retry?.TotalSeconds > 0)
            {
                MessageBox.Show(string.Format(Language.GetString("String16"), retry?.ToString()));
                redirectToSettings = true;
            }
            else if (!authStatus.Status)
            {
                LM.Log(Language.GetString("String78"));
                if (AddamsCore.CanGenerateToken())
                {
                    //await Task.Run(() => AddamsCore.GenerateTokenAsync(_cts));
                    await AddamsCore.GenerateTokenAsync(_cts);
                    IsAuthentified = true;
                    LM.Log(Language.GetString("String79"), Level.Ok);
                }
                else
                {
                    LM.Log(Language.GetString("String1"), Level.Warning);
                    redirectToSettings = true;
                }
            }
        }
        catch (SpotifyConfigException ex)
        {
            LM.Log(ex.Message, Level.Error);
            tabControl1.SelectedIndex = TABS.SETTINGS;
            MessageBox.Show(Language.GetString("String75"), Language.GetString("String76"));
        }
        catch (SpotifyUnauthorizedException ex)
        {
            LM.Log(ex.Message, Level.Error);
            MessageBox.Show(Language.GetString("String75"), Language.GetString("String76"));
        }
        catch (SpotifyException ex)
        {
            LM.Log($"{Language.GetString("String74")} | " +
                $"{ex.GetType().FullName} - {ex.Message}", Level.Error);
        }
        catch (Exception ex)
        {
            redirectToSettings = true;
            LM.Log($"{ex.GetType().FullName} - {ex.Message}", Level.Error);
        }
        finally
        {
            Exporter = new ExportTab(Configuration, tabControl1.TabPages[0]);
            Shower = new ShowTab(Configuration, tabControl1.TabPages[1]);
            Settings = new SettingsTab(Configuration, tabControl1.TabPages[2], this, _cts);
            ThemeManager = new ThemeManager(this, Configuration);

            if (redirectToSettings)
                tabControl1.SelectedIndex = TABS.SETTINGS;
            else
            {
                await Exporter.InitPlaylistView();
            }
        }
    }

    private async void TabControl1_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (tabControl1.SelectedIndex == TABS.EXPORT && !_tabExportLoaded && IsAuthentified)
        {
            _tabExportLoaded = true;
            var check = await Exporter.InitPlaylistView();
            if (!check)
            {
                IsAuthentified = false;
                LM.Log(Language.GetString("String14"), Level.Important);
                Configuration.Reset();
                tabControl1.SelectedIndex = TABS.SETTINGS;
            }
            _tabExportLoaded = false;
        }

        if (tabControl1.SelectedIndex == TABS.SHOW)
        {
            Shower.LoadPlaylistsSaved();
        }

        if (tabControl1.SelectedIndex == TABS.SETTINGS && !_tabSettingsLoaded)
        {
            _tabSettingsLoaded = true;
            Settings.Load();
            _tabSettingsLoaded = false;
        }
    }

    private void ThemeSwitch_Click(object sender, EventArgs e)
    {
        ThemeManager?.Switch(sender, e);
    }

    private void SetCulture()
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(Configuration.AppLanguage);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(Configuration.AppLanguage);
    }

    public void ApplyTexts()
    {
        SetCulture();

        tabPage1.Text = Language.GetString("UI_Tab1");
        tabPage2.Text = Language.GetString("UI_Tab2");
        tabPage3.Text = Language.GetString("UI_Tab3");


        label1.Text = Language.GetString("UI_Label1");
        label4.Text = Language.GetString("UI_Label4");
        label5.Text = Language.GetString("UI_Label5");

        saveSettingsButton.Text = Language.GetString("UI_ButtonSave");
        exportButton.Text = Language.GetString("UI_ButtonExport");
        openFolderButton.Text = Language.GetString("UI_ButtonOpenFolder");
        readSettingButton.Text = Language.GetString("UI_ButtonReadSettings");
        tryAuthenticateButton.Text = Language.GetString("UI_ButtonTryAuthenticate");
    }

    private void DisplayLog(LogEntry entry)
    {
        logRt.BackColor = Color.PeachPuff;
        if (InvokeRequired)
        {
            Invoke(new Action(() => DisplayLog(entry)));
            return;
        }

        logRt.SelectionColor = entry.Color;

        if (entry.Level.Equals(Level.Debug) && !Configuration.IsDebugMode)
            return;

        logRt.AppendText(entry + Environment.NewLine);
        logRt.ScrollToCaret();
    }

    private void SwitchUpdate()
    {
        ThemeSwitch.UpdateColor();
        DebugSwitch.UpdateColor();
    }
}
