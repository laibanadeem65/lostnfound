-- ============================================
-- Lost and Found Database Schema
-- ============================================
create database LostAndFoundDB
-- Table 1: Users (stores both regular users and admins)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    IsAdmin BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);

-- Table 2: Items (stores all lost and found items)
CREATE TABLE Items (
    ItemId INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Category NVARCHAR(50),
    ItemType NVARCHAR(10) NOT NULL CHECK (ItemType IN ('Lost', 'Found')),
    Location NVARCHAR(200),
    DateReported DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active' CHECK (Status IN ('Active', 'Resolved', 'Closed')),
    OwnerId INT NOT NULL,
    ImagePath NVARCHAR(500),
    FOREIGN KEY (OwnerId) REFERENCES Users(UserId)
);

-- Insert a default Admin account (Username: admin, Password: admin123)
INSERT INTO Users (Username, Password, Email, IsAdmin) 
VALUES ('admin', 'admin123', 'admin@lostandfound.com', 1);

-- Insert a test regular user (Username: testuser, Password: test123)
INSERT INTO Users (Username, Password, Email, IsAdmin) 
VALUES ('testuser', 'test123', 'test@example.com', 0);

select * from Users