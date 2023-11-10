
# SwissKnife Request Auditing Library

## About
This library serves as Integration project to be used by services for Http Request Auditing.
The library exposes a middleware which audits http requests.

## Middleware

To configure Http Request Auditing, following needs to be added in Startup.cs
```
//add in Configure method
app.UseHttpRequestAuditMiddleware();

//add a middleware extension
public static class HttpRequestMiddleware
    {
        public static IApplicationBuilder UseHttpRequestAuditMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpRequestAuditMiddleware>();
        }
    }
```

## Implementation

 - The middleware adds a response header **Request-Id** to the http request which contains the traceId for tracing.
 - Following details are extracted from the http request and added to audit model:
	 - Client IP Address
	 - Client Source
	 - Method Type
	 - Request Url
	 - Request Status
	 - Authorization details
- Following custom details can be ingested to request audit from the implementing services:
	- Requested Resource
	- Description
	- Affected Entity

## References
## Contributing
