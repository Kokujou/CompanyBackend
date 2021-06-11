# Readme

## Introduction

This document contains several notes about the demo project itself, but also about possible deployment concerns in the future. 

## The project

The project was developed using .NET 5.0 using the typical REST controller scheme. All requests using a request body require the JSON format. All returned data will also be sent using the JSON format.

Currently there are nu enumerations in place, but also those should and will be sent as strings, to further increase the readability in the client code.

### Startup

This project uses a CosmosDB, currently set to use the CosmosDB Emulator connection. Therefore the [appsettings.json](appsettings.json) file should be used. It stores all connection variables, which can be loaded once during the startup of the service. The existance of the database and the container, dedicated to this service is ensured once on service start.

Further, this service uses the native dependency injection. All future services, data repositories or other providers will be registered and can afterwards be injected in the constructor.

### API

The current controller is registered under the route /company. All requests use RESTful request methods and are supposed to be read in combination with this method to express the purpose of the actual action.

For a full documentation of all defined endpoints please visit the [Swagger UI](https://localhost:5001/swagger), which was added for the automatic documentation purpose of this demo project.

## Deployment Notes

The purpose of this section is to give an overview what kind of services should be deployed and how the deployment of each service would work.

When deployng an Azure App Service it is possible to specify a source file for reading the final app configuration. This is why for this service the previously linked appsettings.json was created.

This kind of deployment also offers the possibility to override the template parameters. So to bring the proper configuration values into the app service, one can take the appsettings.json as a base for all constant values and override them, using previously calculated pipeline variables, which can be calculated e.g. using PowerShell, Azure PowerShell and other steps.

For the sake of a higher security it is recommended to note hard code configuration values, especially when those values contain security tokens, inside the source code. rather those values should be kept inside the environment they are meant for. E.g. for the CosmosDB one could create a powershell script executed on release time, which retrieves the configuration values of the cosmos DB and injects them into the app service. this can be done e.g. in an Azure Devops Release Pipeline. 

This is the same procedure that can be used for any other service connection, which is not constant and may change dpeending on an environemtn. If the service e.g. needs to communicate with other REST services. A popular example for this kind of situation would be an Identity Server.

## Possible Extensions

This section is dealing with a short excurs about how this service could be extended to add additional functionality

### NSwag automated clients

A very frequent use of REST APIs is the combination with some kind of frontend. In such a frontend service, a client would need to be written, using all the routing-information and data structures of the REST service, producing lots of duplicate code. This can be prevented using Swagger. 

Swagger is already used in this project as a tool for documenting the REST API. Based on this documentation it is possible to automatically create a client using NSWag. This client has all the Data structures published in the backend service. It's possible to generate C#, TypeScript and lots of other clients. The only thing the frontend then needs to do is to inject the base URL for the app service. 

The automatic client generation can be executed when building the solution and during the build process a Nuget or NPM package containing the client library could be published, which then could be simply updated in the frontend. Using this procedure not only prevents code duplication, but also heavily increases the maintainability, makes it much simpler to use and prevents frequent coding errors like spelling mistakes in non-typed strings.

### Serilog Logging

Serilog is a very powerful looging library which accepts a lot off different logging types, for example:
- log to console
- log to file
- log to application insights
- ...

To use this service, one can simply install the package and use it in the already existing [Startup.cs](Startup.cs). Using this library, one can have many different ways of configuring the logging, using a lot of different logging adapters. For getting proper information about the actions triggered on an app service, this tool should not be missed.

#### Application Insights

One of the adapters for Serilog logging is the Microsoft Application Insights service. Using this service, not only basic logging but all the app service activity will be received by an additional service inside azure. It also offers the way to calculate metrics, shows downtimes, failed requests and lots of other useful informtion and graphical representations of those information. As such it is very useful in productive environments with a lot of users, where log files would be way too big to handle for developers.

To use it in the project first the app service needs to be deployed. a token will be created, which can like described above injected in the appsettings.json. After doing this, the last thing to do is configure Serilog for sending all the telemetrics data to this service, using very few lines of code in the Startup.cs.

