# certigon-test
Coding challenge implementation for Certigon.

## **The back-end solution is implemented as a ASP.NET Core Web API, using .NET 6.0 Framework.**

- API endpoints for all operations regarding employees can be found in **EmployeeController.cs** API controller.\
The only deviation from the coding challenge request is the URL template of **GetById(int id)** method. The requested URL caused ambiguity problems because it has the same format as the **Get(bool isActive)** method.
- Data mocking is solved by using **EmployeeContainer.cs** as a singletone dependency.
- Request and response logging is solved in a Middleware using NLog library https://nlog-project.org. Implementation can be found in **RequestLoggingBuilderExtensions.cs**. NLog configuration is in the **nlog.config** file and the logs can be found in **~/bin/Debug/net6.0/Logs** directory.
- Since this is a ASP.NET Core application, it can be self-hosted and does not require IIS.
- XML to JSON conversion can be found in **TranslateController.cs**. It is implemented using Newtonsoft.Json library https://newtonsoft.com.