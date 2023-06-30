using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEditor;

namespace MenuCustomizer
{
    /// <summary>Utility class for the MenuCustomizer</summary>
    public static class MCUtil
    {
        /// <summary>Import the xml.</summary>
        /// <typeparam name="T">type of the import object</typeparam>
        /// <param name="xmlPath">import xml path</param>
        /// <param name="encoding">encoding of the xml</param>
        /// <returns>if any errors, return default(T)</returns>
        public static T ImportXml<T>(string xmlPath, Encoding encoding = null)
        {
            if(encoding == null) { encoding = new UTF8Encoding(true); }

            try
            {
                using(StreamReader sr = new StreamReader(xmlPath, encoding))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    return (T)xs.Deserialize(sr);
                }
            }
            catch
            {
                MCDebug.LogError("import xml error : " + xmlPath);
                return default(T);
            }
        }

        /// <summary>Export the specified object as xml file.</summary>
        /// <typeparam name="T">type of the export object</typeparam>
        /// <param name="xmlPath">export xml path</param>
        /// <param name="exportData">export object</param>
        /// <param name="encoding">encoding of the xml</param>
        public static void ExportXml<T>(string xmlPath, T exportData, Encoding encoding = null)
        {
            if(encoding == null) { encoding = new UTF8Encoding(true); }

            try
            {
                CreateRequiredDirectory(xmlPath);

                using(StreamWriter sw = new StreamWriter(xmlPath, false, encoding))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));

                    // remove default namespace
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);

                    xs.Serialize(sw, exportData, ns);
                }
            }
            catch
            {
                MCDebug.LogError("export xml error : " + xmlPath);
            }
        }

        /// <summary>Create the directory that is required by the specified file path.</summary>
        public static void CreateRequiredDirectory(string filePath)
        {
            if(!string.IsNullOrEmpty(filePath))
            {
                string directory = Path.GetDirectoryName(filePath);
                if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }

        /// <summary>Create the item list of the hierarchy menu like unity's default.</summary>
        public static List<MCItem> CreateHierarchyListLikeUnity()
        {
            List<MCItem> list = new List<MCItem>()
            {
                // TODO: implement standard menu command.
                new MCItem(MCOriginalCommand.Copy),
                new MCItem(MCOriginalCommand.Paste),
                MCItem.CreateSeparator(),
                new MCItem(MCOriginalCommand.Rename),
                new MCItem(MCOriginalCommand.Duplicate),
                new MCItem(MCOriginalCommand.Delete),
                MCItem.CreateSeparator(),
                new MCItem(MCOriginalCommand.SelectPrefab),

// If editor version is 4.6 or later, add the following menu commands.
#if UNITY_4_6 || UNITY_5
                MCItem.CreateSeparator(),
                new MCItem(MCType.Input, "GameObject/Create Empty", true, "Create Empty"),
                new MCItem(MCType.Input, "GameObject/3D Object/Cube", true, "3D Object/Cube"),
                new MCItem(MCType.Input, "GameObject/3D Object/Sphere", true, "3D Object/Sphere"),
                new MCItem(MCType.Input, "GameObject/3D Object/Capsule", true, "3D Object/Capsule"),
                new MCItem(MCType.Input, "GameObject/3D Object/Cylinder", true, "3D Object/Cylinder"),
                new MCItem(MCType.Input, "GameObject/3D Object/Plane", true, "3D Object/Plane"),
                new MCItem(MCType.Input, "GameObject/3D Object/Quad", true, "3D Object/Quad"),
                new MCItem(MCType.Input, "GameObject/3D Object/Ragdoll...", true, "3D Object/Ragdoll..."),
                new MCItem(MCType.Input, "GameObject/3D Object/Terrain", true, "3D Object/Terrain"),
                new MCItem(MCType.Input, "GameObject/3D Object/Tree", true, "3D Object/Tree"),
                new MCItem(MCType.Input, "GameObject/3D Object/Wind Zone", true, "3D Object/Wind Zone"),
                new MCItem(MCType.Input, "GameObject/3D Object/3D Text", true, "3D Object/3D Text"),
                new MCItem(MCType.Input, "GameObject/2D Object/Sprite", true, "2D Object/Sprite"),
                new MCItem(MCType.Input, "GameObject/Light/Directional Light", true, "Light/Directional Light"),
                new MCItem(MCType.Input, "GameObject/Light/Point Light", true, "Light/Point Light"),
                new MCItem(MCType.Input, "GameObject/Light/Spotlight", true, "Light/Spotlight"),
                new MCItem(MCType.Input, "GameObject/Light/Area Light", true, "Light/Area Light"),
                new MCItem(MCType.Input, "GameObject/Light/Reflection Probe", true, "Light/Reflection Probe"),
                new MCItem(MCType.Input, "GameObject/Light/Light Probe Group", true, "Light/Light Probe Group"),
                new MCItem(MCType.Input, "GameObject/Audio/Audio Source", true, "Audio/Audio Source"),
                new MCItem(MCType.Input, "GameObject/Audio/Audio Reverb Zone", true, "Audio/Audio Reverb Zone"),
                new MCItem(MCType.Input, "GameObject/UI/Panel", true, "UI/Panel"),
                new MCItem(MCType.Input, "GameObject/UI/Button", true, "UI/Button"),
                new MCItem(MCType.Input, "GameObject/UI/Text", true, "UI/Text"),
                new MCItem(MCType.Input, "GameObject/UI/Image", true, "UI/Image"),
                new MCItem(MCType.Input, "GameObject/UI/Raw Image", true, "UI/Raw Image"),
                new MCItem(MCType.Input, "GameObject/UI/Slider", true, "UI/Slider"),
                new MCItem(MCType.Input, "GameObject/UI/Scrollbar", true, "UI/Scrollbar"),
                new MCItem(MCType.Input, "GameObject/UI/Toggle", true, "UI/Toggle"),
                new MCItem(MCType.Input, "GameObject/UI/Input Field", true, "UI/Input Field"),
                new MCItem(MCType.Input, "GameObject/UI/Canvas", true, "UI/Canvas"),
                new MCItem(MCType.Input, "GameObject/UI/Event System", true, "UI/Event System"),
                new MCItem(MCType.Input, "GameObject/Particle System", true, "Particle System"),
                new MCItem(MCType.Input, "GameObject/Camera", true, "Camera")
#endif
            };
            return list;
        }

        /// <summary>Create the item list of the project meny like unity's default.</summary>
        public static List<MCItem> CreateProjectListLikeUnity()
        {
            string[] assetItems = Unsupported.GetSubmenus("Assets");

            List<MCItem> list = new List<MCItem>();

            foreach(string itemPath in assetItems)
            {
                if(RequiresSeparatorBefore(itemPath))
                {
                    list.Add(MCItem.CreateSeparator());
                }

                list.Add(new MCItem(MCType.Input, itemPath, true, itemPath.Replace("Assets/", "")));

                if(RequiresSeparatorAfter(itemPath))
                {
                    list.Add(MCItem.CreateSeparator());
                }
            }

            return list;
        }

        #region Internal Methods

        /// <summary>Check if the 'Assets' path requires separator before.</summary>
        private static bool RequiresSeparatorBefore(string path)
        {
            switch(path)
            {
                case "Assets/Create/Prefab":
                    return true;
            }
            return false;
        }

        /// <summary>Check if the 'Assets' path requires separator after.</summary>
        private static bool RequiresSeparatorAfter(string path)
        {
            switch(path)
            {
                case "Assets/Create/Folder":
                case "Assets/Create/Prefab":
                case "Assets/Create/Audio Mixer":
                case "Assets/Create/Lightmap Parameters":
                case "Assets/Create/Avatar Mask":
                case "Assets/Create/Physics2D Material":
                case "Assets/Create/Shader Variant Collection":
                case "Assets/Create/Legacy/Cubemap":
                case "Assets/Delete":
                case "Assets/Select Dependencies":
                case "Assets/Reimport":
                case "Assets/Reimport All":
                case "Assets/Run API Updater...":
                    return true;
            }
            return false;
        }

        #endregion
    }
}
