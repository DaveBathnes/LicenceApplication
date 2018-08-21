# Licence Application 

This is a Dotnet console application for taxi licence applications. This currently allows for checking of licence eligibility and returning costs or messages to show reasons why the applicant is not eligible.

### Build Instructions

The solution has been developed using Dot Net Core v2.1, with Visual Studio Community 2017.

- Build Option 1. Build using Visual Studio. Changing the build target to Release and selecting to publish within the Visual Studio IDE will create the release folder.
- Build Option 2. Build using the command line. With dotnet core installed, the following command can be used to create a release folder. You should run the command within the LicenceApplication sub-folder to this project.

```
dotnet publish -c Release
```

You should be able to find the published application in *\bin\Release\netcoreapp2.1\publish*

The application can also be published to a self-contained executable to be easily deployed to multiple runtime environments. For example:

```
dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r ubuntu.16.10-x64
```

### Run instructions

The application will run on any environment with dotnet core installed (Windows/Linux).

If within the published application folder (*\bin\Release\netcoreapp2.1\publish*), the application can be run using:

```
dotnet LicenceApplication.dll
```

### Configuration

The application uses configuration settings to customise the messages displayed, and other settings (such as prices and discounts).

See the appsettings.json file to modify these.
