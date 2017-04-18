using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar;
using System.Drawing;
using System.Windows.Forms;

namespace PENENTUAN_JALUR_TERPENDEK.CLASS_MODULE
{
    class modKoneksiDatabase
    {
        //-- deklarasi variabel objek koneksi --
        public static SqlConnection conn = null;
        public static SqlDataReader dtReader = null;
        public static SqlDataAdapter dtAdapter = null;
        public static SqlCommand cmd = null;
        public static string sql = null;
        public static string PosisiRecord = null;
        public static eToastGlowColor glow = eToastGlowColor.Blue;
        public static eToastPosition pos = eToastPosition.MiddleCenter;


        public static bool BukaDatabase()
        {
            conn = new System.Data.SqlClient.SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DB_Sistem_Penentuan_Jalur_Terpendek_Kota_Merauke_A_Star_Greedy;Integrated Security=True");
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void CloseConn()
        {
            if ((conn != null))
            {
                conn.Close();
                conn = null;
            }
        }


        public static void BacaCombo(ComboBox cbRelasi, string cmd)
        {
            cbRelasi.Items.Clear();
            BukaDatabase();
            //Conn.Open()
            SqlCommand cm = new SqlCommand(cmd, conn);
            try
            {
                System.Data.SqlClient.SqlDataReader rdr =  cm.ExecuteReader();
                if (rdr.Read())
                {
                    cbRelasi.Items.Add(rdr[0].ToString());
                }
                rdr.Close();
            }
            catch (Exception salah)
            {
                MessageBox.Show("PERHATIAN..  " + salah.Message, "Kesalahan");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
