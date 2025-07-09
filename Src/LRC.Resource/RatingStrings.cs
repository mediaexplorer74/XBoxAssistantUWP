// *********************************************************
// Type: LRC.Resources.RatingStrings
// Assembly: LRC.Resource, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9093DA41-95AA-481C-B970-06E86E67D53B
// *********************************************************LRC.Resource.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;


namespace LRC.Resources
{
  [CompilerGenerated]
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    public class RatingStrings
    {
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RatingStrings()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals((object)RatingStrings.resourceMan, (object)null))
                    RatingStrings.resourceMan = new ResourceManager("LRC.Resources.Resources.RatingStrings", 
                        typeof(RatingStrings).GetTypeInfo().Assembly); // Use GetTypeInfo().Assembly
                return RatingStrings.resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static CultureInfo Culture
        {
            get => RatingStrings.resourceCulture;
            set => RatingStrings.resourceCulture = value;
        }

        // Other properties remain unchanged
    }
}
