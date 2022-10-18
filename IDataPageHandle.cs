using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS19ModManager
{
    internal interface IDataPageHandle
    {
        public void handleData(object? data);
    }
}
