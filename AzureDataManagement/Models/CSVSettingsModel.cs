using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class CSVSettingsModel
    {
        public CSVSettingsModel()
        {
            Delimeter = ',';
            TextQuoteCharacter = '"';
        }

        public string FilePath { get; set; }
        public char Delimeter { get; set; }
        public char TextQuoteCharacter { get; set; }
    }
}
