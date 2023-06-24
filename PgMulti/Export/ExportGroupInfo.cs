using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Export
{
    public class ExportGroupInfo: ExportItemInfo
    {
        public string? Name;
        public List<ExportItemInfo> Items = new List<ExportItemInfo>();
        public ExportGroupInfo? ParentGroup;

        public IReadOnlyList<ExportDBInfo> DBs
        {
            get
            {
                return Items.Where(i => i is ExportDBInfo).Select(i => (ExportDBInfo)i).ToList();
            }
        }

        public IReadOnlyList<ExportGroupInfo> Groups
        {
            get
            {
                return Items.Where(i => i is ExportGroupInfo).Select(i => (ExportGroupInfo)i).ToList();
            }
        }

        public override ExportItemInfo ShallowClone()
        {
            ExportGroupInfo gi = new ExportGroupInfo();
            gi.Name = Name;
            return gi;
        }
    }
}
