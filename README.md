# Licence Application 

A console application for taxi licence applications. This currently allows for checking of licence eligibility and returning costs or messages to show reasons why the applicant is not eligible.

### Build Instructions

The solution is developed using Dot Net Core v2 using Visual Studio Community. 

- Option 1. Build using Visual Studio. Changing the build to release and selecting to publish within 
- Option 2. Build using the command line. With dotnet installed the following command can be used to create a release folder when in the LicenceApplication sub folder to this project.

```
dotnet publish -c Release
```

You should be able to find the published application in \bin\Release\netcoreapp2.1\publish

The application can also be published to a self contained executable for multiple runtime environments.

```
dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r ubuntu.16.10-x64
```


### Run instructions

The application will run on any environment with dotnet core installed (Windows/Linux).

The application can be run using:

```
dotnet LicenceApplication.dll
```

### Configuration

The application uses configuration settings to customise the messages displayed, and the settings (such as prices and discounts).

See the appsettings.json file to modify these.
