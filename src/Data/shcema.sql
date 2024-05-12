--este archivo es solo para tener la estructura de la base de datos

-- Creación de la tabla de Usuarios
CREATE TABLE Users (
    UserId INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash CHAR(64) NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Creación de la tabla de Categorías
CREATE TABLE Categories (
    CategoryId INT AUTO_INCREMENT PRIMARY KEY,
    CategoryName VARCHAR(255) NOT NULL,
    Description TEXT
);

-- Creación de la tabla de Productos
CREATE TABLE Products (
    ProductId INT AUTO_INCREMENT PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    CategoryId INT,
    Stock INT DEFAULT 0,
    ImageUrl VARCHAR(500),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- Creación de la tabla de Pedidos
CREATE TABLE Orders (
    OrderId INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT,
    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(50),
    TotalAmount DECIMAL(10, 2),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Creación de la tabla de Detalles de Pedidos
CREATE TABLE OrderDetails (
    OrderDetailId INT AUTO_INCREMENT PRIMARY KEY,
    OrderId INT,
    ProductId INT,
    Quantity INT,
    Price DECIMAL(10, 2),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE Carts (
    CartId INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE CartItems (
    CartItemId INT AUTO_INCREMENT PRIMARY KEY,
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT DEFAULT 1,
    FOREIGN KEY (CartId) REFERENCES Carts(CartId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);


-- Insertar Categorías
INSERT INTO Categories (CategoryName, Description) VALUES ('Electronics', 'Electronic Devices'), ('Clothing', 'Apparel and Accessories');

-- Insertar Productos
INSERT INTO Products (ProductName, Description, Price, CategoryId, Stock, ImageUrl) VALUES
('iPhone 13', 'Latest model of iPhone 13', 799.99, 1, 50, 'url_to_image'),
('Levi Jeans', 'Comfortable and stylish', 40.50, 2, 100, 'url_to_image');

-- Insertar Usuarios
INSERT INTO Users (Username, Email, PasswordHash) VALUES ('john_doe', 'john@example.com', 'hashed_password_here');

-- Insertar Pedidos
INSERT INTO Orders (UserId, Status, TotalAmount) VALUES (1, 'Pending', 840.49);

-- Insertar Detalles de Pedido
INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price) VALUES (1, 1, 1, 799.99);



-- consulta de ejemplo

-- Obtener todos los pedidos de un usuario
SELECT o.OrderId, o.OrderDate, o.Status, o.TotalAmount, od.ProductId, od.Quantity, od.Price
FROM Orders o
JOIN OrderDetails od ON o.OrderId = od.OrderId
WHERE o.UserId = 1;

