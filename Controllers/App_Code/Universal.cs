using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Data.Common;
using System.Web.UI;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ATS
{
    public abstract class Universal
    {
        public static string CONN_STRING = "";
        public static string ProviderName = "MySql.Data.MySqlClient";
        public static void InitConnection()
        {
            Page p = new System.Web.UI.Page();
            string ConString = "Server=166.62.41.158;Port=3306;Database=AirIndia_DB;Uid=db_Airdbuser;Pwd=c3softweb;charset=utf8;default command timeout=0";
            CONN_STRING = ConString;
        }

        public static bool ExecuteTransactionQuery(System.Collections .ArrayList arl)
        {
            DbProviderFactory fs = DbProviderFactories.GetFactory(ProviderName);
            DbConnection cn = fs.CreateConnection();
            cn.ConnectionString = CONN_STRING;
            if (cn.State == ConnectionState.Open)
                cn.Close();
            cn.Open();
            DbTransaction sqlTran = cn.BeginTransaction();
            DbCommand cmd = cn.CreateCommand();
            cmd.Transaction = sqlTran;
            try
            {
                foreach (string qry in arl)
                {
                    cmd.CommandText = qry;
                    cmd.ExecuteNonQuery();
                }
                sqlTran.Commit();
                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                return false;
            }
        }

        public static int ExecuteNonQuery(string cmdText)
        {
            try
            {
                // CONN_STRING = ConfigurationManager.ConnectionStrings["RDFD"].ConnectionString;
                DbProviderFactory fs = DbProviderFactories.GetFactory(ProviderName);
                DbConnection cn = fs.CreateConnection();
                cn.ConnectionString = CONN_STRING;
                DbCommand cmd = fs.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.Connection = cn;
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                cn.Close();
                return i;
            }
            catch (Exception ex) { return 0; }
        }
        public static string currentsession
        {
            get; set;
        }
        public static object ExecuteScalar(string cmdText)
        {
            // CONN_STRING = ConfigurationManager.ConnectionStrings["RDFD"].ConnectionString;

            DbProviderFactory fs = DbProviderFactories.GetFactory(ProviderName);
            DbConnection cn = fs.CreateConnection();
            cn.ConnectionString = CONN_STRING;
            DbCommand cmd = fs.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.Connection = cn;
            if (cn.State == ConnectionState.Open)
                cn.Close();
            cn.Open();
            object i = cmd.ExecuteScalar();
            cn.Close();
            return i;
        }
        public static ArrayList Select(string cmdText)
        {
            // CONN_STRING = ConfigurationManager.ConnectionStrings["RDFD"].ConnectionString;

            DbProviderFactory fs = DbProviderFactories.GetFactory(ProviderName);
            DbConnection cn = fs.CreateConnection();
            cn.ConnectionString = CONN_STRING;
            DbCommand cmd = fs.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.Connection = cn;
            if (cn.State == ConnectionState.Open)
                cn.Close();
            cn.Open();
            ArrayList arl = new ArrayList();
            DbDataReader dr = cmd.ExecuteReader();
            arl.Add("---Select----");
            while (dr.Read())
            {
                arl.Add(dr.GetValue(0).ToString());
            }
            dr.Close();
            cn.Close();
            return arl;
        }

        public static DataTable SelectWithDS(string cmdText, string TableName)
        {
           // CONN_STRING = ConfigurationManager.ConnectionStrings["RDFD"].ConnectionString;
            DbProviderFactory fs = DbProviderFactories.GetFactory(ProviderName);
            DbConnection cn = fs.CreateConnection();
            cn.ConnectionString = CONN_STRING;
            DbDataAdapter cmd = fs.CreateDataAdapter();
            DbCommand cm = fs.CreateCommand();
            cm.CommandText = cmdText;
            cm.Connection = cn;
            cmd.SelectCommand = cm;
            cmd.SelectCommand.Connection = cn;
            DataSet ds = new DataSet();
            cmd.Fill(ds, TableName);
            return ds.Tables[TableName];
        }

        public static DataTable SelectAll(string cmdText)
        {
           // CONN_STRING = ConfigurationManager.ConnectionStrings["RDFD"].ConnectionString;

            DataTable table = new DataTable();
            DbProviderFactory fs = DbProviderFactories.GetFactory(ProviderName);
            DbConnection cn = fs.CreateConnection();
            cn.ConnectionString = CONN_STRING;
            DbCommand cmd = fs.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.Connection = cn;
            if (cn.State == ConnectionState.Open)
                cn.Close();
            cn.Open();
            DbDataReader dr = cmd.ExecuteReader();
            for (int i = 0; i < dr.FieldCount; i++)
                table.Columns.Add(dr.GetName(i));
            while (dr.Read())
            {
                DataRow temp = table.NewRow();
                for (int i = 0; i < dr.FieldCount; i++)
                    temp[i] = dr[i];
                table.Rows.Add(temp);
            }
            dr.Close();
            cn.Close();
            return table;
        }
        public static string convertdate(string date)
        {
            string[] datee = date.Split('/');
            string newdate = datee[2] + "-" + datee[1] + "-" + datee[0];
            return newdate;
        }
        public static string convertToDDMMYYY(string date)
        {
            string[] datee = date.Split('-');
            string newdate = datee[2] + "/" + datee[1] + "/" + datee[0];
            return newdate;
        }
        public static int year { get { return DateTime.Now.AddHours(12).AddMinutes(30).Year; } }
        public static int month { get { return DateTime.Now.AddHours(12).AddMinutes(30).Month; } }
        public static int dayy { get { return DateTime.Now.AddHours(12).AddMinutes(30).Day; } }
        public static string GetDate
        {
            get
            {
                string Date = DateTime.Now.AddHours(12).AddMinutes(30).Year + "-" + DateTime.Now.AddHours(12).AddMinutes(30).Month + "-" + DateTime.Now.AddHours(12).AddMinutes(30).Day;
                return Date;
            }
        }
        public static string GetDateDDMMYY
        {
            get
            {
                string Date = DateTime.Now.AddHours(12).AddMinutes(30).Day + "-" + DateTime.Now.AddHours(12).AddMinutes(30).Month + "-" + DateTime.Now.AddHours(12).AddMinutes(30).Year;
                return Date;
            }
        }
        public static string GetTime
        {
            get
            {
                string Date = DateTime.Now.AddHours(12).AddMinutes(30).Hour + ":" + DateTime.Now.AddHours(12).AddMinutes(30).Minute;
                return Date;
            }
        }
        public static string UID { get { return "" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second; } }
    }
}