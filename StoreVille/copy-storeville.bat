@echo off
set conf=%1
cd "bin\%conf%\net10.0"
xcopy /Y StoreVille.dll ..\..\..\..\PeopleVille\bin\Debug\net10.0
xcopy /Y StoreVille.pdb ..\..\..\..\PeopleVille\bin\Debug\net10.0
xcopy /Y StoreVille.deps.json ..\..\..\..\PeopleVille\bin\Debug\net10.0
