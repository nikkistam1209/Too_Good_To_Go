<h1>Too Good To Go</h1>

<h2>Overview</h2>
<p>This project involves the development of a web application aimed at reducing food waste by allowing students to reserve meal packages at reduced prices. The application is built using the ASP.NET MVC Core framework and is supported by a RESTful Web API for communication with mobile applications.</p>

<h2>Functionality</h2>
<ul>
  <li><strong>Students:</strong> Students can view available meal packages, place reservations, and manage their reserved packages.</li>
  <li><strong>Canteen Staff:</strong> Canteen staff can view available packages and add, edit, or delete new packages.</li>
</ul>

<h2>Technologies</h2>
<ul>
  <li><strong>ASP.NET MVC Core 6:</strong> For developing the web application.</li>
  <li><strong>Entity Framework Core:</strong> For persistence in the database.</li>
  <li><strong>GraphQL and RESTful Web API:</strong> For communication with mobile applications.</li>
  <li><strong>Microsoft Identity:</strong> For authentication and authorization.</li>
  <li><strong>Swagger:</strong> For API documentation.</li>
</ul>

<h2>Architecture</h2>
<p>The project follows the clean (onion) architecture, with the application core at its center and a clear separation of layers. Dependency injection is applied to manage dependencies.</p>

<h2>Testing</h2>
<p>Unit tests are implemented for the business rules of the domain, using mocking to ensure test independence. The RESTful Web API is tested using Postman.</p>

<h2>Deployment</h2>
<p>Continuous deployment is implemented through a development pipeline that automatically starts builds, executes unit tests, and deploys updates.</p>
