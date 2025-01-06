<div align="center">

  ### 📖 $$\text{\color{#2F3C7E}GUIDE:}$$ $$\text{\color{#2F3C7E}Database Setup for Insurance Partners Project}$$

</div>

### 📝 $${\color{#F96167}Overview}$$

This guide provides two methods for setting up the Insurance Partners database:
1. Importing the `.bacpac` file (recommended for quick setup)
2. Running SQL scripts sequentially (recommended for understanding the database structure)

The process takes approximately 5-10 minutes.

## 💻 $${\color{#F96167}Method \space 1: \space Importing \space the}$$ .bacpac $${\color{#F96167} \space file}$$

1️⃣ <ins>**Verify SQL Server Instance**</ins>
* Open **SQL Server Configuration Manager**.
* Check the status of your SQL Server instance:
  * If it is **Stopped**, right-click on the instance and select **Start** to initiate it.

2️⃣ <ins>**Launch SQL Server Management Studio (SSMS)**</ins>
* Open SSMS and connect to your SQL Server instance.
* In the Object Explorer on the left-hand side, locate the **Databases** folder.
* Right-click on **Databases** and select **Import Data-tier Application...**

3️⃣ <ins>**Select the .bacpac File**</ins>
* In the wizard that appears, navigate to the `Database/InsurancePartner.bacpac` file.
* Select the file and click **Next**
* Follow the wizard's steps, keeping default settings.

4️⃣ <ins>**Verify the Import**</ins>
* Once completed, refresh the **Databases** folder.
* Verify that the InsurancePartners database appears with all required tables.

## 🖥️ $${\color{#F96167}Method \space 2: \space Running \space SQL \space Scripts}$$
If you prefer to build the database step by step, you can execute the provided SQL scripts in the following order:

1️⃣ <ins>**Navigate to Scripts Directory**</ins>
* Locate the `Database/Scripts` folder in the project.
* Scripts should be executed in numerical order.

2️⃣ <ins>**Execute Scripts Sequentially**</ins>
1. `01_CreateDatabase.sql` - Creates the database.
2. `02_CreateTables.sql` - Establishes the basic table structure.
3. `03_AddConstraints.sql` - Adds foreign key and unique constraints.
4. `04_CreateIndexes.sql` - Creates necessary indexes.
5. `05_InsertLookupData.sql` - Populates lookup tables.
6. `06_SeedData.sql` - Adds initial sample data *(this is optional script)*.

3️⃣ <ins>**Execution Process**</ins>
For each script:
* Open the script in SSMS.
* Click **Execute** or press **F5**.
* Verify successful execution before proceeding to the next script.

4️⃣ <ins>**Verify Setup**</ins>
After executing all scripts:
* Refresh the database in Object Explorer.
* Verify all tables are present.
* Confirm lookup data is populated.







