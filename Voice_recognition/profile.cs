using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Voice_recognition
{
    public partial class profile : Form
    {
        public profile()
        {
            InitializeComponent();
        }
        String username = Login.Username;
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader data;

        private void profile_Load(object sender, EventArgs e)
        {
            String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
            con = new SqlConnection(conn);
            con.Open();
            String sql = "select * from alexa where name='" + username + "'";
            cmd = new SqlCommand(sql, con);
            data = cmd.ExecuteReader();
            if (data.Read())
            {
                String n = data.GetValue(0).ToString();
                String p = data.GetValue(1).ToString();
                String em = data.GetValue(3).ToString();
                String dob = data.GetValue(4).ToString();
                String pin = data.GetValue(6).ToString();
                String code = data.GetValue(5).ToString();
                label6.Text = n;
                label7.Text = p;
                label8.Text = em;
                label9.Text = dob;
                label10.Text = pin;
                label12.Text = code;
            }
            con.Close();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
