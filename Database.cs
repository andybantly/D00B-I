using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D00B
{
    public class DBTableKey : IComparable<DBTableKey>, IEquatable<DBTableKey>, IComparable
    {
        private string m_strSchema = string.Empty;
        private string m_strTable = string.Empty;
        private string m_strColumn = string.Empty;
        private string m_strJoinTag = string.Empty;

        public DBTableKey()
        {
        }

        public DBTableKey(string strSchema, string strTable, string strColumn) : this()
        {
            Schema = strSchema;
            Table = strTable;
            Column = strColumn;
        }
        public override string ToString()
        {
            return Schema + "." + Table + "." + Column;
        }
        public string Schema
        {
            get { return m_strSchema; }
            set { if (!string.IsNullOrEmpty(value)) m_strSchema = value; }
        }
        public string Table
        {
            get { return m_strTable; }
            set { if (!string.IsNullOrEmpty(value)) m_strTable = value; }
        }

        public string Column
        {
            get { return m_strColumn; }
            set { if (!string.IsNullOrEmpty(value)) m_strColumn = value; }
        }
        public string JoinTag
        {
            get { return m_strJoinTag; }
            set { if (!string.IsNullOrEmpty(value)) m_strJoinTag = value; }
        }
        public override int GetHashCode()
        {
            return Utility.GetHashCode(ToString());
        }
        public int CompareTo(DBTableKey rhs)
        {
            if (Equals(rhs))
                return 0;
            return ToString().CompareTo(rhs.ToString());
        }
        int IComparable.CompareTo(object rhs)
        {
            if (!(rhs is DBTableKey))
                throw new InvalidOperationException("CompareTo: Not a DB Table Key");
            return CompareTo((DBTableKey)rhs);
        }
        public static bool operator <(DBTableKey lhs, DBTableKey rhs) => lhs.CompareTo(rhs) < 0;
        public static bool operator >(DBTableKey lhs, DBTableKey rhs) => lhs.CompareTo(rhs) > 0;
        public bool Equals(DBTableKey rhs) => ToString() == rhs.ToString();
        public override bool Equals(object rhs)
        {
            if (!(rhs is DBTableKey))
                return false;
            return Equals((DBTableKey)rhs);
        }
        public static bool operator ==(DBTableKey lhs, DBTableKey rhs) => lhs.Equals(rhs);
        public static bool operator !=(DBTableKey lhs, DBTableKey rhs) => !(lhs == rhs);
    }
    public class DBJoinKey : DBTableKey
    {
        private readonly Utility.Join m_Join;
        public DBJoinKey()
        {
            m_Join = Utility.Join.Inner;
        }
        public override string ToString()
        {
            string strJoinType;
            switch (m_Join)
            {
                case Utility.Join.Inner:
                    strJoinType = "inner";
                    break;
                case Utility.Join.Left:
                    strJoinType = "left";
                    break;
                case Utility.Join.Right:
                    strJoinType = "right";
                    break;
                case Utility.Join.Full:
                    strJoinType = "full outer";
                    break;
                default:
                    strJoinType = "self";
                    break;
            }
            return strJoinType;
        }

        public DBJoinKey(string strK1, string strK2, string strK3, Utility.Join Join) : this()
        {
            Schema = strK1;
            Table = strK2;
            Column = strK3;
            m_Join = Join;
        }
        public Utility.Join JoinType
        {
            get { return m_Join; }
        }
    }

    public class DBColumn : IComparable<DBColumn>, IEquatable<DBColumn>, IComparable
    {
        string m_strName = string.Empty;
        string m_strFormatString = string.Empty;
        bool m_bIsKey = false;
        List<DBTableKey> m_Tables = new List<DBTableKey>();

        public override string ToString()
        {
            return m_strName;
        }

        public override int GetHashCode()
        {
            return Utility.GetHashCode(ToString());
        }
        public int CompareTo(DBColumn rhs)
        {
            if (Equals(rhs))
                return 0;
            return ToString().CompareTo(rhs.ToString());
        }
        int IComparable.CompareTo(object rhs)
        {
            if (!(rhs is DBColumn))
                throw new InvalidOperationException("CompareTo: Not a Column");
            return CompareTo((DBColumn)rhs);
        }
        public static bool operator <(DBColumn lhs, DBColumn rhs) => lhs.CompareTo(rhs) < 0;
        public static bool operator >(DBColumn lhs, DBColumn rhs) => lhs.CompareTo(rhs) > 0;
        public bool Equals(DBColumn rhs) => ToString() == rhs.ToString();
        public override bool Equals(object rhs)
        {
            if (!(rhs is DBColumn))
                return false;
            return Equals((DBColumn)rhs);
        }
        public static bool operator ==(DBColumn lhs, DBColumn rhs) => lhs.Equals(rhs);
        public static bool operator !=(DBColumn lhs, DBColumn rhs) => !(lhs == rhs);

        public DBColumn()
        {
            m_strFormatString = "G";
        }

        public DBColumn(string strName) : this()
        {
            if (!string.IsNullOrEmpty(strName))
                m_strName = strName;
        }

        public DBColumn(string strName, bool bIsKey) : this(strName)
        {
            m_bIsKey = bIsKey;
        }

        public string Name
        {
            get { return m_strName; }
            set { m_strName = string.IsNullOrEmpty(value) ? value : string.Empty; }
        }

        public bool IsPrimaryKey
        {
            get { return m_bIsKey; }
            set { m_bIsKey = value; }
        }

        public List<DBTableKey> Tables
        {
            get { return m_Tables; }
            set { m_Tables = value; }
        }

        public string FormatString
        { 
            get 
            { 
                return m_strFormatString; 
            } 
            set 
            { 
                m_strFormatString = value; 
            }
        }
        
    }

    internal class DBTable : IComparable<DBTable>, IEquatable<DBTable>, IComparable
    {
        DBTableKey m_TableKey = new DBTableKey();
        private string m_strRows = string.Empty;
        private int m_iSelectedIndex = 0;
        private bool m_bVisited = false;
        readonly List<DBTableKey> m_PKeys;
        List<DBColumn> m_Cols;
        readonly Dictionary<DBTableKey, List<DBTableKey>> m_MapPKtoFK;
        readonly Dictionary<DBTableKey, List<DBTableKey>> m_MapFKtoPK;
        bool m_bView = false;

        public DBTable()
        {
            m_Cols = new List<DBColumn>();
            m_PKeys = new List<DBTableKey>();
            m_MapPKtoFK = new Dictionary<DBTableKey, List<DBTableKey>>();
            m_MapFKtoPK = new Dictionary<DBTableKey, List<DBTableKey>>();
        }
        public DBTable(DBTableKey TableKey) : this()
        {
            m_TableKey = TableKey;
        }
        public void AddPK(DBTableKey PK)
        {
            m_PKeys.Add(PK);
        }
        public bool ContainsPK(string strOwner, string strTable, string strColumn)
        {
            return m_MapPKtoFK.ContainsKey(new DBTableKey(strOwner, strTable, strColumn));
        }
        public bool ContainsFK(string strOwner, string strTable, string strColumn)
        {
            return m_MapFKtoPK.ContainsKey(new DBTableKey(strOwner, strTable, strColumn));
        }
        public List<DBTableKey> PKKeyList(string strOwner, string strTable, string strColumn)
        {
            if (m_MapFKtoPK != null)
            {
                DBTableKey FK = new DBTableKey(strOwner, strTable, strColumn);
                if (m_MapFKtoPK.ContainsKey(FK))
                    return m_MapFKtoPK[FK];
            }
            return null;
        }
        public List<DBTableKey> FKKeyList(string strOwner, string strTable, string strColumn)
        {
            if (m_MapPKtoFK != null)
            {
                DBTableKey PK = new DBTableKey(strOwner, strTable, strColumn);
                if (m_MapPKtoFK.ContainsKey(PK))
                    return m_MapPKtoFK[PK];
            }
            return null;
        }
        public void AddKeyMap(string strPKOwn, string strPKTab, string strPKCol, string strFKOwn, string strFKTab, string strFKCol)
        {
            // Primary Key
            DBTableKey PK = new DBTableKey(strPKOwn, strPKTab, strPKCol);
            List<DBTableKey> FKeyList;
            if (!m_MapPKtoFK.ContainsKey(PK))
            {
                FKeyList = new List<DBTableKey>();
                m_MapPKtoFK[PK] = FKeyList;
            }

            DBTableKey FK = new DBTableKey(strFKOwn, strFKTab, strFKCol);
            FKeyList = m_MapPKtoFK[PK];
            FKeyList.Add(FK);
            m_MapPKtoFK[PK] = FKeyList;

            // Foreign Key
            List<DBTableKey> PKeyList;
            if (!m_MapFKtoPK.ContainsKey(FK))
            {
                PKeyList = new List<DBTableKey>();
                m_MapFKtoPK[FK] = PKeyList;
            }
            PKeyList = m_MapFKtoPK[FK];
            PKeyList.Add(PK);
            m_MapFKtoPK[FK] = PKeyList;
        }
        public override string ToString()
        {
            return m_TableKey.ToString();
        }
        public override int GetHashCode()
        {
            return Utility.GetHashCode(ToString());
        }
        public int CompareTo(DBTable rhs)
        {
            if (Equals(rhs))
                return 0;
            return ToString().CompareTo(rhs.ToString());
        }
        int IComparable.CompareTo(object rhs)
        {
            if (!(rhs is DBTable))
                throw new InvalidOperationException("CompareTo: Not a DBTable");
            return CompareTo((DBTable)rhs);
        }
        public static bool operator <(DBTable lhs, DBTable rhs) => lhs.CompareTo(rhs) < 0;
        public static bool operator >(DBTable lhs, DBTable rhs) => lhs.CompareTo(rhs) > 0;
        public bool Equals(DBTable rhs) => ToString() == rhs.ToString();
        public override bool Equals(object rhs)
        {
            if (!(rhs is DBTable))
                return false;
            return Equals((DBTable)rhs);
        }
        public static bool operator ==(DBTable lhs, DBTable rhs) => lhs.Equals(rhs);
        public static bool operator !=(DBTable lhs, DBTable rhs) => !(lhs == rhs);
        public string TableSchema
        {
            get { return m_TableKey.Schema; }
            set { m_TableKey.Schema = string.IsNullOrEmpty(value) ? value : string.Empty; }
        }
        public string TableName
        {
            get { return m_TableKey.Table; }
            set { m_TableKey.Table = string.IsNullOrEmpty(value) ? value : string.Empty; }
        }
        public List<DBColumn> Columns
        {
            get { return m_Cols; }
            set { m_Cols = value; }
        }
        public List<DBTableKey> Keys
        {
            get { return m_PKeys; }
        }
        public bool HasKey(DBTableKey PK)
        {
            return Keys.Contains(PK);
        }
        public bool Visited
        {
            get { return m_bVisited; }
            set { m_bVisited = value; }
        }
        public string Rows
        {
            get { return m_strRows; }
            set { m_strRows = value; }
        }
        public int SelectedIndex
        {
            get { return m_iSelectedIndex; }
            set { m_iSelectedIndex = value; }
        }

        public bool View
        {
            get { return m_bView; }
            set { m_bView = value; }
        }
    }
}