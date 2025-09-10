using RxGauntlet.Xml;

using System.Xml;

namespace RxGauntlet.Build;

public class ProjectFileRewriter
{
    private readonly XmlDocument document = new();

    private ProjectFileRewriter(string template)
    {
        document.Load(template);
    }

    public void SetTargetFramework(string targetFrameworkMoniker)
    {
        XmlNode targetFrameworkNode = document.GetRequiredNode("/Project/PropertyGroup/TargetFramework");
        targetFrameworkNode.InnerText = targetFrameworkMoniker;
    }

    public void SetTargetFrameworks(string targetFrameworkMonikerList)
    {
        ReplaceProperty("TargetFrameworks", targetFrameworkMonikerList);
    }

    public void AddAssemblyNameProperty(string assemblyName)
    {
        AddPropertyGroup([new("AssemblyName", assemblyName)]);
    }

    public void ReplaceProperty(string propertyName, string newValue)
    {
        XmlNode propertyNode = document.GetRequiredNode($"/Project/PropertyGroup/{propertyName}");
        propertyNode.InnerText = newValue;
    }

    public void ReplacePackageReference(string packageId, PackageIdAndVersion[] replacementPackages)
    {
        XmlNode packageRefNode = document.GetRequiredNode($"/Project/ItemGroup/PackageReference[@Include='{packageId}']");

        if (replacementPackages is [PackageIdAndVersion singleReplacement])
        {
            packageRefNode.SetAttribute("Include", singleReplacement.PackageId);
            packageRefNode.SetAttribute("Version", singleReplacement.Version);
        }
        else
        {
            // We are to replace a single package reference with multiple package references
            // so we remove the original PackageReference and add new ones.
            ReplaceNodeWithPackageReferences(packageRefNode, replacementPackages);
        }
    }

    public void ReplaceProjectReferenceWithPackageReference(
        string targetCsProjNameWithoutDirectory,
        PackageIdAndVersion[] replacementPackages)
    {
        XmlNode projectRefNode = document.GetRequiredNode($"/Project/ItemGroup/ProjectReference[contains(@Include, '{targetCsProjNameWithoutDirectory}')]");
        ReplaceNodeWithPackageReferences(projectRefNode, replacementPackages);
    }

    public void AddPropertyGroup(IEnumerable<KeyValuePair<string, string>> properties)
    {
        XmlNode propertyGroupNode = document.CreateElement("PropertyGroup");
        foreach ((string name, string value) in properties)
        {
            XmlNode propertyNode = document.CreateElement(name);
            propertyNode.InnerText = value;
            propertyGroupNode.AppendChild(propertyNode);
        }

        document.SelectSingleNode("/Project")!.AppendChild(propertyGroupNode);
    }

    public void AddUseUiFrameworksIfRequired(bool? useWpf, bool? useWindowsForms)
    {
        if (useWpf.HasValue || useWindowsForms.HasValue)
        {
            List<KeyValuePair<string, string>> useUiFrameworksProperties = [];

            if (useWpf.HasValue)
            {
                useUiFrameworksProperties.Add(new("UseWPF", useWpf.Value.ToString()));
            }

            if (useWindowsForms.HasValue)
            {
                useUiFrameworksProperties.Add(new("UseWindowsForms", useWindowsForms.Value.ToString()));
            }

            AddPropertyGroup(useUiFrameworksProperties);
        }
    }

    internal void WriteModified(string destinationPath)
    {
        document.Save(destinationPath);
    }

    private static void ReplaceNodeWithPackageReferences(
        XmlNode nodeToReplace,
        PackageIdAndVersion[] replacementPackages)
    {
        XmlNode packageRefItemGroup = nodeToReplace.ParentNode!;
        packageRefItemGroup.RemoveChild(nodeToReplace);

        foreach (PackageIdAndVersion packageIdAndVersion in replacementPackages)
        {
            XmlNode rxNewPackageRefNode = packageRefItemGroup.OwnerDocument!.CreateElement("PackageReference");
            rxNewPackageRefNode.SetAttribute("Include", packageIdAndVersion.PackageId);
            rxNewPackageRefNode.SetAttribute("Version", packageIdAndVersion.Version);
            packageRefItemGroup.AppendChild(rxNewPackageRefNode);
        }
    }

    public static ProjectFileRewriter CreateForCsProj(string template)
    {
        return new ProjectFileRewriter(template);
    }

    public void FixUpProjectReferences(string templateProjectFolder)
    {
        if (document.SelectNodes("/Project/ItemGroup/ProjectReference") is XmlNodeList projectReferences)
        {
            foreach (XmlElement projectReference in projectReferences.OfType<XmlElement>())
            {
                string relativePath = projectReference.GetAttribute("Include");
                string absolutePath = Path.GetFullPath(Path.Combine(templateProjectFolder, relativePath));
                projectReference.SetAttribute("Include", absolutePath);
            }
        }
    }
}
