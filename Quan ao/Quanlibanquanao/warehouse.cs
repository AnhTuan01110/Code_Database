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
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
namespace Quanlibanquanao
{
    
    public partial class warehouse : Form
    {
        SqlConnection conn;
        public warehouse()
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

        private void DisplayDataProduct()
        {

            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "select * from product";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                DataTable data = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(data);
                dgvproduct.DataSource = data;
                MessageBox.Show("Successful");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr DisplayData Product " + ex.Message);
            }
            finally
            {

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }
        private void product_Click_1(object sender, EventArgs e)
        {
            DisplayDataProduct();
        }

        private void btnimage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";
            openFileDialog.Title = "Select a File";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                txtimageData.Text = openFileDialog.FileName;
            }
        }

        private void CreateProduct()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                String sql = " INSERT INTO product (productName, category, size, color, price, stockQuantity, description, imageData) VALUES (@productName, @category, @size, @color, @price, @stockQuantity, @description, @imageData)";

                cmd.Parameters.Add("@productName", SqlDbType.VarChar);
                cmd.Parameters["@productName"].Value = txtproductName.Text.ToString();

                cmd.Parameters.Add("@category", SqlDbType.VarChar);
                cmd.Parameters["@category"].Value = txtcategory.Text.ToString();

                cmd.Parameters.Add("@size", SqlDbType.VarChar);
                cmd.Parameters["@size"].Value = txtsize.Text.ToString();

                cmd.Parameters.Add("@color", SqlDbType.VarChar);
                cmd.Parameters["@color"].Value = cbbcolor.SelectedItem.ToString();

                cmd.Parameters.Add("@price", SqlDbType.Decimal);
                cmd.Parameters["@price"].Value = txtprice.Text.ToString();

                cmd.Parameters.Add("@stockQuantity", SqlDbType.Int);
                cmd.Parameters["@stockQuantity"].Value = Convert.ToInt32(txtstockQuantity.Text.ToString());

                cmd.Parameters.Add("@description", SqlDbType.Text);
                cmd.Parameters["@description"].Value = txtdescription.Text;

                byte[] imageData = File.ReadAllBytes(txtimageData.Text);
                cmd.Parameters.Add("@imageData", SqlDbType.VarBinary).Value = imageData;

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();

                MessageBox.Show(" Successful Create product");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Create Product " + ex.Message);
            }
        }

        private void btncreateProduct_Click(object sender, EventArgs e)
        {
            DisplayDataProduct();
            CreateProduct();
        }

        private void DeleteProduct()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                String sql = " DELETE FROM product WHERE productId = @productId";

                cmd.Parameters.Add("@productID", SqlDbType.Int);
                cmd.Parameters["@productId"].Value = Convert.ToInt32(txtproductId.Text.ToString());

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                MessageBox.Show(" Successful Delete");

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Delete Product " + ex.Message);
            }
        }

        private void btndeleteProduct_Click(object sender, EventArgs e)
        {

            String productId = btndeleteProduct.Text.ToString();
            DialogResult re = MessageBox.Show(" Delete " + productId, "Ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                DisplayDataProduct();
                DeleteProduct();
            }
            DisplayDataProduct();
            DeleteProduct();
        }

        private void EditProduct()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "UPDATE product  SET productName = @productName, category = @category, size = @size, color = @color, price = @price, stockQuantity = @stockQuantity, description = @description, imageData = @imageData  WHERE productId = @productId";


                cmd.Parameters.Add("@productId", SqlDbType.Int);
                cmd.Parameters["@productId"].Value = Convert.ToInt32(txtproductId.Text.ToString());

                cmd.Parameters.Add("@productName", SqlDbType.VarChar);
                cmd.Parameters["@productName"].Value = txtproductName.Text.ToString();

                cmd.Parameters.Add("@category", SqlDbType.VarChar);
                cmd.Parameters["@category"].Value = txtcategory.Text.ToString();

                cmd.Parameters.Add("@size", SqlDbType.VarChar);
                cmd.Parameters["@size"].Value = txtsize.Text.ToString();

                cmd.Parameters.Add("@color", SqlDbType.VarChar);
                cmd.Parameters["@color"].Value = cbbcolor.SelectedItem.ToString();

                cmd.Parameters.Add("@price", SqlDbType.Decimal);
                cmd.Parameters["@price"].Value = txtprice.Text.ToString();

                cmd.Parameters.Add("@stockQuantity", SqlDbType.Int);
                cmd.Parameters["@stockQuantity"].Value = Convert.ToInt32(txtstockQuantity.Text.ToString());

                cmd.Parameters.Add("@description", SqlDbType.Text);
                cmd.Parameters["@description"].Value = txtdescription.Text;

                byte[] imageData = File.ReadAllBytes(txtimageData.Text);
                cmd.Parameters.Add("@imageData", SqlDbType.VarBinary).Value = imageData;

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successful Edit ");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr Edit");
            }
        }
        private void btneditproduct_Click(object sender, EventArgs e)
        {
            DisplayDataProduct();
            EditProduct();
        }


        private void SearchProduct(string productId)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "SELECT productId, productName, category, size, color, price, stockQuantity, description, imageData FROM product WHERE productId = @productId";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@productId", SqlDbType.Int);
                cmd.Parameters["@productId"].Value = productId;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    string productIdResult = reader["productId"].ToString();
                    string productNameResult = reader["productName"].ToString();
                    string categoryResult = reader["category"].ToString();
                    string sizeResult = reader["size"].ToString();
                    string colorResult = reader["color"].ToString();
                    string priceResult = reader["price"].ToString();
                    string stockQuantityResult = reader["stockQuantity"].ToString();
                    string descriptionResult = reader["description"].ToString();

                    byte[] imageData = (byte[])reader["imageData"];
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        pictureBoxProduct.Image = Image.FromStream(ms);
                    }

                    MessageBox.Show("Product Id: " + productIdResult + "\n" +
                                    "Product Name: " + productNameResult + "\n" +
                                    "Category: " + categoryResult + "\n" +
                                    "Size: " + sizeResult + "\n" +
                                    "Color: " + colorResult + "\n" +
                                    "Price: " + priceResult + "\n" +
                                    "Stock Quantity: " + stockQuantityResult + "\n" +
                                    "Description: " + descriptionResult);
                }
                else
                {
                    MessageBox.Show("No product found with this product Id.");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy thông tin Product theo ProductId: " + ex.Message);
            }
        }

        private void btnsearchProduct_Click(object sender, EventArgs e)
        {
            string productId = txtproductId.Text;
            SearchProduct(productId);
            DisplayDataProduct();
        }


        private void btnexit_Click(object sender, EventArgs e)
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

        private DataTable GetFilteredProducts(string category, decimal? minPrice, decimal? maxPrice)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            StringBuilder query = new StringBuilder("SELECT * FROM product WHERE 1 = 1");

            if (!string.IsNullOrEmpty(category))
            {
                query.Append(" AND category LIKE @category");
            }
            if (minPrice.HasValue)
            {
                query.Append(" AND price >= @minPrice");
            }
            if (maxPrice.HasValue)
            {
                query.Append(" AND price <= @maxPrice");
            }

            using (SqlCommand command = new SqlCommand(query.ToString(), conn))
            {
                if (!string.IsNullOrEmpty(category))
                {
                    command.Parameters.Add(new SqlParameter("@category", SqlDbType.VarChar) { Value = "%" + category + "%" });
                }
                if (minPrice.HasValue)
                {
                    command.Parameters.Add(new SqlParameter("@minPrice", SqlDbType.Decimal) { Value = minPrice.Value });
                }
                if (maxPrice.HasValue)
                {
                    command.Parameters.Add(new SqlParameter("@maxPrice", SqlDbType.Decimal) { Value = maxPrice.Value });
                }

                DataTable dt = new DataTable();

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy vấn dữ liệu: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }

                return dt;
            }
        }


        private void btnfilter_Click(object sender, EventArgs e)
        {
            string categoryFilter = txtcategory1.Text.Trim();
            string minPriceText = txtminPrice.Text.Trim();
            string maxPriceText = txtmaxPrice.Text.Trim();


            decimal? minPrice = string.IsNullOrWhiteSpace(minPriceText) ? (decimal?)null : Convert.ToDecimal(minPriceText);
            decimal? maxPrice = string.IsNullOrWhiteSpace(maxPriceText) ? (decimal?)null : Convert.ToDecimal(maxPriceText);


            DataTable filteredProducts = GetFilteredProducts(categoryFilter, minPrice, maxPrice);

            if (filteredProducts.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp với tiêu chí lọc.");
                dgvproduct.DataSource = null;
            }
            else
            {
                dgvproduct.DataSource = filteredProducts;
            }

        }
        private void btnexitProduct_Click(object sender, EventArgs e)
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

        private void DisplayDataOrderDetail()
        {

            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "select * from orderDetails";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                DataTable data = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(data);
                dgvorderDetails.DataSource = data;
                MessageBox.Show("Successful");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr DisplayData Order Details" + ex.Message);
            }
            finally
            {

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }
        private void orderDetails_Click(object sender, EventArgs e)
        {
            DisplayDataOrderDetail();
        }
        private void SearchOrderDetail(string orderDetailsId)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "SELECT orderDetailsId, orderId, productId, quantity, price FROM orders WHERE orderDetailsId = @orderDetailsId";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@orderDetailsId", SqlDbType.Int);
                cmd.Parameters["@orderDetailsId"].Value = orderDetailsId;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    string orderDetailsIdResult = reader["orderDetailsId"].ToString();
                    string orderIdResult = reader["orderId"].ToString();
                    string productIdResult = reader["productId"].ToString();
                    string quantityResult = reader["quantity"].ToString();
                    string priceResult = reader["price"].ToString();

                    MessageBox.Show("Order Details Id: " + orderDetailsIdResult + "\n" +
                                    "Order Id: " + orderIdResult + "\n" +
                                    "Product Id: " + productIdResult + "\n" +
                                    "Quantity: " + quantityResult + "\n" +
                                    "Price: " + priceResult);
                }
                else
                {
                    MessageBox.Show("Successful Search");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr Search orderDetalsId: " + ex.Message);
            }
        }
        private void btnsearchOrderDetailId_Click(object sender, EventArgs e)
        {
            string orderDetailsId = txtorderDetailId.Text;
            SearchOrderDetail(orderDetailsId);
            DisplayDataOrderDetail();
        }

        private void btnexitOrderDetails_Click(object sender, EventArgs e)
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

        private void UpdatePassword()
        {
            if (txtnewPassword.Text != txtconfirmpw.Text)
            {
                MessageBox.Show("New Password and Confirm Password do not match.");
                return;
            }

            try
            {
                conn.Open();

                // Check if the username and password are correct
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                // Use parameters in SQL statement to avoid SQL Injection
                cmd.CommandText = "SELECT COUNT(*) FROM staff WHERE username = @username AND password = @password";
                cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = txtusername1.Text;
                cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = txtpassword1.Text;

                int userExists = (int)cmd.ExecuteScalar();

                if (userExists == 0)
                {
                    MessageBox.Show("Incorrect Username or Password.");
                    return;
                }

                // If the user exists, proceed to update the password
                SqlCommand updateCmd = conn.CreateCommand();
                updateCmd.CommandType = System.Data.CommandType.Text;

                // Use parameters in SQL statement to avoid SQL Injection
                updateCmd.CommandText = "UPDATE staff SET password = @newpw WHERE username = @username";
                updateCmd.Parameters.Add("@username", SqlDbType.VarChar).Value = txtusername1.Text;
                updateCmd.Parameters.Add("@newpw", SqlDbType.VarChar).Value = txtnewPassword.Text;

                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Password updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating password: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnupdatepw_Click(object sender, EventArgs e)
        {
            UpdatePassword();
        }

        private void btnexitPassword1_Click(object sender, EventArgs e)
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

    }
}
