using System;


namespace WeatherApp.Files
{
    [System.AttributeUsage(System.AttributeTargets.Assembly)]
    public class ModuleAssemblyAttribute : Attribute
    {
        public string name;
        public string author;
        public int version;

        public ModuleAssemblyAttribute(string name, int version)
        {
            this.name = name;
            this.version = version;
            author = "Den";
        }
    }
}