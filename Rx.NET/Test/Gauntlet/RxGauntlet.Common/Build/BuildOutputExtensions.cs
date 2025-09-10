namespace RxGauntlet.Build;

public static class BuildOutputExtensions
{
    public static UiFrameworkComponentsInOutput CheckForUiComponentsInOutput(this BuildOutput buildResult)
    {
        bool includesWpf = false;
        bool includesWindowsForms = false;
        foreach (string file in Directory.GetFiles(buildResult.OutputFolder, "*", new EnumerationOptions { RecurseSubdirectories = true }))
        {
            string filename = Path.GetFileName(file);
            if (filename.Equals("PresentationFramework.dll", StringComparison.InvariantCultureIgnoreCase))
            {
                includesWpf = true;
            }

            if (filename.Equals("System.Windows.Forms.dll", StringComparison.InvariantCultureIgnoreCase))
            {
                includesWindowsForms = true;
            }
        }

        return new UiFrameworkComponentsInOutput(
            WpfPresent: includesWpf,
            WindowsFormsPresent: includesWindowsForms);
    }
}
