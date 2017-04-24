using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SistemPendukungKeputusan.Helper
{
    public static class GlobalFunction
    {
        public static DataSet ExecuteQuery(IDbConnection objCon, string strQuery, IDbTransaction ObjTransaction = null)
        {
            DataSet functionReturnValue = default(DataSet);
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = strQuery,
                Connection = (SqlConnection) objCon,
                CommandTimeout = 0
            };
            if ((ObjTransaction != null))
                cmd.Transaction = (SqlTransaction) ObjTransaction;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            functionReturnValue = new DataSet();
            da.Fill(functionReturnValue);
            return functionReturnValue;
        }
    }

 
}
