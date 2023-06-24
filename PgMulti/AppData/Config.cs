using PgMulti.DataAccess;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Xml;
using System.Security.Cryptography;
using System.Text;

namespace PgMulti.AppData
{
    public class Config
    {
        public bool KeepServerSelection;
        public int AutocompleteDelay;
        public bool MergeTables;
        public TransactionModeEnum TransactionMode;
        public TransactionLevelEnum TransactionLevel;
        public int FontSize;
        public int MaxRows;
        public bool ShowWarningSelectedText;

        private Data _Data;

        public Config(Data d, DataRow dr)
        {
            _Data = d;

            KeepServerSelection = dr.Field<long>("keepServerSelection")! == 1L;
            AutocompleteDelay = (int)dr.Field<long>("autocompleteDelay");
            MergeTables = dr.Field<long>("mergeTables")! == 1L;
            TransactionMode = (TransactionModeEnum)dr.Field<long>("transactionMode")!;
            TransactionLevel = (TransactionLevelEnum)dr.Field<long>("transactionLevel")!;
            FontSize = (int)dr.Field<long>("fontSize")!;
            MaxRows = (int)dr.Field<long>("maxRows")!;
            ShowWarningSelectedText = dr.Field<long>("showWarningSelectedText")! == 1L;
        }

        public void Save()
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();

                cmd.CommandText = "UPDATE config SET keepServerSelection=:keepServerSelection,autocompleteDelay=:autocompleteDelay,mergeTables=:mergeTables,transactionMode=:transactionMode,transactionLevel=:transactionLevel,fontSize=:fontSize,maxRows=:maxRows,showWarningSelectedText=:showWarningSelectedText";
                cmd.Parameters.AddWithValue("keepServerSelection", KeepServerSelection);
                cmd.Parameters.AddWithValue("autocompleteDelay", AutocompleteDelay);
                cmd.Parameters.AddWithValue("mergeTables", MergeTables);
                cmd.Parameters.AddWithValue("transactionMode", (int)TransactionMode);
                cmd.Parameters.AddWithValue("transactionLevel", (int)TransactionLevel);
                cmd.Parameters.AddWithValue("fontSize", FontSize);
                cmd.Parameters.AddWithValue("maxRows", MaxRows);
                cmd.Parameters.AddWithValue("showWarningSelectedText", ShowWarningSelectedText);

                cmd.ExecuteNonQuery();
            }
        }

        public enum TransactionModeEnum
        {
            Manual = 0,
            AutoSingle = 1,
            AutoCoordinated = 2
        }

        public enum TransactionLevelEnum
        {
            ReadCommited = 0,
            RepeatableRead = 1,
            Serializable = 2
        }
    }
}
