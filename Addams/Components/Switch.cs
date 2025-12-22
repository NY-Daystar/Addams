using System.Drawing.Drawing2D;

namespace Addams.Components;

/// <summary>
/// Component for winform to create switch button
/// </summary>
public class Switch : CheckBox
{
    /// <summary>
    /// Color of switch when unchecked (Default: White)
    /// </summary>
    private readonly Color ColorUncheck = Color.White;

    /// <summary>
    /// Color of switch when Checked (Default: Black)
    /// </summary>
    private Color ColorCheck = Color.Black;

    public Switch()
    {
        this.Appearance = Appearance.Button;
        this.AutoSize = false;
        this.MinimumSize = new Size(30, 15);
        this.Size = new Size(40, 20);
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0;
        this.BackColor = Color.LightGray;
    }

    /// <summary>
    /// Method to call at the end of view designer
    /// Change ForColor of switch 
    /// </summary>
    public void UpdateColor()
    {
        if (!ForeColor.Name.Equals("ControlText"))
            ColorCheck = ForeColor;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new(0, 0, this.Width - 1, this.Height - 1);
        Color backColor = Checked ? ColorCheck : ColorUncheck;

        using var pen = new Pen(Color.DarkGray, 1);
        using (GraphicsPath path = new())
        {
            int radius = rect.Height; // arrondi = hauteur → pillule
            path.AddArc(rect.X, rect.Y, radius, radius, 90, 180);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 180);
            path.CloseFigure();

            // Appliquer la Region arrondie au contrôle
            this.Region = new Region(path);

            // Dessiner le fond arrondi
            using (SolidBrush brush = new(backColor))
                pevent.Graphics.FillPath(brush, path);

            pevent.Graphics.DrawPath(pen, path);
        }

        // Curseur rond
        int diameter = this.Height - 4;
        int x = this.Checked ? this.Width - diameter - 2 : 2;
        Rectangle knob = new(x, 2, diameter, diameter);

        using (SolidBrush brush = new(Color.White))
            pevent.Graphics.FillEllipse(brush, knob);

        pevent.Graphics.DrawEllipse(pen, knob);
    }
}