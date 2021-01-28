using System;
using System.Collections.Generic;

namespace Spellie.Affiinities.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class AffinityCombineAttribute : Attribute
    {

        /// <summary>
        /// List of affinities required to combine this one
        /// </summary>
        public Type[] affinities;

        public AffinityCombineAttribute(List<Type> affinities)
        {
            this.affinities = affinities.ToArray();
        }
        
        public AffinityCombineAttribute(params Type[] affinities)
        {
            this.affinities = affinities;
        }
    }
}