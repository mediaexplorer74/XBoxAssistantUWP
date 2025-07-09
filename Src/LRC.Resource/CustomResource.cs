// *********************************************************
// Type: LRC.Resources.CustomResource
// Assembly: LRC.Resource, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9093DA41-95AA-481C-B970-06E86E67D53B
// *********************************************************LRC.Resource.dll

using System.Diagnostics.CodeAnalysis;


namespace LRC.Resources
{
  public class CustomResource
  {
    private static readonly Resource resources = new Resource();

    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Making this static prevents accessing the strings from XAML.")]
    public Resource Strings => CustomResource.resources;
  }
}
