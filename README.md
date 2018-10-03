# WorkforceManagement

## Installation

1. ```git clone git@github.com:ScrumptiousJellyfish/WorkforceManagement.git```

2. ```cd``` into ```\WorkforceManagement\BangazonScrumptiousJellyfish\``` or similar depending on OS

3. ```dotnet run``` in directory

4. navigate in the web browser to the site listed in the terminal where you ran ```dotnet run```

## Database Creation

1. Open SSMS. Create a new database.

2. Open the sql file by using (Ctrl + O) then navigating to the project folder and select BangazonDatabaseFinal.sql.

3. Execute the query. No errors should occur.

## Connecting The Database To The Code

1. make an appsettings.json file inside ```\WorkforceManagement\BangazonScrumptiousJellyfish\BangazonScrumptiousJellyfish```

2. copy and paste the contents of templateappsettings.json to appsettings.json and follow the directions in the comments

3. your dbconnection is found when opening ssms

4. your dbname is what you specified when you created a new database
