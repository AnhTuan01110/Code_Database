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

namespace Quanlibanquanao
{
    public partial class admin : Form
    {
        SqlConnection conn;
        public admin()
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

        // Toàn bộ code của phần staff bao gồm (Create , Edit, Delete, Search).
        private void DisplayDataStaff()
        {

            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "select * from staff";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                DataTable data = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(data);
                dgvstaff.DataSource = data;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr DisplayData Staff" + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }
        private void staff_Click(object sender, EventArgs e)
        {
            DisplayDataStaff();
        }
        private void CreateStaff()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                String sql = " INSERT INTO staff (fullName, email, phoneNumber, position, salary, username, password, accessRights) VALUES (@fullName, @email, @phoneNumber, @position, @salary, @username, @password, @accessRights)";

                cmd.Parameters.Add("@fullname", SqlDbType.VarChar);
                cmd.Parameters["@fullname"].Value = txtfullName.Text.ToString();

                cmd.Parameters.Add("@email", SqlDbType.VarChar);
                cmd.Parameters["@email"].Value = txtemail.Text.ToString();

                cmd.Parameters.Add("@phoneNumber", SqlDbType.VarChar);
                cmd.Parameters["@phoneNumber"].Value = txtphoneNumber.Text.ToString();

                cmd.Parameters.Add("@position", SqlDbType.VarChar);
                cmd.Parameters["@position"].Value = cbbposition.SelectedItem.ToString();

                cmd.Parameters.Add("@salary", SqlDbType.Decimal);
                cmd.Parameters["@salary"].Value = txtsalary.Text.ToString();

                cmd.Parameters.Add("@username", SqlDbType.VarChar);
                cmd.Parameters["@username"].Value = txtusername.Text.ToString();

                cmd.Parameters.Add("@password", SqlDbType.VarChar);
                cmd.Parameters["@password"].Value = txtpassword.Text.ToString();

                cmd.Parameters.Add("@accessRights", SqlDbType.VarChar);
                cmd.Parameters["@accessRights"].Value = cbbaccessRights.SelectedItem.ToString();

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();

                MessageBox.Show(" Create Successful Staff");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Create Staff " + ex.Message);
            }
        }

        private void btncreateStaff_Click(object sender, EventArgs e)
        {
            DisplayDataStaff();
            CreateStaff();
        }

        private void DeleteStaff()
        {
            try
            {
                conn.Open();
       
                SqlCommand cmd = conn.CreateCommand();
        
                cmd.CommandType = System.Data.CommandType.Text;
  
                String sql = " DELETE FROM staff WHERE staffId = @staffId";

                cmd.Parameters.Add("@staffId", SqlDbType.Int);
                cmd.Parameters["@staffId"].Value = Convert.ToInt32(txtstaffId.Text.ToString());

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                MessageBox.Show(" xoa thanh cong");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Delete Staff " + ex.Message);
            }
        }

        private void btndeleteStaff_Click(object sender, EventArgs e)
        {

            String staffId = btndeleteStaff.Text.ToString();
            DialogResult re = MessageBox.Show(" Delete " + staffId, "ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                DisplayDataStaff();
                DeleteStaff();
            }
            DisplayDataStaff();
            DeleteStaff();
        }

        private void EditStaff()
        {
            try
            {
                conn.Open();
        
                SqlCommand cmd = conn.CreateCommand();
         
                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "UPDATE staff  SET fullName = @fullName, email = @email, phoneNumber = @phoneNumber, position = @position, salary = @salary, username = @username, password = @password, accessRights = @accessRights WHERE staffId = @staffId";

                cmd.Parameters.Add("@staffId", SqlDbType.Int);
                cmd.Parameters["@staffId"].Value = Convert.ToInt32(txtstaffId.Text.ToString());

                cmd.Parameters.Add("@fullname", SqlDbType.VarChar);
                cmd.Parameters["@fullname"].Value = txtfullName.Text.ToString();

                cmd.Parameters.Add("@email", SqlDbType.VarChar);
                cmd.Parameters["@email"].Value = txtemail.Text.ToString();

                cmd.Parameters.Add("@phoneNumber", SqlDbType.VarChar);
                cmd.Parameters["@phoneNumber"].Value = txtphoneNumber.Text.ToString();

                cmd.Parameters.Add("@position", SqlDbType.VarChar);
                cmd.Parameters["@position"].Value = cbbposition.SelectedItem.ToString();

                cmd.Parameters.Add("@salary", SqlDbType.Decimal);
                cmd.Parameters["@salary"].Value = txtsalary.Text.ToString();

                cmd.Parameters.Add("@username", SqlDbType.VarChar);
                cmd.Parameters["@username"].Value = txtusername.Text.ToString();

                cmd.Parameters.Add("@password", SqlDbType.VarChar);
                cmd.Parameters["@password"].Value = txtpassword.Text.ToString();

                cmd.Parameters.Add("@accessRights", SqlDbType.VarChar);
                cmd.Parameters["@accessRights"].Value = cbbaccessRights.SelectedItem.ToString();

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show(" Successful Edit ");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Edit");
            }
        }

        private void btneditStaff_Click(object sender, EventArgs e)
        {
            DisplayDataStaff();
            EditStaff();
        }

        private void SearchStaff(string staffId)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                String sql = "SELECT staffId, fullName, email, phoneNumber, position, salary, username, password, accessRights FROM staff WHERE staffId = @staffId";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@staffId", SqlDbType.Int);
                cmd.Parameters["@staffId"].Value = staffId;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
   
                    string staffIdResult = reader["staffId"].ToString();

                    string fullNameResult = reader["fullName"].ToString();
                    string emailResult = reader["email"].ToString();
                    string phoneNumberResult = reader["phoneNumber"].ToString();
                    string positionResult = reader["position"].ToString();
                    string salaryResult = reader["salary"].ToString();
                    string usernameResult = reader["username"].ToString();
                    string passwordResult = reader["password"].ToString();
                    string accessRightsResult = reader["accessRights"].ToString();

                    MessageBox.Show("Staff ID: " + staffIdResult + "\n" +
                         "Full Name: " + fullNameResult + "\n" +
                         "Email: " + emailResult + "\n" +
                         "Phone Number: " + phoneNumberResult + "\n" +
                         "Position: " + positionResult + "\n" +
                         "Salary: " + salaryResult + "\n" +
                         "Username: " + usernameResult + "\n" +
                         "Password: " + passwordResult + "\n" +
                         "Access Rights: " + accessRightsResult);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Search Staff: " + ex.Message);
            }
        }

        private void btnsearchStaff_Click(object sender, EventArgs e)
        {
            string staffId = txtstaffId.Text;
            SearchStaff(staffId);
            DisplayDataStaff();
        }

        // Toàn bộ code của phần Product bao gồm (Create , Edit, Delete, Search).
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
        private void product_Click(object sender, EventArgs e)
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

        private void customer_Click(object sender, EventArgs e)
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

        private void btncreateCustomer_Click(object sender, EventArgs e)
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

        private void btndeleteCustomer_Click(object sender, EventArgs e)
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
        private void btneditCustomer_Click(object sender, EventArgs e)
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
        private void btnsearchCustomer_Click(object sender, EventArgs e)
        {
            String customerId = txtcustomerId.Text;
            SearchCustomer(customerId);
            DisplayDataCustomer();
        }


        // Toàn bộ code của phần Order bao gồm (Create , Edit, Delete, Search).

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
        private void order_Click(object sender, EventArgs e)
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
                    string orderdateResult = reader["orderDate"].ToString();
                    string customerIdResult = reader["customerId"].ToString();
                    string staffIdResult = reader["staffId"].ToString();
                    string totalAmountResult = reader["totalAmount"].ToString();

                    MessageBox.Show("Order Id: " + orderIdResult + "\n" +
                                    "Order Date: " + orderdateResult + "\n" +
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
            String orderId = txtorderId.Text;
            SearchOrder(orderId);
            DisplayDataOrder();
        }

        // Toàn bộ code của phần Order Detail bao gồm (Create , Edit, Delete, Search). 
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

        private void CreateOrderDetail()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string sql = "INSERT INTO orderDetails (quantity, productId, orderId, price) VALUES (@quantity, @productId, @orderId, @price)";


                cmd.Parameters.Add("@quantity", SqlDbType.Int);
                cmd.Parameters["@quantity"].Value = Convert.ToInt32(txtquantityod.Text.ToString());

                cmd.Parameters.Add("@price", SqlDbType.Decimal);
                cmd.Parameters["@price"].Value = txtpriceod.Text.ToString();

                cmd.Parameters.Add("@orderId", SqlDbType.Int);
                cmd.Parameters["@orderid"].Value = Convert.ToInt32(txtorderIdod.Text.ToString());

                cmd.Parameters.Add("@productId", SqlDbType.Int);
                cmd.Parameters["@productId"].Value = Convert.ToInt32(txtproductIdod.Text.ToString());

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();

                MessageBox.Show("Successful OrderDetail Create ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr OrderDetail: " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void btncreateOrderDetailId_Click(object sender, EventArgs e)
        {
            DisplayDataOrderDetail();
            CreateOrderDetail();
        }

        private void DeleteOrderDetail()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.Text;
           
                String sql = " DELETE FROM orderDetails WHERE orderDetailsId = @orderDetailsId";
           
                cmd.Parameters.Add("@orderDetailsId", SqlDbType.Int);
                cmd.Parameters["@orderDetailsId"].Value = Convert.ToInt32(txtorderDetailId.Text.ToString());

                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                MessageBox.Show(" Successful Delete");
           
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erorr Delete OrderDetailsId " + ex.Message);
            }
        }
        private void btndeleteOrderDetailId_Click_1(object sender, EventArgs e)
        {
            String orderDetailsId = btndeleteOrderDetailId.Text.ToString();
            DialogResult re = MessageBox.Show(" Delete " + orderDetailsId, "Ok", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                DisplayDataOrderDetail();
                DeleteOrderDetail();
            }
            DisplayDataOrderDetail();
            DeleteOrderDetail();
        }

        private void EditOrderDetail()
        {
            try
            {
                conn.Open();
      
                SqlCommand cmd = conn.CreateCommand();
        
                cmd.CommandType = System.Data.CommandType.Text;
                string sql = "UPDATE orderDetails  SET quantity = @quantity, orderId = @orderId, productId = @productId, price = @price WHERE orderDetailsId = @orderDetailsId";
       

                cmd.Parameters.Add("@orderDetailsId", SqlDbType.Int);
                cmd.Parameters["@orderDetailsId"].Value = Convert.ToInt32(txtorderDetailId.Text.ToString());

                cmd.Parameters.Add("@quantity", SqlDbType.Int);
                cmd.Parameters["@quantity"].Value = Convert.ToInt32(txtquantityod.Text.ToString());

                cmd.Parameters.Add("@orderId", SqlDbType.Int);
                cmd.Parameters["@orderid"].Value = Convert.ToInt32(txtorderIdod.Text.ToString());

                cmd.Parameters.Add("@productId", SqlDbType.Int);
                cmd.Parameters["@productId"].Value = Convert.ToInt32(txtproductIdod.Text.ToString());

                cmd.Parameters.Add("@price", SqlDbType.Decimal);
                cmd.Parameters["@price"].Value = txtpriceod.Text.ToString();

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
        private void btneditOrderDetailId_Click(object sender, EventArgs e)
        {
            DisplayDataOrderDetail();
            EditOrderDetail();
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

        private void btnexitStaff_Click(object sender, EventArgs e)
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

    }
}
