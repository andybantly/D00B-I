using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D00B
{
    public class CVariant : IComparable<CVariant>, IEquatable<CVariant>, IComparable
    {
        private readonly TypeCode[] m_TypeCode = new TypeCode[2];
        private readonly Boolean[] m_boolVal = new Boolean[2];
        private readonly Char[] m_cVal = new Char[2];
        private readonly Byte[] m_byteVal = new Byte[2];
        private readonly SByte[] m_sbyteVal = new SByte[2];
        private readonly Int16[] m_int16Val = new Int16[2];
        private readonly UInt16[] m_uint16Val = new UInt16[2];
        private readonly Int32[] m_int32Val = new Int32[2];
        private readonly UInt32[] m_uint32Val = new UInt32[2];
        private readonly Int64[] m_int64Val = new Int64[2];
        private readonly UInt64[] m_uint64Val = new UInt64[2];
        private readonly Single[] m_fVal = new Single[2];
        private readonly Double[] m_dVal = new Double[2];
        private readonly Decimal[] m_decVal = new Decimal[2];
        private readonly DateTime[] m_dtVal = new DateTime[2];
        private readonly String[] m_strVal = new String[2];
        private readonly int[] m_arrRow = new int[2];
        private bool[] m_bNull = new bool[] { false, false };

        public CVariant()
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Empty;
                m_bNull[i] = true;
            }
        }

        public CVariant(TypeCode TypeCode)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode;
                m_bNull[i] = true;
                switch (TypeCode)
                {
                    case TypeCode.Boolean:
                        m_boolVal[i] = false;
                        break;

                    case TypeCode.Char:
                        m_cVal[i] = Char.MinValue;
                        break;

                    case TypeCode.Byte:
                        m_byteVal[i] = Byte.MinValue;
                        break;

                    case TypeCode.SByte:
                        m_sbyteVal[i] = SByte.MinValue;
                        break;

                    case TypeCode.Int16:
                        m_int16Val[i] = Int16.MinValue;
                        break;

                    case TypeCode.UInt16:
                        m_uint16Val[i] = UInt16.MinValue;
                        break;

                    case TypeCode.Int32:
                        m_int32Val[i] = Int32.MinValue;
                        break;

                    case TypeCode.UInt32:
                        m_uint32Val[i] = UInt32.MinValue;
                        break;

                    case TypeCode.Int64:
                        m_int64Val[i] = Int64.MinValue;
                        break;

                    case TypeCode.UInt64:
                        m_uint64Val[i] = UInt64.MinValue;
                        break;

                    case TypeCode.Single:
                        m_fVal[i] = Single.MinValue;
                        break;

                    case TypeCode.Double:
                        m_dVal[i] = Double.MinValue;
                        break;

                    case TypeCode.Decimal:
                        m_decVal[i] = Decimal.MinValue;
                        break;

                    case TypeCode.DateTime:
                        m_dtVal[i] = DateTime.MinValue;
                        break;

                    case TypeCode.String:
                        m_strVal[i] = string.Empty;
                        break;

                    default:
                        break;
                }
            }
        }
        public CVariant(Boolean boolVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Boolean;
                m_boolVal[i] = boolVal;
            }
        }
        public CVariant(Char cVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Char;
                m_cVal[i] = cVal;
            }
        }

        public CVariant(Byte byteVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Byte;
                m_byteVal[i] = byteVal;
            }
        }

        public CVariant(SByte sbyteVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.SByte;
                m_sbyteVal[i] = sbyteVal;
            }
        }
        public CVariant(Int16 int16Val)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Int16;
                m_int16Val[i] = int16Val;
            }
        }
        public CVariant(UInt16 uint16Val)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.UInt16;
                m_uint16Val[i] = uint16Val;
            }
        }
        public CVariant(Int32 int32Val)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Int32;
                m_int32Val[i] = int32Val;
            }
        }
        public CVariant(UInt32 uint32Val)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.UInt32;
                m_uint32Val[i] = uint32Val;
            }
        }
        public CVariant(Int64 int64Val)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Int64;
                m_int64Val[i] = int64Val;
            }
        }
        public CVariant(UInt64 uint64Val)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.UInt64;
                m_uint64Val[i] = uint64Val;
            }
        }
        public CVariant(Single fVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Single;
                m_fVal[i] = fVal;
            }
        }
        public CVariant(Double dVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Double;
                m_dVal[i] = dVal;
            }
        }
        public CVariant(Decimal decVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.Decimal;
                m_decVal[i] = decVal;
            }
        }
        public CVariant(DateTime dtVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.DateTime;
                m_dtVal[i] = dtVal;
            }
        }
        public CVariant(String strVal)
        {
            for (int i = 0; i < 2; ++i)
            {
                m_TypeCode[i] = TypeCode.String;
                m_strVal[i] = strVal;
            }
        }
        public override string ToString()
        {
            return CellValue;
        }
        public override int GetHashCode()
        {
            return Utility.GetHashCode(ToString());
        }
        public int CompareTo(CVariant rhs)
        {
            int iRet;

            if (m_TypeCode[0] == rhs.m_TypeCode[0])
            {
                switch (m_TypeCode[0])
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
            }
            else
            {
                string strLhs = CellValue;
                string strRhs = rhs.CellValue;

                Double d64Lhs = 0;
                bool bLhsDbl = false, bLhsInt = Int64.TryParse(strLhs, out Int64 i64Lhs);
                if (!bLhsInt)
                    bLhsDbl = Double.TryParse(strLhs, out d64Lhs);

                Double d64Rhs = 0;
                bool bRhsDbl = false, bRhsInt = Int64.TryParse(strRhs, out Int64 i64Rhs);
                if (!bRhsInt)
                    bRhsDbl = Double.TryParse(strRhs, out d64Rhs);

                if (bLhsInt && bRhsInt)
                    iRet = Global.g_bSortOrder ? (i64Lhs < i64Rhs ? -1 : (i64Lhs == i64Rhs ? 0 : 1)) : (i64Lhs < i64Rhs ? 1 : (i64Lhs == i64Rhs ? 0 : -1));
                else if (bLhsDbl && bRhsDbl)
                    iRet = Global.g_bSortOrder ? (d64Lhs < d64Rhs ? -1 : (d64Lhs == d64Rhs ? 0 : 1)) : (d64Lhs < d64Rhs ? 1 : (d64Lhs == d64Rhs ? 0 : -1));
                else if (bLhsInt && bRhsDbl)
                    iRet = Global.g_bSortOrder ? (i64Lhs < d64Rhs ? -1 : (i64Lhs == d64Rhs ? 0 : 1)) : (i64Lhs < d64Rhs ? 1 : (i64Lhs == d64Rhs ? 0 : -1));
                else if (bRhsInt && bLhsDbl)
                    iRet = Global.g_bSortOrder ? (d64Lhs < i64Rhs ? -1 : (d64Lhs == i64Rhs ? 0 : 1)) : (d64Lhs < i64Rhs ? 1 : (d64Lhs == i64Rhs ? 0 : -1));
                else // Lexicographic compare
                    iRet = Global.g_bSortOrder ? string.Compare(strLhs, strRhs, false) : string.Compare(strRhs, strLhs, false);
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
            switch (rhs.m_TypeCode[iFrom])
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

            m_bNull[iTo] = rhs.m_bNull[iFrom];
            m_TypeCode[iTo] = rhs.m_TypeCode[iFrom];
        }
        public void UpdateRow(int iRow)
        {
            m_TypeCode[0] = m_TypeCode[1];
            for (int i = 0; i < 1; ++i)
                m_arrRow[i] = iRow;
        }
        public string CellValue
        {
            get
            {
                string strCellValue = string.Empty;
                if (!m_bNull[0])
                {
                    switch (m_TypeCode[0])
                    {
                        case TypeCode.String:
                            strCellValue = m_strVal[0];
                            break;

                        case TypeCode.Boolean:
                            strCellValue = m_boolVal[0].ToString();
                            break;

                        case TypeCode.Byte:
                            strCellValue = m_byteVal[0].ToString();
                            break;

                        case TypeCode.SByte:
                            strCellValue = m_sbyteVal[0].ToString();
                            break;

                        case TypeCode.Char:
                            strCellValue = m_cVal[0].ToString();
                            break;

                        case TypeCode.Int16:
                            strCellValue = m_int16Val[0].ToString();
                            break;

                        case TypeCode.UInt16:
                            strCellValue = m_uint16Val[0].ToString();
                            break;

                        case TypeCode.Int32:
                            strCellValue = m_int32Val[0].ToString();
                            break;

                        case TypeCode.UInt32:
                            strCellValue = m_uint32Val[0].ToString();
                            break;

                        case TypeCode.Int64:
                            strCellValue = m_int64Val[0].ToString();
                            break;

                        case TypeCode.UInt64:
                            strCellValue = m_uint64Val[0].ToString();
                            break;

                        case TypeCode.Single:
                            strCellValue = m_fVal[0].ToString();
                            break;

                        case TypeCode.Double:
                            strCellValue = m_dVal[0].ToString();
                            break;

                        case TypeCode.Decimal:
                            strCellValue = m_decVal[0].ToString();
                            break;

                        case TypeCode.DateTime:
                            strCellValue = m_dtVal[0].ToString();
                            break;

                        case TypeCode.Empty:
                            strCellValue = string.Empty;
                            break;
                    }
                }
                else
                    strCellValue = "NULL";
                return strCellValue;
            }
        }

        public int Row
        {
            get { return m_arrRow[0]; }
        }
    }
}
