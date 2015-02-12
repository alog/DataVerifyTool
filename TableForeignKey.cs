using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace muweili
{
    // one table foreign key object stand for one foreign key relation
    class TableForeignKey
    {
        public string tableName { get; set; }
        public string columnName { get; set; }
        public string referenceTableName { get; set; }
        public string referenceColName { get; set; }
        public string foreignKeyName { get; set; }


        // in tbl, the columnname is a forgign key,  referencing to another table ( referenceTableName) 's column
        public TableForeignKey(string tbl, string col, string refTbl, string refCol, string key)
        {
            tableName = tbl;
            columnName = col;
            referenceTableName = refTbl;
            referenceColName = refCol;
            foreignKeyName = key;
        }
        public string toString()
        {
            return (String.Format(@"{0} {1} --> {2} {3} foreign key: {4}",
                tableName,columnName,referenceTableName,referenceColName,foreignKeyName));
        }




    }
}
