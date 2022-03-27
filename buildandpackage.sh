rm -rf Site
mkdir Site
dotnet publish -o Site
cp -r .platform/ Site
#cd Site
#zip -r Site.zip ./