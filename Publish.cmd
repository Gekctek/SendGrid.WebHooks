SET configuration=Release
SET out=C:/Publish

call dotnet pack ./src/edjCase.SendGrid.WebHooks -c %configuration% -o "%out%/EdjCase.SendGrid.WebHooks"
