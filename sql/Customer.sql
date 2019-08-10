USE Semesterprojekt;

Drop TABLE IF EXISTS Customer;

CREATE TABLE IF NOT EXISTS Customer (
    id INT NOT NULL AUTO_INCREMENT,
    Customer_id VARCHAR(50),
    Name VARCHAR(50),
    prename VARCHAR(50),
    email VARCHAR(50),
    phone VARCHAR(50),
    website VARCHAR(50),
    password VARCHAR(100),
    PRIMARY KEY(id)
    )