using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlibanquanao
{
    public partial class Login : Form
    {
        SqlConnection conn;
        public Login()
        {
            InitializeComponent();
            createConnection();
        }

        private void createConnection()
        {
            try
            {
                String stringConnection = "Server=DESKTOP-TMCDUUR\\LUCKDAT;Database=ASM2; Integrated Security = true";
                conn = new SqlConnection(stringConnection);
                MessageBox.Show(" Connection Successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr createconnection " + ex.Message);
            }

        }

        private void btnexitLogin_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Exit ?", "Ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                return;
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ giao diện
                string username = txtusername.Text.Trim();
                string password = txtpassword.Text.Trim();
                string selectedAccessRights = cbbaccessRights.Text.Trim().ToLower(); // Lấy quyền từ ComboBox và chuyển sang chữ thường

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedAccessRights))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                conn.Open();

                // Tạo câu lệnh SQL an toàn
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT accessRights FROM staff WHERE username = @username AND password = @password";
                cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) // Kiểm tra nếu có dữ liệu trả về
                {
                    string dbAccessRights = reader["accessRights"].ToString().Trim().ToLower(); // Quyền trong cơ sở dữ liệu

                    // So sánh quyền truy cập
                    if (dbAccessRights == selectedAccessRights)
                    {
                        // Phân quyền truy cập
                        if (dbAccessRights == "admin")
                        {
                            MessageBox.Show("Login successful as Admin!");
                            admin adminForm = new admin();
                            adminForm.Show();
                            this.Hide();
                        }
                        else if (dbAccessRights == "warehouse")
                        {
                            MessageBox.Show("Login successful as Warehouse!");
                            warehouse warehouseForm = new warehouse();
                            warehouseForm.Show();
                            this.Hide();
                        }
                        else if (dbAccessRights == "staff")
                        {
                            MessageBox.Show("Login successful as Staff!");
                            staff staffForm = new staff();
                            staffForm.Show();
                            this.Hide();
                        }
                        else if (dbAccessRights == "employee")
                        {
                            MessageBox.Show("Login successful as Employee!");
                            Employee employeeForm = new Employee();
                            employeeForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid access rights!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Access rights do not match the selected role!");
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect username or password!");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }
    }
}
