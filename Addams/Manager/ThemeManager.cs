using Addams.Core;
using Addams.Core.Utils;

namespace Addams.Manager;

public class ThemeManager
{
    private readonly AddamsConfig Config;

    private readonly AddamsView Application;

    public ThemeManager(AddamsView application, AddamsConfig config)
    {
        Application = application;
        Config = config;
        Application.ThemeSwitch.Checked = Convert.ToBoolean(Config.Theme);
        ApplyTheme();
    }

    public void Switch(object sender, EventArgs e)
    {
        Config.Theme = ((CheckBox)sender).Checked ? AddamsTheme.DARK : AddamsTheme.LIGHT;
        Config.Save();
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        if (Config.Theme == AddamsTheme.DARK)
        {
            Application.BackColor = Color.FromArgb(45, 45, 48); // Dark color
            Application.ForeColor = Color.White;

            foreach (Control c in Application.Controls)
            {
                c.BackColor = Color.FromArgb(28, 28, 28);
                c.ForeColor = Color.White;
            }

            foreach (Control tab in Application.tabControl1.TabPages)
            {
                tab.BackColor = Color.FromArgb(28, 28, 28);
                tab.ForeColor = Color.White;
                foreach (Control c in tab.Controls)
                {
                    c.BackColor = Color.FromArgb(28, 28, 28);
                    c.ForeColor = Color.White;
                }
            }
        }
        else
        {
            Application.BackColor = SystemColors.Control; // Light  color
            Application.ForeColor = Color.Black;

            foreach (Control c in Application.Controls)
            {
                c.BackColor = SystemColors.Control;
                c.ForeColor = Color.Black;
            }

            foreach (Control tab in Application.tabControl1.TabPages)
            {
                tab.BackColor = SystemColors.Control;
                tab.ForeColor = Color.Black;
                foreach (Control c in tab.Controls)
                {
                    c.BackColor = SystemColors.Control;
                    c.ForeColor = Color.Black;
                }
            }
        }
    }
}