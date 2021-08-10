# Simple-REST-API


- Vartotojų rolės ir užduočių sąrašo statusai padaryti per enum'ą (siekiant sutaupyti sql'o vietą ir sumažinti užklausų kiekį)
- Projekte yra naudojamas Newtonsoft, siekiant sukurti JsonIgnore conditional
- Sukurtos 3 migracijos ir 14 commit'ų
- Projekte nėra tikrinama, ar insertinami/updatinami FK ir enumai egzistuoja
- Nėra slaptažodžio keitimo funkcijos


Kita informacija:

- GET {id} ir PUT {id} yra filtruojamas pagal vartotojo rolę. Jei useris bando išgauti/updatinti įrašą, kuriame UserId nėra token UserId, tada išmeta klaidą.

ToDoList GET užklausa su skirtingom rolėm:

![alt text](https://github.com/eSyntax/Simple-REST-API/blob/06d0c7ee6f60edaf36c7c745a9fbdc31638380df/images/user1.PNG?raw=true)

ToDoList PUT/POST užklausa:

![alt text](https://github.com/eSyntax/Simple-REST-API/blob/06d0c7ee6f60edaf36c7c745a9fbdc31638380df/images/user2.PNG?raw=true)

Dependencies:
- EnumExtensions.System.Text.Json
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Relational
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- MySql.Data
- MySql.EntityFrameworkCore
- MySqlConnector
- Newtonsoft.Json
- Swashbuckle.AspNetCore
- Swashbuckle.AspNetCore.Newtonsoft
- Swashbuckle.AspNetCore.SwaggerGen
- Swashbuckle.Core
- Unchase.Swashbuckle.AspNetCore.Extensions
