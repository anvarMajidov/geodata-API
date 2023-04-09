# geodata-API

This is a simple service that provides geodata based on addresses and returns the 10 closest addresses based on a given geolocation. The service also logs all requests and responses, which can be saved to a physical file.

## Endpoints

### Swagger Documentation
Just to make requests go to following link.

[Swagger link](https://2zgrxofpq2v6jnzhn6pyuk5q3m0naxgz.lambda-url.us-east-1.on.aws/swagger/index.html)

## Getting Started
If you want to install project and run on local machine do next steps.

### Prerequisites
To run this service, you need to have the following software installed on your machine:

* dotnet6

## Installation
Clone the repository to your local machine.

To start the service, run the following command:

```bash
dotnet run
```

## Logging
All requests and responses are logged in a physical file located at /Logs/. The log format is as follows:

Copy code
```css
[timestamp] [HTTP method] [endpoint] [status code] [response time in ms]
```
