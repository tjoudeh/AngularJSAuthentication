using System;

namespace AngularJSAuthentication.Data.Attributes
{

    [AttributeUsage(AttributeTargets.All)]
    public class Opertation : Attribute
    {
        public Opertation(string[] values)
        {
            this.Values = values;
        }

        public string[] Values { get; set; }
    }





}
