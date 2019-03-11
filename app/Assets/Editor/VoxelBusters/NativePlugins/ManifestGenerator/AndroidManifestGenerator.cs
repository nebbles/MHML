namespace VoxelBusters.NativePlugins.Internal
{
    using System.Xml;
    using Android;

    public class AndroidManifestGenerator
    {
        #region Public methods

        public static void GenerateManifest()
        {
            Manifest manifest = new Manifest();
            manifest.AddAttribute("xmlns:android" , "http://schemas.android.com/apk/res/android");
            manifest.AddAttribute("package", "com.voxelbusters.nativeplugins");
            manifest.AddAttribute("android:versionCode", "1");
            manifest.AddAttribute("android:versionName", "1.0");

            SDK sdk = new SDK();
            sdk.AddAttribute("android:minSdkVersion", "14");
            sdk.AddAttribute("android:targetSdkVersion", "27");

            // Add sdk
            manifest.Add(sdk);

            Application application = new Application();

            AddActivities(application);
            AddProviders(application);
            AddServices(application);
            AddReceivers(application);
            AddMetaData(application);

            manifest.Add(application);

            AddPermissions(manifest);
            AddFeatures(manifest);


            XmlDocument xmlDocument = new XmlDocument();
            XmlNode     xmlNode     = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

            // Append xml node
            xmlDocument.AppendChild(xmlNode);

            // Get xml hierarchy
            XmlElement element = manifest.GenerateXml(xmlDocument);
            xmlDocument.AppendChild(element);

            // Save to androidmanifest.xml
            xmlDocument.Save(Constants.kPluginAndroidProjectPath + "AndroidManifest.xml");
        }

        #endregion

        #region Private methods

        private static void AddActivities(Application application)
        {
        }

        private static void AddProviders(Application application)
        {
            Provider provider = null;

            provider = new Provider();
            provider.AddAttribute("android:name", "android.support.v4.content.FileProvider");
            provider.AddAttribute("android:authorities", string.Format("{0}.nativeplugins", UnityEngine.Application.identifier));
            provider.AddAttribute("android:exported", "false");
            provider.AddAttribute("android:grantUriPermissions", "true");

            MetaData metaData = new MetaData();
            metaData.AddAttribute("android:name", "android.support.FILE_PROVIDER_PATHS");
            metaData.AddAttribute("android:resource", "@xml/nativeplugins_file_paths");

            provider.Add(metaData);

            application.Add(provider);
        }

        private static void AddServices(Application application)
        {
        }

        private static void AddReceivers(Application application)
        {
        }

        private static void AddMetaData(Application application)
        {
        }

        private static void AddFeatures(Manifest manifest)
        {
        }

        private static void AddPermissions(Manifest manifest)
        {
            Permission permission = null;

            if(NativePluginsSettings.AddressBookSettings.IsEnabled)
            {
                permission = new Permission();
                permission.AddAttribute("android:name", "android.permission.READ_CONTACTS");
                manifest.Add(permission);
            }
        }

        #endregion
    }
}
