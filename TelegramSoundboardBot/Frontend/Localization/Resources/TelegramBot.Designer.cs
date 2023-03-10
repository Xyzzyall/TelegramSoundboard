//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TelegramSoundboardBot.Frontend.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TelegramBot {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TelegramBot() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TelegramSoundboardBot.Frontend.Localization.Resources.TelegramBot", typeof(TelegramBot).Assembly);
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
        ///   Looks up a localized string similar to Greetings! This bot can send your favorite sounds just like stickers!
        ///
        ///WIP..
        /// </summary>
        internal static string BotGreeting_0 {
            get {
                return ResourceManager.GetString("BotGreeting_0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error occured: {0}.
        /// </summary>
        internal static string DEBUG_ErrorOccured {
            get {
                return ResourceManager.GetString("DEBUG_ErrorOccured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sound &quot;{0}&quot; is not found..
        /// </summary>
        internal static string SoundIsNotFound {
            get {
                return ResourceManager.GetString("SoundIsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sound with name &quot;{0}&quot; already exists..
        /// </summary>
        internal static string SoundNameAlreadyExists {
            get {
                return ResourceManager.GetString("SoundNameAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Write the name of sound in the audio&apos;s caption..
        /// </summary>
        internal static string SoundNameIsEmpty {
            get {
                return ResourceManager.GetString("SoundNameIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sound with name `{0}` was succesfully added. *Try it out!*.
        /// </summary>
        internal static string SoundSuccessfullyAdded {
            get {
                return ResourceManager.GetString("SoundSuccessfullyAdded", resourceCulture);
            }
        }
    }
}
