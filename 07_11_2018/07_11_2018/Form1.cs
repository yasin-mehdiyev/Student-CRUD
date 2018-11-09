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
using System.Globalization;

namespace _07_11_2018
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            getFullData();
        }

        private int selectedId;

        //Select
        private void getFullData()
        {
            dgwStudents.Rows.Clear();
            string connectionString = "Server=DESKTOP-JR5VDO0; Database=Student;Integrated Security=SSPI";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "SELECT * FROM Person";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                dgwStudents.Rows.Add(reader.GetInt32(0),
                                     reader.GetString(1),
                                     reader.GetString(2),
                                     reader.GetString(3),
                                     reader.GetBoolean(5) ? "Evli" : "Subay",
                                     reader.GetDateTime(4).ToString("dd.MM.yyyy"),
                                     reader.GetDecimal(6).ToString("0.##")
                                     );
            }
            con.Close();
        }

        //Insert
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtSurname.Text) || string.IsNullOrEmpty(txtPhone.Text) || string.IsNullOrEmpty(txtCap.Text))
            {
                MessageBox.Show("Ad,Soyad,Telefon Nomrəsi və ya CAP-dan biri daxil edilməyib");
                return;
            }


            if (!decimal.TryParse(txtCap.Text, out decimal CAP))
            {
                MessageBox.Show("CAP datasini duzgun daxil edin");
                return;
            }

            string connectionString = "Server=DESKTOP-JR5VDO0; Database=Student;Integrated Security=SSPI";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "INSERT INTO Person([Name],[Surname],[Phone],[Birthday],[IsMarried],[CAP]) VALUES(@name, @surname, @phone, @birthday, @ismarried, @cap); ";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@name", txtName.Text);
            command.Parameters.AddWithValue("@surname", txtSurname.Text);
            command.Parameters.AddWithValue("@phone", txtPhone.Text);
            command.Parameters.AddWithValue("@birthday", dtpBirthday.Value.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@ismarried", rdbSingle.Checked ? 0 : 1);
            command.Parameters.AddWithValue("@cap", CAP);

            int r = command.ExecuteNonQuery();
            con.Close();
            Reset();
        }

        private void Reset()
        {
            getFullData();
            txtName.Text = "";
            txtSurname.Text = "";
            txtPhone.Text = "";
            txtCap.Text = "";
            rdbSingle.Checked = true;
            dtpBirthday.Value = DateTime.Now;

            btnDelete.Visible = false;
            btnUpdate.Visible = false;
            btnAdd.Visible = true;

        }

        private void dgwStudents_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnDelete.Visible = true;
            btnUpdate.Visible = true;
            btnAdd.Visible = false;

            this.selectedId = Convert.ToInt32(dgwStudents.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtName.Text = dgwStudents.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtSurname.Text = dgwStudents.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPhone.Text = dgwStudents.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtCap.Text = dgwStudents.Rows[e.RowIndex].Cells[6].Value.ToString();
            dtpBirthday.Value = DateTime.ParseExact(dgwStudents.Rows[e.RowIndex].Cells[5].Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

            if (dgwStudents.Rows[e.RowIndex].Cells[4].Value.ToString()=="Evli")
            {
                rdbSingle.Checked = false;
                rdbMarried.Checked = true;
            }
            else
            {
                rdbMarried.Checked = false;
                rdbSingle.Checked = true;
            }

        }

        //Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Silmək üçün əminsinizmi?", "Silmə işləmi", MessageBoxButtons.OKCancel);
            if (r == DialogResult.OK)
            {
                string connectionString = "Server=DESKTOP-JR5VDO0; Database=Student;Integrated Security=SSPI";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "DELETE Person WHERE ID=" + this.selectedId;
                SqlCommand command = new SqlCommand(query, con);
                command.ExecuteNonQuery();
                con.Close();

                Reset();
            }
        }

        //Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtSurname.Text) || string.IsNullOrEmpty(txtPhone.Text) || string.IsNullOrEmpty(txtCap.Text))
            {
                MessageBox.Show("Ad,Soyad,Telefon Nomrəsi və ya CAP-dan biri daxil edilməyib");
                return;
            }


            if (!decimal.TryParse(txtCap.Text, out decimal CAP))
            {
                MessageBox.Show("CAP datasini duzgun daxil edin");
                return;
            }

            string connectionString = "Server=DESKTOP-JR5VDO0; Database=Student;Integrated Security=SSPI";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();



            string query = "UPDATE Person SET [Name]=@name,[Surname]= @surname,[Phone]=@phone,[Birthday]=@birthday,[IsMarried]=@ismarried,[CAP]=@cap WHERE Id=@id; ";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@name", txtName.Text);
            command.Parameters.AddWithValue("@surname", txtSurname.Text);
            command.Parameters.AddWithValue("@phone", txtPhone.Text);
            command.Parameters.AddWithValue("@birthday", dtpBirthday.Value.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@ismarried", rdbSingle.Checked ? 0 : 1);
            command.Parameters.AddWithValue("@cap", CAP);
            command.Parameters.AddWithValue("@id", selectedId);

            int r = command.ExecuteNonQuery();
            con.Close();
            Reset();
        }

    }
}
