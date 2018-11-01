using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class JSONModel
    {

        #region Private Members (ex naming: _fieldName)

        #endregion //Private Members

        #region Properties (ex naming: private _PropertyName, public PropertyName)

        public List<KeyValuePair<string, string>> Data { get; set; }

        #endregion //Properties

        #region Constructors

        public JSONModel()
        {
            Data = new List<KeyValuePair<string, string>>();
        }


        #endregion //Constructors


    }
}
