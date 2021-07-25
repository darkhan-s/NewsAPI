# NewsAPI

This repository is meant for the Kaspi.kz interview task. 
This project is based on Visual Studio's Web API template. 

The app does the following things: 
- It parses the news from website of own choice: https://kapital.kz/tehnology (hardcoded).
- It stores the title, date and the contents to the SQL Express database. The connection string can be setup under web.config file. 
- By default, 30 articles are stored in dbo.News under TestDB database (created if does not exist).
- Additionally, several RestAPI functions are implemented. 

Three controllers are developed to support the following functionality:
``` bash
PostsController.cs - /api/posts?from=2020-07-01T00:00:00Z&to=2022-07-07T00:00:00Z - returns posts under a defined timeframe
TopTenController.cs - /api/topten - returns top 10 most used words
SearchController.cs - /api/search?text=Казахстан - returns posts that contain a string
```
Parser.cs file contains the Parser class, which is hardcoded to parse the selected website using HtmlAgilityPack and LINQ. The Parser class writes rows of Title, Date, Content to SqlRows class. 

Connector.cs contains the Connector class, which connects to SQL DB and publishes the updates from the website. The table is updated whenever GET is requested. 

Finally, some simple unit tests are added for controllers.

The application does not yet support authentication, but the initial plan is to implement JWT authentication. 
