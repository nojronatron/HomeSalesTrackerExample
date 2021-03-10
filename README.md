# HomeSalesTrackerExample
The purpose of this repo is to demonstrate my C# experience gained while taking C# programming courses at a local college.

# NOT PRODUCTION CODE
Given the purpose of the repo, the goals of the code were not to produce production-quality code, rather to explore, experiment, and demonstrate coding capabilities gleaned from classes and published materials.

# STATUS
At the time of this repo's creation, the MASTER branch has useable code. There may be some workflow paths that fail or otherwise "dead end", but the app should not crash with any unhandled exceptions.

# FUTURE
As time permits I will update the code to close the loop on incomplete workflow paths.

# BUILD AND USE REQUIREMENTS
.NET Framework 4.x
Entity Framework 6.x
Local SQL Server or other database supported by Entity Framework 6.0
A SQL database with some data in it (this may get added to this repo at some point)

# HOW TO USE
Probably best to Debug-Build and Debug-Run at this point.
The main menu has options for Searching, Adding, Updating, Removing, and running Reports.

Search Menu: Search for Homes; Homes for sale; RE Agents; home buyers; or home owners.
Add Menu: Create a new home; Put an existing home on the market (add); new Agent person; new Buyer person; or new Owner person.
Update Menu: Update existing Homes; update a home as Sold; update information on an agent, buyer, or owner.
Remove Menu: Removes an existing home off the market without recording a sale. Retains the home in the DB.
Reports Menu: View reports based on existing DB data.

# ARCHITECTURE
The front-end UI is WPF and is fairly "heavy" with some code-behind and less-than-efficient use of LINQ, decision trees, and MVVM. Front-end includes:
* Logging
* Exception Handling
* MVVM and MVVM-ish code style that enables viewing data in different contexts
* Separation from database/back-end
* Maintains Collections for data use and access to data back-end
* Wrapper to handle all start-up, operate, and shut-down operations

Back-end:
* Leverages Entity Framework 6.0
* Segregates actual data handling from front-end app
* Employs Exception handling
* Attempts to avoid ambiguity in CRUD-type operations while providing boolean responses to all CRUD actions
* Uses an XML-based initial data library and can update those XML files with data at App shut-down for portability

# FINAL WORDS
This project was built as a learning tool, for me.
In the future this will be a presentation tool to show my work.
While you are free to explore the code I advise against copying it for use in production.
Also, avoid using this as your solution to a class project: Your instructor will know you copied it, and you will not gain the benefit of learning how to do it yourself.
