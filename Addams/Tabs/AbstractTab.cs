using Addams.Core;

namespace Addams.Tabs;

/// <summary>
/// Interface to handle each tabs
/// </summary>
internal class AbstractTab
{
    protected AddamsConfig Configuration = new();

    public AbstractTab() { }

    protected AbstractTab(AddamsConfig configuration)
    {
        Configuration = configuration;
    }
}
