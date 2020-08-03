/*-------------------------------------------------------------------------------------------------
 * Class Name       : DB
 * Module           :Admin
 * Description      :This Class gives Methods for Login of Admin User 
 * Methods          :1)GetConnection()
 *                   2)GetData(SqlCommand cmd) 
 *                   3)GetData(string sql)
 *                   4)ExecuteNonQuery(SqlCommand cmd)
 *                   5)ExecuteScaler(SqlCommand cmd)
 *                   6)GetDataSet(SqlCommand cmd)
 * Class Files      : DB.cs
 * Developed by     :Tushar
 * Date             :21/05/2017
 * Modified By      :
 * Modi. Description:
 * ------------------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

/// <summary>
/// Summary description for DB
/// </summary>
public class DB
{
    #region Declaration
    private static IsolationLevel m_isoLevel = IsolationLevel.ReadUncommitted;
    #endregion
    
    #region "DB Access Functions"
    /// <summary>
	/// define IsolationLevel
	/// </summary>
	/// <value></value>
	/// <returns>IsolationLevel</returns>
	/// <remarks></remarks>
	public static IsolationLevel IsolationLevel {
		get { return m_isoLevel; }
	}
	/// <summary>
	/// Gets Connection out of Web.config
	/// </summary>
	/// <returns>Returns SqlConnection</returns>
	/// <remarks></remarks>
	public static SqlConnection GetConnection()
	{
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connection"].ToString());
		conn.Open();        
		return conn;
       
	}
	/// <summary>
	/// Gets data out of the database
	/// </summary>
	/// <param name="cmd">The SQL Command</param>
	/// <returns>DataTable with the results</returns>

	public static DataTable GetData(SqlCommand cmd)       

	{        
		try {
			if (cmd.Connection != null) {
				using (DataSet ds = new DataSet()) {
					using (SqlDataAdapter da = new SqlDataAdapter()) {
						da.SelectCommand = cmd;
						da.Fill(ds);
						return ds.Tables[0];
					}
				}
			}
			else {
				using (SqlConnection conn = GetConnection()) 
                {
					using (SqlTransaction trans = conn.BeginTransaction(m_isoLevel)) {
						try {
							cmd.Transaction = trans;
							using (DataSet ds = new DataSet()) {
								using (SqlDataAdapter da = new SqlDataAdapter()) {
									da.SelectCommand = cmd;
									da.SelectCommand.Connection = conn;
									da.Fill(ds);
									return ds.Tables[0];
								}
							}
						}
						finally {
							trans.Commit();
                            conn.Close();
						}
					}
				}
			}
		}
		finally {
            
		}
	}
	/// <summary>
	/// Gets data out of database using a plain text string command
	/// </summary>
	/// <param name="sql">string command to be executed</param>
	/// <returns>DataTable with results</returns>
	public static DataTable GetData(string sql)
	{
		try {
			using (SqlConnection conn = GetConnection()) {
				using (SqlTransaction trans = conn.BeginTransaction(m_isoLevel)) {
					try {
						using (SqlCommand cmd = conn.CreateCommand()) {
							cmd.Transaction = trans;
							cmd.CommandType = CommandType.Text;
							cmd.CommandText = sql;
							using (DataSet ds = new DataSet()) {
								using (SqlDataAdapter da = new SqlDataAdapter()) {
									da.SelectCommand = cmd;
									da.SelectCommand.Connection = conn;
									da.Fill(ds);
									return ds.Tables[0];
								}
							}
						}
					}
					finally {
						trans.Commit();
                        conn.Close();
					}
				}
			}
		}
		finally {
			
		}
	}

   
	/// <summary>
	/// Executes a NonQuery
	/// </summary>
	/// <param name="cmd">NonQuery to execute</param>
	public static void ExecuteNonQuery(SqlCommand cmd)
	{
		try {
			using (SqlConnection conn = GetConnection()) {
				using (SqlTransaction trans = conn.BeginTransaction(m_isoLevel)) {
					cmd.Connection = conn;
					cmd.Transaction = trans;
					cmd.ExecuteNonQuery();
					trans.Commit();
				}
                conn.Close();
			}
		}
		finally {
			
		}
	}
    public static void ExecuteNonQuery(string Sql)
    {
        try
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlTransaction trans = conn.BeginTransaction(m_isoLevel))
                {
                    SqlCommand cmd;
                    cmd = new SqlCommand(Sql, conn);
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                conn.Close();
            }
        }
        finally
        {

        }
    }

	/// <summary>
	/// Executes a Scalar Query
	/// </summary>
	/// <param name="cmd">Scalar to execute</param>
	public static object ExecuteScalar(SqlCommand cmd)
	{
		try {
			using (SqlConnection conn = GetConnection()) {
				using (SqlTransaction trans = conn.BeginTransaction(m_isoLevel)) {
					cmd.Connection = conn;
					cmd.Transaction = trans;
					object res = cmd.ExecuteScalar();
					trans.Commit();
                    conn.Close();
					return res;
				}
                
			}
		}
		finally {
			
		}
	}
    /// <summary>
    /// Gets data out of the database
    /// </summary>
    /// <param name="cmd">The SQL Command</param>
    /// <returns>DataSet with the results</returns>

    public static object ExecuteScalar(string str)
    {
        try
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlTransaction trans = conn.BeginTransaction(m_isoLevel))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.Connection = conn;
                    cmd.Transaction = trans;
                    object res = cmd.ExecuteScalar();
                    trans.Commit();
                    conn.Close();
                    return res;
                }

            }
        }
        finally
        {

        }
    }

	public static DataSet GetDataSet(SqlCommand cmd)
	{
		try {
			if (cmd.Connection != null) {
				using (DataSet ds = new DataSet()) {
					using (SqlDataAdapter da = new SqlDataAdapter()) {
						da.SelectCommand = cmd;
						da.Fill(ds);
						return ds;
					}
				}
			}
			else {
				using (SqlConnection conn = GetConnection()) {
					using (SqlTransaction trans = conn.BeginTransaction(m_isoLevel)) {
						try {
							cmd.Transaction = trans;
							using (DataSet ds = new DataSet()) {
								using (SqlDataAdapter da = new SqlDataAdapter()) {
									da.SelectCommand = cmd;
									da.SelectCommand.Connection = conn;
									da.Fill(ds);
									return ds;
								}
							}
						}
						finally {
							trans.Commit();
                            conn.Close();
						}
					}
				}
                
			}
		}
		finally {
			
		}
	}

    public static DataSet DataSetFunc(string strSql)
    {
        DataSet functionReturnValue = default(DataSet);

        DataSet dsTemp = new DataSet();
        SqlDataAdapter DtAdpt = default(SqlDataAdapter);
        SqlConnection conn = GetConnection();
        DtAdpt = new SqlDataAdapter(strSql, conn);

        DtAdpt.Fill(dsTemp);


        DtAdpt.Dispose();
        DtAdpt = null;
        conn.Close();
        functionReturnValue = dsTemp;
        dsTemp.Dispose();

        return functionReturnValue;

    }



#endregion
    
}