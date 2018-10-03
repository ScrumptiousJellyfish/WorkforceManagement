# WorkforceManagement

## Installation

1. ```git clone git@github.com:ScrumptiousJellyfish/WorkforceManagement.git```

### Database Creation

1. Open SSMS. Create a new database.

2. Open the sql file by using (Ctrl + O) then navigating to the project folder and select BangazonDatabaseFinal.sql.

3. Execute the query. No errors should occur.

### Connecting The Database To The Code

1. make an appsettings.json file inside ```\WorkforceManagement\BangazonScrumptiousJellyfish\BangazonScrumptiousJellyfish```

2. copy and paste the contents of templateappsettings.json to appsettings.json and replace ```INSERT_DATABASE_CONNECTION_HERE``` with your database connection along with replacing ```INSERT_DATABASE_NAME_HERE``` with your database name

3. **your database connection is found in the** ```Server name``` **field when opening SSMS**

4. **your database name is what you specified when you created a new database**

### Viewing The Website

1. ```cd``` into ```\WorkforceManagement\BangazonScrumptiousJellyfish\``` or similar depending on OS

2. ```dotnet run``` in directory

3. navigate in the web browser to the site listed in the terminal where you ran ```dotnet run```
