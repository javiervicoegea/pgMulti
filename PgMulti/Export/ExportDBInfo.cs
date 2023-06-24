using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PgMulti.Export
{
    public class ExportDBInfo: ExportItemInfo
    {
        public string? Alias;
        public string? Server;
        public ushort Port;
        public string? DBName;
        public string? User;
        public string? Password;
        public ExportGroupInfo? GroupInfo;

        public override ExportItemInfo ShallowClone()
        {
            ExportDBInfo dbi = new ExportDBInfo();

            dbi.Alias = Alias;
            dbi.Server = Server;
            dbi.Port = Port;
            dbi.DBName = DBName;
            dbi.User = User;
            dbi.Password = Password;

            return dbi;
        }
    }
}
