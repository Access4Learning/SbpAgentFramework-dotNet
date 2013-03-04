/*
* Copyright 2011-2013 Systemic Pty Ltd
* 
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*    http://www.apache.org/licenses/LICENSE-2.0
* 
* Unless required by applicable law or agreed to in writing, software distributed under the License 
* is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
* or implied.
* See the License for the specific language governing permissions and limitations under the License.
*/

namespace Systemic.Sif.Sbp.Framework.Model.Metadata
{

    public class SifRefIdMetadata
    {
        private string @value;
        private string xPath;

        public string XPath
        {
            get { return xPath; }
            set { xPath = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Create an instance of this class.
        /// </summary>
        public SifRefIdMetadata()
        {
        }

        /// <summary>
        /// Create an instance of this class based upon the specified SIF RefId in XPath notation. An example string
        ///  might be @RefId="7C834EA9EDA12090347F83297E1C290C".
        /// </summary>
        /// <param name="sifUniqueId">SIF RefId in XPath notation.</param>
        public SifRefIdMetadata(string sifUniqueId)
        {

            if (sifUniqueId != null)
            {
                string[] values = sifUniqueId.Split('=');

                if (values.Length == 2)
                {
                    XPath = values[0];
                    Value = values[1];
                }

            }

        }

    }

}
