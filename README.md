# OpenEvent

An open event ticketing and hosting platform built in ASP.NET, Angular and MySQL utilising the Stripe API.

Check out the [wiki](../../wiki) for a user guide and documentation

## Installation

### .NET
First you should install the dotnet SDK https://dotnet.microsoft.com/download/dotnet/5.0

### Angular 
To install Angular you need nodejs installed https://nodejs.org/en/download/
once installed you can get the Angular cli https://cli.angular.io/

### MySQL
OpenEvent uses a local MySQL datbase in development so you'll need a local instance running and insert your connection string into OpenEvent.Web/appsettings.json

```
"LocalConnectionString": "server=localhost;database=openEvent;user=openevent;password=Password",
```

## Running

To run the app make sure you have all prerequisites first.

Then simply cd into the OpenEvent.Web directory, restore all the packages (install any extra stuff, should only need to do once) and then run the program.

```sh
$ cd OpenEvent.Web
$ dotnet restore
$ dotnet run
```

Visit http://localhost:5000/

## Maintainers

[@MrHarrisonBarker](https://github.com/MrHarrisonBarker)

## License

[MIT](LICENSE) © Harrison Barker