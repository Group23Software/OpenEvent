# OpenEvent

## Prerequisites

### Dotnet
First you should install the dotnet SDK https://dotnet.microsoft.com/download/dotnet/5.0
### Angular 
To install Angular you need nodejs installed https://nodejs.org/en/download/
once installed you can get the Angular cli https://cli.angular.io/

## Running

To run the app make sure you have all prerequisites first.

Then simply move into the OpenEvent.Web directory, restore all the packages (install any extra stuff, should only need too once) and then run the program.

```sh
$ cd OpenEvent.Web
$ dotnet restore
$ dotnet run
```

This will start the API server. To start the Angular server in another terminal run:

```sh
$ cd OpenEvent.Web/ClientApp/
$ ng serve
```

Visit http://localhost:4200/

## Developing

We will be seperating each "sprint" of work into git branches so you have to make sure your on the correct branch first.
To change branch all you need to do is checkout.

```sh
$ git checkout SPRINT-1
```

You can check which branch you are on by listing the branches (the one with the star is your current)

```sh
$ git branch
```

Once you've finished adding new code you have to add all the files, commit and push the changes

```sh
$ git add .
$ git status // to view all the changes added
$ git commit -m "message describing what has been done"
$ git push
```

## Links

https://github.com/Group23Software/OpenEvent
https://harrisonbarker.atlassian.net/secure/RapidBoard.jspa?projectKey=EVENT&useStoredSettings=true&rapidView=7&atlOrigin=eyJpIjoiMjBmMWJmMTE3ZTExNDc0OThhY2EyNTk0NjFkN2FiYjEiLCJwIjoiaiJ9
