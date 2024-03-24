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
    }

    public class CVariant : IComparable<CVariant>, IEquatable<CVariant>, IComparable
    {
        private readonly TypeCode m_TypeCode;
        private readonly Boolean[] m_boolVal = null;
        private readonly Char[] m_cVal = null;
        private readonly Byte[] m_byteVal = null;
        private readonly SByte[] m_sbyteVal = null;
        private readonly Int16[] m_int16Val = null;
        private readonly UInt16[] m_uint16Val = null;
        private readonly Int32[] m_int32Val = null;
        private readonly UInt32[] m_uint32Val = null;
        private readonly Int64[] m_int64Val = null;
        private readonly UInt64[] m_uint64Val = null;
        private readonly Single[] m_fVal = null;
        private readonly Double[] m_dVal = null;
        private readonly Decimal[] m_decVal = null;
        private readonly DateTime[] m_dtVal = null;
        private readonly String[] m_strVal = null;
        private readonly int[] m_arrRow = null;

        public CVariant(int iRow) 
        {
            m_TypeCode = TypeCode.Empty;
            m_arrRow = new int[] { iRow, iRow };
        }

        public CVariant(TypeCode TypeCode, int iRow)
        {
            m_TypeCode = TypeCode;
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(Boolean boolVal, int iRow)
        {
            m_TypeCode = TypeCode.Boolean;
            m_boolVal = new Boolean[] { boolVal, boolVal };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(Char cVal, int iRow)
        {
            m_TypeCode = TypeCode.Char;
            m_cVal = new Char[] { cVal, cVal };
            m_arrRow = new int[] { iRow, iRow };
        }

        public CVariant(Byte byteVal, int iRow)
        {
            m_TypeCode = TypeCode.Byte;
            m_byteVal = new Byte[] { byteVal, byteVal };
            m_arrRow = new int[] { iRow, iRow };
        }

        public CVariant(SByte sbyteVal, int iRow)
        {
            m_TypeCode = TypeCode.SByte;
            m_sbyteVal = new SByte[] { sbyteVal, sbyteVal };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(Int16 int16Val, int iRow)
        {
            m_TypeCode = TypeCode.Int16;
            m_int16Val = new Int16[] { int16Val, int16Val };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(UInt16 uint16Val, int iRow)
        {
            m_TypeCode = TypeCode.UInt16;
            m_uint16Val = new UInt16[] { uint16Val, uint16Val };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(Int32 int32Val, int iRow)
        {
            m_TypeCode = TypeCode.Int32;
            m_int32Val = new Int32[] { int32Val, int32Val };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(UInt32 uint32Val, int iRow)
        {
            m_TypeCode = TypeCode.UInt32;
            m_uint32Val = new UInt32[] { uint32Val, uint32Val };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(Int64 int64Val, int iRow)
        {
            m_TypeCode = TypeCode.Int64;
            m_int64Val = new Int64[] { int64Val, int64Val };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(UInt64 uint64Val, int iRow)
        {
            m_TypeCode = TypeCode.UInt64;
            m_uint64Val = new UInt64[] { uint64Val, uint64Val };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(Single fVal, int iRow)
        {
            m_TypeCode = TypeCode.Single;
            m_fVal = new Single[] { fVal, fVal };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(Double dVal, int iRow)
        {
            m_TypeCode = TypeCode.Double;
            m_dVal = new Double[] { dVal, dVal };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(Decimal decVal, int iRow)
        {
            m_TypeCode = TypeCode.Decimal;
            m_decVal = new Decimal[] { decVal, decVal };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(DateTime dtVal, int iRow)
        {
            m_TypeCode = TypeCode.DateTime;
            m_dtVal = new DateTime[] { dtVal, dtVal };
            m_arrRow = new int[] { iRow, iRow };
        }
        public CVariant(String strVal, int iRow)
        {
            m_TypeCode = TypeCode.String;
            m_strVal = new String[] { strVal, strVal };
            m_arrRow = new int[] { iRow, iRow };
        }
        public override string ToString()
        {
            return string.Format("[{0}]={1}", m_arrRow[0], CellValue);
        }
        public override int GetHashCode()
        {
            return Utility.GetHashCode(ToString());
        }
        public int CompareTo(CVariant rhs)
        {
            int iRet;
            switch (m_TypeCode)
            {
                case TypeCode.Boolean:
                    Boolean boolLhs = m_boolVal[0];
                    Boolean boolRhs = rhs.m_boolVal[0];
                    iRet = Global.g_bSortOrder ? (boolLhs == true && boolRhs == false ? -1 : (boolLhs == boolRhs ? 0 : 1)) : (boolLhs == true && boolRhs == false ? 1 : (boolLhs == boolRhs ? 0 : -1));
                    break;

                case TypeCode.Byte:
                    Byte bLhs = m_byteVal[0];
                    Byte bRhs = rhs.m_byteVal[0];
                    iRet = Global.g_bSortOrder ? (bLhs < bRhs ? -1 : (bLhs == bRhs ? 0 : 1)) : (bLhs < bRhs ? 1 : (bLhs == bRhs ? 0 : -1));
                    break;

                case TypeCode.SByte:
                    SByte sbLhs = m_sbyteVal[0];
                    SByte sbRhs = rhs.m_sbyteVal[0];
                    iRet = Global.g_bSortOrder ? (sbLhs < sbRhs ? -1 : (sbLhs == sbRhs ? 0 : 1)) : (sbLhs < sbRhs ? 1 : (sbLhs == sbRhs ? 0 : -1));
                    break;

                case TypeCode.Char:
                    Char cLhs = m_cVal[0];
                    Char cRhs = rhs.m_cVal[0];
                    iRet = Global.g_bSortOrder ? (cLhs < cRhs ? -1 : (cLhs == cRhs ? 0 : 1)) : (cLhs < cRhs ? 1 : (cLhs == cRhs ? 0 : -1));
                    break;

                case TypeCode.Int16:
                    Int16 i16Lhs = m_int16Val[0];
                    Int16 i16Rhs = rhs.m_int16Val[0];
                    iRet = Global.g_bSortOrder ? (i16Lhs < i16Rhs ? -1 : (i16Lhs == i16Rhs ? 0 : 1)) : (i16Lhs < i16Rhs ? 1 : (i16Lhs == i16Rhs ? 0 : -1));
                    break;

                case TypeCode.UInt16:
                    UInt16 ui16Lhs = m_uint16Val[0];
                    UInt16 ui16Rhs = rhs.m_uint16Val[0];
                    iRet = Global.g_bSortOrder ? (ui16Lhs < ui16Rhs ? -1 : (ui16Lhs == ui16Rhs ? 0 : 1)) : (ui16Lhs < ui16Rhs ? 1 : (ui16Lhs == ui16Rhs ? 0 : -1));
                    break;

                case TypeCode.Int32:
                    Int32 i32Lhs = m_int32Val[0];
                    Int32 i32Rhs = rhs.m_int32Val[0];
                    iRet = Global.g_bSortOrder ? (i32Lhs < i32Rhs ? -1 : (i32Lhs == i32Rhs ? 0 : 1)) : (i32Lhs < i32Rhs ? 1 : (i32Lhs == i32Rhs ? 0 : -1));
                    break;

                case TypeCode.UInt32:
                    UInt32 ui32Lhs = m_uint32Val[0];
                    UInt32 ui32Rhs = rhs.m_uint32Val[0];
                    iRet = Global.g_bSortOrder ? (ui32Lhs < ui32Rhs ? -1 : (ui32Lhs == ui32Rhs ? 0 : 1)) : (ui32Lhs < ui32Rhs ? 1 : (ui32Lhs == ui32Rhs ? 0 : -1));
                    break;

                case TypeCode.Int64:
                    Int64 i64Lhs = m_int64Val[0];
                    Int64 i64Rhs = rhs.m_int64Val[0];
                    iRet = Global.g_bSortOrder ? (i64Lhs < i64Rhs ? -1 : (i64Lhs == i64Rhs ? 0 : 1)) : (i64Lhs < i64Rhs ? 1 : (i64Lhs == i64Rhs ? 0 : -1));
                    break;

                case TypeCode.UInt64:
                    UInt64 ui64Lhs = m_uint64Val[0];
                    UInt64 ui64Rhs = rhs.m_uint64Val[0];
                    iRet = Global.g_bSortOrder ? (ui64Lhs < ui64Rhs ? -1 : (ui64Lhs == ui64Rhs ? 0 : 1)) : (ui64Lhs < ui64Rhs ? 1 : (ui64Lhs == ui64Rhs ? 0 : -1));
                    break;

                case TypeCode.Single:
                    Single fLhs = m_fVal[0];
                    Single fRhs = rhs.m_fVal[0];
                    iRet = Global.g_bSortOrder ? (fLhs < fRhs ? -1 : (fLhs == fRhs ? 0 : 1)) : (fLhs < fRhs ? 1 : (fLhs == fRhs ? 0 : -1));
                    break;

                case TypeCode.Double:
                    Double dLhs = m_dVal[0];
                    Double dRhs = rhs.m_dVal[0];
                    iRet = Global.g_bSortOrder ? (dLhs < dRhs ? -1 : (dLhs == dRhs ? 0 : 1)) : (dLhs < dRhs ? 1 : (dLhs == dRhs ? 0 : -1));
                    break;

                case TypeCode.Decimal:
                    Decimal decLhs = m_decVal[0];
                    Decimal decRhs = rhs.m_decVal[0];
                    iRet = Global.g_bSortOrder ? (decLhs < decRhs ? -1 : (decLhs == decRhs ? 0 : 1)) : (decLhs < decRhs ? 1 : (decLhs == decRhs ? 0 : -1));
                    break;

                case TypeCode.DateTime:
                    DateTime dtLhs = m_dtVal[0];
                    DateTime dtRhs = rhs.m_dtVal[0];
                    iRet = Global.g_bSortOrder ? (dtLhs < dtRhs ? -1 : (dtLhs == dtRhs ? 0 : 1)) : (dtLhs < dtRhs ? 1 : (dtLhs == dtRhs ? 0 : -1));
                    break;

                case TypeCode.String:
                    string strLhs = m_strVal[0];
                    string strRhs = rhs.m_strVal[0];
                    iRet = Global.g_bSortOrder ? string.Compare(strLhs, strRhs, false) : string.Compare(strRhs, strLhs, false);
                    break;

                case TypeCode.Empty:
                    iRet = 0;
                    break;

                default:
                    iRet = 0;
                    break;
            }
            return iRet;
        }
        int IComparable.CompareTo(object rhs)
        {
            if (!(rhs is CVariant))
                throw new InvalidOperationException("CompareTo: Not a CVariant");
            return CompareTo((CVariant)rhs);
        }
        public static bool operator <(CVariant lhs, CVariant rhs) => lhs.CompareTo(rhs) < 0;
        public static bool operator >(CVariant lhs, CVariant rhs) => lhs.CompareTo(rhs) > 0;
        public bool Equals(CVariant rhs)
        {
            return CompareTo(rhs) == 0;
        }
        public override bool Equals(object rhs)
        {
            if (!(rhs is CVariant))
                return false;
            return Equals((CVariant)rhs);
        }
        public static bool operator ==(CVariant lhs, CVariant rhs) => lhs.Equals(rhs);
        public static bool operator !=(CVariant lhs, CVariant rhs) => !(lhs == rhs);

        public void Copy(CVariant rhs, int iFrom, int iTo)
        {
            switch (m_TypeCode)
            {
                case TypeCode.Boolean:
                    m_boolVal[iTo] = rhs.m_boolVal[iFrom];
                    break;

                case TypeCode.Char:
                    m_cVal[iTo] = rhs.m_cVal[iFrom];
                    break;

                case TypeCode.Byte:
                    m_byteVal[iTo] = rhs.m_byteVal[iFrom];
                    break;

                case TypeCode.SByte:
                    m_sbyteVal[iTo] = rhs.m_sbyteVal[iFrom];
                    break;

                case TypeCode.Int16:
                    m_int16Val[iTo] = rhs.m_int16Val[iFrom];
                    break;

                case TypeCode.UInt16:
                    m_uint16Val[iTo] = rhs.m_uint16Val[iFrom];
                    break;

                case TypeCode.Int32:
                    m_int32Val[iTo] = rhs.m_int32Val[iFrom];
                    break;

                case TypeCode.UInt32:
                    m_uint32Val[iTo] = rhs.m_uint32Val[iFrom];
                    break;

                case TypeCode.Int64:
                    m_int64Val[iTo] = rhs.m_int64Val[iFrom];
                    break;

                case TypeCode.UInt64:
                    m_uint64Val[iTo] = rhs.m_uint64Val[iFrom];
                    break;

                case TypeCode.Single:
                    m_fVal[iTo] = rhs.m_fVal[iFrom];
                    break;

                case TypeCode.Double:
                    m_dVal[iTo] = rhs.m_dVal[iFrom];
                    break;

                case TypeCode.Decimal:
                    m_decVal[iTo] = rhs.m_decVal[iFrom];
                    break;

                case TypeCode.DateTime:
                    m_dtVal[iTo] = rhs.m_dtVal[iFrom];
                    break;

                case TypeCode.String:
                    m_strVal[iTo] = rhs.m_strVal[iFrom];
                    break;

                case TypeCode.Empty:
                    break;

                default:
                    break;
            }
        }
        public void UpdateRow(int iRow)
        {
            m_arrRow[0] = iRow;
            m_arrRow[1] = iRow;
        }
        public string CellValue
        {
            get
            {
                string strCellValue = string.Empty;
                switch (m_TypeCode)
                {
                    case TypeCode.String:
                        if (m_strVal != null)
                            strCellValue = m_strVal[0];
                        break;

                    case TypeCode.Boolean:
                        if (m_boolVal != null)
                            strCellValue = m_boolVal[0].ToString();
                        break;

                    case TypeCode.Byte:
                        if (m_byteVal != null)
                            strCellValue = m_byteVal[0].ToString();
                        break;

                    case TypeCode.SByte:
                        if (m_sbyteVal != null)
                            strCellValue = m_sbyteVal[0].ToString();
                        break;

                    case TypeCode.Char:
                        if (m_cVal != null)
                            strCellValue = m_cVal[0].ToString();
                        break;

                    case TypeCode.Int16:
                        if (m_int16Val != null)
                            strCellValue = m_int16Val[0].ToString();
                        break;

                    case TypeCode.UInt16:
                        if (m_uint16Val != null)
                            strCellValue = m_uint16Val[0].ToString();
                        break;

                    case TypeCode.Int32:
                        if (m_int32Val != null)
                            strCellValue = m_int32Val[0].ToString();
                        break;

                    case TypeCode.UInt32:
                        if (m_uint32Val != null)
                            strCellValue = m_uint32Val[0].ToString();
                        break;

                    case TypeCode.Int64:
                        if (m_int64Val != null)
                            strCellValue = m_int64Val[0].ToString();
                        break;

                    case TypeCode.UInt64:
                        if (m_uint64Val != null)
                            strCellValue = m_uint64Val[0].ToString();
                        break;

                    case TypeCode.Single:
                        if (m_fVal != null)
                            strCellValue = m_fVal[0].ToString();
                        break;

                    case TypeCode.Double:
                        if (m_dVal != null)
                            strCellValue = m_dVal[0].ToString();
                        break;

                    case TypeCode.Decimal:
                        if (m_decVal != null)
                            strCellValue = m_decVal[0].ToString();
                        break;

                    case TypeCode.DateTime:
                        if (m_dtVal != null)
                            strCellValue = m_dtVal[0].ToString();
                        break;

                    case TypeCode.Empty:
                        break;
                }
                return strCellValue;
            }
        }

        public int Row
        {
            get { return m_arrRow[0]; }
        }
    }
    class CArray
    {
        private readonly CVariant[][] m_oData;
        public readonly int m_nRows;
        public readonly int m_nCols;
        public CArray(int nRows, int nCols)
        {
            m_oData = new CVariant[nCols][];
            for (int iCol = 0; iCol < nCols; ++iCol)
                m_oData[iCol] = new CVariant[nRows];
            m_nRows = nRows;
            m_nCols = nCols;
        }
        public CVariant[] this[int iCol]
        {
            get { return m_oData[iCol]; }
        }

        public void Sort(int iCol)
        {
            Array.Sort(m_oData[iCol]);

            for (int i = 0; i < m_nCols; ++i)
            {
                if (i != iCol)
                {
                    for (int j = 0, j2; j < m_nRows; ++j)
                    {
                        j2 = m_oData[iCol][j].Row;
                        if (j == j2)
                            continue;
                        m_oData[i][j].Copy(m_oData[i][j2], 0, 1);
                    }

                    for (int j = 0; j < m_nRows; ++j)
                    {
                        m_oData[i][j].Copy(m_oData[i][j], 1, 0);
                        m_oData[i][j].UpdateRow(j);
                    }
                }
            }

            for (int j = 0; j < m_nRows; ++j)
                m_oData[iCol][j].UpdateRow(j);
        }

        public void ParallelSort(int iCol)
        {
            Array.Sort(m_oData[iCol]);

            Parallel.For(0, m_nCols, i =>
            {
                if (i != iCol)
                {
                    for (int j = 0, j2; j < m_nRows; ++j)
                    {
                        j2 = m_oData[iCol][j].Row;
                        if (j == j2)
                            continue;
                        m_oData[i][j].Copy(m_oData[i][j2], 0, 1);
                    }

                    for (int j = 0; j < m_nRows; ++j)
                    {
                        m_oData[i][j].Copy(m_oData[i][j], 1, 0);
                        m_oData[i][j].UpdateRow(j);
                    }
                }
            });

            for (int j = 0; j < m_nRows; ++j)
                m_oData[iCol][j].UpdateRow(j);
        }
        public int Length
        {
            get { return m_nRows; }
        }
    }
}