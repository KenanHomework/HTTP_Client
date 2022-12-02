using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ReuqestlCLassAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CommandAttribute : Attribute
    {
        public string Info;
        public string CallName;
        public string Parametres = string.Empty;
        public string Tricks = string.Empty;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(CallName);

            if (!string.IsNullOrWhiteSpace(Parametres))
                builder.Append($" <{Parametres}> ");

            if (!string.IsNullOrWhiteSpace(Tricks))
                builder.Append($" ! {Tricks} ");

            if (!string.IsNullOrWhiteSpace(Info))
                builder.Append($" ? {Info} ");

            return builder.ToString();
        }
    }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PropertyAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RequiredAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RequiredIDAttribute : Attribute { }

}
