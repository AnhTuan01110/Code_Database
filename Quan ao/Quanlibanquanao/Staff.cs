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
using System.IO;
using System.Net.NetworkInformation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Cryptography;
using System.Text;

namespace Quanlibanquanao
{
    public partial class staff : Form
    {
        SqlConnection conn;
        public staff()
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
        // Toàn bộ code của phần Prorduct bao gồm ( Search).
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
            
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr DisplayData Product" + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void product_Click(object sender, EventArgs e)
        {
            DisplayDataProduct();
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


        // Toàn bộ code của phần customer bao gồm (Create , Edit, Delete, Search).


        private void DisplayDataCustomer()
        {

            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "select * from customer";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                DataTable data = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(data);
                dgvcustomer.DataSource = data;
           
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr DisplayData Customer" + ex.Message);
            }
            finally
            {

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }
        private void customer_Click_1(object sender, EventArgs e)
        {
            DisplayDataCustomer();
        }
      
        private void CreateCustomer()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                String sql = " INSERT INTO customer (fullName, email, phoneNumber, address) VALUES (@fullName, @email, @phoneNumber, @address)";

                cmd.Parameters.Add("@fullName", SqlDbType.VarChar);
                cmd.Parameters["@fullName"].Value = txtfullNamec.Text.ToString();

                cmd.Parameters.Add("@email", SqlDbType.VarChar);
                cmd.Parameters["@email"].Value = txtemailc.Text.ToString();

                cmd.Parameters.Add("@phoneNumber", SqlDbType.VarChar);
                cmd.Parameters["@phoneNumber"].Value = txtphoneNumberc.Text.ToString();

                cmd.Parameters.Add("@address", SqlDbType.Text);
                cmd.Parameters["@address"].Value = txtaddressc.Text;

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();

                MessageBox.Show(" Successful Create Customer");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Create Customer " + ex.Message);
            }
        }

        private void btncreateCustomer_Click_1(object sender, EventArgs e)
        {
            DisplayDataCustomer();
            CreateCustomer();
        }

        private void DeleteCustomer()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                String sql = " DELETE FROM customer WHERE customerId = @customerId";

                cmd.Parameters.Add("@customerId", SqlDbType.Int);
                cmd.Parameters["@CustomerId"].Value = Convert.ToInt32(txtcustomerId.Text.ToString());

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                MessageBox.Show(" Successful Delete");

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Delete Customer " + ex.Message);
            }
        }

        private void btndeleteCustomer_Click_1(object sender, EventArgs e)
        {
            String customerId = btndeleteCustomer.Text.ToString();
            DialogResult re = MessageBox.Show(" Delete " + customerId, "Ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                DisplayDataCustomer();
                DeleteCustomer();
            }
            DisplayDataCustomer();
            DeleteCustomer();
        }

        private void EditCustomer()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "UPDATE customer  SET fullName = @fullName, email = @email, phoneNumber = @phoneNumber, address = @address  WHERE customerId = @customerId";


                cmd.Parameters.Add("@customerId", SqlDbType.Int);
                cmd.Parameters["@customerId"].Value = Convert.ToInt32(txtcustomerId.Text.ToString());

                cmd.Parameters.Add("@fullName", SqlDbType.VarChar);
                cmd.Parameters["@fullName"].Value = txtfullNamec.Text.ToString();

                cmd.Parameters.Add("@email", SqlDbType.VarChar);
                cmd.Parameters["@email"].Value = txtemailc.Text.ToString();

                cmd.Parameters.Add("@phoneNumber", SqlDbType.VarChar);
                cmd.Parameters["@phoneNumber"].Value = txtphoneNumberc.Text.ToString();

                cmd.Parameters.Add("@address", SqlDbType.Text);
                cmd.Parameters["@address"].Value = txtaddressc.Text;


                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show(" Successfu Edit ");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Edit");
            }
        }
        private void btneditCustomer_Click_1(object sender, EventArgs e)
        {
            DisplayDataCustomer();
            EditCustomer();
        }


        private void SearchCustomer(string customerId)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "SELECT customerId, fullName, email, phoneNumber, address FROM customer WHERE customerId = @customerId";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@customerId", SqlDbType.Int);
                cmd.Parameters["@customerId"].Value = customerId;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    string customerIdResult = reader["customerId"].ToString();
                    string fullNameResult = reader["fullName"].ToString();
                    string emailResult = reader["email"].ToString();
                    string phoneNumberResult = reader["phoneNumber"].ToString();
                    string addressResult = reader["address"].ToString();

                    MessageBox.Show("Customer Id: " + customerIdResult + "\n" +
                                    "Full Name: " + fullNameResult + "\n" +
                                    "Email: " + emailResult + "\n" +
                                    "Phone Number: " + phoneNumberResult + "\n" +
                                    "Address: " + addressResult);
                }
                else
                {
                    MessageBox.Show("Successful Search");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr Search: " + ex.Message);
            }
        }
        private void btnsearchCustomer_Click_1(object sender, EventArgs e)
        {
            string customerId = txtcustomerId.Text;
            SearchCustomer(customerId);
            DisplayDataCustomer();
        }

        private void btnexitCustomer_Click(object sender, EventArgs e)
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

        private void btnexitOrder_Click(object sender, EventArgs e)
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


        private void DisplayDataOrder()
        {

            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "select * from orders";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                DataTable data = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(data);
                dgvorder.DataSource = data;
               
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr DisplayData Order" + ex.Message);
            }
            finally
            {

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }
        private void order_Click_1(object sender, EventArgs e)
        {
            DisplayDataOrder();
        }

        private void CreateOrder()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "INSERT INTO orders (orderDate,customerId, staffId, totalAmount) VALUES (@orderDate, @customerId, @staffId, @totalAmount)";

                if (DateTime.TryParse(txtorderDate.Text, out DateTime orderDate))
                {
                    cmd.Parameters.Add("@orderDate", SqlDbType.Date);
                    cmd.Parameters["@orderDate"].Value = orderDate;
                }
                else
                {
                    throw new Exception(" Erorr Order Date");
                }

                cmd.Parameters.Add("@totalAmount", SqlDbType.Decimal);
                cmd.Parameters["@totalAmount"].Value = txttotalAmount.Text.ToString();

                cmd.Parameters.Add("@staffId", SqlDbType.Int);
                cmd.Parameters["@staffId"].Value = Convert.ToInt32(txtstaffIdo.Text.ToString());

                cmd.Parameters.Add("@customerId", SqlDbType.Int);
                cmd.Parameters["@customerId"].Value = Convert.ToInt32(txtcustomerIdo.Text.ToString());

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();

                MessageBox.Show("Successful Create");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr Create: " + ex.Message);
            }
            finally
            {

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btncreateOrder_Click(object sender, EventArgs e)
        {
            DisplayDataOrder();
            CreateOrder();
        }

        private void DeleteOrder()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                String sql = " DELETE FROM orders WHERE orderId = @orderId";

                cmd.Parameters.Add("@orderId", SqlDbType.Int);
                cmd.Parameters["@orderId"].Value = Convert.ToInt32(txtorderId.Text.ToString());

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                MessageBox.Show(" Successful Delete");

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Delete Order " + ex.Message);
            }
        }

        private void btndeleteOrder_Click(object sender, EventArgs e)
        {
            String orderId = btndeleteOrder.Text.ToString();
            DialogResult re = MessageBox.Show(" Delete" + orderId, "Ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                DisplayDataOrder();
                DeleteOrder();
            }
            DisplayDataOrder();
            DeleteOrder();
        }

        private void EditOrder()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "UPDATE orders  SET orderdate = @orderdate, customerId = @customerId, staffId = @staffId, totalAmount = @totalAmount WHERE orderId = @orderId";


                cmd.Parameters.Add("@orderId", SqlDbType.Int);
                cmd.Parameters["@orderId"].Value = Convert.ToInt32(txtorderId.Text.ToString());

                if (DateTime.TryParse(txtorderDate.Text, out DateTime orderDate))
                {
                    cmd.Parameters.Add("@orderDate", SqlDbType.Date);
                    cmd.Parameters["@orderDate"].Value = orderDate;
                }
                else
                {
                    throw new Exception(" Erorr Order Date");
                }

                cmd.Parameters.Add("@totalAmount", SqlDbType.Decimal);
                cmd.Parameters["@totalAmount"].Value = txttotalAmount.Text.ToString();

                cmd.Parameters.Add("@staffId", SqlDbType.Int);
                cmd.Parameters["@staffId"].Value = Convert.ToInt32(txtstaffIdo.Text.ToString());

                cmd.Parameters.Add("@customerId", SqlDbType.Int);
                cmd.Parameters["@customerId"].Value = Convert.ToInt32(txtcustomerIdo.Text.ToString());


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
        private void btneditOrder_Click(object sender, EventArgs e)
        {
            DisplayDataOrder();
            EditOrder();
        }

        private void SearchOrder(string orderId)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "SELECT orderId, orderDate, customerId, staffId, totalAmount FROM orders WHERE orderId = @orderId";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@orderId", SqlDbType.Int);
                cmd.Parameters["@orderId"].Value = orderId;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string orderIdResult = reader["orderId"].ToString();
                    string orderDateResult = reader["orderDate"].ToString();
                    string customerIdResult = reader["customerId"].ToString();
                    string staffIdResult = reader["staffId"].ToString();
                    string totalAmountResult = reader["totalAmount"].ToString();

                    MessageBox.Show("Order Id: " + orderIdResult + "\n" +
                                    "Order Date: " + orderDateResult + "\n" +
                                    "Customer Id: " + customerIdResult + "\n" +
                                    "Staff Id: " + staffIdResult + "\n" +
                                    "Total Amount: " + totalAmountResult);
                }
                else
                {
                    MessageBox.Show("Successful Search.");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr Search: " + ex.Message);
            }
        }
        private void btnsearchOrder_Click(object sender, EventArgs e)
        {
            string orderId = txtorderId.Text;
            SearchOrder(orderId);
            DisplayDataOrder();
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
                string sql = "SELECT orderDetailsId, orderId, productId, quantity, price FROM orderDetails WHERE orderDetailsId = @orderDetailsId";
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
