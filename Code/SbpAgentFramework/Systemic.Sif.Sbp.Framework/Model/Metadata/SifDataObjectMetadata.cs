using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Edustructures.SifWorks;

namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    internal class SifDataObjectMetadata<T> where T : SifDataObject
    {
        protected T sifDataObject;

        public virtual ICollection<DependentObject> DependentObjects
        {
            get { return null; }
        }

        /// <summary>
        /// Name of the SIF Data Object's underlying type, e.g. StudentPersonal, SchoolInfo.
        /// </summary>
        public string ObjectName
        {
            get { return sifDataObject.ElementDef.Name; }
        }

        public static T CreateFromXml(string xml)
        {

            if (String.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("xml parameter is null or empty.", "xml");
            }

            SifParser sifParser = SifParser.NewInstance();
            return (T)sifParser.Parse(xml);
        }

        public static ICollection<SifRefIdMetadata> ParseSifUniqueId(string sifUniqueId)
        {
            ICollection<SifRefIdMetadata> parsedValues = new Collection<SifRefIdMetadata>();

            if (sifUniqueId != null)
            {

                foreach (string keyValue in sifUniqueId.Split('|'))
                {
                    string[] values = keyValue.Split('=');

                    if (values.Length == 2)
                    {
                        SifRefIdMetadata sifRefIdMetada = new SifRefIdMetadata() { XPath = values[0], Value = values[1] };
                        parsedValues.Add(sifRefIdMetada);
                    }

                }

            }

            return parsedValues;
        }

        /// <summary>
        /// Unique identfier for the SIF Data Object. This is generally the SIF RefId associated with the SIF Data
        /// Object. In the case where the SIF Data Object does not have a single SIF RefId value and is instead made
        /// up of composite key values (e.g. StudentContactRelationship), then this identifier is a concatenation of
        /// all composite key values separated with "|".
        /// </summary>
        public virtual string SifUniqueId
        {
            get { return "@RefId=" + sifDataObject.RefId; }
        }

        /// <summary>
        /// This class provides a convenient way of examining metadata associated with a SIF Data Object.
        /// </summary>
        /// <param name="sifDataObject">SIF Data Object of interest.</param>
        public SifDataObjectMetadata(T sifDataObject)
        {
            this.sifDataObject = sifDataObject;
        }

    }

}
