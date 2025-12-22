using Addams.Components;
using Addams.Core;
using Addams.Core.Exceptions;
using Addams.Core.Logs;
using Addams.Core.Utils;
using Newtonsoft.Json;
using TABS = Addams.Tabs.TabIndex;

namespace Addams.Tabs;

internal class SettingsTab : AbstractTab
{
    private readonly TextBox UserTextBox = new();
    private readonly TextBox ClientIdTextBox = new();
    private readonly TextBox ClientSecretTextBox = new();
    private readonly RichTextBox TokenRichTextBox = new();
    private readonly ComboBox LanguageCb = new();
    private readonly Switch SwitchDebug = new();
    
    private readonly Button SaveSettingsButton = new();
    private readonly Button ReadSettingsButton = new();
    private readonly Button AuthenticateButton = new(); 

    private readonly CancellationTokenSource _cts;

    private readonly AddamsView View;

    public SettingsTab() { }

    internal SettingsTab(AddamsConfig configuration, TabPage tab, AddamsView view, CancellationTokenSource cts)
        : base(configuration)
    {
        View = view;
        _cts = cts;

        if (tab.Controls["userTb"] is TextBox userTb)
            UserTextBox = userTb;

        if (tab.Controls["clientIdTb"] is TextBox clientIdTb)
            ClientIdTextBox = clientIdTb;

        if (tab.Controls["clientSecretTb"] is TextBox clientSecretTb)
            ClientSecretTextBox = clientSecretTb;

        if (tab.Controls["TokenRtb"] is RichTextBox tokenRtb)
            TokenRichTextBox = tokenRtb;

        if (tab.Controls["languageCb"] is ComboBox languageCb)
            LanguageCb = languageCb;

        if (tab.Controls["saveSettingsButton"] is Button saveSettingsButton)
            SaveSettingsButton = saveSettingsButton;
        SaveSettingsButton.Click += SaveSettings_Click;
        
        if (tab.Controls["readSettingButton"] is Button readSettingButton)
            ReadSettingsButton = readSettingButton;
        ReadSettingsButton.Click += ReadSettingButton_Click;

        
        if (tab.Controls["tryAuthenticateButton"] is Button authenticationSettings)
            AuthenticateButton = authenticationSettings;
        AuthenticateButton.Click += TryAuthenticateButton_Click;

        if (tab.Controls["DebugSwitch"] is Switch debugSwitch)
            SwitchDebug = debugSwitch;
        SwitchDebug.Click += DebugSwitch_Click;
        SwitchDebug.Checked = Configuration.IsDebugMode;

        Load();
    }

    internal void Load()
    {
        var cfg = AddamsCore.GetConfiguration();
        UserTextBox.Text = cfg.UserName;
        ClientIdTextBox.Text = cfg.ClientID;
        ClientSecretTextBox.Text = cfg.ClientSecret;
        TokenRichTextBox.Text = JsonConvert.SerializeObject(cfg, Formatting.Indented);

        LanguageCb.DisplayMember = "Display";
        LanguageCb.ValueMember = "Value";
        SetComboLanguage();

        LanguageCb.SelectedValue = cfg.AppLanguage;
        LanguageCb.SelectedIndexChanged += ChangeLanguage;
    }

    private void Save()
    {
        Configuration.UserName = UserTextBox.Text;
        Configuration.ClientID = ClientIdTextBox.Text;
        Configuration.ClientSecret = ClientSecretTextBox.Text;

        Configuration.Save();
        Load();
    }

    private void SetComboLanguage()
    {
        var languages = new List<ComboItem> {
                new(Language.GetString("Language_EN"), "en-US"),
                new(Language.GetString("Language_FR"), "fr-FR"),
                new(Language.GetString("Language_ES"), "es-ES"),
                new(Language.GetString("Language_DE"), "de-DE"),
                new(Language.GetString("Language_IT"), "it-IT"),
            };
        LanguageCb.DataSource = languages;
    }

    private void ChangeLanguage(object sender, EventArgs e)
    {
        var comboBox = sender as ComboBox ?? new ComboBox();
        if (comboBox.SelectedItem == null)
            return;

        var item = comboBox.SelectedItem as ComboItem ?? throw new ComboBoxException();
        var culture = item.Value;

        Configuration.AppLanguage = culture;
        Configuration.Save();

        View.ApplyTexts();
    }

    private void SaveSettings_Click(object sender, EventArgs e)
    {
        Save();
    }

    private void ReadSettingButton_Click(object sender, EventArgs e)
    {
        AddamsCore.OpenConfiguration();
        Configuration = AddamsCore.GetConfiguration();
    }

    private async void TryAuthenticateButton_Click(object sender, EventArgs e)
    {
        Save();
        LoggerManager.Log(Language.GetString("String78"));
        await AddamsCore.GenerateTokenAsync(_cts);
        View.IsAuthentified = true;
        LoggerManager.Log(Language.GetString("String79"), Level.Ok);
        View.tabControl1.SelectedIndex = TABS.EXPORT;
    }

    private void DebugSwitch_Click(object sender, EventArgs e)
    {
        Configuration.IsDebugMode = !Configuration.IsDebugMode;
        Configuration.Save();
        LoggerManager.Log(Language.GetString("String15"), Level.Debug);
    }
}

internal class ComboItem
{
    public string? Display { get; set; }
    public string? Value { get; set; }

    public ComboItem(string display, string value)
    {
        Display = display;
        Value = value;
    }
}
