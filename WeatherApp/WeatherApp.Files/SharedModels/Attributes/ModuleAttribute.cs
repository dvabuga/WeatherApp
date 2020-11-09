using System;


namespace WeatherApp.Files
{
        [System.AttributeUsage(System.AttributeTargets.Class)]  
        public class ModuleAttribute : Attribute
        {            
            public string name;
            public string author;

            public ModuleAttribute(string name)
            {
                this.name = name;
                author = "Den";
            }

        }

}