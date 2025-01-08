Create a C# console application that prompts some details from the user and save them to a file.

Design your solution while keeping in mind that in the future we might need to switch from this platform to a web service architecture or support other platforms, sharing code between them.

Details:

\- Person Details

\- FirstName

\- Surname

\- Date of birth

\- Marital status

\- If married, save husband / wife details (same as above) to a separate linked file (see sample below)

Business Rules:

\- If age less than 18 ask for parent's authorization(just a boolean indicating yes/no "My parents allow registration")

\- If not authorized, deny registration

\- If age less than 16 deny registration

\- Save all records into a file called "People.txt"

\- Spouse information can be saved wherever you want, but keep in mind that different people might have the same wife/husband names

Your solution will be evaluated based on the follow parameters:

•Maintability

•Reusability

•Readability

•Configurability

•Testability

•Performance

•Versatility

•Tests quality & coverage

\===============================================

Example People File at c:\\people\\mainfile.txt:

John|Doe|01-12-1980|Single|null|c:\\people\\spouses\\Anna.txt

Paul|Murphy|01-12-2001|Single|yes|
