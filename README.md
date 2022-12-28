# Pomodoro-BE

## Setup husky
route to repo shouldn't contain any non-latin letters!
execute in PowerShell or cmd: "dotnet husky install"

## Generate local DB
- open PM console
- ensure that `Default project` is set to `Pomodoro.DataAccess`
- run `update-database`
- find generated DB called `Pomodoro` in the `SQL Server Object Explorer` among other local DBs