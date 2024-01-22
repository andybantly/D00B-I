using System.Collections.Generic;

namespace D00B
{
    internal class Maze
    {
        Dictionary<DBTableKey, DBTable> m_TableMap = new Dictionary<DBTableKey, DBTable>();
        List<DBTableKey> m_TablePath;
        public Maze(Dictionary<DBTableKey, DBTable> TableMap)
        {
            m_TableMap = TableMap;
        }

        private void Walk(DBTableKey Key, DBTable Table)
        {
            if (!Table.Visited)
            {
                Table.Visited = true;

                //Console.Write(string.Format("{0}:{1}", Key.Key1, Table.TableName));
                m_TablePath.Add(new DBTableKey(Key.Schema, Table.TableName, string.Empty));

                foreach (DBTableKey PK in Table.Keys)
                {
                    DBTableKey TK = new DBTableKey(PK.Schema, PK.Table, string.Empty);
                    if (m_TableMap.ContainsKey(TK))
                    {
                        DBTable DestTable = m_TableMap[TK];
                        //Console.Write(string.Format("->{0}:{1}:{2}", PK.Key1, DestTable.TableName, PK.Key3));
                        Walk(TK, DestTable);
                    }
                }
            }
        }

        public void DFS()
        {
            foreach (KeyValuePair<DBTableKey, DBTable> KVP in m_TableMap)
            {
                m_TablePath = new List<DBTableKey>();
                DBTableKey Key = KVP.Key;
                DBTable Table = KVP.Value;
                Walk(Key, Table);
                //Console.WriteLine();
            }
        }
    }
}
