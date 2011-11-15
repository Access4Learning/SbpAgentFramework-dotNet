
namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    class SifRefIdMetadata
    {
        private string @value;
        private string xPath;

        internal string XPath
        {
            get { return xPath; }
            set { xPath = value; }
        }

        internal string Value
        {
            get { return value; }
            set { this.value = value; }
        }

    }

}
