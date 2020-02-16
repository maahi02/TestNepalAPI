# TestNepalAPI


Steps for running project API first

1. Open API project in visual studio 2017 and install all the packages.

2. Make sure all the packages installed and build successfully.

3. Change the connection string of your db.

4. Run the project and it will create database that you provided db connection string in web config file.

5. First make sure that db is created and one user is inserted.( In AspNetUsers table)  

    So you can login with the same user credential.  

    Login ID: admin@testnepal.com
    Password: abc123#
    
    (If build didn't succeed, restart VS and run the project and Restore Nuget Packages and try.)
    (project based on Code First Approch Entityframe work).
