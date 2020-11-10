using System;


namespace WeatherApp.Files
{
    [System.AttributeUsage(System.AttributeTargets.Assembly)]
    public class ModuleAssemblyAttribute : Attribute
    {
        public string name;
        public string author;

        public ModuleAssemblyAttribute(string name)
        {
            this.name = name;
            author = "Den";
        }
    }

}