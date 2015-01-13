a-webshop (A Name Not Yet Taken AB)
=========

This is a e-commerce solution that is based on ASP.NET, MVC, C# and MS SQL. The frontend design can be changed in the administration interface of a-webshop, this makes it possible to have unique webshops that are based on the same codebase. This webshop requires connections over https because a webshop handles a lot of sensitive user information.

The documentation for this e-commerce solution is under development and you should be able to read more about this solution at <a href="http://www.a-webshop.com">a-webshop.com (English)</a> or <a href="http://www.a-webshop.se">a-webshop.se (Swedish)</a>

You can see a demo of the webshop at <a href="http://a-webshop-demo.azurewebsites.net/">a-webshop (demo)</a>

This project has three branches, the master branch is used for development. The staging branch is used for testing on a live website and the production branch is used for live production sites.

<b>A quick start guide</b><br />
Set up a website and a MS SQL database on your server. The connection string to the database should be a "appSetting" and the key should be called "ConnectionString". If you use Windows Azure like we do, you can add the appSetting under the CONFIGURE tab in the settings for the website. Another option is to add a app.config file to the solution with contents like this:

<pre>&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
&lt;appSettings&gt;
&lt;add key=&quot;ConnectionString&quot; value=&quot;XXXXXXXXXXXXXXXXXXX&quot; /&gt;
&lt;/appSettings&gt;</pre>

When you have deployed the website, go to ~/admin_default and sign in with Master as the username and an empty password field. The next thing to do is to go to Domains and change the domain name and the webaddress for the first post in the list. The domain name should be stated without http://wwww or https://www, it should just be the domain name. The webaddress is the full webaddress to the website.
