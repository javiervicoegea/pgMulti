using PgMulti.AppData;
using PgMulti.DataAccess;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace PgMulti.Export
{
    public class ExportConnectionsFile
    {
        private const int ConnectionsExportFileVersion = 1;
        public bool EncryptedPasswords = false;
        public ExportGroupInfo RootGroup = new ExportGroupInfo();

        public void LoadFile(string filename)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(filename);

            XmlElement? xmlRoot = xd.DocumentElement;
            if (xmlRoot == null) throw new BadFormatException();

            if (xmlRoot.Name != "pgmulti_connections_export") throw new BadFormatException();

            int version;
            if (!int.TryParse(xmlRoot.GetAttribute("file_version"), out version) || version < 1 || version > ConnectionsExportFileVersion) throw new BadFormatException();

            if (!bool.TryParse(xmlRoot.GetAttribute("encrypted_passwords"), out EncryptedPasswords)) throw new BadFormatException();

            Queue<Tuple<XmlElement, ExportGroupInfo>> queue = new Queue<Tuple<XmlElement, ExportGroupInfo>>();
            queue.Enqueue(new Tuple<XmlElement, ExportGroupInfo>(xmlRoot, RootGroup));

            while (queue.Count > 0)
            {
                Tuple<XmlElement, ExportGroupInfo> t = queue.Dequeue();
                XmlElement xmlGroup = t.Item1;
                ExportGroupInfo gi = t.Item2;

                foreach (XmlElement xmlChildItem in xmlGroup.ChildNodes)
                {
                    switch (xmlChildItem.Name)
                    {
                        case "db":
                            ExportDBInfo dbi = new ExportDBInfo();

                            dbi.Alias = xmlChildItem.GetAttribute("alias");
                            if (string.IsNullOrEmpty(dbi.Alias)) throw new BadFormatException();

                            dbi.Server = xmlChildItem.GetAttribute("server");
                            if (string.IsNullOrEmpty(dbi.Server)) throw new BadFormatException();

                            if (!ushort.TryParse(xmlChildItem.GetAttribute("port"), out dbi.Port)) throw new BadFormatException();
                            if (dbi.Port <= 0 || dbi.Port >= 65535) throw new BadFormatException();

                            dbi.DBName = xmlChildItem.GetAttribute("dbname");
                            if (string.IsNullOrEmpty(dbi.DBName)) throw new BadFormatException();

                            dbi.User = xmlChildItem.GetAttribute("user");
                            if (string.IsNullOrEmpty(dbi.User)) throw new BadFormatException();

                            dbi.Password = xmlChildItem.GetAttribute("password");
                            if (string.IsNullOrEmpty(dbi.Password)) dbi.Password = null;

                            dbi.GroupInfo = gi;

                            gi.Items.Add(dbi);
                            break;
                        case "group":
                            ExportGroupInfo childGroupInfo = new ExportGroupInfo();

                            childGroupInfo.Name = xmlChildItem.GetAttribute("name");
                            if (string.IsNullOrEmpty(childGroupInfo.Name)) throw new BadFormatException();
                            childGroupInfo.ParentGroup = gi;

                            gi.Items.Add(childGroupInfo);

                            queue.Enqueue(new Tuple<XmlElement, ExportGroupInfo>(xmlChildItem, childGroupInfo));
                            break;
                    }

                }
            }
        }

        public void LoadConfig(Data d)
        {
            ExportGroupInfo root = new ExportGroupInfo();
            Queue<Tuple<Group, ExportGroupInfo>> queue = new Queue<Tuple<Group, ExportGroupInfo>>();
            queue.Enqueue(new Tuple<Group, ExportGroupInfo>(d.RootGroup, RootGroup));

            while (queue.Count > 0)
            {
                Tuple<Group, ExportGroupInfo> t = queue.Dequeue();
                Group configGroup = t.Item1;
                ExportGroupInfo gi = t.Item2;

                List<Tuple<int, object>> items = new List<Tuple<int, object>>();

                foreach (DB db in configGroup.DBs)
                {
                    items.Add(new Tuple<int, object>(db.Position, db));
                }

                foreach (Group childGroup in configGroup.ChildGroups)
                {
                    items.Add(new Tuple<int, object>(childGroup.Position, childGroup));
                }

                items = items.OrderBy(t => t.Item1).ToList();

                foreach (Tuple<int, object> childItem in items)
                {
                    if (childItem.Item2 is DB)
                    {
                        DB db = (DB)childItem.Item2;
                        ExportDBInfo dbi = new ExportDBInfo();
                        dbi.Alias = db.Alias;
                        dbi.Server = db.Server;
                        dbi.Port = db.Port;
                        dbi.DBName = db.DBName;
                        dbi.User = db.User;
                        dbi.Password = db.Password;
                        dbi.GroupInfo = gi;

                        gi.Items.Add(dbi);
                    }
                    else if (childItem.Item2 is Group)
                    {
                        Group childGroup = (Group)childItem.Item2;
                        ExportGroupInfo childGroupInfo = new ExportGroupInfo();

                        childGroupInfo.Name = childGroup.Name;
                        childGroupInfo.ParentGroup = gi;
                        gi.Items.Add(childGroupInfo);

                        queue.Enqueue(new Tuple<Group, ExportGroupInfo>(childGroup, childGroupInfo));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }
        }

        public bool HasDbAliasConflicts(Data d)
        {
            Stack<ExportGroupInfo> stack = new Stack<ExportGroupInfo>();

            stack.Push(RootGroup);

            while (stack.Count > 0)
            {
                ExportGroupInfo g = stack.Pop();

                foreach (ExportDBInfo dbi in g.DBs)
                {
                    DB? dbConflict = d.AllDBs.FirstOrDefault(dbd => dbd.Alias == dbi.Alias);
                    if (dbConflict != null)
                    {
                        if (
                            dbConflict.Server != dbi.Server
                            || dbConflict.Port != dbi.Port
                            || dbConflict.DBName != dbi.DBName
                            || dbConflict.User != dbi.User
                            || (
                                dbConflict.Password != dbi.Password
                                && dbi.Password != null
                            )
                        )
                        {
                            return true;
                        }
                        else
                        {
                            ExportGroupInfo? giConflict = dbi.GroupInfo;
                            Group? gConflict = dbConflict.Group;

                            while (giConflict != null)
                            {
                                if (gConflict == null) return true;
                                if (giConflict.Name != gConflict.Name) return true;

                                giConflict = giConflict.ParentGroup;
                                gConflict = gConflict.ParentGroup;
                            }

                            if (gConflict != null) return true;
                        }
                    }
                }
            }

            return false;
        }

        private object ImportItem(Data d, Group parentGroup, ExportItemInfo eii)
        {
            object item;
            if (eii is ExportGroupInfo)
            {
                ExportGroupInfo gi = (ExportGroupInfo)eii;
                Group? group;

                group = parentGroup.ChildGroups.FirstOrDefault(gd => gd.Name == gi.Name);

                if (group == null)
                {
                    group = new Group(d, parentGroup);
                    group.Name = gi.Name!;
                    group.Position = (short)parentGroup.Count;
                    group.Save();
                    parentGroup.ChildGroups.Add(group);
                }

                item = group;
            }
            else if (eii is ExportDBInfo)
            {
                ExportDBInfo dbi = (ExportDBInfo)eii;
                DB? db = d.AllDBs.FirstOrDefault(dbd => dbd.Alias.ToLower() == dbi.Alias!.ToLower());
                if (db == null)
                {
                    db = new DB(d, parentGroup);
                    db.Position = (short)parentGroup.Count;
                    parentGroup.DBs.Add(db);
                    d.AllDBs.Add(db);
                }

                db.Alias = dbi.Alias!;
                db.Server = dbi.Server!;
                db.Port = dbi.Port!;
                db.DBName = dbi.DBName!;
                db.User = dbi.User!;

                if (dbi.Password != null)
                {
                    db.Password = dbi.Password!;
                }

                db.Save();

                item = db;
            }
            else
            {
                throw new NotSupportedException();
            }

            return item;
        }

        public void Import(Data d)
        {
            using (Connection c = d.OpenConnection())
            using (Transaction t = c.Begin())
            {
                Queue<Tuple<Group, ExportGroupInfo>> queue = new Queue<Tuple<Group, ExportGroupInfo>>();
                queue.Enqueue(new Tuple<Group, ExportGroupInfo>(d.RootGroup, RootGroup));

                while (queue.Count > 0)
                {
                    Tuple<Group, ExportGroupInfo> tuple = queue.Dequeue();
                    Group group = tuple.Item1;
                    ExportGroupInfo gi = tuple.Item2;

                    foreach (ExportItemInfo childItemInfo in gi.Items)
                    {
                        object item = ImportItem(d, group, childItemInfo);

                        if (childItemInfo is ExportGroupInfo)
                        {
                            ExportGroupInfo childGroupInfo = (ExportGroupInfo)childItemInfo;
                            queue.Enqueue(new Tuple<Group, ExportGroupInfo>((Group)item, childGroupInfo));
                        }
                    }
                }
                t.Commit();
            }
        }

        public void EncryptPasswords(string password)
        {
            if (EncryptedPasswords) throw new NotSupportedException();

            Stack<ExportGroupInfo> stack = new Stack<ExportGroupInfo>();
            stack.Push(RootGroup);

            while (stack.Count > 0)
            {
                ExportGroupInfo gi = stack.Pop();

                foreach (ExportDBInfo dbi in gi.DBs)
                {
                    if (dbi.Password != null)
                    {
                        dbi.Password = Encrypt(dbi.Password, password);
                    }
                }

                foreach (ExportGroupInfo childGroupInfo in gi.Groups)
                {
                    stack.Push(childGroupInfo);
                }
            }

            EncryptedPasswords = true;
        }

        public void DecryptPasswords(string password)
        {
            if (!EncryptedPasswords) throw new NotSupportedException();

            Stack<ExportGroupInfo> stack = new Stack<ExportGroupInfo>();
            stack.Push(RootGroup);

            while (stack.Count > 0)
            {
                ExportGroupInfo gi = stack.Pop();

                foreach (ExportDBInfo dbi in gi.DBs)
                {
                    if (dbi.Password != null)
                    {
                        dbi.Password = Decrypt(dbi.Password, password);
                    }
                }

                foreach (ExportGroupInfo childGroupInfo in gi.Groups)
                {
                    stack.Push(childGroupInfo);
                }
            }

            EncryptedPasswords = false;
        }

        private XmlElement AppendXmlChild(XmlElement xeParent, ExportItemInfo eii, bool includePasswords)
        {
            XmlElement xe;
            if (eii is ExportGroupInfo)
            {
                ExportGroupInfo gi = (ExportGroupInfo)eii;
                xe = xeParent.OwnerDocument.CreateElement("group");
                xe.SetAttribute("name", gi.Name);
                xeParent.AppendChild(xe);
            }
            else if (eii is ExportDBInfo)
            {
                ExportDBInfo dbi = (ExportDBInfo)eii;
                xe = xeParent.OwnerDocument.CreateElement("db");

                xe.SetAttribute("alias", dbi.Alias);
                xe.SetAttribute("server", dbi.Server);
                xe.SetAttribute("port", dbi.Port.ToString());
                xe.SetAttribute("dbname", dbi.DBName);
                xe.SetAttribute("user", dbi.User);

                if (includePasswords)
                {
                    xe.SetAttribute("password", dbi.Password);
                }

                xeParent.AppendChild(xe);
            }
            else
            {
                throw new NotSupportedException();
            }

            return xe;
        }

        public void SaveFile(string filename, bool includePasswords)
        {
            XmlDocument xd = new XmlDocument();

            xd.AppendChild(xd.CreateXmlDeclaration("1.0", "utf-8", null));

            XmlElement rootXml = xd.CreateElement("pgmulti_connections_export");
            rootXml.SetAttribute("application_version", Application.ProductVersion);
            rootXml.SetAttribute("file_version", ConnectionsExportFileVersion.ToString());
            rootXml.SetAttribute("encrypted_passwords", EncryptedPasswords.ToString());
            xd.AppendChild(rootXml);

            Queue<Tuple<XmlElement, ExportGroupInfo>> queue = new Queue<Tuple<XmlElement, ExportGroupInfo>>();
            queue.Enqueue(new Tuple<XmlElement, ExportGroupInfo>(rootXml, RootGroup));

            while (queue.Count > 0)
            {
                Tuple<XmlElement, ExportGroupInfo> tuple = queue.Dequeue();
                XmlElement xe = tuple.Item1;
                ExportGroupInfo gi = tuple.Item2;

                foreach (ExportItemInfo childItemInfo in gi.Items)
                {
                    XmlElement xeChild = AppendXmlChild(xe, childItemInfo, includePasswords);

                    if (childItemInfo is ExportGroupInfo)
                    {
                        ExportGroupInfo childGroupInfo = (ExportGroupInfo)childItemInfo;
                        queue.Enqueue(new Tuple<XmlElement, ExportGroupInfo>(xeChild, childGroupInfo));
                    }
                }
            }

            xd.Save(filename);
        }

        private string Encrypt(string clearText, string password)
        {
            byte[] passwordBytes;
            using (SHA256 sha = SHA256.Create())
            {
                passwordBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            var tdcs = Aes.Create();

            tdcs.Key = passwordBytes!;
            tdcs.Mode = CipherMode.ECB;
            tdcs.Padding = PaddingMode.PKCS7;

            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);

            byte[] encryptedBytes;
            using (var MyCrytpoTransform = tdcs.CreateEncryptor())
            {
                encryptedBytes = MyCrytpoTransform.TransformFinalBlock(clearBytes, 0, clearBytes.Length);
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        private string? Decrypt(string encryptedText, string password)
        {
            byte[] passwordBytes;
            using (SHA256 sha = SHA256.Create())
            {
                passwordBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            var tdcs = Aes.Create();

            tdcs.Key = passwordBytes;
            tdcs.Mode = CipherMode.ECB;
            tdcs.Padding = PaddingMode.PKCS7;

            byte[] encrypted = Convert.FromBase64String(encryptedText);

            byte[] decrypted;
            using (var MyCrytpoTransform = tdcs.CreateDecryptor())
            {
                decrypted = MyCrytpoTransform.TransformFinalBlock(encrypted, 0, encrypted.Length);
            }

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
