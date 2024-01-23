using System.Collections.Generic;

namespace D00B
{
    internal class Maze
    {
        readonly Dictionary<DBTableKey, DBTable> m_TableMap;
        string m_strSrcSchema = string.Empty;
        string m_strSrcTable = string.Empty;
        string m_strSrcColumn = string.Empty;
        string m_strJoinSchema = string.Empty;
        string m_strJoinTable = string.Empty;
        string m_strJoinColumn = string.Empty;
        List<DBTableKey> m_TablePath;
        public Maze(Dictionary<DBTableKey, DBTable> TableMap)
        {
            m_TableMap = TableMap;
        }

        public string SourceSchema
        {
            get { return m_strSrcSchema; }
            set { if (!string.IsNullOrEmpty(value)) m_strSrcSchema = value; }
        }
        public string SourceTable
        {
            get { return m_strSrcTable; }
            set { if (!string.IsNullOrEmpty(value)) m_strSrcTable = value; }
        }
        public string SourceColumn
        {
            get { return m_strSrcColumn; }
            set { if (!string.IsNullOrEmpty(value)) m_strSrcColumn = value; }
        }
        public string JoinSchema
        {
            get { return m_strJoinSchema; }
            set { if (!string.IsNullOrEmpty(value)) m_strJoinSchema = value; }
        }
        public string JoinTable
        {
            get { return m_strJoinTable; }
            set { if (!string.IsNullOrEmpty(value)) m_strJoinTable = value; }
        }
        public string JoinColumn
        {
            get { return m_strJoinColumn; }
            set { if (!string.IsNullOrEmpty(value)) m_strJoinColumn = value; }
        }

        private void Walk(DBTableKey Key, DBTable Table)
        {
            if (!Table.Visited)
            {
                Table.Visited = true;

                System.Diagnostics.Debug.Write(string.Format("{0}.{1}", Key.Schema, Table.TableName));
                m_TablePath.Add(new DBTableKey(Key.Schema, Table.TableName, string.Empty));

                foreach (DBTableKey PK in Table.Keys)
                {
                    DBTableKey TK = new DBTableKey(PK.Schema, PK.Table, string.Empty);
                    if (m_TableMap.ContainsKey(TK))
                    {
                        DBTable DestTable = m_TableMap[TK];
                        System.Diagnostics.Debug.Write(string.Format("->{0}.{1}.{2}", PK.Schema, DestTable.TableName, PK.Column));
                        Walk(TK, DestTable);
                    }
                }
            }
        }

        public void DFS()
        {
            DBTableKey Begin = new DBTableKey(SourceSchema, SourceTable, string.Empty);
            DBTableKey End = new DBTableKey(JoinSchema, JoinTable, string.Empty);
            DBTable BeginTable = m_TableMap[Begin];
            DBTable EndTable = m_TableMap[End];
            m_TablePath = new List<DBTableKey>();
            Walk(Begin, BeginTable);
            System.Diagnostics.Debug.WriteLine("");
        }
    }
}
