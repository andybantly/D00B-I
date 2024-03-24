using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Runtime;

namespace D00B
{
    internal class SQL
    {
        private SqlConnection m_Connection;
        private SqlCommand m_Command;
        private SqlDataReader m_Reader;

        private string m_strConnectionString = string.Empty;
        private string m_strQueryString = string.Empty;
        private bool m_bStoredProcedure = false;

        private readonly StringCollection m_Columns = new StringCollection();
        private object[] m_CurrentRow = null;
        private readonly List<TypeCode> m_ColTypes = new List<TypeCode>();
        private int m_nFieldCount = 0;

        public SQL()
        {
            m_Connection = new SqlConnection();
        }

        ~SQL()
        {
            Close();
        }

        public SQL(string strConnectionString) : this()
        {
            if (!string.IsNullOrEmpty(strConnectionString))
                ConnectionString = strConnectionString;
        }

        public SQL(string strConnectionString, string strQueryString) : this(strConnectionString)
        {
            if (!string.IsNullOrEmpty(strQueryString))
                QueryString = strQueryString;
        }

        public SQL(string strConnectionString, string strQueryString, bool bStoredProcedure = false) : this(strConnectionString, strQueryString)
        {
            m_bStoredProcedure = bStoredProcedure;
        }

        public string ConnectionString
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    m_strConnectionString = value;
                    m_Connection.ConnectionString = m_strConnectionString;
                }
            }
        }

        protected bool HasConnectionString
        {
            get
            {
                return !string.IsNullOrEmpty(m_strConnectionString);
            }
        }

        public string QueryString
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                    m_strQueryString = value;
            }
        }

        protected bool HasQueryString
        {
            get
            {
                return !string.IsNullOrEmpty(m_strQueryString);
            }
        }

        public bool IsStoredProcedure
        {
            get { return m_bStoredProcedure; }
            set { m_bStoredProcedure = value; }
        }

        public bool ExecuteReader(out string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            if (!HasConnectionString || !HasQueryString)
                return false;

            if (m_Command == null)
            {
                m_Command = new SqlCommand(m_strQueryString, m_Connection);
                if (IsStoredProcedure)
                    m_Command.CommandType = CommandType.StoredProcedure;
                m_Command.CommandTimeout = 0;
            }

            try
            {
                m_Connection.Open();
                m_Reader = m_Command.ExecuteReader();
                for (int iField = 0; iField < m_Reader.FieldCount; iField++)
                {
                    string strColumnName = m_Reader.GetName(iField);
                    m_Columns.Add(strColumnName);
                    Type Type = m_Reader.GetFieldType(iField);
                    TypeCode TypeCode = Type.GetTypeCode(Type);
                    if (TypeCode == TypeCode.Object || TypeCode == TypeCode.DBNull)
                        TypeCode = TypeCode.Empty;
                    m_ColTypes.Add(TypeCode);
                }
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool ExecuteNonQuery(out string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            if (!HasConnectionString || !HasQueryString)
                return false;

            if (m_Command == null)
            {
                m_Command = new SqlCommand(m_strQueryString, m_Connection);
                if (IsStoredProcedure)
                    m_Command.CommandType = CommandType.StoredProcedure;
                m_Command.CommandTimeout = 0;
            }

            try
            {
                m_Connection.Open();
                m_Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public void AddWithValue(string strKey, string strValue, SqlDbType Type = SqlDbType.Int)
        {
            if (m_Command == null)
            {
                if (!HasConnectionString || !HasQueryString)
                    return;

                m_Command = new SqlCommand(m_strQueryString, m_Connection);
                if (IsStoredProcedure)
                    m_Command.CommandType = CommandType.StoredProcedure;
                m_Command.CommandTimeout = 0;
            }
            if (Type == SqlDbType.NVarChar)
                m_Command.Parameters.Add(strKey, Type).Value = strValue;
            else
                m_Command.Parameters.Add(strKey, Type).Value = Convert.ToInt32(strValue);
        }
        public object ExecuteScalar(out string strErrorMessage)
        {
            strErrorMessage = string.Empty;
            object obj;
            if (!HasConnectionString || !HasQueryString)
                return null;

            if (m_Command == null)
            {
                m_Command = new SqlCommand(m_strQueryString, m_Connection);
                if (IsStoredProcedure)
                    m_Command.CommandType = CommandType.StoredProcedure;
                m_Command.CommandTimeout = 0;
            }

            try
            {
                m_Connection.Open();
                obj = m_Command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                return null;
            }
            return obj;
        }

        public bool Read()
        {
            if (m_Reader != null && m_Reader.IsClosed)
                return false;

            bool bReturn = m_Reader != null && m_Reader.Read();
            if (bReturn)
            {
                m_nFieldCount = m_Reader.FieldCount;
                m_CurrentRow = new object[m_nFieldCount];

                bool bReadRow = true;
                for (int iField = 0; bReadRow && iField < m_nFieldCount; iField++)
                {
                    if (m_ColTypes[iField] == TypeCode.Empty)
                        bReadRow = false;
                }

                if (bReadRow)
                    m_Reader.GetValues(m_CurrentRow);
                else
                {
                    for (int iField = 0; iField < m_nFieldCount; iField++)
                    {
                        if (m_ColTypes[iField] != TypeCode.Empty)
                            m_CurrentRow[iField] = m_Reader.GetValue(iField);
                        else
                            m_CurrentRow[iField] = null;
                    }
                }

                for (int iField = 0; iField < m_nFieldCount; iField++)
                    if (m_CurrentRow[iField] == DBNull.Value)
                            m_CurrentRow[iField] = null;
            }
            else
            {
                if (m_Reader != null)
                    m_Reader.Close();
            }
            return bReturn;
        }

        public int FieldCount
        {
            get
            {
                return m_nFieldCount;
            }
        }

        public object GetValue(int iCol)
        {
            return iCol < m_nFieldCount ? m_CurrentRow[iCol] : null;
        }

        public object GetValue(string strColumnName)
        {
            return Columns.Contains(strColumnName) ? m_CurrentRow[Columns.IndexOf(strColumnName)] : null;
        }

        public StringCollection Columns
        {
            get { return m_Columns; }
        }

        public List<TypeCode> ColumnTypes
        {
            get { return m_ColTypes; }
        }

        private void Cleanup()
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
        }
        public void Close()
        {
            if (m_Reader != null)
                m_Reader = null;
            if (m_Command != null)
                m_Command = null;
            if (m_Connection != null)
                m_Connection = null;
            Cleanup();
        }
    }
}
