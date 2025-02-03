namespace VRCFaceTracking.Avalonia.Models;

public class Module
{
    public string ModuleName { get; set; }
    public string ModuleAuthor { get; set; }

    public Module(string moduleName, string moduleAuthor)
    {
        ModuleName = moduleName;
        ModuleAuthor = moduleAuthor;
    }
}
