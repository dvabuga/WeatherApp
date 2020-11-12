using System;


namespace WeatherApp.Files
{
    [System.AttributeUsage(System.AttributeTargets.Assembly)]
    public class ModuleAssemblyAttribute : Attribute
    {
        public string name;
        public string author;
        public string version;

        public ModuleAssemblyAttribute(string name, string version)
        {
            this.name = name;
            this.version = version;
            author = "Den";
        }
    }
}