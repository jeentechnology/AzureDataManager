using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Interfaces
{
    public interface ITranslate<T, R>
    {
        R Translate(T data);
    }
}
