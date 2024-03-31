using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D00B
{
    public class CVariant : IComparable<CVariant>, IEquatable<CVariant>, IComparable, ICloneable
    {
        private TypeCode[] m_TypeCode;
        private Boolean[] m_boolVal;
        private Char[] m_cVal;
        private Byte[] m_byteVal;
        private SByte[] m_sbyteVal;
        private Int16[] m_int16Val;
        private UInt16[] m_uint16Val;
        private Int32[] m_int32Val;
        private UInt32[] m_uint32Val;
        private Int64[] m_int64Val;
        private UInt64[] m_uint64Val;
        private Single[] m_fVal;
        private Double[] m_dVal;
        private Decimal[] m_decVal;
        private DateTime[] m_dtVal;
        private String[] m_strVal;
        private readonly int[] m_arrRow = new int[2];
        private bool[] m_bNull;

        public CVariant()
        {
            m_TypeCode = new TypeCode[] { TypeCode.Empty, TypeCode.Empty };
            m_bNull = new bool[] { true, true };
        }

        public CVariant(TypeCode TypeCode)
        {
            m_TypeCode = new TypeCode[] { TypeCode, TypeCode };
            m_bNull = new bool[] { true, true };
        }
        public CVariant(Boolean boolVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Boolean, TypeCode.Boolean };
            m_boolVal = new Boolean[] { boolVal, boolVal };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(Char cVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Char, TypeCode.Char };
            m_cVal = new Char[] { cVal, cVal };
            m_bNull = new bool[] { false, false };
        }

        public CVariant(Byte byteVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Byte, TypeCode.Byte };
            m_byteVal = new Byte[] { byteVal, byteVal };
            m_bNull = new bool[] { false, false };
        }

        public CVariant(SByte sbyteVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.SByte, TypeCode.SByte };
            m_sbyteVal = new SByte[] { sbyteVal, sbyteVal };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(Int16 int16Val)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Int16, TypeCode.Int16 };
            m_int16Val = new Int16[] { int16Val, int16Val };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(UInt16 uint16Val)
        {
            m_TypeCode = new TypeCode[] { TypeCode.UInt16, TypeCode.UInt16 };
            m_uint16Val = new UInt16[] { uint16Val, uint16Val };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(Int32 int32Val)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Int32, TypeCode.Int32 };
            m_int32Val = new Int32[] { int32Val, int32Val };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(UInt32 uint32Val)
        {
            m_TypeCode = new TypeCode[] { TypeCode.UInt32, TypeCode.UInt32};
            m_uint32Val = new UInt32[] { uint32Val, uint32Val };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(Int64 int64Val)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Int64, TypeCode.Int64 };
            m_int64Val = new Int64[] { int64Val, int64Val };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(UInt64 uint64Val)
        {
            m_TypeCode = new TypeCode[] { TypeCode.UInt64, TypeCode.UInt64 };
            m_uint64Val = new UInt64[] { uint64Val, uint64Val };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(Single fVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Single, TypeCode.Single };
            m_fVal = new Single[] { fVal, fVal };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(Double dVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Double, TypeCode.Double };
            m_dVal = new Double[] { dVal, dVal };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(Decimal decVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.Decimal, TypeCode.Decimal };
            m_decVal = new Decimal[] { decVal, decVal };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(DateTime dtVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.DateTime, TypeCode.DateTime };
            m_dtVal = new DateTime[] { dtVal, dtVal };
            m_bNull = new bool[] { false, false };
        }
        public CVariant(String strVal)
        {
            m_TypeCode = new TypeCode[] { TypeCode.String, TypeCode.String };
            m_strVal = new string[] { strVal, strVal };
            m_bNull = new bool[] { false, false };
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public override int GetHashCode()
        {
            return Utility.GetHashCode(ToString());
        }
        public int CompareTo(CVariant rhs)
        {
            int iRet;

            if (TypeCode == rhs.TypeCode)
            {
                switch (TypeCode)
                {
                    case TypeCode.Boolean:
                        Boolean boolLhs = m_boolVal != null ? m_boolVal[0] : false;
                        Boolean boolRhs = rhs.m_boolVal != null ? rhs.m_boolVal[0] : false;
                        iRet = Global.g_bSortOrder ? (boolLhs == true && boolRhs == false ? -1 : (boolLhs == boolRhs ? 0 : 1)) : (boolLhs == true && boolRhs == false ? 1 : (boolLhs == boolRhs ? 0 : -1));
                        break;

                    case TypeCode.Byte:
                        Byte bLhs = m_byteVal != null ? m_byteVal[0] : Byte.MinValue;
                        Byte bRhs = rhs.m_byteVal != null ? rhs.m_byteVal[0] : Byte.MinValue;
                        iRet = Global.g_bSortOrder ? (bLhs < bRhs ? -1 : (bLhs == bRhs ? 0 : 1)) : (bLhs < bRhs ? 1 : (bLhs == bRhs ? 0 : -1));
                        break;

                    case TypeCode.SByte:
                        SByte sbLhs = m_sbyteVal != null ? m_sbyteVal[0] : SByte.MinValue;
                        SByte sbRhs = rhs.m_sbyteVal != null ? rhs.m_sbyteVal[0] : SByte.MinValue;
                        iRet = Global.g_bSortOrder ? (sbLhs < sbRhs ? -1 : (sbLhs == sbRhs ? 0 : 1)) : (sbLhs < sbRhs ? 1 : (sbLhs == sbRhs ? 0 : -1));
                        break;

                    case TypeCode.Char:
                        Char cLhs = m_cVal != null ? m_cVal[0] : Char.MinValue;
                        Char cRhs = rhs.m_cVal != null ? rhs.m_cVal[0] : Char.MinValue;
                        iRet = Global.g_bSortOrder ? (cLhs < cRhs ? -1 : (cLhs == cRhs ? 0 : 1)) : (cLhs < cRhs ? 1 : (cLhs == cRhs ? 0 : -1));
                        break;

                    case TypeCode.Int16:
                        Int16 i16Lhs = m_int16Val != null ? m_int16Val[0] : Int16.MinValue;
                        Int16 i16Rhs = rhs.m_int16Val != null ? rhs.m_int16Val[0] : Int16.MinValue;
                        iRet = Global.g_bSortOrder ? (i16Lhs < i16Rhs ? -1 : (i16Lhs == i16Rhs ? 0 : 1)) : (i16Lhs < i16Rhs ? 1 : (i16Lhs == i16Rhs ? 0 : -1));
                        break;

                    case TypeCode.UInt16:
                        UInt16 ui16Lhs = m_uint16Val != null ? m_uint16Val[0] : UInt16.MinValue;
                        UInt16 ui16Rhs = rhs.m_uint16Val != null ? rhs.m_uint16Val[0] : UInt16.MinValue;
                        iRet = Global.g_bSortOrder ? (ui16Lhs < ui16Rhs ? -1 : (ui16Lhs == ui16Rhs ? 0 : 1)) : (ui16Lhs < ui16Rhs ? 1 : (ui16Lhs == ui16Rhs ? 0 : -1));
                        break;

                    case TypeCode.Int32:
                        Int32 i32Lhs = m_int32Val != null ? m_int32Val[0] : Int32.MinValue;
                        Int32 i32Rhs = rhs.m_int32Val != null ? rhs.m_int32Val[0] : Int32.MinValue;
                        iRet = Global.g_bSortOrder ? (i32Lhs < i32Rhs ? -1 : (i32Lhs == i32Rhs ? 0 : 1)) : (i32Lhs < i32Rhs ? 1 : (i32Lhs == i32Rhs ? 0 : -1));
                        break;

                    case TypeCode.UInt32:
                        UInt32 ui32Lhs = m_uint32Val != null ? m_uint32Val[0] : UInt32.MinValue;
                        UInt32 ui32Rhs = rhs.m_uint32Val != null ? rhs.m_uint32Val[0] : UInt32.MinValue;
                        iRet = Global.g_bSortOrder ? (ui32Lhs < ui32Rhs ? -1 : (ui32Lhs == ui32Rhs ? 0 : 1)) : (ui32Lhs < ui32Rhs ? 1 : (ui32Lhs == ui32Rhs ? 0 : -1));
                        break;

                    case TypeCode.Int64:
                        Int64 i64Lhs = m_int64Val != null ? m_int64Val[0] : Int64.MinValue;
                        Int64 i64Rhs = rhs.m_int64Val != null ? rhs.m_int64Val[0] : Int64.MinValue;
                        iRet = Global.g_bSortOrder ? (i64Lhs < i64Rhs ? -1 : (i64Lhs == i64Rhs ? 0 : 1)) : (i64Lhs < i64Rhs ? 1 : (i64Lhs == i64Rhs ? 0 : -1));
                        break;

                    case TypeCode.UInt64:
                        UInt64 ui64Lhs = m_uint64Val != null ? m_uint64Val[0] : UInt64.MinValue;
                        UInt64 ui64Rhs = rhs.m_uint64Val != null ? rhs.m_uint64Val[0] : UInt64.MinValue;
                        iRet = Global.g_bSortOrder ? (ui64Lhs < ui64Rhs ? -1 : (ui64Lhs == ui64Rhs ? 0 : 1)) : (ui64Lhs < ui64Rhs ? 1 : (ui64Lhs == ui64Rhs ? 0 : -1));
                        break;

                    case TypeCode.Single:
                        Single fLhs = m_fVal != null ? m_fVal[0] : Single.MinValue;
                        Single fRhs = rhs.m_fVal != null ? rhs.m_fVal[0] : Single.MinValue;
                        iRet = Global.g_bSortOrder ? (fLhs < fRhs ? -1 : (fLhs == fRhs ? 0 : 1)) : (fLhs < fRhs ? 1 : (fLhs == fRhs ? 0 : -1));
                        break;

                    case TypeCode.Double:
                        Double dLhs = m_dVal != null ? m_dVal[0] : Double.MinValue;
                        Double dRhs = rhs.m_dVal != null ? rhs.m_dVal[0] : Double.MinValue;
                        iRet = Global.g_bSortOrder ? (dLhs < dRhs ? -1 : (dLhs == dRhs ? 0 : 1)) : (dLhs < dRhs ? 1 : (dLhs == dRhs ? 0 : -1));
                        break;

                    case TypeCode.Decimal:
                        Decimal decLhs = m_decVal != null ? m_decVal[0] : Decimal.MinValue;
                        Decimal decRhs = rhs.m_decVal != null ? rhs.m_decVal[0] : Decimal.MinValue;
                        iRet = Global.g_bSortOrder ? (decLhs < decRhs ? -1 : (decLhs == decRhs ? 0 : 1)) : (decLhs < decRhs ? 1 : (decLhs == decRhs ? 0 : -1));
                        break;

                    case TypeCode.DateTime:
                        DateTime dtLhs = m_dtVal != null ? m_dtVal[0] : DateTime.MinValue;
                        DateTime dtRhs = rhs.m_dtVal != null ? rhs.m_dtVal[0] : DateTime.MinValue;
                        iRet = Global.g_bSortOrder ? (dtLhs < dtRhs ? -1 : (dtLhs == dtRhs ? 0 : 1)) : (dtLhs < dtRhs ? 1 : (dtLhs == dtRhs ? 0 : -1));
                        break;

                    case TypeCode.String:
                        string strLhs = m_strVal != null ? m_strVal[0] : String.Empty;
                        string strRhs = rhs.m_strVal != null ? rhs.m_strVal[0] : String.Empty;
                        iRet = Global.g_bSortOrder ? string.Compare(strLhs, strRhs, false) : string.Compare(strRhs, strLhs, false);
                        break;

                    case TypeCode.Empty:
                    default:
                        iRet = 0;
                        break;
                }
            }
            else
            {
                string strLhs = Value.ToString();
                string strRhs = rhs.Value.ToString();
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
        public object Clone()
        {
            CVariant oClone;
            switch (TypeCode)
            {
                case TypeCode.Boolean:
                    oClone = m_boolVal != null ? new CVariant(m_boolVal[0]) : new CVariant(TypeCode.Boolean);
                    break;
                case TypeCode.Char:
                    oClone = m_cVal != null ? new CVariant(m_cVal[0]) : new CVariant(TypeCode.Char);
                    break;
                case TypeCode.Byte:
                    oClone = m_byteVal != null ? new CVariant(m_byteVal[0]) : new CVariant(TypeCode.Byte);
                    break;
                case TypeCode.SByte:
                    oClone = m_sbyteVal != null ? new CVariant(m_sbyteVal[0]) : new CVariant(TypeCode.SByte);
                    break;
                case TypeCode.Int16:
                    oClone = m_int16Val != null ? new CVariant(m_int16Val[0]) : new CVariant(TypeCode.Int16);
                    break;
                case TypeCode.UInt16:
                    oClone = m_uint16Val != null ? new CVariant(m_uint16Val[0]) : new CVariant(TypeCode.UInt16);
                    break;
                case TypeCode.Int32:
                    oClone = m_int32Val != null ? new CVariant(m_int32Val[0]) : new CVariant(TypeCode.Int32);
                    break;
                case TypeCode.UInt32:
                    oClone = m_uint32Val != null ? new CVariant(m_uint32Val[0]) : new CVariant(TypeCode.UInt32);
                    break;
                case TypeCode.Int64:
                    oClone = m_int64Val != null ? new CVariant(m_int64Val[0]) : new CVariant(TypeCode.Int64);
                    break;
                case TypeCode.UInt64:
                    oClone = m_uint64Val != null ? new CVariant(m_uint64Val[0]) : new CVariant(TypeCode.UInt64);
                    break;
                case TypeCode.Single:
                    oClone = m_fVal != null ? new CVariant(m_fVal[0]) : new CVariant(TypeCode.Single);
                    break;
                case TypeCode.Double:
                    oClone = m_dVal != null ? new CVariant(m_dVal[0]) : new CVariant(TypeCode.Double);
                    break;
                case TypeCode.Decimal:
                    oClone = m_decVal != null ? new CVariant(m_decVal[0]) : new CVariant(TypeCode.Decimal);
                    break;
                case TypeCode.DateTime:
                    oClone = m_dtVal != null ? new CVariant(m_dtVal[0]) : new CVariant(TypeCode.DateTime);
                    break;
                case TypeCode.String:
                    oClone = m_strVal != null ? new CVariant(m_strVal[0]) : new CVariant(TypeCode.String);
                    break;
                default:
                    oClone = new CVariant();
                    break;
            }
            return oClone;
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
                    if (rhs.m_boolVal != null)
                    {
                        if (m_boolVal == null)
                            m_boolVal = new Boolean[2];
                        m_boolVal[iTo] = rhs.m_boolVal[iFrom];
                    }
                    break;

                case TypeCode.Char:
                    if (rhs.m_cVal != null)
                    {
                        if (m_cVal == null)
                            m_cVal = new Char[2];
                        m_cVal[iTo] = rhs.m_cVal[iFrom];
                    }
                    break;

                case TypeCode.Byte:
                    if (rhs.m_byteVal != null)
                    {
                        if (m_byteVal == null)
                            m_byteVal = new Byte[2];
                        m_byteVal[iTo] = rhs.m_byteVal[iFrom];
                    }
                    break;

                case TypeCode.SByte:
                    if (rhs.m_sbyteVal != null)
                    {
                        if (m_sbyteVal == null)
                            m_sbyteVal = new SByte[2];
                        m_sbyteVal[iTo] = rhs.m_sbyteVal[iFrom];
                    }
                    break;

                case TypeCode.Int16:
                    if (rhs.m_int16Val != null)
                    {
                        if (m_int16Val == null)
                            m_int16Val = new Int16[2];
                        m_int16Val[iTo] = rhs.m_int16Val[iFrom];
                    }
                    break;

                case TypeCode.UInt16:
                    if (rhs.m_uint16Val != null)
                    {
                        if (m_uint16Val == null)
                            m_uint16Val = new UInt16[2];
                        m_uint16Val[iTo] = rhs.m_uint16Val[iFrom];
                    }
                    break;

                case TypeCode.Int32:
                    if (rhs.m_int32Val != null)
                    {
                        if (m_int32Val == null)
                            m_int32Val = new Int32[2];
                        m_int32Val[iTo] = rhs.m_int32Val[iFrom];
                    }
                    break;

                case TypeCode.UInt32:
                    if (rhs.m_uint32Val != null)
                    {
                        if (m_uint32Val == null)
                            m_uint32Val = new UInt32[2];
                        m_uint32Val[iTo] = rhs.m_uint32Val[iFrom];
                    }
                    break;

                case TypeCode.Int64:
                    if (rhs.m_int64Val != null)
                    {
                        if (m_int64Val == null)
                            m_int64Val = new Int64[2];
                        m_int64Val[iTo] = rhs.m_int64Val[iFrom];
                    }
                    break;

                case TypeCode.UInt64:
                    if (rhs.m_uint64Val != null)
                    {
                        if (m_uint64Val == null)
                            m_uint64Val = new UInt64[2];
                        m_uint64Val[iTo] = rhs.m_uint64Val[iFrom];
                    }
                    break;

                case TypeCode.Single:
                    if (rhs.m_fVal != null)
                    {
                        if (m_fVal == null)
                            m_fVal = new Single[2];
                        m_fVal[iTo] = rhs.m_fVal[iFrom];
                    }
                    break;

                case TypeCode.Double:
                    if (rhs.m_dVal != null)
                    {
                        if (m_dVal == null)
                            m_dVal = new Double[2];
                        m_dVal[iTo] = rhs.m_dVal[iFrom];
                    }
                    break;

                case TypeCode.Decimal:
                    if (rhs.m_decVal != null)
                    {
                        if (m_decVal == null)
                            m_decVal = new Decimal[2];
                        m_decVal[iTo] = rhs.m_decVal[iFrom];
                    }
                    break;

                case TypeCode.DateTime:
                    if (rhs.m_dtVal != null)
                    {
                        if (m_dtVal == null)
                            m_dtVal = new DateTime[2];
                        m_dtVal[iTo] = rhs.m_dtVal[iFrom];
                    }
                    break;

                case TypeCode.String:
                    if (rhs.m_strVal != null)
                    {
                        if (m_strVal == null)
                            m_strVal = new String[2];
                        m_strVal[iTo] = rhs.m_strVal[iFrom];
                    }
                    break;

                case TypeCode.Empty:
                default:
                    break;
            }

            m_bNull[iTo] = rhs.m_bNull[iFrom];
            m_TypeCode[iTo] = rhs.m_TypeCode[iFrom];
        }
        public void UpdateRow(int iRow)
        {
            if (m_TypeCode[0] != m_TypeCode[1])
                m_TypeCode[0] = m_TypeCode[1];
            m_arrRow[0] = iRow;
            m_arrRow[1] = iRow;
        }
        public bool Null
        {
            get { return m_bNull[0]; }
        }
        public TypeCode TypeCode
        {
            get { return m_TypeCode[0]; }
        }

        public object Value
        {
            get
            {
                string strCellValue = string.Empty;
                if (!Null)
                {
                    switch (TypeCode)
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
