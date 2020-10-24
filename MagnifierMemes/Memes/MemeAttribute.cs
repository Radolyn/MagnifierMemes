#region

using System;

#endregion

namespace MagnifierMemes.Memes
{
    public class MemeAttribute : Attribute
    {
        public MemeAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public MemeAttribute(string name, string description, params string[] additionalParameters)
        {
            Name = name;
            Description = description;
            AdditionalParameters = additionalParameters;
        }

        public string Name { get; }
        public string Description { get; }
        public string[] AdditionalParameters { get; }
    }
}