using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Export
{
    public abstract class ExportItemInfo
    {
        public abstract ExportItemInfo ShallowClone();
  }
}
