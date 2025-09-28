# Snap-N-Shop

Snap-N-Shop is a B2C e-commerce platform that allows users to browse, search, and shop for a variety of products across multiple categories. This project was developed as part of my .NET training and demonstrates a full-stack web application using Angular for the client side and ASP.NET Core Minimal APIs for the server side.  

---

## Features

- Browse and search products across different categories  
- User registration and login  
- Add products to cart  
- Place orders and manage purchases  
- Classic e-commerce functionalities  

---

## Tech Stack

- **Frontend:** Angular  
- **Backend:** ASP.NET Core Minimal APIs  
- **Database:** MS SQL Server  
- **Other Tools:** Node.js (for Angular), .NET SDK  

---

## Folder Structure

Snap-N-Shop/<br>
 ├── Snap-N-Shop-Client/  
 └── Snap-N-Shop-API/

---

## Setup Instructions

### Prerequisites

- Node.js and npm (for Angular)  
- Angular CLI  
- .NET 6+ SDK  
- SQL Server (local or remote)  

### Server Setup

1. Navigate to the API folder:  
   ```bash
   cd Snap-N-Shop/Snap-N-Shop-API
   
2. Restore dependencies:
    ```bash
    dotnet restore

3.	Update your database connection string in appsettings.json

4.	Run the API:
    ```bash
    dotnet run

5.	The server should start and be accessible at https://localhost:5001 (or your configured port)

### Client Setup

1.	Navigate to the Angular client folder:
    ```bash
    cd Snap-N-Shop/Snap-N-Shop-Client

2.	Install dependencies:
    ```bash
    npm install

3.	Start the development server:
    ```bash
    ng serve

4.	Open your browser and go to http://localhost:4200

---

## Usage
	•	Create an account or login
	•	Browse products by category or search
	•	Add items to your cart
	•	Place orders and view your purchase history

---

## Read More

I will be writing a detailed blog about this project, including behind-the-scenes development stories, challenges, and learnings. You can read it here:

Snap-N-Shop Blog on Hey Sainty [https://hey-sainty.vercel.app/blog/snap-n-shop]

---

## License

This project is for training purposes and does not have a public license.

---

## Contact

Feel free to reach out for any queries or suggestions at:
	•	Email: ppriyanshuchaurasia@gmail.com
	•	Website: hey-sainty.vercel.app[https://hey-sainty.vercel.app]

---

If you want, I can **also add a “Tips for Developers” section** at the end for environment setup and API-client linking, which makes it slightly more helpful for anyone cloning the repo.  

Do you want me to do that?