﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccessScrCtrl.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AccessScrCtrl.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon AccessScrCtrl {
            get {
                object obj = ResourceManager.GetObject("AccessScrCtrl", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error loading &apos;{0}&apos;:
        ///{1}.
        /// </summary>
        internal static string ErrorLoadingObject {
            get {
                return ResourceManager.GetString("ErrorLoadingObject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon Export {
            get {
                object obj = ResourceManager.GetObject("Export", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon Import {
            get {
                object obj = ResourceManager.GetObject("Import", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading objects names....
        /// </summary>
        internal static string LoadingObjectsTree {
            get {
                return ResourceManager.GetString("LoadingObjectsTree", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} objects restored.
        /// </summary>
        internal static string ObjectsLoaded {
            get {
                return ResourceManager.GetString("ObjectsLoaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} objects saved.
        /// </summary>
        internal static string ObjectsSaved {
            get {
                return ResourceManager.GetString("ObjectsSaved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Saving {0}.
        /// </summary>
        internal static string Saving {
            get {
                return ResourceManager.GetString("Saving", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must specify the working copy path before this action.
        /// </summary>
        internal static string WorkingCopyMissing {
            get {
                return ResourceManager.GetString("WorkingCopyMissing", resourceCulture);
            }
        }
    }
}