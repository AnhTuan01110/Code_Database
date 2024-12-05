CREATE DATABASE ASM2;
USE ASM2;

CREATE TABLE staff (
    staffId INT PRIMARY KEY IDENTITY(1,1), 
    fullName VARCHAR(50) NOT NULL,        
    email VARCHAR(100) UNIQUE,             
    phoneNumber VARCHAR(15),               
    position VARCHAR(50) NOT NULL,        
    salary DECIMAL(10, 2),                
    username VARCHAR(50) UNIQUE NOT NULL,   
    password VARCHAR(255) NOT NULL,
    accessRights VARCHAR(20) NOT NULL      
);

CREATE TABLE product (
    productId INT PRIMARY KEY IDENTITY(1,1), 
    productName VARCHAR(100) NOT NULL,      
    category VARCHAR(50),                     
    size VARCHAR(10),                         
    color VARCHAR(30),                       
    price DECIMAL(10, 2) NOT NULL,            
    stockQuantity INT NOT NULL,              
    description TEXT,                        
    imageData VARBINARY(MAX)                 
);


CREATE TABLE customer (
    customerId INT PRIMARY KEY IDENTITY(1,1), 
    fullName VARCHAR(50) NOT NULL,        
    email VARCHAR(100) UNIQUE,                
    phoneNumber VARCHAR(15),                 
    address TEXT,                           
    createdAt DATETIME DEFAULT GETDATE()    
);


CREATE TABLE orders (
    orderId INT PRIMARY KEY IDENTITY(1,1), 
    orderDate DATETIME NOT NULL,           
    customerId INT,                        
    staffId INT,                           
    totalAmount DECIMAL(10, 2),           
    FOREIGN KEY (customerId) REFERENCES customer(customerId),
    FOREIGN KEY (staffId) REFERENCES staff(staffId)
);


CREATE TABLE orderDetails (
    orderDetailsId INT PRIMARY KEY IDENTITY(1,1), 
    orderId INT ,                                   
    productId INT,                                 
    quantity INT NOT NULL,                         
    price DECIMAL(10, 2) NOT NULL,                
    FOREIGN KEY (orderId) REFERENCES orders(orderId),
    FOREIGN KEY (productId) REFERENCES product(productId)
);

-- Thêm 10 dữ liệu vào bảng 'staff'
INSERT INTO staff (fullName, email, phoneNumber, position, salary, username, password, accessRights)
VALUES
('Nguyen Thi Lan', 'nguyenthlan@gmail.com', '0912345670', 'Sales Staff', 16000000, 'tuan1', '123', 'Admin'),
('Tran Minh Tuan', 'tranminhtuan@gmail.com', '0912345671', 'System Administrator', 21000000, 'tuan2', '123', 'Warehouse'),
('Le Thi Lan', 'lethilan@gmail.com', '0912345672', 'Warehouse Staff', 14000000, 'tuan3', '123', 'Staff'),
('Nguyen Thanh Mai', 'nguyenthanhmai@gmail.com', '0912345673', 'Sales Staff', 15500000, 'tuan4', '123', 'Employee'),
('Tran Bao Long', 'tranbaolong@gmail.com', '0912345674', 'System Administrator', 20000000, 'admin3', '123', 'admin'),
('Le Bao Thao', 'lebaothao@gmail.com', '0912345675', 'Warehouse Staff', 13000000, 'warehouse3', '123', 'warehouse'),
('Nguyen Hoang Vinh', 'nguyenhoangvinh@gmail.com', '0912345676', 'Sales Staff', 17000000, 'staff11', '123', 'staff'),
('Tran Thi Hoa', 'tranthihoa@gmail.com', '0912345677', 'System Administrator', 22000000, 'admin4', '123', 'admin'),
('Le Minh Son', 'leminhson@gmail.com', '0912345678', 'Warehouse Staff', 14500000, 'warehouse4', '123', 'warehouse'),
('Nguyen Thi Thanh', 'nguyenthithanh@gmail.com', '0912345679', 'Sales Staff', 16500000, 'staff111', '123', 'staff');
-- Thêm 10 dữ liệu vào bảng 'product'
select * from staff
INSERT INTO product (productName, category, size, color, price, stockQuantity, description, imageData)
VALUES
('Ao so mi', 'Thoi trang', 'M', 'Emerald Green', 100000, 30, 'Ao so mi nu', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\aosominu.jpg', SINGLE_BLOB) AS imageData)),
('Chan vay', 'Thoi trang', 'L', 'Rose Gold', 250000, 25, 'Chan vay nu', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\chanvay.jpg', SINGLE_BLOB) AS imageData)),
('Quna coc', 'Thoi trang', 'M', 'Mustard Yellow', 30000, 10, 'Quan cam nam', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\quancam.jpg', SINGLE_BLOB) AS imageData)),
('Quan soc', 'Thoi trang', 'L', 'Lavender', 35000, 20, 'Quan soc nam', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\quansoc.jpg', SINGLE_BLOB) AS imageData)),
('Vest', 'Thoi trang', 'M', 'Emerald Green', 150000, 40, 'Vest nam', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\vest.jpg', SINGLE_BLOB) AS imageData)),
('Ao len', 'Thoi trang', 'L', 'Rose Gold', 600000, 15, 'Ao len nam', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\aolen.jpg', SINGLE_BLOB) AS imageData)),
('Ao len', 'Thoi trang', 'S', 'Mint Green', 80000, 30, 'Ao len nu', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\aolennam.jpg', SINGLE_BLOB) AS imageData)),
('Vay', 'Thoi trang', 'M', 'Rose Gold', 350000, 15, 'Vay body', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\vaybody.jpg', SINGLE_BLOB) AS imageData)),
('Vay', 'Thoi trang', 'L', 'Burgundy', 85000, 50, 'Quan jean nu', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\vayxoe.jpeg', SINGLE_BLOB) AS imageData)),
('Quan', 'Thoi trang', 'M', 'Teal', 55000, 10, 'Quan dq2', 
    (SELECT * FROM OPENROWSET(BULK N'C:\Users\ADMIN\OneDrive\Hình ảnh\Saved Pictures\quandq2.jpg', SINGLE_BLOB) AS imageData));

	('Ao so mi', 'Thoi trang', 'M', 'Emerald Green', 100000, 30, 'Ao so mi nu', 
    (SELECT * FROM OPENROWSET(BULK N'\aosominu.jpg', SINGLE_BLOB) AS imageData)),
('Chan vay', 'Thoi trang', 'L', 'Rose Gold', 250000, 25, 'Chan vay nu', 
    (SELECT * FROM OPENROWSET(BULK N'\chanvay.jpg', SINGLE_BLOB) AS imageData)),
('Quna coc', 'Thoi trang', 'M', 'Mustard Yellow', 30000, 10, 'Quan cam nam', 
    (SELECT * FROM OPENROWSET(BULK N'\quancam.jpg', SINGLE_BLOB) AS imageData)),
('Quan soc', 'Thoi trang', 'L', 'Lavender', 35000, 20, 'Quan soc nam', 
    (SELECT * FROM OPENROWSET(BULK N'\quansoc.jpg', SINGLE_BLOB) AS imageData)),
('Vest', 'Thoi trang', 'M', 'Emerald Green', 150000, 40, 'Vest nam', 
    (SELECT * FROM OPENROWSET(BULK N'\vest.jpg', SINGLE_BLOB) AS imageData)),
('Ao len', 'Thoi trang', 'L', 'Rose Gold', 600000, 15, 'Ao len nam', 
    (SELECT * FROM OPENROWSET(BULK N'\aolen.jpg', SINGLE_BLOB) AS imageData)),
('Ao len', 'Thoi trang', 'S', 'Mint Green', 80000, 30, 'Ao len nu', 
    (SELECT * FROM OPENROWSET(BULK N'\aolennam.jpg', SINGLE_BLOB) AS imageData)),
('Vay', 'Thoi trang', 'M', 'Rose Gold', 350000, 15, 'Vay body', 
    (SELECT * FROM OPENROWSET(BULK N'\vaybody.jpg', SINGLE_BLOB) AS imageData)),
('Vay', 'Thoi trang', 'L', 'Burgundy', 85000, 50, 'Quan jean nu', 
    (SELECT * FROM OPENROWSET(BULK N'\vayxoe.jpeg', SINGLE_BLOB) AS imageData)),
('Quan', 'Thoi trang', 'M', 'Teal', 55000, 10, 'Quan dq2', 
    (SELECT * FROM OPENROWSET(BULK N'\quandq2.jpg', SINGLE_BLOB) AS imageData));
-- Thêm 10 dữ liệu vào bảng 'customer'
INSERT INTO customer (fullName, email, phoneNumber, address)
VALUES
('Pham Thanh Son', 'phanthanhson@gmail.com', '0987654324', '12 Nguyen Tri Phuong, Hue'),
('Nguyen Thi Minh', 'nguyenthiminh@gmail.com', '0987654325', '22 Le Duan, Hanoi'),
('Le Quang Minh', 'lequangminh@gmail.com', '0987654326', '45 Pham Hong Thai, HCM'),
('Nguyen Thi Lan', 'nguyenthilan2@gmail.com', '0987654327', '18 Trieu Quang Phuc, Hanoi'),
('Tran Thi Lan', 'tranthilan2@gmail.com', '0987654328', '25 Nguyen Thi Minh Khai, HCM'),
('Le Minh Tuan', 'leminhtuan2@gmail.com', '0987654329', '30 Duong Quang Trung, Hanoi'),
('Pham Bao Long', 'phambaolong2@gmail.com', '0987654330', '35 Nguyen Thi Minh Khai, Hanoi'),
('Tran Minh Son', 'tranminhson2@gmail.com', '0987654331', '10 Nguyen Du, HCM'),
('Nguyen Thi Hoa', 'nguyenthihhoa2@gmail.com', '0987654332', '60 Le Thanh Ton, HCM'),
('Le Thi Thanh', 'lethithanh2@gmail.com', '0987654333', '22 Nguyen Binh Khiem, Hanoi');

-- Thêm 10 dữ liệu vào bảng 'orders'
INSERT INTO orders (orderDate, customerId, staffId, totalAmount)
VALUES
('2024-11-05 ', 1, 1, 1000000),
('2024-11-06 ', 2, 2, 750000),
('2024-11-07 ', 3, 3, 1200000),
('2024-11-08 ', 4, 4, 1100000),
('2024-11-09 ', 5, 5, 800000),
('2024-11-10 ', 6, 6, 1300000),
('2024-11-11 ', 7, 7, 900000),
('2024-11-12 ', 8, 8, 1500000),
('2024-11-13 ', 9, 9, 950000),
('2024-11-14 ', 10, 10, 1100000);

-- Thêm 10 dữ liệu vào bảng 'order_details'
INSERT INTO orderDetails (orderId, productId, quantity, price)
VALUES
(1, 1, 3, 500000),
(2, 2, 1, 250000),
(3, 3, 2, 300000),
(4, 4, 1, 350000),
(5, 5, 1, 150000),
(6, 6, 2, 600000),
(7, 7, 1, 200000),
(8, 8, 3, 350000),
(9, 9, 1, 400000),
(10, 10, 2, 250000);
