using Addams.Core;
using Addams.Core.Logs;
using Addams.Core.Entities;
using Addams.Core.Utils;
using Addams.Core.Exceptions;

namespace Addams.Tabs;

internal class ExportTab : AbstractTab
{
    private readonly ListView PlayListView = new();
    private readonly Button ExportButton = new();

    public ExportTab() { }

    internal ExportTab(AddamsConfig configuration, TabPage tab)
        : base(configuration)
    {
        if (tab.Controls["listView2"] is ListView lv)
            PlayListView = lv;

        if (tab.Controls["exportButton"] is Button btn)
            ExportButton = btn;

        ExportButton.Click += ExportButton_Click;
    }

    internal async Task<bool> InitPlaylistView()
    {
        List<string> columns = [(Language.GetString("UI_ColumnName"))];

        for (var i = 0; i < PlayListView.Columns.Count; i++)
            PlayListView.Columns[i].Text = columns[i];

        return await LoadPlaylists();
    }

    private async void ExportButton_Click(object sender, EventArgs e)
    {
        await ExportAsync();
    }

    internal async Task ExportAsync()
    {
        var allPlaylists = PlayListView.Items.Cast<ListViewItem>().ToList();
        var selected = allPlaylists.Where(item => item.Selected).ToList();
        var playlists = selected.Count != 0 ? selected : allPlaylists;

        LoggerManager.Log(Language.GetString("String71"));

        try
        {
            await AddamsCore.ExportAsync([.. playlists.Select(item => item.Text)]);
            MessageBox.Show(Language.GetString("String72"));
            LoggerManager.Log(Language.GetString("String72"), Level.Ok);
        }
        catch (Exception ex)
        {
            LoggerManager.Log($"{ex.GetType().FullName} {ex.Message}", Level.Error);
            MessageBox.Show(Language.GetString("String81"));
            LoggerManager.Log(Language.GetString("String81"), Level.Error);
        }
    }

    private async Task<bool> LoadPlaylists()
    {
        LoggerManager.Log($"{Language.GetString("String77")} {Configuration.UserName}");

        IEnumerable<PlaylistEntity> playlists = [];
        try
        {
            playlists = await AddamsCore.GetPlaylistsAsync();
        }
        catch (HttpRequestException)
        {
            MessageBox.Show(Language.GetString("String82"));
            LoggerManager.Log(Language.GetString("String82"), Level.Error);
        }
        catch (SpotifyException ex)
        {
            LoggerManager.Log(Language.GetString("String13"), Level.Error);
            LoggerManager.Log($"{ex.Message} : {ex.StackTrace}", Level.Error);
            return false;
        }

        foreach (var p in playlists)
        {
            var item = new ListViewItem
            {
                Text = p.Name,
                ForeColor = Color.DarkBlue,
                BackColor = Color.LightGray
            };
            PlayListView.Items.Add(item);

        }
        LoggerManager.Log(Language.GetString("String73"), Level.Ok);
        
        return true;
    }
}
