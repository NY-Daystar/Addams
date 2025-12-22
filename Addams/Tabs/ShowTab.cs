using Addams.Core;
using Addams.Core.Utils;
using System.Diagnostics;

namespace Addams.Tabs;

internal class ShowTab : AbstractTab
{
    private readonly ListView PlayListView = new();
    private readonly Button OpenFolderButton = new();

    public ShowTab() { }

    internal ShowTab(AddamsConfig configuration, TabPage tab)
        : base(configuration)
    {
        if (tab.Controls["listView1"] is ListView lv)
            PlayListView = lv;

        if (tab.Controls["openFolderButton"] is Button btn)
            OpenFolderButton = btn;

        PlayListView.SelectedIndexChanged += ListView1_DoubleClick;
        OpenFolderButton.Click += GoToPlaylist_Click;
    }

    internal void LoadPlaylistsSaved()
    {
        InitView();

        PlayListView.Items.Clear();

        IEnumerable<string> files = AddamsCore.ShowPlaylists();
        foreach (var file in files)
        {
            var info = new FileInfo(file);

            var item = new ListViewItem(info.Name);
            item.SubItems.Add(info.Extension);
            item.SubItems.Add((info.Length / 1024).ToString("N0"));
            item.SubItems.Add(info.LastWriteTime.ToString("g"));
            item.Tag = info.FullName;

            PlayListView.Items.Add(item);
        }
    }

    internal void OpenPlaylist()
    {
        if (PlayListView.SelectedItems.Count == 0)
            return;

        string path = PlayListView.SelectedItems[0].Tag!.ToString()!;

        Process.Start(new ProcessStartInfo(path)
        {
            UseShellExecute = true
        });
    }

    private void InitView()
    {
        List<string> columns =
        [
            Language.GetString("UI_ColumnName"),
            Language.GetString("UI_ColumnExtension"),
            Language.GetString("UI_ColumnSize"),
            Language.GetString("UI_ColumnModifiedAt"),
        ];

        for (var i = 0; i < PlayListView.Columns.Count; i++)
            PlayListView.Columns[i].Text = columns[i];
    }

    private void ListView1_DoubleClick(object sender, EventArgs e)
    {
        OpenPlaylist();
    }
    private void GoToPlaylist_Click(object sender, EventArgs e)
    {
        AddamsCore.GoToPlaylists();
    }
}
